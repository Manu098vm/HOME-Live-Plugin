namespace HomeLive.Plugins;

partial class ViewerForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerForm));
        grpConnection = new GroupBox();
        numPort = new NumericUpDown();
        lblIP = new Label();
        radioUSB = new RadioButton();
        lblPort = new Label();
        txtBoxIP = new TextBox();
        radioWiFi = new RadioButton();
        grpForceConv = new GroupBox();
        radioConvertAny = new RadioButton();
        radioConvertForce = new RadioButton();
        radioConvertSpecific = new RadioButton();
        grpBoxes = new GroupBox();
        comboBox = new ComboBox();
        btnConnect = new Button();
        progressBar = new ProgressBar();
        grpSelectionType = new GroupBox();
        radioSBox = new RadioButton();
        radioBox = new RadioButton();
        radioSlot = new RadioButton();
        grpSlot = new GroupBox();
        lblSlot = new Label();
        lblBox = new Label();
        cmbSlot = new ComboBox();
        cmbBox = new ComboBox();
        grpConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        grpForceConv.SuspendLayout();
        grpBoxes.SuspendLayout();
        grpSelectionType.SuspendLayout();
        grpSlot.SuspendLayout();
        SuspendLayout();
        // 
        // grpConnection
        // 
        grpConnection.Controls.Add(numPort);
        grpConnection.Controls.Add(lblIP);
        grpConnection.Controls.Add(radioUSB);
        grpConnection.Controls.Add(lblPort);
        grpConnection.Controls.Add(txtBoxIP);
        grpConnection.Controls.Add(radioWiFi);
        grpConnection.Location = new Point(12, 12);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(466, 125);
        grpConnection.TabIndex = 2;
        grpConnection.TabStop = false;
        grpConnection.Text = "Connection";
        // 
        // numPort
        // 
        numPort.Location = new Point(387, 72);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(72, 27);
        numPort.TabIndex = 5;
        numPort.Value = new decimal(new int[] { 6000, 0, 0, 0 });
        // 
        // lblIP
        // 
        lblIP.AutoSize = true;
        lblIP.Location = new Point(6, 75);
        lblIP.Name = "lblIP";
        lblIP.Size = new Size(81, 20);
        lblIP.TabIndex = 2;
        lblIP.Text = "IP Address:";
        // 
        // radioUSB
        // 
        radioUSB.AutoSize = true;
        radioUSB.Location = new Point(279, 26);
        radioUSB.Name = "radioUSB";
        radioUSB.Size = new Size(57, 24);
        radioUSB.TabIndex = 1;
        radioUSB.TabStop = true;
        radioUSB.Text = "USB";
        radioUSB.UseVisualStyleBackColor = true;
        radioUSB.CheckedChanged += RadioUSB_CheckedChanged;
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Location = new Point(343, 75);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(38, 20);
        lblPort.TabIndex = 3;
        lblPort.Text = "Port:";
        // 
        // txtBoxIP
        // 
        txtBoxIP.Location = new Point(93, 72);
        txtBoxIP.Name = "txtBoxIP";
        txtBoxIP.Size = new Size(213, 27);
        txtBoxIP.TabIndex = 4;
        txtBoxIP.Text = "192.168.1.1";
        txtBoxIP.TextAlign = HorizontalAlignment.Right;
        txtBoxIP.KeyPress += TxtBoxIP_KeyPress;
        // 
        // radioWiFi
        // 
        radioWiFi.AutoSize = true;
        radioWiFi.Location = new Point(128, 26);
        radioWiFi.Name = "radioWiFi";
        radioWiFi.Size = new Size(65, 24);
        radioWiFi.TabIndex = 0;
        radioWiFi.TabStop = true;
        radioWiFi.Text = "Wi-Fi";
        radioWiFi.UseVisualStyleBackColor = true;
        radioWiFi.CheckedChanged += RadioWiFi_CheckedChanged;
        // 
        // grpForceConv
        // 
        grpForceConv.Controls.Add(radioConvertAny);
        grpForceConv.Controls.Add(radioConvertForce);
        grpForceConv.Controls.Add(radioConvertSpecific);
        grpForceConv.Location = new Point(12, 143);
        grpForceConv.Name = "grpForceConv";
        grpForceConv.Size = new Size(466, 129);
        grpForceConv.TabIndex = 3;
        grpForceConv.TabStop = false;
        grpForceConv.Text = "Conversion Method";
        // 
        // radioConvertAny
        // 
        radioConvertAny.AutoSize = true;
        radioConvertAny.Location = new Point(6, 86);
        radioConvertAny.Name = "radioConvertAny";
        radioConvertAny.Size = new Size(231, 24);
        radioConvertAny.TabIndex = 6;
        radioConvertAny.TabStop = true;
        radioConvertAny.Text = "Convert any PKM data (illegal)";
        radioConvertAny.UseVisualStyleBackColor = true;
        // 
        // radioConvertForce
        // 
        radioConvertForce.AutoSize = true;
        radioConvertForce.Location = new Point(6, 56);
        radioConvertForce.Name = "radioConvertForce";
        radioConvertForce.Size = new Size(418, 24);
        radioConvertForce.TabIndex = 5;
        radioConvertForce.TabStop = true;
        radioConvertForce.Text = "Convert any PKM data if compatible with the current game";
        radioConvertForce.UseVisualStyleBackColor = true;
        // 
        // radioConvertSpecific
        // 
        radioConvertSpecific.AutoSize = true;
        radioConvertSpecific.Location = new Point(6, 26);
        radioConvertSpecific.Name = "radioConvertSpecific";
        radioConvertSpecific.Size = new Size(349, 24);
        radioConvertSpecific.TabIndex = 4;
        radioConvertSpecific.TabStop = true;
        radioConvertSpecific.Text = "Convert only if game specific data already exists";
        radioConvertSpecific.UseVisualStyleBackColor = true;
        // 
        // grpBoxes
        // 
        grpBoxes.Controls.Add(comboBox);
        grpBoxes.Location = new Point(12, 347);
        grpBoxes.Name = "grpBoxes";
        grpBoxes.Size = new Size(466, 71);
        grpBoxes.TabIndex = 4;
        grpBoxes.TabStop = false;
        grpBoxes.Text = "Select Boxes:";
        // 
        // comboBox
        // 
        comboBox.FormattingEnabled = true;
        comboBox.Location = new Point(6, 26);
        comboBox.Name = "comboBox";
        comboBox.Size = new Size(453, 28);
        comboBox.TabIndex = 5;
        // 
        // btnConnect
        // 
        btnConnect.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
        btnConnect.Location = new Point(12, 424);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(466, 57);
        btnConnect.TabIndex = 5;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        btnConnect.Click += BtnConnect_Click;
        // 
        // progressBar
        // 
        progressBar.Location = new Point(12, 487);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(466, 29);
        progressBar.TabIndex = 8;
        // 
        // grpSelectionType
        // 
        grpSelectionType.Controls.Add(radioSBox);
        grpSelectionType.Controls.Add(radioBox);
        grpSelectionType.Controls.Add(radioSlot);
        grpSelectionType.Location = new Point(12, 278);
        grpSelectionType.Name = "grpSelectionType";
        grpSelectionType.Size = new Size(466, 63);
        grpSelectionType.TabIndex = 9;
        grpSelectionType.TabStop = false;
        grpSelectionType.Text = "Targets";
        // 
        // radioSBox
        // 
        radioSBox.AutoSize = true;
        radioSBox.Location = new Point(170, 26);
        radioSBox.Name = "radioSBox";
        radioSBox.Size = new Size(111, 24);
        radioSBox.TabIndex = 2;
        radioSBox.TabStop = true;
        radioSBox.Text = "Specific Box";
        radioSBox.UseVisualStyleBackColor = true;
        radioSBox.CheckedChanged += RadioSBox_CheckedChanged;
        // 
        // radioBox
        // 
        radioBox.AutoSize = true;
        radioBox.Location = new Point(6, 26);
        radioBox.Name = "radioBox";
        radioBox.Size = new Size(101, 24);
        radioBox.TabIndex = 1;
        radioBox.TabStop = true;
        radioBox.Text = "Box Range";
        radioBox.UseVisualStyleBackColor = true;
        radioBox.CheckedChanged += RadioBox_CheckedChanged;
        // 
        // radioSlot
        // 
        radioSlot.AutoSize = true;
        radioSlot.Location = new Point(347, 26);
        radioSlot.Name = "radioSlot";
        radioSlot.Size = new Size(112, 24);
        radioSlot.TabIndex = 0;
        radioSlot.TabStop = true;
        radioSlot.Text = "Specific Slot";
        radioSlot.UseVisualStyleBackColor = true;
        radioSlot.CheckedChanged += RadioSlot_CheckedChanged;
        // 
        // grpSlot
        // 
        grpSlot.Controls.Add(lblSlot);
        grpSlot.Controls.Add(lblBox);
        grpSlot.Controls.Add(cmbSlot);
        grpSlot.Controls.Add(cmbBox);
        grpSlot.Enabled = false;
        grpSlot.Location = new Point(12, 347);
        grpSlot.Name = "grpSlot";
        grpSlot.Size = new Size(466, 71);
        grpSlot.TabIndex = 10;
        grpSlot.TabStop = false;
        grpSlot.Text = "Select Slot";
        grpSlot.Visible = false;
        // 
        // lblSlot
        // 
        lblSlot.AutoSize = true;
        lblSlot.Location = new Point(264, 29);
        lblSlot.Name = "lblSlot";
        lblSlot.Size = new Size(38, 20);
        lblSlot.TabIndex = 9;
        lblSlot.Text = "Slot:";
        // 
        // lblBox
        // 
        lblBox.AutoSize = true;
        lblBox.Location = new Point(6, 29);
        lblBox.Name = "lblBox";
        lblBox.Size = new Size(37, 20);
        lblBox.TabIndex = 8;
        lblBox.Text = "Box:";
        // 
        // cmbSlot
        // 
        cmbSlot.FormattingEnabled = true;
        cmbSlot.Location = new Point(308, 26);
        cmbSlot.Name = "cmbSlot";
        cmbSlot.Size = new Size(151, 28);
        cmbSlot.TabIndex = 6;
        // 
        // cmbBox
        // 
        cmbBox.FormattingEnabled = true;
        cmbBox.Location = new Point(49, 26);
        cmbBox.Name = "cmbBox";
        cmbBox.Size = new Size(151, 28);
        cmbBox.TabIndex = 7;
        // 
        // ViewerForm
        // 
        AcceptButton = btnConnect;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(491, 532);
        Controls.Add(grpSlot);
        Controls.Add(grpSelectionType);
        Controls.Add(progressBar);
        Controls.Add(btnConnect);
        Controls.Add(grpBoxes);
        Controls.Add(grpForceConv);
        Controls.Add(grpConnection);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ViewerForm";
        Text = "Home Live Viewer Plugin";
        FormClosing += Form_FormClosing;
        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        grpForceConv.ResumeLayout(false);
        grpForceConv.PerformLayout();
        grpBoxes.ResumeLayout(false);
        grpSelectionType.ResumeLayout(false);
        grpSelectionType.PerformLayout();
        grpSlot.ResumeLayout(false);
        grpSlot.PerformLayout();
        ResumeLayout(false);
    }

    private void TxtBoxPort_KeyPress1(object sender, KeyPressEventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

    private GroupBox grpConnection;
    private Label lblIP;
    private RadioButton radioUSB;
    private Label lblPort;
    private TextBox txtBoxIP;
    private RadioButton radioWiFi;
    private GroupBox grpForceConv;
    private RadioButton radioConvertAny;
    private RadioButton radioConvertForce;
    private RadioButton radioConvertSpecific;
    private GroupBox grpBoxes;
    private ComboBox comboBox;
    private Button btnConnect;
    private ProgressBar progressBar;
    private NumericUpDown numPort;
    private GroupBox grpSelectionType;
    private RadioButton radioBox;
    private RadioButton radioSlot;
    private GroupBox grpSlot;
    private Label lblSlot;
    private Label lblBox;
    private ComboBox cmbSlot;
    private ComboBox cmbBox;
    private RadioButton radioSBox;
}