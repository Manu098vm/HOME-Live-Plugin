using System;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace HOME
{
    public partial class DumpForm : Form
    {
        private HOME PluginInstance = null!;
        private Dictionary<string, string> Strings = null!;

        public DumpForm(HOME instance)
        {
            InitializeComponent();
            PluginInstance = instance;

            GenerateDictionary();
            TranslateDictionary(PluginInstance.Language);
            this.TranslateInterface(PluginInstance.Language);

            for (int i = 1; i <= 200; i++)
                ComboBox.Items.Add($"{Strings["Word.Box"]} {i}");
            for (int i = 1; i <= 30; i++)
                ComboSlot.Items.Add($"{Strings["Word.Slot"]} {i}");
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

        private void GenerateDictionary()
        {
            Strings = new Dictionary<string, string>
            {
                { "Action.Connecting", "" },
                { "Warning.ConnectionMethod", "" },
                { "Warning.IpAddress", "" },
                { "Warning.Port", "" },
                { "Warning.Conversion", "" },
                { "Warning.DumpTarget", "" },
                { "Warning.DumpFormat", "" },
                { "Warning.BoxPath", "" },
                { "Action.Loading", "" },
                { "Action.Completed", "" },
                { "Action.Compatible", "" },
                { "Action.Files", "" },
                { "Action.Elaborated", "" },
                { "Warning.IncompatibleFile", "" },
                { "Warning.Name1", "" },
                { "Warning.Name2", "" },
                { "Warning.Line1", "" },
                { "Warning.Line2", "" },
                { "Warning.Line3", "" },
                { "Warning.Line4", "" },
                { "Warning.Line5", "" },
                { "Warning.Line6b", "" },
                { "Warning.Line7", "" },
                { "Warning.Line8", "" },
                { "Warning.Line9", "" },
                { "Warning.Line10", "" },
                { "Warning.Line11", "" },
                { "Warning.Line12", "" },
                { "Word.Box", "" },
                { "Word.Slot", "" },
            };
        }

        private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (!RadioWiFi.Checked && !RadioUSB.Checked)
            {
                WriteLog(Strings["Warning.ConnectionMethod"]);
                return;
            }
            else if (TxtBoxIP.Enabled && string.IsNullOrWhiteSpace(TxtBoxIP.Text))
            {
                WriteLog(Strings["Warning.IpAddress"]);
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtBoxPort.Text) || CheckPortTxt())
            {
                WriteLog(Strings["Warning.Port"]);
                return;
            }
            else if(!RadioBox.Checked && !RadioSlot.Checked && !RadioTargetAll.Checked)
            {
                WriteLog(Strings["Warning.DumpTarget"]);
                return;
            }
            else if(!RadioEncrypted.Checked && !RadioDecrypted.Checked && !RadioEncAndDec.Checked)
            {
                WriteLog(Strings["Warning.DumpFormat"]);
                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtBoxPath.Text))
            {
                WriteLog(Strings["Warning.BoxPath"]);
                return;
            }

            Properties.Settings.Default["WiFi"] = RadioWiFi.Checked;
            Properties.Settings.Default["USB"] = RadioUSB.Checked;
            Properties.Settings.Default["IP"] = TxtBoxIP.Text;
            Properties.Settings.Default["Port"] = UInt32.Parse(TxtBoxPort.Text);
            Properties.Settings.Default["Path"] = TxtBoxPath.Text;
            Properties.Settings.Default.Save();

            WriteLog(Strings["Action.Connecting"]);
            GrpConnection.Enabled = false;
            GrpAction.Enabled = false;
            GrpDump.Enabled = false;
            GrpPath.Enabled = false;
            BtnConnect.Enabled = false;
            toolsToolStripMenuItem.Enabled = false;
            ChkBoxFolders.Enabled = false;

            BackgroundWorker.RunWorkerAsync();
        }

        private void DecryptFromFiles_Click(object sender, EventArgs e) => LoadLocalFiles(DumpFormat.Encrypted);

        private void EncryptFromFiles_Click(object sender, EventArgs e) => LoadLocalFiles(DumpFormat.Decrypted);

        private void LoadToEditor_Click(object sender, EventArgs e) => LoadLocalFiles(null);

        private void LoadToBoxes_Click(object sender, EventArgs e) => LoadLocalFiles(null, true);

        private void LoadLocalFiles(DumpFormat? originFormat, bool toBoxes = false)
        {
            if (originFormat == null && toBoxes == false)
                OpenFileDialog.Multiselect = false;
            else
                OpenFileDialog.Multiselect = true;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (originFormat == null)
                {
                    DisableAll();
                    BackgroundLoader.RunWorkerAsync(argument: toBoxes);
                }
                else if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DisableAll();
                    BackGroundWorkerLocal.RunWorkerAsync(argument: originFormat);
                }
            }
        }

        private void BackGroundWorkerLocal_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var bgWorker = sender as BackgroundWorker;
                if (bgWorker != null)
                {
                    var i = 0;
                    foreach (String file in OpenFileDialog.FileNames)
                    {
                        i++;
                        PluginInstance.StartEncryptorDecryptor(this, (DumpFormat)e.Argument!, file, SaveFileDialog.SelectedPath.ToString());
                        WriteLog($"{Strings["Action.Loading"]} [{i}] {Strings["Action.Files"]}.");
                        bgWorker.ReportProgress(6000 / OpenFileDialog.FileNames.Length * i);
                    }
                    bgWorker.ReportProgress(6000);
                    WriteLog($"{Strings["Action.Completed"]} [{i}] {Strings["Action.Files"]} {Strings["Action.Elaborated"]}.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BackgroundLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            string warning = $"{Strings["Warning.Name1"]}\n" +
                $"{Strings["Warning.Line1"]}\n" +
                $"{Strings["Warning.Line2"]}\n" +
                $"{Strings["Warning.Line3"]}\n" +
                $"{Strings["Warning.Line4"]}\n" +
                $"{Strings["Warning.Line5"]}\n" +
                $"{Strings["Warning.Line6b"]}\n" +
                $"{Strings["Warning.Line7"]}\n" +
                $"{Strings["Warning.Line8"]}\n" +
                $"{Strings["Warning.Line9"]}\n" +
                $"{Strings["Warning.Line10"]}\n" +
                $"{Strings["Warning.Line11"]}\n" +
                $"\n{Strings["Warning.Line12"]}";

            var bgWorker = sender as BackgroundWorker;
            if (bgWorker != null)
            {
                DialogResult disclaimer = MessageBox.Show(warning, Strings["Warning.Name1"], MessageBoxButtons.YesNo);
                if (disclaimer == DialogResult.Yes)
                {
                    if ((bool)e.Argument! == false)
                    {
                        if (!PluginInstance.StartLoader(this, OpenFileDialog.FileName, (bool)e.Argument))
                            WriteLog(Strings["Warning.IncompatibleFile"]);
                        else
                            WriteLog($"{Strings["Action.Completed"]} [1] {Strings["Action.Compatible"]} {Strings["Action.Files"]} {Strings["Action.Elaborated"]}.");
                    }
                    else
                    {
                        var currSlot = 0;
                        var currBox = 0;
                        var i = 0;
                        PluginInstance.GetSavAvalableBoxAndSlots(out int savSlots, out int savBoxes);
                        foreach (String file in OpenFileDialog.FileNames)
                        {
                            if (currSlot >= savSlots)
                            {
                                currSlot = 0;
                                currBox++;
                            }
                            if (currBox < savBoxes)
                            {
                                if (PluginInstance.StartLoader(this, file, (bool)e.Argument, currBox, currSlot))
                                {
                                    currSlot++;
                                    i++;
                                }
                                WriteLog($"{Strings["Action.Loading"]} [{i}] {Strings["Action.Compatible"]} {Strings["Action.Files"]}.");
                            }
                            else
                                break;

                            bgWorker.ReportProgress(6000 / OpenFileDialog.FileNames.Length * i);
                        }
                        WriteLog($"{Strings["Action.Completed"]} [{i}] {Strings["Action.Compatible"]} {Strings["Action.Files"]} {Strings["Action.Elaborated"]}.");
                        PluginInstance.ReloadSav();
                    }
                    bgWorker.ReportProgress(6000);
                }
                else if (disclaimer == DialogResult.No)
                    bgWorker.ReportProgress(6000);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PluginInstance.StartDumper(this, sender as BackgroundWorker);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false)
                EnableAll();
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
            ChkBoxFolders.Checked = false;
            ChkBoxFolders.Enabled = false;
        }

        private void RadioSlot_CheckedChanged(object sender, EventArgs e)
        {
            ComboBox.Enabled = true;
            ComboSlot.Enabled = true;
            ChkBoxFolders.Checked = false;
            ChkBoxFolders.Enabled = false;
        }

        private void RadioTargetAll_CheckedChanged(object sender, EventArgs e)
        {
            ComboBox.Enabled = false;
            ComboSlot.Enabled = false;
            ChkBoxFolders.Enabled = true;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                TxtBoxPath.Text = FolderBrowser.SelectedPath;
            }
        }

        private bool CheckPortTxt()
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

        private void EnableAll()
        {
            GrpConnection.Enabled = true;
            GrpAction.Enabled = true;
            GrpDump.Enabled = true;
            GrpPath.Enabled = true;
            BtnConnect.Enabled = true;
            toolsToolStripMenuItem.Enabled = true;
            ChkBoxFolders.Enabled = true;
        }

        private void DisableAll()
        {
            GrpConnection.Enabled = false;
            GrpAction.Enabled = false;
            GrpDump.Enabled = false;
            GrpPath.Enabled = false;
            BtnConnect.Enabled = false;
            toolsToolStripMenuItem.Enabled = false;
            ChkBoxFolders.Enabled = false;
        }

        public string GetIP() => TxtBoxIP.Text;
        public int GetPort() => (int)UInt32.Parse(TxtBoxPort.Text);

        public int GetTargetBox()
        {
            if(ComboBox.InvokeRequired)
            {
                var res = 0;
                ComboBox.Invoke(() => { res = ComboBox.SelectedIndex; });
                return res;
            }
            else
                return ComboBox.SelectedIndex;
        }

        public int GetTargetSlot()
        {
            if (ComboSlot.InvokeRequired)
            {
                var res = 0;
                ComboSlot.Invoke(() => { res = ComboSlot.SelectedIndex; });
                return res;
            }
            else
                return ComboSlot.SelectedIndex;
        }

        public string GetPath()
        {
            if (TxtBoxPath.InvokeRequired)
            {
                var res = "";
                TxtBoxPath.Invoke(() => { res = TxtBoxPath.Text; });
                return res;
            }
            else 
            if (TxtBoxPath.InvokeRequired)
            {
                var res = "";
                TxtBoxPath.Invoke(() => { res = TxtBoxPath.Text; });
                return res;
            }
            else
                return TxtBoxPath.Text;
        }

        public bool GetBoxFolderRequested()
        {
            var radio = false;
            var check = false;

            if(RadioTargetAll.InvokeRequired)
                RadioTargetAll.Invoke(() => { radio = RadioTargetAll.Checked; });
            else
                radio = RadioTargetAll.Checked;

            if (ChkBoxFolders.InvokeRequired)
                ChkBoxFolders.Invoke(() => { check = ChkBoxFolders.Checked; });
            else
                check = ChkBoxFolders.Checked;

            return radio && check;
        }

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

        public void WriteLog(string str) 
        {
            if(TxtBoxLog.InvokeRequired)
                TxtBoxLog.Invoke(() => { TxtBoxLog.Text = str; });
            else
                TxtBoxLog.Text = str;
        }

        public void AppendLog(string str) 
        {
            if (TxtBoxLog.InvokeRequired)
                TxtBoxLog.Invoke(() => { TxtBoxLog.AppendText(str); });
            else 
                TxtBoxLog.AppendText(str);
        }
    }
}
