using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace MyFileManager
{
    public partial class DownloadManagerForm : Form
    {
        List<Download> downloadsList = new List<Download>();
        string downloadFolder;
        public DownloadManagerForm()
        {
            InitializeComponent();
        }
        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(Path.Combine(downloadFolder + textBoxFileName.Text), FileMode.Create);
            ListViewItem item = new ListViewItem(new string[] { textBoxURL.Text, "" });
            listView1.Items.Add(item);
            CancellationTokenSource cts = new CancellationTokenSource();
            Download down = new Download(item, textBoxURL.Text, fs, Progress, cts);
            downloadsList.Add(down);
            await down.DownloadAsync();
        }
        private void Progress(int value, ListViewItem item)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)delegate { item.SubItems[1].Text = value.ToString() + "%"; });
            else
                item.SubItems[1].Text = value.ToString() + "%";
        }
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelDownload(listView1.FocusedItem.Index);
        }
        private void CancelDownload(int index)
        {
            downloadsList[index].cts.Cancel();
            downloadsList[index].LVItem.BackColor = Color.Red;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.SelectedIndices.Count == 1)
                {
                    if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                    {
                        contextMenuStrip1.Show(Cursor.Position);
                    }
                }
            }
        }
    }
}
