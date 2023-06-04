namespace HomeLive.Plugins;

partial class SettingsForm
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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
        lblLatestIP = new Label();
        lblLatestPort = new Label();
        lblPreferredProtocol = new Label();
        lblLatestPath = new Label();
        lblLegality = new Label();
        txtBoxIP = new TextBox();
        numPort = new NumericUpDown();
        txtBoxPath = new TextBox();
        cmbProtocol = new ComboBox();
        cmbLegality = new ComboBox();
        btnCancel = new Button();
        btnApply = new Button();
        toolTipLegality = new ToolTip(components);
        btnPath = new Button();
        folderBrowser = new FolderBrowserDialog();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        SuspendLayout();
        // 
        // lblLatestIP
        // 
        lblLatestIP.AutoSize = true;
        lblLatestIP.Location = new Point(10, 19);
        lblLatestIP.Name = "lblLatestIP";
        lblLatestIP.Size = new Size(61, 15);
        lblLatestIP.TabIndex = 0;
        lblLatestIP.Text = "Default IP:";
        // 
        // lblLatestPort
        // 
        lblLatestPort.AutoSize = true;
        lblLatestPort.Location = new Point(10, 43);
        lblLatestPort.Name = "lblLatestPort";
        lblLatestPort.Size = new Size(73, 15);
        lblLatestPort.TabIndex = 1;
        lblLatestPort.Text = "Default Port:";
        // 
        // lblPreferredProtocol
        // 
        lblPreferredProtocol.AutoSize = true;
        lblPreferredProtocol.Location = new Point(10, 93);
        lblPreferredProtocol.Name = "lblPreferredProtocol";
        lblPreferredProtocol.Size = new Size(96, 15);
        lblPreferredProtocol.TabIndex = 2;
        lblPreferredProtocol.Text = "Default Protocol:";
        // 
        // lblLatestPath
        // 
        lblLatestPath.AutoSize = true;
        lblLatestPath.Location = new Point(10, 68);
        lblLatestPath.Name = "lblLatestPath";
        lblLatestPath.Size = new Size(75, 15);
        lblLatestPath.TabIndex = 3;
        lblLatestPath.Text = "Default Path:";
        // 
        // lblLegality
        // 
        lblLegality.AutoSize = true;
        lblLegality.Location = new Point(10, 118);
        lblLegality.Name = "lblLegality";
        lblLegality.Size = new Size(98, 15);
        lblLegality.TabIndex = 4;
        lblLegality.Text = "Auto Fix Legality:";
        // 
        // txtBoxIP
        // 
        txtBoxIP.Location = new Point(134, 16);
        txtBoxIP.Margin = new Padding(3, 2, 3, 2);
        txtBoxIP.Name = "txtBoxIP";
        txtBoxIP.Size = new Size(217, 23);
        txtBoxIP.TabIndex = 5;
        txtBoxIP.Text = "192.168.1.1";
        txtBoxIP.KeyPress += TxtBoxIP_KeyPress;
        // 
        // numPort
        // 
        numPort.Location = new Point(134, 41);
        numPort.Margin = new Padding(3, 2, 3, 2);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(216, 23);
        numPort.TabIndex = 6;
        numPort.Value = new decimal(new int[] { 6000, 0, 0, 0 });
        // 
        // txtBoxPath
        // 
        txtBoxPath.Location = new Point(134, 66);
        txtBoxPath.Margin = new Padding(3, 2, 3, 2);
        txtBoxPath.Name = "txtBoxPath";
        txtBoxPath.Size = new Size(185, 23);
        txtBoxPath.TabIndex = 7;
        // 
        // cmbProtocol
        // 
        cmbProtocol.FormattingEnabled = true;
        cmbProtocol.Items.AddRange(new object[] { "WiFi", "USB" });
        cmbProtocol.Location = new Point(134, 91);
        cmbProtocol.Margin = new Padding(3, 2, 3, 2);
        cmbProtocol.Name = "cmbProtocol";
        cmbProtocol.Size = new Size(217, 23);
        cmbProtocol.TabIndex = 8;
        // 
        // cmbLegality
        // 
        cmbLegality.FormattingEnabled = true;
        cmbLegality.Items.AddRange(new object[] { "True", "False" });
        cmbLegality.Location = new Point(134, 116);
        cmbLegality.Margin = new Padding(3, 2, 3, 2);
        cmbLegality.Name = "cmbLegality";
        cmbLegality.Size = new Size(217, 23);
        cmbLegality.TabIndex = 9;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(10, 149);
        btnCancel.Margin = new Padding(3, 2, 3, 2);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(167, 34);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // btnApply
        // 
        btnApply.Location = new Point(183, 149);
        btnApply.Margin = new Padding(3, 2, 3, 2);
        btnApply.Name = "btnApply";
        btnApply.Size = new Size(167, 34);
        btnApply.TabIndex = 11;
        btnApply.Text = "Apply";
        btnApply.UseVisualStyleBackColor = true;
        btnApply.Click += BtnApply_Click;
        // 
        // toolTipLegality
        // 
        toolTipLegality.AutoPopDelay = 5000;
        toolTipLegality.InitialDelay = 10;
        toolTipLegality.IsBalloon = true;
        toolTipLegality.ReshowDelay = 100;
        toolTipLegality.ShowAlways = true;
        toolTipLegality.ToolTipIcon = ToolTipIcon.Info;
        toolTipLegality.ToolTipTitle = "Auto Fix Legality";
        // 
        // btnPath
        // 
        btnPath.Location = new Point(325, 66);
        btnPath.Margin = new Padding(3, 2, 3, 2);
        btnPath.Name = "btnPath";
        btnPath.Size = new Size(26, 23);
        btnPath.TabIndex = 12;
        btnPath.Text = "...";
        btnPath.UseVisualStyleBackColor = true;
        btnPath.Click += BtnPath_Click;
        // 
        // folderBrowser
        // 
        folderBrowser.Description = "Folder where the files will be saved";
        // 
        // SettingsForm
        // 
        AcceptButton = btnApply;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(363, 190);
        Controls.Add(btnPath);
        Controls.Add(btnApply);
        Controls.Add(btnCancel);
        Controls.Add(cmbLegality);
        Controls.Add(cmbProtocol);
        Controls.Add(txtBoxPath);
        Controls.Add(numPort);
        Controls.Add(txtBoxIP);
        Controls.Add(lblLegality);
        Controls.Add(lblLatestPath);
        Controls.Add(lblPreferredProtocol);
        Controls.Add(lblLatestPort);
        Controls.Add(lblLatestIP);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(3, 2, 3, 2);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingsForm";
        ShowInTaskbar = false;
        Text = "Home Live Plugins Settings";
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblLatestIP;
    private Label lblLatestPort;
    private Label lblPreferredProtocol;
    private Label lblLatestPath;
    private Label lblLegality;
    private TextBox txtBoxIP;
    private NumericUpDown numPort;
    private TextBox txtBoxPath;
    private ComboBox cmbProtocol;
    private ComboBox cmbLegality;
    private Button btnCancel;
    private Button btnApply;
    private ToolTip toolTipLegality;
    private Button btnPath;
    private FolderBrowserDialog folderBrowser;
}