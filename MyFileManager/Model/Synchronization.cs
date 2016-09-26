using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyFileManager
{
    class Synchronization
    {
        string directoryPath1;
        MyFolder folder1;
        public MyWatcher watcher1;
        string directoryPath2;
        MyFolder folder2;
        public MyWatcher watcher2;
        MyWatcherEvent handlers;
        public Synchronization(string directoryPath1, string directoryPath2)
        {
            folder1 = new MyFolder(directoryPath1);
            if (folder1.Exists)
            {
                this.directoryPath1 = directoryPath1;
            }
            else
            {
                throw new ArgumentException();
            }
            folder2 = new MyFolder(directoryPath2);
            if (folder2.Exists)
            {
                this.directoryPath2 = directoryPath2;
            }
            else
            {
                throw new ArgumentException();
            }
            Initialization();
        }
        public void AddHandler(MyWatcherEvent handler)
        {
            handlers += handler;
        }
        public void RemoveHandler(MyWatcherEvent handler)
        {
            handlers -= handler;
        }
        private void Initialization()
        {
            watcher1 = new MyWatcher(folder1, true);
            watcher2 = new MyWatcher(folder2, true);
            MergeFolders(directoryPath1, directoryPath2);
            watcher1.OnFileCreate += Handler;
            watcher1.OnFileDelete += Handler;
            watcher1.OnFileModify += Handler;
            watcher2.OnFileCreate += Handler;
            watcher2.OnFileDelete += Handler;
            watcher2.OnFileModify += Handler;
            watcher1.EnableRaisingEvents = true;
            watcher2.EnableRaisingEvents = true;
        }
        public void Stop()
        {
            if (watcher1 != null)
            {
                watcher1.EnableRaisingEvents = false;
            }
            if (watcher2 != null)
            {
                watcher2.EnableRaisingEvents = false;
            }
        }
        private void Handler(object sender, MyWatcherChangeType type, MyFile file)
        {
            switch (type)
            {
                case MyWatcherChangeType.Create:
                    {
                        OnCreate(file);
                        break;
                    }
                case MyWatcherChangeType.Delete:
                    {
                        OnDelete(file);
                        break;
                    }
                case MyWatcherChangeType.Modify:
                    {
                        OnModify(file);
                        break;
                    }
            }
            handlers(sender, type, file);            
        }
        private void OnCreate(MyFile file)
        {
            string path = file.FullPath;
            int situation = DefineDirectory(path);
            switch (situation)
            {
                case 1:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath1, directoryPath2);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        MyFile newFile = new MyFile(newFilePath);
                        if (newFile.Exists)
                        {
                            FileManager.Copy(path, newFileDir);
                        }
                        break;
                    }
                case 2:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath2, directoryPath1);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        MyFile newFile = new MyFile(newFilePath);
                        if (newFile.Exists)
                        {
                            FileManager.Copy(path, newFileDir);
                        }
                        break;
                    }
            }
        }
        private void OnDelete(MyFile file)
        {
            string path = file.FullPath;
            int situation = DefineDirectory(path);
            switch (situation)
            {
                case 1:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath1, directoryPath2);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        MyFile newFile = new MyFile(newFilePath);
                        if (newFile.Exists)
                        {
                            FileManager.Delete(newFilePath);
                        }
                        break;
                    }
                case 2:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath2, directoryPath1);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        MyFile newFile = new MyFile(newFilePath);
                        if (newFile.Exists)
                        {
                            FileManager.Delete(newFilePath);
                        }
                        break;
                    }
            }
        }
        private void OnModify(MyFile file)
        {
            string path = file.FullPath;
            int situation = DefineDirectory(path);
            switch (situation)
            {
                case 1:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath1, directoryPath2);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        FileManager.Copy(path, newFileDir);
                        break;
                    }
                case 2:
                    {
                        string newFileDir = SwitchPathToAnotherDir(path, directoryPath2, directoryPath1);
                        string newFilePath = Path.Combine(newFileDir, Path.GetFileName(path));
                        FileManager.Copy(path, newFileDir);
                        break;
                    }
            }
        }
        private int DefineDirectory(string fileFullPath)
        {
            if (fileFullPath.Contains(directoryPath1)) return 1;
            if (fileFullPath.Contains(directoryPath2)) return 2;
            return 0;
        }
        private string SwitchPathToAnotherDir(string fileFullPath, string parentDirectoryPath, string newDirectoryPath)
        {
            string newPart = fileFullPath.Substring(parentDirectoryPath.Length+1); //+1 на символ //
            return Path.GetDirectoryName(Path.Combine(newDirectoryPath, newPart));
        }
        private void MergeFolders(string path1, string path2)
        {
            MyFolder folder1 = new MyFolder(path1);
            if (!folder1.Exists) folder1.DirectoryCreate();
            MyFolder folder2 = new MyFolder(path2);
            if (!folder2.Exists) folder2.DirectoryCreate();
            var files1 = folder1.DirectoryGetFiles;
            var files2 = folder2.DirectoryGetFiles;
            foreach (var file in files1) //copying path1's files to path2
            {
                var fullPath = Path.Combine(path1, file.FullPath);
                FileManager.Copy(fullPath, path2);
            }
            foreach (var file in files2) //copying path2's files to path1
            {
                var fullPath = Path.Combine(path2, file.FullPath);
                FileManager.Copy(fullPath, path1);
            }
            var dirs1 = folder1.DirectoryGetFolders;
            var dirs2 = folder2.DirectoryGetFolders;
            foreach (var dir in dirs2) // creating folders from path2 in path1
            {
                var dirFullPath1 = Path.Combine(path1, dir.Name);
                MyFolder dir1 = new MyFolder(dirFullPath1);
                dir1.DirectoryCreate();
                var dirFullPath2 = Path.Combine(path2, dir.FullPath);
                MergeFolders(dirFullPath1, dirFullPath2);
            }
            foreach (var dir in dirs1) // creating folders from path1 in path2
            {
                var dirFullPath2 = Path.Combine(path1, dir.Name);
                MyFolder dir2 = new MyFolder(dirFullPath2);
                dir2.DirectoryCreate();
                var dirFullPath1 = Path.Combine(path2, dir.Name);
                MergeFolders(dirFullPath2, dirFullPath1);
            }
        }
    }
}
