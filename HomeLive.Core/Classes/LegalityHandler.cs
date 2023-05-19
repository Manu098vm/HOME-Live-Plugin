using PKHeX.Core;

namespace HomeLive.Core;

public static class LegalityHandler
{
    public static LegalityAnalysis FixLegality(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid)
        {
            pkm.Heal();
            pkm.FixCopyHeight();
            pkm.FixConversionPLA();
            pkm.FixConversionBDSP();
            pkm.FixConversionLGPE();
            pkm.FixMoveSet();
        }
        return new LegalityAnalysis(pkm);
    }

    private static void FixConversionPLA(this PKM pkm)
    {
        const int PLA_DEFLOC = 60000;

        //PLA -> SWSH GameData conversion incorrectly set a wrong GameVersion, Met Location and Egg Met Location
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid && pkm is PK8 pk8 && (GameVersion)pk8.Version is GameVersion.PLA)
        {
            pk8.Version = (int)GameVersion.SW;
            pk8.Met_Location = PLA_DEFLOC;
            if (pk8.Egg_Location == 65534)
                pk8.Egg_Location = 0;
        }
    }

    private static void FixCopyHeight(this PKM pkm)
    {
        //Most PA8 conversions have the Copy Height (Scale) as 0. Try to fix by copying from the value from the actual Height.
        if (pkm is PA8 pa8)
        {
            //CopyHeight legality errors falls under the Encounter CheckIdentifier.
            //There are more Encounter CheckResults, so iterate through all of them just to be sure.
            var legality = new LegalityAnalysis(pkm);
            var encs = legality.Results.Where(e => e.Identifier is CheckIdentifier.Encounter).ToArray();
            foreach (var enc in encs)
            {
                //Only try to apply the CopyHeight to the Invalid encounters
                if (enc.Judgement is Severity.Invalid)
                {
                    var clone = pa8.Clone();
                    clone.Scale = clone.HeightScalar;

                    //Check if the legality has been fixed
                    var newencs = new LegalityAnalysis(clone).Results.Where(e => e.Identifier is CheckIdentifier.Encounter).ToArray();
                    var oldInvalid = encs.Where(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid).Count();
                    var newInvalid = newencs.Where(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid).Count();
                    var success = newInvalid < oldInvalid;

                    //If it's legal now, apply the CopyHeight to the actual PKM and stop the iteration
                    if (success)
                    {
                        pa8.Scale = pa8.HeightScalar;
                        break;
                    }
                }
            }
        }
    }

    private static void FixConversionBDSP(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid)
        {
            if ((GameVersion)pkm.Version is GameVersion.SP or GameVersion.BD)
            {
                //Only apply if Egg Location is 0 but recognized as Egg
                if (pkm is PB8 pb8 && pb8.Egg_Location == 0)
                    pb8.Egg_Location = ushort.MaxValue;
                //PK8 Origin Game incorrectly applied as BD or SP, should be applied as Sword or Shield
                else if (pkm is PK8 pk8)
                    pk8.FixSinnohToGalar();
            }
        }
    }

    private static void FixSinnohToGalar(this PKM pkm)
    {
        const int SP_DEFLOC = 59998;
        const int BD_DEFLOC = 59999;

        //PK8s Can not have origin game as Brilliant Diamond or Shining Pearl
        if ((GameVersion)pkm.Version is GameVersion.BD)
        {
            pkm.Version = (int)GameVersion.SW;
            pkm.Met_Location = BD_DEFLOC;
        }
        else if ((GameVersion)pkm.Version is GameVersion.SP)
        {
            pkm.Version = (int)GameVersion.SH;
            pkm.Met_Location = SP_DEFLOC;
        }

        //All the PB8 are set as Egg during the conversion, check the encounter and eventually unset the Egg location
        var clone = pkm.Clone();
        var legality = new LegalityAnalysis(clone);
        var encounter = legality.Results.Where(f => f.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
        var applied = false;
        if (encounter.Judgement is Severity.Invalid && clone.Egg_Location == 65534)
        {
            clone.Egg_Location = 0;
            applied = true;
        }

        //If the Egg legality has been fixed, apply the edits to the original PKM file
        legality = new LegalityAnalysis(clone);
        encounter = legality.Results.Where(f => f.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
        if (applied && encounter.Judgement is not Severity.Invalid)
            pkm.Egg_Location = 0;
    }

    private static void FixConversionLGPE(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if(pkm is PB7 pb7 && !legality.Valid)
        {
            //Fix AVs if there's any AV related legality result
            var AVs = CheckAVs(legality);
            for(var i = 0; i < AVs.Length; i++)
                if (AVs[i] > 0)
                    pb7.SetAV(i, AVs[i]);

            //Recalc CP if there's a CP legality result
            var invalidEncs = legality.Results.Where(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid).Count();
            if(invalidEncs > 0)
            {
                var clone = pb7.Clone();
                clone.ResetCP();
                var newInvalid = new LegalityAnalysis(clone).Results.Where(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid).Count();
                if (newInvalid < invalidEncs)
                    pb7.ResetCP();
            }
        }
    }
    
    private static void FixMoveSet(this PKM pkm)
    {
        var invalidMoveset = false;
        var legality = new LegalityAnalysis(pkm);
        foreach (var move in legality.Info.Moves)
            if (!move.Valid) invalidMoveset = true;
        foreach (var move in legality.Info.Relearn)
            if (!move.Valid) invalidMoveset = true;

        if (invalidMoveset)
        {
            var relearn = (Span<ushort>)stackalloc ushort[4];
            var moves = (Span<ushort>)stackalloc ushort[4];
            legality.GetSuggestedRelearnMoves(relearn, legality.EncounterMatch);
            legality.GetSuggestedCurrentMoves(moves, MoveSourceType.LevelUp);
            pkm.SetRelearnMoves(relearn);
            pkm.SetMoves(moves);
            pkm.HealPP();
        }
    }

    private static byte[] CheckAVs(LegalityAnalysis legality)
    {
        var AVs = new byte[6];
        var avComments = legality.Results.Where(r => r.Identifier is CheckIdentifier.AVs).Select(r => r.Comment).ToArray();
        foreach (var str in avComments)
        {
            var avWords = new string[] { "AV_HP", "AV_ATK", "AV_DEF", "AV_SPA", "AV_SPD", "AV_SPE" };
            foreach (var (av, j) in avWords.Select((av, j) => (av, j)))
            {
                if (str.Contains(av))
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (char.IsDigit(str[i]))
                        {
                            var valueEnd = i + 1;
                            while (valueEnd < str.Length && char.IsDigit(str[valueEnd]))
                                valueEnd++;

                            var valueLength = valueEnd - i;
                            var valueString = str.Substring(i, valueLength);
                            if (int.TryParse(valueString, out int result))
                                AVs[j] = (byte)result;
                        }
                    }
                }
            }
        }
        return AVs;
    }
}
