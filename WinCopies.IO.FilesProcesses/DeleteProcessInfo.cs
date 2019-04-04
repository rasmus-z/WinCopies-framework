using System;
using System.ComponentModel;

namespace WinCopies.IO.FileProcesses
{
    public class DeleteProcessInfo : Process
    {
        public override ActionType ActionType => throw new NotImplementedException();

        private long _deletedFiles = 0;

        public long DeletedFiles

        {

            get => _deletedFiles;

            private set => OnPropertyChanged(nameof(DeletedFiles), nameof(_deletedFiles), value, typeof(DeleteProcessInfo));

        }

        public FileSystemInfo _currentDeletedFile = null;

        public FileSystemInfo CurrentDeletedFile

        {

            get => _currentDeletedFile;

            set => OnPropertyChanged(nameof(CurrentDeletedFile), nameof(_currentDeletedFile), value, typeof(DeleteProcessInfo));

        }

        private Size _current_Deleted_Size = new Size(0, SizeUnit.Byte);

        public Size CurrentDeletedSize { get => _current_Deleted_Size; set => OnPropertyChanged(nameof(CurrentDeletedSize), nameof(_current_Deleted_Size), value, typeof(DeleteProcessInfo)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProcessInfo"/> class.
        /// </summary>
        public DeleteProcessInfo()

        {



        }

        public void StartCopy() => StartCopy(false);

        public void StartCopy(bool onlyFirstFile)

        {

            if (FilesInfoLoader == null)

                throw new ArgumentNullException("FilesInfoLoader");

            if (FilesInfoLoader.IsBusy)

                //TODO:

                throw new Exception(Generic.LoadingFilesInfoModuleIsRunning);

            if (!FilesInfoLoader.IsLoaded)

                //TODO:

                throw new Exception(Generic.LoadingFilesInfoModuleHasNotRanYet);

            _bgWorker.RunWorkerAsync(onlyFirstFile);

        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            OnPropertyChanged(nameof(IsBusy), false, true);

            bool onlyFirstFile = (bool)e.Argument;



        }
    }
}
