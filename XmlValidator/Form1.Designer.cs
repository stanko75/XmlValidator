namespace XmlValidator
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslNumOfXmlLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslNumOfXmlCnt = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslNumOfXslLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslNumOfXslCnt = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssCiiStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssUblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlConfiguration = new System.Windows.Forms.Panel();
            this.cbOnlyCII = new System.Windows.Forms.CheckBox();
            this.pnlLocationOfDownload = new System.Windows.Forms.Panel();
            this.splitter7 = new System.Windows.Forms.Splitter();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tbLocationOfDownload = new System.Windows.Forms.TextBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlUblXsdFilesLocation = new System.Windows.Forms.Panel();
            this.splitter6 = new System.Windows.Forms.Splitter();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbUblXsdFilesLocation = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlCiiXsdFilesLocation = new System.Windows.Forms.Panel();
            this.splitter5 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tbCiiXsdFilesLocation = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlXslFileOrFolder = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tbXslFileOrFolderName = new System.Windows.Forms.TextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlXmlFileOrFolder = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.pnlTxtXmlFileOrFolder = new System.Windows.Forms.Panel();
            this.tbXmlFileOrFolderName = new System.Windows.Forms.TextBox();
            this.pnlLblXmlFileOrFolder = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.dgvXmlFiles = new System.Windows.Forms.DataGridView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dgvXslFiles = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSaveToXml = new System.Windows.Forms.Button();
            this.btnLoadFromXml = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCopyGoodFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnDownloadCiiXsdFiles = new System.Windows.Forms.Button();
            this.btnDownloadUblXsdFiles = new System.Windows.Forms.Button();
            this.btnDownloadXslFiles = new System.Windows.Forms.Button();
            this.btnDownloadKositValidatorConfiguration = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.pnlConfiguration.SuspendLayout();
            this.pnlLocationOfDownload.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlUblXsdFilesLocation.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.pnlCiiXsdFilesLocation.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlXslFileOrFolder.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlXmlFileOrFolder.SuspendLayout();
            this.pnlTxtXmlFileOrFolder.SuspendLayout();
            this.pnlLblXmlFileOrFolder.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXmlFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXslFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslNumOfXmlLbl,
            this.tsslNumOfXmlCnt,
            this.tsslNumOfXslLbl,
            this.tsslNumOfXslCnt,
            this.tssCiiStatus,
            this.tssUblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 701);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "Number of XML files";
            // 
            // tsslNumOfXmlLbl
            // 
            this.tsslNumOfXmlLbl.Name = "tsslNumOfXmlLbl";
            this.tsslNumOfXmlLbl.Size = new System.Drawing.Size(119, 17);
            this.tsslNumOfXmlLbl.Text = "Number of XML files:";
            // 
            // tsslNumOfXmlCnt
            // 
            this.tsslNumOfXmlCnt.Name = "tsslNumOfXmlCnt";
            this.tsslNumOfXmlCnt.Size = new System.Drawing.Size(13, 17);
            this.tsslNumOfXmlCnt.Text = "0";
            // 
            // tsslNumOfXslLbl
            // 
            this.tsslNumOfXslLbl.Name = "tsslNumOfXslLbl";
            this.tsslNumOfXslLbl.Size = new System.Drawing.Size(111, 17);
            this.tsslNumOfXslLbl.Text = "Number of XSL files";
            // 
            // tsslNumOfXslCnt
            // 
            this.tsslNumOfXslCnt.Name = "tsslNumOfXslCnt";
            this.tsslNumOfXslCnt.Size = new System.Drawing.Size(13, 17);
            this.tsslNumOfXslCnt.Text = "0";
            // 
            // tssCiiStatus
            // 
            this.tssCiiStatus.Name = "tssCiiStatus";
            this.tssCiiStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // tssUblStatus
            // 
            this.tssUblStatus.Name = "tssUblStatus";
            this.tssUblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnStart.Location = new System.Drawing.Point(0, 471);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(800, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlConfiguration
            // 
            this.pnlConfiguration.Controls.Add(this.cbOnlyCII);
            this.pnlConfiguration.Controls.Add(this.pnlLocationOfDownload);
            this.pnlConfiguration.Controls.Add(this.pnlUblXsdFilesLocation);
            this.pnlConfiguration.Controls.Add(this.pnlCiiXsdFilesLocation);
            this.pnlConfiguration.Controls.Add(this.pnlXslFileOrFolder);
            this.pnlConfiguration.Controls.Add(this.pnlXmlFileOrFolder);
            this.pnlConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConfiguration.Location = new System.Drawing.Point(0, 0);
            this.pnlConfiguration.Name = "pnlConfiguration";
            this.pnlConfiguration.Size = new System.Drawing.Size(800, 179);
            this.pnlConfiguration.TabIndex = 9;
            // 
            // cbOnlyCII
            // 
            this.cbOnlyCII.AutoSize = true;
            this.cbOnlyCII.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbOnlyCII.Location = new System.Drawing.Point(0, 155);
            this.cbOnlyCII.Name = "cbOnlyCII";
            this.cbOnlyCII.Size = new System.Drawing.Size(800, 17);
            this.cbOnlyCII.TabIndex = 5;
            this.cbOnlyCII.Text = "Test only CII files";
            this.cbOnlyCII.UseVisualStyleBackColor = true;
            // 
            // pnlLocationOfDownload
            // 
            this.pnlLocationOfDownload.Controls.Add(this.splitter7);
            this.pnlLocationOfDownload.Controls.Add(this.panel8);
            this.pnlLocationOfDownload.Controls.Add(this.panel9);
            this.pnlLocationOfDownload.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocationOfDownload.Location = new System.Drawing.Point(0, 123);
            this.pnlLocationOfDownload.Name = "pnlLocationOfDownload";
            this.pnlLocationOfDownload.Size = new System.Drawing.Size(800, 32);
            this.pnlLocationOfDownload.TabIndex = 9;
            // 
            // splitter7
            // 
            this.splitter7.Location = new System.Drawing.Point(131, 0);
            this.splitter7.Name = "splitter7";
            this.splitter7.Size = new System.Drawing.Size(3, 32);
            this.splitter7.TabIndex = 6;
            this.splitter7.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.tbLocationOfDownload);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(131, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(669, 32);
            this.panel8.TabIndex = 3;
            // 
            // tbLocationOfDownload
            // 
            this.tbLocationOfDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLocationOfDownload.Location = new System.Drawing.Point(0, 0);
            this.tbLocationOfDownload.Name = "tbLocationOfDownload";
            this.tbLocationOfDownload.Size = new System.Drawing.Size(669, 20);
            this.tbLocationOfDownload.TabIndex = 3;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label5);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(131, 32);
            this.panel9.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Location of download:";
            // 
            // pnlUblXsdFilesLocation
            // 
            this.pnlUblXsdFilesLocation.Controls.Add(this.splitter6);
            this.pnlUblXsdFilesLocation.Controls.Add(this.panel6);
            this.pnlUblXsdFilesLocation.Controls.Add(this.panel7);
            this.pnlUblXsdFilesLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUblXsdFilesLocation.Location = new System.Drawing.Point(0, 91);
            this.pnlUblXsdFilesLocation.Name = "pnlUblXsdFilesLocation";
            this.pnlUblXsdFilesLocation.Size = new System.Drawing.Size(800, 32);
            this.pnlUblXsdFilesLocation.TabIndex = 8;
            // 
            // splitter6
            // 
            this.splitter6.Location = new System.Drawing.Point(131, 0);
            this.splitter6.Name = "splitter6";
            this.splitter6.Size = new System.Drawing.Size(3, 32);
            this.splitter6.TabIndex = 6;
            this.splitter6.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tbUblXsdFilesLocation);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(131, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(669, 32);
            this.panel6.TabIndex = 3;
            // 
            // tbUblXsdFilesLocation
            // 
            this.tbUblXsdFilesLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUblXsdFilesLocation.Location = new System.Drawing.Point(0, 0);
            this.tbUblXsdFilesLocation.Name = "tbUblXsdFilesLocation";
            this.tbUblXsdFilesLocation.Size = new System.Drawing.Size(669, 20);
            this.tbUblXsdFilesLocation.TabIndex = 3;
            this.tbUblXsdFilesLocation.Leave += new System.EventHandler(this.tbUblXsdFilesLocation_Leave);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(131, 32);
            this.panel7.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Location of UBL XSD files:";
            // 
            // pnlCiiXsdFilesLocation
            // 
            this.pnlCiiXsdFilesLocation.Controls.Add(this.splitter5);
            this.pnlCiiXsdFilesLocation.Controls.Add(this.panel4);
            this.pnlCiiXsdFilesLocation.Controls.Add(this.panel5);
            this.pnlCiiXsdFilesLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCiiXsdFilesLocation.Location = new System.Drawing.Point(0, 59);
            this.pnlCiiXsdFilesLocation.Name = "pnlCiiXsdFilesLocation";
            this.pnlCiiXsdFilesLocation.Size = new System.Drawing.Size(800, 32);
            this.pnlCiiXsdFilesLocation.TabIndex = 7;
            // 
            // splitter5
            // 
            this.splitter5.Location = new System.Drawing.Point(131, 0);
            this.splitter5.Name = "splitter5";
            this.splitter5.Size = new System.Drawing.Size(3, 32);
            this.splitter5.TabIndex = 6;
            this.splitter5.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tbCiiXsdFilesLocation);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(131, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(669, 32);
            this.panel4.TabIndex = 3;
            // 
            // tbCiiXsdFilesLocation
            // 
            this.tbCiiXsdFilesLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCiiXsdFilesLocation.Location = new System.Drawing.Point(0, 0);
            this.tbCiiXsdFilesLocation.Name = "tbCiiXsdFilesLocation";
            this.tbCiiXsdFilesLocation.Size = new System.Drawing.Size(669, 20);
            this.tbCiiXsdFilesLocation.TabIndex = 3;
            this.tbCiiXsdFilesLocation.Leave += new System.EventHandler(this.tbCiiXsdFilesLocation_Leave);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(131, 32);
            this.panel5.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Location of CII XSD files:";
            // 
            // pnlXslFileOrFolder
            // 
            this.pnlXslFileOrFolder.Controls.Add(this.panel3);
            this.pnlXslFileOrFolder.Controls.Add(this.panel2);
            this.pnlXslFileOrFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlXslFileOrFolder.Location = new System.Drawing.Point(0, 32);
            this.pnlXslFileOrFolder.Name = "pnlXslFileOrFolder";
            this.pnlXslFileOrFolder.Size = new System.Drawing.Size(800, 27);
            this.pnlXslFileOrFolder.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tbXslFileOrFolderName);
            this.panel3.Controls.Add(this.splitter3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(50, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(750, 27);
            this.panel3.TabIndex = 2;
            // 
            // tbXslFileOrFolderName
            // 
            this.tbXslFileOrFolderName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbXslFileOrFolderName.Location = new System.Drawing.Point(3, 0);
            this.tbXslFileOrFolderName.Name = "tbXslFileOrFolderName";
            this.tbXslFileOrFolderName.Size = new System.Drawing.Size(747, 20);
            this.tbXslFileOrFolderName.TabIndex = 6;
            this.tbXslFileOrFolderName.Leave += new System.EventHandler(this.tbXslFileOrFolderName_Leave);
            // 
            // splitter3
            // 
            this.splitter3.Location = new System.Drawing.Point(0, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(3, 27);
            this.splitter3.TabIndex = 5;
            this.splitter3.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 27);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Xsl files:";
            // 
            // pnlXmlFileOrFolder
            // 
            this.pnlXmlFileOrFolder.Controls.Add(this.splitter4);
            this.pnlXmlFileOrFolder.Controls.Add(this.pnlTxtXmlFileOrFolder);
            this.pnlXmlFileOrFolder.Controls.Add(this.pnlLblXmlFileOrFolder);
            this.pnlXmlFileOrFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlXmlFileOrFolder.Location = new System.Drawing.Point(0, 0);
            this.pnlXmlFileOrFolder.Name = "pnlXmlFileOrFolder";
            this.pnlXmlFileOrFolder.Size = new System.Drawing.Size(800, 32);
            this.pnlXmlFileOrFolder.TabIndex = 4;
            // 
            // splitter4
            // 
            this.splitter4.Location = new System.Drawing.Point(50, 0);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(3, 32);
            this.splitter4.TabIndex = 6;
            this.splitter4.TabStop = false;
            // 
            // pnlTxtXmlFileOrFolder
            // 
            this.pnlTxtXmlFileOrFolder.Controls.Add(this.tbXmlFileOrFolderName);
            this.pnlTxtXmlFileOrFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTxtXmlFileOrFolder.Location = new System.Drawing.Point(50, 0);
            this.pnlTxtXmlFileOrFolder.Name = "pnlTxtXmlFileOrFolder";
            this.pnlTxtXmlFileOrFolder.Size = new System.Drawing.Size(750, 32);
            this.pnlTxtXmlFileOrFolder.TabIndex = 3;
            // 
            // tbXmlFileOrFolderName
            // 
            this.tbXmlFileOrFolderName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbXmlFileOrFolderName.Location = new System.Drawing.Point(0, 0);
            this.tbXmlFileOrFolderName.Name = "tbXmlFileOrFolderName";
            this.tbXmlFileOrFolderName.Size = new System.Drawing.Size(750, 20);
            this.tbXmlFileOrFolderName.TabIndex = 3;
            this.tbXmlFileOrFolderName.Leave += new System.EventHandler(this.tbXmlFileOrFolderName_Leave);
            // 
            // pnlLblXmlFileOrFolder
            // 
            this.pnlLblXmlFileOrFolder.Controls.Add(this.label1);
            this.pnlLblXmlFileOrFolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLblXmlFileOrFolder.Location = new System.Drawing.Point(0, 0);
            this.pnlLblXmlFileOrFolder.Name = "pnlLblXmlFileOrFolder";
            this.pnlLblXmlFileOrFolder.Size = new System.Drawing.Size(50, 32);
            this.pnlLblXmlFileOrFolder.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Xml files:";
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.dgvXmlFiles);
            this.pnlGrids.Controls.Add(this.splitter1);
            this.pnlGrids.Controls.Add(this.dgvXslFiles);
            this.pnlGrids.Controls.Add(this.splitter2);
            this.pnlGrids.Controls.Add(this.dgvResult);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(0, 179);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(800, 292);
            this.pnlGrids.TabIndex = 10;
            // 
            // dgvXmlFiles
            // 
            this.dgvXmlFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvXmlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvXmlFiles.Location = new System.Drawing.Point(0, 0);
            this.dgvXmlFiles.Name = "dgvXmlFiles";
            this.dgvXmlFiles.Size = new System.Drawing.Size(800, 90);
            this.dgvXmlFiles.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 90);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(800, 8);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // dgvXslFiles
            // 
            this.dgvXslFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvXslFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvXslFiles.Location = new System.Drawing.Point(0, 98);
            this.dgvXslFiles.Name = "dgvXslFiles";
            this.dgvXslFiles.Size = new System.Drawing.Size(800, 76);
            this.dgvXslFiles.TabIndex = 3;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 174);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(800, 8);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvResult.Location = new System.Drawing.Point(0, 182);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.Size = new System.Drawing.Size(800, 110);
            this.dgvResult.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(0, 540);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(800, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClear.Location = new System.Drawing.Point(0, 563);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(800, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSaveToXml
            // 
            this.btnSaveToXml.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSaveToXml.Location = new System.Drawing.Point(0, 494);
            this.btnSaveToXml.Name = "btnSaveToXml";
            this.btnSaveToXml.Size = new System.Drawing.Size(800, 23);
            this.btnSaveToXml.TabIndex = 13;
            this.btnSaveToXml.Text = "Save to XML";
            this.btnSaveToXml.UseVisualStyleBackColor = true;
            this.btnSaveToXml.Click += new System.EventHandler(this.btnSaveToXml_Click);
            // 
            // btnLoadFromXml
            // 
            this.btnLoadFromXml.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLoadFromXml.Location = new System.Drawing.Point(0, 517);
            this.btnLoadFromXml.Name = "btnLoadFromXml";
            this.btnLoadFromXml.Size = new System.Drawing.Size(800, 23);
            this.btnLoadFromXml.TabIndex = 14;
            this.btnLoadFromXml.Text = "Load from XML";
            this.btnLoadFromXml.UseVisualStyleBackColor = true;
            this.btnLoadFromXml.Click += new System.EventHandler(this.btnLoadFromXml_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnCopyGoodFiles
            // 
            this.btnCopyGoodFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCopyGoodFiles.Location = new System.Drawing.Point(0, 586);
            this.btnCopyGoodFiles.Name = "btnCopyGoodFiles";
            this.btnCopyGoodFiles.Size = new System.Drawing.Size(800, 23);
            this.btnCopyGoodFiles.TabIndex = 15;
            this.btnCopyGoodFiles.Text = "Copy good files";
            this.btnCopyGoodFiles.UseVisualStyleBackColor = true;
            this.btnCopyGoodFiles.Click += new System.EventHandler(this.btnCopyGoodFiles_Click);
            // 
            // btnDownloadCiiXsdFiles
            // 
            this.btnDownloadCiiXsdFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDownloadCiiXsdFiles.Location = new System.Drawing.Point(0, 609);
            this.btnDownloadCiiXsdFiles.Name = "btnDownloadCiiXsdFiles";
            this.btnDownloadCiiXsdFiles.Size = new System.Drawing.Size(800, 23);
            this.btnDownloadCiiXsdFiles.TabIndex = 16;
            this.btnDownloadCiiXsdFiles.Text = "Download CII files for XSD validation from UNECE";
            this.btnDownloadCiiXsdFiles.UseVisualStyleBackColor = true;
            this.btnDownloadCiiXsdFiles.Click += new System.EventHandler(this.btnDownloadCiiXsdFiles_Click);
            // 
            // btnDownloadUblXsdFiles
            // 
            this.btnDownloadUblXsdFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDownloadUblXsdFiles.Location = new System.Drawing.Point(0, 632);
            this.btnDownloadUblXsdFiles.Name = "btnDownloadUblXsdFiles";
            this.btnDownloadUblXsdFiles.Size = new System.Drawing.Size(800, 23);
            this.btnDownloadUblXsdFiles.TabIndex = 17;
            this.btnDownloadUblXsdFiles.Text = "Download UBL files for XSD validation from OASIS Open";
            this.btnDownloadUblXsdFiles.UseVisualStyleBackColor = true;
            this.btnDownloadUblXsdFiles.Click += new System.EventHandler(this.btnDownloadUblXsdFiles_Click);
            // 
            // btnDownloadXslFiles
            // 
            this.btnDownloadXslFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDownloadXslFiles.Location = new System.Drawing.Point(0, 655);
            this.btnDownloadXslFiles.Name = "btnDownloadXslFiles";
            this.btnDownloadXslFiles.Size = new System.Drawing.Size(800, 23);
            this.btnDownloadXslFiles.TabIndex = 18;
            this.btnDownloadXslFiles.Text = "Download XSL files for validation";
            this.btnDownloadXslFiles.UseVisualStyleBackColor = true;
            this.btnDownloadXslFiles.Click += new System.EventHandler(this.btnDownloadXslFiles_Click);
            // 
            // btnDownloadKositValidatorConfiguration
            // 
            this.btnDownloadKositValidatorConfiguration.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDownloadKositValidatorConfiguration.Location = new System.Drawing.Point(0, 678);
            this.btnDownloadKositValidatorConfiguration.Name = "btnDownloadKositValidatorConfiguration";
            this.btnDownloadKositValidatorConfiguration.Size = new System.Drawing.Size(800, 23);
            this.btnDownloadKositValidatorConfiguration.TabIndex = 19;
            this.btnDownloadKositValidatorConfiguration.Text = "KoSIT Validator Configuration for XRechnung (it contains XSL and XSD)";
            this.btnDownloadKositValidatorConfiguration.UseVisualStyleBackColor = true;
            this.btnDownloadKositValidatorConfiguration.Click += new System.EventHandler(this.btnDownloadKositValidatorConfiguration_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 723);
            this.Controls.Add(this.pnlGrids);
            this.Controls.Add(this.pnlConfiguration);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSaveToXml);
            this.Controls.Add(this.btnLoadFromXml);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCopyGoodFiles);
            this.Controls.Add(this.btnDownloadCiiXsdFiles);
            this.Controls.Add(this.btnDownloadUblXsdFiles);
            this.Controls.Add(this.btnDownloadXslFiles);
            this.Controls.Add(this.btnDownloadKositValidatorConfiguration);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlConfiguration.ResumeLayout(false);
            this.pnlConfiguration.PerformLayout();
            this.pnlLocationOfDownload.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.pnlUblXsdFilesLocation.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.pnlCiiXsdFilesLocation.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.pnlXslFileOrFolder.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlXmlFileOrFolder.ResumeLayout(false);
            this.pnlTxtXmlFileOrFolder.ResumeLayout(false);
            this.pnlTxtXmlFileOrFolder.PerformLayout();
            this.pnlLblXmlFileOrFolder.ResumeLayout(false);
            this.pnlLblXmlFileOrFolder.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvXmlFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvXslFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslNumOfXmlLbl;
        private System.Windows.Forms.ToolStripStatusLabel tsslNumOfXmlCnt;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlConfiguration;
        private System.Windows.Forms.Panel pnlTxtXmlFileOrFolder;
        private System.Windows.Forms.Panel pnlLblXmlFileOrFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlGrids;
        private System.Windows.Forms.DataGridView dgvXmlFiles;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dgvXslFiles;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.ToolStripStatusLabel tsslNumOfXslLbl;
        private System.Windows.Forms.ToolStripStatusLabel tsslNumOfXslCnt;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSaveToXml;
        private System.Windows.Forms.Button btnLoadFromXml;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel pnlXmlFileOrFolder;
        private System.Windows.Forms.CheckBox cbOnlyCII;
        private System.Windows.Forms.Button btnCopyGoodFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnDownloadCiiXsdFiles;
        private System.Windows.Forms.ToolStripStatusLabel tssCiiStatus;
        private System.Windows.Forms.Button btnDownloadUblXsdFiles;
        private System.Windows.Forms.ToolStripStatusLabel tssUblStatus;
        private System.Windows.Forms.Button btnDownloadXslFiles;
        private System.Windows.Forms.Button btnDownloadKositValidatorConfiguration;
        private System.Windows.Forms.Panel pnlXslFileOrFolder;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbXslFileOrFolderName;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbXmlFileOrFolderName;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Panel pnlCiiXsdFilesLocation;
        private System.Windows.Forms.Splitter splitter5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox tbCiiXsdFilesLocation;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlUblXsdFilesLocation;
        private System.Windows.Forms.Splitter splitter6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbUblXsdFilesLocation;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlLocationOfDownload;
        private System.Windows.Forms.Splitter splitter7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox tbLocationOfDownload;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label5;
    }
}

