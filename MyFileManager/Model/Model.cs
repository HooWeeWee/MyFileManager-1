using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFileManager
{
    public delegate void ModelChangeEvent(ModelChangeEventArgs eArgs);
    public class ModelChangeEventArgs
    {
        public ModelChangeEventArgs(MyFolder currentDirectory, MyFolder currentDrive, List<MyFolder> driveList,
            List<MyFolder> directoryList, List<MyFile> fileList)
        {
            this.currentDirectory = currentDirectory;
            this.currentDrive = currentDrive;
            this.driveList = driveList;
            this.directoryList = directoryList;
            this.fileList = fileList;
        }
        public MyFolder CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
        }
        public MyFolder CurrentDrive
        {
            get
            {
                return currentDrive;
            }
        }
        public List<MyFolder> DriveList
        {
            get
            {
                return driveList;
            }
        }
        public List<MyFolder> DirectoryList
        {
            get
            {
                return directoryList;
            }
        }
        public List<MyFile> FileList
        {
            get
            {
                return fileList;
            }
        }

        private MyFolder currentDirectory;
        private MyFolder currentDrive;
        private List<MyFolder> driveList;
        private List<MyFolder> directoryList;
        private List<MyFile> fileList;
    }
    class Model
    {
        public Model(IView view, MyFolder folder = null)
        {
            if (folder == null)
                folder = Factory.GetSpecialFolder(Environment.SpecialFolder.MyDocuments);
            if (!folder.Exists)
            {
                throw new MyDirectoryNotFoundException(folder.FullPath);
            }
            directoryViewerSource = new DirectoryViewerSource(folder);
            InitializateView(view);
            myWatcher = new MyWatcher(folder, false);
            myWatcher.OnFileDelete += (sender, e, path) =>
            {
                directoryViewerSource.Refresh();
                ModelChange(GetArgs());
            }; //myWatcherChange;
            myWatcher.OnFileCreate += (sender, e, path) =>
            {
                directoryViewerSource.Refresh();
                ModelChange(GetArgs());
            };
            myWatcher.OnFileModify += (sender, e, path) =>
            {
                directoryViewerSource.Refresh();
                ModelChange(GetArgs());
            };
            myWatcher.EnableRaisingEvents = true;
        }
        private void InitializateView(IView view)
        {
            ModelChange += view.OnModelChange;
            view.AskModelGenerateModelChange += OnAskModelGenerateModelChange;
            view.AskModelChangeDirectory += OnAskModelChangeDirectory;
            view.AskModelChangeDrive += OnAskModelChangeDrive;
            view.AskModelGoUp += OnAskModelGoUp;
            view.AskModelExecuteFile += OnAskModelExecuteFile;
            view.AskModelDeleteEntry += OnAskModelDeleteEntry;
            view.AskModelCopyEntries += OnAskModelCopyEntries;
            view.AskModelMoveEntries += OnAskModelMoveEntries;
            view.AskModelMultiThreadingZipping += OnAskModelMultiThreadingZipping;
            view.AskModelASyncDelegateZipping += OnAskModelASyncDelegateZipping;
            view.AskModelParallelForEachZipping += OnAskModelParallelForEachZipping;
            view.AskModelTaskZipping += OnAskModelTaskZipping;
            view.AskModelAsyncAwaitZipping += OnAskModelAsyncAwaitZipping;
            view.AskModelFinding += OnAskModelFinding;
            view.AskModelUnzipFile += OnAskModelUnzipFile;
            view.AskModelRenameEntry += OnAskModelRenameEntry;
            view.AskModelCalcMD5 += OnAskModelCalcMD5;
            view.AskModelGetEncoding += OnAskModelGetEncoding;
            view.AskModelGetPermissions += OnAskModelGetPermissions;
            view.AskModelGetTXTStats += OnAskModelGetTXTStats;
            view.AskModelCryption += OnAskModelCryption;
        }
        private void ChangeWatcherFolder()
        {
            MyFolder newWatcherFolder = directoryViewerSource.CurrentDirectory;
            myWatcher.ChangeDirectory(newWatcherFolder);
        }
        private void OnAskModelGenerateModelChange()
        {
            ModelChange(GetArgs());
        }
        private void OnAskModelChangeDrive(MyFolder newDrive)
        {
            directoryViewerSource.ChangeDrive(newDrive);
            ModelChange(GetArgs());
            ChangeWatcherFolder();
        }
        private void OnAskModelChangeDirectory(MyFolder newDirectory)
        {
            MyFolder currentFolder = directoryViewerSource.CurrentDirectory;
            try
            {
                directoryViewerSource.ChangeDirectory(newDirectory);
            }
            catch (MyUnauthorizedAccessException)
            {
                directoryViewerSource.ChangeDirectory(currentFolder);
            }
            catch (Exception)
            {
                directoryViewerSource.ChangeDirectory(currentFolder);
            }
            finally
            {
                ModelChange(GetArgs());
                ChangeWatcherFolder();
            }
        }
        private void OnAskModelGoUp()
        {
            directoryViewerSource.GoUp();
            ModelChange(GetArgs());
            ChangeWatcherFolder();
        }
        private void OnAskModelDeleteEntry(Entry entry)
        {
            entry.Delete();
            ModelChange(GetArgs());
        }
        private void OnAskModelExecuteFile(MyFile file)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = file.FullPath;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
        private void OnAskModelCopyEntries(List<Entry> entries)
        {
            FileManager.Copy(entries, directoryViewerSource.CurrentDirectory);
            ModelChange(GetArgs());
        }
        private void OnAskModelMoveEntries(List<Entry> entries)
        {
            FileManager.Move(entries, directoryViewerSource.CurrentDirectory);
            ModelChange(GetArgs());
        }
        private void OnAskModelMultiThreadingZipping(List<Entry> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            MultiThreadingZipping multiThreadingZipping =
                            new MultiThreadingZipping(items, reset, reportProgress, complited);
            multiThreadingZipping.Run();
        }
        private void OnAskModelASyncDelegateZipping(List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            ASyncDelegateZipping asyncDelegateZipping =
                            new ASyncDelegateZipping(items, reset, reportProgress, complited);
            asyncDelegateZipping.Run();
        }
        private void OnAskModelParallelForEachZipping(List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            ParallelForEachZipping parallelForEachZipping =
                            new ParallelForEachZipping(items, reset, reportProgress, complited);
            parallelForEachZipping.Run();
        }
        private void OnAskModelTaskZipping(List<Entry> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            TaskZipping taskZipping =
                        new TaskZipping(items, reset, reportProgress, complited);
            taskZipping.Run();
        }
        private void OnAskModelAsyncAwaitZipping(IProgress<int> progress, System.Threading.CancellationTokenSource cts, Action OnCTSCancel,
            List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            AsyncAwaitZipping asyncAwaitZipping = new AsyncAwaitZipping(progress, cts, OnCTSCancel, items, reset, null, complited);
            asyncAwaitZipping.Run();
        }
        private void OnAskModelFinding(Finding finding, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            FindingRunner find = new FindingRunner(finding, reset, reportProgress, complited);
            find.Run();
        }
        private void OnAskModelUnzipFile(MyFile file)
        {
            FileManager.Decompress(file);
            ModelChange(GetArgs());
        }
        private void OnAskModelRenameEntry(Entry entry, string newName)
        {
            entry.Rename(newName);
            ModelChange(GetArgs());
        }
        private string OnAskModelCalcMD5(Entry entry)
        {
            try
            {
                return entry.FileMD5;
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }
        private Encoding OnAskModelGetEncoding(MyFile file)
        {
            try
            {
                return file.FileEncoding;
            }
            catch (Exception exc)
            {
                return null;
            }
        }
        private string OnAskModelGetPermissions(MyFile file)
        {
            try
            {
                return file.FilePermissions;
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }
        private string OnAskModelGetTXTStats(MyFile file)
        {
            if (Path.GetExtension(file.FullPath) == ".txt")
            {
                try
                {
                    string[] lines = file.FileReadAllLines();
                    int countOfLines = lines.Length;
                    char[] separators = new char[] { ' ', ',', '!', ':', ';', '"', '–', '.' };
                    List<string> words = new List<string>();
                    for (int i = 0; i < countOfLines; i++)
                    {
                        words.AddRange(lines[i].Split(separators));
                    }
                    int countOfWords = words.Count();
                    var topTen = (from word in words
                                  where word.Length > 3
                                  group word by word into grp
                                  orderby grp.Count() descending
                                  select (grp.Key) + ": " + grp.Count() + ".").Take(10).ToArray();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Count of lines:");
                    sb.AppendLine(countOfLines.ToString());
                    sb.AppendLine("Count of words:");
                    sb.AppendLine(countOfWords.ToString());
                    sb.AppendLine("Top 10:");
                    foreach (var str in topTen)
                    {
                        sb.AppendLine(str);
                    }
                    string result = sb.ToString();
                    return result;
                }
                catch (Exception exc)
                {
                    return exc.Message;
                }
            }
            return string.Empty;
        }
        private void OnAskModelCryption(List<Entry> entries, bool crypt, string key)
        {
            FileSystemContainer container = new FileSystemContainer(entries, false, true);
            container.Accept(new DESCryptoVisitor(crypt, key));
            ModelChange(GetArgs());
        }

        private ModelChangeEventArgs GetArgs()
        {
            directoryViewerSource.Refresh();
            return new ModelChangeEventArgs(directoryViewerSource.CurrentDirectory, directoryViewerSource.CurrentDrive,
                directoryViewerSource.DriveList, directoryViewerSource.DirectoryList, directoryViewerSource.FileList);
        }
        DirectoryViewerSource directoryViewerSource;
        MyWatcher myWatcher;

        public event ModelChangeEvent ModelChange = (e) => { };
    }
    class DirectoryViewerSource
    {
        public MyFolder CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
        }
        public MyFolder CurrentDrive
        {
            get
            {
                return currentDrive;
            }
        }
        public List<MyFolder> DriveList
        {
            get
            {
                return driveList;
            }
        }
        public List<MyFolder> DirectoryList
        {
            get
            {
                return directoryList;
            }
        }
        public List<MyFile> FileList
        {
            get
            {
                return fileList;
            }
        }
        private MyFolder currentDirectory;
        private MyFolder currentDrive;
        private List<MyFolder> driveList;
        private List<MyFolder> directoryList;
        private List<MyFile> fileList;

        public DirectoryViewerSource(MyFolder folder = null)
        {
            if (folder == null)
                folder = Factory.GetSpecialFolder(Environment.SpecialFolder.MyDocuments);
            if (!folder.Exists)
            {
                throw new MyDirectoryNotFoundException(folder.FullPath);
            }
            RefreshDrives();
            currentDrive = folder.RootDirectory;
            currentDirectory = folder;
            Refresh();
        }
        public void ChangeDirectory(MyFolder newFolder)
        {
            try
            {
                if (newFolder.Exists)
                {
                    currentDrive = newFolder.RootDirectory;
                    currentDirectory = newFolder;
                    Refresh();
                }
                else
                {
                    throw new MyDirectoryNotFoundException(newFolder.FullPath);
                }
            }
            catch (FileNotFoundException inner)
            {
                throw new MyFileNotFoundException(inner.Message, inner);
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (DriveNotFoundException inner)
            {
                throw new MyDriveNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new UnauthorizedAccessException(inner.Message, inner);
            }
        }
        public void GoUp()
        {
            //MyFolder current = new MyFolder(currentDirectory);
            //ChangeDirectory(current.ParentDirectory == null ? current.RootDirectory.FullPath : current.ParentDirectory.FullPath);
            ChangeDirectory(currentDirectory.ParentDirectory);
        }
        public void ChangeDrive(MyFolder newDrive)
        {
            DriveInfo di = new DriveInfo(newDrive.FullPath);
            if (di.IsReady)
            {
                currentDrive = newDrive;
                ChangeDirectory(newDrive);
            }
            else
            {
                throw new MyDriveNotFoundException();
            }
        }
        public void Refresh()
        {
            RefreshDrives();
            var newDirs = currentDirectory.DirectoryGetFolders;
            directoryList = new List<MyFolder>();
            foreach (var dir in newDirs)
            {
                directoryList.Add(dir);
            }
            var newFiles = currentDirectory.DirectoryGetFiles;
            fileList = new List<MyFile>();
            foreach (var file in newFiles)
            {
                fileList.Add(file);
            }
        }
        public void RefreshDrives()
        {
            var drives = DriveInfo.GetDrives();
            driveList = new List<MyFolder>();
            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    MyFolder driveAsFolder;
                    if (Factory.TryGetFolder(drive.ToString(), out driveAsFolder))
                    {
                        driveList.Add(driveAsFolder);
                    }
                }
            }
        }
    }
}
