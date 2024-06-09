using PKHeX.Core;
using HomeLive.Core;
using HomeLive.DeviceExecutor;
using SysBot.Base;

namespace HomeLive.Plugins;

public partial class DumperForm : Form
{
    private DeviceExecutor<DeviceState> Executor = null!;
    private Dictionary<string, string> Strings = null!;

    private readonly ISaveFileProvider Provider = null!;
    private readonly IPKMView Editor = null!;

    private readonly CancellationTokenSource TokenSource = new();

    public DumperForm(ISaveFileProvider provider, IPKMView editor, string language)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(language);
        this.TranslateInterface(language);

        Provider = provider;
        Editor = editor;

        if (Properties.Settings.Default.USB)
            radioUSB.Select();
        else
            radioWiFi.Select();

        txtBoxIP.Text = Properties.Settings.Default.ip_address;
        numPort.Value = Properties.Settings.Default.port;
        txtBoxPath.Text = Properties.Settings.Default.path;

        for (var i = 0; i < 200; i++)
            comboBox.Items.Add($"{Strings["Word.Box"]} {i + 1}");
        for (var i = 0; i < 30; i++)
            comboSlot.Items.Add($"{Strings["Word.Slot"]} {i + 1}");

        comboBox.SelectedIndex = 0;
        comboSlot.SelectedIndex = 0;

