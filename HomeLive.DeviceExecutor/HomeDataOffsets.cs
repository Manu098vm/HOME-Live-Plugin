namespace HomeLive.DeviceExecutor;

public static class HomeDataOffsets
{
    public const string HomeTitleID = "010015F008C54000";
    public const string HomeSupportedVersion = "3.2.1";

    public const int HomeSlotSize       = 720; //0x2D0
    public const int HomeSlotCount      = 30;
    public const int HomeBoxCount       = 200;
    public const int HeaderLength       = 0x10;
    public const int SeedOffset         = 0x02;
    public const int EncSizeOffset      = 0x0E;

    //private const uint BoxStartOffset = 0x12B90; //Heap
    public static IReadOnlyList<long> BoxStartPointer { get; } = [0x8D6E78, 0x08, 0x58];

    public static ulong GetBoxOffset(ulong startOffset, int box) => startOffset + (ulong)(HomeSlotSize * HomeSlotCount * box);
    public static ulong GetSlotOffset(ulong startOffset, int box, int slot) => GetBoxOffset(startOffset, box) + (uint)(HomeSlotSize * slot);
}