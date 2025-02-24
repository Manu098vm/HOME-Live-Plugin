using PKHeX.Core;

namespace HomeLive.Core;

public static class SaveFileHandler
{
    public static bool SetPoke(this SaveFile sav, IPKMView view, HomeWrapper? pkm, ConversionType conversionType, bool fixLegality)
    {
        var res = pkm.ConvertToType(sav.PKMType, conversionType);
        if (res is not null)
        {
            sav.AdaptToSaveFile(res, false);
            view.PopulateFieldsSafe(fixLegality ? res.FixLegality() : res);
            return true;
        }
        return false;
    }

    public static void SetPokeList(this SaveFile sav, List<HomeWrapper?> pks, ConversionType conversionType, bool fixLegality)
    {
        var version = sav.GetGameVersion();
        var numBox = version.GetBoxCount();
        var numSlot = version.GetSlotCount();

        var convertedList = pks.ConvertListToType(sav.PKMType, conversionType);
        if (convertedList.Count > 0) sav.ClearBoxes();

        foreach (var (entity, i) in convertedList.Select((entity, i) => (entity, i)))
        {
            if (i > (numBox * numSlot)) break;
            if (entity is not null)
            {
                sav.AdaptToSaveFile(entity, false);
                sav.SetBoxSlotAtIndex(fixLegality ? entity.FixLegality() : entity, i / numSlot, i % numSlot);
            }
        }
    }

    public static void ReloadSlotsSafe(this ISaveFileProvider sav)
    {
        dynamic control = sav;
        if (control.InvokeRequired)
            control.Invoke(new Action(sav.ReloadSlots));
        else
            sav.ReloadSlots();
    }

    public static void PopulateFieldsSafe(this IPKMView view, PKM pkm)
    {
        dynamic control = view;
        if (control.InvokeRequired)
            control.Invoke(new Action(() => view.PopulateFields(pkm)));
        else
            view.PopulateFields(pkm);
    }

    public static bool IsBlankSaveFile(this SaveFile sav) => sav.OT.Equals("PKHeX");
}