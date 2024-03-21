/* Legacy code for PKH Data Version 2.
 * https://github.com/kwsch/PKHeX/blob/75ec6ca38dcd9ac50e781434e390c14d671ebae1/PKHeX.Core/PKM/HOME/GameDataPB8.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data for <see cref="PB8"/> data transferred into HOME.
/// </summary>
public sealed class GameData2PB8 : HomeOptional2, IGameDataSide2<PB8>, IGameDataSplitAbility, IPokerusStatus
{
    private const HomeGameDataFormat ExpectFormat = HomeGameDataFormat.PB8;
    private const int SIZE = 0x2F;
    protected override HomeGameDataFormat Format => ExpectFormat;

    public GameData2PB8() : base(SIZE) { }
    public GameData2PB8(Memory<byte> data) : base(data) => EnsureSize(SIZE);
    public GameData2PB8 Clone() => new(ToArray());
    public int WriteTo(Span<byte> result) => WriteWithHeader(result);

    #region Structure

    public ushort Move1 { get => ReadUInt16LittleEndian(Data); set => WriteUInt16LittleEndian(Data, value); }
    public ushort Move2 { get => ReadUInt16LittleEndian(Data[0x02..]); set => WriteUInt16LittleEndian(Data[0x02..], value); }
    public ushort Move3 { get => ReadUInt16LittleEndian(Data[0x04..]); set => WriteUInt16LittleEndian(Data[0x04..], value); }
    public ushort Move4 { get => ReadUInt16LittleEndian(Data[0x06..]); set => WriteUInt16LittleEndian(Data[0x06..], value); }

    public int Move1_PP { get => Data[0x08]; set => Data[0x08] = (byte)value; }
    public int Move2_PP { get => Data[0x09]; set => Data[0x09] = (byte)value; }
    public int Move3_PP { get => Data[0x0A]; set => Data[0x0A] = (byte)value; }
    public int Move4_PP { get => Data[0x0B]; set => Data[0x0B] = (byte)value; }
    public int Move1_PPUps { get => Data[0x0C]; set => Data[0x0C] = (byte)value; }
    public int Move2_PPUps { get => Data[0x0D]; set => Data[0x0D] = (byte)value; }
    public int Move3_PPUps { get => Data[0x0E]; set => Data[0x0E] = (byte)value; }
    public int Move4_PPUps { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }

    public ushort RelearnMove1 { get => ReadUInt16LittleEndian(Data[0x10..]); set => WriteUInt16LittleEndian(Data[0x10..], value); }
    public ushort RelearnMove2 { get => ReadUInt16LittleEndian(Data[0x12..]); set => WriteUInt16LittleEndian(Data[0x12..], value); }
    public ushort RelearnMove3 { get => ReadUInt16LittleEndian(Data[0x14..]); set => WriteUInt16LittleEndian(Data[0x14..], value); }
    public ushort RelearnMove4 { get => ReadUInt16LittleEndian(Data[0x16..]); set => WriteUInt16LittleEndian(Data[0x16..], value); }

    private Span<byte> RecordFlag => Data.Slice(0x18, 14);
    public bool GetMoveRecordFlag(int index) => FlagUtil.GetFlag(RecordFlag, index >> 3, index & 7);
    public void SetMoveRecordFlag(int index, bool value) => FlagUtil.SetFlag(RecordFlag, index >> 3, index & 7, value);
    public bool GetMoveRecordFlagAny() => RecordFlag.IndexOfAnyExcept<byte>(0) >= 0;
    public void ClearMoveRecordFlags() => RecordFlag.Clear();

    public byte Ball { get => Data[0x26]; set => Data[0x26] = (byte)value; }
    public ushort EggLocation { get => ReadUInt16LittleEndian(Data[0x27..]); set => WriteUInt16LittleEndian(Data[0x27..], value); }
    public ushort MetLocation { get => ReadUInt16LittleEndian(Data[0x29..]); set => WriteUInt16LittleEndian(Data[0x29..], value); }

    // Rev2 Additions
    public byte PokerusState { get => Data[0x2B]; set => Data[0x2B] = value; }
    public ushort Ability { get => ReadUInt16LittleEndian(Data[0x2C..]); set => WriteUInt16LittleEndian(Data[0x2C..], value); }
    public byte AbilityNumber { get => Data[0x2E]; set => Data[0x2E] = value; }

    #endregion

    #region Conversion

    public PersonalInfo GetPersonalInfo(ushort species, byte form) => PersonalTable.BDSP.GetFormEntry(species, form);

    public void CopyTo(PB8 pk, PH2 pkh)
    {
        this.CopyTo(pk);
        // Move Records are not settable in PB8; do not copy even if nonzero (illegal).
        pk.PokerusState = PokerusState;
        pk.AbilityNumber = AbilityNumber;
        pk.Ability = Ability;
    }

    public void CopyFrom(PB8 pk, PH2 pkh)
    {
        this.CopyFrom(pk);
        // Move Records are not settable in PB8; do not copy even if nonzero (illegal).
        PokerusState = pk.PokerusState;
        AbilityNumber = (byte)pk.AbilityNumber;
        Ability = (ushort)pk.Ability;
    }

    public PB8 ConvertToPKM(PH2 pkh)
    {
        var pk = new PB8();
        pkh.CopyTo(pk);
        CopyTo(pk, pkh);

        pk.ResetPartyStats();
        pk.RefreshChecksum();
        return pk;
    }

    #endregion

    /// <summary> Reconstructive logic to best apply suggested values. </summary>
    public static GameData2PB8? TryCreate(PH2 pkh)
    {
        var side = GetNearestNeighbor(pkh);
        if (side == null)
            return null;

        var result = new GameData2PB8();
        result.InitializeFrom(side, pkh);
        return result;
    }

    private static IGameDataSide2? GetNearestNeighbor(PH2 pkh) => pkh.DataPK9 as IGameDataSide2
                                                              ?? pkh.DataPK8 as IGameDataSide2
                                                              ?? pkh.DataPB7 as IGameDataSide2
                                                              ?? pkh.DataPA8;

    public void InitializeFrom(IGameDataSide2 side, PH2 pkh)
    {
        Ball = side.Ball;
        MetLocation = side.MetLocation == 0 ? Locations.Default8bNone : side.MetLocation;
        EggLocation = side.EggLocation == 0 ? Locations.Default8bNone : side.EggLocation;

        if (side is IPokerusStatus p)
            PokerusState = p.PokerusState;
        if (side is IGameDataSplitAbility a)
            AbilityNumber = a.AbilityNumber;
        else
            AbilityNumber = 1;

        PopulateFromCore(pkh);
    }

    private void PopulateFromCore(PH2 pkh)
    {
        var pi = PersonalTable.BDSP.GetFormEntry(pkh.Species, pkh.Form);
        Ability = (ushort)pi.GetAbilityAtIndex(AbilityNumber >> 1);

        var level = Experience.GetLevel(pkh.EXP, pi.EXPGrowth);
        this.ResetMoves(pkh.Species, pkh.Form, level, LearnSource8BDSP.Instance, EntityContext.Gen8b);
    }
}
