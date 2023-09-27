namespace HomeLive.Core;

public static class HomeDataOffsets
{
    public const string HomeTitleID = "010015F008C54000";
    public const string HomeBuildID = "5EABBB752498C865";

    public const int HomeSlotSize       = 720; //0x2D0
    public const int HomeSlotCount      = 30;
    public const int HomeBoxCount       = 200;
    public const int HeaderLength       = 0x10;
    public const int SeedOffset         = 0x02;
    public const int EncSizeOffset      = 0x0E;

    private const uint BoxStartOffset = 0x12B90; //Heap

    public static uint GetBoxOffset(int box) => BoxStartOffset + (uint)(HomeSlotSize * HomeSlotCount * box);
    public static uint GetSlotOffset(int box, int slot) => GetBoxOffset(box) + (uint)(HomeSlotSize * slot);
}