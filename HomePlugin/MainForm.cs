using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace HOME
{
    public partial class MainForm : Form
    {

        HOME PluginInstance = null!;

        public MainForm(HOME instance)
        {
            InitializeComponent();
            PluginInstance = instance;

            BtnReset.Visible = false;

            for (int i = 1; i <= 200; i++)
                ComboBox.Items.Add($"Box {i}");
            for (int i = 1; i <= 30; i++)
                ComboSlot.Items.Add($"Slot {i}");
            ComboBox.SelectedIndex = 0;
            ComboSlot.SelectedIndex = 0;

            if ((bool)Properties.Settings.Default["WiFi"])
                RadioWiFi.Select();
            else if ((bool)Properties.Settings.Default["USB"])
                RadioUSB.Select();

            TxtBoxIP.Text = Properties.Settings.Default["IP"].ToString();
            TxtBoxPort.Text = Properties.Settings.Default["Port"].ToString();

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
            else if(!RadioBox.Checked && !RadioSlot.Checked && !RadioTargetAll.Checked)
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
            Properties.Settings.Default["Path"] = TxtBoxPath.Text;
            Properties.Settings.Default.Save();

            TxtBoxLog.Text = "Connecting....";
            GrpConnection.Enabled = false;
            GrpAction.Enabled = false;
            GrpDump.Enabled = false;
            GrpPath.Enabled = false;
            BtnConnect.Enabled = false;
            toolsToolStripMenuItem.Enabled = false;

            BackgroundWorker.RunWorkerAsync();
        }

        private void DecryptFromFiles_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                    BackGroundWorkerLocal.RunWorkerAsync();
        }

        private void BackGroundWorkerLocal_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = sender as BackgroundWorker;
            if (bgWorker != null)
            {
                var i = 0;
                foreach (String file in OpenFileDialog.FileNames)
                {
                    PluginInstance.ProcessLocal(this, bgWorker, file, SaveFileDialog.SelectedPath.ToString());
                    i++;
                    TxtBoxLog.Text = $"Loading [{i}] file(s).";
                    bgWorker.ReportProgress(6000 / OpenFileDialog.FileNames.Length * i);
                }
                bgWorker.ReportProgress(6000);
                TxtBoxLog.Text = $"Process completed. [{i}] file(s) elaborated.";
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PluginInstance.ProcessRemote(this, sender as BackgroundWorker);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled == false)
            {
                GrpConnection.Enabled = true;
                GrpAction.Enabled = true;
                GrpDump.Enabled = true;
                GrpPath.Enabled = true;
                BtnConnect.Enabled = true;
                toolsToolStripMenuItem.Enabled = true;
            }
        }

        private void MainForm_Close(object sender, EventArgs e)
        {
            BackgroundWorker.CancelAsync();
        }

        private void RadioUSB_CheckedChanged(object sender, EventArgs e)
        {
            TxtBoxIP.Enabled = false;
        }

        private void RadioWiFi_CheckedChanged(object sender, EventArgs e)
        {
            TxtBoxIP.Enabled = true;
        }

        private void RadioBox_CheckedChanged(object sender, EventArgs e)
        {
            ComboBox.Enabled = true;
            ComboSlot.Enabled = false;
        }

        private void RadioSlot_CheckedChanged(object sender, EventArgs e)
        {
            ComboBox.Enabled = true;
            ComboSlot.Enabled = true;
        }

        private void RadioTargetAll_CheckedChanged(object sender, EventArgs e)
        {
            ComboBox.Enabled = false;
            ComboSlot.Enabled = false;
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
            RadioBox.Checked = false;
            RadioSlot.Checked = false;
            RadioTargetAll.Checked = false;
            RadioEncrypted.Checked = false;
            RadioDecrypted.Checked = false;
            RadioEncAndDec.Checked = false;
            TxtBoxIP.Enabled = true;
            ComboBox.SelectedIndex = 0;
            ComboSlot.SelectedIndex = 0;
            TxtBoxIP.Text = "";
            TxtBoxPort.Text = "";
            TxtBoxPath.Text = "";
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
        public int GetTargetBox() => ComboBox.SelectedIndex;
        public int GetTargetSlot() => ComboSlot.SelectedIndex;
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
            else if (RadioSlot.Checked)
                return DumpTarget.TargetSlot;
            else
                return DumpTarget.TargetBox;
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
