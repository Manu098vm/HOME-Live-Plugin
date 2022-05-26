using System;
using System.Windows.Forms;

namespace HOME
{
    public partial class MainForm : Form
    {

        HOME ConnectInstance = null!;

        public MainForm(HOME instance)
        {
            InitializeComponent();

            ConnectInstance = instance;

            if ((bool)Properties.Settings.Default["WiFi"])
                RadioWiFi.Select();
            else if ((bool)Properties.Settings.Default["USB"])
                RadioUSB.Select();

            TxtBoxIP.Text = Properties.Settings.Default["IP"].ToString();
            TxtBoxPort.Text = Properties.Settings.Default["Port"].ToString();

            if ((bool)Properties.Settings.Default["B1S1"])
                RadioB1S1.Select();
            else if ((bool)Properties.Settings.Default["DumpAll"])
                RadioTargetAll.Select();

            if ((bool)Properties.Settings.Default["Encrypted"])
                RadioEncrypted.Select();
            else if ((bool)Properties.Settings.Default["Decrypted"])
                RadioDecrypted.Select();
            else if ((bool)Properties.Settings.Default["Both"])
                RadioEncAndDec.Select();

            TxtBoxPath.Text = Properties.Settings.Default["Path"].ToString();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (!RadioWiFi.Checked && !RadioUSB.Checked)
            {
                TxtBoxLog.Text = "Select the connection method.";
                return;
            }
            else if (TxtBoxIP.Enabled && string.IsNullOrWhiteSpace(TxtBoxIP.Text))
            {
                TxtBoxLog.Text = "Insert a proper IP Address.";
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtBoxPort.Text) || CheckPortTxt(TxtBoxPort.Text))
            {
                TxtBoxLog.Text = "Insert a proper Port.";
                return;
            }
            else if(!RadioB1S1.Checked && !RadioTargetAll.Checked)
            {
                TxtBoxLog.Text = "Select the dump Target.";
                return;
            }
            else if(!RadioEncrypted.Checked && !RadioDecrypted.Checked && !RadioEncAndDec.Checked)
            {
                TxtBoxLog.Text = "Select the dump Format.";
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtBoxPath.Text))
            {
                TxtBoxLog.Text = "Insert a proper IP Address.";
                return;
            }

            Properties.Settings.Default["WiFi"] = RadioWiFi.Checked;
            Properties.Settings.Default["USB"] = RadioUSB.Checked;
            Properties.Settings.Default["IP"] = TxtBoxIP.Text;
            Properties.Settings.Default["Port"] = UInt32.Parse(TxtBoxPort.Text);
            Properties.Settings.Default["B1S1"] = RadioB1S1.Checked;
            Properties.Settings.Default["DumpAll"] = RadioTargetAll.Checked;
            Properties.Settings.Default["Encrypted"] = RadioEncrypted.Checked;
            Properties.Settings.Default["Decrypted"] = RadioDecrypted.Checked;
            Properties.Settings.Default["Both"] = RadioEncAndDec.Checked;
            Properties.Settings.Default["Path"] = TxtBoxPath.Text;
            Properties.Settings.Default.Save();

            TxtBoxLog.Text = "Connecting....";

            ConnectInstance.StartProcess(this);
        }

        private void RadioUSB_CheckedChanged(object sender, EventArgs e)
        {
            TxtBoxIP.Enabled = false;
        }

        private void RadioWiFi_CheckedChanged(object sender, EventArgs e)
        {
            TxtBoxIP.Enabled = true;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                TxtBoxPath.Text = FolderBrowser.SelectedPath;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            RadioWiFi.Checked = false;
            RadioUSB.Checked = false;
            RadioB1S1.Checked = false;
            RadioTargetAll.Checked = false;
            RadioEncrypted.Checked = false;
            RadioDecrypted.Checked = false;
            RadioEncAndDec.Checked = false;
            TxtBoxIP.Enabled = true;
            TxtBoxIP.Text = "";
            TxtBoxPort.Text = "";
            TxtBoxLog.Text = "Reset done.";
        }

        private bool CheckPortTxt(string str)
        {
            try
            {
                return Int64.Parse(TxtBoxPort.Text) < 0 || UInt32.Parse(TxtBoxPort.Text) > 65535;
            }
            catch
            {
                return true;
            }
        }

        public string GetIP() => TxtBoxIP.Text;
        public int GetPort() => (int)UInt32.Parse(TxtBoxPort.Text);
        public string GetPath() => TxtBoxPath.Text;
        public ConnectionType GetConnectionType()
        {
            if (RadioUSB.Checked)
                return ConnectionType.USB;
            else
                return ConnectionType.WiFi;
        }
        public DumpTarget GetDumpTarget()
        {
            if (RadioTargetAll.Checked)
                return DumpTarget.TargetAll;
            else
                return DumpTarget.B1S1;
        }
        public DumpFormat GetWantedFormats()
        {
            if (RadioEncAndDec.Checked)
                return DumpFormat.EncAndDec;
            else if (RadioDecrypted.Checked)
                return DumpFormat.Decrypted;
            else
                return DumpFormat.Encrypted;
        }
        public void WriteLog(string str) => TxtBoxLog.Text = str;
        public void AppendLog(string str) => TxtBoxLog.AppendText(str);
    }
}
