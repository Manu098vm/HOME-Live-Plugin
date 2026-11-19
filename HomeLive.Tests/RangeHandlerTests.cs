using HomeLive.Core;

namespace HomeLive.Tests;

public class RangeHandlerTests
{
    // Let's Go: 40 boxes x 25 slots = 1000 slots per range, not a multiple of the 30 slots of a HOME box.
    private const int LetsGoBoxes = 40;
    private const int LetsGoSlots = 25;

    [Fact]
    public void GetDownloadRanges_LetsGo_SplitsHomeIntoNineRanges()
    {
        var ranges = RangeHandler.GetDownloadRanges(LetsGoBoxes, LetsGoSlots);

        Assert.Equal(9, ranges.Count);
        Assert.Equal(((1, 1), (34, 10)), ranges[0]);
        Assert.Equal(((34, 11), (67, 20)), ranges[1]);
        Assert.Equal(((267, 21), (300, 30)), ranges[^1]);
    }

    [Theory]
    [InlineData(32, 30, 10)] // SWSH, PLA, SV, ZA
    [InlineData(40, 30, 8)]  // BDSP
    public void GetDownloadRanges_CapacityOfWholeHomeBoxes_UsesWholeBoxes(int boxes, int slots, int expectedRanges)
    {
        var ranges = RangeHandler.GetDownloadRanges(boxes, slots);

        Assert.Equal(expectedRanges, ranges.Count);
        Assert.All(ranges, range =>
        {
            Assert.Equal(1, range.start.slot);
            Assert.Equal(30, range.end.slot);
        });
        Assert.Equal(300, ranges[^1].end.box);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(8)]
    public void GetEntriesInRange_LetsGo_CollectsExactlyTheSlotsOfTheRange(int rangeIndex)
    {
        var (start, end) = RangeHandler.GetDownloadRanges(LetsGoBoxes, LetsGoSlots)[rangeIndex];

        var collected = new List<(int box, int slot)>();
        for (var box = start.box; box <= end.box; box++)
            collected.AddRange(RangeHandler.GetEntriesInRange(FakeHomeBox(box), box, start, end));

        Assert.Equal(LetsGoBoxes * LetsGoSlots, collected.Count);
        Assert.Equal(start, collected[0]);
        Assert.Equal(end, collected[^1]);
    }

    // A HOME box whose entries are their own 1-based (box, slot) position.
    private static List<(int box, int slot)> FakeHomeBox(int box) =>
        Enumerable.Range(1, 30).Select(slot => (box, slot)).ToList();
}
