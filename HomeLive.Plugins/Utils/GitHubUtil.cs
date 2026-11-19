using Octokit;

namespace HomeLive.Plugins;

public static class GitHubUtil
{
    // Bounds the PKHeX startup delay when GitHub is slow or unreachable.
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

    public static async Task<bool> IsUpdateAvailable()
    {
        try
        {
            return IsNewerVersion(GetPluginVersion(), await GetLatestReleaseName());
        }
        catch (Exception)
        {
            // No connection, rate limit or no release: never report an update.
            return false;
        }
    }

    /// <summary>
    /// Checks if the release name <paramref name="latest"/> (e.g. "4.10.0" or "v4.2.0") is a newer version than <paramref name="current"/>.
    /// A name without a readable version number never counts as an update.
    /// </summary>
    public static bool IsNewerVersion(string current, string? latest) =>
        TryParseVersion(current, out var currentVersion) &&
        TryParseVersion(latest, out var latestVersion) &&
        latestVersion > currentVersion;

    private static string GetPluginVersion() => HomePlugin.Version;

    private static async Task<string?> GetLatestReleaseName()
    {
        var client = new GitHubClient(new ProductHeaderValue("HOME-Live-Plugin"));
        client.SetRequestTimeout(RequestTimeout);
        var release = await client.Repository.Release.GetLatest("Manu098vm", "HOME-Live-Plugin");
        return string.IsNullOrWhiteSpace(release.Name) ? release.TagName : release.Name;
    }

    private static bool TryParseVersion(string? text, out Version version)
    {
        version = new Version();
        if (string.IsNullOrWhiteSpace(text))
            return false;

        // Only the leading number counts: "v4.2.0" and "4.2.0 - HOME 4.2.0 support" both read as 4.2.0.
        var span = text.AsSpan().Trim().TrimStart("vV");
        var length = 0;
        while (length < span.Length && (char.IsAsciiDigit(span[length]) || span[length] == '.'))
            length++;

        if (!Version.TryParse(span[..length].TrimEnd('.'), out var parsed))
            return false;

        // Missing parts count as 0, so "4.1" equals "4.1.0".
        version = new Version(parsed.Major, parsed.Minor, Math.Max(parsed.Build, 0), Math.Max(parsed.Revision, 0));
        return true;
    }
}
