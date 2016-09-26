using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileManager
{
    public delegate void AskModelGenerateModelChangeEvent();
    public delegate void AskModelChangeDriveEvent(MyFolder newDrive);
    public delegate void AskModelChangeDirectoryEvent(MyFolder newDirectory);
    public delegate void AskModelGoUpEvent();
    public delegate void AskModelAsyncAwaitZippingEvent(IProgress<int> progress, System.Threading.CancellationTokenSource cts, Action OnCTSCancel,
            List<string> items, Action reset, Action<int> reportProgress, Action<TimeSpan> complited);
    public delegate void AskModelCryptionEvent(List<Entry> entries, bool crypt, string key);
    interface IView
    {
        void OnModelChange(ModelChangeEventArgs e);
        event AskModelGenerateModelChangeEvent AskModelGenerateModelChange;
        event AskModelChangeDriveEvent AskModelChangeDrive;
        event AskModelChangeDirectoryEvent AskModelChangeDirectory;
        event AskModelGoUpEvent AskModelGoUp;
        event Action<Entry> AskModelDeleteEntry;
        event Action<MyFile> AskModelExecuteFile;
        event Action<List<Entry>> AskModelCopyEntries;
        event Action<List<Entry>> AskModelMoveEntries;
        event Action<List<Entry>, Action, Action<int>, Action<TimeSpan>> AskModelMultiThreadingZipping;
        event Action<List<string>, Action, Action<int>, Action<TimeSpan>> AskModelASyncDelegateZipping;
        event Action<List<string>, Action, Action<int>, Action<TimeSpan>> AskModelParallelForEachZipping;
        event Action<List<Entry>, Action, Action<int>, Action<TimeSpan>> AskModelTaskZipping;
        event AskModelAsyncAwaitZippingEvent AskModelAsyncAwaitZipping;
        event Action<Finding, Action, Action<int>, Action<TimeSpan>> AskModelFinding;
        event Action<MyFile> AskModelUnzipFile;
        event Action<Entry, string> AskModelRenameEntry;
        event Func<Entry, string> AskModelCalcMD5;
        event Func<MyFile, Encoding> AskModelGetEncoding;
        event Func<MyFile, string> AskModelGetPermissions;
        event Func<MyFile, string> AskModelGetTXTStats;
        event AskModelCryptionEvent AskModelCryption;
    }
}
