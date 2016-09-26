using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFileManager;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class MyFileManagerUnitTest
    {
        [TestMethod]
        public void FindSecrtesInFile()
        {
            // arrange
            ASyncDelegateFinding finding = new ASyncDelegateFinding(Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData));
            Entry entry = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));
            entry.FileAppendAllText("+79129198344\n+79221231233\nfiletwo@yadnex.ru\n+7 999 216 7669\nblablabla123199+78dasd"); //пишем данные в файл
            Entry targetEntry = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));

            // act
            bool result = true;
            finding.FindInFile(entry, targetEntry);
            var resultArray = targetEntry.FileReadAllLines(); //считываем результат
            StringBuilder sb = new StringBuilder();
            foreach (var resStr in resultArray) //переводим результат из массива строк в одну строку
            {
                sb.AppendLine(resStr);
            }
            string resultAsString = sb.ToString();
            if (!resultAsString.Contains("+79129198344") || !resultAsString.Contains("+79221231233") || !resultAsString.Contains("filetwo@yadnex.ru") ||
                !resultAsString.Contains("+7 999 216 7669") || resultAsString.Contains("blablabla123199+78dasd") || resultAsString.Contains("123199+78"))
            {
                result = false;
            }
            entry.Delete(); targetEntry.Delete();

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateAndDeleteFile()
        {
            // arrange
            Entry entry = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));

            // act
            entry.Delete();

            // assert
            Assert.IsFalse(entry.Exists);
        }

        [TestMethod]
        public void CreateAndDeleteFolder()
        {
            // arrange
            Entry entry = Factory.CreateFolder(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
            
            // act
            entry.Delete();

            // assert
            Assert.IsFalse(entry.Exists);
        }

        [TestMethod]
        public void CreateAndDeleteZipArchive()
        {
            // arrange
            Entry entry = Factory.CreateZipArchive(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())+".zip"));

            // act
            entry.Delete();

            // assert
            Assert.IsFalse(entry.Exists);
        }

        [TestMethod]
        public void AddFileToZipArchiveAndUnzip()
        {
            // arrange
            MyZipArchive zipArchive = Factory.CreateZipArchive(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip"));
            MyFile file = Factory.CreateFile(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));
            MyFolder tempFolder = Factory.GetSpecialFolder(Environment.SpecialFolder.ApplicationData);

            // act
            file.FileAppendAllText("content1");
            file.MoveToDirectory(zipArchive); //перемещаем файл в архив
            if (file.Exists) Assert.Fail("File " + file.FullPath + "is moved, but Exists is true!");
            var files = zipArchive.DirectoryGetFiles;
            if (files.Count != 1) Assert.Fail("Archive contains " + files.Count + " files, expected 1!");
            files[0].CopyToDirectory(tempFolder); //копируем файл из архива на прежнее место            
            if (!file.Exists) Assert.Fail("File " + file.FullPath + "is unzipped, but Exists is false!");
            string fileContent = file.FileReadAllLines()[0];
            // удаляем все файлы
            zipArchive.Delete();
            file.Delete();

            // assert
            Assert.AreEqual("content1", fileContent);
        }

        [TestMethod]
        public void CryptDecryptFileWithTrueKey()
        {
            // arrange
            MyFile file = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));
            file.FileAppendAllText("secretInfo123123");
            List<Entry> entries = new List<Entry>();
            entries.Add(file);
            string key = "key123";

            // act
            FileSystemContainer container = new FileSystemContainer(entries, false, true);
            container.Accept(new DESCryptoVisitor(true, key));
            MyFile cryptFile;
            if (Factory.TryGetFile(file.FullPath+"_crypted", out cryptFile))
            {
                entries.Clear();
                entries.Add(cryptFile);
                container = new FileSystemContainer(entries, false, true);
                container.Accept(new DESCryptoVisitor(false, key));
            } else
            {
                Assert.Fail("File " + file.FullPath + " is crypted, but " + file.FullPath+"_crypted " + "could not be found!");
            }
            string decryptResult = file.FileReadAllLines()[0];
            file.Delete();

            // assert
            Assert.AreEqual("secretInfo123123", decryptResult);
        }

        [TestMethod]
        public void CryptDecryptFileWithWrongKey()
        {
            // arrange
            MyFile file = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));
            file.FileAppendAllText("secretInfo123123");
            List<Entry> entries = new List<Entry>();
            entries.Add(file);
            string key = "key123";

            // act
            FileSystemContainer container = new FileSystemContainer(entries, false, true);
            container.Accept(new DESCryptoVisitor(true, key));
            MyFile cryptFile;
            if (Factory.TryGetFile(file.FullPath + "_crypted", out cryptFile))
            {
                entries.Clear();
                entries.Add(cryptFile);
                container = new FileSystemContainer(entries, false, true);
                container.Accept(new DESCryptoVisitor(false, "key1234"));
            }
            else
            {
                Assert.Fail("File " + file.FullPath + " is crypted, but " + file.FullPath + "_crypted " + "could not be found!");
            }
            if (file.Exists || !cryptFile.Exists)
            {
                Assert.Fail("Crypting goes wrong!");
            }
            cryptFile.Delete();

            // assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FindingByName()
        {
            // arrange
            MyFolder folder = Factory.CreateFolder(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
            MyFile file1 = Factory.CreateFile(folder.FullPath + "\\document.doc");
            MyFile file2 = Factory.CreateFile(folder.FullPath + "\\documentx.docx");
            MyFile file3 = Factory.CreateFile(folder.FullPath + "\\document.html");
            MyFile file4 = Factory.CreateFile(folder.FullPath + "\\document.dot");
            MyFile file5 = Factory.CreateFile(folder.FullPath + "\\page.doc");
            MyFile file6 = Factory.CreateFile(folder.FullPath + "\\image.jpg");
            HashSet<Entry> expectedFiles = new HashSet<Entry>();
            expectedFiles.Add(file1); expectedFiles.Add(file2);
            expectedFiles.Add(file4); expectedFiles.Add(file5);
            string mask = "*.do*"; //ищем .doc и .dot
            bool isDone = false;
            bool isError = false;
            FindResultsViewer find = new FindResultsViewer(folder, mask,
                (item) => 
                {
                    if (!expectedFiles.Contains(item)) isError = true;
                    else expectedFiles.Remove(item);
                },
                () => { }, () => { isDone = true; });
            
            // act
            while (!isDone)
            {

            }
            folder.Delete();

            // assert
            Assert.IsTrue(expectedFiles.Count == 0 && !isError);
        }

        [TestMethod]
        public void TestMD5()
        {
            // arrange
            MyFile file = Factory.CreateFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetRandomFileName()));
            string content = "S);PFE_NWaf9fAk";
            file.FileAppendAllText(content);
            string expectedMD5 = "5cd46939a10ed4faf1d44c99fb2e4be2";

            // act
            string actualMD5 = file.FileMD5;
            file.Delete();

            // assert
            Assert.AreEqual(expectedMD5, actualMD5);
        }

        [TestMethod]
        public void ZipArchiveAddFiles()
        {
            // arrange
            MyZipArchive zipArchive = Factory.CreateZipArchive(Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip"));
            MyFolder folder = Factory.CreateFolder(Path.Combine
    (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
            string content = "S);PFE_NWaf9fAk";
            MyFile file1 = Factory.CreateFile(folder.FullPath + "\\document.doc"); file1.FileAppendAllText(content);
            MyFile file2 = Factory.CreateFile(folder.FullPath + "\\documentx.docx"); file2.FileAppendAllText(content);
            MyFile file3 = Factory.CreateFile(folder.FullPath + "\\document.html"); file3.FileAppendAllText(content);
            MyFile file4 = Factory.CreateFile(folder.FullPath + "\\document.dot"); file4.FileAppendAllText(content);
            MyFile file5 = Factory.CreateFile(folder.FullPath + "\\page.doc"); file5.FileAppendAllText(content);
            MyFile file6 = Factory.CreateFile(folder.FullPath + "\\image.jpg"); file6.FileAppendAllText(content);
            HashSet<string> paths = new HashSet<string>();
            paths.Add(file1.Name); paths.Add(file2.Name); paths.Add(file3.Name);
            paths.Add(file4.Name); paths.Add(file5.Name); paths.Add(file6.Name);

            // act
            folder.CopyToDirectory(zipArchive);
            var dirs = zipArchive.DirectoryGetFolders;
            if (dirs.Count != 1)
            {
                Assert.Fail("There is " + dirs.Count + " folders. Expected 1.");
            }
            var files = dirs[0].DirectoryGetFiles;
            if (files.Count != 6)
            {
                Assert.Fail("There is " + files.Count + " files in " + dirs[0].FullPath + ". Expected 6.");
            }
            foreach (var file in files)
            {
                if (!paths.Contains(file.Name))
                {
                    Assert.Fail(dirs[0].FullPath + "does not contain " + file.Name + ".");
                } else
                {
                    paths.Remove(file.Name);
                }
            }
            // удаляем используемые entry
            folder.Delete();
            zipArchive.Delete();
            // assert
            Assert.IsTrue(paths.Count == 0);
        }
    }
}
