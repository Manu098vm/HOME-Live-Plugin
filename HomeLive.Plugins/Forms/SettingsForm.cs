namespace HomeLive.Plugins;

public partial class SettingsForm : Form
{
    private Dictionary<string, string> Strings = null!;

    public SettingsForm(string language)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(language);
        this.TranslateInterface(language);

        cmbProtocol.Items.Clear();
        cmbProtocol.Items.Add(Strings["SettingsForm.cmbProtocol.Item1"]);
        cmbProtocol.Items.Add(Strings["SettingsForm.cmbProtocol.Item2"]);

        cmbLegality.Items.Clear();
        cmbLegality.Items.Add(Strings["SettingsForm.cmbLegality.Item1"]);
        cmbLegality.Items.Add(Strings["SettingsForm.cmbLegality.Item2"]);

        txtBoxIP.Text = Properties.Settings.Default.ip_address;
        numPort.Value = Properties.Settings.Default.port;
        txtBoxPath.Text = Properties.Settings.Default.path;
        cmbProtocol.SelectedIndex = Properties.Settings.Default.USB ? 1 : 0;
        cmbLegality.SelectedIndex = Properties.Settings.Default.fix_legality ? 0 : 1;

        toolTipLegality.SetToolTip(lblLegality, Strings["SettingsForm.lblLegality.ToolTip"]);
        btnPath.Location = new Point(btnPath.Location.X, txtBoxPath.Location.Y);
        btnPath.Height = txtBoxPath.Height;
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "SettingsForm.cmbProtocol.Item1", "" },
            { "SettingsForm.cmbProtocol.Item2", "" },
            { "SettingsForm.cmbLegality.Item1", "" },
            { "SettingsForm.cmbLegality.Item2", "" },
            { "SettingsForm.btnApply.Done", "" },
            { "SettingsForm.lblLegality.ToolTip", "" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void TxtBoxIP_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || c == '.'))
            e.Handled = true;
    }

    private void BtnPath_Click(object sender, EventArgs e)
    {
        if (folderBrowser.ShowDialog() is DialogResult.OK)
            txtBoxPath.Text = folderBrowser.SelectedPath;
    }

    private void BtnApply_Click(object sender, EventArgs e)
    {
        Properties.Settings.Default.ip_address = txtBoxIP.Text;
        Properties.Settings.Default.port = (int)numPort.Value;
        Properties.Settings.Default.path = txtBoxPath.Text;
        Properties.Settings.Default.USB = cmbProtocol.SelectedIndex == 1;
        Properties.Settings.Default.fix_legality = cmbLegality.SelectedIndex == 0;
        Properties.Settings.Default.Save();

        MessageBox.Show(Strings["SettingsForm.btnApply.Done"], "HomeLive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e) => Close();
}