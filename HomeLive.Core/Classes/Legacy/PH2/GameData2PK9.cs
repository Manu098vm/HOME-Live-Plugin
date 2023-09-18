using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data for <see cref="PK9"/> data transferred into HOME.
/// </summary>
public sealed class GameData2PK9 : HomeOptional2, IGameDataSide2<PK9>, IScaledSize3, IGameDataSplitAbility
{
    private const HomeGameDataFormat ExpectFormat = HomeGameDataFormat.PK9;
    private const int SIZE = 0x3D;
    protected override HomeGameDataFormat Format => ExpectFormat;

    public GameData2PK9() : base(SIZE) { }
    public GameData2PK9(Memory<byte> data) : base(data) => EnsureSize(SIZE);
    public GameData2PK9 Clone() => new(ToArray());
    public int WriteTo(Span<byte> result) => WriteWithHeader(result);

    #region Structure

    public byte Scale { get => Data[0x00]; set => Data[0x00] = value; }
    public ushort Move1 { get => ReadUInt16LittleEndian(Data[0x01..]); set => WriteUInt16LittleEndian(Data[0x01..], value); }
    public ushort Move2 { get => ReadUInt16LittleEndian(Data[0x03..]); set => WriteUInt16LittleEndian(Data[0x03..], value); }
    public ushort Move3 { get => ReadUInt16LittleEndian(Data[0x05..]); set => WriteUInt16LittleEndian(Data[0x05..], value); }
    public ushort Move4 { get => ReadUInt16LittleEndian(Data[0x07..]); set => WriteUInt16LittleEndian(Data[0x07..], value); }

