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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using WinCopies.Collections.DotNetFix;
using WinCopies.IO;
using WinCopies.Util;
using static WinCopies.Util.Util;
using Size = WinCopies.IO.Size;

namespace WinCopies.GUI.IO
{

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

    public abstract class Process : INotifyPropertyChanged
    {

        /// <summary>
        /// Gets the inner background worker.
        /// </summary>
        protected PausableBackgroundWorker BackgroundWorker { get; } = new PausableBackgroundWorker();

        /// <summary>
        /// Gets or sets (protected) the initial total item size.
        /// </summary>
        public Size InitialSize { get; protected set; }

        /// <summary>
        /// Gets or sets (protected) the initial total item count.
        /// </summary>
        public int InitialItemCount { get; protected set; }

        protected ObservableQueueCollection<IPathInfo> _Paths { get; } = new ObservableQueueCollection<IPathInfo>();

        /// <summary>
        /// Gets the paths that have been loaded.
        /// </summary>
        public ProcessQueueCollection Paths { get; }

        private bool _completed = false;

        /// <summary>
        /// Gets a value that indicates whether the process has completed.
        /// </summary>
        public bool Completed { get => _completed; protected set { _completed = value; OnPropertyChanged(nameof(Completed)); } }

        /// <summary>
        /// Gets the source root path.
        /// </summary>
        public string SourcePath { get; }

        private bool _pathsLoaded = false;

        /// <summary>
        /// Gets a value that indicates whether all the paths and subpaths are loaded.
        /// </summary>
        public bool ArePathsLoaded { get => _pathsLoaded; protected set { _pathsLoaded = value; OnPropertyChanged(nameof(ArePathsLoaded)); } }

        /// <summary>
        /// Gets or sets a value that indicates whether the process supports cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation { get => BackgroundWorker.WorkerSupportsCancellation; set { BackgroundWorker.WorkerSupportsCancellation = value; OnPropertyChanged(nameof(WorkerSupportsCancellation)); } }

        /// <summary>
        /// Gets or sets a value that indicates whether the process reports progress.
        /// </summary>
        public bool WorkerReportsProgress { get => BackgroundWorker.WorkerReportsProgress; set { BackgroundWorker.WorkerReportsProgress = value; OnPropertyChanged(nameof(WorkerReportsProgress)); } }

        /// <summary>
        /// Gets a value that indicates whether the process is busy.
        /// </summary>
        public bool IsBusy => BackgroundWorker.IsBusy;

        /// <summary>
        /// Gets a value that indicates whether a cancellation is pending.
        /// </summary>
        public bool CancellationPending => BackgroundWorker.CancellationPending;

        /// <summary>
        /// Gets or sets a value that indicates whether the process supports pausing.
        /// </summary>
        public bool WorkerSupportsPausing { get => BackgroundWorker.WorkerSupportsPausing; set => BackgroundWorker.WorkerSupportsPausing = value; }

        /// <summary>
        /// Gets a value that indicates whether a pause is pending.
        /// </summary>
        public bool PausePending => BackgroundWorker.PausePending;

        protected ObservableQueueCollection<IErrorPathInfo> _ErrorPaths { get; } = new ObservableQueueCollection<IErrorPathInfo>();

        public ReadOnlyObservableQueueCollection<IErrorPathInfo> ErrorPaths { get; }

        /// <summary>
        /// Gets the global process error, if any.
        /// </summary>
        public ProcessError Error { get; protected set; }

        /// <summary>
        /// Gets the current processed <see cref="IPathInfo"/>.
        /// </summary>
        public IPathInfo CurrentPath { get; protected set; }

        public event DoWorkEventHandler DoWork;

        public event ProgressChangedEventHandler ProgressChanged;

        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Process"/> class.
        /// </summary>
        /// <param name="sourcePath">The source root path of the process.</param>
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

