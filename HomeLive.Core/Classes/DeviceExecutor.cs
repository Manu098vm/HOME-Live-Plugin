using PKHeX.Core;
using SysBot.Base;
using System.Text;
using System.Buffers.Binary;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeLive.Core;

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

public class DeviceExecutor<T> : SwitchRoutineExecutor<T> where T : DeviceState
{
    private const string HomeTitleID = "010015F008C54000";
    private const string HomeBuildID = "ABF8AFD82FE1B2BD";

    public DeviceExecutor(DeviceState cfg) : base(cfg) { }

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
            (var title, var build) = await IsRunningHome(token).ConfigureAwait(false);
            Log($"Valid Title ID ({title})");
            Log($"Valid Build ID ({build})");
            Log("Connection Test OK.");

            if (Config.Connection.Protocol is SwitchProtocol.WiFi)
                SwitchConnection.MaximumTransferSize = HomeDataOffsets.HomeSlotSize * HomeDataOffsets.HomeBoxCount;

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
        if(Connection.Connected)
            Connection.Disconnect();
    }

    private async Task<(string, string)> IsRunningHome(CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
        var build = await GetBuildID(token).ConfigureAwait(false);
        if (title.Equals(HomeTitleID) && build.Equals(HomeBuildID))
            return (title, build);
        else if (!title.Equals(HomeTitleID)) 
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

    public async Task<string> ReadPlayerOTName(CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log("Reading MyStatus OT Name...");
        var bytes = await SwitchConnection.ReadBytesMainAsync(HomeDataOffsets.PlayerNameOffset, 0x1A, token).ConfigureAwait(false);
        var str = StringConverter8.GetString(bytes.AsSpan());
        Log($"Done. (OT: {str})");
        return str;
    }

    public async Task<byte[]> ReadBoxData(int box, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Reading Box {box}...");
        var boxOffset = HomeDataOffsets.GetBoxOffset(box);
        var size = HomeDataOffsets.HomeSlotSize * HomeDataOffsets.HomeSlotCount;
        var data = await SwitchConnection.ReadBytesAsync(boxOffset, size, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }

    public async Task<byte[]> ReadSlotData(int box, int slot, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Reading Box {box}, Slot {slot}...");
        var slotOffset = HomeDataOffsets.GetSlotOffset(box, slot);
        var data = await SwitchConnection.ReadBytesAsync(slotOffset, HomeDataOffsets.HomeSlotSize, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }

    public async Task<byte[]> ReadRange((int box, int slot) start, (int box, int slot) end, CancellationToken token)
    {
        if (!Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Reading from Box {start.box}, Slot {start.slot} to Box {end.box}, Slot {end.slot}...");
        var startOffset = HomeDataOffsets.GetSlotOffset(start.box, start.slot);
        var endOffset = HomeDataOffsets.GetSlotOffset(end.box, end.slot);
        var size = (int)(endOffset - startOffset);
        var data = await SwitchConnection.ReadBytesAsync(startOffset, size, token).ConfigureAwait(false);
        Log("Done.");
        return data;
    }
}
