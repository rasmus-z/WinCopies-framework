/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.WindowsAPICodePack.Win32Native;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using WinCopies.Collections.DotNetFix;
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

    public sealed class ProcessQueueCollection : ReadOnlyObservableQueueCollection<IPathInfo> // todo: inherits from ReadOnlyObservableQueueCollection when it has been implemented, make it sealed and make constructors public.
    {
        private Size _size;

        public Size Size { get => _size; private set { _size = value; RaisePropertyChangedEvent(nameof(Size)); } }

        public ProcessQueueCollection(ObservableQueueCollection<IPathInfo> queueCollection) : base(queueCollection) { }

        protected override void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<IPathInfo> e)
        {
            base.OnCollectionChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    if (!e.Item.IsDirectory)

                        Size += e.Item.Size.Value;

                    break;

                case NotifyCollectionChangedAction.Reset:

                    Size = new Size(0ul);

                    break;
            }
        }

        public void DecrementSize(ulong sizeInBytes) => Size -= sizeInBytes;
    }

    //public class ReadOnlyObservableQueueCollection<T, U> : INotifyPropertyChanged where T : ObservableQueueCollection<U> // todo: remove when CopyProcessQueueCollection has been updated.
    //{
    //    private readonly T _innerCollection;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public ReadOnlyObservableQueueCollection(T innerCollection)
    //    {
    //        _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

    //        innerCollection.PropertyChanged += PropertyChanged;
    //    }

    //    public U Peek() => _innerCollection.Peek();
    //}

    //public class ReadOnlyCopyProcessQueueCollection // todo: remove when CopyProcessQueueCollection has been updated.
    //{
    //    private readonly ProcessQueueCollection _innerCollection;

    //    public Size Size => _innerCollection.Size;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public ReadOnlyCopyProcessQueueCollection(ProcessQueueCollection innerCollection)
    //    {
    //        _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

    //        innerCollection.PropertyChanged += PropertyChanged;
    //    }

    //    public IPathInfo Peek() => _innerCollection.Peek();
    //}

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

        protected ObservableQueueCollection<IPathInfo> _Paths { get; } = new ObservableQueueCollection<IPathInfo>();

        public ProcessQueueCollection Paths { get; }

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

        public ReadOnlyObservableQueueCollection< IErrorPathInfo> ErrorPaths { get; }

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

            Paths = new ProcessQueueCollection(_Paths);

            ErrorPaths = new ReadOnlyObservableQueueCollection<IErrorPathInfo>(_ErrorPaths);
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

#if DEBUG

    public delegate bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags);

    public enum PathDirectoryType
    {
        Source,

        Destination
    }

    public class CopyProcessSimulationParameters
    {

        public Size PathsSize { get; set; }

        public bool SourcePathRootExists { get; set; }

        public bool SourceDriveReady { get; set; }

        public bool DestPathRootExists { get; set; }

        public bool DestDriveReady { get; set; }

        public long DestDriveTotalFreeSpace { get; set; }

        private Func<string, string> _renameOnDuplicateAction;

        private InvalidOperationException GetInvalidOperationException() => new InvalidOperationException("Value cannot be null.");

        public Func<string, string> RenameOnDuplicateAction { get => _renameOnDuplicateAction ?? throw GetInvalidOperationException(); set => _renameOnDuplicateAction = value ?? throw GetInvalidOperationException(); }

        private CopyFileEx _copyFileExAction;

        public CopyFileEx CopyFileExAction { get => _copyFileExAction ?? throw GetInvalidOperationException(); set => _copyFileExAction = value ?? throw GetInvalidOperationException(); }

        private Func<string, bool> _createDirectoryWAction;

        public Func<string, bool> CreateDirectoryWAction { get => _createDirectoryWAction ?? throw GetInvalidOperationException(); set => _createDirectoryWAction = value ?? throw GetInvalidOperationException(); }

        private Func<string, bool> _destPathExistsAction;

        public Func<string, bool> DestPathExistsAction { get => _destPathExistsAction ?? throw GetInvalidOperationException(); set => _destPathExistsAction = value ?? throw GetInvalidOperationException(); }

        private Func<string, PathDirectoryType, Exception> _creatingFileStreamSucceedsAction;

        public Func<string, PathDirectoryType, Exception> CreatingFileStreamSucceedsAction { get => _creatingFileStreamSucceedsAction ?? throw GetInvalidOperationException(); set => _creatingFileStreamSucceedsAction = value ?? throw GetInvalidOperationException(); }

        private Func<string, string, Func<bool>, bool?> _isDuplicateAction;

        public Func<string, string, Func<bool>, bool?> IsDuplicateAction { get => _isDuplicateAction ?? throw GetInvalidOperationException(); set => _isDuplicateAction = value ?? throw GetInvalidOperationException(); }

    }

