//Most of code in these functions is taken from PKHeX Plugins.
using System;
using System.Collections.Generic;
using System.Linq;
using PKHeX.Core;

namespace HOME
{
    public static class LegalityHelper
    {
        public static bool CheckAndFixLegality(PKM pkm)
        {
            var l = new LegalityAnalysis(pkm);
            if (!l.Valid)
            {

                //As of PKHeX 22.06.26, If Specific Game Data was not existing, the converted format ends up with no moves.
                /*if ((Move)pkm.Move1 == Move.None)
                    pkm.SetMoves(l.GetSuggestedCurrentMoves().AsSpan(), false);*/

                //Handle HOME PP bug and PKM details
                pkm.Heal();
                /*if (pkm is PA8 || pkm is G8PKM)
                    new LegalityRejuvenator().Rejuvenate(pkm, pkm);*/

                //Converted Data sometimes uncorrectly set Battle Memory and Contest Memory Ribbons for unknown reason, making the resulting Pokémon Illegal.
                var report = l.Report();
                if (report.Contains(string.Format(LegalityCheckStrings.LRibbonFInvalid_0, "")))
                {
                    var val = string.Format(LegalityCheckStrings.LRibbonFInvalid_0, "");
                    string[] ribbonList = GetRequiredRibbons(report, val);
                    var invalidRibbons = GetRibbonsRequired(pkm, ribbonList);
                    pkm.RemoveMemoryRibbon(invalidRibbons);
                }

                l.ResetParse();
            }
            return l.Valid;
        }

        private static string[] GetRequiredRibbons(string Report, string val)
        {
            return Report.Split(new[] { val }, StringSplitOptions.None)[1].Split(new[] { "\r\n" }, StringSplitOptions.None)[0].Split(new[] { ", " }, StringSplitOptions.None);
        }

        private static IEnumerable<string> GetRibbonsRequired(PKM pk, string[] ribbonList)
        {
            foreach (var RibbonName in GetRibbonNames(pk))
            {
                string v = RibbonStrings.GetName(RibbonName).Replace("Ribbon", "");
                if (ribbonList.Contains(v))
                    yield return RibbonName;
            }
        }

        private static IEnumerable<string> GetRibbonNames(PKM pk) => ReflectUtil.GetPropertiesStartWithPrefix(pk.GetType(), "Ribbon").Distinct();

        private static void RemoveMemoryRibbon(this PKM pk, IEnumerable<string> ribNames)
        {
            foreach (string rName in ribNames)
                if (rName is nameof(PK6.RibbonCountMemoryBattle) || rName is nameof(PK6.RibbonCountMemoryContest))
                    ReflectUtil.SetValue(pk, rName, 0);
        }
    }
}
