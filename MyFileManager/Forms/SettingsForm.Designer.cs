namespace MyFileManager
{
    partial class SettingsForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonChangeFont = new System.Windows.Forms.Button();
            this.labelCurFont = new System.Windows.Forms.Label();
            this.buttonChangeDirColor = new System.Windows.Forms.Button();
            this.labelCurrentDirColor = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.labelBackColor = new System.Windows.Forms.Label();
            this.labelCurrentBackColor = new System.Windows.Forms.Label();
            this.buttonBackColorChange = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonChangeFont, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelCurFont, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonChangeDirColor, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCurrentDirColor, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelBackColor, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCurrentBackColor, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonBackColorChange, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(287, 364);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Dir color:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Font:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonChangeFont
            // 
            this.buttonChangeFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonChangeFont.Location = new System.Drawing.Point(217, 3);
            this.buttonChangeFont.Name = "buttonChangeFont";
            this.buttonChangeFont.Size = new System.Drawing.Size(67, 26);
            this.buttonChangeFont.TabIndex = 1;
            this.buttonChangeFont.Text = "change..";
            this.buttonChangeFont.UseVisualStyleBackColor = true;
            this.buttonChangeFont.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelCurFont
            // 
            this.labelCurFont.AutoSize = true;
            this.labelCurFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurFont.Location = new System.Drawing.Point(103, 0);
            this.labelCurFont.Name = "labelCurFont";
            this.labelCurFont.Size = new System.Drawing.Size(108, 32);
            this.labelCurFont.TabIndex = 2;
            this.labelCurFont.Text = "Current font";
            this.labelCurFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonChangeDirColor
            // 
            this.buttonChangeDirColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonChangeDirColor.Location = new System.Drawing.Point(217, 35);
            this.buttonChangeDirColor.Name = "buttonChangeDirColor";
            this.buttonChangeDirColor.Size = new System.Drawing.Size(67, 26);
            this.buttonChangeDirColor.TabIndex = 4;
            this.buttonChangeDirColor.Text = "change..";
            this.buttonChangeDirColor.UseVisualStyleBackColor = true;
            this.buttonChangeDirColor.Click += new System.EventHandler(this.buttonChangeDirColor_Click);
            // 
            // labelCurrentDirColor
            // 
            this.labelCurrentDirColor.AutoSize = true;
            this.labelCurrentDirColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentDirColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentDirColor.Location = new System.Drawing.Point(107, 39);
            this.labelCurrentDirColor.Margin = new System.Windows.Forms.Padding(7);
            this.labelCurrentDirColor.Name = "labelCurrentDirColor";
            this.labelCurrentDirColor.Size = new System.Drawing.Size(100, 18);
            this.labelCurrentDirColor.TabIndex = 5;
            // 
            // labelBackColor
            // 
            this.labelBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBackColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelBackColor.Location = new System.Drawing.Point(3, 64);
            this.labelBackColor.Name = "labelBackColor";
            this.labelBackColor.Size = new System.Drawing.Size(94, 32);
            this.labelBackColor.TabIndex = 6;
            this.labelBackColor.Text = "Back color:";
            this.labelBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCurrentBackColor
            // 
            this.labelCurrentBackColor.AutoSize = true;
            this.labelCurrentBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCurrentBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentBackColor.Location = new System.Drawing.Point(107, 71);
            this.labelCurrentBackColor.Margin = new System.Windows.Forms.Padding(7);
            this.labelCurrentBackColor.Name = "labelCurrentBackColor";
            this.labelCurrentBackColor.Size = new System.Drawing.Size(100, 18);
            this.labelCurrentBackColor.TabIndex = 7;
            // 
            // buttonBackColorChange
            // 
            this.buttonBackColorChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBackColorChange.Location = new System.Drawing.Point(217, 67);
            this.buttonBackColorChange.Name = "buttonBackColorChange";
            this.buttonBackColorChange.Size = new System.Drawing.Size(67, 26);
            this.buttonBackColorChange.TabIndex = 8;
            this.buttonBackColorChange.Text = "change..";
            this.buttonBackColorChange.UseVisualStyleBackColor = true;
            this.buttonBackColorChange.Click += new System.EventHandler(this.buttonBackColorChange_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonChangeFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Label labelCurFont;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonChangeDirColor;
        private System.Windows.Forms.Label labelCurrentDirColor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Label labelBackColor;
        private System.Windows.Forms.Label labelCurrentBackColor;
        private System.Windows.Forms.Button buttonBackColorChange;
    }
}