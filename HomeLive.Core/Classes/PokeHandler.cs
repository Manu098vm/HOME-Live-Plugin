using PKHeX.Core;
using System.Buffers.Binary;
using System.Text;
using HomeLive.DeviceExecutor;

namespace HomeLive.Core;

public static class PokeHandler
{
    public static readonly string[] CompatibleFormats = [".ph3", ".eh3", ".ph2", ".eh2", ".ph1", ".eh1", ".pkh", ".ekh"];

    public static bool IsCompatibleExtension(string ext)
    {
        foreach (var format in CompatibleFormats)
            if (format.Equals(ext)) return true;
        return false;
    }

    public static void Dump(this PKM pkm, DumpFormat format, string path) => Dump(new HomeWrapper(pkm), format, path);

    public static void Dump(this HomeWrapper pkh, DumpFormat format, string path)
    {
        Directory.CreateDirectory(path);
        var filename = pkh.GetFileName();
        if (format is DumpFormat.Encrypted or DumpFormat.EncAndDec)
        {
            var ext = $".eh{pkh.DataVersion}";
            File.WriteAllBytes($"{Path.Combine(path, filename)}{ext}", pkh.EncryptedData);
        }
        if (format is DumpFormat.Decrypted or DumpFormat.EncAndDec)
        {
            var ext = $".ph{pkh.DataVersion}";
            File.WriteAllBytes($"{Path.Combine(path, filename)}{ext}", pkh.Data);
        }
    }

    public static HomeWrapper? GenerateEntityFromPath(string path)
    {
        var data = File.ReadAllBytes(path);
        return GenerateEntityFromBin(data);
    }

    public static HomeWrapper? GenerateEntityFromBin(ReadOnlySpan<byte> bin)
    {
        var header = bin[..HomeDataOffsets.HeaderLength];

        if (BinaryPrimitives.ReadUInt64LittleEndian(header[HomeDataOffsets.SeedOffset..]) == 0) 
            return null;

        var encSize = HomeDataOffsets.HeaderLength + BinaryPrimitives.ReadUInt16LittleEndian(header[HomeDataOffsets.EncSizeOffset..]);
        var pk = new HomeWrapper(bin[..encSize].ToArray());

        if (pk.IsValid) 
            return pk;

        return null;
    }

    public static List<HomeWrapper?> GenerateEntitiesFromBoxBin(ReadOnlySpan<byte> data)
    {
        var pkmsData = EnumerateSplit(data.ToArray(), HomeDataOffsets.HomeSlotSize);
        var list = new List<HomeWrapper?>();
        foreach(var entityData in pkmsData)
            list.Add(GenerateEntityFromBin(entityData));
        return list;
    }

    public static string GetFileName(this HomeWrapper pkh)
    {
        var pkm = pkh.PKM!;
        var name = $"{pkm.Species:0000}";
        if (pkm.Form > 0 || pkm.Species == (ushort)Species.Alcremie)

            name += $"-{pkm.Form:00}";
        if (pkm.Species == (ushort)Species.Alcremie && pkm is IFormArgument arg)
            name += $"-{arg.FormArgument:00}";

        if (pkm.IsShiny)
            if (pkm.ShinyXor == 0 || pkm.FatefulEncounter)
                name += " ■";
            else name += " ★";

        if (pkh.HasPK8() && pkh.ConvertToPK8() is IGigantamax g && g.CanGigantamax)
            name += " (GMax)";

        if (pkh.HasPA8())
        {
            var pa8 = pkh.ConvertToPA8();
            if (pa8 is IAlpha a && a.IsAlpha)
                name += " (Alpha)";
            if (pa8 is INoble n && n.IsNoble)
                name += " (Noble)";
        }

        name += $" - {pkm.GetFilteredString()}";
        name += $" {pkh.Tracker:X16}";

        return name;
    }

    private static string GetFilteredString(this PKM pkm)
    {
        var sb = new StringBuilder();
        foreach (char c in pkm.Nickname)
            if (c != '\\' && c != '/' && c != ':' && c != '*' && c != '?' && c != '"' && c != '<' && c != '>' && c != '|')
                sb.Append(c);
        return sb.ToString();
    }

    private static IEnumerable<T[]> EnumerateSplit<T>(T[] bin, int size, int start = 0)
    {
        for (int i = start; i < bin.Length; i += size)
            yield return bin.AsSpan(i, size).ToArray();
    }
}