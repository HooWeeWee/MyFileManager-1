namespace MyFileManager
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.explorer = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.drivesComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCurrentDir = new System.Windows.Forms.TextBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonGo = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.buttonCut = new System.Windows.Forms.Button();
            this.buttonZip = new System.Windows.Forms.Button();
            this.buttonUnZip = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSecret = new System.Windows.Forms.Button();
            this.buttonSync = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.textBoxSearchMask = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTest1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.asdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mD5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.permissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aSyncDelegateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiThreadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parallelForEachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asyncawaitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typeDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asyncDownloaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cryptoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.explorer, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.drivesComboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1297, 358);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // explorer
            // 
            this.explorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.explorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnSize});
            this.explorer.FullRowSelect = true;
            this.explorer.GridLines = true;
            this.explorer.LabelEdit = true;
            this.explorer.Location = new System.Drawing.Point(3, 105);
            this.explorer.Name = "explorer";
            this.explorer.Size = new System.Drawing.Size(642, 190);
            this.explorer.TabIndex = 0;
            this.explorer.UseCompatibleStateImageBehavior = false;
            this.explorer.View = System.Windows.Forms.View.Details;
            this.explorer.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.explorer_AfterLabelEdit);
            this.explorer.ItemActivate += new System.EventHandler(this.explorer_ItemActivate);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 566;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 67;
            // 
            // drivesComboBox
            // 
            this.drivesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drivesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drivesComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.drivesComboBox.FormattingEnabled = true;
            this.drivesComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.drivesComboBox.Location = new System.Drawing.Point(3, 39);
            this.drivesComboBox.Name = "drivesComboBox";
            this.drivesComboBox.Size = new System.Drawing.Size(642, 24);
            this.drivesComboBox.TabIndex = 1;
            this.drivesComboBox.SelectionChangeCommitted += new System.EventHandler(this.drivesComboBox_SelectionChangeCommitted);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxCurrentDir, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonUp, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonGo, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 69);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(642, 30);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // textBoxCurrentDir
            // 
            this.textBoxCurrentDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrentDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxCurrentDir.Location = new System.Drawing.Point(33, 3);
            this.textBoxCurrentDir.Name = "textBoxCurrentDir";
            this.textBoxCurrentDir.Size = new System.Drawing.Size(576, 22);
            this.textBoxCurrentDir.TabIndex = 0;
            this.textBoxCurrentDir.Text = "drive:\\\\path\\path";
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(3, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(24, 23);
            this.buttonUp.TabIndex = 1;
            this.buttonUp.Text = "UP";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(615, 3);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(24, 23);
            this.buttonGo.TabIndex = 2;
            this.buttonGo.Text = "G";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 10;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.tableLayoutPanel3.Controls.Add(this.buttonCopy, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonPaste, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonCut, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonZip, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonUnZip, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonDelete, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSecret, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSync, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonDownload, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxSearchMask, 9, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(642, 30);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(3, 3);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(49, 23);
            this.buttonCopy.TabIndex = 0;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Location = new System.Drawing.Point(58, 3);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(49, 23);
            this.buttonPaste.TabIndex = 1;
            this.buttonPaste.Text = "Paste";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.buttonPaste_Click);
            // 
            // buttonCut
            // 
            this.buttonCut.Location = new System.Drawing.Point(113, 3);
            this.buttonCut.Name = "buttonCut";
            this.buttonCut.Size = new System.Drawing.Size(49, 23);
            this.buttonCut.TabIndex = 2;
            this.buttonCut.Text = "Cut";
            this.buttonCut.UseVisualStyleBackColor = true;
            this.buttonCut.Click += new System.EventHandler(this.buttonCut_Click);
            // 
            // buttonZip
            // 
            this.buttonZip.Location = new System.Drawing.Point(168, 3);
            this.buttonZip.Name = "buttonZip";
            this.buttonZip.Size = new System.Drawing.Size(49, 23);
            this.buttonZip.TabIndex = 3;
            this.buttonZip.Text = "Zip";
            this.buttonZip.UseVisualStyleBackColor = true;
            this.buttonZip.Click += new System.EventHandler(this.buttonZip_Click);
            // 
            // buttonUnZip
            // 
            this.buttonUnZip.Location = new System.Drawing.Point(223, 3);
            this.buttonUnZip.Name = "buttonUnZip";
            this.buttonUnZip.Size = new System.Drawing.Size(49, 23);
            this.buttonUnZip.TabIndex = 4;
            this.buttonUnZip.Text = "UnZip";
            this.buttonUnZip.UseVisualStyleBackColor = true;
            this.buttonUnZip.Click += new System.EventHandler(this.buttonUnZip_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(278, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(49, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSecret
            // 
            this.buttonSecret.Location = new System.Drawing.Point(333, 3);
            this.buttonSecret.Name = "buttonSecret";
            this.buttonSecret.Size = new System.Drawing.Size(49, 23);
            this.buttonSecret.TabIndex = 6;
            this.buttonSecret.Text = "FIND";
            this.buttonSecret.UseVisualStyleBackColor = true;
            this.buttonSecret.Click += new System.EventHandler(this.buttonSecret_Click);
            // 
            // buttonSync
            // 
            this.buttonSync.Location = new System.Drawing.Point(388, 3);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(49, 23);
            this.buttonSync.TabIndex = 7;
            this.buttonSync.Text = "Sync";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(443, 3);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(49, 23);
            this.buttonDownload.TabIndex = 8;
            this.buttonDownload.Text = "Down";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // textBoxSearchMask
            // 
            this.textBoxSearchMask.Location = new System.Drawing.Point(498, 4);
            this.textBoxSearchMask.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.textBoxSearchMask.Name = "textBoxSearchMask";
            this.textBoxSearchMask.Size = new System.Drawing.Size(138, 20);
            this.textBoxSearchMask.TabIndex = 9;
            this.textBoxSearchMask.TextChanged += new System.EventHandler(this.textBoxSearchMask_TextChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel4.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 301);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(639, 29);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(537, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(546, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatus.Location = new System.Drawing.Point(3, 336);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(3);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 19);
            this.labelStatus.TabIndex = 4;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 583F));
            this.tableLayoutPanel5.Controls.Add(this.buttonTest1, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(651, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(643, 30);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // buttonTest1
            // 
            this.buttonTest1.Location = new System.Drawing.Point(3, 3);
            this.buttonTest1.Name = "buttonTest1";
            this.buttonTest1.Size = new System.Drawing.Size(54, 23);
            this.buttonTest1.TabIndex = 0;
            this.buttonTest1.Text = "button1";
            this.buttonTest1.UseVisualStyleBackColor = true;
            this.buttonTest1.Click += new System.EventHandler(this.buttonTest1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asdToolStripMenuItem,
            this.fileInfoToolStripMenuItem,
            this.typeToolStripMenuItem,
            this.typeDownloadToolStripMenuItem,
            this.cryptoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1321, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // asdToolStripMenuItem
            // 
            this.asdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.asdToolStripMenuItem.Name = "asdToolStripMenuItem";
            this.asdToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.asdToolStripMenuItem.Text = "Menu";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.settingsToolStripMenuItem.Text = "Settings..";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // fileInfoToolStripMenuItem
            // 
            this.fileInfoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mD5ToolStripMenuItem,
            this.encodingToolStripMenuItem,
            this.permissionsToolStripMenuItem,
            this.txtStatsToolStripMenuItem});
            this.fileInfoToolStripMenuItem.Name = "fileInfoToolStripMenuItem";
            this.fileInfoToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.fileInfoToolStripMenuItem.Text = "FileInfo";
            // 
            // mD5ToolStripMenuItem
            // 
            this.mD5ToolStripMenuItem.Name = "mD5ToolStripMenuItem";
            this.mD5ToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.mD5ToolStripMenuItem.Text = "MD5";
            this.mD5ToolStripMenuItem.Click += new System.EventHandler(this.mD5ToolStripMenuItem_Click);
            // 
            // encodingToolStripMenuItem
            // 
            this.encodingToolStripMenuItem.Name = "encodingToolStripMenuItem";
            this.encodingToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.encodingToolStripMenuItem.Text = "Encoding";
            this.encodingToolStripMenuItem.Click += new System.EventHandler(this.encodingToolStripMenuItem_Click);
            // 
            // permissionsToolStripMenuItem
            // 
            this.permissionsToolStripMenuItem.Name = "permissionsToolStripMenuItem";
            this.permissionsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.permissionsToolStripMenuItem.Text = "Permissions";
            this.permissionsToolStripMenuItem.Click += new System.EventHandler(this.permissionsToolStripMenuItem_Click);
            // 
            // txtStatsToolStripMenuItem
            // 
            this.txtStatsToolStripMenuItem.Name = "txtStatsToolStripMenuItem";
            this.txtStatsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.txtStatsToolStripMenuItem.Text = ".txt stats";
            this.txtStatsToolStripMenuItem.Click += new System.EventHandler(this.txtStatsToolStripMenuItem_Click);
            // 
            // typeToolStripMenuItem
            // 
            this.typeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aSyncDelegateToolStripMenuItem,
            this.multiThreadingToolStripMenuItem,
            this.parallelForEachToolStripMenuItem,
            this.tasksToolStripMenuItem,
            this.asyncawaitToolStripMenuItem});
            this.typeToolStripMenuItem.Name = "typeToolStripMenuItem";
            this.typeToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.typeToolStripMenuItem.Text = "TypeAsync";
            // 
            // aSyncDelegateToolStripMenuItem
            // 
            this.aSyncDelegateToolStripMenuItem.Checked = true;
            this.aSyncDelegateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.aSyncDelegateToolStripMenuItem.Name = "aSyncDelegateToolStripMenuItem";
            this.aSyncDelegateToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.aSyncDelegateToolStripMenuItem.Tag = "";
            this.aSyncDelegateToolStripMenuItem.Text = "ASyncDelegate";
            this.aSyncDelegateToolStripMenuItem.Click += new System.EventHandler(this.aSyncDelegateToolStripMenuItem_Click);
            // 
            // multiThreadingToolStripMenuItem
            // 
            this.multiThreadingToolStripMenuItem.Name = "multiThreadingToolStripMenuItem";
            this.multiThreadingToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.multiThreadingToolStripMenuItem.Tag = "";
            this.multiThreadingToolStripMenuItem.Text = "Multi Threading";
            this.multiThreadingToolStripMenuItem.Click += new System.EventHandler(this.multiThreadingToolStripMenuItem_Click);
            // 
            // parallelForEachToolStripMenuItem
            // 
            this.parallelForEachToolStripMenuItem.Name = "parallelForEachToolStripMenuItem";
            this.parallelForEachToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.parallelForEachToolStripMenuItem.Tag = "";
            this.parallelForEachToolStripMenuItem.Text = "Parallel.ForEach";
            this.parallelForEachToolStripMenuItem.Click += new System.EventHandler(this.parallelForEachToolStripMenuItem_Click);
            // 
            // tasksToolStripMenuItem
            // 
            this.tasksToolStripMenuItem.Name = "tasksToolStripMenuItem";
            this.tasksToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.tasksToolStripMenuItem.Tag = "";
            this.tasksToolStripMenuItem.Text = "Tasks";
            this.tasksToolStripMenuItem.Click += new System.EventHandler(this.tasksToolStripMenuItem_Click);
            // 
            // asyncawaitToolStripMenuItem
            // 
            this.asyncawaitToolStripMenuItem.Name = "asyncawaitToolStripMenuItem";
            this.asyncawaitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.asyncawaitToolStripMenuItem.Text = "async/await";
            this.asyncawaitToolStripMenuItem.Click += new System.EventHandler(this.asyncawaitToolStripMenuItem_Click);
            // 
            // typeDownloadToolStripMenuItem
            // 
            this.typeDownloadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.webClientToolStripMenuItem,
            this.asyncDownloaderToolStripMenuItem});
            this.typeDownloadToolStripMenuItem.Name = "typeDownloadToolStripMenuItem";
            this.typeDownloadToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.typeDownloadToolStripMenuItem.Text = "TypeDownload";
            // 
            // webClientToolStripMenuItem
            // 
            this.webClientToolStripMenuItem.Checked = true;
            this.webClientToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.webClientToolStripMenuItem.Name = "webClientToolStripMenuItem";
            this.webClientToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.webClientToolStripMenuItem.Tag = "";
            this.webClientToolStripMenuItem.Text = "web client";
            this.webClientToolStripMenuItem.Click += new System.EventHandler(this.webClientToolStripMenuItem_Click);
            // 
            // asyncDownloaderToolStripMenuItem
            // 
            this.asyncDownloaderToolStripMenuItem.Name = "asyncDownloaderToolStripMenuItem";
            this.asyncDownloaderToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.asyncDownloaderToolStripMenuItem.Tag = "";
            this.asyncDownloaderToolStripMenuItem.Text = "async downloader";
            this.asyncDownloaderToolStripMenuItem.Click += new System.EventHandler(this.asyncDownloaderToolStripMenuItem_Click);
            // 
            // cryptoToolStripMenuItem
            // 
            this.cryptoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cryptToolStripMenuItem,
            this.decryptToolStripMenuItem});
            this.cryptoToolStripMenuItem.Name = "cryptoToolStripMenuItem";
            this.cryptoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.cryptoToolStripMenuItem.Text = "Crypto..";
            // 
            // cryptToolStripMenuItem
            // 
            this.cryptToolStripMenuItem.Name = "cryptToolStripMenuItem";
            this.cryptToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.cryptToolStripMenuItem.Text = "Crypt";
            this.cryptToolStripMenuItem.Click += new System.EventHandler(this.cryptToolStripMenuItem_Click);
            // 
            // decryptToolStripMenuItem
            // 
            this.decryptToolStripMenuItem.Name = "decryptToolStripMenuItem";
            this.decryptToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.decryptToolStripMenuItem.Text = "Decrypt";
            this.decryptToolStripMenuItem.Click += new System.EventHandler(this.decryptToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 397);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "My File Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView explorer;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ComboBox drivesComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxCurrentDir;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.Button buttonCut;
        private System.Windows.Forms.Button buttonZip;
        private System.Windows.Forms.Button buttonUnZip;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem asdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button buttonSecret;
        private System.Windows.Forms.ToolStripMenuItem fileInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mD5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem permissionsToolStripMenuItem;
        private System.Windows.Forms.Button buttonSync;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aSyncDelegateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiThreadingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parallelForEachToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tasksToolStripMenuItem;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.ToolStripMenuItem typeDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asyncDownloaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asyncawaitToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxSearchMask;
        private System.Windows.Forms.ToolStripMenuItem txtStatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cryptoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button buttonTest1;
    }
}

