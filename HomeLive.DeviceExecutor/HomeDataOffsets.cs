namespace HomeLive.DeviceExecutor;

public static class HomeDataOffsets
{
    public const string HomeTitleID = "010015F008C54000";
    public const string HomeSupportedVersion = "4.1.0";

    public const int HomeSlotSize = 0x330;
    public const int HomeSlotCount = 30;
    public const int HomeBoxCount = 300;
    public const int HeaderLength = 0x10;
    public const int SeedOffset = 0x02;
    public const int EncSizeOffset = 0x0E;

    public static uint BoxStartOffset { get; } = 0x715FC0;

    public static ulong GetBoxOffset(ulong startOffset, int box) => startOffset + (ulong)(HomeSlotSize * HomeSlotCount * box);
    public static ulong GetSlotOffset(ulong startOffset, int box, int slot) => GetBoxOffset(startOffset, box) + (uint)(HomeSlotSize * slot);
}