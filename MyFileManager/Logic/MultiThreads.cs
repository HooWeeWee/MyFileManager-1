using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace MyFileManager
{
    public enum FuncType {Zipping, Unzipping, Finding};
    public class MultiThreads
    {
        readonly static int CORES = Environment.ProcessorCount - 1;
        static int totalFiles = 0;
        static int processedFiles = 0;
        MultiQueues Queues;
        Thread[] Threads = new Thread[CORES];
        public bool IsAlive
        {
            get
            {
                foreach (var t in Threads)
                {
                    if (t != null && t.IsAlive) return true;
                }
                return false;
            }
        }
        public MultiThreads(MyFolder folder, bool deep = false)
        {
            Queues = new MultiQueues(CORES, folder, deep);            
        }
        public MultiThreads(IEnumerable<Entry> items, bool deep = false)
        {
            Queues = new MultiQueues(CORES, items, deep);
        }
        public void StartProcess(FuncType type)
        {
            switch (type)
            {
                case FuncType.Finding: 
                    {
                        InitializeThreads((ParameterizedThreadStart)Finding);
                        break;
                    }
                case FuncType.Zipping:
                    {
                        InitializeThreads((ParameterizedThreadStart)Zipping);
                        break;
                    }
                case FuncType.Unzipping:
                    {
                        InitializeThreads((ParameterizedThreadStart)Unzipping);
                        break;
                    }
            }
        }
        private void InitializeThreads(ParameterizedThreadStart threadStart)
        {
            for (int i = 0; i < CORES; i++)
            {
                Threads[i] = new Thread(threadStart);
                Threads[i].IsBackground = true;
                Threads[i].Start(Queues[i]);
                totalFiles += Queues[i].Count;
            }
        }
        public void EndProcess()
        {
        }
        string target = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "Secrets.txt");
        private static readonly object syncRoot = new object();
        private static readonly object readLineSyncRoot = new object();
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
        private void Finding(Object obj)
        {
            var Q = (Queue<Entry>)obj;
            foreach (var file in Q)
            {
                try
                {
                    using (StreamReader SR = file.FileGetStreamReader())
                    {
                        string line;
                        MyFile targetFile = new MyFile(target);
                        while ((line = GetLineWithLock(SR)) != null)
                        {
                            int i = -1;
                            foreach (var reg in RegExpsArray)
                            {
                                i++;
                                var matches = reg.Matches(line);
                                foreach (Match match in matches)
                                {
                                    if (match.Success)
                                    {
                                        lock (syncRoot)
                                        {
                                            targetFile.FileAppendAllText(i + " " + match.Value + Environment.NewLine);
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
                    processedFiles++;
                }
            }
        }
        private string GetLineWithLock(StreamReader SR)
        {
            lock (readLineSyncRoot)
            {
                return SR.ReadLine();
            }
        }
        private void Zipping(Object obj)
        {
            var Q = (Queue<string>)obj;
            foreach (var file in Q)
            {
                //System.Windows.Forms.MessageBox.Show(file + " is processing");
                FileManager.Compress(file);
                processedFiles++;
            }
        }
        private void Unzipping(Object obj)
        {
            var Q = (Queue<string>)obj;
            foreach (var file in Q)
            {
                FileManager.Decompress(file);
                processedFiles++;
            }
        }
        public string GetState(out double rate)
        {
            rate = (double)processedFiles / (double)totalFiles;
            return processedFiles + " / " + totalFiles + " processed.";
        }
        public int GetProcessedFiles()
        {
            return processedFiles;
        }
        public int GetTotalFiles()
        {
            return totalFiles;
        }
    }
}
