using PKHeX.Core;

namespace HomeLive.Core;

public static class LegalityHandler
{
    public static PKM FixLegality(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        //If the PKM is not legal, try to fix it
        if (!legality.Valid)
        {
            //Try to legalize a clone, if the editings are successful, apply the changes to the actual PKM
            var clone = pkm.Clone();
            clone.Heal();
            clone.FixCopyHeight();
            clone.FixConversionSV();
            clone.FixConversionPLA();
            clone.FixConversionBDSP();
            clone.FixConversionLGPE();
            clone.FixMoveSet();

            legality = new LegalityAnalysis(clone);
            if (legality.Valid)
                return clone;
        }
        return pkm;
    }

    private static void FixCopyHeight(this PKM pkm)
    {
        //Most PH1 PA8 conversions have the Copy Height (Scale) as 0. Try to fix by copying from the value from the actual Height.
        if (pkm is PA8 pa8)
        {
            //CopyHeight legality errors falls under the "Encounter" CheckIdentifier.
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
                    var oldInvalid = encs.Count(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid);
                    var newInvalid = newencs.Count(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid);
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

    private static void FixConversionSV(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid && pkm is PK8 pk8)
        {
            if (pk8.Version is GameVersion.SL)
            {
                pk8.Version = GameVersion.SW;
                pk8.MetLocation = LocationsHOME.SWSL;
            }
            else if (pk8.Version is GameVersion.VL)
            {
                pk8.Version = GameVersion.SH;
                pk8.MetLocation = LocationsHOME.SHVL;
            }
        }
    }

    private static void FixConversionPLA(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid && pkm.Version is GameVersion.PLA)
        {
            //Set Ability number for PLA mons
            if (legality.Results.Any(r => r.Identifier is CheckIdentifier.Ability && r.Judgement is Severity.Invalid))
            {
                var index = legality.EncounterMatch.Ability switch
                {
                    AbilityPermission.OnlyFirst or AbilityPermission.Any12 => 0,
                    AbilityPermission.OnlySecond => 1,
                    AbilityPermission.OnlyHidden or AbilityPermission.Any12H => 2,
                    _ => 0,
                };
                pkm.SetAbilityIndex(index);
            }
            //PLA -> SWSH PH1 GameData conversion incorrectly set a wrong GameVersion, Met Location and Egg Met Location
            if (pkm is PK8 pk8)
            {
                pk8.Version = GameVersion.SW;
                pk8.MetLocation = LocationsHOME.SWLA;
                if (pk8.EggLocation == 65534)
                    pk8.EggLocation = 0;
            }
        }
    }

    private static void FixConversionBDSP(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if (!legality.Valid)
        {
            if (pkm.Version is GameVersion.SP or GameVersion.BD)
            {
                //Only apply if Egg Location is 0 but recognized as Egg
                if (pkm is PB8 pb8 && pb8.EggLocation == 0)
                    pb8.EggLocation = ushort.MaxValue;
                //BDSP origin PH1 -> PK8 Origin Game incorrectly applied as BD or SP, should be applied as Sword or Shield
                else if (pkm is PK8 pk8)
                    pk8.FixSinnohToGalar();
            }
        }
    }

    private static void FixSinnohToGalar(this PKM pkm)
    {
        //PK8s Can not have origin game as Brilliant Diamond or Shining Pearl
        if (pkm.Version is GameVersion.BD)
        {
            pkm.Version = GameVersion.SW;
            pkm.MetLocation = LocationsHOME.SWBD;
        }
        else if (pkm.Version is GameVersion.SP)
        {
            pkm.Version = GameVersion.SH;
            pkm.MetLocation = LocationsHOME.SHSP;
        }

        //All the PB8 are set as Egg during the conversion, check the encounter and eventually unset the Egg location
        var clone = pkm.Clone();
        var legality = new LegalityAnalysis(clone);
        var encounter = legality.Results.Where(f => f.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
        var applied = false;
        if (encounter.Judgement is Severity.Invalid && clone.EggLocation == LocationsHOME.SWSHEgg)
        {
            clone.EggLocation = 0;
            applied = true;
        }

        //If the Egg legality has been fixed, apply the edits to the original PKM file
        legality = new LegalityAnalysis(clone);
        encounter = legality.Results.Where(f => f.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
        if (applied && encounter.Judgement is not Severity.Invalid)
            pkm.EggLocation = 0;
    }

    private static void FixConversionLGPE(this PKM pkm)
    {
        var legality = new LegalityAnalysis(pkm);
        if(pkm is PB7 pb7 && !legality.Valid)
        {
            //Reset ability
            if (legality.Results.Any(r => r.Identifier is CheckIdentifier.Ability && r.Judgement is Severity.Invalid))
            {
                var index = legality.EncounterMatch.Ability switch
                {
                    AbilityPermission.OnlyFirst or AbilityPermission.Any12 => 0,
                    AbilityPermission.OnlySecond => 1,
                    AbilityPermission.OnlyHidden or AbilityPermission.Any12H => 2,
                    _ => 0,
                };
                pkm.SetAbilityIndex(index);
            }

            if (legality.Results.Any(r => r.Identifier is CheckIdentifier.Encounter && r.Judgement is Severity.Invalid) && legality.EncounterMatch is WB7 { CardID: 9028 } wb7)
            {
                pb7.HeightAbsolute = wb7.GetHomeHeightAbsolute();
                pb7.WeightAbsolute = wb7.GetHomeWeightAbsolute();
            }

            pb7.ResetCP();
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
        }
        pkm.HealPP();
    }
}