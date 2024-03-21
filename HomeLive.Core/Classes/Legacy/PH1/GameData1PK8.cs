/* Legacy code for PKH Data Version 1.
 * https://github.com/kwsch/PKHeX/blob/be88ec387bd67b96018654ad8c7f13fbf2b02561/PKHeX.Core/PKM/HOME/GameDataPK8.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Side game data for <see cref="PK8"/> data transferred into HOME.
/// </summary>
public sealed class GameData1PK8 : HomeOptional1, IGameDataSide1, IGigantamax, IDynamaxLevel, ISociability
{
    private const HomeGameDataFormat ExpectFormat = HomeGameDataFormat.PK8;
    private const int SIZE = 0x44;
    protected override HomeGameDataFormat Format => ExpectFormat;

    public GameData1PK8() : base(SIZE) { }
    public GameData1PK8(Memory<byte> buffer) : base(buffer) => EnsureSize(SIZE);
    public GameData1PK8 Clone() => new(ToArray());
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
    public ushort EggLocation { get => ReadUInt16LittleEndian(Data[0x40..]); set => WriteUInt16LittleEndian(Data[0x40..], (ushort)value); }
    public ushort MetLocation { get => ReadUInt16LittleEndian(Data[0x42..]); set => WriteUInt16LittleEndian(Data[0x42..], (ushort)value); }

    #endregion

    #region Conversion

    public PersonalInfo GetPersonalInfo(ushort species, byte form) => PersonalTable.SWSH.GetFormEntry(species, form);

    public void CopyTo(PK8 pk)
    {
        ((IGameDataSide1)this).CopyTo(pk);
        pk.CanGigantamax = CanGigantamax;
        pk.Sociability = Sociability;
        pk.DynamaxLevel = DynamaxLevel;
        pk.Fullness = Fullness;
        pk.Palma = Palma;
        PokeJob.CopyTo(pk.PokeJob);
        RecordFlags.CopyTo(pk.RecordFlags);
    }

    public PKM ConvertToPKM(PH1 pkh) => ConvertToPK8(pkh);

    public PK8 ConvertToPK8(PH1 pkh)
    {
        var pk = new PK8();
        pkh.CopyTo(pk);
        CopyTo(pk);
        return pk;
    }

    #endregion

    /// <summary> Reconstructive logic to best apply suggested values. </summary>
    public static GameData1PK8? TryCreate(PH1 pkh)
    {
        if (pkh.DataPB7 is { } x)
            return GameData1PB7.Create<GameData1PK8>(x);

        var side = pkh.DataPB8 as IGameDataSide1
                ?? pkh.DataPA8 as IGameDataSide1;
        if (side is not null)
            return Create(side, pkh.Version);

        return null;
    }

    private static GameData1PK8 Create(IGameDataSide1 side, GameVersion ver)
    {
        var met = side.MetLocation;
        var ball = GetBall(side.Ball);
        var egg = GetEggLocation(side.EggLocation);
        if (!IsOriginallySWSH(ver, met))
            RemapMetEgg(ver, ref met, ref egg);
        return new GameData1PK8 { Ball = ball, MetLocation = met, EggLocation = egg };
    }

    private static void RemapMetEgg(GameVersion ver, ref ushort met, ref ushort egg)
    {
        var remap = LocationsHOME.GetMetSWSH(met, ver);
        if (remap == met)
            return;

        met = remap;
        egg = LocationsHOME.SWSHEgg;
    }

    private static bool IsOriginallySWSH(GameVersion ver, int loc) => ver is GameVersion.SW or GameVersion.SH && !IsFakeMetLocation(loc);
    private static bool IsFakeMetLocation(int met) => met is LocationsHOME.SWLA or LocationsHOME.SWBD or LocationsHOME.SHSP;
    private static byte GetBall(byte ball) => ball > (byte)PKHeX.Core.Ball.Beast ? (byte)4 : ball;
    private static ushort GetEggLocation(ushort egg) => egg == Locations.Default8bNone ? (ushort)0 : egg;
}
