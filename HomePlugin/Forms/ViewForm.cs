using System;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace HOME
{
    public partial class ViewForm : Form
    {
        private HOME PluginInstance = null!;
        private Dictionary<string, string> Strings = null!;

        public ViewForm(HOME instance)
        {
            InitializeComponent();
            PluginInstance = instance;

            GenerateDictionary();
            TranslateDictionary(PluginInstance.Language);
            this.TranslateInterface(PluginInstance.Language);

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

        private void GenerateDictionary()
        {
            Strings = new Dictionary<string, string>
            {
                { "Action.Connecting", "" },
                { "Warning.ConnectionMethod", "" },
                { "Warning.IpAddress", "" },
                { "Warning.Port", "" },
                { "Warning.Conversion", "" },
                { "Warning.Name1", "" },
                { "Warning.Name2", "" },
                { "Warning.Line1", "" },
                { "Warning.Line2", "" },
                { "Warning.Line3", "" },
                { "Warning.Line4", "" },
                { "Warning.Line5", "" },
                { "Warning.Line6", "" },
                { "Warning.Line7", "" },
                { "Warning.Line8", "" },
                { "Warning.Line9", "" },
                { "Warning.Line10", "" },
                { "Warning.Line11", "" },
                { "Warning.Line12", "" },
            };
        }

        private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

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
                TxtLog.Text = Strings["Warning.ConnectionMethod"];
                return;
            }
            else if (TxtAddress.Enabled && string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                TxtLog.Text = Strings["Warning.IpAddress"];
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtPort.Text) || CheckPortTxt())
            {
                TxtLog.Text = Strings["Warning.Port"];
                return;
            }
            else if(!RadioConvertForce.Checked && !RadioConvertSpecific.Checked && !RadioConvertAny.Checked)
            {
                TxtLog.Text = Strings["Warning.Conversion"];
                return;
            }

            Properties.Settings.Default["WiFi"] = RadioWiFi.Checked;
            Properties.Settings.Default["USB"] = RadioUSB.Checked;
            Properties.Settings.Default["IP"] = TxtAddress.Text;
            Properties.Settings.Default["Port"] = UInt32.Parse(TxtPort.Text);
            Properties.Settings.Default.Save();

            TxtLog.Text = Strings["Action.Connecting"];
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
            string warning = $"{Strings["Warning.Name1"]}\n" +
                $"{Strings["Warning.Line1"]}\n" +
                $"{Strings["Warning.Line2"]}\n" +
                $"{Strings["Warning.Line3"]}\n" +
                $"{Strings["Warning.Line4"]}\n" +
                $"{Strings["Warning.Line5"]}\n" +
                $"{Strings["Warning.Line6"]}\n" +
                $"{Strings["Warning.Line7"]}\n" +
                $"{Strings["Warning.Line8"]}\n" +
                $"{Strings["Warning.Line9"]}\n" +
                $"{Strings["Warning.Line10"]}\n" +
                $"{Strings["Warning.Line11"]}\n" +
                $"\n{Strings["Warning.Line12"]}";

            DialogResult disclaimer = MessageBox.Show(warning, Strings["Warning.Name2"], MessageBoxButtons.YesNo);
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
