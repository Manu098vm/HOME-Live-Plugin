namespace HOME
{
    partial class DumpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnConnect = new System.Windows.Forms.Button();
            this.TxtBoxLog = new System.Windows.Forms.RichTextBox();
            this.TxtBoxIP = new System.Windows.Forms.TextBox();
            this.TxtBoxPort = new System.Windows.Forms.TextBox();
            this.LblIP = new System.Windows.Forms.Label();
            this.LblPort = new System.Windows.Forms.Label();
            this.RadioEncrypted = new System.Windows.Forms.RadioButton();
            this.RadioDecrypted = new System.Windows.Forms.RadioButton();
            this.RadioEncAndDec = new System.Windows.Forms.RadioButton();
            this.GrpDump = new System.Windows.Forms.GroupBox();
            this.GrpConnection = new System.Windows.Forms.GroupBox();
            this.RadioUSB = new System.Windows.Forms.RadioButton();
            this.RadioWiFi = new System.Windows.Forms.RadioButton();
            this.GrpAction = new System.Windows.Forms.GroupBox();
            this.ChkBoxFolders = new System.Windows.Forms.CheckBox();
            this.ComboSlot = new System.Windows.Forms.ComboBox();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.RadioSlot = new System.Windows.Forms.RadioButton();
            this.RadioBox = new System.Windows.Forms.RadioButton();
            this.RadioTargetAll = new System.Windows.Forms.RadioButton();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.GrpPath = new System.Windows.Forms.GroupBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.TxtBoxPath = new System.Windows.Forms.TextBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.BackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.Tools = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptFromFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptFromFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFilesToBoxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.BackGroundWorkerLocal = new System.ComponentModel.BackgroundWorker();
            this.BackgroundLoader = new System.ComponentModel.BackgroundWorker();
            this.GrpDump.SuspendLayout();
            this.GrpConnection.SuspendLayout();
            this.GrpAction.SuspendLayout();
            this.GrpPath.SuspendLayout();
            this.Tools.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnConnect
            // 
            this.BtnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConnect.Location = new System.Drawing.Point(12, 377);
            this.BtnConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(439, 59);
            this.BtnConnect.TabIndex = 0;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // TxtBoxLog
            // 
            this.TxtBoxLog.Enabled = false;
            this.TxtBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtBoxLog.Location = new System.Drawing.Point(12, 441);
            this.TxtBoxLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtBoxLog.Name = "TxtBoxLog";
            this.TxtBoxLog.Size = new System.Drawing.Size(439, 77);
            this.TxtBoxLog.TabIndex = 1;
            this.TxtBoxLog.Text = "";
            // 
            // TxtBoxIP
            // 
            this.TxtBoxIP.Location = new System.Drawing.Point(91, 53);
            this.TxtBoxIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtBoxIP.Name = "TxtBoxIP";
            this.TxtBoxIP.Size = new System.Drawing.Size(200, 22);
            this.TxtBoxIP.TabIndex = 2;
            this.TxtBoxIP.Text = "192.168.1.1";
            this.TxtBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TxtBoxPort
            // 
            this.TxtBoxPort.Location = new System.Drawing.Point(348, 53);
            this.TxtBoxPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtBoxPort.Name = "TxtBoxPort";
            this.TxtBoxPort.Size = new System.Drawing.Size(76, 22);
            this.TxtBoxPort.TabIndex = 3;
            this.TxtBoxPort.Text = "6000";
            this.TxtBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LblIP
            // 
            this.LblIP.AutoSize = true;
            this.LblIP.Location = new System.Drawing.Point(5, 53);
            this.LblIP.Name = "LblIP";
            this.LblIP.Size = new System.Drawing.Size(79, 16);
            this.LblIP.TabIndex = 4;
            this.LblIP.Text = "IP Address :";
            // 
            // LblPort
            // 
            this.LblPort.AutoSize = true;
            this.LblPort.Location = new System.Drawing.Point(305, 53);
            this.LblPort.Name = "LblPort";
            this.LblPort.Size = new System.Drawing.Size(37, 16);
            this.LblPort.TabIndex = 5;
            this.LblPort.Text = "Port :";
            // 
            // RadioEncrypted
            // 
            this.RadioEncrypted.AutoSize = true;
            this.RadioEncrypted.Location = new System.Drawing.Point(21, 21);
            this.RadioEncrypted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioEncrypted.Name = "RadioEncrypted";
            this.RadioEncrypted.Size = new System.Drawing.Size(89, 20);
            this.RadioEncrypted.TabIndex = 6;
            this.RadioEncrypted.TabStop = true;
            this.RadioEncrypted.Text = "Encrypted";
            this.RadioEncrypted.UseVisualStyleBackColor = true;
            // 
            // RadioDecrypted
            // 
            this.RadioDecrypted.AutoSize = true;
            this.RadioDecrypted.Location = new System.Drawing.Point(131, 21);
            this.RadioDecrypted.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioDecrypted.Name = "RadioDecrypted";
            this.RadioDecrypted.Size = new System.Drawing.Size(91, 20);
            this.RadioDecrypted.TabIndex = 7;
            this.RadioDecrypted.TabStop = true;
            this.RadioDecrypted.Text = "Decrypted";
            this.RadioDecrypted.UseVisualStyleBackColor = true;
            // 
            // RadioEncAndDec
            // 
            this.RadioEncAndDec.AutoSize = true;
            this.RadioEncAndDec.Location = new System.Drawing.Point(243, 21);
            this.RadioEncAndDec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioEncAndDec.Name = "RadioEncAndDec";
            this.RadioEncAndDec.Size = new System.Drawing.Size(181, 20);
            this.RadioEncAndDec.TabIndex = 8;
            this.RadioEncAndDec.TabStop = true;
            this.RadioEncAndDec.Text = "Encrypted and Decrypted";
            this.RadioEncAndDec.UseVisualStyleBackColor = true;
            // 
            // GrpDump
            // 
            this.GrpDump.Controls.Add(this.RadioEncrypted);
            this.GrpDump.Controls.Add(this.RadioEncAndDec);
            this.GrpDump.Controls.Add(this.RadioDecrypted);
            this.GrpDump.Location = new System.Drawing.Point(12, 249);
            this.GrpDump.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpDump.Name = "GrpDump";
            this.GrpDump.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpDump.Size = new System.Drawing.Size(439, 60);
            this.GrpDump.TabIndex = 9;
            this.GrpDump.TabStop = false;
            this.GrpDump.Text = "Dump Format";
            // 
            // GrpConnection
            // 
            this.GrpConnection.Controls.Add(this.RadioUSB);
            this.GrpConnection.Controls.Add(this.RadioWiFi);
            this.GrpConnection.Controls.Add(this.LblIP);
            this.GrpConnection.Controls.Add(this.TxtBoxIP);
            this.GrpConnection.Controls.Add(this.TxtBoxPort);
            this.GrpConnection.Controls.Add(this.LblPort);
            this.GrpConnection.Location = new System.Drawing.Point(12, 32);
            this.GrpConnection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpConnection.Name = "GrpConnection";
            this.GrpConnection.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpConnection.Size = new System.Drawing.Size(439, 102);
            this.GrpConnection.TabIndex = 10;
            this.GrpConnection.TabStop = false;
            this.GrpConnection.Text = "Connection";
            // 
            // RadioUSB
            // 
            this.RadioUSB.AutoSize = true;
            this.RadioUSB.Location = new System.Drawing.Point(268, 20);
            this.RadioUSB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioUSB.Name = "RadioUSB";
            this.RadioUSB.Size = new System.Drawing.Size(56, 20);
            this.RadioUSB.TabIndex = 1;
            this.RadioUSB.TabStop = true;
            this.RadioUSB.Text = "USB";
            this.RadioUSB.UseVisualStyleBackColor = true;
            this.RadioUSB.CheckedChanged += new System.EventHandler(this.RadioUSB_CheckedChanged);
            // 
            // RadioWiFi
            // 
            this.RadioWiFi.AutoSize = true;
            this.RadioWiFi.Location = new System.Drawing.Point(108, 20);
            this.RadioWiFi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioWiFi.Name = "RadioWiFi";
            this.RadioWiFi.Size = new System.Drawing.Size(59, 20);
            this.RadioWiFi.TabIndex = 0;
            this.RadioWiFi.TabStop = true;
            this.RadioWiFi.Text = "Wi-Fi";
            this.RadioWiFi.UseVisualStyleBackColor = true;
            this.RadioWiFi.CheckedChanged += new System.EventHandler(this.RadioWiFi_CheckedChanged);
            // 
            // GrpAction
            // 
            this.GrpAction.Controls.Add(this.ChkBoxFolders);
            this.GrpAction.Controls.Add(this.ComboSlot);
            this.GrpAction.Controls.Add(this.ComboBox);
            this.GrpAction.Controls.Add(this.RadioSlot);
            this.GrpAction.Controls.Add(this.RadioBox);
            this.GrpAction.Controls.Add(this.RadioTargetAll);
            this.GrpAction.Location = new System.Drawing.Point(12, 140);
            this.GrpAction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpAction.Name = "GrpAction";
            this.GrpAction.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpAction.Size = new System.Drawing.Size(439, 102);
            this.GrpAction.TabIndex = 11;
            this.GrpAction.TabStop = false;
            this.GrpAction.Text = "Dump Target";
            // 
            // ChkBoxFolders
            // 
            this.ChkBoxFolders.AutoSize = true;
            this.ChkBoxFolders.Enabled = false;
            this.ChkBoxFolders.Location = new System.Drawing.Point(261, 63);
            this.ChkBoxFolders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChkBoxFolders.Name = "ChkBoxFolders";
            this.ChkBoxFolders.Size = new System.Drawing.Size(171, 20);
            this.ChkBoxFolders.TabIndex = 6;
            this.ChkBoxFolders.Text = "Create folders for boxes";
            this.ChkBoxFolders.UseVisualStyleBackColor = true;
            // 
            // ComboSlot
            // 
            this.ComboSlot.FormattingEnabled = true;
            this.ComboSlot.Location = new System.Drawing.Point(147, 63);
            this.ComboSlot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ComboSlot.Name = "ComboSlot";
            this.ComboSlot.Size = new System.Drawing.Size(103, 24);
            this.ComboSlot.TabIndex = 5;
            // 
            // ComboBox
            // 
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(8, 63);
            this.ComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(103, 24);
            this.ComboBox.TabIndex = 4;
            // 
            // RadioSlot
            // 
            this.RadioSlot.AutoSize = true;
            this.RadioSlot.Location = new System.Drawing.Point(147, 21);
            this.RadioSlot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioSlot.Name = "RadioSlot";
            this.RadioSlot.Size = new System.Drawing.Size(99, 20);
            this.RadioSlot.TabIndex = 3;
            this.RadioSlot.TabStop = true;
            this.RadioSlot.Text = "Specifc Slot";
            this.RadioSlot.UseVisualStyleBackColor = true;
            this.RadioSlot.CheckedChanged += new System.EventHandler(this.RadioSlot_CheckedChanged);
            // 
            // RadioBox
            // 
            this.RadioBox.AutoSize = true;
            this.RadioBox.Location = new System.Drawing.Point(8, 21);
            this.RadioBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioBox.Name = "RadioBox";
            this.RadioBox.Size = new System.Drawing.Size(102, 20);
            this.RadioBox.TabIndex = 2;
            this.RadioBox.TabStop = true;
            this.RadioBox.Text = "Specific Box";
            this.RadioBox.UseVisualStyleBackColor = true;
            this.RadioBox.CheckedChanged += new System.EventHandler(this.RadioBox_CheckedChanged);
            // 
            // RadioTargetAll
            // 
            this.RadioTargetAll.AutoSize = true;
            this.RadioTargetAll.Location = new System.Drawing.Point(268, 21);
            this.RadioTargetAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RadioTargetAll.Name = "RadioTargetAll";
            this.RadioTargetAll.Size = new System.Drawing.Size(143, 20);
            this.RadioTargetAll.TabIndex = 1;
            this.RadioTargetAll.TabStop = true;
            this.RadioTargetAll.Text = "All Boxes and Slots";
            this.RadioTargetAll.UseVisualStyleBackColor = true;
            this.RadioTargetAll.CheckedChanged += new System.EventHandler(this.RadioTargetAll_CheckedChanged);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "Dump Folder";
            // 
            // GrpPath
            // 
            this.GrpPath.Controls.Add(this.BtnBrowse);
            this.GrpPath.Controls.Add(this.TxtBoxPath);
            this.GrpPath.Location = new System.Drawing.Point(12, 314);
            this.GrpPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpPath.Name = "GrpPath";
            this.GrpPath.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GrpPath.Size = new System.Drawing.Size(439, 57);
            this.GrpPath.TabIndex = 13;
            this.GrpPath.TabStop = false;
            this.GrpPath.Text = "Dump Folder Path";
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(360, 21);
            this.BtnBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(73, 25);
            this.BtnBrowse.TabIndex = 1;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // TxtBoxPath
            // 
            this.TxtBoxPath.Location = new System.Drawing.Point(21, 21);
            this.TxtBoxPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtBoxPath.Name = "TxtBoxPath";
            this.TxtBoxPath.Size = new System.Drawing.Size(333, 22);
            this.TxtBoxPath.TabIndex = 0;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(12, 524);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ProgressBar.Maximum = 6000;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(439, 26);
            this.ProgressBar.Step = 1;
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBar.TabIndex = 14;
            // 
            // BackgroundWorker
            // 
            this.BackgroundWorker.WorkerReportsProgress = true;
            this.BackgroundWorker.WorkerSupportsCancellation = true;
            this.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.BackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.BackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // Tools
            // 
            this.Tools.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Tools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.Tools.Location = new System.Drawing.Point(0, 0);
            this.Tools.Name = "Tools";
            this.Tools.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.Tools.Size = new System.Drawing.Size(457, 28);
            this.Tools.TabIndex = 16;
            this.Tools.Text = "menuStrip2";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decryptFromFilesToolStripMenuItem,
            this.encryptFromFilesToolStripMenuItem,
            this.loadFileToEditorToolStripMenuItem,
            this.loadFilesToBoxesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // decryptFromFilesToolStripMenuItem
            // 
            this.decryptFromFilesToolStripMenuItem.Name = "decryptFromFilesToolStripMenuItem";
            this.decryptFromFilesToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.decryptFromFilesToolStripMenuItem.Text = "Decrypt from files";
            this.decryptFromFilesToolStripMenuItem.Click += new System.EventHandler(this.DecryptFromFiles_Click);
            // 
            // encryptFromFilesToolStripMenuItem
            // 
            this.encryptFromFilesToolStripMenuItem.Name = "encryptFromFilesToolStripMenuItem";
            this.encryptFromFilesToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.encryptFromFilesToolStripMenuItem.Text = "Encrypt from files";
            this.encryptFromFilesToolStripMenuItem.Click += new System.EventHandler(this.EncryptFromFiles_Click);
            // 
            // loadFileToEditorToolStripMenuItem
            // 
            this.loadFileToEditorToolStripMenuItem.Name = "loadFileToEditorToolStripMenuItem";
            this.loadFileToEditorToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.loadFileToEditorToolStripMenuItem.Text = "Load file to Editor";
            this.loadFileToEditorToolStripMenuItem.Click += new System.EventHandler(this.LoadToEditor_Click);
            // 
            // loadFilesToBoxesToolStripMenuItem
            // 
            this.loadFilesToBoxesToolStripMenuItem.Name = "loadFilesToBoxesToolStripMenuItem";
            this.loadFilesToBoxesToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.loadFilesToBoxesToolStripMenuItem.Text = "Load files to Boxes";
            this.loadFilesToBoxesToolStripMenuItem.Click += new System.EventHandler(this.LoadToBoxes_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.DefaultExt = "eh1";
            this.OpenFileDialog.FileName = "OpenFileDialog";
            this.OpenFileDialog.Filter = "All files (*.*)|*.*|Encrypted files (*.eh1)|*.eh1|Decrypted files (*.ph1)|*.ph1";
            this.OpenFileDialog.Multiselect = true;
            this.OpenFileDialog.ReadOnlyChecked = true;
            this.OpenFileDialog.ShowReadOnly = true;
            this.OpenFileDialog.SupportMultiDottedExtensions = true;
            this.OpenFileDialog.Title = "Open Pokémon Home data file";
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.Description = "Save decrypted Pokémon Home data";
            // 
            // BackGroundWorkerLocal
            // 
            this.BackGroundWorkerLocal.WorkerReportsProgress = true;
            this.BackGroundWorkerLocal.WorkerSupportsCancellation = true;
            this.BackGroundWorkerLocal.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackGroundWorkerLocal_DoWork);
            this.BackGroundWorkerLocal.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.BackGroundWorkerLocal.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // BackgroundLoader
            // 
            this.BackgroundLoader.WorkerReportsProgress = true;
            this.BackgroundLoader.WorkerSupportsCancellation = true;
            this.BackgroundLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundLoader_DoWork);
            this.BackgroundLoader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.BackgroundLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // DumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 562);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.GrpPath);
            this.Controls.Add(this.GrpAction);
            this.Controls.Add(this.GrpConnection);
            this.Controls.Add(this.GrpDump);
            this.Controls.Add(this.TxtBoxLog);
            this.Controls.Add(this.BtnConnect);
            this.Controls.Add(this.Tools);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DumpForm";
            this.ShowIcon = false;
            this.Text = "Home Plugin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Close);
            this.GrpDump.ResumeLayout(false);
            this.GrpDump.PerformLayout();
            this.GrpConnection.ResumeLayout(false);
            this.GrpConnection.PerformLayout();
            this.GrpAction.ResumeLayout(false);
            this.GrpAction.PerformLayout();
            this.GrpPath.ResumeLayout(false);
            this.GrpPath.PerformLayout();
            this.Tools.ResumeLayout(false);
            this.Tools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.RichTextBox TxtBoxLog;
        private System.Windows.Forms.TextBox TxtBoxIP;
        private System.Windows.Forms.TextBox TxtBoxPort;
        private System.Windows.Forms.Label LblIP;
        private System.Windows.Forms.Label LblPort;
        private System.Windows.Forms.RadioButton RadioEncrypted;
        private System.Windows.Forms.RadioButton RadioDecrypted;
        private System.Windows.Forms.RadioButton RadioEncAndDec;
        private System.Windows.Forms.GroupBox GrpDump;
        private System.Windows.Forms.GroupBox GrpConnection;
        private System.Windows.Forms.RadioButton RadioUSB;
        private System.Windows.Forms.RadioButton RadioWiFi;
        private System.Windows.Forms.GroupBox GrpAction;
        private System.Windows.Forms.RadioButton RadioTargetAll;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.GroupBox GrpPath;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.TextBox TxtBoxPath;
        private System.Windows.Forms.RadioButton RadioSlot;
        private System.Windows.Forms.RadioButton RadioBox;
        private System.Windows.Forms.ComboBox ComboSlot;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.ComponentModel.BackgroundWorker BackgroundWorker;
        private System.Windows.Forms.MenuStrip Tools;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptFromFilesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.ComponentModel.BackgroundWorker BackGroundWorkerLocal;
        private System.Windows.Forms.FolderBrowserDialog SaveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem encryptFromFilesToolStripMenuItem;
        private System.Windows.Forms.CheckBox ChkBoxFolders;
        private System.Windows.Forms.ToolStripMenuItem loadFileToEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFilesToBoxesToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker BackgroundLoader;
    }
}