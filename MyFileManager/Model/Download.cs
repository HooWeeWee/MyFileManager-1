using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MyFileManager
{
    class Download : IProgress<int>
    {
        public ListViewItem LVItem;
        public CancellationTokenSource cts;
        private CancellationToken ct;
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
        }
        private double status;
        public double Status
        {
            get
            {
                return status;
            }
        }
        private bool isCompleted;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }
        private bool isDownloading;
        public bool IsDownloading
        {
            get
            {
                return isDownloading;
            }
        }
        private FileStream writer;
        public FileStream Writer
        {
            get
            {
                return writer;
            }
        }
        private long contentLenght;
        public long ContentLenght
        {
            get
            {
                return contentLenght;
            }
        }
        private long currentSize;
        public long CurrentSize
        {
            get
            {
                return currentSize;
            }           
        }
        private int bufferSize;
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                bufferSize = value;
            }
        }
        private Action<int, ListViewItem> progressHandler;
        public Download(ListViewItem LVItem, string url, FileStream writer, Action<int, ListViewItem> progressHandler, CancellationTokenSource cts, int bufferSize = 256)
        {
            this.LVItem = LVItem;
            this.url = url;
            this.writer = writer;
            this.progressHandler = progressHandler;
            this.cts = cts;
            this.ct = cts.Token;
            this.bufferSize = bufferSize;
            status = 0;
            contentLenght = 0;
            isCompleted = false;
            isDownloading = false;
        }
        public async Task DownloadAsync()
        {
            await downloadAsync();
            return;
        }
        private async Task downloadAsync()
        {
            await Task.Run(() =>
           {
               try
               {
                   //устанавливаем подключение
                   HttpWebRequest myFileWebRequest = (HttpWebRequest)WebRequest.Create(url);
                   HttpWebResponse myFileWebResponse = (HttpWebResponse)myFileWebRequest.GetResponse();
                   contentLenght = myFileWebResponse.ContentLength;
                   //начинаем загрузку
                   Stream receiveStream = myFileWebResponse.GetResponseStream();
                   BinaryReader readStream = new BinaryReader(receiveStream);
                   byte[] readBuffer = new byte[bufferSize];
                   int count = readStream.Read(readBuffer, 0, bufferSize);
                   currentSize = count;
                   this.Report(Convert.ToInt32((double)currentSize / (double)contentLenght * 100));
                   while (count > 0)
                   {
                       ct.ThrowIfCancellationRequested();
                       writer.Write(readBuffer, 0, count);
                       count = readStream.Read(readBuffer, 0, bufferSize);
                       currentSize += count;
                       this.Report(Convert.ToInt32((double)currentSize / (double)contentLenght * 100));
                   }
                   readStream.Close();
                   //закрываем подключение
                   myFileWebResponse.Close();
                   isCompleted = true;
               }
               catch (OperationCanceledException)
               {

               }
           });
        }
        public void Report(int value)
        {
            progressHandler(value, LVItem);
        }
    }
}
