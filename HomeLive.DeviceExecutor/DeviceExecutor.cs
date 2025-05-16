using SysBot.Base;
using System.Globalization;
using System.Text;

namespace HomeLive.DeviceExecutor;

public enum RoutineType
{
    None,
    ReadWrite,
}

public class DeviceState : BotState<RoutineType, SwitchConnectionConfig>
{
    public override void IterateNextRoutine() => CurrentRoutineType = NextRoutineType;
    public override void Initialize() => Resume();
    public override void Pause() => NextRoutineType = RoutineType.None;
    public override void Resume() => NextRoutineType = InitialRoutine;
}

public class DeviceExecutor<T>(DeviceState cfg) : SwitchRoutineExecutor<T>(cfg) where T : DeviceState
{
    private const decimal BotbaseVersion = 2.4m;
    private static ulong BoxStartAddress = 0;

    public override string GetSummary()
    {
        var current = Config.CurrentRoutineType;
        var initial = Config.InitialRoutine;
        if (current == initial)
            return $"{Connection.Label} - {initial}";
        return $"{Connection.Label} - {initial} ({current})";
    }

    public override void SoftStop() => Config.Pause();
    public override Task HardStop() => Task.CompletedTask;

    public override async Task MainLoop(CancellationToken token)
    {
        try
        {
            var botbase = await VerifyBotbaseVersion(token).ConfigureAwait(false);
            Log($"Valid botbase version: {botbase}");
            (var title, var build) = await IsRunningHome(token).ConfigureAwait(false);
            Log($"Valid Title ID ({title})");
            Log($"Valid Build ID ({build})");
            Log("Connection Test OK.");

            if (Config.Connection.Protocol is SwitchProtocol.WiFi)
                SwitchConnection.MaximumTransferSize = HomeDataOffsets.HomeSlotSize * HomeDataOffsets.HomeSlotCount;

            Config.IterateNextRoutine();
        }
        catch (Exception ex)
        {
            Disconnect();
            throw new InvalidOperationException($"{ex.Message}");
        }
    }

    public async Task Connect(CancellationToken token)
    {
        Connection.Connect();
        Log("Initializing connection with console...");
        await InitialStartup(token).ConfigureAwait(false);
    }

    public void Disconnect()
    {
        HardStop();
        BoxStartAddress = 0;
        if(Connection.Connected)
            Connection.Disconnect();
    }

    private async Task<(string, string)> IsRunningHome(CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
        var build = await GetBuildID(token).ConfigureAwait(false);
        if (title.Equals(HomeDataOffsets.HomeTitleID) && build.Equals(HomeDataOffsets.HomeBuildID))
            return (title, build);
        else if (!title.Equals(HomeDataOffsets.HomeTitleID)) 
            throw new InvalidOperationException($"Invalid Title ID: {title}. The Pokémon HOME application is not running.");
        else 
            throw new InvalidOperationException($"Invalid Build ID: {build}. The Pokémon HOME application is on a non-supported version.");
    }

    private async Task<string> GetBuildID(CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var cmd = SwitchCommand.GetBuildID(Config.Connection.Protocol is SwitchProtocol.WiFi);
        var bytes = await SwitchConnection.ReadRaw(cmd, 17, token).ConfigureAwait(false);
        var str = Encoding.ASCII.GetString(bytes).Trim().ToUpper();
        return str;
    }

    //Thanks Anubis
    //https://github.com/kwsch/SysBot.NET/blob/b26c8c957364efe316573bec4b82e8c5c5a1a60f/SysBot.Pokemon/Actions/PokeRoutineExecutor.cs#L88
    //AGPL v3 License
    public async Task<string> VerifyBotbaseVersion(CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var data = await SwitchConnection.GetBotbaseVersion(token).ConfigureAwait(false);
        var version = decimal.TryParse(data, CultureInfo.InvariantCulture, out var v) ? v : 0;
        if (version < BotbaseVersion)
        {
            var protocol = Config.Connection.Protocol;
            var msg = protocol is SwitchProtocol.WiFi ? "sys-botbase" : "usb-botbase";
            msg += $" version is not supported. Expected version {BotbaseVersion} or greater, and current version is {version}. Please download the latest version from: ";
            if (protocol is SwitchProtocol.WiFi)
                msg += "https://github.com/olliz0r/sys-botbase/releases/latest";
            else
                msg += "https://github.com/Koi-3088/usb-botbase/releases/latest";
            throw new Exception(msg);
        }
        return data;
    }

    public async Task<ulong> GetBoxStartOffset(CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        if (BoxStartAddress == 0)
            BoxStartAddress = await SwitchConnection.PointerAll(HomeDataOffsets.BoxStartPointer, token).ConfigureAwait(false);

        return BoxStartAddress;
    }

    public async Task<byte[]> ReadBoxData(int box, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log("Getting Box offset...");
        var boxOffset = HomeDataOffsets.GetBoxOffset(await GetBoxStartOffset(token).ConfigureAwait(false), box);
        Log($"Found offset 0x{boxOffset:X8}");
        Log($"Reading Box {box}...");
        var size = HomeDataOffsets.HomeSlotSize * HomeDataOffsets.HomeSlotCount;
        var data = await SwitchConnection.ReadBytesAbsoluteAsync(boxOffset, size, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }

    public async Task<byte[]> ReadSlotData(int box, int slot, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log("Getting Slot offset...");
        var slotOffset = HomeDataOffsets.GetSlotOffset(await GetBoxStartOffset(token).ConfigureAwait(false), box, slot);
        Log($"Found offset 0x{slotOffset:X8}");
        Log($"Reading Box {box}, Slot {slot}...");
        var data = await SwitchConnection.ReadBytesAbsoluteAsync(slotOffset, HomeDataOffsets.HomeSlotSize, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }

    public async Task<byte[]> ReadRange((int box, int slot) start, (int box, int slot) end, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Reading from Box {start.box}, Slot {start.slot} to Box {end.box}, Slot {end.slot}...");
        var startOffset = HomeDataOffsets.GetSlotOffset(await GetBoxStartOffset(token).ConfigureAwait(false), start.box, start.slot);
        var endOffset = HomeDataOffsets.GetSlotOffset(await GetBoxStartOffset(token).ConfigureAwait(false), end.box, end.slot);
        var size = endOffset - startOffset;
        var data = await SwitchConnection.ReadBytesAbsoluteAsync(startOffset, (int)size, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }
}