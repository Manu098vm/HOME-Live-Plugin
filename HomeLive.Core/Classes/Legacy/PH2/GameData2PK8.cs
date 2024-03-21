/* Legacy code for PKH Data Version 2.
 * https://github.com/kwsch/PKHeX/blob/75ec6ca38dcd9ac50e781434e390c14d671ebae1/PKHeX.Core/PKM/HOME/GameDataPK8.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data for <see cref="PK8"/> data transferred into HOME.
/// </summary>
public sealed class GameData2PK8 : HomeOptional2, IGameDataSide2<PK8>, IGigantamax, IDynamaxLevel, ISociability, IGameDataSplitAbility, IPokerusStatus
{
    private const HomeGameDataFormat ExpectFormat = HomeGameDataFormat.PK8;
    private const int SIZE = 0x48;
    protected override HomeGameDataFormat Format => ExpectFormat;

    public GameData2PK8() : base(SIZE) { }
    public GameData2PK8(Memory<byte> buffer) : base(buffer) => EnsureSize(SIZE);
    public GameData2PK8 Clone() => new(ToArray());
    public int WriteTo(Span<byte> result) => WriteWithHeader(result);

    #region Structure

    public bool CanGigantamax { get => Data[0x00] != 0; set => Data[0x00] = (byte)(value ? 1 : 0); }
    public uint Sociability { get => ReadUInt32LittleEndian(Data[0x01..]); set => WriteUInt32LittleEndian(Data[0x01..], value); }

    public ushort Move1 { get => ReadUInt16LittleEndian(Data[0x05..]); set => WriteUInt16LittleEndian(Data[0x05..], value); }
    public ushort Move2 { get => ReadUInt16LittleEndian(Data[0x07..]); set => WriteUInt16LittleEndian(Data[0x07..], value); }
    public ushort Move3 { get => ReadUInt16LittleEndian(Data[0x09..]); set => WriteUInt16LittleEndian(Data[0x09..], value); }
    public ushort Move4 { get => ReadUInt16LittleEndian(Data[0x0B..]); set => WriteUInt16LittleEndian(Data[0x0B..], value); }

    public int Move1_PP { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }
    public int Move2_PP { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
    public int Move3_PP { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }
    public int Move4_PP { get => Data[0x10]; set => Data[0x10] = (byte)value; }
    public int Move1_PPUps { get => Data[0x11]; set => Data[0x11] = (byte)value; }
    public int Move2_PPUps { get => Data[0x12]; set => Data[0x12] = (byte)value; }
    public int Move3_PPUps { get => Data[0x13]; set => Data[0x13] = (byte)value; }
    public int Move4_PPUps { get => Data[0x14]; set => Data[0x14] = (byte)value; }

    public ushort RelearnMove1 { get => ReadUInt16LittleEndian(Data[0x15..]); set => WriteUInt16LittleEndian(Data[0x15..], value); }
    public ushort RelearnMove2 { get => ReadUInt16LittleEndian(Data[0x17..]); set => WriteUInt16LittleEndian(Data[0x17..], value); }
    public ushort RelearnMove3 { get => ReadUInt16LittleEndian(Data[0x19..]); set => WriteUInt16LittleEndian(Data[0x19..], value); }
    public ushort RelearnMove4 { get => ReadUInt16LittleEndian(Data[0x1B..]); set => WriteUInt16LittleEndian(Data[0x1B..], value); }
    public byte DynamaxLevel { get => Data[0x1D]; set => Data[0x1D] = value; }

    private Span<byte> PokeJob => Data.Slice(0x1E, 14);
    public bool GetPokeJobFlag(int index) => FlagUtil.GetFlag(PokeJob, index >> 3, index & 7);
    public void SetPokeJobFlag(int index, bool value) => FlagUtil.SetFlag(PokeJob, index >> 3, index & 7, value);
    public bool GetPokeJobFlagAny() => PokeJob.IndexOfAnyExcept<byte>(0) >= 0;
    public void ClearPokeJobFlags() => PokeJob.Clear();

    public byte Fullness { get => Data[0x2C]; set => Data[0x2C] = value; }

    private Span<byte> RecordFlags => Data.Slice(0x2D, 14);
    public bool GetMoveRecordFlag(int index) => FlagUtil.GetFlag(RecordFlags, index >> 3, index & 7);
    public void SetMoveRecordFlag(int index, bool value) => FlagUtil.SetFlag(RecordFlags, index >> 3, index & 7, value);
    public bool GetMoveRecordFlagAny() => RecordFlags.IndexOfAnyExcept<byte>(0) >= 0;
    public void ClearMoveRecordFlags() => RecordFlags.Clear();

    public int Palma { get => ReadInt32LittleEndian(Data[0x3B..]); set => WriteInt32LittleEndian(Data[0x3B..], value); }
    public byte Ball { get => Data[0x3F]; set => Data[0x3F] = (byte)value; }
    public ushort EggLocation { get => ReadUInt16LittleEndian(Data[0x40..]); set => WriteUInt16LittleEndian(Data[0x40..], value); }
    public ushort MetLocation { get => ReadUInt16LittleEndian(Data[0x42..]); set => WriteUInt16LittleEndian(Data[0x42..], value); }

    // Rev2 Additions
    public byte PokerusState { get => Data[0x44]; set => Data[0x44] = value; }
    public ushort Ability { get => ReadUInt16LittleEndian(Data[0x45..]); set => WriteUInt16LittleEndian(Data[0x45..], value); }
    public byte AbilityNumber { get => Data[0x47]; set => Data[0x47] = value; }

    #endregion

    #region Conversion

    public PersonalInfo GetPersonalInfo(ushort species, byte form) => PersonalTable.SWSH.GetFormEntry(species, form);

    public void CopyTo(PK8 pk, PH2 pkh)
    {
        this.CopyTo(pk);
        pk.CanGigantamax = CanGigantamax;
        pk.Sociability = Sociability;
        pk.DynamaxLevel = DynamaxLevel;
        pk.Fullness = Fullness;
        pk.Palma = Palma;
        PokeJob.CopyTo(pk.PokeJob);
        RecordFlags.CopyTo(pk.RecordFlags);
        pk.PokerusState = PokerusState;
        pk.AbilityNumber = AbilityNumber;
        pk.Ability = Ability;

        if (!IsOriginallySWSH(pkh.Version, pk.MetLocation))
            pk.Version = LocationsHOME.GetVersionSWSH(pkh.Version);
    }

    public void CopyFrom(PK8 pk, PH2 pkh)
    {
        this.CopyFrom(pk);
        CanGigantamax = pk.CanGigantamax;
        Sociability = pk.Sociability;
        DynamaxLevel = pk.DynamaxLevel;
        Fullness = pk.Fullness;
        Palma = pk.Palma;
        pk.PokeJob.CopyTo(PokeJob);
        pk.RecordFlags.CopyTo(RecordFlags);
        PokerusState = pk.PokerusState;
        AbilityNumber = (byte)pk.AbilityNumber;
        Ability = (ushort)pk.Ability;
    }

    public void CopyFrom(PK7 pk, PH2 pkh)
    {
        this.CopyFrom(pk);
        PokerusState = pk.PokerusState;
        AbilityNumber = (byte)pk.AbilityNumber;
        Ability = (ushort)pk.Ability;

        pkh.MarkingValue &= 0b1111_1111_1111;
        if (!pk.IsNicknamed)
            pkh.Nickname = SpeciesName.GetSpeciesNameGeneration(pk.Species, pk.Language, 8);
        if (FormInfo.IsTotemForm(pk.Species, pk.Form))
            pkh.Form = FormInfo.GetTotemBaseForm(pk.Species, pk.Form);
    }

    public PK8 ConvertToPKM(PH2 pkh)
    {
        var pk = new PK8();
        pkh.CopyTo(pk);
        CopyTo(pk, pkh);

        pk.ResetPartyStats();
        pk.RefreshChecksum();
        return pk;
    }

    #endregion

    /// <summary> Reconstructive logic to best apply suggested values. </summary>
    public static GameData2PK8? TryCreate(PH2 pkh)
    {
        if (pkh.DataPB7 is { } x)
            return CreateViaPB7(pkh, x);

        var side = GetNearestNeighbor(pkh);
        if (side is not null)
            return Create(side, pkh);

        return null;
    }

    // Ignores LGP/E, already preferred if exists.
    private static IGameDataSide2? GetNearestNeighbor(PH2 pkh) => pkh.DataPK9 as IGameDataSide2
                                                              ?? pkh.DataPB8 as IGameDataSide2
                                                              ?? pkh.DataPA8;

    private static GameData2PK8 CreateViaPB7(PH2 pkh, GameData2PB7 x)
    {
        var result = new GameData2PK8();
        x.CopyTo(result); // Moves are copied by default.
        result.AbilityNumber = x.AbilityNumber;

        result.PopulateFromCore(pkh);
        return result;
    }

    private static GameData2PK8 Create(IGameDataSide2 side, PH2 pkh)
    {
        var result = new GameData2PK8();
        result.InitializeFrom(side, pkh);

        result.ResetMoves(pkh.Species, pkh.Form, pkh.CurrentLevel, LearnSource8SWSH.Instance, EntityContext.Gen8);
        return result;
    }

    public void InitializeFrom(IGameDataSide2 side, PH2 pkh)
    {
        // BDSP->SWSH: Set the Met Location to the magic Location, set the Egg Location to 0 if -1, otherwise BDSPEgg
        // (0 is a valid location, but no eggs can be EggMet there -- only hatched.)
        // PLA->SWSH: Set the Met Location to the magic Location, set the Egg Location to 0 (no eggs in game).
        var ver = pkh.Version;
        var met = side.MetLocation;
        var ball = GetBall(side.Ball);
        var egg = GetEggLocation(side.EggLocation);
        if (!IsOriginallySWSH(ver, met))
            RemapMetEgg(ver, ref met, ref egg);
        Ball = ball;
        MetLocation = met;
        EggLocation = egg;
        if (side is IGameDataSplitAbility a)
            AbilityNumber = a.AbilityNumber;
        if (side is IPokerusStatus p)
            PokerusState = p.PokerusState;

        PopulateFromCore(pkh);
    }

    private void PopulateFromCore(PH2 pkh)
    {
        var pi = PersonalTable.SWSH.GetFormEntry(pkh.Species, pkh.Form);
        Ability = (ushort)pi.GetAbilityAtIndex(AbilityNumber >> 1);
    }

    private static void RemapMetEgg(GameVersion ver, ref ushort met, ref ushort egg)
    {
        var remap = LocationsHOME.GetMetSWSH(met, ver);
        if (remap == met)
            return;

        met = remap;
        egg = egg is 0 or Locations.Default8bNone ? (ushort)0 : LocationsHOME.SWSHEgg;
    }

    private static bool IsOriginallySWSH(GameVersion ver, ushort loc) => ver is GameVersion.SW or GameVersion.SH && !IsFakeMetLocation(loc);
    private static bool IsFakeMetLocation(ushort met) => LocationsHOME.IsLocationSWSH(met);
    private static byte GetBall(byte ball) => ball > (byte)PKHeX.Core.Ball.Beast ? (byte)4 : ball;
    private static ushort GetEggLocation(ushort egg) => egg == Locations.Default8bNone ? (ushort)0 : egg;
}
