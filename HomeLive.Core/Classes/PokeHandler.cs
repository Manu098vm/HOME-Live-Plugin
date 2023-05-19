using PKHeX.Core;
using System.Buffers.Binary;
using System.Text;

namespace HomeLive.Core;

public static class PokeHandler
{
    public static readonly string[] CompatibleFormats = { ".ph1", ".eh1", ".pkh", ".ekh" };

    public static bool IsCompatibleExtension(string ext)
    {
        foreach (var format in CompatibleFormats)
            if (format.Equals(ext)) return true;
        return false;
    }

    public static void Dump(this PKH pkh, DumpFormat format, string path)
    {
        Directory.CreateDirectory(path);
        var filename = pkh.GetFileName();
        if (format is DumpFormat.Encrypted or DumpFormat.EncAndDec)
        {
            var ext = $".eh{pkh.DataVersion}";
            File.WriteAllBytes($"{Path.Combine(path, filename)}{ext}", HomeCrypto.Encrypt(pkh.Data));
        }
        if (format is DumpFormat.Decrypted or DumpFormat.EncAndDec)
        {
            var ext = $".ph{pkh.DataVersion}";
            File.WriteAllBytes($"{Path.Combine(path, filename)}{ext}", pkh.Data);
        }
    }

    public static PKH? GenerateEntityFromPath(string path)
    {
        var data = File.ReadAllBytes(path);
        return GenerateEntityFromBin(data);
    }

    public static PKH? GenerateEntityFromBin(ReadOnlySpan<byte> bin)
    {
        var header = bin[..HomeDataOffsets.HeaderLength];

        if (BinaryPrimitives.ReadUInt64LittleEndian(header[HomeDataOffsets.SeedOffset..]) == 0) 
            return null;

        var encSize = HomeDataOffsets.HeaderLength + BinaryPrimitives.ReadUInt16LittleEndian(header[HomeDataOffsets.EncSizeOffset..]);
        var pk = new PKH(bin[..encSize].ToArray());

        if (pk.ChecksumValid) 
            return pk;

        return null;
    }

    public static List<PKH?> GenerateEntitiesFromBoxBin(ReadOnlySpan<byte> data)
    {
        var pkmsData = ArrayUtil.EnumerateSplit(data.ToArray(), HomeDataOffsets.HomeSlotSize);
        var list = new List<PKH?>();
        foreach(var entityData in pkmsData)
            list.Add(GenerateEntityFromBin(entityData));
        return list;
    }

    public static string GetFileName(this PKH pkm)
    {
        var name = $"{pkm.Species:0000}";
        if (pkm.Form > 0 || pkm.Species == (ushort)Species.Alcremie)
            name += $"-{pkm.Form:00}";
        if (pkm.Species == (ushort)Species.Alcremie)
            name += $"-{pkm.FormArgument:00}";

        if (pkm.IsShiny)
        {
            if (pkm.ShinyXor == 0 || pkm.FatefulEncounter)
                name += " ■";
            else
                name += " ★";
        }

        if (pkm.DataPK8 is IGigantamax g && g.CanGigantamax)
            name += " (GMax)";
        if (pkm.DataPA8 is GameDataPA8 a && a.IsAlpha)
            name += " (Alpha)";
        if (pkm.DataPA8 is GameDataPA8 n && n.IsNoble)
            name += " (Noble)";

        name += $" - {pkm.GetFilteredString()}";
        name += $" {pkm.Tracker:X16}";

        return name;
    }

    private static string GetFilteredString(this PKH pkm)
    {
        var sb = new StringBuilder();
        foreach (char c in pkm.Nickname)
            if (c != '\\' && c != '/' && c != ':' && c != '*' && c != '?' && c != '"' && c != '<' && c != '>' && c != '|')
                sb.Append(c);
        return sb.ToString();
    }

}