using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace HOME
{
    public partial class DumpForm : Form
    {

        HOME PluginInstance = null!;

        public DumpForm(HOME instance)
        {
            InitializeComponent();
            PluginInstance = instance;

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
            else if (string.IsNullOrWhiteSpace(TxtBoxPort.Text) || CheckPortTxt())
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
                        PluginInstance.StartEncryptorDecryptor(this, (DumpFormat)e.Argument, file, SaveFileDialog.SelectedPath.ToString());
                        TxtBoxLog.Text = $"Loading [{i}] file(s).";
                        bgWorker.ReportProgress(6000 / OpenFileDialog.FileNames.Length * i);
                    }
                    bgWorker.ReportProgress(6000);
                    TxtBoxLog.Text = $"Process completed. [{i}] file(s) elaborated.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BackgroundLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            string warning = $"WARNING/DISCLOSURE:\n" +
                $"PKHeX simulates a conversion from the Pokémon HOME data format (PH1) to standard PKM file formats based on the current loaded save file.\n" +
                $"This process is unofficial and there is always the chance that it does not accurately replicate an official transfer.\n" +
                $"If you proceed with this tool, you accept the following:\n" +
                $"- The PKM files from the conversion are NOT legitimate in any way, even if the original encounter was.\n" +
                $"- The resulting files from the conversion may not even be legal in some circumstances.\n" +
                $"- If the File does not contain Specific Game Data, it is likely that the resulting Pokémon will be illegal.\n" +
                $"- Do NOT use converted PKM in online battles/trades.\n" +
                $"- Do NOT use converted files to report legality issues, whether in the Project Pokémon forums/Discord or in the PKHeX Development Projects Discord.\n" +
                $"- This Plugin is intended for research, learning, and entertainment purposes.\n" +
                $"- This Plugin is not developed by the PKHeX Development Projects server, so do NOT report problems or request support there. Use the Project Pokémon thread instead.\n" +
                $"- The creators of this tool are not responsible for any adverse outcomes or side effects of using this tool.\n" +
                $"\nIf you agree with the above, click the 'Yes' button. Click 'No' otherwise.";

            var bgWorker = sender as BackgroundWorker;
            if (bgWorker != null)
            {
                DialogResult disclaimer = MessageBox.Show(warning, "Disclaimer", MessageBoxButtons.YesNo);
                if (disclaimer == DialogResult.Yes)
                {
                    if ((bool)e.Argument == false)
                    {
                        if (!PluginInstance.StartLoader(this, OpenFileDialog.FileName, (bool)e.Argument))
                            TxtBoxLog.Text = "File not compatible with the current Save File";
                        else
                            TxtBoxLog.Text = "Process completed. [1] compatibile file elaborated.";
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
                                TxtBoxLog.Text = $"Loading [{i}] compatible file(s).";
                            }
                            else
                                break;

                            bgWorker.ReportProgress(6000 / OpenFileDialog.FileNames.Length * i);
                        }
                        TxtBoxLog.Text = $"Process completed. [{i}] compatibile file(s) elaborated.";
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
        public int GetTargetBox() => ComboBox.SelectedIndex;
        public int GetTargetSlot() => ComboSlot.SelectedIndex;
        public string GetPath() => TxtBoxPath.Text;
        public bool GetBoxFolderRequested() => RadioTargetAll.Checked && ChkBoxFolders.Checked;
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
