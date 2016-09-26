using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MyFileManager
{
    static class FileManager
    {
        #region COPY
        public static void Copy(string sourceFilePath, string destDirectoryPath)
        {
            Entry sourceEntry = Factory.GetEntry(sourceFilePath);
            MyFolder destDirectory;
            if (Factory.TryGetFolder(destDirectoryPath, out destDirectory))
            {
                sourceEntry.CopyToDirectory(destDirectory);
            } else
            {
                throw new ArgumentException("Папка назначения отсутсвует или недоступна.");
            }
        }
        public static void Copy(List<string> sourcePaths, string destPath)
        {
            foreach (var path in sourcePaths)
            {
                Copy(path, destPath);
            }
        }
        public static void Copy(Entry sourceEntry, MyFolder destDirectory)
        {
            if (destDirectory.Exists)
            {
                sourceEntry.CopyToDirectory(destDirectory);
            }
            else
            {
                throw new ArgumentException("Папка назначения отсутсвует или недоступна.");
            }
        }
        public static void Copy(List<Entry> sourceEntries, MyFolder destDirectory)
        {
            foreach (var entry in sourceEntries)
            {
                Copy(entry, destDirectory);
            }
        }
        #endregion
        #region DELETE
        public static void Delete(string path)
        {
            Entry entry = Factory.GetEntry(path);
            entry.Delete();
        }
        public static void Delete(List<string> paths)
        {
            foreach (var path in paths)
            {
                Delete(path);
            }
        }
        #endregion
        #region MOVE
        public static void Move(string sourcePath, string destDirectoryPath)
        {
            Entry sourceEntry = Factory.GetEntry(sourcePath);
            MyFolder destDirectory;
            if (Factory.TryGetFolder(destDirectoryPath, out destDirectory))
            {
                sourceEntry.MoveToDirectory(destDirectory);
            }
            else
            {
                throw new ArgumentException("Папка назначения отсутсвует или недоступна.");
            }
        }
        public static void Move(List<string> sourcePaths, string destPath)
        {
            foreach (var path in sourcePaths)
            {
                Move(path, destPath);
            }
        }
        public static void Move(Entry sourceEntry, MyFolder destDirectory)
        {
            if (destDirectory.Exists)
            {
                sourceEntry.MoveToDirectory(destDirectory);
            }
            else
            {
                throw new ArgumentException("Папка назначения отсутсвует или недоступна.");
            }
        }
        public static void Move(List<Entry> sourceEntries, MyFolder destDirectory)
        {
            foreach (var entry in sourceEntries)
            {
                Move(entry, destDirectory);
            }
        }
        #endregion
        #region RENAME
        public static void Rename(string oldPath, string newName)
        {
            Entry entry = Factory.GetEntry(oldPath);
            entry.Rename(newName);
        }
        #endregion
        #region GZIP
        private static void CompressionFile(string sourcePath, string folderToZippedFile, bool compress = true)
        {
            const int bufferSize = 16834;
            byte[] buffer = new byte[bufferSize];
            string destPath = Path.Combine(folderToZippedFile, Path.GetFileName(sourcePath) + ".gz");
            MyFile sourceFile;
            if (Factory.TryGetFile(sourcePath, out sourceFile))
            {
                MyFile outFile = Factory.CreateFile(destPath);
                using (Stream inFileStream = sourceFile.FileOpen(FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream outFileStream = outFile.FileOpen(FileMode.Create, FileAccess.Write, FileShare.Read))
                using (GZipStream gZipStream = new GZipStream(compress ? outFileStream : inFileStream, compress ? CompressionMode.Compress : CompressionMode.Decompress))
                {
                    Stream inStream = compress ? inFileStream : gZipStream;
                    Stream outStream = compress ? gZipStream : outFileStream;
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = inStream.Read(buffer, 0, bufferSize);
                        outStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                }
            }
        }
        private static void CompressionDir(string path)
        {
            MyFolder oldDir;
            if (Factory.TryGetFolder(path, out oldDir))
            {
                string newDirPath = path + "_gziped";
                MyFolder newDir = Factory.CreateFolder(newDirPath);
                var files = oldDir.DirectoryGetFiles;
                foreach (var file in files)
                {
                    CompressionFile(file.FullPath, newDirPath, true);
                }
                var dirs = oldDir.DirectoryGetFolders;
                foreach (var dir in dirs)
                {
                    string innerDirPath = Path.Combine(newDirPath, dir.Name);
                    MyFolder innderDir = Factory.CreateFolder(innerDirPath);
                    CompressionSubDir(dir, innerDirPath);
                }
            }
        }
        private static void CompressionSubDir(MyFolder from, string toPath)
        {
            string newDirPath = toPath + "_gziped";
            MyFolder newDir = Factory.CreateFolder(newDirPath);
            var files = from.DirectoryGetFiles;
            foreach (var file in files)
            {
                CompressionFile(file.FullPath, newDirPath, true);
            }
            var dirs = from.DirectoryGetFolders;
            foreach (var dir in dirs)
            {
                string innerDir = Path.Combine(from.FullPath, dir.Name);
                CompressionSubDir(dir, innerDir);
            }
        }
        public static void Compress(string path)
        {
            Entry entry = Factory.GetEntry(path);
            MyFile file = entry as MyFile;
            if (file != null)
            {
                CompressionFile(path, Path.GetDirectoryName(path), true);
            }
            else
            {
                MyFolder folder = entry as MyFolder;
                if (folder != null)
                {
                    CompressionDir(path);
                }
            }
        }
        public static void Compress(MyFile file)
        {

        }
        public static void Decompress(string path)
        {
            if (Path.GetExtension(path) == ".gz")
            {
                CompressionFile(path, Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)), false);
            }
        }
        public static void Decompress(MyFile file)
        {
            string path = file.FullPath;
            if (Path.GetExtension(path) == ".gz")
            {
                CompressionFile(path, Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)), false);
            }
        }
        #endregion
    }
}
