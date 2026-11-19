using HomeLive.Core;
using PKHeX.Core;

namespace HomeLive.Tests;

public class PokeHandlerFileNameTests
{
    [Fact]
    public void GetFileName_AlphaPA8_HasAlphaSuffix()
    {
        var pa8 = new PA8 { Species = (ushort)Species.Pikachu, IsAlpha = true };

        Assert.Contains("(Alpha)", new HomeWrapper(pa8).GetFileName());
    }

    [Fact]
    public void GetFileName_NoblePA8_HasNobleSuffix()
    {
        var pa8 = new PA8 { Species = (ushort)Species.Kleavor, IsNoble = true };

        Assert.Contains("(Noble)", new HomeWrapper(pa8).GetFileName());
    }

    [Fact]
    public void GetFileName_AlphaPA9_HasAlphaSuffix()
    {
        var pa9 = new PA9 { Species = (ushort)Species.Pikachu, IsAlpha = true };

        Assert.Contains("(Alpha)", new HomeWrapper(pa9).GetFileName());
    }

    [Fact]
    public void GetFileName_AlphaStoredInHomeData_HasAlphaSuffix()
    {
        var pa8 = new PA8 { Species = (ushort)Species.Pikachu, IsAlpha = true };
        var home = new HomeWrapper(PKH.ConvertFromPKM(pa8));

        Assert.Contains("(Alpha)", home.GetFileName());
    }

    [Fact]
    public void GetFileName_RegularPA8_HasNoAlphaOrNobleSuffix()
    {
        var name = new HomeWrapper(new PA8 { Species = (ushort)Species.Pikachu }).GetFileName();

        Assert.DoesNotContain("(Alpha)", name);
        Assert.DoesNotContain("(Noble)", name);
    }
}
