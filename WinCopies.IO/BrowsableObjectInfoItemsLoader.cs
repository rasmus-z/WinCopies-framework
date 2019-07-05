﻿using System;
using System.ComponentModel;

using WinCopies.Util;

using BackgroundWorker = WinCopies.Util.BackgroundWorker;
using System.Collections.Generic;
using System.Threading;

namespace WinCopies.IO
{

    /// <summary>
    /// The base class for the <see cref="IBrowsableObjectInfo"/> items loaders.
    /// </summary>
    public abstract class BrowsableObjectInfoItemsLoader : IBackgroundWorker, IDisposable

    {

        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        private readonly BrowsableObjectInfo _path = null;

        //public void changePath(IBrowsableObjectInfo newValue)

        //{



        //} 

        /// <summary>
        /// Gets the path from which load items.
        /// </summary>
        public BrowsableObjectInfo Path
        {
            get => _path; internal set
            {

                // We try to set the property and we throw an exception if backgroundWorker is busy.

                _ = this.SetBackgroundWorkerProperty(nameof(Path), nameof(_path), value, typeof(BrowsableObjectInfoItemsLoader), true);

                // We reach this point only if the test above succeeded.

                InitializePath();

            }
        }

        private readonly IEnumerable<string> _filter = null;

        public IEnumerable<string> Filter
        {

            get => _filter;

            set => this.SetBackgroundWorkerProperty(nameof(Filter), nameof(_filter), value, typeof(BrowsableObjectInfoItemsLoader), true);

        }

        /// <summary>
        /// Gets a value that indicates whether the thread is busy.
        /// </summary>
        public bool IsBusy => backgroundWorker.IsBusy;

        /// <summary>
        /// Gets or sets a value that indicates whether the thread can notify of the progress.
        /// </summary>
        public bool WorkerReportsProgress { get => backgroundWorker.WorkerReportsProgress; set => backgroundWorker.WorkerReportsProgress = value; }

        /// <summary>
        /// Gets or sets a value that indicates whether the thread supports cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation { get => backgroundWorker.WorkerSupportsCancellation; set => backgroundWorker.WorkerSupportsCancellation = value; }

        public ApartmentState ApartmentState { get => backgroundWorker.ApartmentState; set => backgroundWorker.ApartmentState = value; }

        public bool CancellationPending => throw new NotImplementedException();

        public bool IsCancelled => throw new NotImplementedException();

        public int Progress => throw new NotImplementedException();

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// <para>This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        public event ProgressChangedEventHandler ProgressChanged;

        public event RunWorkerCompletedEventHandler RunWorkerCompleted;
        public event EventHandler Disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        public BrowsableObjectInfoItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation)
        {

            WorkerReportsProgress = workerReportsProgress;

            WorkerSupportsCancellation = workerSupportsCancellation;

            ProgressChanged += (object sender, ProgressChangedEventArgs e) => OnProgressChanged(e);

            backgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged(this, e);

            DoWork += (object sender, DoWorkEventArgs e) => OnDoWork(e);

            backgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => DoWork(this, e);

            backgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {

                _path.AreItemsLoaded = true;

                RunWorkerCompleted?.Invoke(this, e);

            };

        }

        public abstract bool CheckFilter(string path);

        public void ReportProgress(int percentProgress) => backgroundWorker.ReportProgress(percentProgress);

        public void ReportProgress(int percentProgress, object userState) => backgroundWorker.ReportProgress(percentProgress, userState);

        protected abstract void InitializePath();

        public static FileTypes FileTypeToFileTypeFlags(FileType fileType)

        {

            fileType.ThrowIfNotValidEnumValue();

            if (fileType == IO.FileType.SpecialFolder) throw new ArgumentException("'" + nameof(fileType) + "' must be None, Folder, File, Drive, Link or Archive. '" + nameof(fileType) + "' is " + fileType.ToString() + ".");

            switch (fileType)

            {

                case IO.FileType.None:

                    return FileTypes.None;

                case IO.FileType.Folder:

                    return FileTypes.Folder;

                case IO.FileType.File:

                    return FileTypes.File;

                case IO.FileType.Drive:

                    return FileTypes.Drive;

                case IO.FileType.Link:

                    return FileTypes.Link;

                case IO.FileType.Archive:

                    return FileTypes.Archive;

                default:

                    // This code should never be reached.

                    throw new NotImplementedException();

            }

        }

        protected abstract void OnDoWork();

        /// <summary>
        /// When overridden in a derived class, provides a handler for the <see cref="DoWork"/> event.
        /// </summary>
        /// <param name="e">Event args for the current event</param>
        protected virtual void OnDoWork(DoWorkEventArgs e) => OnDoWork();

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e) => Path.items.Add((IBrowsableObjectInfo)e.UserState);

        // /// <summary>
        // /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class with an <see cref="IBrowsableObjectInfo"/>.
        // /// </summary>
        // /// <param name="path">The path from which load items.</param>
        // public BrowsableObjectInfoItemsLoader(IBrowsableObjectInfo path) { path.ItemsLoader = this; }

        private void InitLoad()

        {

            if (_path == null) throw new NullReferenceException("'Path' is null.");

            if (!_path.IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, _path.FileType.ToString(), _path.ToString()));

            else if (backgroundWorker.IsBusy)

                backgroundWorker.Cancel();

            Path.items.Clear();

        }

        /// <summary>
        /// Loads the items of the <see cref="Path"/> object.
        /// </summary>
        public void LoadItems()

        {

            InitLoad();

            OnDoWork();

        }

        public void LoadItemsAsync()

        {

            InitLoad();

            backgroundWorker.RunWorkerAsync();

        }

        public void CancelAsync() => backgroundWorker.CancelAsync();

        public void Cancel() => backgroundWorker.Cancel();

        public virtual void Dispose() => backgroundWorker.Dispose();
        public void Suspend() => throw new NotImplementedException();
        public void Resume() => throw new NotImplementedException();
    }

}
