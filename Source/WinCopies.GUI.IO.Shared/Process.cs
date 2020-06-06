using Microsoft.WindowsAPICodePack.Win32Native;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinCopies.Collections.DotNetFix;
using WinCopies.IO;
using WinCopies.Util;
using static WinCopies.Util.Util;
using Size = WinCopies.IO.Size;

namespace WinCopies.GUI.IO
{
    public interface IPathInfo : WinCopies.IO.IPathInfo
    {
        Size? Size { get; }
    }

    public readonly struct PathInfo : IPathInfo
    {
        public string Path { get; }

        public Size? Size { get; }

        public bool IsDirectory => Size.HasValue;

        public PathInfo(string path, Size? size)
        {
            Path = path;

            Size = size;
        }
    }

    public class ProcessQueueCollection : ObservableQueueCollection<IPathInfo> // todo: inherits from ReadOnlyObservableQueueCollection when it has been implemented, make it sealed and make constructors public.
    {
        private Size _size;

        public Size Size { get => _size; private set { _size = value; RaisePropertyChangedEvent(nameof(Size)); } }

        internal ProcessQueueCollection() : base() { }

        internal ProcessQueueCollection(Queue<IPathInfo> queue) : base(queue) { }

        protected override void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<IPathInfo> e)
        {
            base.OnCollectionChanged(e);

            switch (e.Action)
            {
                case SimpleLinkedCollectionChangeAction.Added:

                    if (!e.Item.IsDirectory)

                        Size += e.Item.Size.Value;

                    break;

                case SimpleLinkedCollectionChangeAction.Cleared:

                    Size = new Size(0ul);

                    break;
            }
        }

