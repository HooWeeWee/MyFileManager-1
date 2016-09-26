using System;
using System.Collections.Generic;
using System.IO;

namespace MyFileManager
{
    public class MultiQueues
    {
        readonly static int CORES = Environment.ProcessorCount - 1;
        int count;
        public int Count
        {
            get
            {
                return count;
            }
        }
        int capacity;
        public int Capacity
        {
            get
            {
                return capacity;
            }
        }
        Queue<Entry>[] Queues;
        private void Initialize(int count)
        {
            this.count = count;
            Queues = new Queue<Entry>[count];
            for (int i = 0; i < count; i++)
            {
                Queues[i] = new Queue<Entry>();
            }
        }
        private void SetCapacity()
        {
            capacity = 0;
            for (int i = 0; i<count; i++)
            {
                capacity += Queues[i].Count;
            }
        }
        public Queue<Entry> this[int i]
        {
            get
            {
                return Queues[i];
            }
        }
        private bool includeDirectoryNames;
        public MultiQueues(MyFolder folder, bool deep = false, bool includeDirectoryNames = false) : this(CORES, folder, deep, includeDirectoryNames)
        { }
        public MultiQueues(int count, MyFolder folder, bool deep = false, bool includeDirectoryNames = false)
        {
            this.includeDirectoryNames = includeDirectoryNames;
            Initialize(count);
            if (deep)
            {
                FillQueuesDeep(folder);
            }
            else
            {
                FillQueues(folder);
            }
            SetCapacity();
        }
        public MultiQueues(IEnumerable<Entry> items, bool deep = false) : this(CORES, items, deep)
        { }
        public MultiQueues(int count, IEnumerable<Entry> entries, bool deep = false)
        {
            Initialize(count);
            if (deep)
            {
                foreach (var entry in entries)
                {
                    if (entry.Exists)
                    {
                        if (entry.Type == EntryType.Folder)
                        {
                            FillQueuesDeep((MyFolder)entry);
                        }
                        else if (entry.Type == EntryType.File)
                        {
                            AddSingleFile(entry);
                        }
                    }
                };
            }
            else
            {
                foreach (var entry in entries)
                {
                    if (entry.Exists)
                    {
                        if (entry.Type == EntryType.Folder)
                        {
                            FillQueues((MyFolder)entry);
                        }
                        else if (entry.Type == EntryType.File)
                        {
                            AddSingleFile(entry);
                        }
                    }
                };
            }
            SetCapacity();
        }
        void FillQueues(MyFolder folder)
        {
            if (includeDirectoryNames) AddSingleFile(folder);
            var files = folder.DirectoryGetFiles;
            int i = GetIndex();
            foreach (var file in files)
            {
                Queues[i].Enqueue(file);
                i++;
                if (i == count) i = 0;
            }
        }
        void FillQueuesDeep(MyFolder folder)
        {
            try
            {
                if (includeDirectoryNames) AddSingleFile(folder);
                var files = folder.DirectoryGetFiles;
                var dirs = folder.DirectoryGetFolders;
                int i = GetIndex();
                foreach (var file in files)
                {
                    Queues[i].Enqueue(file);
                    i++;
                    if (i == count) i = 0;
                }
                foreach (var dir in dirs)
                {
                    FillQueuesDeep(dir);
                }
            }
            catch (Exception)
            {
            }
        }
        void AddSingleFile(Entry entry)
        {
            int i = GetIndex();
            Queues[i].Enqueue(entry);
            i++;
            if (i == count) i = 0;
        }
        int GetIndex()
        {
            //int min = 0;
            //for (int i = 1; i < count; i++)
            //{
            //    if (Queues[i].Count < min) min = i;
            //}
            //return min;
            Random rand = new Random();
            return rand.Next(count);
        }
    }
}
