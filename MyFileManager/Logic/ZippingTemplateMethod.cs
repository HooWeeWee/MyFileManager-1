using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyFileManager
{
    abstract class Zipping
    {
        Stopwatch sw;
        Timer timer;
        Action reset;
        Action<int> reportProgress;
        Action<TimeSpan> complited;
        protected abstract void Init();
        protected abstract void Zip();
        protected abstract bool Done
        {
            get;
        }
        protected abstract int TotalFiles
        {
            get;
        }
        protected abstract int ProcessedFiles
        {
            get;
        }
        public Zipping(Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
        {
            this.reset = reset;
            this.reportProgress += reportProgress; ;
            this.complited += complited;
        }
        public void Run()
        {
            reset();
            Init();
            StartTimer();
            Zip();
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

        protected Queue<string> singleQueue;
        protected int singleQueueSize;
        protected int processedFiles;
        object syncRoot = new object();
        protected void ZipSingleQueue()
        {
            try
            {
                processedFiles = 0;
                foreach (var file in singleQueue)
                {
                    ZipFile(file);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
        }
        protected void ZipFile(string file)
        {
            try
            {
                FileManager.Compress(file);
                processedFiles++;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message + " while Zipping");
            }
        }
        protected void FillSingleQueue(IEnumerable<string> items) //заполнение очереди
        {
            foreach (var path in items)
            {
                try
                {
                    MyFile file = new MyFile(path);
                    if (file.Exists)
                    {
                        singleQueue.Enqueue(path);
                    }
                    else
                    {
                        MyFolder folder = new MyFolder(path);
                        if (folder.Exists)
                        {
                            var fileList = folder.DirectoryGetFiles;
                            foreach (var f in fileList)
                            {
                                singleQueue.Enqueue(f.FullPath);
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            singleQueueSize = singleQueue.Count;
        }
        protected void ResetSingleQueue() //очистка очереди
        {
            singleQueue = new Queue<string>();
            singleQueueSize = 0;
        }
    }

    class MultiThreadingZipping : Zipping
    {
        List<Entry> items;
        MultiThreads MT;
        public MultiThreadingZipping(List<Entry> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
            : base(reset, reportProgress, complited)
        {
            this.items = items;
        }
        protected override bool Done
        {
            get
            {
                return !MT.IsAlive;
            }
        }

        protected override int ProcessedFiles
        {
            get
            {
                return MT.GetProcessedFiles();
            }
        }

        protected override int TotalFiles
        {
            get
            {
                return MT.GetTotalFiles();
            }
        }

        protected override void Init()
        {
            MT = new MultiThreads(items, false);
        }

        protected override void Zip()
        {
            MT.StartProcess(FuncType.Zipping);
        }
    }

    class ASyncDelegateZipping : Zipping
    {
        List<string> items;
        bool isDone;
        public ASyncDelegateZipping(List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
            : base(reset, reportProgress, complited)
        {
            this.items = items;
        }
        protected override bool Done
        {
            get
            {
                return isDone;
            }
        }

        protected override int ProcessedFiles
        {
            get
            {
                return processedFiles;
            }
        }

        protected override int TotalFiles
        {
            get
            {
                return singleQueueSize;
            }
        }

        protected override void Init()
        {
            isDone = false;
            ResetSingleQueue();
            FillSingleQueue(items);
        }
        private void ZipHandler(IEnumerable<string> items)
        {
            ResetSingleQueue();
            FillSingleQueue(items);
            ZipSingleQueue();
        }

        protected override void Zip()
        {
            LongOperationZip itemsZipping = ZipHandler;
            IAsyncResult iAR = itemsZipping.BeginInvoke(items, new AsyncCallback(Callback), new object());
        }
        private void Callback(IAsyncResult obj)
        {
            isDone = true;
        }
    }

    class ParallelForEachZipping : Zipping
    {
        List<string> items;
        bool isDone;
        public ParallelForEachZipping(List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
            : base(reset, reportProgress, complited)
        {
            this.items = items;
        }
        protected override bool Done
        {
            get
            {
                return isDone;
            }
        }

        protected override int ProcessedFiles
        {
            get
            {
                return processedFiles;
            }
        }

        protected override int TotalFiles
        {
            get
            {
                return singleQueueSize;
            }
        }

        protected override void Init()
        {
            isDone = false;
            ResetSingleQueue();
            FillSingleQueue(items);
        }
        protected override void Zip()
        {
            Task.Run(() =>
            {
                Parallel.ForEach(singleQueue, file =>
                {
                    ZipFile(file);
                });
                isDone = true;
            });
        }
    }
    class TaskZipping : Zipping
    {
        MultiQueues MQ;
        bool[] TasksMQDone;
        List<Entry> items;
        int QueuesCount;
        public TaskZipping(List<Entry> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
            : base(reset, reportProgress, complited)
        {
            this.items = items;
        }
        protected override bool Done
        {
            get
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

        protected override int ProcessedFiles
        {
            get
            {
                return processedFiles;
            }
        }

        protected override int TotalFiles
        {
            get
            {
                return MQ.Capacity;
            }
        }
        protected override void Init()
        {
            MQ = new MultiQueues(items, true);
            QueuesCount = MQ.Count;
            TasksMQDone = new bool[QueuesCount];
            for (int i = 0; i < QueuesCount; i++)
            {
                TasksMQDone[i] = false;
            }
            processedFiles = 0;
        }
        object syncRoot = new object();
        protected override void Zip()
        {
            for (int j = 0; j < QueuesCount; j++)
            {
                int currentTaskNumber = j;
                Task.Run(() =>
                {
                    var Queue = MQ[currentTaskNumber];
                    foreach (var item in Queue)
                    {
                        ZipFile(item.FullPath);
                    }
                    TasksMQDone[currentTaskNumber] = true;
                });
            }
        }
    }
    class AsyncAwaitZipping : Zipping
    {
        IProgress<int> progress;
        List<string> items;
        bool isDone;
        Action OnCTSCancel;
        System.Threading.CancellationTokenSource cts;
        public AsyncAwaitZipping(IProgress<int> progress, System.Threading.CancellationTokenSource cts, Action OnCTSCancel,
            List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited)
            : base(reset, reportProgress, complited)
        {
            this.items = items;
            this.progress = progress;
            this.cts = cts;
            this.OnCTSCancel += OnCTSCancel;
        }
        protected override void  Init()
        {
            isDone = false;
            ResetSingleQueue();
            FillSingleQueue(items);
        }
        object cancelRoot = new object();
        protected override void Zip()
        {
            zip();
        }
        bool cancelled = false;
        private async void zip()
        {
            await Task.Run(() =>
            {
                ResetSingleQueue();
                FillSingleQueue(items);
                singleQueueSize = singleQueue.Count;
                processedFiles = 0;
                Parallel.ForEach<string>(items, (item, state) =>
                {
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        ZipFile(item);
                        progress.Report(Convert.ToInt32((double)processedFiles / (double)singleQueueSize * 100));
                    }
                    catch (OperationCanceledException)
                    {
                        lock (cancelRoot)
                        {
                            cancelled = true;
                            state.Break();
                            OnCTSCancel();
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message + " while Zipping");
                    }
                });
                if (!cancelled) isDone = true;
            });
        }
        protected override int ProcessedFiles
        {
            get
            {
                return 0;
            }
        }
        protected override int TotalFiles
        {
            get
            {
                return 0;
            }
        }
        protected override bool Done
        {
            get
            {
                return isDone;
            }
        }
    }
}
