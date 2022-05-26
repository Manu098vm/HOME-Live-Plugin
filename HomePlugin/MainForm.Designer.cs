namespace HOME
{
    partial class MainForm
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
            this.RadioTargetAll = new System.Windows.Forms.RadioButton();
            this.RadioB1S1 = new System.Windows.Forms.RadioButton();
            this.BtnReset = new System.Windows.Forms.Button();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.GrpPath = new System.Windows.Forms.GroupBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.TxtBoxPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.GrpDump.SuspendLayout();
            this.GrpConnection.SuspendLayout();
            this.GrpAction.SuspendLayout();
            this.GrpPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnConnect
            // 
            this.BtnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConnect.Location = new System.Drawing.Point(12, 312);
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
            this.TxtBoxLog.Location = new System.Drawing.Point(12, 377);
            this.TxtBoxLog.Name = "TxtBoxLog";
            this.TxtBoxLog.Size = new System.Drawing.Size(439, 77);
            this.TxtBoxLog.TabIndex = 1;
            this.TxtBoxLog.Text = "";
            // 
            // TxtBoxIP
            // 
            this.TxtBoxIP.Location = new System.Drawing.Point(100, 74);
            this.TxtBoxIP.Name = "TxtBoxIP";
            this.TxtBoxIP.Size = new System.Drawing.Size(200, 22);
            this.TxtBoxIP.TabIndex = 2;
            this.TxtBoxIP.Text = "192.168.1.1";
            this.TxtBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TxtBoxPort
            // 
            this.TxtBoxPort.Location = new System.Drawing.Point(359, 74);
            this.TxtBoxPort.Name = "TxtBoxPort";
            this.TxtBoxPort.Size = new System.Drawing.Size(76, 22);
            this.TxtBoxPort.TabIndex = 3;
            this.TxtBoxPort.Text = "6000";
            this.TxtBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LblIP
            // 
            this.LblIP.AutoSize = true;
            this.LblIP.Location = new System.Drawing.Point(15, 77);
            this.LblIP.Name = "LblIP";
            this.LblIP.Size = new System.Drawing.Size(79, 16);
            this.LblIP.TabIndex = 4;
            this.LblIP.Text = "IP Address :";
            // 
            // LblPort
            // 
            this.LblPort.AutoSize = true;
            this.LblPort.Location = new System.Drawing.Point(316, 77);
            this.LblPort.Name = "LblPort";
            this.LblPort.Size = new System.Drawing.Size(37, 16);
            this.LblPort.TabIndex = 5;
            this.LblPort.Text = "Port :";
            // 
            // RadioEncrypted
            // 
            this.RadioEncrypted.AutoSize = true;
            this.RadioEncrypted.Location = new System.Drawing.Point(21, 21);
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
            this.RadioDecrypted.Location = new System.Drawing.Point(130, 21);
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
            this.RadioEncAndDec.Location = new System.Drawing.Point(252, 21);
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
            this.GrpDump.Location = new System.Drawing.Point(12, 184);
            this.GrpDump.Name = "GrpDump";
            this.GrpDump.Size = new System.Drawing.Size(439, 60);
            this.GrpDump.TabIndex = 9;
            this.GrpDump.TabStop = false;
            this.GrpDump.Text = "Dump Format";
            // 
            // GrpConnection
            // 
            this.GrpConnection.Controls.Add(this.RadioUSB);
            this.GrpConnection.Controls.Add(this.RadioWiFi);
            this.GrpConnection.Location = new System.Drawing.Point(12, 12);
            this.GrpConnection.Name = "GrpConnection";
            this.GrpConnection.Size = new System.Drawing.Size(439, 46);
            this.GrpConnection.TabIndex = 10;
            this.GrpConnection.TabStop = false;
            this.GrpConnection.Text = "Connection Type";
            // 
            // RadioUSB
            // 
            this.RadioUSB.AutoSize = true;
            this.RadioUSB.Location = new System.Drawing.Point(268, 20);
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
            this.GrpAction.Controls.Add(this.RadioTargetAll);
            this.GrpAction.Controls.Add(this.RadioB1S1);
            this.GrpAction.Location = new System.Drawing.Point(12, 120);
            this.GrpAction.Name = "GrpAction";
            this.GrpAction.Size = new System.Drawing.Size(439, 58);
            this.GrpAction.TabIndex = 11;
            this.GrpAction.TabStop = false;
            this.GrpAction.Text = "Dump Target";
            // 
            // RadioTargetAll
            // 
            this.RadioTargetAll.AutoSize = true;
            this.RadioTargetAll.Location = new System.Drawing.Point(231, 21);
            this.RadioTargetAll.Name = "RadioTargetAll";
            this.RadioTargetAll.Size = new System.Drawing.Size(143, 20);
            this.RadioTargetAll.TabIndex = 1;
            this.RadioTargetAll.TabStop = true;
            this.RadioTargetAll.Text = "All Boxes and Slots";
            this.RadioTargetAll.UseVisualStyleBackColor = true;
            // 
            // RadioB1S1
            // 
            this.RadioB1S1.AutoSize = true;
            this.RadioB1S1.Location = new System.Drawing.Point(70, 21);
            this.RadioB1S1.Name = "RadioB1S1";
            this.RadioB1S1.Size = new System.Drawing.Size(127, 20);
            this.RadioB1S1.TabIndex = 0;
            this.RadioB1S1.TabStop = true;
            this.RadioB1S1.Text = "Only Box 1 Slot 1";
            this.RadioB1S1.UseVisualStyleBackColor = true;
            // 
            // BtnReset
            // 
            this.BtnReset.Location = new System.Drawing.Point(360, 102);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(75, 23);
            this.BtnReset.TabIndex = 12;
            this.BtnReset.Text = "Reset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.Description = "Dump Folder";
            // 
            // GrpPath
            // 
            this.GrpPath.Controls.Add(this.BtnBrowse);
            this.GrpPath.Controls.Add(this.TxtBoxPath);
            this.GrpPath.Location = new System.Drawing.Point(12, 250);
            this.GrpPath.Name = "GrpPath";
            this.GrpPath.Size = new System.Drawing.Size(439, 56);
            this.GrpPath.TabIndex = 13;
            this.GrpPath.TabStop = false;
            this.GrpPath.Text = "Dump Folder Path";
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(360, 21);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(73, 24);
            this.BtnBrowse.TabIndex = 1;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // TxtBoxPath
            // 
            this.TxtBoxPath.Location = new System.Drawing.Point(21, 21);
            this.TxtBoxPath.Name = "TxtBoxPath";
            this.TxtBoxPath.Size = new System.Drawing.Size(333, 22);
            this.TxtBoxPath.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 462);
            this.Controls.Add(this.GrpPath);
            this.Controls.Add(this.BtnReset);
            this.Controls.Add(this.GrpAction);
            this.Controls.Add(this.GrpConnection);
            this.Controls.Add(this.GrpDump);
            this.Controls.Add(this.LblPort);
            this.Controls.Add(this.LblIP);
            this.Controls.Add(this.TxtBoxPort);
            this.Controls.Add(this.TxtBoxIP);
            this.Controls.Add(this.TxtBoxLog);
            this.Controls.Add(this.BtnConnect);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Home Plugin";
            this.GrpDump.ResumeLayout(false);
            this.GrpDump.PerformLayout();
            this.GrpConnection.ResumeLayout(false);
            this.GrpConnection.PerformLayout();
            this.GrpAction.ResumeLayout(false);
            this.GrpAction.PerformLayout();
            this.GrpPath.ResumeLayout(false);
            this.GrpPath.PerformLayout();
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
        private System.Windows.Forms.RadioButton RadioB1S1;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.GroupBox GrpPath;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.TextBox TxtBoxPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}