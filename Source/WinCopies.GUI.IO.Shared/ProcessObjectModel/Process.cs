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

using System;
using System.ComponentModel;
using System.IO;

using WinCopies.Collections.DotNetFix;
using WinCopies.Util.Data;
using WinCopies.Util.DotNetFix;

using static WinCopies.Util.Util;

using Size = WinCopies.IO.Size;

namespace WinCopies.GUI.IO.Process
{
    public abstract class Process<T> : ViewModelBase where T : WinCopies.IO.IPathInfo
    {
        public static string GetSourcePathFromPathCollection(in PathCollection<T> paths) => (paths ?? throw GetArgumentNullException(nameof(paths))).Path;

        /// <summary>
        /// Gets the inner background worker.
        /// </summary>
        protected PausableBackgroundWorker BackgroundWorker { get; } = new PausableBackgroundWorker();

        private Size _initialSize;

        /// <summary>
        /// Gets or sets (protected) the initial total item size.
        /// </summary>
        public Size InitialItemSize
        {
            get => _initialSize; protected set
            {
                if (value != _initialSize)
                {
                    _initialSize = value;

                    OnPropertyChanged(nameof(InitialItemSize));
                }
            }
        }

        private int _initialItemCount;

        /// <summary>
        /// Gets or sets (protected) the initial total item count.
        /// </summary>
        public int InitialItemCount
        {
            get => _initialItemCount; protected set
            {
                if (value != _initialItemCount)
                {
                    _initialItemCount = value;

                    OnPropertyChanged(nameof(InitialItemCount));
                }
            }
        }

        protected ObservableQueueCollection<IPathInfo> _Paths { get; } = new ObservableQueueCollection<IPathInfo>();

        /// <summary>
        /// Gets the paths that have been loaded.
        /// </summary>
        public ProcessQueueCollection Paths { get; }

        private bool _completed = false;

