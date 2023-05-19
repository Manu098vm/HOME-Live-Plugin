using PKHeX.Core;

namespace HomeLive.Core;

public static class VersionHandler
{
    private const int LGPEBoxes = 40;
    private const int LGPESlots = 25;
    private const int SWSHBoxes = 32;
    private const int SWSHSlots = 30;
    private const int BDSPBoxes = 40;
    private const int BDSPSlots = 30;
    private const int LABoxes = 32;
    private const int LASlots = 30;
    private const int SVBoxes = 32;
    private const int SVSlots = 30;

    public static GameVersion GetGameVersion(this SaveFile sav) => GetGameVersion(sav.Version);
    public static GameVersion GetGameVersion(int version) => GetGameVersion((GameVersion)version);

    public static GameVersion GetGameVersion(this Type type)
    {
        if (type == typeof(PB7)) return GameVersion.GG;
        if (type == typeof(PK8)) return GameVersion.SWSH;
        if (type == typeof(PB8)) return GameVersion.BDSP;
        if (type == typeof(PA8)) return GameVersion.PLA;
        if (type == typeof(PK9)) return GameVersion.SV;
        else throw new ArgumentException($"{nameof(type)} is not a valid version for this operation.");
    }

    public static GameVersion GetGameVersion(this GameVersion version) => version switch
    {
        GameVersion.GG or GameVersion.GP or GameVersion.GE => GameVersion.GG,
        GameVersion.SWSH or GameVersion.SW or GameVersion.SH => GameVersion.SWSH,
        GameVersion.BDSP or GameVersion.BD or GameVersion.SP => GameVersion.BDSP,
        GameVersion.PLA => GameVersion.PLA,
        GameVersion.SV or GameVersion.SL or GameVersion.VL => GameVersion.SV,
        _ => throw new ArgumentException($"{nameof(version)} is not a valid version for this operation.")
    };

    public static int GetBoxCount(this GameVersion version) => GetGameVersion(version) switch
    {
        GameVersion.GG => LGPEBoxes,
        GameVersion.SWSH => SWSHBoxes,
        GameVersion.BDSP => BDSPBoxes,
        GameVersion.PLA => LABoxes,
        GameVersion.SV => SVBoxes,
        _ => throw new ArgumentException($"{nameof(version)} is not a valid version for this operation.")
    };

    public static int GetSlotCount(this GameVersion version) => version switch
    {
        GameVersion.GG => LGPESlots,
        GameVersion.SWSH => SWSHSlots,
        GameVersion.BDSP => BDSPSlots,
        GameVersion.PLA => LASlots,
        GameVersion.SV => SVSlots,
        _ => throw new ArgumentException($"{nameof(version)} is not a valid version for this operation.")
    };

    public static List<PKM?> ConvertListToType(this List<PKH?> list, Type destType, ConversionType conversionType)
    {
        var pks = new List<PKM?>();
        foreach(var pkh in list)
            pks.Add(pkh?.ConvertToType(destType, conversionType));
        return pks;        
    }

    public static PKM? ConvertToType(this PKH? pkm, Type destType, ConversionType conversionType)
    {
        if (pkm is null) return null;
        var forceConversion = conversionType is ConversionType.AnyData || (conversionType is ConversionType.CompatibleData && CanConvertToType(pkm, destType));
        return pkm.ConvertToPKM(destType, forceConversion);
    }

    private static PKM? ConvertToPKM(this PKH pkm, Type destType, bool forceConversion)
    {
        if (destType == typeof(PB7) && (pkm.DataPB7 is not null && CanConvertToType(pkm, destType)|| forceConversion))
            return pkm.ConvertToPB7();
        else if (destType == typeof(PK8) && (pkm.DataPK8 is not null && CanConvertToType(pkm, destType) || forceConversion))
            return pkm.ConvertToPK8();
        else if (destType == typeof(PB8) && (pkm.DataPB8 is not null && CanConvertToType(pkm, destType) || forceConversion))
            return pkm.ConvertToPB8();
        else if (destType == typeof(PA8) && (pkm.DataPA8 is not null && CanConvertToType(pkm, destType) || forceConversion))
            return pkm.ConvertToPA8();
        else if (destType == typeof(PK9) && (pkm.DataPK9 is not null && CanConvertToType(pkm, destType) || forceConversion))
            return pkm.ConvertToPK9();

        return null;
    }

    public static bool CanConvertToType(PKH pkm, Type destType)
    {
        var version = GetGameVersion(destType);
        return CheckVersionAvailability(pkm, version);
    }

    public static bool CheckVersionAvailability(PKM pkm, GameVersion version) => version switch
    {
        GameVersion.GG => pkm.CheckLGPEAvailability(),
        GameVersion.SWSH => pkm.CheckSWSHAvailability(),
        GameVersion.BDSP => pkm.CheckBDSPAvailability(),
        GameVersion.PLA => pkm.CheckPLAAvailability(),
        GameVersion.SV => pkm.CheckSVAvailability(),
        _ => throw new ArgumentException($"{nameof(version)} is not a valid version for this operation.")
    };

    private static bool CheckSVAvailability(this PKM pk) => PersonalTable.SV.IsPresentInGame(pk.Species, pk.Form);
    private static bool CheckPLAAvailability(this PKM pk) => PersonalTable.LA.IsPresentInGame(pk.Species, pk.Form);
    private static bool CheckBDSPAvailability(this PKM pk) => PersonalTable.BDSP.IsPresentInGame(pk.Species, pk.Form);
    private static bool CheckSWSHAvailability(this PKM pk) => PersonalTable.SWSH.IsPresentInGame(pk.Species, pk.Form);
    private static bool CheckLGPEAvailability(this PKM pk) => PersonalTable.GG.IsPresentInGame(pk.Species, pk.Form);
}