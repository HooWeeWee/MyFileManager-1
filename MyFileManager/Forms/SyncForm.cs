using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFileManager
{
    public partial class SyncForm : Form
    {
        Form parent;
        public SyncForm(Form parent)
        {
            InitializeComponent();
            textBoxLog.Visible = true;
            this.parent = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        Synchronization sync;
        private void button3_Click(object sender, EventArgs e)
        {
            sync = new Synchronization(textBox1.Text, textBox2.Text);
            sync.AddHandler(WriteToLog);
            button3.Enabled = false;
        }
        private void WriteToLog(object sender, MyWatcherChangeType type, MyFile file)
        {            
            this.Invoke((MethodInvoker)delegate
            {
                string path = file.FullPath;
                textBoxLog.AppendText(path + " has " + type.ToString());
                textBoxLog.AppendText(Environment.NewLine);
            });
        }
        private void SyncForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sync != null)
            {
                sync.Stop();
            }
        }
    }
}
