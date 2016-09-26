using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MyFileManager
{
    public delegate void SearchNewElementHandler(Entry element);
    public class FindResultsViewer
    {
        public List<MyFolder> DirList
        {
            get
            {
                return dirList;
            }
        }
        public List<MyFile> FileList
        {
            get
            {
                return fileList;
            }
        }
        private List<MyFolder> dirList;
        private List<MyFile> fileList;
        private string mask;
        private Regex regex;
        private MyFolder searchFolder;
        private MultiQueues MQ;
        private SearchNewElementHandler NewElementHandler;
        private Action MaskChangeHandler;
        private Action SearchingCompletedHandler;
        private System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
        private int CountOfCompletedTasks;
        private int CountOfTasks;
        private object CompeledTasksIncSyncRoot = new object();
        public FindResultsViewer(MyFolder searchFolder, string mask, SearchNewElementHandler NewElementHandler, Action MaskChangeHandler, Action SearchingCompletedHandler)
        {
            this.searchFolder = searchFolder;
            this.NewElementHandler += NewElementHandler;
            this.MaskChangeHandler += MaskChangeHandler;
            this.SearchingCompletedHandler += SearchingCompletedHandler;
            ChangeMask(mask);
        }
        public FindResultsViewer(MyFolder searchFolder, SearchNewElementHandler NewElementHandler, Action MaskChangeHandler, Action SearchingCompletedHandler)
        {
            this.searchFolder = searchFolder;
            this.NewElementHandler += NewElementHandler;
            this.MaskChangeHandler += MaskChangeHandler;
            this.SearchingCompletedHandler += SearchingCompletedHandler;
        }
        public void ChangeMask(string mask)
        {
            StopFinding();
            dirList = new List<MyFolder>();
            fileList = new List<MyFile>();
            MaskChangeHandler();
            this.mask = mask;
            ConstructRegex();
            FillQueues();
            cts = new System.Threading.CancellationTokenSource();
            StartFinding(cts.Token);
        }
        private void ConstructRegex()
        {
            string regexEscape = Regex.Escape(mask);
            string regexmask = "^" + regexEscape + "$";
            StringBuilder regexPattern = new StringBuilder(regexmask);
            regexPattern.Replace(@"\*", @"([a-zA-Z0-9АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя_\.\(\)\[\],])*");
            regexPattern.Replace(@"\?", @"([a-zA-Z0-9АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя_\.\(\)\[\],]){1}");
            regex = new Regex(regexPattern.ToString());
        }
        public void FillQueues()
        {
            MQ = new MultiQueues(searchFolder, true, true);
        }
        private void StartFinding(System.Threading.CancellationToken ct)
        {
            int count = MQ.Count;
            CountOfTasks = count;
            CountOfCompletedTasks = 0;
            for (int i = 0; i< count; i++)
            {
                int currentTaskNumber = i;
                Task.Run(() =>
                {
                    try
                    {
                        var result = MQ[currentTaskNumber].Where((item) => (Predicate(item)));
                        foreach (var entry in result)
                        {
                            ct.ThrowIfCancellationRequested();
                            if (entry.Type == EntryType.File)
                            {
                                fileList.Add((MyFile)entry);
                            } else if (entry.Type == EntryType.Folder)
                            {
                                dirList.Add((MyFolder)entry);
                            }
                            NewElementHandler(entry);
                        }
                    }
                    catch (OperationCanceledException)
                    {

                    }
                    catch (Exception exc)
                    {
                        System.Windows.Forms.MessageBox.Show(exc.Message);
                    }
                    finally
                    {
                        lock (CompeledTasksIncSyncRoot)
                        {
                            CountOfCompletedTasks++;
                            if (CountOfCompletedTasks == CountOfTasks)
                            {
                                SearchingCompletedHandler();
                            }
                        }
                    }
                });
            }
        }
        private bool Predicate(Entry entry)
        {
            return regex.IsMatch(entry.Name);
        }
        private void StopFinding()
        {
            cts.Cancel();
        }
    }
}
