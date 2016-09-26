namespace MyFileManager
{
    partial class SearchForm
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
            this.explorer = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFullPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelSearchingStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // explorer
            // 
            this.explorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.explorer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnFullPath,
            this.columnSize});
            this.explorer.FullRowSelect = true;
            this.explorer.GridLines = true;
            this.explorer.LabelEdit = true;
            this.explorer.Location = new System.Drawing.Point(12, 38);
            this.explorer.Name = "explorer";
            this.explorer.Size = new System.Drawing.Size(642, 298);
            this.explorer.TabIndex = 1;
            this.explorer.UseCompatibleStateImageBehavior = false;
            this.explorer.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 241;
            // 
            // columnFullPath
            // 
            this.columnFullPath.Text = "Full Path";
            this.columnFullPath.Width = 329;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 63;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(241, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelSearchingStatus
            // 
            this.labelSearchingStatus.AutoSize = true;
            this.labelSearchingStatus.Location = new System.Drawing.Point(259, 15);
            this.labelSearchingStatus.Name = "labelSearchingStatus";
            this.labelSearchingStatus.Size = new System.Drawing.Size(0, 13);
            this.labelSearchingStatus.TabIndex = 3;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 366);
            this.Controls.Add(this.labelSearchingStatus);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.explorer);
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView explorer;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ColumnHeader columnFullPath;
        private System.Windows.Forms.Label labelSearchingStatus;
    }
}