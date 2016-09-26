using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using Ionic.Zip;

namespace MyFileManager
{
    public enum EntryType { File, Folder };
    public static class Factory
    {
        public static Entry GetEntry(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    if (Path.GetExtension(path) == ".zip")
                    {
                        return new MyZipArchive(path);
                    }
                    return new MyFile(path);
                }
                else
                {
                    if (Directory.Exists(path))
                    {
                        MyFolder folder = new MyFolder(path);
                        if (folder.Exists)
                        {
                            return folder;
                        }
                    }
                }
            } catch (UnauthorizedAccessException inner)
            {
                throw new MyUnauthorizedAccessException(inner.Message, inner);
            }
            throw new ArgumentException("Не удалось определить Entry");
        }
        public static bool TryGetFolder(string path, out MyFolder folder)
        {
            try
            {
                Entry entry = GetEntry(path);
                folder = entry as MyFolder;
                if (folder != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                folder = null;
                return false;
            }
        }
        public static bool TryGetFile(string path, out MyFile file)
        {
            try
            {
                Entry entry = GetEntry(path);
                file = entry as MyFile;
                if (file != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                file = null;
                return false;
            }
        }
        public static MyFile CreateFile(string path)
        {
            try
            {
                MyFile file = new MyFile(path);
                file.FileCreate().Close();
                return file;
            }
            catch (FileNotFoundException inner)
            {
                throw new MyFileNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new MyUnauthorizedAccessException(inner.Message, inner);
            }
        }
        public static MyFolder CreateFolder(string path)
        {
            try
            {
                MyFolder folder = new MyFolder(path);
                folder.DirectoryCreate();
                return folder;
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new MyUnauthorizedAccessException(inner.Message, inner);
            }
        }
        public static MyZipArchive CreateZipArchive(string path)
        {
            try
            {
                MyZipArchive zipArch = new MyZipArchive(path);
                zipArch.DirectoryCreate();
                return zipArch;
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new MyUnauthorizedAccessException(inner.Message, inner);
            }
        }
        public static MyFolder GetSpecialFolder(Environment.SpecialFolder specialFolder)
        {
            try
            {
                return new MyFolder(Environment.GetFolderPath(specialFolder));
            }
            catch (DirectoryNotFoundException inner)
            {
                throw new MyDirectoryNotFoundException(inner.Message, inner);
            }
            catch (UnauthorizedAccessException inner)
            {
                throw new MyUnauthorizedAccessException(inner.Message, inner);
            }
        }
    }
    public abstract class Entry
    {
        public Entry(string path)
        {
            this.fullPath = path;
        }
        protected string name;
        protected string fullPath;
        public abstract EntryType Type
        {
            get;
        }
        public abstract bool IsZipped
        {
            get;
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string FullPath
        {
            get
            {
                return fullPath;
            }
        }
        public abstract bool Exists
        {
            get;
        }
        public abstract long Length
        {
            get;
        }
        public abstract void CopyToDirectory(MyFolder directory);
        public abstract void MoveToDirectory(MyFolder directory);
        public abstract DateTime CreationTime
        {
            get;
        }
        public abstract DateTime LastWriteTimeUtc
        {
            get;
        }
        public abstract string FileMD5
        {
            get;
        }
        public abstract Encoding FileEncoding
        {
            get;
        }
        public abstract FileSecurity FileAccessControl
        {
            get;
        }
        public abstract string FilePermissions
        {
            get;
        }
        public abstract FileStream FileCreate();
        public abstract FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
        public abstract StreamReader FileGetStreamReader();
        public abstract string[] FileReadAllLines();
        public abstract void FileAppendAllText(string contents);
        public abstract void Delete();
        public abstract Entry Rename(string newName);
        public abstract List<Entry> DirectoryGetEntries
        {
            get;
        }
        public abstract List<MyFile> DirectoryGetFiles
        {
            get;
        }
        public abstract List<MyFolder> DirectoryGetFolders
        {
            get;
        }
        public abstract MyFolder ParentDirectory
        {
            get;
        }
        public abstract MyFolder RootDirectory
        {
            get;
        }
        public abstract void DirectoryCreate();
        public abstract void ZipFolderAddEntry(Entry entry);
    }
    public class MyFile : Entry
    {
        public MyFile(string path) : base(path)
        {
            name = Path.GetFileName(path);
            fi = new FileInfo(fullPath);
        }
        protected FileInfo fi;
        public override EntryType Type
        {
            get
            {
                return EntryType.File;
            }
        }
        public override bool IsZipped
        {
            get
            {
                return false;
            }
        }
        public override long Length
        {
            get
            {
                return fi.Length;
            }
        }
        public override bool Exists
        {
            get
            {
                return File.Exists(fullPath);
            }
        }
        public override DateTime CreationTime
        {
            get
            {
                return fi.CreationTime;
            }
        }
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                return fi.LastWriteTimeUtc;
            }
        }
        public override string FileMD5
        {
            get
            {
                    return fi.CalcMD5();
            }
        }
        public override Encoding FileEncoding
        {
            get
            {
                return fi.GetEncoding();                
            }
        }
        public override FileSecurity FileAccessControl
        {
            get
            {
                try
                {
                    return fi.GetAccessControl();
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
        }
        public override string FilePermissions
        {
            get
            {
                return fi.GetPermissions();                
            }
        }
        public override List<Entry> DirectoryGetEntries
        {
            get
            {
                return new List<Entry>();
            }
        }
        public override List<MyFile> DirectoryGetFiles
        {
            get
            {
                return new List<MyFile>();
            }
        }
        public override List<MyFolder> DirectoryGetFolders
        {
            get
            {
                return new List<MyFolder>();
            }
        }
        public override MyFolder ParentDirectory
        {
            get
            {
                return new MyFolder(fi.Directory.FullName);
            }
        }
        public override MyFolder RootDirectory
        {
            get
            {
                return new MyFolder(fi.Directory.Parent.FullName);
            }
        }
        public override FileStream FileCreate()
        {
            try
            {
                return File.Create(fullPath);
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
        public override FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            try
            {
                return File.Open(fullPath, fileMode, fileAccess, fileShare);
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
        public override StreamReader FileGetStreamReader()
        {
            try
            {
                return new StreamReader(FullPath);
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
        public override string[] FileReadAllLines()
        {
            try
            {
                return File.ReadAllLines(fullPath, FileEncoding);
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
        public override void FileAppendAllText(string contents)
        {
            try
            {
                File.AppendAllText(fullPath, contents);
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
        public override void Delete()
        {
            try
            {
                File.Delete(fullPath);
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
        public override Entry Rename(string newName)
        {
            try
            {
                File.Move(fullPath, Path.Combine(Path.GetDirectoryName(fullPath), newName));
                return new MyFile(Path.Combine(Path.GetDirectoryName(fullPath), newName));
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
        public override void DirectoryCreate()
        {
            throw new NotImplementedException();
        }
        public override void CopyToDirectory(MyFolder directory)
        {
            try
            {
                if (directory.IsZipped == false)
                {
                    if (!this.Exists)
                    {
                        throw new FileNotFoundException("Копируемый файл не найден по пути: " + this.FullPath);
                    }
                    if (!directory.Exists) directory.DirectoryCreate();
                    string destDirectoryPath = directory.FullPath;
                    string sourceFilePath = this.FullPath;
                    string outFilePath = Path.Combine(destDirectoryPath, Path.GetFileName(sourceFilePath));
                    MyFile outFile = new MyFile(outFilePath);
                    using (Stream inFileStream = this.FileOpen(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    using (Stream outFileStream = outFile.FileCreate())
                    {
                        int bufferSize = 16384;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = inFileStream.Read(buffer, 0, bufferSize);
                            outFileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
                else
                {
                    directory.ZipFolderAddEntry(this);
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
        public override void MoveToDirectory(MyFolder directory)
        {
            try
            {
            if (directory.IsZipped == false)
            {
                if (!this.Exists)
                {
                    throw new FileNotFoundException("Перемещаемый файл не найден по пути: " + this.FullPath);
                }
                if (!directory.Exists) directory.DirectoryCreate();
                string destPath = directory.FullPath;
                string sourcePath = this.FullPath;
                string temppath = Path.Combine(destPath, Path.GetFileName(sourcePath));
                MyFile outFile = new MyFile(temppath);
                using (Stream inFileStream = this.FileOpen(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                using (Stream outFileStream = outFile.FileCreate())
                {
                    int bufferSize = 16384;
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = inFileStream.Read(buffer, 0, bufferSize);
                        outFileStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                }
            } else
            {
                directory.ZipFolderAddEntry(this);
            }
            this.Delete();
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
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            MyFile objAsFile = obj as MyFile;
            if (objAsFile == null) return false;
            return this.FullPath == objAsFile.FullPath;
        }
        public override int GetHashCode()
        {
            return fullPath.GetHashCode();
        }
        public override string ToString()
        {
            return fullPath;
        }
        public override void ZipFolderAddEntry(Entry entry)
        {
            throw new NotImplementedException();
        }
    }
    public class MyFolder : Entry
    {
        public MyFolder(string path) : base(path)
        {
            name = Path.GetFileName(path);
        }
        protected List<Entry> entries;
        public override EntryType Type
        {
            get
            {
                return EntryType.Folder;
            }
        }
        public override bool IsZipped
        {
            get
            {
                return false;
            }
        }
        public override List<Entry> DirectoryGetEntries
        {
            get
            {
                return entries;
            }
        }
        public override List<MyFile> DirectoryGetFiles
        {
            get
            {
                try
                {
                    List<MyFile> files = new List<MyFile>();
                    //foreach (var entry in entries)
                    //{
                    //    if (entry is MyFile) files.Add((MyFile)entry);
                    //}
                    //return files;
                    var filespaths = Directory.GetFiles(fullPath);
                    foreach (var file in filespaths)
                    {
                        MyFile myFile;
                        if (Factory.TryGetFile(file, out myFile))
                        {
                            files.Add(new MyFile(file));
                        }
                    }
                    return files;
                }
                catch (Exception exc)
                {
                    return new List<MyFile>();
                }
            }
        }
        public override List<MyFolder> DirectoryGetFolders
        {
            get
            {
                try
                {
                    List<MyFolder> folders = new List<MyFolder>();
                    var dirspaths = Directory.GetDirectories(fullPath);
                    foreach (var forlder in dirspaths)
                    {
                        folders.Add(new MyFolder(forlder));
                    }
                    var filespaths = Directory.GetFiles(fullPath);
                    foreach (var file in filespaths)
                    {
                        MyFolder myFolder;
                        if (Factory.TryGetFolder(file, out myFolder))
                        {
                            folders.Add(myFolder);
                        }
                    }
                    return folders;
                }
                catch (Exception exc)
                {
                    return new List<MyFolder>();
                }
            }
        }
        public override bool Exists
        {
            get
            {
                return Directory.Exists(fullPath);
            }
        }
        public override MyFolder ParentDirectory
        {
            get
            {
                DirectoryInfo parent = Directory.GetParent(fullPath);
                return parent == null ? RootDirectory : new MyFolder(parent.FullName);
            }
        }
        public override MyFolder RootDirectory
        {
            get
            {
                DirectoryInfo di = new DirectoryInfo(fullPath);
                return new MyFolder(di.Root.FullName);
            }
        }
        public override long Length
        {
            get
            {
                return 0;
            }
        }
        public override DateTime CreationTime
        {
            get
            {
                return Directory.GetCreationTime(fullPath);
            }
        }
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                return Directory.GetLastWriteTimeUtc(fullPath);
            }
        }
        public override string FileMD5
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override Encoding FileEncoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override FileSecurity FileAccessControl
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override string FilePermissions
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override void DirectoryCreate()
        {
            try
            {
                Directory.CreateDirectory(fullPath);
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
        public override void Delete()
        {
            try
            {
                foreach (var file in DirectoryGetFiles)
                {
                    file.Delete();
                }
                var folders = DirectoryGetFolders;
                if (folders.Count == 0)
                {
                    Directory.Delete(fullPath);
                    return;
                }
                foreach (var dir in folders)
                {
                    dir.Delete();
                }
                Directory.Delete(fullPath);
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
        public override Entry Rename(string newName)
        {
            try
            {
                Directory.Move(fullPath, Path.Combine(Path.GetDirectoryName(fullPath), newName));
                return new MyFolder(Path.Combine(Path.GetDirectoryName(fullPath), newName));
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
        public override FileStream FileCreate()
        {
            throw new NotImplementedException();
        }
        public override FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            throw new NotImplementedException();
        }
        public override StreamReader FileGetStreamReader()
        {
            throw new NotImplementedException();
        }
        public override string[] FileReadAllLines()
        {
            throw new NotImplementedException();
        }
        public override void FileAppendAllText(string contents)
        {
            throw new NotImplementedException();
        }
        public override void CopyToDirectory(MyFolder directory)
        {
            try
            {
                if (directory.IsZipped == false)
                {
                    string sourceDirPath = this.FullPath;
                    string destDirPath = directory.FullPath;
                    bool subDirsCopy = true;
                    if (!this.Exists)
                    {
                        throw new DirectoryNotFoundException(
                            "Копируемая папка не найдена по пути: "
                            + sourceDirPath);
                    }
                    MyFolder destDir = new MyFolder(Path.Combine(destDirPath, this.Name));
                    if (!destDir.Exists)
                    {
                        destDir.DirectoryCreate();
                    }
                    var files = this.DirectoryGetFiles;
                    foreach (var file in files)
                    {
                        file.CopyToDirectory(destDir);
                    }
                    if (subDirsCopy)
                    {
                        var subDirs = this.DirectoryGetFolders;
                        foreach (var subDir in subDirs)
                        {
                            subDir.CopyToDirectory(destDir);
                        }
                    }
                }
                else
                {
                    directory.ZipFolderAddEntry(this);
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
        public override void MoveToDirectory(MyFolder directory)
        {
            try
            {
                if (directory.IsZipped == false)
                {
                    string sourceDirPath = this.FullPath;
                    string destDirPath = directory.FullPath;
                    bool subDirsCopy = true;
                    if (!this.Exists)
                    {
                        throw new DirectoryNotFoundException(
                            "Копируемая папка не найдена по пути: "
                            + sourceDirPath);
                    }
                    MyFolder destDir = new MyFolder(Path.Combine(destDirPath, this.Name));
                    if (!destDir.Exists)
                    {
                        destDir.DirectoryCreate();
                    }
                    var files = this.DirectoryGetFiles;
                    foreach (var file in files)
                    {
                        file.MoveToDirectory(destDir);
                    }
                    if (subDirsCopy)
                    {
                        var subDirs = this.DirectoryGetFolders;
                        foreach (var subDir in subDirs)
                        {
                            subDir.MoveToDirectory(destDir);
                        }
                    }
                }
                else
                {
                    directory.ZipFolderAddEntry(this);
                }
                this.Delete();
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
        public override string ToString()
        {
            return FullPath;
        }
        public override void ZipFolderAddEntry(Entry entry)
        {
            throw new NotImplementedException();
        }
    }
    public class MyZipArchive : MyFolder
    {
        string zipArchivePath;
        FileInfo zipFileInfo;
        object zipArchiveSyncRoot = new object();
        public MyZipArchive(string path) : base(path)
        {
            name = Path.GetFileName(path);
            zipArchivePath = path;
            zipFileInfo = new FileInfo(path);
        }
        public override EntryType Type
        {
            get
            {
                return EntryType.Folder;
            }
        }
        public override bool IsZipped
        {
            get
            {
                return true;
            }
        }
        public override List<Entry> DirectoryGetEntries
        {
            get
            {
                return entries;
            }
        }
        public override List<MyFile> DirectoryGetFiles
        {
            get
            {
                try
                {
                    using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                    {
                        List<MyFile> files = new List<MyFile>();
                        foreach (var entry in zipArchive)
                        {
                            if (!entry.IsDirectory)
                            {
                                string entryPath = entry.FileName;
                                if (!entryPath.Contains('/'))
                                {
                                    MyZipEntry zipEntry = new MyZipEntry(entry.FileName, this, zipArchive.Name, entry.FileName);
                                    files.Add(zipEntry);
                                }
                            }
                        }
                        return files;
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
        }
        public override List<MyFolder> DirectoryGetFolders
        {
            get
            {
                try
                {
                    using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                    {
                        List<MyFolder> folders = new List<MyFolder>();
                        HashSet<string> folderLocalPathsSet = new HashSet<string>();
                        //если делать работу с папками, нужно заморочиться тут
                        foreach (var entry in zipArchive)
                        {
                            if (!entry.IsDirectory)
                            {
                                string entryPath = entry.FileName;
                                if ((entryPath.Contains('/')))
                                {
                                    string localPathChildFolder = entryPath.Substring(0, entryPath.IndexOf('/') + 1);
                                    folderLocalPathsSet.Add(localPathChildFolder);
                                }
                            }
                        }
                        foreach (var localPathChildFolder in folderLocalPathsSet)
                        {
                            MyZipFolder zipFolder = new MyZipFolder(null, localPathChildFolder, zipArchive.Name);
                            folders.Add(zipFolder);
                        }
                        return folders;
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
        }
        public override bool Exists
        {
            get
            {
                //return File.Exists(fullPath); //File, т.к. .zip является файлом в System.IO
                return zipFileInfo.Exists;
            }
        }
        public override MyFolder ParentDirectory
        {
            get
            {
                DirectoryInfo parent = zipFileInfo.Directory;
                return parent == null ? null : new MyFolder(parent.FullName); //получаем папку, которая содержит .zip-архив
            }
        }
        public override MyFolder RootDirectory
        {
            get
            {
                DirectoryInfo root = zipFileInfo.Directory;
                return new MyFolder(root.Root.FullName); //получаем корень, который содержит .zip-архив
            }
        }
        public override long Length
        {
            get
            {
                return zipFileInfo.Length;
            }
        }
        public override DateTime CreationTime
        {
            get
            {
                return zipFileInfo.CreationTime;
            }
        }
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                return zipFileInfo.LastWriteTimeUtc;
            }
        }
        public override string FileMD5
        {
            get
            {
                return zipFileInfo.CalcMD5();
            }
        }
        public override Encoding FileEncoding
        {
            get
            {
                return zipFileInfo.GetEncoding();
            }
        }
        public override FileSecurity FileAccessControl
        {
            get
            {
                try
                {
                    return zipFileInfo.GetAccessControl();
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
        }
        public override string FilePermissions
        {
            get
            {
                return zipFileInfo.GetPermissions();
            }
        }
        public override void DirectoryCreate()
        {
            try
            {
                using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                { }
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
        public override void Delete()
        {
            try
            {
                zipFileInfo.Delete();
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
        public override Entry Rename(string newName)
        {
            try
            {
                File.Move(fullPath, Path.Combine(Path.GetDirectoryName(fullPath), newName));
                return new MyFile(Path.Combine(Path.GetDirectoryName(fullPath), newName));
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
        public override FileStream FileCreate()
        {
            try
            {
                return File.Create(fullPath);
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
        public override FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            try
            {
                return File.Open(fullPath, fileMode, fileAccess, fileShare);
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
        public override StreamReader FileGetStreamReader()
        {
            throw new NotImplementedException();
            //return new StreamReader(FullPath);
        }
        public override string[] FileReadAllLines()
        {
            try
            {
                return File.ReadAllLines(fullPath, FileEncoding);
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
        public override void FileAppendAllText(string contents)
        {
            try
            {
                File.AppendAllText(fullPath, contents);
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
        public override void CopyToDirectory(MyFolder directory)
        {
            try
            {
                if (directory.IsZipped == false)
                {
                    if (!this.Exists)
                    {
                        throw new FileNotFoundException("Копируемый .zip-архив не найден по пути: " + this.FullPath);
                    }
                    if (!directory.Exists) directory.DirectoryCreate();
                    string destDirectoryPath = directory.FullPath;
                    string sourceFilePath = this.FullPath;
                    string outFilePath = Path.Combine(destDirectoryPath, Path.GetFileName(sourceFilePath));
                    MyFile outFile = new MyFile(outFilePath);
                    using (Stream inFileStream = this.FileOpen(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    using (Stream outFileStream = outFile.FileCreate())
                    {
                        int bufferSize = 16384;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = inFileStream.Read(buffer, 0, bufferSize);
                            outFileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                } else
                {
                    directory.ZipFolderAddEntry(this);
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
        public override void MoveToDirectory(MyFolder directory)
        {
            try
            {
                if (directory.IsZipped == false)
                {
                    if (!this.Exists)
                    {
                        throw new FileNotFoundException("Перемещаемый .zip-архив не найден по пути: " + this.FullPath);
                    }
                    if (!directory.Exists) directory.DirectoryCreate();
                    string destPath = directory.FullPath;
                    string sourcePath = this.FullPath;
                    string temppath = Path.Combine(destPath, Path.GetFileName(sourcePath));
                    MyFile outFile = new MyFile(temppath);
                    using (Stream inFileStream = this.FileOpen(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    using (Stream outFileStream = outFile.FileCreate())
                    {
                        int bufferSize = 16384;
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = inFileStream.Read(buffer, 0, bufferSize);
                            outFileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
                else
                {
                    directory.ZipFolderAddEntry(this);
                }
                this.Delete();
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
        public override void ZipFolderAddEntry(Entry entry)
        {
            try
            {
                if (entry.Type == EntryType.File && entry.IsZipped == false || entry.Type == EntryType.Folder && entry.FullPath.EndsWith(".zip"))
                {
                    using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                    {
                        zipArchive.AddFile(entry.FullPath, "");
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.File && entry.IsZipped == true)
                {
                    using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                    {
                        string pathToEntryInZip = entry.FullPath;
                        int pos = pathToEntryInZip.LastIndexOf("/") + 1;
                        pathToEntryInZip = pathToEntryInZip.Substring(pos);
                        zipArchive.AddEntry(pathToEntryInZip, entry.FileOpen(FileMode.Open, FileAccess.Read, FileShare.Read));
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.Folder && entry.IsZipped == false)
                {
                    using (ZipFile zipArchive = new ZipFile(zipArchivePath))
                    {
                        List<string> list = GetSimpleFiles((MyFolder)entry);
                        int symbolsToSkip = entry.FullPath.LastIndexOf(@"\") + 1;
                        var processedList = list.Select((s) =>
                        {
                            string localPath = s.Substring(symbolsToSkip);
                            string localPathWithoutFile = localPath.Substring(0, Math.Max(localPath.LastIndexOf(@"\"), localPath.LastIndexOf("/")));
                            return localPathWithoutFile;
                        });
                        foreach (var s in list)
                        {
                            string localPath = s.Substring(symbolsToSkip);
                            string localPathWithoutFile = localPath.Substring(0, Math.Max(localPath.LastIndexOf(@"\"), localPath.LastIndexOf("/")));
                            zipArchive.AddFile(s, localPathWithoutFile);
                        }
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.Folder && entry.IsZipped == true)
                {
                    MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                    MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                    entry.CopyToDirectory(temp);
                    var dirs = temp.DirectoryGetFolders;
                    foreach (var dir in dirs)
                    {
                        if (dir.Name == entry.Name)
                        {
                            ZipFolderAddEntry(dir);
                            temp.Delete();
                            return;
                        }
                    }
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
        private List<string> GetSimpleFiles(MyFolder folder)
        {
            List<string> list = new List<string>();
            GetSimpleFilesRec(folder, list);
            return list;
        }
        private void GetSimpleFilesRec(MyFolder folder, List<string> list)
        {
            var files = folder.DirectoryGetFiles;
            foreach (var file in files)
            {
                if (file.Type == EntryType.File && file.IsZipped == false)
                {
                    list.Add(file.FullPath);
                }
            }
            var dirs = folder.DirectoryGetFolders;
            foreach (var dir in dirs)
            {
                if (dir.Type == EntryType.File && dir.IsZipped == true && dir.FullPath.EndsWith(".zip"))
                {
                    list.Add(dir.FullPath);
                } else
                {
                    GetSimpleFilesRec(dir, list);
                }
            }
        }
    }
    public class MyZipFolder : MyFolder
    {
        public MyZipFolder(string path, string localFolderPath, string parentZipArchivePath) : base(path)
        {
            string folderName = localFolderPath.Substring(0, localFolderPath.Length - 1);
            if (!folderName.Contains('/'))
            {
                name = folderName;
            }
            else
            {
                name = folderName.Substring(folderName.LastIndexOf('/') + 1);
            }
            this.parentZipArchivePath = parentZipArchivePath;
            using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
            {
                this.fullPath = parentZipArchive.Name + @"/" + localFolderPath;
                this.localFolderPath = localFolderPath;
                this.parentZipArchivePath = parentZipArchive.Name;
            }
        }
        string localFolderPath;
        string parentZipArchivePath;
        //object zipArchiveSyncRoot = new object();
        public override EntryType Type
        {
            get
            {
                return EntryType.Folder;
            }
        }
        public override bool IsZipped
        {
            get
            {
                return true;
            }
        }
        public override List<Entry> DirectoryGetEntries
        {
            get
            {
                return entries;
            }
        }
        public override List<MyFile> DirectoryGetFiles
        {
            get
            {
                try
                {
                    using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                    {
                        List<MyFile> files = new List<MyFile>();
                        foreach (var entry in parentZipArchive)
                        {
                            if (!entry.IsDirectory)
                            {
                                string entryPath = entry.FileName;
                                if (entryPath.StartsWith(localFolderPath))
                                {
                                    if (!entryPath.Skip(localFolderPath.Length).Contains('/'))
                                    {
                                        MyZipEntry zipEntry = new MyZipEntry(entry.FileName, this, parentZipArchive.Name, entry.FileName);
                                        files.Add(zipEntry);
                                    }
                                }
                            }
                        }
                        return files;
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
        }
        public override List<MyFolder> DirectoryGetFolders
        {
            get
            {
                try
                {
                    using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                    {
                        List<MyFolder> folders = new List<MyFolder>();
                        HashSet<string> folderLocalPathsSet = new HashSet<string>();
                        //если делать работу с папками, нужно заморочиться тут
                        foreach (var entry in parentZipArchive)
                        {
                            if (!entry.IsDirectory)
                            {
                                string entryPath = entry.FileName;
                                if (entryPath.StartsWith(localFolderPath))
                                {
                                    if ((entryPath.Skip(localFolderPath.Length).Contains('/')))
                                    {
                                        string localPathChildFolder = entryPath.Substring(localFolderPath.Length);
                                        localPathChildFolder = localFolderPath + localPathChildFolder.Substring(0, localPathChildFolder.IndexOf('/') + 1);
                                        folderLocalPathsSet.Add(localPathChildFolder);
                                    }
                                }
                            }
                        }
                        foreach (var localPathChildFolder in folderLocalPathsSet)
                        {
                            MyZipFolder zipFolder = new MyZipFolder(null, localPathChildFolder, parentZipArchive.Name);
                            folders.Add(zipFolder);
                        }
                        return folders;
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
        }
        public override bool Exists
        {
            get
            {
                try
                {
                    using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                    {
                        //return File.Exists(fullPath); //File, т.к. .zip является файлом в System.IO
                        foreach (var entry in parentZipArchive)
                        {
                            string entryPath = entry.FileName;
                            if (entryPath.StartsWith(localFolderPath)) return true;
                        }
                        return false;
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
        }
        public override MyFolder ParentDirectory
        {
            get
            {
                try
                {
                    using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                    {
                        if (localFolderPath.IndexOf('/') == localFolderPath.Length - 1)
                        {
                            return new MyZipArchive(parentZipArchive.Name);
                        }
                        string localPathParentFolder = localFolderPath.Substring(0, localFolderPath.Length - 1);
                        localPathParentFolder = localPathParentFolder.Substring(0, localPathParentFolder.LastIndexOf('/') + 1);
                        MyZipFolder zipFolder = new MyZipFolder(null, localPathParentFolder, parentZipArchive.Name);
                        return zipFolder;
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
        }
        public override MyFolder RootDirectory
        {
            get
            {
                try
                {
                    using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                    {
                        string rootPath = Directory.GetDirectoryRoot(parentZipArchive.Name);
                        return new MyFolder(rootPath);
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
        }
        public override long Length
        {
            get
            {
                return 0;
            }
        }
        public override DateTime CreationTime
        {
            get
            {
                return new DateTime();
            }
        }
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                return new DateTime();
            }
        }
        public override string FileMD5
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override Encoding FileEncoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override FileSecurity FileAccessControl
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override string FilePermissions
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override void DirectoryCreate()
        {
            throw new NotImplementedException();
            //Directory.CreateDirectory(fullPath);
        }
        public override void Delete()
        {
            try
            {
                using (ZipFile parentZipArchive = new ZipFile(parentZipArchivePath))
                {
                    List<ZipEntry> entriesToDelete = new List<ZipEntry>();
                    foreach (var entry in parentZipArchive)
                    {
                        string entryPath = entry.FileName;
                        if (entryPath.StartsWith(localFolderPath)) entriesToDelete.Add(entry);
                    }
                    parentZipArchive.RemoveEntries(entriesToDelete);
                    parentZipArchive.Save();
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
        public override Entry Rename(string newName)
        {
            throw new NotImplementedException();
        }
        public override FileStream FileCreate()
        {
            throw new NotImplementedException();
        }
        public override FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            throw new NotImplementedException();
        }
        public override StreamReader FileGetStreamReader()
        {
            throw new NotImplementedException();
            //return new StreamReader(FullPath);
        }
        public override string[] FileReadAllLines()
        {
            throw new NotImplementedException();
        }
        public override void FileAppendAllText(string contents)
        {
            throw new NotImplementedException();
        }
        public override void CopyToDirectory(MyFolder directory)
        {
            try
            {
                MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                using (ZipFile zipArchive = new ZipFile(parentZipArchivePath))
                {
                    foreach (var entry in zipArchive)
                    {
                        if (entry.FileName.StartsWith(localFolderPath))
                        {
                            entry.Extract(temp.FullPath);
                        }
                    }
                }
                MyFolder folderOnDisk;
                if (Factory.TryGetFolder(Path.Combine(temp.FullPath, localFolderPath), out folderOnDisk))
                {
                    folderOnDisk.CopyToDirectory(directory);
                    temp.Delete();
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
        public override void MoveToDirectory(MyFolder directory)
        {
            try
            {
                MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                using (ZipFile zipArchive = new ZipFile(parentZipArchivePath))
                {
                    foreach (var entry in zipArchive)
                    {
                        if (entry.FileName.StartsWith(localFolderPath))
                        {
                            entry.Extract(temp.FullPath);
                        }
                    }
                }
                MyFolder folderOnDisk;
                if (Factory.TryGetFolder(Path.Combine(temp.FullPath, localFolderPath), out folderOnDisk))
                {
                    folderOnDisk.CopyToDirectory(directory);
                    temp.Delete();
                }
                this.Delete();
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
        public override void ZipFolderAddEntry(Entry entry)
        {
            try
            {
                if (entry.Type == EntryType.File && entry.IsZipped == false || entry.Type == EntryType.Folder && entry.FullPath.EndsWith(".zip"))
                {
                    using (ZipFile zipArchive = new ZipFile(parentZipArchivePath))
                    {
                        zipArchive.AddFile(entry.FullPath, localFolderPath);
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.File && entry.IsZipped == true)
                {
                    using (ZipFile zipArchive = new ZipFile(parentZipArchivePath))
                    {
                        string pathToEntryInZip = entry.FullPath;
                        int pos = pathToEntryInZip.LastIndexOf("/") + 1;
                        pathToEntryInZip = localFolderPath + pathToEntryInZip.Substring(pos);
                        zipArchive.AddEntry(pathToEntryInZip, entry.FileOpen(FileMode.Open, FileAccess.Read, FileShare.Read));
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.Folder && entry.IsZipped == false)
                {
                    using (ZipFile zipArchive = new ZipFile(parentZipArchivePath))
                    {
                        string normalizedPath = entry.FullPath.Replace('/', '\\');
                        normalizedPath = normalizedPath.Remove(normalizedPath.Length - 1);
                        string addition = normalizedPath.Substring(0, normalizedPath.LastIndexOf('\\'));
                        int symbolsToSkip = addition.Length + 1;
                        //addition = addition.Substring(addition.LastIndexOf('\\'));
                        List<string> list = GetSimpleFiles((MyFolder)entry);
                        //int symbolsToSkip = entry.FullPath.LastIndexOf(@"\") + 1;
                        foreach (var s in list)
                        {
                            string localPath = s.Substring(symbolsToSkip);
                            string localPathWithoutFile = localPath.Substring(0, Math.Max(localPath.LastIndexOf(@"\"), localPath.LastIndexOf("/")));
                            localPathWithoutFile = this.localFolderPath + localPathWithoutFile;
                            localPathWithoutFile = localPathWithoutFile.Replace('\\', '/') + '/';
                            zipArchive.AddFile(s, localPathWithoutFile);
                        }
                        zipArchive.Save();
                    }
                    return;
                }
                if (entry.Type == EntryType.Folder && entry.IsZipped == true)
                {
                    MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                    MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                    entry.CopyToDirectory(temp);
                    var dirs = temp.DirectoryGetFolders;
                    foreach (var dir in dirs)
                    {
                        if (dir.Name == entry.Name)
                        {
                            ZipFolderAddEntry(dir);
                            temp.Delete();
                            return;
                        }
                    }
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
        private List<string> GetSimpleFiles(MyFolder folder)
        {
            List<string> list = new List<string>();
            GetSimpleFilesRec(folder, list);
            return list;
        }
        private void GetSimpleFilesRec(MyFolder folder, List<string> list)
        {
            var files = folder.DirectoryGetFiles;
            foreach (var file in files)
            {
                if (file.Type == EntryType.File && file.IsZipped == false)
                {
                    list.Add(file.FullPath);
                }
            }
            var dirs = folder.DirectoryGetFolders;
            foreach (var dir in dirs)
            {
                if (dir.Type == EntryType.File && dir.IsZipped == true && dir.FullPath.EndsWith(".zip"))
                {
                    list.Add(dir.FullPath);
                }
                else
                {
                    GetSimpleFilesRec(dir, list);
                }
            }
        }
    }
    public class MyZipEntry : MyFile
    {
        //ZipEntry zipEntry;
        string zipEntryNameInArchive;
        string parentZipFilePath;
        MyFolder parentZipFolder;
        public MyZipEntry(string path, MyFolder parentZipFolder, string parentZipFilePath, string zipEntryNameInArchive) : base(path) // path - путь к .zip
        {
            this.parentZipFilePath = parentZipFilePath;
            this.zipEntryNameInArchive = zipEntryNameInArchive;
            using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
            {
                ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                string entryName = zipEntry.FileName;
                if (entryName.Contains('/'))
                {
                    name = entryName.Substring(entryName.LastIndexOf('/') + 1);
                }
                else
                {
                    name = entryName;
                }
                this.fullPath = parentZipFile.Name + @"/" + zipEntry.FileName;                
                this.parentZipFolder = parentZipFolder;
                parentZipFilePath = parentZipFile.Name;
            }
        }
        public override EntryType Type
        {
            get
            {
                return EntryType.File;
            }
        }
        public override bool IsZipped
        {
            get
            {
                return true;
            }
        }
        public override long Length
        {
            get
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    return zipEntry.CompressedSize;
                }
            }
        }
        public override bool Exists
        {
            get
            {
                try
                {
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        return parentZipFile[zipEntry.FileName] != null;
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
        }
        public override DateTime CreationTime
        {
            get
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    return zipEntry.CreationTime;
                }
            }
        }
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    return zipEntry.LastModified;
                }
            }
        }
        public override string FileMD5
        {
            get
            {
                try
                {
                    //string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    //zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    //string filePath = Path.Combine(dir, this.Name);
                    //FileInfo fi = new FileInfo(filePath);
                    //return fi.CalcMD5();
                    return String.Empty;
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
        }
        public override Encoding FileEncoding
        {
            get
            {
                try
                {
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                        string filePath = Path.Combine(dir, this.Name);
                        FileInfo fi = new FileInfo(filePath);
                        return fi.GetEncoding();
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
        }
        public override FileSecurity FileAccessControl
        {
            get
            {
                try
                {
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                        string filePath = Path.Combine(dir, this.Name);
                        FileInfo fi = new FileInfo(filePath);
                        return fi.GetAccessControl();
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
        }
        public override string FilePermissions
        {
            get
            {
                try
                {
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                        string filePath = Path.Combine(dir, this.Name);
                        FileInfo fi = new FileInfo(filePath);
                        return fi.GetPermissions();
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
        }
        public override List<Entry> DirectoryGetEntries
        {
            get
            {
                return new List<Entry>();
            }
        }
        public override List<MyFile> DirectoryGetFiles
        {
            get
            {
                return new List<MyFile>();
            }
        }
        public override List<MyFolder> DirectoryGetFolders
        {
            get
            {
                return new List<MyFolder>();
            }
        }
        public override MyFolder ParentDirectory
        {
            get
            {
                return parentZipFolder;
            }
        }
        public override MyFolder RootDirectory
        {
            get
            {
                return parentZipFolder.RootDirectory;
            }
        }
        public override FileStream FileCreate()
        {
            throw new NotImplementedException();
            //return File.Create(fullPath);
        }
        public override FileStream FileOpen(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    string filePath = Path.Combine(dir, zipEntry.FileName);
                    return File.Open(filePath, fileMode, fileAccess, fileShare);
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
        public override StreamReader FileGetStreamReader()
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    string filePath = Path.Combine(dir, zipEntry.FileName);
                    return new StreamReader(filePath);
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
        public override string[] FileReadAllLines()
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    string filePath = Path.Combine(dir, zipEntry.FileName);
                    FileInfo fi = new FileInfo(filePath);
                    Encoding enc = fi.GetEncoding();
                    return File.ReadAllLines(filePath, enc);
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
        public override void FileAppendAllText(string contents)
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    zipEntry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                    string filePath = Path.Combine(dir, zipEntry.FileName);
                    FileInfo fi = new FileInfo(filePath);
                    File.AppendAllText(filePath, contents);
                    parentZipFile.AddFile(filePath);
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
        public override void Delete()
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    parentZipFile.RemoveEntry(zipEntry);
                    parentZipFile.Save();
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
        public override Entry Rename(string newName)
        {
            try
            {
                using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                {
                    ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                    zipEntry.FileName = newName;
                    parentZipFile.Save();
                    return this;
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
        public override void DirectoryCreate()
        {
            throw new NotImplementedException();
        }
        public override void CopyToDirectory(MyFolder directory)
        {
            try
            {
                if (!this.Exists)
                {
                    throw new FileNotFoundException("Копируемый файл не найден по пути: " + this.FullPath);
                }
                if (!directory.Exists)
                {
                    directory.DirectoryCreate();
                    string destDirectoryPath = directory.FullPath;
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        zipEntry.Extract(destDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                } else if (directory.IsZipped == false)
                {
                    string destDirectoryPath = directory.FullPath;
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        zipEntry.Extract(destDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                } else if (directory.IsZipped == true)
                {
                    MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                    MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                    this.CopyToDirectory(temp);
                    var files = temp.DirectoryGetFiles;
                    foreach (var file in files)
                    {
                        if (file.Name == this.name)
                        {
                            directory.ZipFolderAddEntry(file);
                            temp.Delete();
                            return;
                        }
                    }
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
        public override void MoveToDirectory(MyFolder directory)
        {
            try
            {
                if (!this.Exists)
                {
                    throw new FileNotFoundException("Перемещаемый файл не найден по пути: " + this.FullPath);
                }
                if (!directory.Exists)
                {
                    directory.DirectoryCreate();
                    string destDirectoryPath = directory.FullPath;
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        zipEntry.Extract(destDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                    this.Delete();
                }
                else if (directory.IsZipped == false)
                {
                    string destDirectoryPath = directory.FullPath;
                    using (ZipFile parentZipFile = new ZipFile(parentZipFilePath))
                    {
                        ZipEntry zipEntry = parentZipFile[zipEntryNameInArchive];
                        zipEntry.Extract(destDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                    this.Delete();
                }
                else if (directory.IsZipped == true)
                {
                    MyFolder appData = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);
                    MyFolder temp = Factory.CreateFolder(Path.Combine(appData.FullPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
                    this.CopyToDirectory(temp);
                    var files = temp.DirectoryGetFiles;
                    foreach (var file in files)
                    {
                        if (file.Name == this.name)
                        {
                            directory.ZipFolderAddEntry(file);
                            temp.Delete();
                            this.Delete();
                            return;
                        }
                    }
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
    }
}
