using HomeLive.Plugins;

namespace HomeLive.Tests;

public class GitHubUtilTests
{
    [Theory]
    [InlineData("4.1.0", "4.2.0", true)]
    [InlineData("4.1.0", "4.10.0", true)]
    [InlineData("4.1.0", "v4.2.0", true)]
    [InlineData("4.1.0", "5.0.0 - HOME 5.0.0 support", true)]
    [InlineData("4.1.0", "4.1.0", false)]
    [InlineData("4.1.0", "4.0.9", false)]
    [InlineData("4.10.0", "4.9.0", false)]
    public void IsNewerVersion_ComparesVersionNumbers(string current, string latest, bool expected)
        => Assert.Equal(expected, GitHubUtil.IsNewerVersion(current, latest));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("latest")]
    public void IsNewerVersion_UnreadableReleaseName_IsNotAnUpdate(string? latest)
        => Assert.False(GitHubUtil.IsNewerVersion("4.1.0", latest));
}
