using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MyFileManager
{
    public class FindingRunner
    {
        Finding finding;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt");
        Entry targetEntry = Factory.GetEntry(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt"));
        Stopwatch sw;
        Timer timer;
        Action reset;
        Action<int> reportProgress;
        Action<TimeSpan> complited;
        int ProcessedFiles
        {
            get
            {
                return finding.GetProcessedFilesCount();
            }
        }
        int TotalFiles
        {
            get
            {
                return finding.GetTotalFilesCount();
            }
        }
        bool Done
        {
            get
            {
                return finding.Done();
            }
        }
        public FindingRunner(Finding finding, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            this.finding = finding;
            this.reset += reset;
            this.reportProgress += reportProgress;
            this.complited += complited;
        }
        public FindingRunner(Finding finding)
        {
            this.finding = finding;
        }
        public void Run()
        {
            reset();
            finding.Init();
            StartTimer();
            finding.Find();
        }
        private void StartTimer()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += OnTimerTick;
            timer.Start();
            sw = new Stopwatch();
            sw.Start();
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (TotalFiles != 0)
            {
                int intRate = Convert.ToInt32((double)ProcessedFiles / (double)TotalFiles * 100);
                ReportProgress(intRate);
            }
            if (Done)
            {
                timer.Stop();
                timer.Tick -= OnTimerTick;
                Complited(sw.Elapsed);
            }
        }
        private void ReportProgress(int percent)
        {
            reportProgress(percent);
        }
        private void Complited(TimeSpan time)
        {
            complited(time);
        }
        static double timeoutsec = 0.5;
        public static Regex[] RegExpsArray = new Regex[]{
            //new Regex(@"vk.com(\S)*(?= |'|)"),
            new Regex(@"^(ht|f)tp(s?)\:\/\/([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$){0,10}", RegexOptions.None, TimeSpan.FromSeconds(timeoutsec)),
            new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(timeoutsec)),
            new Regex(@"((8|\+7)[\s]?[\-]?[\s]?)([\s]?\d[\s]?){9,11}", RegexOptions.None, TimeSpan.FromSeconds(timeoutsec)),
            new Regex(@"video-[\d]{1,}_[\d]{1,}", RegexOptions.None, TimeSpan.FromSeconds(timeoutsec)),
            new Regex(@"photo[\d]{1,}_[\d]{1,}", RegexOptions.None, TimeSpan.FromSeconds(timeoutsec)),
            new Regex(@"audio_info[\d/-/_]*(?=\"")", RegexOptions.None, TimeSpan.FromSeconds(timeoutsec))
        };
    }

    public interface Finding
    {
        void Init();
        void Find();
        int GetProcessedFilesCount();
        int GetTotalFilesCount();
        bool Done();
    }
    public abstract class SingleQueue
    {
        protected Queue<Entry> singleQueue;
        protected int singleQueueSize;
        protected int processedFiles;
        object syncRoot = new object();
        protected void FindInSingleQueue(Entry targetEntry)//проверка регулярок для всех файлов из папки path, вывод результатов в файл target
        {
            try
            {
                processedFiles = 0;
                foreach (var file in singleQueue)
                {
                    FindInFile(file, targetEntry);
                }
            }
            catch (MyUnauthorizedAccessException)
            {
                return;
            }
        }
        public void FindInFile(Entry entry, Entry targetEntry)//проверка регулярок в файле file, вывод результатов в файл target
        {
            try
            {
                using (StreamReader SR = entry.FileGetStreamReader())
                {
                    string line;                 
                    while ((line = SR.ReadLine()) != null)
                    {
                        int i = -1;
                        foreach (var reg in FindingRunner.RegExpsArray)
                        {
                            i++;
                            var matches = reg.Matches(line);
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                {
                                    lock (syncRoot)
                                    {
                                        targetEntry.FileAppendAllText(i + " " + match.Value + Environment.NewLine);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                System.Threading.Interlocked.Increment(ref processedFiles);
            }
        } 
        protected void FillSingleQueue(Entry entry, bool deep = true) //заполнение очереди
        {
            FillSingleQueueRecursive(entry, true);
            singleQueueSize = singleQueue.Count;
        }
        private void FillSingleQueueRecursive(Entry entry, bool deep = true) //рекурсивное заполнение очереди файлами из папки path
        {
            try
            {
                if (entry.Type == EntryType.File)
                {
                    singleQueue.Enqueue(entry);
                    return;
                } else if (entry.Type == EntryType.Folder)
                {
                    var fileList = entry.DirectoryGetFiles;
                    foreach (var f in fileList)
                    {
                        singleQueue.Enqueue(f);
                    }
                    if (deep)
                    {
                        var dirList = entry.DirectoryGetFolders;
                        Parallel.ForEach(dirList, dir =>
                        {
                            FillSingleQueue(dir);
                        });
                    }
                }                
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }
        }
        protected void ResetSingleQueue() //очистка очереди
        {
            singleQueue = new Queue<Entry>();
            singleQueueSize = 0;
        }
    }
    public class MultiThreadingFinding : Finding
    {
        MultiThreads MT;
        MyFolder folder;
        public MultiThreadingFinding(MyFolder folder, string target = null)
        {
            this.folder = folder;
        }
        public void Init()
        {
            MT = new MultiThreads(folder, true);         
        }
        public void Find()
        {
            MT.StartProcess(FuncType.Finding);
        }
        public int GetProcessedFilesCount()
        {
            return MT.GetProcessedFiles();
        }
        public int GetTotalFilesCount()
        {
            return MT.GetTotalFiles();
        }
        public bool Done()
        {
            return !MT.IsAlive;
        }
    }
    public class ASyncDelegateFinding : SingleQueue, Finding
    {
        bool isDone;
        MyFolder folder;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt");
        Entry targetEntry = Factory.GetEntry(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt"));
        public ASyncDelegateFinding(MyFolder folder, string target = null)
        {
            this.folder = folder;
            if (target != null)
            {
                this.target = target;
            }
        }
        public void Init()
        {
            isDone = false;
            ResetSingleQueue();
            FillSingleQueue(folder, true);
        }
        private void FindHandler(MyFolder folder, Entry targetEntry)
        {
            ResetSingleQueue();
            FillSingleQueue(folder, true);
            FindInSingleQueue(targetEntry);
        }
        public void Find()
        {
            LongOperationFind getSecrets = FindHandler;
            IAsyncResult iAR = getSecrets.BeginInvoke(folder, targetEntry, new AsyncCallback(Callback), new object());
        }
        private void Callback(IAsyncResult obj)
        {
            isDone = true;
        }
        public int GetProcessedFilesCount()
        {
            return processedFiles;
        }
        public int GetTotalFilesCount()
        {
            return singleQueueSize;
        }
        public bool Done()
        {
            return isDone;
        }
    }
    public class ParallelForEachFinding : SingleQueue, Finding
    {
        bool isDone;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt");
        Entry targetEntry = Factory.GetEntry(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt"));
        MyFolder folder;
        public ParallelForEachFinding(MyFolder folder, string target = null)
        {
            this.folder = folder;
            if (target != null)
            {
                this.target = target;
            }
        }
        public void Init()
        {
            isDone = false;
            processedFiles = 0;
            ResetSingleQueue();
            FillSingleQueue(folder, true);
        }
        object syncRoot = new object();
        public void Find()
        {
            Task.Run(() =>
            {
                Parallel.ForEach(singleQueue, file =>
                {
                    FindInFile(file, targetEntry);
                });
                isDone = true;
            });
        }

        public int GetProcessedFilesCount()
        {
            return processedFiles;            
        }

        public int GetTotalFilesCount()
        {
            return singleQueueSize;
        }
        public bool Done()
        {
            return isDone;
        }
    }
    public class TaskFinding : SingleQueue, Finding
    {
        MultiQueues MQ;
        bool[] TasksMQDone;
        MyFolder folder;
        int QueuesCount;
        public TaskFinding(MyFolder folder)
        {
            this.folder = folder;
        }
        public void Init()
        {
            MQ = new MultiQueues(folder, true);
            QueuesCount = MQ.Count;
            TasksMQDone = new bool[QueuesCount];
            for (int i = 0; i < QueuesCount; i++)
            {
                TasksMQDone[i] = false;
            }
        }
        object syncRoot = new object();
        public void Find()
        {
            for (int j = 0; j < QueuesCount; j++)
            {
                Entry targetEntry = Factory.GetEntry(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt"));
                int currentTaskNumber = j;
                Task.Run(() => {
                    var Queue = MQ[currentTaskNumber];
                    foreach (var file in Queue)
                    {
                        FindInFile(file, targetEntry);
                    }
                    TasksMQDone[currentTaskNumber] = true;
                });
            }
        }
        public int GetProcessedFilesCount()
        {
            return processedFiles;
        }
        public int GetTotalFilesCount()
        {
            return MQ.Capacity;
        }
        public bool Done()
        {
            for (int i = 0; i < MQ.Count; i++)
            {
                if (TasksMQDone[i] == false)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
    public class AsyncAwaitFinding : SingleQueue, Finding
    {
        IProgress<int> progress;
        bool isDone;
        Action OnCTSCancel;
        System.Threading.CancellationTokenSource cts;
        MyFolder folder;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt");
        Entry targetEntry = Factory.GetEntry(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Secrets.txt"));
        public AsyncAwaitFinding(IProgress<int> progress, System.Threading.CancellationTokenSource cts, Action OnCTSCancel, MyFolder folder, string target = null)
        {
            if (target != null)
            {
                this.target = target;
            }
            this.progress = progress;
            this.cts = cts;
            this.OnCTSCancel = OnCTSCancel;
            this.folder = folder;
        }       
        public void Init()
        {
            isDone = false;  
        }
        object cancelRoot = new object();
        public async void Find()
        {
            await Task.Run(() =>
            {
                try
                {
                    ResetSingleQueue();
                    FillSingleQueue(folder);
                    singleQueueSize = singleQueue.Count;
                    processedFiles = 0;
                    Parallel.ForEach<Entry>(singleQueue, (item, state) =>
                    {
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            FindInFile(item, targetEntry);
                            progress.Report(Convert.ToInt32((double)processedFiles / (double)singleQueueSize * 100));
                        }
                        catch (OperationCanceledException)
                        {
                            lock (cancelRoot)
                            {
                                state.Break();
                                OnCTSCancel();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    });
                    if (!cts.IsCancellationRequested) isDone = true;
                }
                catch (MyUnauthorizedAccessException)
                {
                    return;
                }
            });
        }
        public int GetProcessedFilesCount()
        {
            return 0;
        }
        public int GetTotalFilesCount()
        {
            return 0;
        }
        public bool Done()
        {
            return isDone;
        }
    }

}