    public class CopyProcess : Process
    {
        private IEnumerator<WinCopies.IO.IPathInfo> _pathsToLoadEnumerator;

#if DEBUG

        public CopyProcessSimulationParameters SimulationParameters { get; }

#endif

        /// <summary>
        /// Gets the destination root path.
        /// </summary>
        public string DestPath { get; }

        private bool _autoRenameFiles;

        /// <summary>
        /// Gets a value that indicates whether files are automatically renamed when they conflict with existing paths.
        /// </summary>
        public bool AutoRenameFiles { get => _autoRenameFiles; set => _autoRenameFiles = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value; }

        private int _bufferLength;

        public int BufferLength { get => _bufferLength; set => _bufferLength = BackgroundWorker.IsBusy ? throw new InvalidOperationException("The BackgroundWorker is busy.") : value < 0 ? throw new ArgumentOutOfRangeException($"{nameof(value)} cannot be less than zero.") : value; }

        public CopyProcess(PathCollection pathsToLoad, string destPath
#if DEBUG
            , CopyProcessSimulationParameters simulationParameters
#endif
            ) : base((pathsToLoad ?? throw GetArgumentNullException(nameof(pathsToLoad))).Path)
        {
#if DEBUG
            SimulationParameters = simulationParameters;
#endif

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
                    >= Paths.Size.ValueInBytes)
                                {
                                    void dequeueErrorPath(ProcessError _error)
                                    {

                                        _ErrorPaths.Enqueue(new ErrorPathInfo(CurrentPath, _error));

                                        _ = _Paths.Dequeue();

                                    }

                                    string sourcePath;
                                    string destPath;
                                    bool alreadyRenamed;

                                    void renameOnDuplicate()
                                    {
                                        destPath =
#if DEBUG
                                            SimulationParameters?.RenameOnDuplicateAction(destPath) ??
#endif
                                            WinCopies.IO.Path.RenameDuplicate(destPath);

                                        alreadyRenamed = true;
                                    }

                                    CopyProgressRoutine copyProgressRoutine = (long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, uint streamNumber, CopyProgressCallbackReason copyProgressCallbackReason, IntPtr sourceFile, IntPtr destinationFile, IntPtr data) =>
                                    {
                                        if (Paths.Size.ValueInBytes.IsNaN)

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

                                        if (CurrentPath.Size.HasValue)
                                        {

                                            CopyFileFlags copyFileFlags = CopyFileFlags.FailIfExists | CopyFileFlags.NoBuffering;

                                            if (CurrentPath.Path.EndsWith(".lnk", true, CultureInfo.InvariantCulture))

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
                                            // todo: the current version of this process is not optimized: when a file name conflict occurs when we want to create a folder, we know that all the subpaths won't be able to be copied neither. So, we should have a tree structure, so we can dequeue all the path in conflict with all of its subpaths at one time.
                                            case ErrorCode.AlreadyExists:

                                                if (CurrentPath.Size.HasValue) // We do not try to rename folders, because folder name conflicts are handled the same way as file name conflicts.
                                                {
                                                    if (alreadyRenamed)
                                                    {
                                                        dequeueErrorPath(ProcessError.FileRenamingFailed);

                                                        return;
                                                    }

                                                    if (_autoRenameFiles)
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

                                        alreadyRenamed = false;

                                        CurrentPath = _Paths.Peek();

                                        sourcePath = $"{SourcePath}{WinCopies.IO.Path.PathSeparator}{CurrentPath.Path}";

                                        destPath = $"{DestPath}{WinCopies.IO.Path.PathSeparator}{CurrentPath.Path}";

                                        if (

#if DEBUG
                                        (SimulationParameters == null &&
#endif
                                            WinCopies.IO.Path.Exists(destPath)
#if DEBUG
                                            ) || SimulationParameters.DestPathExistsAction(destPath)
#endif
                                            )

                                            if (_autoRenameFiles)

                                                if (_bufferLength == 0)

                                                    renameOnDuplicate();

                                                else
                                                {
#if DEBUG
                                                    FileStream sourceFileStream = null;
#endif
                                                    try
                                                    {
#if DEBUG
                                                        if (SimulationParameters == null)
#else
                                                    using
#if CS7
                                                        (
#endif
                                                        var
#endif
                                                            sourceFileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None)
#if CS7 && !DEBUG
                                                        )
                                                    {
#else
                                                        ;
#endif

#if DEBUG
                                                        else
                                                        {
                                                            Exception exception = SimulationParameters.CreatingFileStreamSucceedsAction(sourcePath, PathDirectoryType.Source);

                                                            if (exception == null)

                                                                sourceFileStream = null;

                                                            else

                                                                throw exception;
                                                        }

                                                        FileStream destFileStream = null;
#endif

                                                        try
                                                        {
#if DEBUG
                                                            if (SimulationParameters == null)
#else
                                                        using
#if CS7
                                                            (
#endif
                                                            var
#endif
                                                                destFileStream = new FileStream(destPath, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferLength, FileOptions.None)
#if CS7 && !DEBUG
                                                            )
                                                        {
#else
                                                            ;
#endif

#if DEBUG

                                                            else
                                                            {
                                                                Exception exception = SimulationParameters.CreatingFileStreamSucceedsAction(destPath, PathDirectoryType.Destination);

                                                                if (exception != null)

                                                                    throw exception;
                                                            }
#endif

                                                            bool? _result;
#if DEBUG
                                                            if (SimulationParameters == null)
#endif

                                                                _result = WinCopies.IO.File.IsDuplicate(sourceFileStream, destFileStream, _bufferLength, () => PausePending || CancellationPending);

#if DEBUG

                                                            else

                                                                _result = SimulationParameters.IsDuplicateAction(sourcePath, destPath, () => PausePending || CancellationPending);

#endif

                                                            if (checkIfPauseOrCancellationPending())

                                                                return;

                                                            if (_result.HasValue && _result.Value)
                                                            {
                                                                if (checkIfPauseOrCancellationPending())

                                                                    return;

                                                                renameOnDuplicate();
                                                            }

                                                            else

                                                                _ = _Paths.Dequeue();
#if CS7 && !DEBUG
                                                        }
#endif
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
#if DEBUG
                                                        finally
                                                        {
                                                            destFileStream?.Dispose();
                                                        }
#endif
                                                    }

                                                    catch (System.IO.IOException ex) when (ex.Is(false, typeof(System.IO.FileNotFoundException), typeof(System.IO.DirectoryNotFoundException)))
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
#if DEBUG
                                                    finally
                                                    {
                                                        sourceFileStream.Dispose();
                                                    }
#endif
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

            void loadPathsAndCopy(IEnumerator<WinCopies.IO.IPathInfo> pathsToLoad)
            {
                bool _continue = true;

                while (_continue)

                    try
                    {
                        if (checkIfPauseOrCancellationPending())

                            return;

                        _continue = pathsToLoad.MoveNext();

                        _Paths.Enqueue(new PathInfo(pathsToLoad.Current.Path, pathsToLoad.Current.IsDirectory ? (Size?)null : (Size)new FileInfo(pathsToLoad.Current.Path).Length)); // todo: use Windows API Code Pack's Shell's implementation instead.
                    }

                    catch (Exception) { }

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
                IEnumerator<WinCopies.IO.IPathInfo> _pathsToLoad = WinCopies.IO.Directory.Enumerate((PathCollection)e.Argument
#if DEBUG
                    , SimulationParameters?.FileSystemEntryEnumeratorProcessSimulation
#endif
                    ).GetEnumerator();

                if (WorkerSupportsPausing)

                    _pathsToLoadEnumerator = _pathsToLoad;

                loadPathsAndCopy(_pathsToLoad);
            }
        }
    }
}
