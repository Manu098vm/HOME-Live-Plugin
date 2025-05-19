using System.Diagnostics;
using PKHeX.Core;

namespace HomeLive.Plugins;

public class HomePlugin : IPlugin
{
    public const string Version = "3.2.8";
    public string Name => nameof(HomePlugin);
    public int Priority => 1;

    public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
    public IPKMView PKMEditor { get; private set; } = null!;
    public Dictionary<string, string> Strings = null!;
    public string Language = GameInfo.CurrentLanguage;

    public ToolStripMenuItem Plugin = new("Home Live Plugins");
    public ToolStripMenuItem Viewer = new("Home Live Viewer");
    public ToolStripMenuItem Dumper = new("Home Live Dumper");
    public ToolStripMenuItem Settings = new("Home Live Plugin Settings");

    public void Initialize(params object[] args)
    {
        GenerateDictionary();
        TranslateDictionary(Language);
        Task.Run(TryUpdate).Wait();
        Plugin.Image = Properties.Resources.icon.ToBitmap();
        SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
        PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
        if (((ToolStrip)Array.Find(args, z => z is ToolStrip)!).Items.Find("Menu_Tools", false)[0] is not ToolStripDropDownItem tools)
            throw new ArgumentException("Not a valid menu tool.");
        Viewer.Click += (s, e) => new ViewerForm(SaveFileEditor, PKMEditor, Language).Show();
        Dumper.Click += (s, e) => new DumperForm(SaveFileEditor, PKMEditor, Language).Show();
        Settings.Click += (s, e) => new SettingsForm(Language).ShowDialog();
        Plugin.DropDownItems.Add(Viewer);
        Plugin.DropDownItems.Add(Dumper);
        Plugin.DropDownItems.Add(Settings);
        tools.DropDownItems.Add(Plugin);
        NotifySaveLoaded();
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "Plugin.HomeLivePlugins", "" },
            { "Plugin.DumperPlugin", "" },
            { "Plugin.ViewerPlugin", "" },
            { "Plugin.SettingsPlugin", "" },
            { "Update.Message", "" },
            { "Update.Popup", "" }
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    public void TranslatePlugins()
    {
        Plugin.Text = Strings["Plugin.HomeLivePlugins"];
        Dumper.Text = Strings["Plugin.DumperPlugin"];
        Viewer.Text = Strings["Plugin.ViewerPlugin"];
        Settings.Text = Strings["Plugin.SettingsPlugin"];
    }

    private async Task TryUpdate()
    {
        if (await GitHubUtil.IsUpdateAvailable())
        {
            var result = MessageBox.Show(Strings["Update.Message"], Strings["Update.Popup"], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Process.Start(new ProcessStartInfo { FileName = @"https://github.com/Manu098vm/HOME-Live-Plugin/releases", UseShellExecute = true });
        }
    }

    public void NotifySaveLoaded()
    {
        Language = GameInfo.CurrentLanguage;
        TranslatePlugins();
        if (SaveFileEditor.SAV is SAV7b or SAV8SWSH or SAV8BS or SAV8LA or SAV9SV)
            Plugin.Enabled = true;
        else
            Plugin.Enabled = false;
    }

    public void NotifyDisplayLanguageChanged(string language)
    {
        Language = language;
        TranslateDictionary(language);
        TranslatePlugins();
    }

    public bool TryLoadFile(string path) => DumperForm.TryLoadFile(path, SaveFileEditor, PKMEditor);
}