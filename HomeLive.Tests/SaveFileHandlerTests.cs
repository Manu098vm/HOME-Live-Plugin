using HomeLive.Core;
using PKHeX.Core;

namespace HomeLive.Tests;

public class SaveFileHandlerTests
{
    [Fact]
    public void SetPokeList_MoreEntriesThanBoxSlots_DoesNotWritePastTheLastBox()
    {
        // In S/V the ride legend (Koraidon/Miraidon) is stored right after the last box slot.
        var sav = new SAV9SV();
        var rideLegendBefore = sav.BoxInfo.RideLegend.ToArray();
        var capacity = sav.BoxCount * sav.BoxSlotCount;
        var list = Enumerable.Range(0, capacity + 1)
            .Select(_ => (HomeWrapper?)new HomeWrapper(new PK9 { Species = (ushort)Species.Pikachu }))
            .ToList();

        sav.SetPokeList(list, ConversionType.AnyData, fixLegality: false);

        Assert.Equal((ushort)Species.Pikachu, sav.GetBoxSlotAtIndex(sav.BoxCount - 1, sav.BoxSlotCount - 1).Species);
        Assert.Equal(rideLegendBefore, sav.BoxInfo.RideLegend.ToArray());
    }
}
