using System;
using System.ComponentModel;

using WinCopies.Util;

using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

using BackgroundWorker = WinCopies.Util.BackgroundWorker;
using System.Diagnostics;
using System.Collections.Generic;

namespace WinCopies.IO
{

    /// <summary>
    /// The base class for the <see cref="IBrowsableObjectInfo"/> items loaders.
    /// </summary>
    public abstract class BrowsableObjectInfoItemsLoader : INotifyPropertyChanged, IDisposable

    {

        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {

            (bool propertyChanged, object oldValue) = (this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        private BrowsableObjectInfo _path = null;

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

                if (backgroundWorker.IsBusy)

                    throw new InvalidOperationException("The BackgroundWorker is busy.");

                OnPropertyChanged(nameof(Path), nameof(_path), value, typeof(BrowsableObjectInfoItemsLoader));
            }
        }

        private readonly FileTypesFlags _fileTypes = FileTypesFlags.All;

        public FileTypesFlags FileTypes

        {

            get => _fileTypes;

            set

            {

                if (backgroundWorker.IsBusy)

                    throw new InvalidOperationException("The BackgroundWorker is busy.");

                OnPropertyChanged(nameof(FileTypes), nameof(_fileTypes), value, typeof(BrowsableObjectInfoItemsLoader));

            }

        }

        private IEnumerable<string> _filter = null;

        public IEnumerable<string> Filter
        {

            get => _filter;

            set
            {

                if (IsBusy)

                    throw new InvalidOperationException("The " + nameof(LoadFolder) + " is busy.");

                _filter = value;

            }

        }

        /// <summary>
        /// Gets a value that indicates if the thread is busy.
        /// </summary>
        public bool IsBusy => backgroundWorker.IsBusy;

        /// <summary>
        /// Gets or sets a value that indicates if the thread can notify of the progress.
        /// </summary>
        public bool WorkerReportsProgress { get => backgroundWorker.WorkerReportsProgress; set { backgroundWorker.WorkerReportsProgress = value; OnPropertyChanged(nameof(WorkerReportsProgress), !backgroundWorker.WorkerReportsProgress, backgroundWorker.WorkerReportsProgress); } }

        public bool WorkerSupportsCancellation { get => backgroundWorker.WorkerSupportsCancellation; set { backgroundWorker.WorkerSupportsCancellation = value; OnPropertyChanged(nameof(WorkerSupportsCancellation), !backgroundWorker.WorkerSupportsCancellation, backgroundWorker.WorkerSupportsCancellation); } }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// <para>This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        public event ProgressChangedEventHandler ProgressChanged;

        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        public BrowsableObjectInfoItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes)
        {

            WorkerReportsProgress = workerReportsProgress;

            WorkerSupportsCancellation = workerSupportsCancellation;

            FileTypes = fileTypes;

            PropertyChanged += BrowsableObjectInfoItemsLoader_PropertyChanged;

            ProgressChanged += OnProgressChanged;

            backgroundWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged(this, e);

            DoWork += OnDoWork;

            backgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => DoWork(this, e);

            backgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {

                _path.AreItemsLoaded = true;

                RunWorkerCompleted?.Invoke(this, e);

            };

        }

        public void ReportProgress(int percentProgress) => backgroundWorker.ReportProgress(percentProgress);

        public void ReportProgress(int percentProgress, object userState) => backgroundWorker.ReportProgress(percentProgress, userState);

        private void BrowsableObjectInfoItemsLoader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(Path))

                Init();

        }

        protected abstract void Init();

        public bool CheckFilter(string directory)

        {

            if (Filter == null) return true;

            foreach (string filter in Filter)

                if (!IO.Path.MatchToFilter(directory, filter)) return false;

            return true;

        }

        public static FileTypesFlags FileTypeToFileTypeFlags(FileType fileType)

        {

            fileType.ThrowIfNotValidEnumValue();

            if (fileType == IO.FileType.SpecialFolder) throw new ArgumentException("'" + nameof(fileType) + "' must be None, Folder, File, Drive, Link or Archive. '" + nameof(fileType) + "' is " + fileType.ToString() + ".");

            switch (fileType)

            {

                case IO.FileType.None:

                    return FileTypesFlags.None;

                case IO.FileType.Folder:

                    return FileTypesFlags.Folder;

                case IO.FileType.File:

                    return FileTypesFlags.File;

                case IO.FileType.Drive:

                    return FileTypesFlags.Drive;

                case IO.FileType.Link:

                    return FileTypesFlags.Link;

                case IO.FileType.Archive:

                    return FileTypesFlags.Archive;

                default:

                    // This code should never be reached.

                    throw new NotImplementedException();

            }

        }

        /// <summary>
        /// When overriden in a derived class, provides a handler for the <see cref="DoWork"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDoWork(object sender, DoWorkEventArgs e)

        {

            if (FileTypes == FileTypesFlags.None) return;

            else if (FileTypes.HasFlag(FileTypesFlags.All) && FileTypes.HasMultipleFlags())

                throw new InvalidOperationException("FileTypes cannot have the All flag in combination with other flags.");

#if DEBUG

            Debug.WriteLine("Dowork event started.");

            Debug.WriteLine(FileTypes);

            try
            {

                Debug.WriteLine("Path == null: " + (Path == null).ToString());

                Debug.WriteLine("Path.Path: " + Path?.Path);

                Debug.WriteLine("Path.ShellObject: " + (Path as ShellObjectInfo)?.ShellObject.ToString());

            }
            catch (Exception) { }

#endif

        }

        protected virtual void OnProgressChanged(object sender, ProgressChangedEventArgs e) => Path.items.Add((IBrowsableObjectInfo)e.UserState);

        // /// <summary>
        // /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class with an <see cref="IBrowsableObjectInfo"/>.
        // /// </summary>
        // /// <param name="path">The path from which load items.</param>
        // public BrowsableObjectInfoItemsLoader(IBrowsableObjectInfo path) { path.ItemsLoader = this; }

        /// <summary>
        /// Loads the items of the <see cref="Path"/> object.
        /// </summary>
        public void LoadItems()

        {

            if (_path == null) throw new NullReferenceException("'Path' is null.");

            if (!_path.IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, _path.FileType.ToString(), _path.ToString()));

            else if (backgroundWorker.IsBusy)

                backgroundWorker.Cancel();

            Path.items.Clear();

            backgroundWorker.RunWorkerAsync();

        }

        public void CancelAsync() => backgroundWorker.CancelAsync();

        public void Cancel() => backgroundWorker.Cancel();

        public virtual void Dispose() => backgroundWorker.Dispose();

    }

}