        public void DecrementSize(ulong sizeInBytes) => Size -= sizeInBytes;
    }

    public class ReadOnlyObservableQueueCollection<T, U> : INotifyPropertyChanged where T : ObservableQueueCollection<U> // todo: remove when CopyProcessQueueCollection has been updated.
    {
        private readonly T _innerCollection;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableQueueCollection(T innerCollection)
        {
            _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

            innerCollection.PropertyChanged += PropertyChanged;
        }

        public U Peek() => _innerCollection.Peek();
    }

    public class ReadOnlyCopyProcessQueueCollection // todo: remove when CopyProcessQueueCollection has been updated.
    {
        private readonly ProcessQueueCollection _innerCollection;

        public Size Size => _innerCollection.Size;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyCopyProcessQueueCollection(ProcessQueueCollection innerCollection)
        {
            _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

            innerCollection.PropertyChanged += PropertyChanged;
        }

        public IPathInfo Peek() => _innerCollection.Peek();
    }

    public enum ProcessError : byte
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        UnknownError = 1,

        /// <summary>
        /// The process was aborted by user.
        /// </summary>
        AbortedByUser = 2,

        /// <summary>
        /// One part or all of the source or destination path was not found.
        /// </summary>
        PathNotFound = 3,

        /// <summary>
        /// The source or destination drive is not ready.
        /// </summary>
        DriveNotReady = 4,

        /// <summary>
        /// The source path is read-protected.
        /// </summary>
        ReadProtection = 5,

        /// <summary>
        /// The destination path is read-protected.
        /// </summary>
        DestinationReadProtection = 6,

        /// <summary>
        /// The destination path is write-protected.
        /// </summary>
        WriteProtection = 7,

        /// <summary>
        /// The source or destination path cannot be accessed.
        /// </summary>
        AccessDenied = 8,

        /// <summary>
        /// The destination path is too long.
        /// </summary>
        PathTooLong = 9,

        /// <summary>
        /// The destination disk has not enough space.
        /// </summary>
        NotEnoughSpace = 10,

        /// <summary>
        /// A file or folder already exists with the same name.
        /// </summary>
        FileSystemEntryAlreadyExists = 11,

        /// <summary>
        /// A folder already exists with the same name.
        /// </summary>
        FolderAlreadyExists = 12,

        /// <summary>
        /// The file could not be renamed.
        /// </summary>
        FileRenamingFailed = 13,

        /// <summary>
        /// The source and destination relative paths are equal.
        /// </summary>
        SourceAndDestPathAreEqual = 14,

        /// <summary>
        /// The destination path is a sub-path of the source path.
        /// </summary>
        DestPathIsASubPath = 15,

        /// <summary>
        /// An unknown disk error occurred.
        /// </summary>
        DiskError = 16,

        EncryptionFailed = 17
    }

    // todo: move to WinCopies.Util

    public class PausableBackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        public bool PausePending { get; private set; }

        private bool _workerSupportsPausing = false;

        public bool WorkerSupportsPausing { get => _workerSupportsPausing; set => _workerSupportsPausing = IsBusy ? throw new InvalidOperationException("The BackgroundWorker is running.") : value; }

        public void PauseAsync()
        {
            if (!_workerSupportsPausing)

                throw new InvalidOperationException("The BackgroundWorker does not support pausing.");

            if (IsBusy)

                PausePending = true;
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);

            PausePending = false;
        }
    }

    public interface IErrorPathInfo
    {
        IPathInfo Path { get; }

        ProcessError Error { get; }
    }

    public struct ErrorPathInfo : IErrorPathInfo
    {
        public IPathInfo Path { get; }

        public ProcessError Error { get; }

        public ErrorPathInfo(IPathInfo path, ProcessError error)
        {
            Path = path;

            Error = error;
        }
    }

    public abstract class Process : INotifyPropertyChanged
    {
        protected PausableBackgroundWorker BackgroundWorker { get; } = new PausableBackgroundWorker();

        public Size InitialSize { get; protected set; }

        public int InitialItemCount { get; protected set; }

        protected ProcessQueueCollection _Paths { get; } = new ProcessQueueCollection();

        public ReadOnlyCopyProcessQueueCollection Paths { get; }

        private bool _completed = false;

        public bool Completed { get => _completed; protected set { _completed = value; OnPropertyChanged(nameof(Completed)); } }

        public string SourcePath { get; }

        private bool _pathsLoaded = false;

        public bool ArePathsLoaded { get => _pathsLoaded; set { _pathsLoaded = value; OnPropertyChanged(nameof(ArePathsLoaded)); } }

        public bool WorkerSupportsCancellation { get => BackgroundWorker.WorkerSupportsCancellation; set { BackgroundWorker.WorkerSupportsCancellation = value; OnPropertyChanged(nameof(WorkerSupportsCancellation)); } }

        public bool WorkerReportsProgress { get => BackgroundWorker.WorkerReportsProgress; set { BackgroundWorker.WorkerReportsProgress = value; OnPropertyChanged(nameof(WorkerReportsProgress)); } }

        public bool IsBusy => BackgroundWorker.IsBusy;

        public bool CancellationPending => BackgroundWorker.CancellationPending;

        public bool WorkerSupportsPausing { get => BackgroundWorker.WorkerSupportsPausing; set => BackgroundWorker.WorkerSupportsPausing = value; }

        public bool PausePending => BackgroundWorker.PausePending;

        protected ObservableQueueCollection<IErrorPathInfo> _ErrorPaths { get; } = new ObservableQueueCollection<IErrorPathInfo>();

        public ReadOnlyObservableQueueCollection<ObservableQueueCollection<IErrorPathInfo>, IErrorPathInfo> ErrorPaths { get; }

        public ProcessError Error { get; private set; }

        public IPathInfo CurrentPath { get; private set; }

        public event DoWorkEventHandler DoWork;

        public event ProgressChangedEventHandler ProgressChanged;

        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        public Process(string sourcePath)
        {
            SourcePath = sourcePath;

            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

            DoWork += (object sender, DoWorkEventArgs e) => OnDoWorkProcess(e);

            BackgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProgressChanged(e);

            ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProcessProgressChanged(e);

            BackgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerCompleted(e);

            RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerProcessCompleted(e);

            Paths = new ReadOnlyCopyProcessQueueCollection(_Paths);

            ErrorPaths = new ReadOnlyObservableQueueCollection<ObservableQueueCollection<IErrorPathInfo>, IErrorPathInfo>(_ErrorPaths);
        }

        protected virtual void OnDoWork(DoWorkEventArgs e)
        {
            OnPropertyChanged(nameof(IsBusy));

            DoWork?.Invoke(this, e);
        }

        protected virtual void OnDoWorkProcess(DoWorkEventArgs e) { }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e) => ProgressChanged?.Invoke(this, e);

        protected virtual void OnProcessProgressChanged(ProgressChangedEventArgs e) { }

        protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged(nameof(IsBusy));

            RunWorkerCompleted?.Invoke(this, e);
        }

        protected virtual void OnRunWorkerProcessCompleted(RunWorkerCompletedEventArgs e) { }

        public void PauseAsync() => BackgroundWorker.PauseAsync();

        public void CancelAsync() => OnCancelAsync();

        protected virtual void OnCancelAsync()
        {
            BackgroundWorker.CancelAsync();

            OnPropertyChanged(nameof(CancellationPending));
        }

        public void RunWorkerAsync() => BackgroundWorker.RunWorkerAsync();

        public void RunWorkerAsync(object argument) => BackgroundWorker.RunWorkerAsync(argument);

        public void ReportProgress(int percentProgress) => BackgroundWorker.ReportProgress(percentProgress);

        public void ReportProgress(int percentProgress, object userState) => BackgroundWorker.ReportProgress(percentProgress, userState);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }

    public class CopyProcess : Process
    {
        private IEnumerator<IPathInfo> _pathsToLoadEnumerator;

        public string DestPath { get; }

        private ProcessError _error;

        public ProcessError Error { get => _error; private set { _error = value; OnPropertyChanged(nameof(Error)); } }

        private bool _autoRenamePaths;

        public bool AutoRenamePaths { get => _autoRenamePaths; set => _autoRenamePaths = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value; }

        private int _bufferLength;

        public int BufferLength { get => _bufferLength; set => _bufferLength = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value < 0 ? throw new ArgumentOutOfRangeException($"{nameof(value)} cannot be less than zero.") : value; }

        public CopyProcess(PathCollection pathsToLoad, string destPath) : base((pathsToLoad ?? throw GetArgumentNullException(nameof(pathsToLoad))).Path)
        {
            DestPath = WinCopies.IO.Path.IsFileSystemPath(destPath) && System.IO.Path.IsPathRooted(destPath) ? destPath : throw new ArgumentException($"{nameof(destPath)} is not a valid path.");

            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

            BackgroundWorker.RunWorkerAsync(pathsToLoad);
        }

        protected override void OnDoWorkProcess(DoWorkEventArgs e)
        {
            if (Completed)

                throw new InvalidOperationException("The process has already been completed.");

            bool checkIfPauseOrCancellationPending()
            {
                if (PausePending)
                {
                    Error = ProcessError.AbortedByUser;

                    return true;
                }

                if (CancellationPending)
                {
                    Error = ProcessError.AbortedByUser;

                    _Paths.Clear();

                    Completed = true;

                    return true;
                }

                return false;
            }

            void copy()
            {
                if (Paths.Size.ValueInBytes.IsNaN)
                {
                    Error = ProcessError.NotEnoughSpace;

                    return;
                }

                string drive = System.IO.Path.GetPathRoot(SourcePath);

                if (System.IO.Directory.Exists(drive))
                {
                    var driveInfo = new DriveInfo(drive);

                    if (driveInfo.IsReady)
                    {
                        drive = System.IO.Path.GetPathRoot(DestPath);

                        if (System.IO.Directory.Exists(drive))
                        {

                            if (driveInfo.IsReady)
                            {
                                if (driveInfo.TotalFreeSpace >= Paths.Size.ValueInBytes)
                                {
                                    IPathInfo path;

                                    void dequeueErrorPath(ProcessError _error)
                                    {

                                        _ErrorPaths.Enqueue(new ErrorPathInfo(path, _error));

                                        _ = _Paths.Dequeue();

                                    }

                                    string sourcePath;
                                    string destPath;
                                    bool alreadyRenamed = false;

                                    void renameOnDuplicate()
                                    {
                                        destPath = WinCopies.IO.Path.RenameOnDuplicate(destPath);

                                        alreadyRenamed = true;
                                    }

                                    CopyProgressRoutine copyProgressRoutine = (long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint streamNumber, CopyProgressCallbackReason copyProgressCallbackReason, IntPtr sourceFile, IntPtr destinationFile, IntPtr data) =>
                                    {
                                        if (_Paths.Size.ValueInBytes.IsNaN)

                                            copyProgressRoutine = (long _totalFileSize, long _totalBytesTransferred, long _streamSize, long _streamBytesTransferred, uint _streamNumber, CopyProgressCallbackReason _copyProgressCallbackReason, IntPtr _sourceFile, IntPtr _destinationFile, IntPtr _data) =>
                                             {
                                                 ReportProgress((_Paths.Count / InitialItemCount) * 100);

                                                 return CancellationPending ? CopyProgressResult.Cancel : CopyProgressResult.Continue;
                                             };

                                        else

                                            copyProgressRoutine = (long _totalFileSize, long _totalBytesTransferred, long _streamSize, long _streamBytesTransferred, uint _streamNumber, CopyProgressCallbackReason _copyProgressCallbackReason, IntPtr _sourceFile, IntPtr _destinationFile, IntPtr _data) =>
                                        {
                                            _Paths.DecrementSize((ulong)_totalBytesTransferred);

                                            ReportProgress((int)(_Paths.Size / InitialSize) * 100);

                                            return CancellationPending ? CopyProgressResult.Cancel : CopyProgressResult.Continue;
                                        };

                                        return CopyProgressResult.Quiet;
                                    };

                                    void copyFileOrCreateDirectory()
                                    {
                                        bool cancel = false;
                                        CopyFileFlags copyFileFlags = CopyFileFlags.FailIfExists | CopyFileFlags.NoBuffering;

                                        if (path.Path.EndsWith(".lnk"))

                                            copyFileFlags |= CopyFileFlags.CopySymLink;

                                        bool result = path.Size.HasValue ? Shell.CopyFileEx(sourcePath, destPath, copyProgressRoutine , IntPtr.Zero, ref cancel, copyFileFlags) : Directory.CreateDirectoryW(destPath, IntPtr.Zero );

                                        if (result)

                                            return;

                                        var error = (ErrorCode)Marshal.GetLastWin32Error();

                                        switch (error)
                                        {
                                            case ErrorCode.AlreadyExists:

                                                if (path.Size.HasValue) // We do not try to rename folders, because folder name conflicts are handled the same way as file name conflicts.
                                                {
                                                    if (alreadyRenamed)
                                                    {
                                                        dequeueErrorPath(ProcessError.FileRenamingFailed);

                                                        return;
                                                    }

                                                    if (_autoRenamePaths)
                                                    {
                                                        renameOnDuplicate();

                                                        copyFileOrCreateDirectory();

                                                        return;
                                                    }
                                                }

                                                dequeueErrorPath(ProcessError.FileSystemEntryAlreadyExists);

                                                break;

                                            case ErrorCode.PathNotFound:

                                                dequeueErrorPath(ProcessError.PathNotFound);

                                                break;

                                            case ErrorCode.AccessDenied:

                                                dequeueErrorPath(ProcessError.AccessDenied);

                                                break;

                                            case ErrorCode.DiskFull:

                                                dequeueErrorPath(ProcessError.NotEnoughSpace);

                                                break;

                                            case ErrorCode.DiskOperationFailed:

                                                dequeueErrorPath(ProcessError.DiskError);

                                                break;

                                            case (ErrorCode)6000: // Encryption failed

                                                dequeueErrorPath(ProcessError.EncryptionFailed);

                                                break;

                                            default:

                                                dequeueErrorPath(ProcessError.UnknownError);

                                                break;
                                        }
                                    }

                                    while (_Paths.Count > 0)
                                    {
                                        if (checkIfPauseOrCancellationPending())

                                            return;

                                        path = _Paths.Peek();

                                        sourcePath = $"{SourcePath}{WinCopies.IO.Path.PathSeparator}{path.Path}";

                                        destPath = $"{DestPath}{WinCopies.IO.Path.PathSeparator}{path.Path}";

                                        if (WinCopies.IO.Path.Exists(destPath))

                                            if (_autoRenamePaths && _bufferLength == 0)

                                                renameOnDuplicate();

                                            else if (_bufferLength > 0)
                                            {
                                                try
                                                {
                                                    var sourceFileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None);

                                                    try
                                                    {
                                                        var destFileStream = new FileStream(destPath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None);

                                                        bool? _result = WinCopies.IO.File.IsDuplicate(sourceFileStream, destFileStream, _bufferLength, () =>
                                                                   PausePending || CancellationPending
                                                                );

                                                        if (_result.HasValue && _result.Value)
                                                        {
                                                            if (checkIfPauseOrCancellationPending())

                                                                return;

                                                            if (_autoRenamePaths)

                                                                renameOnDuplicate();

                                                            else
                                                            {
                                                                dequeueErrorPath(ProcessError.FileSystemEntryAlreadyExists);

                                                                continue;
                                                            }
                                                        }

                                                        else
                                                        {
                                                            if (checkIfPauseOrCancellationPending())

                                                                return;

                                                            _ = _Paths.Dequeue();
                                                        }
                                                    }
                                                    catch (System.IO.FileNotFoundException)
                                                    {
                                                        // Left empty.
                                                    }

                                                    catch (Exception ex) when (ex.Is(false, typeof(System.UnauthorizedAccessException), typeof(System.Security.SecurityException)))
                                                    {
                                                        dequeueErrorPath(ProcessError.DestinationReadProtection);

                                                        continue;
                                                    }
                                                }
                                                catch (System.IO.FileNotFoundException)
                                                {
                                                    dequeueErrorPath(ProcessError.PathNotFound);

                                                    continue;
                                                }

                                                catch (System.IO.DirectoryNotFoundException)
                                                {
                                                    dequeueErrorPath(ProcessError.PathNotFound);

                                                    continue;
                                                }

                                                catch (System.IO.PathTooLongException)
                                                {
                                                    dequeueErrorPath(ProcessError.PathTooLong);

                                                    continue;
                                                }

                                                catch (System.IO.IOException)
                                                {
                                                    dequeueErrorPath(ProcessError.UnknownError);

                                                    continue;
                                                }

                                                catch (Exception ex) when (ex.Is(false, typeof(System.UnauthorizedAccessException), typeof(System.Security.SecurityException)))
                                                {
                                                    dequeueErrorPath(ProcessError.ReadProtection);

                                                    continue;
                                                }
                                            }

                                            else
                                            {
                                                dequeueErrorPath(ProcessError.FileSystemEntryAlreadyExists);

                                                continue;
                                            }

                                        copyFileOrCreateDirectory();
                                    }

                                    Completed = true;

                                    return;
                                }
                                else
                                {
                                    Error = ProcessError.NotEnoughSpace;

                                    return;
                                }
                            }
                        }
                    }
                }

                Error = ProcessError.DriveNotReady;
            }

            void loadPathsAndCopy(IEnumerator<IPathInfo> pathsToLoad)
            {
                while (pathsToLoad.MoveNext())
                {
                    if (checkIfPauseOrCancellationPending())

                        return;

                    _Paths.Enqueue(pathsToLoad.Current);
                }

                InitialSize = _Paths.Size;

                InitialItemCount = _Paths.Count;

                ArePathsLoaded = true;

                _pathsToLoadEnumerator = null;

                copy();
            }

            if (ArePathsLoaded)

                copy();

            else if (_pathsToLoadEnumerator != null)

                loadPathsAndCopy(_pathsToLoadEnumerator);

            else
            {
                IEnumerator<IPathInfo> _pathsToLoad = ((PathCollection)e.Argument).GetEnumerator();

                if (WorkerSupportsPausing)

                    _pathsToLoadEnumerator = _pathsToLoad;

                loadPathsAndCopy(_pathsToLoad);
            }
        }
    }
}
