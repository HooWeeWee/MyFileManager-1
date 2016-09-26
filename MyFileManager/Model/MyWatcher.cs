using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFileManager
{
    public delegate void MyWatcherEvent(object sender, MyWatcherChangeType type, MyFile file);
    public enum MyWatcherChangeType { Create, Delete, Modify };
    struct MyFileInfo
    {
        MyFile file;
        public string FullPath
        {
            get
            {
                return file.FullPath;
            }
        }
        DateTime createTime;
        public DateTime CreateTime
        {
            get
            {
                return createTime;
            }
        }
        DateTime lastModifyTimeUtc;
        public DateTime LastModifyTimeUtc
        {
            get
            {
                return lastModifyTimeUtc;
            }
        }
        public string MD5
        {
            get
            {
                return file.FileMD5;
            }
        }
        long size;
        public long Size
        {
            get
            {
                return size;
            }
        }
        public MyFileInfo(MyFile file)
        {
            this.file = file;            
            this.createTime = file.CreationTime;
            this.lastModifyTimeUtc = file.LastWriteTimeUtc;
            this.size = file.Length;
        }
        public bool isModifedTo(MyFileInfo mfi)
        {
            if (mfi.size != this.size)
                return true;
            if (mfi.lastModifyTimeUtc > this.lastModifyTimeUtc)
            {
                if (mfi.MD5 != this.MD5)
                    return true;
            }
            return false;
        }
    }
    class MyWatcher
    {
        Task task;
        CancellationTokenSource cts;
        Dictionary<MyFile, MyFileInfo> currentData;
        MyFolder directory;
        bool deep;
        bool needToChangeDirectory = false;
        bool isWorking = false;
        bool isDirectorySet = false;
        public MyWatcherEvent OnFileCreate;
        public MyWatcherEvent OnFileDelete;
        public MyWatcherEvent OnFileModify;
        public bool EnableRaisingEvents
        {
            get
            {
                return isWorking;
            }
            set
            {
                if (value == isWorking) return;
                if (isWorking)
                {
                    this.Stop();
                } else
                {
                    if (isDirectorySet)
                    {
                        Initialization();
                    } else
                    {
                        isWorking = false;
                    }
                }
            }
        }
        public MyWatcher(bool deep = true)
        {
            this.deep = deep;
            isWorking = false;
            isDirectorySet = false;
        }
        public MyWatcher(MyFolder folder, bool deep = true)
        {
            if (folder.Exists)
            {
                directory = folder;
            }
            else
            {
                throw new ArgumentException("Directory " + folder.FullPath + " does not exist!");
            }
            this.deep = deep;
            isWorking = false;
            isDirectorySet = true;
            //Initialization();
        }
        public void ChangeDirectory(MyFolder newFolder)
        {
            if (!isDirectorySet)
            {
                directory = newFolder;
                isDirectorySet = true;
                if (isWorking) Initialization();
            }
            if (newFolder.FullPath != directory.FullPath)
            {
                needToChangeDirectory = true;
                directory = newFolder;
            }
        }
        private MyFolder DirectoryChange()
        {
            MyFolder directorySafe = directory;
            currentData = new Dictionary<MyFile, MyFileInfo>();
            FillData(directory, currentData, deep);
            needToChangeDirectory = false;
            return directorySafe;
        }
        private void OnError()
        {
            Stop();
            isWorking = false;
            directory = null;
            isDirectorySet = false;
            currentData = new Dictionary<MyFile, MyFileInfo>();            
        }
        private void Initialization()
        {
            cts = new CancellationTokenSource();
            currentData = new Dictionary<MyFile, MyFileInfo>();
            FillData(directory, currentData, deep);
            isWorking = true;
            StartWatch();
        }
        public void Stop()
        {
            isWorking = false;
            cts.Cancel();
        }
        private void FillData(MyFolder directory, Dictionary<MyFile, MyFileInfo> dict, bool deep = true)
        {
            try
            {
                FillDictionaryWithDir(directory, dict, deep);            
            }
            catch (MyDirectoryNotFoundException exc)
            {
                OnError();
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                OnError();
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
        }

        private void FillDictionaryWithDir(MyFolder folder, Dictionary<MyFile, MyFileInfo> dict, bool deep = true)
        {
            var files = folder.DirectoryGetFiles;
            foreach (var file in files)
            {
                MyFileInfo mfi = new MyFileInfo(file);
                dict.Add(file, mfi);
            }
            if (deep)
            {
                var dirs = folder.DirectoryGetFolders;
                foreach (var dir in dirs)
                {
                    FillDictionaryWithDir(dir, dict);
                }
            }
        }
        private void StartWatch()
        {
            var token = cts.Token;
            task = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var directorySafe = directory;
                        if (needToChangeDirectory)
                        {
                            directorySafe = DirectoryChange();
                        }
                        token.ThrowIfCancellationRequested();
                        if (isWorking)
                        {
                            Dictionary<MyFile, MyFileInfo> newData = new Dictionary<MyFile, MyFileInfo>();
                            FillData(directorySafe, newData, deep);
                            CompareData(currentData, newData);
                        }
                        Thread.Sleep(200);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                
                }
            });
        }
        private void CompareData(Dictionary<MyFile, MyFileInfo> oldData, Dictionary<MyFile, MyFileInfo> newData)
        {
            try
            {
                foreach (var key in oldData.Keys)
                {
                    MyFileInfo newMfi;
                    if (newData.TryGetValue(key, out newMfi))
                    {
                        MyFileInfo oldMfi = oldData[key]; //возможно, т.к. key из oldData
                                                          //if (oldMfi.LastModifyTimeUtc < newMfi.LastModifyTimeUtc)
                                                          //{
                                                          //    GenerateFileModify(key);
                                                          //}
                        if (oldMfi.isModifedTo(newMfi))
                        {
                            GenerateFileModify(key);
                        }
                        //if (Math.Abs((oldMfi.LastModifyTimeUtc - newMfi.LastModifyTimeUtc).Milliseconds) > 1000)
                        //{
                        //    GenerateFileModify(key);
                        //}
                    }
                    else
                    {
                        GenerateFileDelete(key);
                    }
                }
                foreach (var key in newData.Keys)
                {
                    MyFileInfo oldMfi;
                    if (!oldData.TryGetValue(key, out oldMfi))
                    {
                        GenerateFileCreate(key);
                    }
                }
                currentData = newData;            
            }
            catch (DirectoryNotFoundException exc)
            {
                OnError();
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                OnError();
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
        }
        private void GenerateFileModify(MyFile file)
        {
            OnFileModify(this, MyWatcherChangeType.Modify, file);
        }
        private void GenerateFileDelete(MyFile file)
        {
            OnFileDelete(this, MyWatcherChangeType.Delete, file);
        }
        private void GenerateFileCreate(MyFile file)
        {
            OnFileCreate(this, MyWatcherChangeType.Create, file);
        }
    }
}
