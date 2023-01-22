using PKHeX.Core;

namespace HOME
{
    public static class LegalityHelper
    {
        //This is only used when loading PKH to Pokémon Editors/Sav Boxes. Files dumped are not altered.
        public static bool CheckAndFixLegality(PKM pkm, SaveFile? sav = null)
        {
            var l = new LegalityAnalysis(pkm);
            if (!l.Valid)
            {
                pkm.Heal();

                if ((Move)pkm.Move1 == Move.None)
                {
                    pkm.SetRelearnMoves(l.GetSuggestedRelearnMoves());
                    pkm.SetMoves(l.GetSuggestedCurrentMoves(), false);
                }

                l = new LegalityAnalysis(pkm);
            }
            return l.Valid;
        }
    }
}