        /// <summary>
        /// Gets a value that indicates whether the process has completed.
        /// </summary>
        public bool IsCompleted
        {
            get => _completed; protected set
            {
                if (value != _completed)
                {
                    _completed = value;

                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        /// <summary>
        /// Gets the source root path.
        /// </summary>
        public string SourcePath { get; }

        private bool _pathsLoaded = false;

        /// <summary>
        /// Gets a value that indicates whether all the paths and subpaths are loaded.
        /// </summary>
        public bool ArePathsLoaded
        {
            get => _pathsLoaded; protected set
            {
                if (value != _pathsLoaded)
                {
                    _pathsLoaded = value;

                    OnPropertyChanged(nameof(ArePathsLoaded));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the process supports cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation
        {
            get => BackgroundWorker.WorkerSupportsCancellation; set
            {
                if (IsBusy)

                    throw new InvalidOperationException("The BackgroundWorker is busy.");

                if (value != BackgroundWorker.WorkerSupportsCancellation)
                {
                    BackgroundWorker.WorkerSupportsCancellation = value;

                    OnPropertyChanged(nameof(WorkerSupportsCancellation));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the process reports progress.
        /// </summary>
        public bool WorkerReportsProgress
        {
            get => BackgroundWorker.WorkerReportsProgress; set
            {
                if (IsBusy)

                    throw new InvalidOperationException("The BackgroundWorker is busy.");

                if (value != BackgroundWorker.WorkerReportsProgress)
                {
                    BackgroundWorker.WorkerReportsProgress = value;

                    OnPropertyChanged(nameof(WorkerReportsProgress));
                }
            }
        }

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
        public bool WorkerSupportsPausing
        {
            get => BackgroundWorker.WorkerSupportsPausing; set
            {
                BackgroundWorker.WorkerSupportsPausing = value;

                if (value != BackgroundWorker.WorkerSupportsPausing) // We make this test after trying to update the inner BackgroundWorker property because this property checks if the BackgroundWorker is busy before updating the underlying value. Because this check has to be performed even if the new value is the same as the old one, in order to let the user know even in this case if there is a bug, and because this check is performed in the inner BackgroundWorker property, to make the check of this line here makes possible to let the user know if there is a bug in all cases, without performing the is-busy check twice.

                    OnPropertyChanged(nameof(WorkerSupportsPausing));
            }
        }

        /// <summary>
        /// Gets a value that indicates whether a pause is pending.
        /// </summary>
        public bool PausePending => BackgroundWorker.PausePending;

        private ObservableQueueCollection<IErrorPathInfo> _ErrorPaths { get; } = new ObservableQueueCollection<IErrorPathInfo>();

        public ReadOnlyObservableQueueCollection<IErrorPathInfo> ErrorPaths { get; }

        /// <summary>
        /// Gets the global process error, if any.
        /// </summary>
        public ProcessError Error { get; private set; }

        private IPathInfo _currentPath;

        /// <summary>
        /// Gets the current processed <see cref="IPathInfo"/>.
        /// </summary>
        public IPathInfo CurrentPath
        {
            get => _currentPath; protected set
            {
                if (value != _currentPath)
                {
                    _currentPath = value;

                    OnPropertyChanged(nameof(CurrentPath));
                }
            }
        }

        private int _progressPercentage = 0;

        /// <summary>
        /// Gets the progress percentage of the current process.
        /// </summary>
        public int ProgressPercentage
        {
            get => _progressPercentage;

            private set
            {
                if (value != _progressPercentage)
                {
                    _progressPercentage = value;

                    OnPropertyChanged(nameof(ProgressPercentage));
                }
            }
        }

        protected PathCollection<T> PathCollection { get; }

        public event ProgressChangedEventHandler ProgressChanged;

        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="Process"/> class.
        ///// </summary>
        protected Process(in PathCollection<T> paths)
        {
            SourcePath = GetSourcePathFromPathCollection(paths);

            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

            BackgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProgressChanged(e);

            ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProcessProgressChanged(e);

            BackgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerCompleted(e);

            RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerProcessCompleted(e);

            Paths = new ProcessQueueCollection(_Paths);

            PathCollection = paths;

            ErrorPaths = new ReadOnlyObservableQueueCollection<IErrorPathInfo>(_ErrorPaths);
        }

        protected void ThrowIfCompleted()
        {
            if (IsCompleted)

                throw new InvalidOperationException("The process has already been completed.");
        }

        protected virtual bool CheckIfEnoughSpace()
        {
            if (Paths.Size.ValueInBytes.IsNaN)
            {
                Error = ProcessError.NotEnoughSpace;

                return false;
            }

            return true;
        }

        protected virtual bool CheckIfDriveIsReady(
#if DEBUG
            in ProcessSimulationParameters simulationParameters
#endif
            )
        {
            string drive = System.IO.Path.GetPathRoot(SourcePath);

            if (
#if DEBUG
                    simulationParameters?.SourcePathRootExists ??
#endif
                    System.IO.Directory.Exists(drive))
            {
                var driveInfo = new DriveInfo(drive);

                if (
#if DEBUG
                    simulationParameters?.SourceDriveReady ??
#endif
                    driveInfo.IsReady) return true;
            }

            Error = ProcessError.DriveNotReady;

            return false;
        }

        protected virtual ProcessError OnPausePending() => ProcessError.AbortedByUser;

        protected virtual ProcessError OnCancellationPending()
        {
            _Paths.Clear();

            IsCompleted = true;

            return ProcessError.AbortedByUser;
        }

        public bool CheckIfPauseOrCancellationPending()
        {
            if (PausePending)

                Error = OnPausePending();

            if (CancellationPending)

                Error = OnCancellationPending();

            return Error != ProcessError.None;
        }

        protected virtual void OnDoWork(DoWorkEventArgs e)
        {
            OnPropertyChanged(nameof(IsBusy));

            LoadPaths(e);

            OnProcessDoWork(e);
        }

        private void LoadPaths(DoWorkEventArgs e)
        {
            if ((Error = OnLoadPaths(e)) == ProcessError.None)
            {
                InitialItemSize = Paths.Size;

                InitialItemCount = _Paths.Count;

                ArePathsLoaded = true;

                _DoWork(e);
            }
        }

        protected abstract ProcessError OnLoadPaths(DoWorkEventArgs e);

        private void _DoWork(DoWorkEventArgs e)
        {
            if ((Error = OnProcessDoWork(e)) == ProcessError.None)

                IsCompleted = true;
        }

        protected void DequeueErrorPath(ProcessError error)
        {
            _ErrorPaths.Enqueue(new ErrorPathInfo(CurrentPath, error));

            _ = _Paths.Dequeue();
        }

        protected abstract ProcessError OnProcessDoWork(DoWorkEventArgs e);

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e) => ProgressChanged?.Invoke(this, e);

        protected virtual void OnProcessProgressChanged(ProgressChangedEventArgs e) => ProgressPercentage = (e ?? throw GetArgumentNullException(nameof(e))).ProgressPercentage;

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

        public void ReportProgress(int progressPercentage) => BackgroundWorker.ReportProgress(progressPercentage);

        public void ReportProgress(int progressPercentage, object userState) => BackgroundWorker.ReportProgress(progressPercentage, userState);

        //protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        //protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
