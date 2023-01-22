using System;
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
            {
                RadioConvertForce.Enabled = false;
                RadioConvertAny.Enabled = false;
            }
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
            else if(!RadioConvertForce.Checked && !RadioConvertSpecific.Checked && !RadioConvertAny.Checked)
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
            GrpForceConv.Enabled = false;
            GrpWarning.Enabled = false;

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
                GrpForceConv.Enabled = true;
                GrpWarning.Enabled = true;
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

        public int GetBoxIndex()
        {
            if (ComboBox.InvokeRequired)
            {
                var res = 0;
                ComboBox.Invoke(() => { res = ComboBox.SelectedIndex; });
                return res;
            }
            else
                return ComboBox.SelectedIndex;
        }

        public bool IsLastIndex() => ComboBox.SelectedIndex+1 == ComboBox.Items.Count;

        public string GetIP() => TxtAddress.Text;
        public int GetPort() => (int)UInt32.Parse(TxtPort.Text);

        public ConversionType GetConversionType()
        {
            if (RadioConvertForce.Checked)
                return ConversionType.CompatibleData;
            else if (RadioConvertAny.Checked)
                return ConversionType.AnyData;
            else
                return ConversionType.SpecificData;
        }

        public int GetCurrentProgress() => ProgressBar.Value;

        public void WriteLog(string str)
        {
            if(TxtLog.InvokeRequired)
                TxtLog.Invoke(() => { TxtLog.Text = str; });
            else 
                TxtLog.Text = str;
        }

        private void ChkWarning_Click(object sender, EventArgs e)
        {
            string warning = $"WARNING/DISCLOSURE:\n" +
                $"PKHeX simulates a conversion from the Pokémon HOME data format (PH1) to standard PKM file formats based on the current loaded save file.\n" +
                $"This process is unofficial and there is always the chance that it does not accurately replicate an official transfer.\n" +
                $"If you proceed with this tool, you accept the following:\n" +
                $"- The PKM files from the conversion are NOT legitimate in any way, even if the original encounter was.\n" +
                $"- The resulting files from the conversion may not even be legal in some circumstances.\n" +
                $"- When using 'Convert any PKM data' methods, it is likely that the resulting Pokémon will be illegal.\n" +
                $"- Do NOT use converted PKM in online battles/trades.\n" +
                $"- Do NOT use converted files to report legality issues, whether in the Project Pokémon forums/Discord or in the PKHeX Development Projects Discord.\n" +
                $"- This Plugin is intended for research, learning, and entertainment purposes.\n" +
                $"- This Plugin is not developed by the PKHeX Development Projects server, so do NOT report problems or request support there. Use the Project Pokémon thread instead.\n" +
                $"- The creators of this tool are not responsible for any adverse outcomes or side effects of using this tool.\n" +
                $"\nIf you agree with the above, click the 'Yes' button. Click 'No' otherwise.";

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
