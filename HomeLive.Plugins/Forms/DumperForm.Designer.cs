namespace HomeLive.Plugins;

partial class DumperForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DumperForm));
        menuStrip = new MenuStrip();
        toolsToolStripMenuItem = new ToolStripMenuItem();
        decryptFromFilesToolStripMenuItem = new ToolStripMenuItem();
        encryptFromFilesToolStripMenuItem = new ToolStripMenuItem();
        loadFileToEditorToolStripMenuItem = new ToolStripMenuItem();
        loadFilesToBoxesToolStripMenuItem = new ToolStripMenuItem();
        loadFolderToBoxesToolStripMenuItem = new ToolStripMenuItem();
        grpConnection = new GroupBox();
        numPort = new NumericUpDown();
        lblIP = new Label();
        radioUSB = new RadioButton();
        lblPort = new Label();
        txtBoxIP = new TextBox();
        radioWiFi = new RadioButton();
        grpAction = new GroupBox();
        comboSlot = new ComboBox();
        chkBoxFolders = new CheckBox();
        comboBox = new ComboBox();
        radioTargetAll = new RadioButton();
        radioBox = new RadioButton();
        radioSlot = new RadioButton();
        btnConnect = new Button();
        grpPath = new GroupBox();
        btnBrowse = new Button();
        txtBoxPath = new TextBox();
        progressBar = new ProgressBar();
        folderBrowser = new FolderBrowserDialog();
        grpDump = new GroupBox();
        radioEncAndDec = new RadioButton();
        radioDecrypted = new RadioButton();
        radioEncrypted = new RadioButton();
        openFileDialog = new OpenFileDialog();
        menuStrip.SuspendLayout();
        grpConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        grpAction.SuspendLayout();
        grpPath.SuspendLayout();
        grpDump.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.BackColor = SystemColors.MenuBar;
        menuStrip.ImageScalingSize = new Size(20, 20);
        menuStrip.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(512, 28);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip1";
        // 
        // toolsToolStripMenuItem
        // 
        toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { decryptFromFilesToolStripMenuItem, encryptFromFilesToolStripMenuItem, loadFileToEditorToolStripMenuItem, loadFilesToBoxesToolStripMenuItem, loadFolderToBoxesToolStripMenuItem });
        toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        toolsToolStripMenuItem.Size = new Size(58, 24);
        toolsToolStripMenuItem.Text = "Tools";
        // 
        // decryptFromFilesToolStripMenuItem
        // 
        decryptFromFilesToolStripMenuItem.Name = "decryptFromFilesToolStripMenuItem";
        decryptFromFilesToolStripMenuItem.Size = new Size(230, 26);
        decryptFromFilesToolStripMenuItem.Text = "Decrypt from files";
        decryptFromFilesToolStripMenuItem.Click += DecryptFromFiles_Click;
        // 
        // encryptFromFilesToolStripMenuItem
        // 
        encryptFromFilesToolStripMenuItem.Name = "encryptFromFilesToolStripMenuItem";
        encryptFromFilesToolStripMenuItem.Size = new Size(230, 26);
        encryptFromFilesToolStripMenuItem.Text = "Encrypt from files";
        encryptFromFilesToolStripMenuItem.Click += EncryptFromFiles_Click;
        // 
        // loadFileToEditorToolStripMenuItem
        // 
        loadFileToEditorToolStripMenuItem.Name = "loadFileToEditorToolStripMenuItem";
        loadFileToEditorToolStripMenuItem.Size = new Size(230, 26);
        loadFileToEditorToolStripMenuItem.Text = "Load file to Editor";
        loadFileToEditorToolStripMenuItem.Click += LoadFileToEditor_Click;
        // 
        // loadFilesToBoxesToolStripMenuItem
        // 
        loadFilesToBoxesToolStripMenuItem.Name = "loadFilesToBoxesToolStripMenuItem";
        loadFilesToBoxesToolStripMenuItem.Size = new Size(230, 26);
        loadFilesToBoxesToolStripMenuItem.Text = "Load files to Boxes";
        loadFilesToBoxesToolStripMenuItem.Click += LoadFilesToBoxes_Click;
        // 
        // loadFolderToBoxesToolStripMenuItem
        // 
        loadFolderToBoxesToolStripMenuItem.Name = "loadFolderToBoxesToolStripMenuItem";
        loadFolderToBoxesToolStripMenuItem.Size = new Size(230, 26);
        loadFolderToBoxesToolStripMenuItem.Text = "Load folder to Boxes";
        loadFolderToBoxesToolStripMenuItem.Click += LoadFolderToBoxes_Click;
        // 
        // grpConnection
        // 
        grpConnection.Controls.Add(numPort);
        grpConnection.Controls.Add(lblIP);
        grpConnection.Controls.Add(radioUSB);
        grpConnection.Controls.Add(lblPort);
        grpConnection.Controls.Add(txtBoxIP);
        grpConnection.Controls.Add(radioWiFi);
        grpConnection.Location = new Point(12, 31);
        grpConnection.Name = "grpConnection";
        grpConnection.Size = new Size(488, 125);
        grpConnection.TabIndex = 1;
        grpConnection.TabStop = false;
        grpConnection.Text = "Connection";
        // 
        // numPort
        // 
        numPort.Location = new Point(394, 72);
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
        radioUSB.CheckedChanged += USB_CheckedChanged;
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Location = new Point(343, 74);
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
        radioWiFi.CheckedChanged += WiFi_CheckedChanged;
        // 
        // grpAction
        // 
        grpAction.Controls.Add(comboSlot);
        grpAction.Controls.Add(chkBoxFolders);
        grpAction.Controls.Add(comboBox);
        grpAction.Controls.Add(radioTargetAll);
        grpAction.Controls.Add(radioBox);
        grpAction.Controls.Add(radioSlot);
        grpAction.Location = new Point(12, 162);
        grpAction.Name = "grpAction";
        grpAction.Size = new Size(488, 125);
        grpAction.TabIndex = 2;
        grpAction.TabStop = false;
        grpAction.Text = "Dump Target";
        // 
        // comboSlot
        // 
        comboSlot.FormattingEnabled = true;
        comboSlot.Location = new Point(159, 79);
        comboSlot.Name = "comboSlot";
        comboSlot.Size = new Size(107, 28);
        comboSlot.TabIndex = 7;
        // 
        // chkBoxFolders
        // 
        chkBoxFolders.AutoSize = true;
        chkBoxFolders.Enabled = false;
        chkBoxFolders.Font = new Font("Segoe UI", 7.8F);
        chkBoxFolders.Location = new Point(293, 83);
        chkBoxFolders.Name = "chkBoxFolders";
        chkBoxFolders.Size = new Size(173, 21);
        chkBoxFolders.TabIndex = 8;
        chkBoxFolders.Text = "Create folders for boxes";
        chkBoxFolders.UseVisualStyleBackColor = true;
        // 
        // comboBox
        // 
        comboBox.FormattingEnabled = true;
        comboBox.Location = new Point(6, 79);
        comboBox.Name = "comboBox";
        comboBox.Size = new Size(107, 28);
        comboBox.TabIndex = 6;
        // 
        // radioTargetAll
        // 
        radioTargetAll.AutoSize = true;
        radioTargetAll.Location = new Point(303, 36);
        radioTargetAll.Name = "radioTargetAll";
        radioTargetAll.Size = new Size(156, 24);
        radioTargetAll.TabIndex = 2;
        radioTargetAll.TabStop = true;
        radioTargetAll.Text = "All Boxes and Slots";
        radioTargetAll.UseVisualStyleBackColor = true;
        radioTargetAll.CheckedChanged += RadioTargetAll_CheckedChanged;
        // 
        // radioBox
        // 
        radioBox.AutoSize = true;
        radioBox.Location = new Point(6, 36);
        radioBox.Name = "radioBox";
        radioBox.Size = new Size(111, 24);
        radioBox.TabIndex = 0;
        radioBox.TabStop = true;
        radioBox.Text = "Specific Box";
        radioBox.UseVisualStyleBackColor = true;
        radioBox.CheckedChanged += RadioBox_CheckedChanged;
        // 
        // radioSlot
        // 
        radioSlot.AutoSize = true;
        radioSlot.Location = new Point(159, 36);
        radioSlot.Name = "radioSlot";
        radioSlot.Size = new Size(112, 24);
        radioSlot.TabIndex = 1;
        radioSlot.TabStop = true;
        radioSlot.Text = "Specific Slot";
        radioSlot.UseVisualStyleBackColor = true;
        radioSlot.CheckedChanged += RadioSlot_CheckedChanged;
        // 
        // btnConnect
        // 
        btnConnect.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
        btnConnect.Location = new Point(12, 440);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(488, 57);
        btnConnect.TabIndex = 3;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        btnConnect.Click += BtnConnect_Click;
        // 
        // grpPath
        // 
        grpPath.Controls.Add(btnBrowse);
        grpPath.Controls.Add(txtBoxPath);
        grpPath.Location = new Point(12, 365);
        grpPath.Name = "grpPath";
        grpPath.Size = new Size(488, 69);
        grpPath.TabIndex = 6;
        grpPath.TabStop = false;
        grpPath.Text = "Dump Folder Path";
        // 
        // btnBrowse
        // 
        btnBrowse.Location = new Point(384, 26);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(82, 29);
        btnBrowse.TabIndex = 7;
        btnBrowse.Text = "Browse";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += BtnBrowse_Click;
        // 
        // txtBoxPath
        // 
        txtBoxPath.Location = new Point(7, 26);
        txtBoxPath.Name = "txtBoxPath";
        txtBoxPath.Size = new Size(365, 27);
        txtBoxPath.TabIndex = 0;
        // 
        // progressBar
        // 
        progressBar.Location = new Point(12, 503);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(488, 29);
        progressBar.TabIndex = 7;
        // 
        // folderBrowser
        // 
        folderBrowser.Description = "Folder where the files will be saved";
        // 
        // grpDump
        // 
        grpDump.Controls.Add(radioEncAndDec);
        grpDump.Controls.Add(radioDecrypted);
        grpDump.Controls.Add(radioEncrypted);
        grpDump.Location = new Point(12, 293);
        grpDump.Name = "grpDump";
        grpDump.Size = new Size(488, 66);
        grpDump.TabIndex = 8;
        grpDump.TabStop = false;
        grpDump.Text = "Dump Format";
        // 
        // radioEncAndDec
        // 
        radioEncAndDec.AutoSize = true;
        radioEncAndDec.Location = new Point(261, 26);
        radioEncAndDec.Name = "radioEncAndDec";
        radioEncAndDec.Size = new Size(198, 24);
        radioEncAndDec.TabIndex = 2;
        radioEncAndDec.TabStop = true;
        radioEncAndDec.Text = "Encrypted and Decrypted";
        radioEncAndDec.UseVisualStyleBackColor = true;
        // 
        // radioDecrypted
        // 
        radioDecrypted.AutoSize = true;
        radioDecrypted.Location = new Point(143, 26);
        radioDecrypted.Name = "radioDecrypted";
        radioDecrypted.Size = new Size(99, 24);
        radioDecrypted.TabIndex = 1;
        radioDecrypted.TabStop = true;
        radioDecrypted.Text = "Decrypted";
        radioDecrypted.UseVisualStyleBackColor = true;
        // 
        // radioEncrypted
        // 
        radioEncrypted.AutoSize = true;
        radioEncrypted.Location = new Point(7, 26);
        radioEncrypted.Name = "radioEncrypted";
        radioEncrypted.Size = new Size(96, 24);
        radioEncrypted.TabIndex = 0;
        radioEncrypted.TabStop = true;
        radioEncrypted.Text = "Encrypted";
        radioEncrypted.UseVisualStyleBackColor = true;
        // 
        // openFileDialog
        // 
        openFileDialog.Multiselect = true;
        // 
        // DumperForm
        // 
        AcceptButton = btnConnect;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(512, 539);
        Controls.Add(grpDump);
        Controls.Add(progressBar);
        Controls.Add(grpPath);
        Controls.Add(btnConnect);
        Controls.Add(grpAction);
        Controls.Add(grpConnection);
        Controls.Add(menuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DumperForm";
        Text = "Home Live Dumper Plugin";
        FormClosing += Form_FormClosing;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        grpConnection.ResumeLayout(false);
        grpConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        grpAction.ResumeLayout(false);
        grpAction.PerformLayout();
        grpPath.ResumeLayout(false);
        grpPath.PerformLayout();
        grpDump.ResumeLayout(false);
        grpDump.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem decryptFromFilesToolStripMenuItem;
    private ToolStripMenuItem encryptFromFilesToolStripMenuItem;
    private ToolStripMenuItem loadFileToEditorToolStripMenuItem;
    private ToolStripMenuItem loadFilesToBoxesToolStripMenuItem;
    private GroupBox grpConnection;
    private TextBox txtBoxIP;
    private Label lblPort;
    private Label lblIP;
    private RadioButton radioUSB;
    private RadioButton radioWiFi;
    private GroupBox grpAction;
    private RadioButton radioTargetAll;
    private RadioButton radioSlot;
    private RadioButton radioBox;
    private ComboBox comboSlot;
    private CheckBox chkBoxFolders;
    private ComboBox comboBox;
    private Button btnConnect;
    private GroupBox grpPath;
    private Button btnBrowse;
    private TextBox txtBoxPath;
    private ProgressBar progressBar;
    private FolderBrowserDialog folderBrowser;
    private NumericUpDown numPort;
    private GroupBox grpDump;
    private RadioButton radioEncAndDec;
    private RadioButton radioDecrypted;
    private RadioButton radioEncrypted;
    private OpenFileDialog openFileDialog;
    private ToolStripMenuItem loadFolderToBoxesToolStripMenuItem;
}