using PKHeX.Core;
using HomeLive.Core;
using SysBot.Base;
using System.Buffers.Binary;

namespace HomeLive.Plugins;

public partial class ViewerForm : Form
{
    private DeviceExecutor<DeviceState> Executor = null!;
    private Dictionary<string, string> Strings = null!;

    private readonly ISaveFileProvider Provider = null!;
    private readonly IPKMView Editor = null!;

    private readonly CancellationTokenSource TokenSource = new();

    public ViewerForm(ISaveFileProvider provider, IPKMView editor, string language)
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

        radioSBox.Select();
        comboBox.Items.AddRange(CalculateBoxStrings());
        comboBox.SelectedIndex = 0;

        for (var i = 0; i < HomeDataOffsets.HomeBoxCount; i++)
            cmbBox.Items.Add($"{Strings["Word.Box"]} {i + 1}");
        for (var i = 0; i < HomeDataOffsets.HomeSlotCount; i++)
            cmbSlot.Items.Add($"{Strings["Word.Slot"]} {i + 1}");

        cmbBox.SelectedIndex = 0;
        cmbSlot.SelectedIndex = 0;
    }

    private void TxtBoxPort_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            e.Handled = true;
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "Warning.ConnectionMethod", "" },
            { "Warning.IpAddress", "" },
            { "Warning.Port", "" },
            { "Warning.Conversion", "" },
            { "Warning.SomethingWrong", "" },
            { "Warning.CheckInstallation", "" },
            { "Warning.Name1", "" },
            { "Warning.Name2", "" },
            { "Warning.Line1", "" },
            { "Warning.Line2", "" },
            { "Warning.Line3", "" },
            { "Warning.Line4", "" },
            { "Warning.Line5", "" },
            { "Warning.Line6", "" },
            { "Warning.Line7", "" },
            { "Word.Box", "" },
            { "Word.Slot", "" },
            { "Action.Completed", "" },
            { "ViewerForm.grpBox", "" },
            { "ViewerForm.grpSlot", "" },
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

    private void RadioWiFi_CheckedChanged(object sender, EventArgs e) => txtBoxIP.Enabled = radioWiFi.Checked;
    private void RadioUSB_CheckedChanged(object sender, EventArgs e) => txtBoxIP.Enabled = !radioUSB.Checked;

    private void RadioBox_CheckedChanged(object sender, EventArgs e)
    {
        grpSlot.Enabled = grpSlot.Visible = !radioBox.Checked;
        grpBoxes.Enabled = grpBoxes.Visible = radioBox.Checked;
    }

    private void RadioSBox_CheckedChanged(object sender, EventArgs e)
    {
        grpSlot.Text = Strings["ViewerForm.grpBox"];
        grpSlot.Enabled = grpSlot.Visible = radioSBox.Checked;
        cmbSlot.Enabled = grpSlot.Visible && !radioSBox.Checked;
        grpBoxes.Enabled = grpBoxes.Visible = !radioSBox.Checked;
    }

    private void RadioSlot_CheckedChanged(object sender, EventArgs e)
    {
        grpSlot.Text = Strings["ViewerForm.grpSlot"];
        grpSlot.Enabled = grpSlot.Visible = radioSlot.Checked;
        cmbSlot.Enabled = grpSlot.Visible && radioSlot.Checked;
        grpBoxes.Enabled = grpBoxes.Visible = !radioSlot.Checked;
    }

    private void UpdateProgressBar(int n)
    {
        if (progressBar.InvokeRequired)
            progressBar.Invoke(() => progressBar.Value = n);
        else
            progressBar.Value = n;
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
        else if (!radioConvertAny.Checked && !radioConvertSpecific.Checked && !radioConvertForce.Checked)
        {
            MessageBox.Show(Strings["Warning.Conversion"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Properties.Settings.Default.USB = radioUSB.Checked;
        Properties.Settings.Default.ip_address = txtBoxIP.Text;
        Properties.Settings.Default.port = (int)numPort.Value;
        Properties.Settings.Default.Save();

        var warning = $"{Strings["Warning.Name1"]}{Environment.NewLine}" +
                $"{Environment.NewLine}{Strings["Warning.Line1"]}{Environment.NewLine}" +
                $"{Strings["Warning.Line2"]}{Environment.NewLine}" +
                $"{Environment.NewLine}{Strings["Warning.Line3"]}{Environment.NewLine}" +
                $"{Strings["Warning.Line4"]}{Environment.NewLine}" +
                $"{Strings["Warning.Line5"]}{Environment.NewLine}" +
                $"{Strings["Warning.Line6"]}{Environment.NewLine}" +
                $"{Environment.NewLine}{Strings["Warning.Line7"]}";

        var disclaimer = MessageBox.Show(warning, Strings["Warning.Name2"], MessageBoxButtons.YesNo);
        if (disclaimer is not DialogResult.Yes)
            return;

        await ConnectViewer().ConfigureAwait(false);
    }

    private int GetComboBoxIndex()
    {
        var res = 0;
        if (comboBox.InvokeRequired)
            comboBox.Invoke(() => { res = comboBox.SelectedIndex; });
        else
            res = comboBox.SelectedIndex;
        return res;
    }

    private int GetBoxIndex()
    {
        var index = 0;
        if (cmbBox.InvokeRequired)
            cmbBox.Invoke(() => index = cmbBox.SelectedIndex);
        else
            index = cmbBox.SelectedIndex;
        return index;
    }

    private int GetSlotIndex()
    {
        var index = 0;
        if (cmbSlot.InvokeRequired)
            cmbSlot.Invoke(() => index = cmbSlot.SelectedIndex);
        else
            index = cmbSlot.SelectedIndex;
        return index;
    }

    private void DisableControls()
    {
        if (grpConnection.InvokeRequired)
            grpConnection.Invoke(() => grpConnection.Enabled = false);
        else
            grpConnection.Enabled = false;

        if (btnConnect.InvokeRequired)
            btnConnect.Invoke(() => btnConnect.Enabled = false);
        else
            btnConnect.Enabled = false;

        if (grpForceConv.InvokeRequired)
            grpForceConv.Invoke(() => grpForceConv.Enabled = false);
        else
            grpForceConv.Enabled = false;

        if (grpSelectionType.InvokeRequired)
            grpSelectionType.Invoke(() => grpSelectionType.Enabled = false);
        else
            grpSelectionType.Enabled = false;

        if (grpBoxes.InvokeRequired)
            grpBoxes.Invoke(() => grpBoxes.Enabled = false);
        else
            grpBoxes.Enabled = false;

        if (grpSlot.InvokeRequired)
            grpSlot.Invoke(() => grpSlot.Enabled = false);
        else
            grpSlot.Enabled = false;
    }

    private void EnableControls()
    {
        if (grpConnection.InvokeRequired)
            grpConnection.Invoke(() => grpConnection.Enabled = true);
        else
            grpConnection.Enabled = true;

        if (btnConnect.InvokeRequired)
            btnConnect.Invoke(() => btnConnect.Enabled = true);
        else
            btnConnect.Enabled = true;

        if (grpForceConv.InvokeRequired)
            grpForceConv.Invoke(() => grpForceConv.Enabled = true);
        else
            grpForceConv.Enabled = true;

        if (grpSelectionType.InvokeRequired)
            grpSelectionType.Invoke(() => grpSelectionType.Enabled = true);
        else
            grpSelectionType.Enabled = true;

        if (grpBoxes.InvokeRequired)
            grpBoxes.Invoke(() => grpBoxes.Enabled = radioBox.Checked);
        else
            grpBoxes.Enabled = radioBox.Checked;

        if (grpSlot.InvokeRequired)
            grpSlot.Invoke(() => grpSlot.Enabled = radioSlot.Checked || radioSBox.Checked);
        else
            grpSlot.Enabled = radioSlot.Checked || radioSBox.Checked;
    }

    private string[] CalculateBoxStrings()
    {
        var version = Provider.SAV.GetGameVersion();
        var localSlotCount = version.GetSlotCount();
        var localBoxCount = version.GetBoxCount();

        var tuples = GetDownloadRanges(localBoxCount, localSlotCount);
        var strings = new string[tuples.Count];

        foreach (var (el, i) in tuples.Select((el, i) => (el, i)))
            strings[i] = $"{Strings["Word.Box"]} {el.start.box}{(el.start.slot > 1 ? $" {Strings["Word.Slot"]} {el.start.slot}" : "")} " +
                $"- {Strings["Word.Box"]} {el.end.box} {(el.end.slot < 30 ? $"{Strings["Word.Slot"]} {el.end.slot}" : "")}";

        return strings;
    }

    private static List<((int box, int slot) start, (int box, int slot) end)> GetDownloadRanges(int localBoxes, int localSlots)
    {
        var ranges = new List<((int, int), (int, int))>();

        var homeBoxes = HomeDataOffsets.HomeBoxCount;
        var homeSlots = HomeDataOffsets.HomeSlotCount;

        var totalSlots = localBoxes * localSlots;
        var startSlot = 1;
        var endSlot = totalSlots;

        while (startSlot <= homeSlots * homeBoxes)
        {
            var startBox = (startSlot - 1) / homeSlots + 1;
            var endBox = (endSlot - 1) / homeSlots + 1;

            if (endBox > homeBoxes)
            {
                endBox = homeBoxes;
                endSlot = homeBoxes * homeSlots;
            }

            ranges.Add(((startBox, (startSlot - 1) % homeSlots + 1), (endBox, (endSlot - 1) % homeSlots + 1)));

            startSlot = endSlot + 1;
            endSlot = startSlot + totalSlots - 1;
        }

        return ranges;
    }

    private SwitchProtocol GetProtocol()
    {
        if (radioUSB.Checked)
            return SwitchProtocol.USB;
        return SwitchProtocol.WiFi;
    }

    private ConversionType GetConversionType()
    {
        if (radioConvertAny.Checked)
            return ConversionType.AnyData;
        else if (radioConvertForce.Checked)
            return ConversionType.CompatibleData;
        else
            return ConversionType.SpecificData;
    }

    private DumpTarget GetTargetType()
    {
        if (radioSlot.Checked)
            return DumpTarget.TargetSlot;
        else if (radioSBox.Checked)
            return DumpTarget.TargetBox;
        else
            return DumpTarget.TargetAll;
    }

    private async Task ConnectViewer()
    {
        var token = TokenSource.Token;
        DisableControls();

        try
        {
            var config = GetProtocol() switch
            {
                SwitchProtocol.USB => new SwitchConnectionConfig { Port = (int)numPort.Value, Protocol = SwitchProtocol.USB },
                _ => new SwitchConnectionConfig { IP = txtBoxIP.Text, Port = (int)numPort.Value, Protocol = SwitchProtocol.WiFi },
            };
            var state = new DeviceState
            {
                Connection = config,
                InitialRoutine = RoutineType.ReadWrite
            };
            Executor = new DeviceExecutor<DeviceState>(state);
            await Executor.RunAsync(token).ConfigureAwait(false);

            var mode = GetTargetType();
            var conversionType = GetConversionType();

            await Executor.Connect(token);

            if(Provider.SAV.IsBlankSaveFile())
                Provider.SAV.OT = await Executor.ReadPlayerOTName(token).ConfigureAwait(false);

            if (mode is DumpTarget.TargetSlot)
            {
                var box = GetBoxIndex();
                var slot = GetSlotIndex();
                var data = await Executor.ReadSlotData(box, slot, token).ConfigureAwait(false);
                var pkh = PokeHandler.GenerateEntityFromBin(data);
                Provider.SAV.SetPoke(Editor, pkh, conversionType, Properties.Settings.Default.fix_legality);
            }
            else if (mode is DumpTarget.TargetBox)
            {
                var box = GetBoxIndex();
                var data = await Executor.ReadBoxData(box, token).ConfigureAwait(false);
                var pkhlist = PokeHandler.GenerateEntitiesFromBoxBin(data);
                Provider.SAV.SetPokeList(pkhlist, conversionType, Properties.Settings.Default.fix_legality);
                Provider.ReloadSlotsSafe();
            }
            else
            {
                var version = Provider.SAV.GetGameVersion();
                var localSlotCount = version.GetSlotCount();
                var localBoxCount = version.GetBoxCount();
                var (start, end) = GetDownloadRanges(localBoxCount, localSlotCount)[GetComboBoxIndex()];

                var range = end.box - start.box;
                var pkhlist = new List<PKH?>();

                for (var i = 0; i <= range; i++)
                {
                    var boxBin = await Executor.ReadBoxData(start.box + i - 1, token).ConfigureAwait(false);
                    var boxMons = PokeHandler.GenerateEntitiesFromBoxBin(boxBin);
                    if (i == end.box - 1 && end.slot != 0)
                        for (var j = 0; j < end.slot; j++)
                            pkhlist.Add(boxMons[j]);
                    else
                        pkhlist.AddRange(boxMons);

                    var value = i * 100 / range;
                    UpdateProgressBar(value);
                }

                Provider.SAV.SetPokeList(pkhlist, conversionType, Properties.Settings.Default.fix_legality);
                Provider.ReloadSlotsSafe();
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
            UpdateProgressBar(0);
        }
    }
}
