namespace HomeLive.Core;

public static class HomeDataOffsets
{
    public const int HomeSlotSize       = 584; //0x248
    public const int HomeSlotCount      = 30;
    public const int HomeBoxCount       = 200;

    public const int HeaderLength       = 0x10;
    public const int SeedOffset         = 0x02;
    public const int EncSizeOffset      = 0x0E;

    private static uint BoxStartOffset = 0x36C000;

    public static uint GetBoxOffset(int box) => BoxStartOffset + (uint)(HomeSlotSize * HomeSlotCount * box);
    public static uint GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (uint)(HomeSlotSize * slot);
}