﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using WinCopies.Util;
using BackgroundWorker = WinCopies.Util.BackgroundWorker;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace WinCopies.IO.FileProcesses
{

    //TODO : 'abstract' utile ?

    //TODO : revoir les commentaires xml pour cette classe

    /// <summary>
    /// Provides the base class for all processes manager in the WinCopies framework. This is an abstract class.
    /// </summary>
    public abstract class Process : IBackgroundWorker, INotifyPropertyChanged
    {

        protected void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType)

        {

            var (propertyChanged, oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected void OnPropertyChangedWhenNotBusy(string propertyName, string fieldName, object newValue, Type declaringType, bool throwIfBusy)

        {

            var (propertyChanged, oldValue) = Extensions.SetPropertyWhenNotBusy(this, propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // public string SourcePath { get => FilesInfoLoader.SourcePath; } 

        // public Size TotalSize => _filesInfoLoader.totalSize;  

        // private ActionType _actionType = ActionType.Unknown; 

        #region BackgroundWorker implementation

        protected readonly BackgroundWorker _bgWorker = new BackgroundWorker();



        /// <summary>
        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
        /// </summary>
        public ApartmentState ApartmentState { get => _bgWorker.ApartmentState; }

        /// <summary>
        /// Gets a value that indicates if the thread must try to cancel before finished the background tasks.
        /// </summary>
        public bool CancellationPending { get => _bgWorker.CancellationPending; }

        /// <summary>
        /// Gets a value that indicates if the thread is busy.
        /// </summary>
        public bool IsBusy { get => _bgWorker.IsBusy; }

        /// <summary>
        /// Gets a value that indicates if the working is cancelled.
        /// </summary>
        public bool IsCancelled { get => _bgWorker.IsCancelled; }

        /// <summary>
        /// Gets the current progress of the working in percent.
        /// </summary>
        public int Progress { get => _bgWorker.Progress; }

        public ISite Site { get => _bgWorker.Site; set => _bgWorker.Site = value; }

        /// <summary>
        /// Gets or sets a value that indicates if the thread can notify of the progress.
        /// </summary>
        public bool WorkerReportsProgress
        {
            get => _bgWorker.WorkerReportsProgress; set

            {

                if (value != _bgWorker.WorkerReportsProgress)

                {

                    object previousValue = _bgWorker.WorkerReportsProgress;

                    _bgWorker.WorkerReportsProgress = value;

                    OnPropertyChanged(nameof(WorkerReportsProgress), previousValue, value);

                }

            }

        }

        /// <summary>
        /// Gets or sets a value that indicates if the thread supports the cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation
        {

            get => _bgWorker.WorkerSupportsCancellation; set

            {

                if (value != _bgWorker.WorkerSupportsCancellation)

                {

                    object previousValue = _bgWorker.WorkerSupportsCancellation;

                    _bgWorker.WorkerSupportsCancellation = value;

                    OnPropertyChanged(nameof(WorkerSupportsCancellation), previousValue, value);

                }

            }

        }

        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        public void CancelAsync() => _bgWorker.CancelAsync();

        /// <summary>
        /// Cancels the working.
        /// </summary>
        public void Cancel() => _bgWorker.Cancel();

        public void Dispose() => _bgWorker.Dispose();

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        public void ReportProgress(int percentProgress) => _bgWorker.ReportProgress(percentProgress);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        public void ReportProgress(int percentProgress, object userState) => _bgWorker.ReportProgress(percentProgress, userState);

        protected void Pause()

        {

            PausePending = false;

            IsPaused = true;

            _bgWorker.Suspend();

        }

        public void Resume()

        {

            PausePending = false;

            IsPaused = false;

            if (_bgWorker.IsBusy)

                _bgWorker.Resume();

            else

                _bgWorker.RunWorkerAsync();

        }

        /// <summary>
        /// <para>This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        /// <summary>
        /// <para>This event is called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// <para>This event is called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;

        public event EventHandler Disposed;

        #endregion    

        /// <summary>
        /// Determines the type of this process.
        /// </summary>
        public abstract ActionType ActionType { get; }    // { get => _actionType; private    set => OnPropertyChanged(nameof(ActionType), nameof(_actionType), value);    } 

        #region Exceptions manager



        //TODO :



        // /// <summary>
        // /// Liste de toutes les exceptions s'étant déroulées dans le processus.
        // /// </summary>
        // public List<int> List_Of_Total_Exceptions { get; set; }



        // TODO : tout en public ? -- List<int> ?



        //        private Controls.ReadOnlyObservableCollection<int> _paths_With_Same_Name___Paths_Info_Read_Only = null;

        ///// <summary>
        ///// Liste des répertoires avec le même nom.
        ///// </summary>
        //public ReadOnlyObservableCollection<int> Paths_With_Same_Name___Paths_Info
        //{

        //    get
        //    {

        //        return _paths_With_Same_Name___Paths_Info_Read_Only.ReadOnlyObservableCollection;

        //    }

        //}



        // /// <summary>
        // /// Les exceptions pour lesquelles il faut relancer la copie. Certaines exceptions peuvent être évitées en définnissant des valeurs adéquates dans chaque instance de WinCopies.IO.PathInfo liée à un fichier présentant une exception.
        // /// </summary>
        // public Types_Of_Exceptions_Enum Types_Of_Exceptions_To_Retry { get; set; }

        // private Types_Of_Exceptions_Enum Types_Of_Exceptions_Occurred_Property { get; set; }

        #endregion

        // TODO : ?

        // private string _Volume_Label = string.Empty;

        // public string Volume_Label { get; }

        private int _startItemIndex = 0;

        // /// <summary>
        // /// Endroit à partir duquel la copie ou le déplacement doivent  (re) commencer dans la liste des répertoires à copier ou déplacer.
        // /// </summary>

        public int StartItemIndex { get => _startItemIndex; set => OnPropertyChanged(nameof(StartItemIndex), nameof(_startItemIndex), value, typeof(Process)); }

        // private LoadFilesInfo _filesInfoLoader = null;

        // TODO : utile en tant que champ/propriété ? meilleur nom ? 

        private LoadFilesInfo _loadFilesInfo = null;

        /// <summary>
        /// Gets the loader of the files and folders for this process.
        /// </summary>
        public LoadFilesInfo FilesInfoLoader
        {
            get => _loadFilesInfo; set
            {

                // FilesInfoLoader.RunWorkerCompleted += loadFilesInfo_RunWorkerCompleted;

                // FilesInfoLoader.PathsInfo = pathsInfo;

                OnPropertyChanged(nameof(FilesInfoLoader), nameof(_loadFilesInfo), value, typeof(Process));

                value.ActionType = ActionType.Copy;
            }
        }

        private bool _exceptionsOccurred = false;

        public bool ExceptionsOccurred { get => _exceptionsOccurred; protected set => OnPropertyChanged(nameof(ExceptionsOccurred), nameof(_exceptionsOccurred), value, typeof(Process)); }

        private Exceptions _exceptionsToRetry = Exceptions.None;

        public Exceptions ExceptionsToRetry { get => _exceptionsToRetry; set { OnPropertyChanged(nameof(ExceptionsToRetry), nameof(_exceptionsToRetry), value, typeof(Process)); } }

        private bool _isPaused = false;

        public bool IsPaused { get => _isPaused; private set => OnPropertyChanged(nameof(IsPaused), nameof(_isPaused), value, typeof(Process)); }

        private bool _pausePending = false;

        public bool PausePending { get => _pausePending; private set => OnPropertyChanged(nameof(PausePending), nameof(_pausePending), value, typeof(Process)); }

        private System.Collections.ObjectModel.ObservableCollection<FileSystemInfo> _pausedFiles = null;

        protected System.Collections.ObjectModel.ObservableCollection<FileSystemInfo> _PausedFiles

        {

            get => _pausedFiles;    

            set

            {

                _pausedFiles = value;

                PausedFiles = new System.Collections.ObjectModel.ReadOnlyObservableCollection<FileSystemInfo>(value);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PausedFiles)));

            }

        }

        public System.Collections.ObjectModel.ReadOnlyObservableCollection<FileSystemInfo> PausedFiles { get; private set; } = null;

        public event PropertyChangedEventHandler PropertyChanged;

        //public event EventHandler<CopyProgressChangedEventArgs> ProgressChanged;

        //  public Process() => SetProperties(new LoadFilesInfo()); // { PathsInfo = null; }

        // public Process() => SetProperties();
        // {



        // _pathsInfo = new List<PathInfoBase>(pathsInfo);

        // _pathsInfoReadOnly = new ReadOnlyCollection<PathInfoBase>(_pathsInfo);



        // _filesInfoLoader.PathsInfo = pathsInfo;

        // _filesInfoLoader.ActionType = ActionType;





        // }

        // TODO : vraiment utile ? 

        /// <summary>
        /// Initializes a new instance of the <see cref="Process"/> class.
        /// </summary>
        public Process() => SetProperties();

        private void SetProperties()
        {

            // FilesInfoLoader = filesInfoLoader;

            WorkerReportsProgress = true;

            WorkerSupportsCancellation = true;

            _PausedFiles = new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>();    

            _bgWorker.DoWork += (object sender, DoWorkEventArgs e) => DoWork?.Invoke(this, e);

            DoWork += (object sender, DoWorkEventArgs e) =>
            {

                OnDoWork(e);
            };

            _bgWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) => ProgressChanged?.Invoke(sender, e);

            _bgWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => OnRunWorkerCompleted(e);

            _bgWorker.Disposed += (object sender, EventArgs e) => Disposed?.Invoke(sender, e);

        }

        public void Suspend() => PausePending = true;

        protected abstract void OnDoWork(DoWorkEventArgs e);

        private void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)

        {

            OnPropertyChanged(nameof(IsBusy), true, false);

            RunWorkerCompleted?.Invoke(this, e);

        }

        //TODO : vraiment utile ?

        //public void Initialize(bool reset_Paths_Info_Properties = false)
        //{

        //CancellationPendingProperty = false;

        //    if (reset_Paths_Info_Properties) { _totalSize = 0; }

        //}

        //public void ReportProgress(CopyProgressChangedEventArgs eventArgs)
        //{
        //    ProgressChanged?.Invoke(this, eventArgs);
        //}

        // public void LoadFilesInfo() => _filesInfoLoader.LoadAsync();
    }

}