#endif 

    public class CopyProcess : Process
    {
        private IEnumerator<IPathInfo> _pathsToLoadEnumerator;

#if DEBUG

        public CopyProcessSimulationParameters SimulationParameters { get; }

#endif

        public string DestPath { get; }

        private ProcessError _error;

        public ProcessError Error { get => _error; private set { _error = value; OnPropertyChanged(nameof(Error)); } }

        private bool _autoRenamePaths;

        public bool AutoRenamePaths { get => _autoRenamePaths; set => _autoRenamePaths = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value; }

        private int _bufferLength;

        public int BufferLength { get => _bufferLength; set => _bufferLength = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value < 0 ? throw new ArgumentOutOfRangeException($"{nameof(value)} cannot be less than zero.") : value; }

        public CopyProcess(PathCollection pathsToLoad, string destPath
#if DEBUG
            , CopyProcessSimulationParameters simulationParameters
#endif
            ) : base((pathsToLoad ?? throw GetArgumentNullException(nameof(pathsToLoad))).Path)
        {
            SimulationParameters = simulationParameters;

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
                if (
#if DEBUG
                    (SimulationParameters?.PathsSize ??
#endif
                    Paths.Size
#if DEBUG
                    )
#endif
                    .ValueInBytes.IsNaN)
                {
                    Error = ProcessError.NotEnoughSpace;

                    return;
                }

                string drive = System.IO.Path.GetPathRoot(SourcePath);

                if (
#if DEBUG
                    SimulationParameters?.SourcePathRootExists ??
#endif
                    System.IO.Directory.Exists(drive))
                {
                    var driveInfo = new DriveInfo(drive);

                    if (
#if DEBUG
                    SimulationParameters?.SourceDriveReady ??
#endif
                    driveInfo.IsReady)
                    {
                        drive = System.IO.Path.GetPathRoot(DestPath);

                        if (
#if DEBUG
                    SimulationParameters?.DestPathRootExists ??
#endif
                    System.IO.Directory.Exists(drive))
                        {

                            if (
#if DEBUG
                    SimulationParameters?.DestDriveReady ??
#endif
                    driveInfo.IsReady)
                            {
                                if (
#if DEBUG
                    (SimulationParameters?.DestDriveTotalFreeSpace ??
#endif
                    driveInfo.TotalFreeSpace
#if DEBUG
                    )
#endif
                    >=
#if DEBUG
                    (SimulationParameters?.PathsSize ??
#endif
                    Paths.Size
#if DEBUG
                    )
#endif
                    .ValueInBytes
                    )
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
                                        destPath =
#if DEBUG
                                            SimulationParameters?.RenameOnDuplicateAction(destPath) ??
#endif
                                            WinCopies.IO.Path.RenameOnDuplicate(destPath);

                                        alreadyRenamed = true;
                                    }

                                    CopyProgressRoutine copyProgressRoutine = (long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint streamNumber, CopyProgressCallbackReason copyProgressCallbackReason, IntPtr sourceFile, IntPtr destinationFile, IntPtr data) =>
                                    {
                                        if (
#if DEBUG
                                        (SimulationParameters?.PathsSize ??
#endif
                                        Paths.Size
#if DEBUG
                                        )
#endif
                                        .ValueInBytes.IsNaN)

                                            copyProgressRoutine = (long _totalFileSize, long _totalBytesTransferred, long _streamSize, long _streamBytesTransferred, uint _streamNumber, CopyProgressCallbackReason _copyProgressCallbackReason, IntPtr _sourceFile, IntPtr _destinationFile, IntPtr _data) =>
                                             {
                                                 ReportProgress((_Paths.Count / InitialItemCount) * 100);

                                                 return CancellationPending ? CopyProgressResult.Cancel : CopyProgressResult.Continue;
                                             };

                                        else

                                            copyProgressRoutine = (long _totalFileSize, long _totalBytesTransferred, long _streamSize, long _streamBytesTransferred, uint _streamNumber, CopyProgressCallbackReason _copyProgressCallbackReason, IntPtr _sourceFile, IntPtr _destinationFile, IntPtr _data) =>
                                        {
                                            Paths.DecrementSize((ulong)_totalBytesTransferred);

                                            ReportProgress((int)(Paths.Size / InitialSize) * 100);

                                            return CancellationPending ? CopyProgressResult.Cancel : CopyProgressResult.Continue;
                                        };

                                        return CopyProgressResult.Quiet;
                                    };

                                    void copyFileOrCreateDirectory()
                                    {
                                        bool cancel = false;
                                        bool result;

                                        if (
#if DEBUG
                                            (SimulationParameters?.PathsSize ??
#endif
                                            path.Size
#if DEBUG
                                            )
#endif
                                            .HasValue)
                                        {

                                            CopyFileFlags copyFileFlags = CopyFileFlags.FailIfExists | CopyFileFlags.NoBuffering;

                                            if (path.Path.EndsWith(".lnk"))

                                                copyFileFlags |= CopyFileFlags.CopySymLink;

                                            result =
#if DEBUG
                                                (SimulationParameters?.CopyFileExAction ??
#endif
                                                Shell.CopyFileEx
#if DEBUG
                                                )
#endif
                                                (sourcePath, destPath, copyProgressRoutine, IntPtr.Zero, ref cancel, copyFileFlags);

                                        }

                                        else

                                            result =
#if DEBUG
                                                SimulationParameters?.CreateDirectoryWAction?.Invoke(destPath) ??
#endif
                                        Directory.CreateDirectoryW(destPath, IntPtr.Zero);

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

                                        if (
#if DEBUG
                                            (SimulationParameters?.DestPathExistsAction ??
#endif
                                            WinCopies.IO.Path.Exists
#if DEBUG
                                            )
#endif
                                            (destPath))

                                            if (_autoRenamePaths && _bufferLength == 0)

                                                renameOnDuplicate();

                                            else if (_bufferLength > 0)
                                            {
                                                try
                                                {
                                                    FileStream sourceFileStream;
#if DEBUG
                                                    if (SimulationParameters == null)
#endif
                                                        sourceFileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None);

#if DEBUG
                                                    else
                                                    {
                                                        Exception exception = SimulationParameters.CreatingFileStreamSucceedsAction(sourcePath, PathDirectoryType.Source);

                                                        if (exception == null)

                                                            sourceFileStream = null;

                                                        else

                                                            throw exception;
                                                    }
#endif

                                                    try
                                                    {
                                                        FileStream destFileStream;

#if DEBUG
                                                        if (SimulationParameters == null)
#endif

                                                            destFileStream = new FileStream(destPath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None);

#if DEBUG

                                                        else
                                                        {
                                                            Exception exception = SimulationParameters.CreatingFileStreamSucceedsAction(destPath, PathDirectoryType.Destination);

                                                            if (exception == null)

                                                                destFileStream = null;

                                                            else

                                                                throw exception;
                                                        }
#endif

                                                        bool? _result;
#if DEBUG
                                                        if (SimulationParameters == null)
#endif

                                                            _result = WinCopies.IO.File.IsDuplicate(sourceFileStream, destFileStream, _bufferLength, () =>
                                                                   PausePending || CancellationPending
                                                                );

#if DEBUG

                                                        else

                                                            _result = SimulationParameters.IsDuplicateAction(sourcePath, destPath, () => PausePending || CancellationPending);

#endif

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

                                                            _ = _Paths.Dequeue();
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

                InitialSize = Paths.Size;

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
