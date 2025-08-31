/* Legacy code for PKH Data Version 1.
 * https://github.com/kwsch/PKHeX/blob/be88ec387bd67b96018654ad8c7f13fbf2b02561/PKHeX.Core/PKM/HOME/GameDataPB7.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data for <see cref="PB7"/> data transferred into HOME.
/// </summary>
public sealed class GameData1PB7 : HomeOptional1, IGameDataSide1, IScaledSizeAbsolute, IMemoryOT
{
    private const HomeGameDataFormat ExpectFormat = HomeGameDataFormat.PB7;
    private const int SIZE = 0x3B;
    protected override HomeGameDataFormat Format => ExpectFormat;

    public GameData1PB7() : base(SIZE) { }
    public GameData1PB7(Memory<byte> data) : base(data) => EnsureSize(SIZE);
    public GameData1PB7 Clone() => new(ToArray());
    public int WriteTo(Span<byte> result) => WriteWithHeader(result);

    #region Structure

    public byte AV_HP  { get => Data[0x00]; set => Data[0x00] = value; }
    public byte AV_ATK { get => Data[0x01]; set => Data[0x01] = value; }
    public byte AV_DEF { get => Data[0x02]; set => Data[0x02] = value; }
    public byte AV_SPE { get => Data[0x03]; set => Data[0x03] = value; }
    public byte AV_SPA { get => Data[0x04]; set => Data[0x04] = value; }
    public byte AV_SPD { get => Data[0x05]; set => Data[0x05] = value; }
    public byte ResortEventState { get => Data[0x06]; set => Data[0x06] = value; }

    public ushort Move1 { get => ReadUInt16LittleEndian(Data[0x07..]); set => WriteUInt16LittleEndian(Data[0x07..], value); }
    public ushort Move2 { get => ReadUInt16LittleEndian(Data[0x09..]); set => WriteUInt16LittleEndian(Data[0x09..], value); }
    public ushort Move3 { get => ReadUInt16LittleEndian(Data[0x0B..]); set => WriteUInt16LittleEndian(Data[0x0B..], value); }
    public ushort Move4 { get => ReadUInt16LittleEndian(Data[0x0D..]); set => WriteUInt16LittleEndian(Data[0x0D..], value); }

    public int Move1_PP { get => Data[0x0F]; set => Data[0x0F] = (byte)value; }
    public int Move2_PP { get => Data[0x10]; set => Data[0x10] = (byte)value; }
    public int Move3_PP { get => Data[0x11]; set => Data[0x11] = (byte)value; }
    public int Move4_PP { get => Data[0x12]; set => Data[0x12] = (byte)value; }
    public int Move1_PPUps { get => Data[0x13]; set => Data[0x13] = (byte)value; }
    public int Move2_PPUps { get => Data[0x14]; set => Data[0x14] = (byte)value; }
    public int Move3_PPUps { get => Data[0x15]; set => Data[0x15] = (byte)value; }
    public int Move4_PPUps { get => Data[0x16]; set => Data[0x16] = (byte)value; }

    public ushort RelearnMove1  { get => ReadUInt16LittleEndian(Data[0x17..]); set => WriteUInt16LittleEndian(Data[0x17..], value); }
    public ushort RelearnMove2  { get => ReadUInt16LittleEndian(Data[0x19..]); set => WriteUInt16LittleEndian(Data[0x19..], value); }
    public ushort RelearnMove3  { get => ReadUInt16LittleEndian(Data[0x1B..]); set => WriteUInt16LittleEndian(Data[0x1B..], value); }
    public ushort RelearnMove4  { get => ReadUInt16LittleEndian(Data[0x1D..]); set => WriteUInt16LittleEndian(Data[0x1D..], value); }
    public float HeightAbsolute { get => ReadSingleLittleEndian(Data[0x1F..]); set => WriteSingleLittleEndian(Data[0x1F..], value); }
    public float WeightAbsolute { get => ReadSingleLittleEndian(Data[0x23..]); set => WriteSingleLittleEndian(Data[0x23..], value); }

    public byte Spirit { get => Data[0x27]; set => Data[0x27] = value; }
    public byte Mood { get => Data[0x28]; set => Data[0x28] = value; }
    public byte Fullness { get => Data[0x29]; set => Data[0x29] = value; }
    public byte Rank { get => Data[0x2A]; set => Data[0x2A] = value; }
    public int OriginalTrainerAffection { get => Data[0x2B]; set => Data[0x2B] = (byte)value; }
    public byte OriginalTrainerMemoryIntensity { get => Data[0x2C]; set => Data[0x2C] = value; }
    public byte OriginalTrainerMemory { get => Data[0x2D]; set => Data[0x2D] = value; }
    public ushort OriginalTrainerMemoryVariable { get => ReadUInt16LittleEndian(Data[0x2E..]); set => WriteUInt16LittleEndian(Data[0x2E..], value); }
    public byte OriginalTrainerMemoryFeeling { get => Data[0x30]; set => Data[0x30] = value; }
    public byte Enjoyment { get => Data[0x31]; set => Data[0x31] = value; }
    public uint GeoPadding { get => ReadUInt32LittleEndian(Data[0x32..]); set => WriteUInt32LittleEndian(Data[0x32..], value); }
    public byte Ball { get => Data[0x36]; set => Data[0x36] = value; }
    public ushort EggLocation { get => ReadUInt16LittleEndian(Data[0x37..]); set => WriteUInt16LittleEndian(Data[0x37..], value); }
    public ushort MetLocation { get => ReadUInt16LittleEndian(Data[0x39..]); set => WriteUInt16LittleEndian(Data[0x39..], value); }

    #endregion

    #region Conversion

    public PersonalInfo GetPersonalInfo(ushort species, byte form) => PersonalTable.GG.GetFormEntry(species, form);

    public void CopyTo(PB7 pk)
    {
        ((IGameDataSide1)this).CopyTo(pk);
        pk.AV_HP = AV_HP;
        pk.AV_ATK = AV_ATK;
        pk.AV_DEF = AV_DEF;
        pk.AV_SPE = AV_SPE;
        pk.AV_SPA = AV_SPA;
        pk.AV_SPD = AV_SPD;
        pk.ResortEventStatus = (ResortEventState)ResortEventState;
        pk.HeightAbsolute = pk.CalcHeightAbsolute; // Ignore the stored value, be nice and recalculate for the user.
        pk.WeightAbsolute = pk.CalcWeightAbsolute; // Ignore the stored value, be nice and recalculate for the user.

        // Some fields are unused as PB7, don't bother copying.
        pk.Spirit = Spirit;
        pk.Mood = Mood;
        pk.Fullness = Fullness;
        // pk.Rank = Rank;
        // pk.OT_Affection
        // pk.OT_Intensity
        // pk.OT_Memory
        // pk.OT_TextVar
        // pk.OT_Feeling
        pk.Enjoyment = Enjoyment;
        // pk.GeoPadding = GeoPadding;
    }

    public PKM ConvertToPKM(PH1 pkh) => ConvertToPB7(pkh);

    public PB7 ConvertToPB7(PH1 pkh)
    {
        var pk = new PB7();
        pkh.CopyTo(pk);
        CopyTo(pk);
        return pk;
    }

    #endregion

    /// <summary> Reconstructive logic to best apply suggested values. </summary>
    public static GameData1PB7? TryCreate(PH1 pkh)
    {
        ushort met = 0;
        byte ball = (int)PKHeX.Core.Ball.Poke;
        if (pkh.DataPK8 is { } x)
        {
            met = x.MetLocation;
            ball = x.Ball;
        }
        else if (pkh.DataPB8 is { } y)
        {
            met = y.MetLocation;
            ball = y.Ball;
        }
        else if (pkh.DataPA8 is { } z)
        {
            met = z.MetLocation;
            ball = z.Ball;
        }
        if (met == 0)
            return null;

        if (pkh.Version is GameVersion.GO)
            return new GameData1PB7 { Ball = ball, MetLocation = Locations.GO7 };
        if (pkh.Version is GameVersion.GP or GameVersion.GE)
            return new GameData1PB7 { Ball = ball, MetLocation = met };

        return null;
    }

    public static T Create<T>(GameData1PB7 data) where T : IGameDataSide1, new() => new()
    {
        Ball = data.Ball,
        MetLocation = data.MetLocation,
        EggLocation = data.EggLocation,
    };
}