    public int Move1_PP    { get => Data[0x09]; set => Data[0x09] = (byte)value; }
    public int Move2_PP    { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
    public int Move3_PP    { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }
    public int Move4_PP    { get => Data[0x0C]; set => Data[0x0C] = (byte)value; }
    public int Move1_PPUps { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }
    public int Move2_PPUps { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
    public int Move3_PPUps { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }
    public int Move4_PPUps { get => Data[0x10]; set => Data[0x10] = (byte)value; }

    public ushort RelearnMove1 { get => ReadUInt16LittleEndian(Data[0x11..]); set => WriteUInt16LittleEndian(Data[0x11..], value); }
    public ushort RelearnMove2 { get => ReadUInt16LittleEndian(Data[0x13..]); set => WriteUInt16LittleEndian(Data[0x13..], value); }
    public ushort RelearnMove3 { get => ReadUInt16LittleEndian(Data[0x15..]); set => WriteUInt16LittleEndian(Data[0x15..], value); }
    public ushort RelearnMove4 { get => ReadUInt16LittleEndian(Data[0x17..]); set => WriteUInt16LittleEndian(Data[0x17..], value); }
    public MoveType TeraTypeOriginal { get => (MoveType)Data[0x19]; set => Data[0x19] = (byte)value; }
    public MoveType TeraTypeOverride { get => (MoveType)Data[0x1A]; set => Data[0x1A] = (byte)value; }
    public int Ball { get => Data[0x1B]; set => Data[0x1B] = (byte)value; }
    public int Egg_Location { get => ReadUInt16LittleEndian(Data[0x1C..]); set => WriteUInt16LittleEndian(Data[0x1C..], (ushort)value); }
    public int Met_Location { get => ReadUInt16LittleEndian(Data[0x1E..]); set => WriteUInt16LittleEndian(Data[0x1E..], (ushort)value); }

    private const int RecordStart = 0x20;
    private const int RecordCount = 200; // Up to 200 TM flags, but not all are used.
    private const int RecordLength = RecordCount / 8;
    private Span<byte> RecordFlags => Data.Slice(RecordStart, RecordLength);

    public bool GetMoveRecordFlag(int index)
    {
        if ((uint)index > RecordCount) // 0x19 bytes, 8 bits
            throw new ArgumentOutOfRangeException(nameof(index));
        return FlagUtil.GetFlag(RecordFlags, index >> 3, index & 7);
    }

    public void SetMoveRecordFlag(int index, bool value = true)
    {
        if ((uint)index > RecordCount) // 0x19 bytes, 8 bits
            throw new ArgumentOutOfRangeException(nameof(index));
        FlagUtil.SetFlag(RecordFlags, index >> 3, index & 7, value);
    }

    public bool GetMoveRecordFlagAny() => RecordFlags.IndexOfAnyExcept<byte>(0) >= 0;
    public void ClearMoveRecordFlags() => RecordFlags.Clear();

    // Rev2 Additions
    public byte Obedience_Level { get => Data[0x39]; set => Data[0x39] = value; }
    public ushort Ability { get => ReadUInt16LittleEndian(Data[0x3A..]); set => WriteUInt16LittleEndian(Data[0x3A..], value); }
    public byte AbilityNumber { get => Data[0x3C]; set => Data[0x3C] = value; }

    #endregion

    #region Conversion

    public PersonalInfo GetPersonalInfo(ushort species, byte form) => PersonalTable.SV.GetFormEntry(species, form);

    public void CopyTo(PK9 pk, PH2 pkh)
    {
        this.CopyTo(pk);
        pk.Scale = Scale;
        pk.TeraTypeOriginal = TeraTypeOriginal;
        pk.TeraTypeOverride = TeraTypeOverride;
        RecordFlags.CopyTo(pk.RecordFlagsBase);
        pk.Obedience_Level = Obedience_Level;
        pk.Ability = Ability;
        pk.AbilityNumber = AbilityNumber;
    }

    public void CopyFrom(PK9 pk, PH2 pkh)
    {
        this.CopyFrom(pk);
        pkh.HeightScalar = Scale = pk.Scale; // Overwrite Height
        TeraTypeOriginal = pk.TeraTypeOriginal;
        TeraTypeOverride = pk.TeraTypeOverride;
        pk.RecordFlagsBase.CopyTo(RecordFlags);
        Obedience_Level = pk.Obedience_Level;
        Ability = (ushort)pk.Ability;
        AbilityNumber = (byte)pk.AbilityNumber;
    }

    public PK9 ConvertToPKM(PH2 pkh)
    {
        var pk = new PK9();
        pkh.CopyTo(pk);
        CopyTo(pk, pkh);
        pk.ResetPartyStats();
        pk.RefreshChecksum();
        return pk;
    }

    #endregion

    /// <summary> Reconstructive logic to best apply suggested values. </summary>
    public static GameData2PK9? TryCreate(PH2 pkh)
    {
        if (!PersonalTable.SV.IsPresentInGame(pkh.Species, pkh.Form))
            return null;

        var result = CreateInternal(pkh);
        if (result == null)
            return null;

        result.PopulateFromCore(pkh);
        return result;
    }

    private static GameData2PK9? CreateInternal(PH2 pkh)
    {
        var side = GetNearestNeighbor(pkh);
        if (side == null)
            return null;

        var result = new GameData2PK9();
        result.InitializeFrom(side, pkh);
        return result;
    }

    private static IGameDataSide2? GetNearestNeighbor(PH2 pkh) => pkh.DataPK8 as IGameDataSide2
                                                              ?? pkh.DataPB8 as IGameDataSide2
                                                              ?? pkh.DataPB7 as IGameDataSide2
                                                              ?? pkh.DataPA8;

    public void InitializeFrom(IGameDataSide2 side, PH2 pkh)
    {
        Ball = side.Ball;
        Met_Location = side.Met_Location == Locations.Default8bNone ? 0 : side.Met_Location;
        Egg_Location = side.Egg_Location == Locations.Default8bNone ? 0 : side.Egg_Location;

        if (side is IScaledSize3 s3)
            Scale = s3.Scale;
        else
            Scale = pkh.HeightScalar;
        if (side is IGameDataSplitAbility a)
            AbilityNumber = a.AbilityNumber;
        else
            AbilityNumber = 1;

        PopulateFromCore(pkh);
    }

    private void PopulateFromCore(PH2 pkh)
    {
        Obedience_Level = (byte)pkh.Met_Level;

        var pi = PersonalTable.SV.GetFormEntry(pkh.Species, pkh.Form);
        Ability = (ushort)pi.GetAbilityAtIndex(AbilityNumber >> 1);
        TeraTypeOriginal = TeraTypeOverride = TeraTypeUtil.GetTeraTypeImport(pi.Type1, pi.Type2);

        var level = Experience.GetLevel(pkh.EXP, pi.EXPGrowth);
        this.ResetMoves(pkh.Species, pkh.Form, level, LearnSource9SV.Instance, EntityContext.Gen9);
    }
}
