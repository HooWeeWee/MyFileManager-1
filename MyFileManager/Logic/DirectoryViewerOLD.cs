using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MyFileManager
{
    public abstract class Item
    {
        protected Entry entry;
        public Item(string p)
        {
            fullPath = p;
            entry = Factory.GetEntry(p);
        }
        public string FullPath
        {
            get
            {
                return fullPath;
            }
        }
        protected string fullPath;
        public abstract string Name { get; }
        public override string ToString()
        {
            return fullPath;
        }
    }
    class DirItem : Item
    {
        public DirItem(string p) : base(p)
        {
        }
        public override string Name
        {
            get
            {
                return entry.Name;
            }
        }
    }
    class FileItem : Item
    {
        public FileItem(string p) : base(p)
        {
        }
        public override string Name
        {
            get
            {
                return entry.Name;
            }
        }
        public long GetSizeBytes
        {
            get
            {

                return entry.Length;
            }
        }        
    }

    class DirectoryViewerOLD
    {
        public string CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
        }
        public string CurrentDrive
        {
            get
            {
                return currentDrive;
            }
        }
        public List<DirItem> DriveList
        {
            get
            {
                return driveList;
            }
        }
        public List<DirItem> DirList
        {
            get
            {
                return dirList;
            }
        }
        public List<FileItem> FileList
        {
            get
            {
                return fileList;
            }
        }
        private string currentDirectory;
        private string currentDrive;
        private List<DirItem> driveList;
        private List<DirItem> dirList;
        private List<FileItem> fileList;

        public DirectoryViewerOLD(string path = null)
        {
            var drives = DriveInfo.GetDrives();
            driveList = new List<DirItem>();
            foreach (var drive in drives)
            {
                try
                {
                    driveList.Add(new DirItem(drive.Name));
                }
                catch (Exception exc)
                {

                }
            }
            currentDrive = Path.GetPathRoot(path);
            currentDirectory = path;
            Refresh();
        }
        public void ChangeDirectory(string newDir)
        {
            MyFolder newFolder = new MyFolder(newDir);
            if (newFolder.Exists)
            {
                currentDrive = Path.GetPathRoot(newDir);
                currentDirectory = newDir;
                Refresh();
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
        public void GoUp()
        {
            MyFolder current = new MyFolder(currentDirectory);
            ChangeDirectory(current.ParentDirectory == null ? current.RootDirectory.FullPath : current.ParentDirectory.FullPath);
        }
        public void ChangeDrive(string newDrive)
        {
            DriveInfo di = new DriveInfo(newDrive);
            if (di.IsReady)
            {
                currentDrive = newDrive;
                ChangeDirectory(newDrive);
            }
            else
            {
                throw new DriveNotFoundException();
            }
        }
        public void Refresh()
        {
            RefreshDrives();
            MyFolder currentFolder = new MyFolder(currentDirectory);
            var newDirs = currentFolder.DirectoryGetFolders;
            dirList = new List<DirItem>();
            foreach (var dir in newDirs)
            {
                dirList.Add(new DirItem(dir.FullPath));
            }
            var newFiles = currentFolder.DirectoryGetFiles;
            fileList = new List<FileItem>();
            foreach (var file in newFiles)
            {
                fileList.Add(new FileItem(file.FullPath));
            }
        }
        public void RefreshDrives()
        {
            var drives = DriveInfo.GetDrives();
            driveList = new List<DirItem>();
            foreach (var drive in drives)
            {
                try
                {
                    driveList.Add(new DirItem(drive.Name));
                }
                catch (Exception exc)
                {

                }
            }
        }        
    }    
}
