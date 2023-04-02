using PKHeX.Core;
using System;

namespace HOME
{
    public static class LegalityHelper
    {
        //This is only used when loading PKH to Pokémon Editors/Sav Boxes. Files dumped are not altered.
        public static bool CheckAndFixLegality(PKM pkm)
        {
            var l = new LegalityAnalysis(pkm);
            if (!l.Valid)
            {
                pkm.Heal();

                if ((Move)pkm.Move1 == Move.None)
                {
                    Span<ushort> relearn = stackalloc ushort[4];
                    l.GetSuggestedRelearnMoves(relearn);
                    pkm.SetRelearnMoves(relearn);

                    Span<ushort> moves = stackalloc ushort[4];
                    l.GetSuggestedCurrentMoves(moves);
                    pkm.SetMoves(moves, false);
                }

                l = new LegalityAnalysis(pkm);
            }
            return l.Valid;
        }
    }
}
