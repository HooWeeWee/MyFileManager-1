using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace MyFileManager
{
    public partial class DownloadForm : Form
    {
        FileWebResponse myFileWebResponse;
        public DownloadForm()
        {
            InitializeComponent();
        }
        private void buttonStartDownload_Click(object sender, EventArgs e)
        {
            Reset();
            buttonCancel.Enabled = false;
            string uri = textBoxUri.Text;
            GetFileWithWebClient(uri);      
        }
        private void Reset()
        {
            buttonCancel.Enabled = true;
            progressBar1.Value = progressBar1.Minimum;
            textBoxResult.Text = "";
        }
        string wcResult;
        WebClient wc = new WebClient();
        private void GetFileWithWebClient(string uri)
        {
            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                if (eArgs.Cancelled)
                {
                    wcResult = "";
                    progressBar1.Value = progressBar1.Minimum;
                    textBoxResult.Text = "Cancelled!";
                    buttonCancel.Enabled = true;
                }
                else
                {
                    try
                    {
                        wcResult = eArgs.Result;
                        progressBar1.Value = progressBar1.Maximum;
                        textBoxResult.AppendText(wcResult.Substring(0, 255));
                        buttonCancel.Enabled = false;
                    }
                    catch (AggregateException exc)
                    {
                        MessageBox.Show(exc.InnerException.Message);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            };
            wc.DownloadProgressChanged += (sender, e) => { progressBar1.Value = e.ProgressPercentage; };
            wc.DownloadStringAsync(new Uri(uri));
        }
        private bool makeFileRequest(string uri)
        {
            bool isRequestOk = false;
            try
            {
                Uri myUrl = new Uri(uri);
                // Create a FileWebRequest object using the passed URI. 
                FileWebRequest myFileWebRequest = (FileWebRequest)WebRequest.Create(myUrl);
                // Get the FileWebResponse object.
                myFileWebResponse = (FileWebResponse)myFileWebRequest.GetResponse();
                isRequestOk = true;
            }
            catch (WebException e)
            {
                Console.WriteLine("WebException: " + e.Message);
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("UriFormatWebException: " + e.Message);
            }
            return isRequestOk;
        }
        private void printFile()
        {
            try
            {
                // Create the file stream. 
                Stream receiveStream = myFileWebResponse.GetResponseStream();
                // Create a reader object to read the file content.
                StreamReader readStream = new StreamReader(receiveStream);
                // Create a local buffer for a temporary storage of the 
                // read data.
                char[] readBuffer = new Char[256];
                // Read the first 256 bytes.
                int count = readStream.Read(readBuffer, 0, 256);
                Console.WriteLine("The file content is:");
                Console.WriteLine("");
                // Loop to read the remaining data in blocks of 256 bytes
                // and display the data on the console.
                while (count > 0)
                {
                    String str = new String(readBuffer, 0, count);
                    Console.WriteLine(str + "\n");
                    count = readStream.Read(readBuffer, 0, 256);
                }

                readStream.Close();

                // Release the response object resources.
                myFileWebResponse.Close();

            }
            catch (WebException e)
            {
                Console.WriteLine("The WebException: " + e.Message);
            }
            catch (UriFormatException e)
            {
                Console.WriteLine("The UriFormatException: " + e.Message);
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            wc.CancelAsync();
        }
    }
}
