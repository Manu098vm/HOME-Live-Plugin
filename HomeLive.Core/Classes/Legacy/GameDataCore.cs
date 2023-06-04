/*Legacy code for PKH Data Version 1.
 * https://github.com/kwsch/PKHeX/blob/be88ec387bd67b96018654ad8c7f13fbf2b02561/PKHeX.Core/PKM/HOME/GameDataCore.cs
 * GPL v3 License
 * I claim no ownership of this code. Thanks to all the PKHeX contributors.*/

using PKHeX.Core;
using System.Numerics;
using static System.Buffers.Binary.BinaryPrimitives;

namespace HomeLive.Core.Legacy;

/// <summary>
/// Core game data storage, format 1.
/// </summary>
public sealed class GameDataCore : IHomeTrack, ISpeciesForm, ITrainerID, INature, IFatefulEncounter, IContestStats, IScaledSize, ITrainerMemories, IHandlerLanguage, IBattleVersion, IHyperTrain, IFormArgument, IFavorite,
    IRibbonSetEvent3, IRibbonSetEvent4, IRibbonSetCommon3, IRibbonSetCommon4, IRibbonSetCommon6, IRibbonSetMemory6, IRibbonSetCommon7,
    IRibbonSetCommon8, IRibbonSetMark8,
    IRibbonSetCommon9, IRibbonSetMark9
{
    // Internal Attributes set on creation
    private readonly Memory<byte> Buffer; // Raw Storage
    private Span<byte> Data => Buffer.Span;

    public GameDataCore(Memory<byte> buffer)
    {
        if (buffer.Length != HomeCrypto.SIZE_1CORE)
            throw new ArgumentException("Invalid Format 1 Core Data!");

        Buffer = buffer;
    }

    public int WriteTo(Span<byte> result)
    {
        var span = Data;
        span.CopyTo(result);
        return span.Length;
    }

    public ulong Tracker { get => ReadUInt64LittleEndian(Data); set => WriteUInt64LittleEndian(Data, value); }
    public uint EncryptionConstant { get => ReadUInt32LittleEndian(Data[0x08..]); set => WriteUInt32LittleEndian(Data[0x08..], value); }
    public bool IsBadEgg { get => Data[0x0C] != 0; set => Data[0x0C] = (byte)(value ? 1 : 0); }
    public ushort Species { get => ReadUInt16LittleEndian(Data[0x0D..]); set => WriteUInt16LittleEndian(Data[0x0D..], value); }
    public uint ID32 { get => ReadUInt32LittleEndian(Data[0x0F..]); set => WriteUInt32LittleEndian(Data[0x0F..], value); }
    public ushort TID16 { get => ReadUInt16LittleEndian(Data[0x0F..]); set => WriteUInt16LittleEndian(Data[0x0F..], value); }
    public ushort SID16 { get => ReadUInt16LittleEndian(Data[0x11..]); set => WriteUInt16LittleEndian(Data[0x11..], value); }
    public uint EXP { get => ReadUInt32LittleEndian(Data[0x13..]); set => WriteUInt32LittleEndian(Data[0x13..], value); }
    public int Ability { get => ReadUInt16LittleEndian(Data[0x17..]); set => WriteUInt16LittleEndian(Data[0x17..], (ushort)value); }
    public int AbilityNumber { get => Data[0x19] & 7; set => Data[0x19] = (byte)((Data[0x19] & ~7) | (value & 7)); }
    public bool IsFavorite { get => Data[0x1A] != 0; set => Data[0x1A] = (byte)(value ? 1 : 0); }
    public int MarkValue { get => ReadUInt16LittleEndian(Data[0x1B..]); set => WriteUInt16LittleEndian(Data[0x1B..], (ushort)value); }
    public uint PID { get => ReadUInt32LittleEndian(Data[0x1D..]); set => WriteUInt32LittleEndian(Data[0x1D..], value); }
    public int Nature { get => Data[0x21]; set => Data[0x21] = (byte)value; }
    public int StatNature { get => Data[0x22]; set => Data[0x22] = (byte)value; }
    public bool FatefulEncounter { get => Data[0x23] != 0; set => Data[0x23] = (byte)(value ? 1 : 0); }
    public int Gender { get => Data[0x24]; set => Data[0x24] = (byte)value; }
    public byte Form { get => Data[0x25]; set => WriteUInt16LittleEndian(Data[0x25..], value); }
    public int EV_HP { get => Data[0x27]; set => Data[0x27] = (byte)value; }
    public int EV_ATK { get => Data[0x28]; set => Data[0x28] = (byte)value; }
    public int EV_DEF { get => Data[0x29]; set => Data[0x29] = (byte)value; }
    public int EV_SPE { get => Data[0x2A]; set => Data[0x2A] = (byte)value; }
    public int EV_SPA { get => Data[0x2B]; set => Data[0x2B] = (byte)value; }
    public int EV_SPD { get => Data[0x2C]; set => Data[0x2C] = (byte)value; }
    public byte CNT_Cool { get => Data[0x2D]; set => Data[0x2D] = value; }
    public byte CNT_Beauty { get => Data[0x2E]; set => Data[0x2E] = value; }
    public byte CNT_Cute { get => Data[0x2F]; set => Data[0x2F] = value; }
    public byte CNT_Smart { get => Data[0x30]; set => Data[0x30] = value; }
    public byte CNT_Tough { get => Data[0x31]; set => Data[0x31] = value; }
    public byte CNT_Sheen { get => Data[0x32]; set => Data[0x32] = value; }
    private byte PKRS { get => Data[0x33]; set => Data[0x33] = value; }
    public int PKRS_Days { get => PKRS & 0xF; set => PKRS = (byte)((PKRS & ~0xF) | value); }
    public int PKRS_Strain { get => PKRS >> 4; set => PKRS = (byte)((PKRS & 0xF) | (value << 4)); }

    private bool GetFlag(int offset, int bit) => FlagUtil.GetFlag(Data, offset, bit);
    private void SetFlag(int offset, int bit, bool value) => FlagUtil.SetFlag(Data, offset, bit, value);

    public bool RibbonChampionKalos   { get => GetFlag(0x34, 0); set => SetFlag(0x34, 0, value); }
    public bool RibbonChampionG3      { get => GetFlag(0x34, 1); set => SetFlag(0x34, 1, value); }
    public bool RibbonChampionSinnoh  { get => GetFlag(0x34, 2); set => SetFlag(0x34, 2, value); }
    public bool RibbonBestFriends     { get => GetFlag(0x34, 3); set => SetFlag(0x34, 3, value); }
    public bool RibbonTraining        { get => GetFlag(0x34, 4); set => SetFlag(0x34, 4, value); }
    public bool RibbonBattlerSkillful { get => GetFlag(0x34, 5); set => SetFlag(0x34, 5, value); }
    public bool RibbonBattlerExpert   { get => GetFlag(0x34, 6); set => SetFlag(0x34, 6, value); }
    public bool RibbonEffort          { get => GetFlag(0x34, 7); set => SetFlag(0x34, 7, value); }

    public bool RibbonAlert    { get => GetFlag(0x35, 0); set => SetFlag(0x35, 0, value); }
    public bool RibbonShock    { get => GetFlag(0x35, 1); set => SetFlag(0x35, 1, value); }
    public bool RibbonDowncast { get => GetFlag(0x35, 2); set => SetFlag(0x35, 2, value); }
    public bool RibbonCareless { get => GetFlag(0x35, 3); set => SetFlag(0x35, 3, value); }
    public bool RibbonRelax    { get => GetFlag(0x35, 4); set => SetFlag(0x35, 4, value); }
    public bool RibbonSnooze   { get => GetFlag(0x35, 5); set => SetFlag(0x35, 5, value); }
    public bool RibbonSmile    { get => GetFlag(0x35, 6); set => SetFlag(0x35, 6, value); }
    public bool RibbonGorgeous { get => GetFlag(0x35, 7); set => SetFlag(0x35, 7, value); }

    public bool RibbonRoyal         { get => GetFlag(0x36, 0); set => SetFlag(0x36, 0, value); }
    public bool RibbonGorgeousRoyal { get => GetFlag(0x36, 1); set => SetFlag(0x36, 1, value); }
    public bool RibbonArtist        { get => GetFlag(0x36, 2); set => SetFlag(0x36, 2, value); }
    public bool RibbonFootprint     { get => GetFlag(0x36, 3); set => SetFlag(0x36, 3, value); }
    public bool RibbonRecord        { get => GetFlag(0x36, 4); set => SetFlag(0x36, 4, value); }
    public bool RibbonLegend        { get => GetFlag(0x36, 5); set => SetFlag(0x36, 5, value); }
    public bool RibbonCountry       { get => GetFlag(0x36, 6); set => SetFlag(0x36, 6, value); }
    public bool RibbonNational      { get => GetFlag(0x36, 7); set => SetFlag(0x36, 7, value); }

    public bool RibbonEarth    { get => GetFlag(0x37, 0); set => SetFlag(0x37, 0, value); }
    public bool RibbonWorld    { get => GetFlag(0x37, 1); set => SetFlag(0x37, 1, value); }
    public bool RibbonClassic  { get => GetFlag(0x37, 2); set => SetFlag(0x37, 2, value); }
    public bool RibbonPremier  { get => GetFlag(0x37, 3); set => SetFlag(0x37, 3, value); }
    public bool RibbonEvent    { get => GetFlag(0x37, 4); set => SetFlag(0x37, 4, value); }
    public bool RibbonBirthday { get => GetFlag(0x37, 5); set => SetFlag(0x37, 5, value); }
    public bool RibbonSpecial  { get => GetFlag(0x37, 6); set => SetFlag(0x37, 6, value); }
    public bool RibbonSouvenir { get => GetFlag(0x37, 7); set => SetFlag(0x37, 7, value); }

    // ribbon u32
    public bool RibbonWishing          { get => GetFlag(0x38, 0); set => SetFlag(0x38, 0, value); }
    public bool RibbonChampionBattle   { get => GetFlag(0x38, 1); set => SetFlag(0x38, 1, value); }
    public bool RibbonChampionRegional { get => GetFlag(0x38, 2); set => SetFlag(0x38, 2, value); }
    public bool RibbonChampionNational { get => GetFlag(0x38, 3); set => SetFlag(0x38, 3, value); }
    public bool RibbonChampionWorld    { get => GetFlag(0x38, 4); set => SetFlag(0x38, 4, value); }
    public bool HasContestMemoryRibbon { get => GetFlag(0x38, 5); set => SetFlag(0x38, 5, value); }
    public bool HasBattleMemoryRibbon  { get => GetFlag(0x38, 6); set => SetFlag(0x38, 6, value); }
    public bool RibbonChampionG6Hoenn  { get => GetFlag(0x38, 7); set => SetFlag(0x38, 7, value); }

    public bool RibbonContestStar      { get => GetFlag(0x39, 0); set => SetFlag(0x39, 0, value); }
    public bool RibbonMasterCoolness   { get => GetFlag(0x39, 1); set => SetFlag(0x39, 1, value); }
    public bool RibbonMasterBeauty     { get => GetFlag(0x39, 2); set => SetFlag(0x39, 2, value); }
    public bool RibbonMasterCuteness   { get => GetFlag(0x39, 3); set => SetFlag(0x39, 3, value); }
    public bool RibbonMasterCleverness { get => GetFlag(0x39, 4); set => SetFlag(0x39, 4, value); }
    public bool RibbonMasterToughness  { get => GetFlag(0x39, 5); set => SetFlag(0x39, 5, value); }
    public bool RibbonChampionAlola    { get => GetFlag(0x39, 6); set => SetFlag(0x39, 6, value); }
    public bool RibbonBattleRoyale     { get => GetFlag(0x39, 7); set => SetFlag(0x39, 7, value); }

    public bool RibbonBattleTreeGreat  { get => GetFlag(0x3A, 0); set => SetFlag(0x3A, 0, value); }
    public bool RibbonBattleTreeMaster { get => GetFlag(0x3A, 1); set => SetFlag(0x3A, 1, value); }
    public bool RibbonChampionGalar    { get => GetFlag(0x3A, 2); set => SetFlag(0x3A, 2, value); }
    public bool RibbonTowerMaster      { get => GetFlag(0x3A, 3); set => SetFlag(0x3A, 3, value); }
    public bool RibbonMasterRank       { get => GetFlag(0x3A, 4); set => SetFlag(0x3A, 4, value); }
    public bool RibbonMarkLunchtime    { get => GetFlag(0x3A, 5); set => SetFlag(0x3A, 5, value); }
    public bool RibbonMarkSleepyTime   { get => GetFlag(0x3A, 6); set => SetFlag(0x3A, 6, value); }
    public bool RibbonMarkDusk         { get => GetFlag(0x3A, 7); set => SetFlag(0x3A, 7, value); }

    public bool RibbonMarkDawn         { get => GetFlag(0x3B, 0); set => SetFlag(0x3B, 0, value); }
    public bool RibbonMarkCloudy       { get => GetFlag(0x3B, 1); set => SetFlag(0x3B, 1, value); }
    public bool RibbonMarkRainy        { get => GetFlag(0x3B, 2); set => SetFlag(0x3B, 2, value); }
    public bool RibbonMarkStormy       { get => GetFlag(0x3B, 3); set => SetFlag(0x3B, 3, value); }
    public bool RibbonMarkSnowy        { get => GetFlag(0x3B, 4); set => SetFlag(0x3B, 4, value); }
    public bool RibbonMarkBlizzard     { get => GetFlag(0x3B, 5); set => SetFlag(0x3B, 5, value); }
    public bool RibbonMarkDry          { get => GetFlag(0x3B, 6); set => SetFlag(0x3B, 6, value); }
    public bool RibbonMarkSandstorm    { get => GetFlag(0x3B, 7); set => SetFlag(0x3B, 7, value); }

    public byte RibbonCountMemoryContest { get => Data[0x3C]; set => HasContestMemoryRibbon = (Data[0x3C] = value) != 0; }
    public byte RibbonCountMemoryBattle  { get => Data[0x3D]; set => HasBattleMemoryRibbon  = (Data[0x3D] = value) != 0; }
    // !!! no padding, unlike PKM formats!

    // 0x3E Ribbon 3
    public bool RibbonMarkMisty        { get => GetFlag(0x3E, 0); set => SetFlag(0x3E, 0, value); }
    public bool RibbonMarkDestiny      { get => GetFlag(0x3E, 1); set => SetFlag(0x3E, 1, value); }
    public bool RibbonMarkFishing      { get => GetFlag(0x3E, 2); set => SetFlag(0x3E, 2, value); }
    public bool RibbonMarkCurry        { get => GetFlag(0x3E, 3); set => SetFlag(0x3E, 3, value); }
    public bool RibbonMarkUncommon     { get => GetFlag(0x3E, 4); set => SetFlag(0x3E, 4, value); }
    public bool RibbonMarkRare         { get => GetFlag(0x3E, 5); set => SetFlag(0x3E, 5, value); }
    public bool RibbonMarkRowdy        { get => GetFlag(0x3E, 6); set => SetFlag(0x3E, 6, value); }
    public bool RibbonMarkAbsentMinded { get => GetFlag(0x3E, 7); set => SetFlag(0x3E, 7, value); }

    public bool RibbonMarkJittery     { get => GetFlag(0x3F, 0); set => SetFlag(0x3F, 0, value); }
    public bool RibbonMarkExcited     { get => GetFlag(0x3F, 1); set => SetFlag(0x3F, 1, value); }
    public bool RibbonMarkCharismatic { get => GetFlag(0x3F, 2); set => SetFlag(0x3F, 2, value); }
    public bool RibbonMarkCalmness    { get => GetFlag(0x3F, 3); set => SetFlag(0x3F, 3, value); }
    public bool RibbonMarkIntense     { get => GetFlag(0x3F, 4); set => SetFlag(0x3F, 4, value); }
    public bool RibbonMarkZonedOut    { get => GetFlag(0x3F, 5); set => SetFlag(0x3F, 5, value); }
    public bool RibbonMarkJoyful      { get => GetFlag(0x3F, 6); set => SetFlag(0x3F, 6, value); }
    public bool RibbonMarkAngry       { get => GetFlag(0x3F, 7); set => SetFlag(0x3F, 7, value); }

    public bool RibbonMarkSmiley       { get => GetFlag(0x40, 0); set => SetFlag(0x40, 0, value); }
    public bool RibbonMarkTeary        { get => GetFlag(0x40, 1); set => SetFlag(0x40, 1, value); }
    public bool RibbonMarkUpbeat       { get => GetFlag(0x40, 2); set => SetFlag(0x40, 2, value); }
    public bool RibbonMarkPeeved       { get => GetFlag(0x40, 3); set => SetFlag(0x40, 3, value); }
    public bool RibbonMarkIntellectual { get => GetFlag(0x40, 4); set => SetFlag(0x40, 4, value); }
    public bool RibbonMarkFerocious    { get => GetFlag(0x40, 5); set => SetFlag(0x40, 5, value); }
    public bool RibbonMarkCrafty       { get => GetFlag(0x40, 6); set => SetFlag(0x40, 6, value); }
    public bool RibbonMarkScowling     { get => GetFlag(0x40, 7); set => SetFlag(0x40, 7, value); }

    public bool RibbonMarkKindly       { get => GetFlag(0x41, 0); set => SetFlag(0x41, 0, value); }
    public bool RibbonMarkFlustered    { get => GetFlag(0x41, 1); set => SetFlag(0x41, 1, value); }
    public bool RibbonMarkPumpedUp     { get => GetFlag(0x41, 2); set => SetFlag(0x41, 2, value); }
    public bool RibbonMarkZeroEnergy   { get => GetFlag(0x41, 3); set => SetFlag(0x41, 3, value); }
    public bool RibbonMarkPrideful     { get => GetFlag(0x41, 4); set => SetFlag(0x41, 4, value); }
    public bool RibbonMarkUnsure       { get => GetFlag(0x41, 5); set => SetFlag(0x41, 5, value); }
    public bool RibbonMarkHumble       { get => GetFlag(0x41, 6); set => SetFlag(0x41, 6, value); }
    public bool RibbonMarkThorny       { get => GetFlag(0x41, 7); set => SetFlag(0x41, 7, value); }

    public bool RibbonMarkVigor        { get => GetFlag(0x42, 0); set => SetFlag(0x42, 0, value); }
    public bool RibbonMarkSlump        { get => GetFlag(0x42, 1); set => SetFlag(0x42, 1, value); }
    public bool RibbonHisui            { get => GetFlag(0x42, 2); set => SetFlag(0x42, 2, value); }
    public bool RibbonTwinklingStar    { get => GetFlag(0x42, 3); set => SetFlag(0x42, 3, value); }
    public bool RibbonChampionPaldea   { get => GetFlag(0x42, 4); set => SetFlag(0x42, 4, value); }
    public bool RibbonMarkJumbo        { get => GetFlag(0x42, 5); set => SetFlag(0x42, 5, value); }
    public bool RibbonMarkMini         { get => GetFlag(0x42, 6); set => SetFlag(0x42, 6, value); }
    public bool RibbonMarkItemfinder   { get => GetFlag(0x42, 7); set => SetFlag(0x42, 7, value); }

    public bool RibbonMarkPartner      { get => GetFlag(0x43, 0); set => SetFlag(0x43, 0, value); }
    public bool RibbonMarkGourmand     { get => GetFlag(0x43, 1); set => SetFlag(0x43, 1, value); }
    public bool RibbonOnceInALifetime  { get => GetFlag(0x43, 2); set => SetFlag(0x43, 2, value); }
    public bool RibbonMarkAlpha        { get => GetFlag(0x43, 3); set => SetFlag(0x43, 3, value); }
    public bool RibbonMarkMightiest    { get => GetFlag(0x43, 4); set => SetFlag(0x43, 4, value); }
    public bool RibbonMarkTitan        { get => GetFlag(0x43, 5); set => SetFlag(0x43, 5, value); }
    public bool RIB45_6                { get => GetFlag(0x43, 6); set => SetFlag(0x43, 6, value); }
    public bool RIB45_7                { get => GetFlag(0x43, 7); set => SetFlag(0x43, 7, value); }

    public bool RIB46_0                { get => GetFlag(0x44, 0); set => SetFlag(0x44, 0, value); }
    public bool RIB46_1                { get => GetFlag(0x44, 1); set => SetFlag(0x44, 1, value); }
    public bool RIB46_2                { get => GetFlag(0x44, 2); set => SetFlag(0x44, 2, value); }
    public bool RIB46_3                { get => GetFlag(0x44, 3); set => SetFlag(0x44, 3, value); }
    public bool RIB46_4                { get => GetFlag(0x44, 4); set => SetFlag(0x44, 4, value); }
    public bool RIB46_5                { get => GetFlag(0x44, 5); set => SetFlag(0x44, 5, value); }
    public bool RIB46_6                { get => GetFlag(0x44, 6); set => SetFlag(0x44, 6, value); }
    public bool RIB46_7                { get => GetFlag(0x44, 7); set => SetFlag(0x44, 7, value); }

    public bool RIB47_0                { get => GetFlag(0x45, 0); set => SetFlag(0x45, 0, value); }
    public bool RIB47_1                { get => GetFlag(0x45, 1); set => SetFlag(0x45, 1, value); }
    public bool RIB47_2                { get => GetFlag(0x45, 2); set => SetFlag(0x45, 2, value); }
    public bool RIB47_3                { get => GetFlag(0x45, 3); set => SetFlag(0x45, 3, value); }
    public bool RIB47_4                { get => GetFlag(0x45, 4); set => SetFlag(0x45, 4, value); }
    public bool RIB47_5                { get => GetFlag(0x45, 5); set => SetFlag(0x45, 5, value); }
    public bool RIB47_6                { get => GetFlag(0x45, 6); set => SetFlag(0x45, 6, value); }
    public bool RIB47_7                { get => GetFlag(0x45, 7); set => SetFlag(0x45, 7, value); }

    public int RibbonCount     => BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x34..]) & 0b00000000_00011111__11111111_11111111__11111111_11111111__11111111_11111111)
                                + BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x3E..]) & 0b00000000_00000000__00000100_00011100__00000000_00000000__00000000_00000000);
    public int MarkCount       => BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x34..]) & 0b11111111_11100000__00000000_00000000__00000000_00000000__00000000_00000000)
                                + BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x3E..]) & 0b00000000_00000000__00111011_11100011__11111111_11111111__11111111_11111111);
    public int RibbonMarkCount => BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x34..]) & 0b11111111_11111111__11111111_11111111__11111111_11111111__11111111_11111111)
                                + BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x3E..]) & 0b00000000_00000000__00111111_11111111__11111111_11111111__11111111_11111111);

    public bool HasMarkEncounter8 => BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x34..]) & 0b11111111_11100000__00000000_00000000__00000000_00000000__00000000_00000000)
                                   + BitOperations.PopCount(ReadUInt64LittleEndian(Data[0x3E..]) & 0b00000000_00000000__00000000_00000011__11111111_11111111__11111111_11111111) != 0;
    public bool HasMarkEncounter9 => (Data[0x43] & 0b00111000) != 0;

    public byte HeightScalar { get => Data[0x46]; set => Data[0x46] = value; }
    public byte WeightScalar { get => Data[0x47]; set => Data[0x47] = value; }

    public Span<byte> Nickname_Trash => Data.Slice(0x48, 26);

    public string Nickname
    {
        get => StringConverter8.GetString(Nickname_Trash);
        set => StringConverter8.SetString(Nickname_Trash, value, 12, StringConverterOption.None);
    }

    public int Stat_HPCurrent { get => ReadUInt16LittleEndian(Data[0x62..]); set => WriteUInt16LittleEndian(Data[0x62..], (ushort)value); }
    public int IV_HP  { get => Data[0x64]; set => Data[0x64] = (byte)value; }
    public int IV_ATK { get => Data[0x65]; set => Data[0x65] = (byte)value; }
    public int IV_DEF { get => Data[0x66]; set => Data[0x66] = (byte)value; }
    public int IV_SPE { get => Data[0x67]; set => Data[0x67] = (byte)value; }
    public int IV_SPA { get => Data[0x68]; set => Data[0x68] = (byte)value; }
    public int IV_SPD { get => Data[0x69]; set => Data[0x69] = (byte)value; }
    public bool IsEgg { get => Data[0x6A] != 0; set => Data[0x6A] = (byte)(value ? 1 : 0); }
    public bool IsNicknamed { get => Data[0x6B] != 0; set => Data[0x6B] = (byte)(value ? 1 : 0); }
    public int Status_Condition { get => ReadInt32LittleEndian(Data[0x6C..]); set => WriteInt32LittleEndian(Data[0x6C..], value); }
    public Span<byte> HT_Trash => Data.Slice(0x70, 26);
    public string HT_Name
    {
        get => StringConverter8.GetString(HT_Trash);
        set => StringConverter8.SetString(HT_Trash, value, 12, StringConverterOption.None);
    }
    public int HT_Gender      { get => Data[0x8A]; set => Data[0x8A] = (byte)value; }
    public byte HT_Language   { get => Data[0x8B]; set => Data[0x8B] = value; }
    public int CurrentHandler { get => Data[0x8C]; set => Data[0x8C] = (byte)value; }
    public int HT_TrainerID   { get => ReadUInt16LittleEndian(Data[0x8D..]); set => WriteUInt16LittleEndian(Data[0x8D..], (ushort)value); } // unused?
    public int HT_Friendship  { get => Data[0x8F]; set => Data[0x8F] = (byte)value; }
    public byte HT_Intensity  { get => Data[0x90]; set => Data[0x90] = value; }
    public byte HT_Memory     { get => Data[0x91]; set => Data[0x91] = value; }
    public byte HT_Feeling    { get => Data[0x92]; set => Data[0x92] = value; }
    public ushort HT_TextVar  { get => ReadUInt16LittleEndian(Data[0x93..]); set => WriteUInt16LittleEndian(Data[0x93..], value); }
    public int Version        { get => Data[0x95]; set => Data[0x95] = (byte)value; }
    public byte BattleVersion { get => Data[0x96]; set => Data[0x96] = value; }
    public int Language       { get => Data[0x97]; set => Data[0x97] = (byte)value; }
    public uint FormArgument        { get => ReadUInt32LittleEndian(Data[0x98..]); set => WriteUInt32LittleEndian(Data[0x98..], value); }
    public byte FormArgumentRemain  { get => (byte)FormArgument; set => FormArgument = (FormArgument & ~0xFFu) | value; }
    public byte FormArgumentElapsed { get => (byte)(FormArgument >> 8); set => FormArgument = (FormArgument & ~0xFF00u) | (uint)(value << 8); }
    public byte FormArgumentMaximum { get => (byte)(FormArgument >> 16); set => FormArgument = (FormArgument & ~0xFF0000u) | (uint)(value << 16); }
    public sbyte AffixedRibbon      { get => (sbyte)Data[0x9C]; set => Data[0x9C] = (byte)value; } // selected ribbon
    public Span<byte> OT_Trash => Data.Slice(0x9D, 26);
    public string OT_Name
    {
        get => StringConverter8.GetString(OT_Trash);
        set => StringConverter8.SetString(OT_Trash, value, 12, StringConverterOption.None);
    }
    public int OT_Friendship    { get => Data[0xB7]; set => Data[0xB7] = (byte)value; }
    public byte OT_Intensity    { get => Data[0xB8]; set => Data[0xB8] = value; }
    public byte OT_Memory       { get => Data[0xB9]; set => Data[0xB9] = value; }
    public ushort OT_TextVar    { get => ReadUInt16LittleEndian(Data[0xBA..]); set => WriteUInt16LittleEndian(Data[0xBA..], value); }
    public byte OT_Feeling      { get => Data[0xBC]; set => Data[0xBC] = value; }
    public int Egg_Year         { get => Data[0xBD]; set => Data[0xBD] = (byte)value; }
    public int Egg_Month        { get => Data[0xBE]; set => Data[0xBE] = (byte)value; }
    public int Egg_Day          { get => Data[0xBF]; set => Data[0xBF] = (byte)value; }
    public int Met_Year         { get => Data[0xC0]; set => Data[0xC0] = (byte)value; }
    public int Met_Month        { get => Data[0xC1]; set => Data[0xC1] = (byte)value; }
    public int Met_Day          { get => Data[0xC2]; set => Data[0xC2] = (byte)value; }
    public int Met_Level        { get => Data[0xC3]; set => Data[0xC3] = (byte)value; }
    public int OT_Gender        { get => Data[0xC4]; set => Data[0xC4] = (byte)value; }
    public byte HyperTrainFlags { get => Data[0xC5]; set => Data[0xC5] = value; }
    public bool HT_HP { get => ((HyperTrainFlags >> 0) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 0)) | ((value ? 1 : 0) << 0)); }
    public bool HT_ATK { get => ((HyperTrainFlags >> 1) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 1)) | ((value ? 1 : 0) << 1)); }
    public bool HT_DEF { get => ((HyperTrainFlags >> 2) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 2)) | ((value ? 1 : 0) << 2)); }
    public bool HT_SPA { get => ((HyperTrainFlags >> 3) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 3)) | ((value ? 1 : 0) << 3)); }
    public bool HT_SPD { get => ((HyperTrainFlags >> 4) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 4)) | ((value ? 1 : 0) << 4)); }
    public bool HT_SPE { get => ((HyperTrainFlags >> 5) & 1) == 1; set => HyperTrainFlags = (byte)((HyperTrainFlags & ~(1 << 5)) | ((value ? 1 : 0) << 5)); }

    public int HeldItem { get => ReadUInt16LittleEndian(Data[0xC6..]); set => WriteUInt16LittleEndian(Data[0xC6..], (ushort)value); }

    public int MarkingCount => 6;

    public TrainerIDFormat TrainerIDDisplayFormat => ((GameVersion)Version).GetGeneration() >= 7 ? TrainerIDFormat.SixDigit : TrainerIDFormat.SixteenBit;

    public int GetMarking(int index)
    {
        if ((uint)index >= MarkingCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        return (MarkValue >> (index * 2)) & 3;
    }

    public void SetMarking(int index, int value)
    {
        if ((uint)index >= MarkingCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        var shift = index * 2;
        MarkValue = (MarkValue & ~(0b11 << shift)) | ((value & 3) << shift);
    }

    public void CopyTo(PKM pk)
    {
        pk.EncryptionConstant = EncryptionConstant;
        pk.PID = PID;
        pk.Species = Species;
        pk.Form = Form;
        pk.Gender = Gender;
        pk.TID16 = TID16;
        pk.SID16 = SID16;
        pk.EXP = EXP;
        pk.Ability = Ability;
        pk.AbilityNumber = AbilityNumber;
        pk.MarkValue = MarkValue;
        pk.Nature = Nature;
        pk.StatNature = StatNature;
        pk.FatefulEncounter = FatefulEncounter;
        pk.HeldItem = HeldItem;
        pk.IV_HP  = IV_HP;
        pk.IV_ATK = IV_ATK;
        pk.IV_DEF = IV_DEF;
        pk.IV_SPE = IV_SPE;
        pk.IV_SPA = IV_SPA;
        pk.IV_SPD = IV_SPD;
        pk.IsEgg = IsEgg;
        pk.IsNicknamed = IsNicknamed;
        pk.EV_HP  = EV_HP;
        pk.EV_ATK = EV_ATK;
        pk.EV_DEF = EV_DEF;
        pk.EV_SPE = EV_SPE;
        pk.EV_SPA = EV_SPA;
        pk.EV_SPD = EV_SPD;
        pk.PKRS_Strain = PKRS_Strain;
        pk.PKRS_Days = PKRS_Days;

        pk.HT_Gender = HT_Gender;
        pk.CurrentHandler = CurrentHandler;
        // pk.HT_TrainerID
        pk.HT_Friendship = HT_Friendship;

        pk.Version = Version;
        pk.Language = Language;

        pk.OT_Friendship = OT_Friendship;
        pk.Egg_Year = Egg_Year;
        pk.Egg_Month = Egg_Month;
        pk.Egg_Day = Egg_Day;
        pk.Met_Year = Met_Year;
        pk.Met_Month = Met_Month;
        pk.Met_Day = Met_Day;
        pk.Met_Level = Met_Level;
        pk.OT_Gender = OT_Gender;

        CopyConditionalInterface(pk);

        OT_Trash.CopyTo(pk.OT_Trash);
        Nickname_Trash.CopyTo(pk.Nickname_Trash);
        HT_Trash.CopyTo(pk.HT_Trash);

        CopyConditionalRibbonMark(pk);
    }

    private void CopyConditionalInterface(PKM pk)
    {
        if (pk is IScaledSize ss)
        {
            ss.HeightScalar = HeightScalar;
            ss.WeightScalar = WeightScalar;
        }

        if (pk is IMemoryOT ot)
        {
            ot.OT_Intensity = OT_Intensity;
            ot.OT_Memory = OT_Memory;
            ot.OT_TextVar = OT_TextVar;
            ot.OT_Feeling = OT_Feeling;
        }
        if (pk is IMemoryHT hm)
        {
            hm.HT_Intensity = HT_Intensity;
            hm.HT_Memory = HT_Memory;
            hm.HT_Feeling = HT_Feeling;
            hm.HT_TextVar = HT_TextVar;
        }
        if (pk is IHandlerLanguage hl)
            hl.HT_Language = HT_Language;

        if (pk is IContestStats cm)
            this.CopyContestStatsTo(cm);
        if (pk is IRibbonSetAffixed affix)
            affix.AffixedRibbon = AffixedRibbon;
        if (pk is IHyperTrain ht)
            ht.HyperTrainFlags = HyperTrainFlags;
        if (pk is IFormArgument fa)
            fa.FormArgument = FormArgument;
        if (pk is IBattleVersion bv)
            bv.BattleVersion = BattleVersion;
        if (pk is IFavorite fav)
            fav.IsFavorite = IsFavorite;
        if (pk is IHomeTrack home)
            home.Tracker = Tracker;
    }

    private void CopyConditionalRibbonMark(PKM pk)
    {
        if (pk is IRibbonSetEvent3 e3)
            CopyRibbonSetEvent3(this, e3);
        if (pk is IRibbonSetEvent4 e4)
            CopyRibbonSetEvent4(this, e4);
        if (pk is IRibbonSetCommon3 c3)
            CopyRibbonSetCommon3(this, c3);
        if (pk is IRibbonSetCommon4 c4)
            CopyRibbonSetCommon4(this, c4);
        if (pk is IRibbonSetCommon6 c6)
            CopyRibbonSetCommon6(this, c6);
        if (pk is IRibbonSetMemory6 m6)
            CopyRibbonSetMemory6(this, m6);
        if (pk is IRibbonSetCommon7 c7)
            CopyRibbonSetCommon7(this, c7);
        if (pk is IRibbonSetCommon8 c8)
            CopyRibbonSetCommon8(this, c8);
        if (pk is IRibbonSetMark8 m8)
            CopyRibbonSetMark8(this, m8);
    }

    internal static void CopyRibbonSetEvent3(IRibbonSetEvent3 set, IRibbonSetEvent3 dest)
    {
        dest.RibbonEarth = set.RibbonEarth;
        dest.RibbonNational = set.RibbonNational;
        dest.RibbonCountry = set.RibbonCountry;
        dest.RibbonChampionBattle = set.RibbonChampionBattle;
        dest.RibbonChampionRegional = set.RibbonChampionRegional;
        dest.RibbonChampionNational = set.RibbonChampionNational;
    }

    internal static void CopyRibbonSetEvent4(IRibbonSetEvent4 set, IRibbonSetEvent4 dest)
    {
        dest.RibbonClassic = set.RibbonClassic;
        dest.RibbonWishing = set.RibbonWishing;
        dest.RibbonPremier = set.RibbonPremier;
        dest.RibbonEvent = set.RibbonEvent;
        dest.RibbonBirthday = set.RibbonBirthday;
        dest.RibbonSpecial = set.RibbonSpecial;
        dest.RibbonWorld = set.RibbonWorld;
        dest.RibbonChampionWorld = set.RibbonChampionWorld;
        dest.RibbonSouvenir = set.RibbonSouvenir;
    }

    internal static void CopyRibbonSetCommon3(IRibbonSetCommon3 set, IRibbonSetCommon3 dest)
    {
        dest.RibbonChampionG3 = set.RibbonChampionG3;
        dest.RibbonArtist = set.RibbonArtist;
        dest.RibbonEffort = set.RibbonEffort;
    }

    internal static void CopyRibbonSetCommon4(IRibbonSetCommon4 set, IRibbonSetCommon4 dest)
    {
        dest.RibbonChampionSinnoh = set.RibbonChampionSinnoh;
        dest.RibbonAlert = set.RibbonAlert;
        dest.RibbonShock = set.RibbonShock;
        dest.RibbonDowncast = set.RibbonDowncast;
        dest.RibbonCareless = set.RibbonCareless;
        dest.RibbonRelax = set.RibbonRelax;
        dest.RibbonSnooze = set.RibbonSnooze;
        dest.RibbonSmile = set.RibbonSmile;
        dest.RibbonGorgeous = set.RibbonGorgeous;
        dest.RibbonRoyal = set.RibbonRoyal;
        dest.RibbonGorgeousRoyal = set.RibbonGorgeousRoyal;
        dest.RibbonFootprint = set.RibbonFootprint;
        dest.RibbonRecord = set.RibbonRecord;
        dest.RibbonLegend = set.RibbonLegend;
    }

    internal static void CopyRibbonSetCommon6(IRibbonSetCommon6 set, IRibbonSetCommon6 dest)
    {
        dest.RibbonChampionKalos = set.RibbonChampionKalos;
        dest.RibbonChampionG6Hoenn = set.RibbonChampionG6Hoenn;
        dest.RibbonBestFriends = set.RibbonBestFriends;
        dest.RibbonTraining = set.RibbonTraining;
        dest.RibbonBattlerSkillful = set.RibbonBattlerSkillful;
        dest.RibbonBattlerExpert = set.RibbonBattlerExpert;
        dest.RibbonContestStar = set.RibbonContestStar;
        dest.RibbonMasterCoolness = set.RibbonMasterCoolness;
        dest.RibbonMasterBeauty = set.RibbonMasterBeauty;
        dest.RibbonMasterCuteness = set.RibbonMasterCuteness;
        dest.RibbonMasterCleverness = set.RibbonMasterCleverness;
        dest.RibbonMasterToughness = set.RibbonMasterToughness;
    }

    internal static void CopyRibbonSetMemory6(IRibbonSetMemory6 set, IRibbonSetMemory6 dest)
    {
        dest.HasContestMemoryRibbon = set.HasContestMemoryRibbon;
        dest.HasBattleMemoryRibbon = set.HasBattleMemoryRibbon;
        dest.RibbonCountMemoryContest = set.RibbonCountMemoryContest;
        dest.RibbonCountMemoryBattle = set.RibbonCountMemoryBattle;
    }

    internal static void CopyRibbonSetCommon7(IRibbonSetCommon7 set, IRibbonSetCommon7 dest)
    {
        dest.RibbonChampionAlola = set.RibbonChampionAlola;
        dest.RibbonBattleRoyale = set.RibbonBattleRoyale;
        dest.RibbonBattleTreeGreat = set.RibbonBattleTreeGreat;
        dest.RibbonBattleTreeMaster = set.RibbonBattleTreeMaster;
    }

    internal static void CopyRibbonSetCommon8(IRibbonSetCommon8 set, IRibbonSetCommon8 dest)
    {
        dest.RibbonChampionGalar = set.RibbonChampionGalar;
        dest.RibbonTowerMaster = set.RibbonTowerMaster;
        dest.RibbonMasterRank = set.RibbonMasterRank;
        dest.RibbonTwinklingStar = set.RibbonTwinklingStar;
        dest.RibbonHisui = set.RibbonHisui;
    }

    internal static void CopyRibbonSetMark8(IRibbonSetMark8 set, IRibbonSetMark8 dest)
    {
        dest.RibbonMarkLunchtime = set.RibbonMarkLunchtime;
        dest.RibbonMarkSleepyTime = set.RibbonMarkSleepyTime;
        dest.RibbonMarkDusk = set.RibbonMarkDusk;
        dest.RibbonMarkDawn = set.RibbonMarkDawn;
        dest.RibbonMarkCloudy = set.RibbonMarkCloudy;
        dest.RibbonMarkRainy = set.RibbonMarkRainy;
        dest.RibbonMarkStormy = set.RibbonMarkStormy;
        dest.RibbonMarkSnowy = set.RibbonMarkSnowy;
        dest.RibbonMarkBlizzard = set.RibbonMarkBlizzard;
        dest.RibbonMarkDry = set.RibbonMarkDry;
        dest.RibbonMarkSandstorm = set.RibbonMarkSandstorm;
        dest.RibbonMarkMisty = set.RibbonMarkMisty;
        dest.RibbonMarkDestiny = set.RibbonMarkDestiny;
        dest.RibbonMarkFishing = set.RibbonMarkFishing;
        dest.RibbonMarkCurry = set.RibbonMarkCurry;
        dest.RibbonMarkUncommon = set.RibbonMarkUncommon;
        dest.RibbonMarkRare = set.RibbonMarkRare;
        dest.RibbonMarkRowdy = set.RibbonMarkRowdy;
        dest.RibbonMarkAbsentMinded = set.RibbonMarkAbsentMinded;
        dest.RibbonMarkJittery = set.RibbonMarkJittery;
        dest.RibbonMarkExcited = set.RibbonMarkExcited;
        dest.RibbonMarkCharismatic = set.RibbonMarkCharismatic;
        dest.RibbonMarkCalmness = set.RibbonMarkCalmness;
        dest.RibbonMarkIntense = set.RibbonMarkIntense;
        dest.RibbonMarkZonedOut = set.RibbonMarkZonedOut;
        dest.RibbonMarkJoyful = set.RibbonMarkJoyful;
        dest.RibbonMarkAngry = set.RibbonMarkAngry;
        dest.RibbonMarkSmiley = set.RibbonMarkSmiley;
        dest.RibbonMarkTeary = set.RibbonMarkTeary;
        dest.RibbonMarkUpbeat = set.RibbonMarkUpbeat;
        dest.RibbonMarkPeeved = set.RibbonMarkPeeved;
        dest.RibbonMarkIntellectual = set.RibbonMarkIntellectual;
        dest.RibbonMarkFerocious = set.RibbonMarkFerocious;
        dest.RibbonMarkCrafty = set.RibbonMarkCrafty;
        dest.RibbonMarkScowling = set.RibbonMarkScowling;
        dest.RibbonMarkKindly = set.RibbonMarkKindly;
        dest.RibbonMarkFlustered = set.RibbonMarkFlustered;
        dest.RibbonMarkPumpedUp = set.RibbonMarkPumpedUp;
        dest.RibbonMarkZeroEnergy = set.RibbonMarkZeroEnergy;
        dest.RibbonMarkPrideful = set.RibbonMarkPrideful;
        dest.RibbonMarkUnsure = set.RibbonMarkUnsure;
        dest.RibbonMarkHumble = set.RibbonMarkHumble;
        dest.RibbonMarkThorny = set.RibbonMarkThorny;
        dest.RibbonMarkVigor = set.RibbonMarkVigor;
        dest.RibbonMarkSlump = set.RibbonMarkSlump;
    }
}