        btnBrowse.Location = new Point(btnBrowse.Location.X, txtBoxPath.Location.Y);
        btnBrowse.Height = txtBoxPath.Height;
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "Warning.ConnectionMethod", "" },
            { "Warning.IpAddress", "" },
            { "Warning.Port", "" },
            { "Warning.DumpTarget", "" },
            { "Warning.DumpFormat", "" },
            { "Warning.BoxPath", "" },
            { "Warning.SomethingWrong", "" },
            { "Warning.CheckInstallation", "" },
            { "Action.Completed", "" },
            { "Word.Box", "" },
            { "Word.Slot", "" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void Form_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!TokenSource.IsCancellationRequested)
            TokenSource.Cancel();
    }

    private void TxtBoxIP_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || c == '.'))
            e.Handled = true;
    }

    private void WiFi_CheckedChanged(object sender, EventArgs e) => txtBoxIP.Enabled = true;
    private void USB_CheckedChanged(object sender, EventArgs e) => txtBoxIP.Enabled = false;

    private void RadioBox_CheckedChanged(object sender, EventArgs e)
    {
        comboBox.Enabled = true;
        comboSlot.Enabled = false;
    }

    private void RadioSlot_CheckedChanged(object sender, EventArgs e)
    {
        comboBox.Enabled = true;
        comboSlot.Enabled = true;
    }

    private void RadioTargetAll_CheckedChanged(object sender, EventArgs e)
    {
        comboBox.Enabled = false;
        comboSlot.Enabled = false;
        chkBoxFolders.Enabled = radioTargetAll.Checked;
    }

    private void DisableControls()
    {
        toolsToolStripMenuItem.Enabled = false;

        if (grpConnection.InvokeRequired)
            grpConnection.Invoke(() => grpConnection.Enabled = false);
        else
            grpConnection.Enabled = false;

        if (grpAction.InvokeRequired)
            grpAction.Invoke(() => grpAction.Enabled = false);
        else
            grpAction.Enabled = false;

        if (grpPath.InvokeRequired)
            grpPath.Invoke(() => grpPath.Enabled = false);
        else
            grpPath.Enabled = false;

        if (grpDump.InvokeRequired)
            grpDump.Invoke(() => grpDump.Enabled = false);
        else
            grpDump.Enabled = false;

        if (btnConnect.InvokeRequired)
            btnConnect.Invoke(() => btnConnect.Enabled = false);
        else
            btnConnect.Enabled = false;
    }

    private void EnableControls()
    {
        toolsToolStripMenuItem.Enabled = true;

        if (grpConnection.InvokeRequired)
            grpConnection.Invoke(() => grpConnection.Enabled = true);
        else
            grpConnection.Enabled = true;

        if (grpAction.InvokeRequired)
            grpAction.Invoke(() => grpAction.Enabled = true);
        else
            grpAction.Enabled = true;

        if (grpPath.InvokeRequired)
            grpPath.Invoke(() => grpPath.Enabled = true);
        else
            grpPath.Enabled = true;

        if (grpDump.InvokeRequired)
            grpDump.Invoke(() => grpDump.Enabled = true);
        else
            grpDump.Enabled = true;

        if (btnConnect.InvokeRequired)
            btnConnect.Invoke(() => btnConnect.Enabled = true);
        else
            btnConnect.Enabled = true;
    }

    private int GetTargetBox()
    {
        var res = 0;
        if (comboBox.InvokeRequired)
            comboBox.Invoke(() => res = comboBox.SelectedIndex);
        else
            res = comboBox.SelectedIndex;
        return res;
    }

    private int GetTargetSlot()
    {
        var res = 0;
        if (comboSlot.InvokeRequired)
            comboSlot.Invoke(() => res = comboSlot.SelectedIndex);
        else
            res = comboSlot.SelectedIndex;
        return res;
    }

    private bool GetCreateFoldersForBoxes()
    {
        var res = false;
        if (chkBoxFolders.InvokeRequired)
            chkBoxFolders.Invoke(() => res = chkBoxFolders.Checked);
        else
            res = chkBoxFolders.Checked;
        return res;
    }

    private string GetPath()
    {
        var str = "";
        if (txtBoxPath.InvokeRequired)
            txtBoxPath.Invoke(() => str = txtBoxPath.Text);
        else
            str = txtBoxPath.Text;
        return str;
    }

    private void UpdateProgressBar(int n)
    {
        if (progressBar.InvokeRequired)
            progressBar.Invoke(() => progressBar.Value = n);
        else
            progressBar.Value = n;
    }

    private void BtnBrowse_Click(object sender, EventArgs e)
    {
        if (folderBrowser.ShowDialog() is DialogResult.OK)
            txtBoxPath.Text = folderBrowser.SelectedPath;
    }

    private async void BtnConnect_Click(object sender, EventArgs e)
    {
        if (!radioWiFi.Checked && !radioUSB.Checked)
        {
            MessageBox.Show(Strings["Warning.ConnectionMethod"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else if (txtBoxIP.Enabled && String.IsNullOrWhiteSpace(txtBoxIP.Text))
        {
            MessageBox.Show(Strings["Warning.IpAddress"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else if (numPort.Value == 0)
        {
            MessageBox.Show(Strings["Warning.Port"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else if (!radioBox.Checked && !radioSlot.Checked && !radioTargetAll.Checked)
        {
            MessageBox.Show(Strings["Warning.DumpTarget"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else if (!radioEncrypted.Checked && !radioEncAndDec.Checked && !radioDecrypted.Checked)
        {
            MessageBox.Show(Strings["Warning.DumpFormat"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else if (string.IsNullOrWhiteSpace(txtBoxPath.Text) || !Directory.Exists(txtBoxPath.Text))
        {
            MessageBox.Show(Strings["Warning.BoxPath"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Properties.Settings.Default.USB = radioUSB.Checked;
        Properties.Settings.Default.ip_address = txtBoxIP.Text;
        Properties.Settings.Default.port = (int)numPort.Value;
        Properties.Settings.Default.path = txtBoxPath.Text;
        Properties.Settings.Default.Save();

        DisableControls();
        await ConnectDumper().ConfigureAwait(false);
    }

    public SwitchProtocol GetProtocol()
    {
        if (radioUSB.Checked)
            return SwitchProtocol.USB;
        else
            return SwitchProtocol.WiFi;
    }

    public DumpTarget GetDumpTarget()
    {
        if (radioTargetAll.Checked)
            return DumpTarget.TargetAll;
        else if (radioSlot.Checked)
            return DumpTarget.TargetSlot;
        else
            return DumpTarget.TargetBox;
    }

    public DumpFormat GetDumpFormat()
    {
        if (radioEncAndDec.Checked)
            return DumpFormat.EncAndDec;
        else if (radioDecrypted.Checked)
            return DumpFormat.Decrypted;
        else
            return DumpFormat.Encrypted;
    }

    private void DecryptFromFiles_Click(object sender, EventArgs e)
    {
        var filter = "All files (*.*)|*.*";
        foreach (var ext in PokeHandler.CompatibleFormats)
            if (ext[1] == 'e')
                filter += $"|Encrypted {ext[1..]}|*{ext}";

        openFileDialog.Filter = filter;
        openFileDialog.Multiselect = true;

        EncryptOrDecrypt(DumpFormat.Decrypted);
    }

    private void EncryptFromFiles_Click(object sender, EventArgs e)
    {
        var filter = "All files (*.*)|*.*";
        foreach (var ext in PokeHandler.CompatibleFormats)
            if (ext[1] == 'p')
                filter += $"|Decrypted {ext[1..]}|*{ext}";

        openFileDialog.Filter = filter;
        openFileDialog.Multiselect = true;

        EncryptOrDecrypt(DumpFormat.Encrypted);
    }

    private void EncryptOrDecrypt(DumpFormat format)
    {
        if (openFileDialog.ShowDialog() is DialogResult.OK)
        {
            var pkhlist = new List<Core.HomeWrapper?>();
            foreach (var path in openFileDialog.FileNames)
                if (File.Exists(path))
                    if (PokeHandler.IsCompatibleExtension(Path.GetExtension(path)))
                        pkhlist.Add(PokeHandler.GenerateEntityFromPath(path));

            if (pkhlist.Count > 0)
                if (folderBrowser.ShowDialog() is DialogResult.OK)
                    if (Directory.Exists(folderBrowser.SelectedPath))
                        foreach (var pkh in pkhlist)
                            pkh?.Dump(format, folderBrowser.SelectedPath);

            MessageBox.Show($"{Strings["Action.Completed"]}", "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void LoadFileToEditor_Click(object sender, EventArgs e)
    {
        var filter = "All files (*.*)|*.*";
        foreach (var ext in PokeHandler.CompatibleFormats)
            filter += $"{(ext[1] == 'p' ? $"|Decrypted" : "|Encrypted")} {ext[1..]}|*{ext}";

        openFileDialog.Filter = filter;
        openFileDialog.Multiselect = false;

        if (openFileDialog.ShowDialog() is DialogResult.OK)
            TryLoadFile(openFileDialog.FileName);
    }

    private void LoadFilesToBoxes_Click(object sender, EventArgs e)
    {
        var filter = "All files (*.*)|*.*";
        foreach (var ext in PokeHandler.CompatibleFormats)
            filter += $"{(ext[1] == 'p' ? $"|Decrypted" : "|Encrypted")} {ext[1..]}|*{ext}";

        openFileDialog.Filter = filter;
        openFileDialog.Multiselect = true;

        if (openFileDialog.ShowDialog() is DialogResult.OK)
            TryLoadFiles(openFileDialog.FileNames);
    }

    private void LoadFolderToBoxes_Click(object sender, EventArgs e)
    {
        if (folderBrowser.ShowDialog() is DialogResult.OK)
            TryLoadFiles(folderBrowser.SelectedPath);
    }

    public bool TryLoadFile(string path) => TryLoadFile(path, Provider, Editor);
    public bool TryLoadFiles(string path) => TryLoadFiles(path, Provider);
    public bool TryLoadFiles(string[] path) => TryLoadFiles("", Provider, path);

    public static bool TryLoadFile(string path, ISaveFileProvider provider, IPKMView editor)
    {
        if (Directory.Exists(path))
            return TryLoadFiles(path, provider);

        if (File.Exists(path))
        {
            var ext = Path.GetExtension(path);
            if (PokeHandler.IsCompatibleExtension(ext))
            {
                var pkm = PokeHandler.GenerateEntityFromPath(path);
                return provider.SAV.SetPoke(editor, pkm, ConversionType.AnyData, Properties.Settings.Default.fix_legality);
            }
        }
        return false;
    }

    public static bool TryLoadFiles(string path, ISaveFileProvider provider, string[]? filelist = null)
    {
        var pkmlist = new List<Core.HomeWrapper?>();

        if (filelist is null && Directory.Exists(path))
            filelist = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).ToArray();

        if (filelist is not null && filelist.Length > 0)
            foreach (var file in filelist)
                if (PokeHandler.IsCompatibleExtension(Path.GetExtension(file)))
                    pkmlist.Add(PokeHandler.GenerateEntityFromPath(file));

        if (pkmlist.Count > 0)
        {
            provider.SAV.SetPokeList(pkmlist, ConversionType.AnyData, Properties.Settings.Default.fix_legality);
            provider.ReloadSlotsSafe();
            return true;
        }

        return false;
    }

    private async Task ConnectDumper()
    {
        var targets = GetDumpTarget();
        var format = GetDumpFormat();
        var token = TokenSource.Token;

        try
        {
            var config = GetProtocol() switch
            {
                SwitchProtocol.USB => new SwitchConnectionConfig { Port = (int)numPort.Value, Protocol = SwitchProtocol.USB },
                _ => new SwitchConnectionConfig { IP = txtBoxIP.Text, Port = (int)numPort.Value, Protocol = SwitchProtocol.WiFi }
            };
            var state = new DeviceState
            {
                Connection = config,
                InitialRoutine = RoutineType.ReadWrite,
            };

            Executor = new DeviceExecutor<DeviceState>(state);
            await Executor.RunAsync(token).ConfigureAwait(false);

            if (targets is DumpTarget.TargetSlot)
            {
                var box = GetTargetBox();
                var slot = GetTargetSlot();
                await Executor.Connect(token).ConfigureAwait(false);
                var data = await Executor.ReadSlotData(box, slot, token).ConfigureAwait(false);
                var pkh = PokeHandler.GenerateEntityFromBin(data);
                pkh?.Dump(format, GetPath());
                UpdateProgressBar(95);
            }
            else if (targets is DumpTarget.TargetBox)
            {
                var box = GetTargetBox();
                await Executor.Connect(token).ConfigureAwait(false);
                var data = await Executor.ReadBoxData(box, token).ConfigureAwait(false);
                var entities = PokeHandler.GenerateEntitiesFromBoxBin(data);
                foreach (var entity in entities)
                    entity?.Dump(format, GetPath());
                UpdateProgressBar(95);
            }
            else if (targets is DumpTarget.TargetAll)
            {
                await Executor.Connect(token).ConfigureAwait(false);
                for (var box = 0; box < HomeDataOffsets.HomeBoxCount; box++)
                {
                    var path = GetCreateFoldersForBoxes() ? Path.Combine(GetPath(), $"{Strings["Word.Box"]} {box + 1:000}") : GetPath();
                    var data = await Executor.ReadBoxData(box, token).ConfigureAwait(false);
                    var entities = PokeHandler.GenerateEntitiesFromBoxBin(data);
                    foreach (var entity in entities)
                        entity?.Dump(format, path);

                    var progress = box * 100 / HomeDataOffsets.HomeBoxCount;
                    UpdateProgressBar(progress);
                }
            }

            Executor.Disconnect();
            UpdateProgressBar(100);
            MessageBox.Show($"{Strings["Action.Completed"]}", "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Information);

            EnableControls();
        }
        catch (Exception ex)
        {
            Executor.Disconnect();
            MessageBox.Show($"{Strings["Warning.SomethingWrong"]}{Environment.NewLine}{Strings["Warning.CheckInstallation"]}" +
                $"{Environment.NewLine}{Environment.NewLine}{ex.Message}", "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            EnableControls();
            UpdateProgressBar(100);
        }
    }
}