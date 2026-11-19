using HomeLive.Core;
using PKHeX.Core;

namespace HomeLive.Tests;

public class VersionHandlerTests
{
    [Theory]
    [InlineData(GameVersion.GP, 40, 25)]
    [InlineData(GameVersion.SW, 32, 30)]
    [InlineData(GameVersion.BD, 40, 30)]
    [InlineData(GameVersion.PLA, 32, 30)]
    [InlineData(GameVersion.SL, 32, 30)]
    [InlineData(GameVersion.ZA, 32, 30)]
    public void BoxAndSlotCounts_AcceptSingleGameVersions(GameVersion version, int boxes, int slots)
    {
        Assert.Equal(boxes, version.GetBoxCount());
        Assert.Equal(slots, version.GetSlotCount());
    }
}
