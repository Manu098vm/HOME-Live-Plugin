using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace HOME
{
    public partial class ViewForm : Form
    {
        HOME PluginInstance = null!;
        public ViewForm(HOME instance)
        {
            InitializeComponent();
            PluginInstance = instance;

            if ((bool)Properties.Settings.Default["WiFi"])
                RadioWiFi.Select();
            else if ((bool)Properties.Settings.Default["USB"])
                RadioUSB.Select();

            TxtAddress.Text = Properties.Settings.Default["IP"].ToString();
            TxtPort.Text = Properties.Settings.Default["Port"].ToString();

            ComboBox.Items.AddRange(instance.CalculateBoxes());
            ComboBox.SelectedIndex = 0;

            if (instance.GetNonForceableConvSaveFile())
                RadioConvertForce.Enabled = false;
        }

        private void RadioWiFi_CheckedChanged(object sender, EventArgs e)
        {
            TxtAddress.Enabled = true;
        }

        private void RadioUSB_CheckedChanged(object sender, EventArgs e)
        {
            TxtAddress.Enabled = false;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (!RadioWiFi.Checked && !RadioUSB.Checked)
            {
                TxtLog.Text = "Select the connection method.";
                return;
            }
            else if (TxtAddress.Enabled && string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                TxtLog.Text = "Insert a proper IP Address.";
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtPort.Text) || CheckPortTxt())
            {
                TxtLog.Text = "Insert a proper Port.";
                return;
            }
            else if(!RadioConvertForce.Checked && !RadioConvertSpecific.Checked)
            {
                TxtLog.Text = "Select the conversion method.";
                return;
            }

            Properties.Settings.Default["WiFi"] = RadioWiFi.Checked;
            Properties.Settings.Default["USB"] = RadioUSB.Checked;
            Properties.Settings.Default["IP"] = TxtAddress.Text;
            Properties.Settings.Default["Port"] = UInt32.Parse(TxtPort.Text);
            Properties.Settings.Default.Save();

            TxtLog.Text = "Connecting....";
            GrpConnection.Enabled = false;
            ComboBox.Enabled = false;
            BtnConnect.Enabled = false;

            BackGroundWorker.RunWorkerAsync();
        }

        private void BackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PluginInstance.StartViewer(this, sender as BackgroundWorker);
        }

        private void BackGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void BackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                GrpConnection.Enabled = true;
                ComboBox.Enabled = true;
                BtnConnect.Enabled = true;
            }
        }

        private bool CheckPortTxt()
        {
            try
            {
                return Int64.Parse(TxtPort.Text) < 0 || UInt32.Parse(TxtPort.Text) > 65535;
            }
            catch
            {
                return true;
            }
        }

        public ConnectionType GetConnectionType()
        {
            if (RadioUSB.Checked)
                return ConnectionType.USB;
            else
                return ConnectionType.WiFi;
        }

        public int GetBoxIndex() => ComboBox.SelectedIndex;

        public bool IsLastIndex() => ComboBox.SelectedIndex+1 == ComboBox.Items.Count;

        public string GetIP() => TxtAddress.Text;
        public int GetPort() => (int)UInt32.Parse(TxtPort.Text);

        public bool GetForceConversion() => RadioConvertForce.Checked;

        public int GetCurrentProgress() => ProgressBar.Value;
        public void WriteLog(string str) => TxtLog.Text = str;

        private void ChkWarning_Click(object sender, EventArgs e)
        {
            string warning = $"WARNING DISCLOSURE:\n" +
                $"PKHeX will try to simulate a conversion from Pokémon Home data format (PH1) into standard PKM file format based on the current Save File type.\n" +
                $"The process is still under development, and there is the chance it could never be as accurate as transfers with official approved tools.\n" +
                $"If you proceed with the tool, you declare to comprehend and agree with the following statements:\n" +
                $"- The PKM files from the conversion are NOT legitimate by any way, even if the original encounter was.\n" +
                $"- The resulting files from the conversion might not even be legal in some circumstances.\n" +
                $"- If using the 'Convert any PKM data if compatible with save file' process, it's more likely that the resulted Pokémon won't be legal.\n" +
                $"- Do NOT use these PKM in online battles/trades.\n" +
                $"- Do NOT use these files to report legality issues, nor in the Project Pokémon forums/discord nor in the PKHeX Dev discord.\n" +
                $"- The Viewer goal is for reserch, learning and entertainment purposes.\n" +
                $"- This Plugin is not developed by the PKHeX Dev discord staff, thus do NOT report problems or bug to them. Use the Project Pokémon thread instead.\n" +
                $"\nIf you're willing to agree with the above, click the 'Yes' button. Click 'No' otherwise.";

            DialogResult disclaimer = MessageBox.Show(warning, "Disclaimer", MessageBoxButtons.YesNo);
            if (disclaimer == DialogResult.Yes)
            {
                ChkWarning.Checked = true;
                BtnConnect.Enabled = true;
            }
            else if (disclaimer == DialogResult.No)
            {
                ChkWarning.Checked = false;
                BtnConnect.Enabled = false;
            }
        }
    }
}
