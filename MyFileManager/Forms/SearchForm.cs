using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MyFileManager
{
    public partial class SearchForm : Form
    {
        public SearchForm(MainForm MyParent, MyFolder folderToSearch, string mask)
        {
            InitializeComponent();
            this.folderToSearch = folderToSearch;
            this.vs = MyParent.vs;
            this.startMask = mask;
        }
        VisualSettings vs;
        MyFolder folderToSearch;
        string startMask;
        FindResultsViewer frviewer;
        private void buttonSearch_Click(object sender, EventArgs e)
        {
        }
        private void NewElement(Entry element)
        {
            this.Invoke((MethodInvoker)delegate
            {
                AddItemToExplorer(element);
            });
        }
        private void MaskChanged()
        {
            this.Invoke((MethodInvoker)delegate
            {
                labelSearchingStatus.Text = "";
                explorer.Items.Clear();
                dirGroup = new ListViewGroup();
                dirGroup.Name = "folder";
                fileGroup = new ListViewGroup();
                fileGroup.Name = "file";
            });
        }
        private void SearchingComleted()
        {
            this.Invoke((MethodInvoker)delegate
            {
                labelSearchingStatus.Text = "Searching completed!";
            });
        }
        private void AddItemToExplorer(Entry entry)
        {
            if (entry.Type == EntryType.File)
            {
                MyFile entryAsFile = entry as MyFile;
                ListViewItem lvitem = new ListViewItem(entryAsFile.Name, fileGroup);
                long size = entryAsFile.Length;
                string sizeText = size.ToString() + " B";
                if (size > 1024)
                {
                    sizeText = size / 1024 + " KB";
                }
                lvitem.SubItems.Add(entryAsFile.FullPath);
                lvitem.SubItems.Add(sizeText);
                explorer.Items.Add(lvitem);
            }
            else if (entry.Type == EntryType.Folder)
            {
                MyFolder entryAsFolder = entry as MyFolder;
                ListViewItem lvitem = new ListViewItem(entryAsFolder.Name, dirGroup);
                lvitem.BackColor = vs.DirColor;
                lvitem.SubItems.Add(entryAsFolder.FullPath);
                explorer.Items.Add(lvitem);
            }
        }
        private ListViewGroup dirGroup;
        private ListViewGroup fileGroup;
        private void ReloadData()
        {
            this.Invoke((MethodInvoker)delegate
            {
                explorer.Items.Clear();
                var dirGroup = new ListViewGroup();
                dirGroup.Name = "folder";
                var dirList = frviewer.DirList;
                foreach (var dir in dirList)
                {
                    ListViewItem item = new ListViewItem(dir.Name, dirGroup);
                    item.Tag = dir;
                    item.BackColor = vs.DirColor;
                    item.SubItems.Add(dir.FullPath);
                    explorer.Items.Add(item);
                }
                var fileList = frviewer.FileList;
                var fileGroup = new ListViewGroup();
                fileGroup.Name = "file";
                foreach (var file in fileList)
                {
                    ListViewItem item = new ListViewItem(file.Name, fileGroup);
                    item.Tag = file;
                    long size = file.Length;
                    string sizeText = size.ToString() + " B";
                    if (size > 1024)
                    {
                        sizeText = size / 1024 + " KB";
                    }
                    item.SubItems.Add(file.FullPath);
                    item.SubItems.Add(sizeText);
                    explorer.Items.Add(item);
                }
            });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            frviewer.ChangeMask(textBox1.Text);
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            frviewer = new FindResultsViewer(folderToSearch, NewElement, MaskChanged, SearchingComleted);
            textBox1.Text = startMask;
        }
    }
}
