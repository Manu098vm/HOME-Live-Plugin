namespace HOME
{
    partial class ViewForm
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
            this.LblAddress = new System.Windows.Forms.Label();
            this.GrpConnection = new System.Windows.Forms.GroupBox();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.LblPort = new System.Windows.Forms.Label();
            this.TxtAddress = new System.Windows.Forms.TextBox();
            this.RadioUSB = new System.Windows.Forms.RadioButton();
            this.RadioWiFi = new System.Windows.Forms.RadioButton();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.LblBoxes = new System.Windows.Forms.Label();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.TxtLog = new System.Windows.Forms.RichTextBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.BackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.GrpForceConv = new System.Windows.Forms.GroupBox();
            this.RadioConvertAny = new System.Windows.Forms.RadioButton();
            this.RadioConvertForce = new System.Windows.Forms.RadioButton();
            this.RadioConvertSpecific = new System.Windows.Forms.RadioButton();
            this.GrpWarning = new System.Windows.Forms.GroupBox();
            this.ChkWarning = new System.Windows.Forms.CheckBox();
            this.GrpConnection.SuspendLayout();
            this.GrpForceConv.SuspendLayout();
            this.GrpWarning.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblAddress
            // 
            this.LblAddress.AutoSize = true;
            this.LblAddress.Location = new System.Drawing.Point(36, 95);
            this.LblAddress.Name = "LblAddress";
            this.LblAddress.Size = new System.Drawing.Size(81, 20);
            this.LblAddress.TabIndex = 0;
            this.LblAddress.Text = "IP Address:";
            // 
            // GrpConnection
            // 
            this.GrpConnection.Controls.Add(this.TxtPort);
            this.GrpConnection.Controls.Add(this.LblPort);
            this.GrpConnection.Controls.Add(this.TxtAddress);
            this.GrpConnection.Controls.Add(this.RadioUSB);
            this.GrpConnection.Controls.Add(this.RadioWiFi);
            this.GrpConnection.Controls.Add(this.LblAddress);
            this.GrpConnection.Location = new System.Drawing.Point(12, 15);
            this.GrpConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpConnection.Name = "GrpConnection";
            this.GrpConnection.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpConnection.Size = new System.Drawing.Size(383, 162);
            this.GrpConnection.TabIndex = 1;
            this.GrpConnection.TabStop = false;
            this.GrpConnection.Text = "Connection";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(292, 91);
            this.TxtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(42, 27);
            this.TxtPort.TabIndex = 5;
            this.TxtPort.Text = "6000";
            this.TxtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LblPort
            // 
            this.LblPort.AutoSize = true;
            this.LblPort.Location = new System.Drawing.Point(252, 95);
            this.LblPort.Name = "LblPort";
            this.LblPort.Size = new System.Drawing.Size(38, 20);
            this.LblPort.TabIndex = 4;
            this.LblPort.Text = "Port:";
            // 
            // TxtAddress
            // 
            this.TxtAddress.Location = new System.Drawing.Point(118, 91);
            this.TxtAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(128, 27);
            this.TxtAddress.TabIndex = 3;
            this.TxtAddress.Text = "0.0.0.0";
            this.TxtAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RadioUSB
            // 
            this.RadioUSB.AutoSize = true;
            this.RadioUSB.Location = new System.Drawing.Point(199, 41);
            this.RadioUSB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RadioUSB.Name = "RadioUSB";
            this.RadioUSB.Size = new System.Drawing.Size(57, 24);
            this.RadioUSB.TabIndex = 2;
            this.RadioUSB.TabStop = true;
            this.RadioUSB.Text = "USB";
            this.RadioUSB.UseVisualStyleBackColor = true;
            this.RadioUSB.CheckedChanged += new System.EventHandler(this.RadioUSB_CheckedChanged);
            // 
            // RadioWiFi
            // 
            this.RadioWiFi.AutoSize = true;
            this.RadioWiFi.Location = new System.Drawing.Point(118, 41);
            this.RadioWiFi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RadioWiFi.Name = "RadioWiFi";
            this.RadioWiFi.Size = new System.Drawing.Size(65, 24);
            this.RadioWiFi.TabIndex = 1;
            this.RadioWiFi.TabStop = true;
            this.RadioWiFi.Text = "Wi-Fi";
            this.RadioWiFi.UseVisualStyleBackColor = true;
            this.RadioWiFi.CheckedChanged += new System.EventHandler(this.RadioWiFi_CheckedChanged);
            // 
            // BtnConnect
            // 
            this.BtnConnect.Enabled = false;
            this.BtnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnConnect.Location = new System.Drawing.Point(12, 478);
            this.BtnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(383, 80);
            this.BtnConnect.TabIndex = 2;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // LblBoxes
            // 
            this.LblBoxes.AutoSize = true;
            this.LblBoxes.Location = new System.Drawing.Point(25, 399);
            this.LblBoxes.Name = "LblBoxes";
            this.LblBoxes.Size = new System.Drawing.Size(95, 20);
            this.LblBoxes.TabIndex = 3;
            this.LblBoxes.Text = "Select Boxes:";
            // 
            // ComboBox
            // 
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(12, 422);
            this.ComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(383, 28);
            this.ComboBox.TabIndex = 4;
            // 
            // TxtLog
            // 
            this.TxtLog.Enabled = false;
            this.TxtLog.Location = new System.Drawing.Point(12, 579);
            this.TxtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(383, 89);
            this.TxtLog.TabIndex = 5;
            this.TxtLog.Text = "";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(12, 676);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(383, 29);
            this.ProgressBar.TabIndex = 6;
            // 
            // BackGroundWorker
            // 
            this.BackGroundWorker.WorkerReportsProgress = true;
            this.BackGroundWorker.WorkerSupportsCancellation = true;
            this.BackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackGroundWorker_DoWork);
            this.BackGroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackGroundWorker_ProgressChanged);
            this.BackGroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackGroundWorker_RunWorkerCompleted);
            // 
            // GrpForceConv
            // 
            this.GrpForceConv.Controls.Add(this.RadioConvertAny);
            this.GrpForceConv.Controls.Add(this.RadioConvertForce);
            this.GrpForceConv.Controls.Add(this.RadioConvertSpecific);
            this.GrpForceConv.Location = new System.Drawing.Point(12, 185);
            this.GrpForceConv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpForceConv.Name = "GrpForceConv";
            this.GrpForceConv.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpForceConv.Size = new System.Drawing.Size(383, 130);
            this.GrpForceConv.TabIndex = 7;
            this.GrpForceConv.TabStop = false;
            this.GrpForceConv.Text = "Conversion Method";
            // 
            // RadioConvertAny
            // 
            this.RadioConvertAny.AutoSize = true;
            this.RadioConvertAny.Location = new System.Drawing.Point(7, 94);
            this.RadioConvertAny.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RadioConvertAny.Name = "RadioConvertAny";
            this.RadioConvertAny.Size = new System.Drawing.Size(235, 24);
            this.RadioConvertAny.TabIndex = 2;
            this.RadioConvertAny.TabStop = true;
            this.RadioConvertAny.Text = "Convert any PKM data (illegal!)";
            this.RadioConvertAny.UseVisualStyleBackColor = true;
            // 
            // RadioConvertForce
            // 
            this.RadioConvertForce.AutoSize = true;
            this.RadioConvertForce.Location = new System.Drawing.Point(7, 60);
            this.RadioConvertForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RadioConvertForce.Name = "RadioConvertForce";
            this.RadioConvertForce.Size = new System.Drawing.Size(359, 24);
            this.RadioConvertForce.TabIndex = 1;
            this.RadioConvertForce.TabStop = true;
            this.RadioConvertForce.Text = "Convert any PKM data if compatible with save file";
            this.RadioConvertForce.UseVisualStyleBackColor = true;
            // 
            // RadioConvertSpecific
            // 
            this.RadioConvertSpecific.AutoSize = true;
            this.RadioConvertSpecific.Location = new System.Drawing.Point(7, 26);
            this.RadioConvertSpecific.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RadioConvertSpecific.Name = "RadioConvertSpecific";
            this.RadioConvertSpecific.Size = new System.Drawing.Size(349, 24);
            this.RadioConvertSpecific.TabIndex = 0;
            this.RadioConvertSpecific.TabStop = true;
            this.RadioConvertSpecific.Text = "Convert only if game specific data already exists";
            this.RadioConvertSpecific.UseVisualStyleBackColor = true;
            // 
            // GrpWarning
            // 
            this.GrpWarning.Controls.Add(this.ChkWarning);
            this.GrpWarning.Location = new System.Drawing.Point(12, 322);
            this.GrpWarning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpWarning.Name = "GrpWarning";
            this.GrpWarning.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GrpWarning.Size = new System.Drawing.Size(383, 72);
            this.GrpWarning.TabIndex = 8;
            this.GrpWarning.TabStop = false;
            this.GrpWarning.Text = "WARNING!";
            // 
            // ChkWarning
            // 
            this.ChkWarning.AutoSize = true;
            this.ChkWarning.Location = new System.Drawing.Point(75, 28);
            this.ChkWarning.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChkWarning.Name = "ChkWarning";
            this.ChkWarning.Size = new System.Drawing.Size(226, 24);
            this.ChkWarning.TabIndex = 0;
            this.ChkWarning.Text = "Read and Accept before use!!";
            this.ChkWarning.UseVisualStyleBackColor = true;
            this.ChkWarning.Click += new System.EventHandler(this.ChkWarning_Click);
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 718);
            this.Controls.Add(this.GrpWarning);
            this.Controls.Add(this.GrpForceConv);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.TxtLog);
            this.Controls.Add(this.ComboBox);
            this.Controls.Add(this.LblBoxes);
            this.Controls.Add(this.BtnConnect);
            this.Controls.Add(this.GrpConnection);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewForm";
            this.ShowIcon = false;
            this.Text = "Home Plugin";
            this.GrpConnection.ResumeLayout(false);
            this.GrpConnection.PerformLayout();
            this.GrpForceConv.ResumeLayout(false);
            this.GrpForceConv.PerformLayout();
            this.GrpWarning.ResumeLayout(false);
            this.GrpWarning.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblAddress;
        private System.Windows.Forms.GroupBox GrpConnection;
        private System.Windows.Forms.RadioButton RadioWiFi;
        private System.Windows.Forms.RadioButton RadioUSB;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.Label LblPort;
        private System.Windows.Forms.TextBox TxtAddress;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.Label LblBoxes;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.RichTextBox TxtLog;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.ComponentModel.BackgroundWorker BackGroundWorker;
        private System.Windows.Forms.GroupBox GrpForceConv;
        private System.Windows.Forms.RadioButton RadioConvertForce;
        private System.Windows.Forms.RadioButton RadioConvertSpecific;
        private System.Windows.Forms.GroupBox GrpWarning;
        private System.Windows.Forms.CheckBox ChkWarning;
        private System.Windows.Forms.RadioButton RadioConvertAny;
    }
}