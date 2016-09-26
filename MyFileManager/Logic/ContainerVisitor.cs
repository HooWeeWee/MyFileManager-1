using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace MyFileManager
{
    public interface IContainer
    {
        int Count { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
        void Purge();
        void Accept(IVisitor visitor);
    }
    public class FileSystemContainer : IContainer
    {
        #region IContainerImplementation
        public int Count
        {
            get
            {
                return entries.Count;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return entries.Count == 0;
            }
        }
        public bool IsFull
        {
            get
            {
                return false;
            }
        }        
        public void Accept(IVisitor visitor)
        {
            foreach (var entry in entries)
            {
                if (visitor.IsDone)
                {
                    return;
                }
                visitor.Visit(entry);                
            }
        }
        public void Purge()
        {
            entries.Clear();
        }
        #endregion
        List<Entry> entries;
        public FileSystemContainer(List<Entry> entries, bool includeFoldersNames = false, bool recursive = false)
        {
            this.entries = entries;
            //FillEntries(entries, includeFoldersNames, recursive);
            RecursiveFillingVisitor filler = new RecursiveFillingVisitor(includeFoldersNames, recursive);
            Accept(filler);
            this.entries = filler.Result;
        }
        public class RecursiveFillingVisitor : FileSystemVisitor
        {
            List<Entry> resultEntries;
            bool includeFoldersNames;
            bool recursive;
            public RecursiveFillingVisitor(bool includeFoldersNames, bool recursive)
            {
                resultEntries = new List<Entry>();
                this.includeFoldersNames = includeFoldersNames;
                this.recursive = recursive;
            }
            public override void Visit(MyFolder folder)
            {
                if (includeFoldersNames) resultEntries.Add(folder);
                var files = folder.DirectoryGetFiles;
                foreach (var file in files)
                {
                    resultEntries.Add(file);
                }
                if (recursive)
                {
                    var subFolders = folder.DirectoryGetFolders;
                    foreach (var subFolder in subFolders)
                    {
                        Visit(subFolder);
                    }
                }
            }

            public override void Visit(MyFile file)
            {
                resultEntries.Add(file);
            }
            public List<Entry> Result
            {
                get
                {
                    return resultEntries;
                }
            }
        }
    }
    public interface IVisitor
    {
        void Visit(object obj);
        bool IsDone { get; }
    }
    public abstract class FileSystemVisitor : IVisitor
    {
        #region IVisitorImplementation
        public bool IsDone
        {
            get
            {
                return false;
            }
        }
        public void Visit(object obj)
        {
            Entry entry = obj as Entry;
            if (entry == null) throw new ArgumentException("Попытка обработать элемент, не являющийся Entry");
            if (entry.Type == EntryType.File)
            {
                MyFile entryAsFile = obj as MyFile;
                Visit(entryAsFile);
            }
            else if (entry.Type == EntryType.Folder)
            {
                MyFolder entryAsFolder = obj as MyFolder;
                Visit(entryAsFolder);
            }
        }
        #endregion
        public abstract void Visit(MyFile file);
        public abstract void Visit(MyFolder folder);
    }
    public class SearchVisitor : FileSystemVisitor
    {
        Func<Entry, bool> predicate;
        List<MyFolder> folders;
        List<MyFile> files;
        public SearchVisitor(Func<Entry, bool> predicate)
        {
            this.predicate = predicate;
            folders = new List<MyFolder>();
            files = new List<MyFile>();
        }
        public override void Visit(MyFolder folder)
        {
            if (predicate(folder)) folders.Add(folder);
        }
        public override void Visit(MyFile file)
        {
            if (predicate(file)) files.Add(file);
        }
    }
    public class DESCryptoVisitor : FileSystemVisitor
    {
        bool crypt;
        DESCryptoServiceProvider desProvider;
        string key;
        byte[] desKey;
        byte[] desIV;
        public DESCryptoVisitor(bool crypt, string key)
        {            
            this.crypt = crypt;
            this.key = key;
            byte[] bytes = KeyToBytes(key);
            this.desKey = bytes;
            this.desIV = bytes;
            desProvider = new DESCryptoServiceProvider();
        }
        private byte[] KeyToBytes(string key)
        {
            if (key == null || key == string.Empty) throw new ArgumentException("Invalid key!");
            if (key.Length < 8)
            {
                while (key.Length != 8)
                {
                    key += " ";
                }
            } else if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            return Encoding.UTF8.GetBytes(key);
        }
        public override void Visit(MyFolder folder)
        {
            throw new NotImplementedException();
        }
        public override void Visit(MyFile file)
        {
            if (crypt)
            {
                using (FileStream fin = file.FileOpen(FileMode.Open, FileAccess.Read, FileShare.None))
                {                    
                    MyFile cryptedFile = Factory.CreateFile(file.FullPath + "_crypted");
                    using (FileStream fout = cryptedFile.FileOpen(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        fout.SetLength(0);
                        //Create variables to help with read and write.
                        byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                        long rdlen = 0;              //This is the total number of bytes written.
                        long totlen = fin.Length;    //This is the total length of the input file.
                        int len;                     //This is the number of bytes to be written at a time.
                        DES des = new DESCryptoServiceProvider();
                        using (CryptoStream encStream = new CryptoStream(fout, desProvider.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write))
                        {
                            //Read from the input file, then encrypt and write to the output file.
                            while (rdlen < totlen)
                            {
                                len = fin.Read(bin, 0, 100);
                                encStream.Write(bin, 0, len);
                                rdlen = rdlen + len;
                            }
                        }
                    }
                }
                file.Delete();
            } else
            {
                bool success = true;
                using (FileStream fin = file.FileOpen(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    MyFile decryptedFile;
                    string pathToCryptedFile = file.FullPath;
                    if (pathToCryptedFile.EndsWith("_crypted"))
                    {
                        decryptedFile = Factory.CreateFile(file.FullPath.Remove(pathToCryptedFile.Length - "_crypted".Length));
                    } else
                    {
                        decryptedFile = Factory.CreateFile(pathToCryptedFile + "_decrypted");
                    }
                    using (FileStream fout = decryptedFile.FileOpen(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        fout.SetLength(0);
                        //Create variables to help with read and write.
                        byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
                        long rdlen = 0;              //This is the total number of bytes written.
                        long totlen = fin.Length;    //This is the total length of the input file.
                        int len;                     //This is the number of bytes to be written at a time.
                        DES des = new DESCryptoServiceProvider();
                        try
                        {
                            using (CryptoStream encStream = new CryptoStream(fout, desProvider.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write))
                            {
                                //Read from the input file, then decrypt and write to the output file.
                                while (rdlen < totlen)
                                {
                                    len = fin.Read(bin, 0, 100);
                                    encStream.Write(bin, 0, len);
                                    rdlen = rdlen + len;
                                }
                            }
                        }
                        catch (CryptographicException exc)
                        {
                            System.Windows.Forms.MessageBox.Show(exc.Message + "Неверный ключ.");
                            success = false;
                        }
                        catch (IOException ioexc)
                        {
                            System.Windows.Forms.MessageBox.Show(ioexc.Message + "Ошибка доступа.");
                        }
                        catch (Exception exc)
                        {
                            System.Windows.Forms.MessageBox.Show(exc.Message + "Неопознанная ошибка.");
                        }
                    }
                    if (!success)
                    {
                        decryptedFile.Delete();
                    }
                }
                if (success)
                {
                    file.Delete();
                }
            }
        }
    }
}
