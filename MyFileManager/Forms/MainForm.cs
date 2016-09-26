using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace MyFileManager
{
    public enum ASyncType { ASyncDelegate, MultiThreading, ParallelForEach, Task, AsyncAwait };
    public enum DownloadType { WebClient, FileWebResponse };
    public delegate void LongOperationFind(MyFolder folder, Entry targetEntry);
    public delegate void LongOperationZip(List<string> items);
    public partial class MainForm : Form, IView
    {
        public VisualSettings vs;
        MyFolder currentFolder;
        ASyncType asyncType = ASyncType.ASyncDelegate;
        DownloadType downType = DownloadType.WebClient;
        public MainForm()
        {
            InitializeComponent();
        }
        public event AskModelGenerateModelChangeEvent AskModelGenerateModelChange;
        public event AskModelChangeDriveEvent AskModelChangeDrive;
        public event AskModelChangeDirectoryEvent AskModelChangeDirectory;
        public event AskModelGoUpEvent AskModelGoUp;
        public event Action<Entry> AskModelDeleteEntry;
        public event Action<MyFile> AskModelExecuteFile;
        public event Action<List<Entry>> AskModelCopyEntries;
        public event Action<List<Entry>> AskModelMoveEntries;
        public event Action<MyFile> AskModelUnzipFile;
        public event Action<Entry, string> AskModelRenameEntry;

        public event Func<Entry, string> AskModelCalcMD5;
        public event Func<MyFile, Encoding> AskModelGetEncoding;
        public event Func<MyFile, string> AskModelGetPermissions;
        public event Func<MyFile, string> AskModelGetTXTStats;

        public event AskModelCryptionEvent AskModelCryption;
        public void OnModelChange(ModelChangeEventArgs eArgs)
        {
            string currentDir = eArgs.CurrentDirectory.FullPath;
            currentFolder = eArgs.CurrentDirectory;
            this.Invoke((MethodInvoker)delegate {
                textBoxCurrentDir.Text = currentDir;
                drivesComboBox.DataSource = eArgs.DriveList;
                drivesComboBox.Text = eArgs.CurrentDrive.FullPath;
                explorer.Items.Clear();

                var upGroup = new ListViewGroup();
                upGroup.Name = "up";
                var rootItem = new ListViewItem(@"\", upGroup);
                rootItem.Tag = new object();
                explorer.Items.Add(rootItem);

                var dirGroup = new ListViewGroup();
                dirGroup.Name = "folder";
                var dirList = eArgs.DirectoryList;
                foreach (var dir in dirList)
                {
                    ListViewItem item = new ListViewItem(dir.Name, dirGroup);
                    item.Tag = dir;
                    item.BackColor = vs.DirColor;
                    explorer.Items.Add(item);
                }

                var fileList = eArgs.FileList;
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
                    item.SubItems.Add(sizeText);
                    explorer.Items.Add(item);
                }
            });
        }
        public void ModelGoUp()
        {
            AskModelGoUp();
        }
        public void ModelChangeDrive(MyFolder newDrive)
        {
            AskModelChangeDrive(newDrive);
        }
        public void ModelChangeDirectory(MyFolder newDirectory)
        {
            AskModelChangeDirectory(newDirectory);
        }
        private void ResetStatusBar()
        {
            this.Invoke((MethodInvoker)delegate {
                labelStatus.Text = string.Empty;
                progressBar1.Value = progressBar1.Minimum;
            });
        }
        private void ReportProgress(int percent)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = percent;
            });
        }
        private void Complited(TimeSpan time)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = progressBar1.Maximum;
                labelStatus.Text = "Completed in " + time.ToString(@"m\:ss\.ff") + "seconds.";
            });
        }
        private void ComplitedAsyncAwait(TimeSpan time)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = progressBar1.Maximum;
                labelStatus.Text = "Completed in " + time.ToString(@"m\:ss\.ff") + "seconds.";
                MessageBox.Show("End!");
                buttonCancel.Enabled = false;
            });
        }
        private void buttonUp_Click(object sender, EventArgs e)
        {
            ModelGoUp();
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            string newDir = textBoxCurrentDir.Text;
            MyFolder newFolder = new MyFolder(newDir);
            ModelChangeDirectory(newFolder);
        }
        private void explorer_ItemActivate(object sender, EventArgs e)
        {
            var items = explorer.SelectedItems;
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    if (listItem.Group.Name == "folder")
                    {
                        MyFolder folder = listItem.Tag as MyFolder;
                        ModelChangeDirectory(folder);
                    }
                    else if (listItem.Group.Name == "file")
                    {
                        try
                        {
                            MyFile file = listItem.Tag as MyFile;
                            AskModelExecuteFile(file);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message + " while exectuing a file");
                        }
                    }
                    else if (listItem.Group.Name == "up")
                    {
                        ModelGoUp();
                    }
                }
            }
        }
        List<Entry> tempPaths = new List<Entry>();
        enum ChangeType { None, Copy, Cut };
        ChangeType currentChange;
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            var items = explorer.SelectedItems;
            if (items.Count == 0) return;
            currentChange = ChangeType.Copy;
            tempPaths = new List<Entry>();
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    Entry listItemAsEntry = listItem.Tag as Entry;
                    if (listItemAsEntry != null)
                        tempPaths.Add(listItemAsEntry);
                }
            }
        }
        private void buttonCut_Click(object sender, EventArgs e)
        {
            var items = explorer.SelectedItems;
            if (items.Count == 0) return;
            currentChange = ChangeType.Cut;
            tempPaths = new List<Entry>();
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    Entry listItemAsEntry = listItem.Tag as Entry;
                    if (listItemAsEntry != null)
                        tempPaths.Add(listItemAsEntry);
                }
            }
        }
        private void buttonPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentChange == ChangeType.None || tempPaths.Count == 0)
                {
                    MessageBox.Show("Nothing to paste");
                }
                else if (currentChange == ChangeType.Copy)
                {
                    AskModelCopyEntries(tempPaths);
                }
                else if (currentChange == ChangeType.Cut)
                {
                    AskModelMoveEntries(tempPaths);
                    tempPaths = new List<Entry>();
                    currentChange = ChangeType.None;
                }
            }
            catch (AggregateException exc)//Exception
            {
                MessageBox.Show(exc.Message + " while pasting");
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var items = explorer.SelectedItems;
            if (items.Count == 0) return;
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    try
                    {
                        Entry itemAsEntry = listItem.Tag as Entry;
                        AskModelDeleteEntry(itemAsEntry);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message + " while deleting");
                    }
                }
            }
        }
        
        private List<string> GetSelectedItemsAsStrings()
        {
            var items = explorer.SelectedItems;
            List<string> stringItems = new List<string>();
            if (items.Count == 0) return stringItems;
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    Entry itemAsEntry = listItem.Tag as Entry;
                    stringItems.Add(itemAsEntry.FullPath);
                }
            }
            return stringItems;
        }
        private List<Entry> GetSelectedItemsAsEntries()
        {
            var items = explorer.SelectedItems;
            List<Entry> itemsAsEntries = new List<Entry>();
            if (items.Count == 0) return itemsAsEntries;
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    Entry itemAsEntry = listItem.Tag as Entry;
                    if (itemAsEntry != null)
                    {
                        itemsAsEntries.Add(itemAsEntry);
                    }
                }
            }
            return itemsAsEntries;
        }
        private Entry GetSelectedItemAsEntry()
        {
            var Items = explorer.SelectedItems;
            if (Items.Count != 0)
            {
                Entry entry = Items[0].Tag as Entry;
                if (entry != null) return entry;
            }
            return null;
        }
        private bool TryGetSelectedItemAsMyFile(out MyFile file)
        {
            var Items = explorer.SelectedItems;
            file = null;
            if (Items.Count != 0)
            {
                file = Items[0].Tag as MyFile;
                if (file != null) return true;
            }
            return false;
        }
        public event Action<List<Entry>, Action, Action<int>, Action<TimeSpan>> AskModelMultiThreadingZipping;
        public event Action<List<string>, Action, Action<int>, Action<TimeSpan>> AskModelASyncDelegateZipping;
        public event Action<List<string>, Action, Action<int>, Action<TimeSpan>> AskModelParallelForEachZipping;
        public event Action<List<Entry>, Action, Action<int>, Action<TimeSpan>> AskModelTaskZipping;
        public event AskModelAsyncAwaitZippingEvent AskModelAsyncAwaitZipping;
        private void buttonZip_Click(object sender, EventArgs e)
        {
            var stringItems = GetSelectedItemsAsStrings();
            var entryItems = GetSelectedItemsAsEntries();
            if (stringItems.Count == 0) return;
            switch (asyncType)
            {
                case ASyncType.MultiThreading:
                    {
                        AskModelMultiThreadingZipping(entryItems, ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.ASyncDelegate:
                    {
                        AskModelASyncDelegateZipping(stringItems, ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.ParallelForEach:
                    {
                        AskModelParallelForEachZipping(stringItems, ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.Task:
                    {
                        AskModelTaskZipping(entryItems, ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.AsyncAwait:
                    {
                        cts = new System.Threading.CancellationTokenSource();
                        progress = new Progress<int>(percent =>
                        {
                            progressBar1.Value = percent;
                        });
                        wasShown = false;
                        buttonCancel.Enabled = true;
                        AskModelAsyncAwaitZipping(progress, cts, OnCTSCancel, stringItems, ResetStatusBar, null, ComplitedAsyncAwait);
                        break;
                    }
            }
        }
        System.Threading.CancellationTokenSource cts;
        IProgress<int> progress;
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
        bool wasShown;
        private void OnCTSCancel()
        {
            if (!wasShown)
            {
                MessageBox.Show("Operation cancelled!");
                this.Invoke((MethodInvoker)delegate { buttonCancel.Enabled = false; });
                wasShown = true;
            }
        }
        public event Action<Finding, Action, Action<int>, Action<TimeSpan>> AskModelFinding;
        private void buttonSecret_Click(object sender, EventArgs e)
        {
            switch (asyncType)
            {
                case ASyncType.MultiThreading:
                    {
                        AskModelFinding(new MultiThreadingFinding(currentFolder), ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.ASyncDelegate:
                    {
                        AskModelFinding(new ASyncDelegateFinding(currentFolder), ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.ParallelForEach:
                    {
                        AskModelFinding(new ParallelForEachFinding(currentFolder), ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.Task:
                    {
                        AskModelFinding(new TaskFinding(currentFolder), ResetStatusBar, ReportProgress, Complited);
                        break;
                    }
                case ASyncType.AsyncAwait:
                    {
                        cts = new System.Threading.CancellationTokenSource();
                        progress = new Progress<int>(percent =>
                        {
                            progressBar1.Value = percent;
                        });
                        wasShown = false;
                        buttonCancel.Enabled = true;
                        AskModelFinding(new AsyncAwaitFinding(progress, cts, OnCTSCancel, currentFolder), ResetStatusBar, null, ComplitedAsyncAwait);
                        break;
                    }
            }
        }
        private void buttonUnZip_Click(object sender, EventArgs e)
        {
            var items = explorer.SelectedItems;
            if (items.Count == 0) return;
            foreach (var item in items)
            {
                ListViewItem listItem = item as ListViewItem;
                if (listItem != null)
                {
                    MyFile listItemAsMyFile = listItem.Tag as MyFile;
                    if (listItemAsMyFile != null)
                    {
                        try
                        {
                            AskModelUnzipFile(listItemAsMyFile);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message + " while Unzipping");
                        }
                    }
                }
            }
        }

        private void explorer_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            try
            {
                Entry entry = explorer.Items[e.Item].Tag as Entry;
                if (entry != null)
                {
                    string newName = e.Label;
                    AskModelRenameEntry(entry, newName);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                e.CancelEdit = true;
            }
        }

        private void drivesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newDrivePath = drivesComboBox.SelectedItem.ToString();
            DriveInfo di = new DriveInfo(newDrivePath);
            if (di.IsReady)
            {
                MyFolder newDrive;
                if (Factory.TryGetFolder(newDrivePath, out newDrive))
                {
                    AskModelChangeDrive(newDrive);
                }
            }
            else
            {
                drivesComboBox.Text = currentFolder.RootDirectory.FullPath;
                MessageBox.Show("Drive is not ready");
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this);
            sf.ShowDialog();
        }
        private void SetGUI()
        {
            SetGUIControls();
            vs = VisualSettings.LoadFromFile();
        }
        List<Control> GUIControls;
        private void SetGUIControls()
        {
            GUIControls = new List<Control>();
            GUIControls.AddRange(this.Controls.Cast<Control>());
        }
        public void RedrawGUI()
        {
            foreach (var ctrl in GUIControls)
            {
                ctrl.Font = vs.Font;
                ctrl.BackColor = vs.BackColor;
            }
            AskModelGenerateModelChange();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetGUI();
            RedrawGUI();
            var items = typeToolStripMenuItem.DropDownItems;
            int length = items.Count;
            for (int i=0;i<length;i++)
            {
                items[i].Tag = i;
            }
            items = typeDownloadToolStripMenuItem.DropDownItems;
            length = items.Count;
            for (int i = 0; i < length; i++)
            {
                items[i].Tag = i;
            }
        }
        private void mD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Entry entry = GetSelectedItemAsEntry();
                MessageBox.Show(AskModelCalcMD5(entry));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " while calc MD5");
            }
        }
        private void encodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyFile file;
                if (TryGetSelectedItemAsMyFile(out file))
                {
                    MessageBox.Show(AskModelGetEncoding(file).ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " while getting permissions");
            }
        }
        private void permissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MyFile file;
                if (TryGetSelectedItemAsMyFile(out file))
                {
                    MessageBox.Show(AskModelGetPermissions(file));
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " while getting permissions");
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
        }
        private void buttonSync_Click(object sender, EventArgs e)
        {
            SyncForm sf = new SyncForm(this);
            sf.ShowDialog();
        }
        private void ChangeASyncType(object sender)
        {
            var s = sender as ToolStripMenuItem;
            int selectedId = (int)s.Tag;
            var items = typeToolStripMenuItem.DropDownItems;
            foreach (ToolStripMenuItem item in items)
            {
                item.Checked = false;
            }
            var selectedItem = (ToolStripMenuItem)items[selectedId];
            selectedItem.Checked = true;
            asyncType = (ASyncType)selectedId;
        }
        private void aSyncDelegateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeASyncType(sender);
        }
        private void multiThreadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeASyncType(sender);
        }
        private void parallelForEachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeASyncType(sender);
        }
        private void tasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeASyncType(sender);
        }
        private void asyncawaitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeASyncType(sender);
        }
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            switch (downType)
            {
                case DownloadType.WebClient:
                    {
                        DownloadForm df = new DownloadForm();
                        df.ShowDialog();
                        break;
                    }
                case DownloadType.FileWebResponse:
                    {
                        DownloadManagerForm dmf = new DownloadManagerForm();
                        dmf.ShowDialog();
                        break;                    
                    }
            }
        }
        private void ChangeDownType(object sender)
        {
            var s = sender as ToolStripMenuItem;
            int selectedId = (int)s.Tag;
            var items = typeDownloadToolStripMenuItem.DropDownItems;
            foreach (ToolStripMenuItem item in items)
            {
                item.Checked = false;
            }
            var selectedItem = (ToolStripMenuItem)items[selectedId];
            selectedItem.Checked = true;
            downType = (DownloadType)selectedId;
        }
        private void webClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDownType(sender);
        }
        private void asyncDownloaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDownType(sender);
        }

        private void textBoxSearchMask_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearchMask.Text != string.Empty)
            {
                SearchForm sf = new SearchForm(this, currentFolder, textBoxSearchMask.Text);
                sf.ShowDialog();
            }
        }

        private void txtStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyFile file;
            if (TryGetSelectedItemAsMyFile(out file))
            {
                MessageBox.Show(AskModelGetTXTStats(file));
            }
        }
        private void cryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inputBoxResult = Microsoft.VisualBasic.Interaction.InputBox("Key to encrypt files:");
            if (inputBoxResult != string.Empty)
            {
                AskModelCryption(GetSelectedItemsAsEntries(), true, inputBoxResult);
            }
        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inputBoxResult = Microsoft.VisualBasic.Interaction.InputBox("Decrypt key:");
            if (inputBoxResult != string.Empty)
            {
                AskModelCryption(GetSelectedItemsAsEntries(), false, inputBoxResult);                
            }
        }

        private void buttonTest1_Click(object sender, EventArgs e)
        {
            Entry entry = GetSelectedItemAsEntry();
            List<MyFile> entries = entry.DirectoryGetFiles;
            int k = 0;
        }

        
    }    
}
