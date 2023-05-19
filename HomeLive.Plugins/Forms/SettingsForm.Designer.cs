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
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        SuspendLayout();
        // 
        // lblLatestIP
        // 
        lblLatestIP.AutoSize = true;
        lblLatestIP.Location = new Point(12, 25);
        lblLatestIP.Name = "lblLatestIP";
        lblLatestIP.Size = new Size(77, 20);
        lblLatestIP.TabIndex = 0;
        lblLatestIP.Text = "Default IP:";
        // 
        // lblLatestPort
        // 
        lblLatestPort.AutoSize = true;
        lblLatestPort.Location = new Point(12, 57);
        lblLatestPort.Name = "lblLatestPort";
        lblLatestPort.Size = new Size(91, 20);
        lblLatestPort.TabIndex = 1;
        lblLatestPort.Text = "Default Port:";
        // 
        // lblPreferredProtocol
        // 
        lblPreferredProtocol.AutoSize = true;
        lblPreferredProtocol.Location = new Point(12, 124);
        lblPreferredProtocol.Name = "lblPreferredProtocol";
        lblPreferredProtocol.Size = new Size(121, 20);
        lblPreferredProtocol.TabIndex = 2;
        lblPreferredProtocol.Text = "Default Protocol:";
        // 
        // lblLatestPath
        // 
        lblLatestPath.AutoSize = true;
        lblLatestPath.Location = new Point(12, 91);
        lblLatestPath.Name = "lblLatestPath";
        lblLatestPath.Size = new Size(93, 20);
        lblLatestPath.TabIndex = 3;
        lblLatestPath.Text = "Default Path:";
        // 
        // lblLegality
        // 
        lblLegality.AutoSize = true;
        lblLegality.Location = new Point(12, 158);
        lblLegality.Name = "lblLegality";
        lblLegality.Size = new Size(122, 20);
        lblLegality.TabIndex = 4;
        lblLegality.Text = "Auto Fix Legality:";
        // 
        // txtBoxIP
        // 
        txtBoxIP.Location = new Point(153, 22);
        txtBoxIP.Name = "txtBoxIP";
        txtBoxIP.Size = new Size(247, 27);
        txtBoxIP.TabIndex = 5;
        txtBoxIP.Text = "192.168.1.1";
        txtBoxIP.KeyPress += TxtBoxIP_KeyPress;
        // 
        // numPort
        // 
        numPort.Location = new Point(153, 55);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(247, 27);
        numPort.TabIndex = 6;
        numPort.Value = new decimal(new int[] { 6000, 0, 0, 0 });
        // 
        // txtBoxPath
        // 
        txtBoxPath.Location = new Point(153, 88);
        txtBoxPath.Name = "txtBoxPath";
        txtBoxPath.Size = new Size(247, 27);
        txtBoxPath.TabIndex = 7;
        // 
        // cmbProtocol
        // 
        cmbProtocol.FormattingEnabled = true;
        cmbProtocol.Items.AddRange(new object[] { "WiFi", "USB" });
        cmbProtocol.Location = new Point(153, 121);
        cmbProtocol.Name = "cmbProtocol";
        cmbProtocol.Size = new Size(247, 28);
        cmbProtocol.TabIndex = 8;
        // 
        // cmbLegality
        // 
        cmbLegality.FormattingEnabled = true;
        cmbLegality.Items.AddRange(new object[] { "True", "False" });
        cmbLegality.Location = new Point(153, 155);
        cmbLegality.Name = "cmbLegality";
        cmbLegality.Size = new Size(247, 28);
        cmbLegality.TabIndex = 9;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(12, 210);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(191, 45);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // btnApply
        // 
        btnApply.Location = new Point(209, 210);
        btnApply.Name = "btnApply";
        btnApply.Size = new Size(191, 45);
        btnApply.TabIndex = 11;
        btnApply.Text = "Apply";
        btnApply.UseVisualStyleBackColor = true;
        btnApply.Click += BtnApply_Click;
        // 
        // SettingsForm
        // 
        AcceptButton = btnApply;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(415, 267);
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
}