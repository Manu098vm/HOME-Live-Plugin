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
                    var val = new RibbonVerifierArguments(pkm, l.EncounterMatch, l.Info.EvoChainsAllGens);
                    RibbonApplicator.FixInvalidRibbons(val);
                }

                l.ResetParse();
            }
            return l.Valid;
        }
    }
}
