using HomeLive.DeviceExecutor;

namespace HomeLive.Core;

public static class RangeHandler
{
    /// <summary>
    /// Splits the HOME boxes into consecutive ranges, each one filling a local save file of <paramref name="localBoxes"/> x <paramref name="localSlots"/> slots.
    /// Boxes and slots are 1-based.
    /// </summary>
    public static List<((int box, int slot) start, (int box, int slot) end)> GetDownloadRanges(int localBoxes, int localSlots)
    {
        var ranges = new List<((int, int), (int, int))>();

        var homeBoxes = HomeDataOffsets.HomeBoxCount;
        var homeSlots = HomeDataOffsets.HomeSlotCount;

        var totalSlots = localBoxes * localSlots;
        var startSlot = 1;
        var endSlot = totalSlots;

        while (startSlot <= homeSlots * homeBoxes)
        {
            var startBox = (startSlot - 1) / homeSlots + 1;
            var endBox = (endSlot - 1) / homeSlots + 1;

            if (endBox > homeBoxes)
            {
                endBox = homeBoxes;
                endSlot = homeBoxes * homeSlots;
            }

            ranges.Add(((startBox, (startSlot - 1) % homeSlots + 1), (endBox, (endSlot - 1) % homeSlots + 1)));

            startSlot = endSlot + 1;
            endSlot = startSlot + totalSlots - 1;
        }

        return ranges;
    }

    /// <summary>
    /// Gets the entries of the HOME box <paramref name="box"/> (1-based) that belong to the range from <paramref name="start"/> to <paramref name="end"/>.
    /// </summary>
    public static IEnumerable<T> GetEntriesInRange<T>(IReadOnlyList<T> boxEntries, int box, (int box, int slot) start, (int box, int slot) end)
    {
        if (box < start.box || box > end.box)
            yield break;

        // The first and the last box of a range can be partial.
        var first = box == start.box ? start.slot - 1 : 0;
        var last = box == end.box ? Math.Min(end.slot, boxEntries.Count) : boxEntries.Count;
        for (var i = first; i < last; i++)
            yield return boxEntries[i];
    }
}
