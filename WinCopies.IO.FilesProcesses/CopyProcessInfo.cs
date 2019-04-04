using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using WinCopies.Util;
using static WinCopies.IO.FileProcesses.Copy;

namespace WinCopies.IO.FileProcesses
{

    //TODO : supprimer les 'OK' des commentaires et revoir l'ensemble des commentaires

    //TODO : rationnaliser les classes pour les différents types de processus ?

    //TODO : d'autres éléments à mettre en abstract ou pas dans la classe de base (Process) ?

    //TODO : revérifier les commentaires xml pour cette classe !

    //TODO : classer les propriétés et les champs par ordre alphabétique



    /// <summary>
    /// Provides instance methods and properties to copy files and folders. This class inherits of <see cref="Process"/>.
    /// </summary>
    public class CopyProcessInfo : Process
    {

        private readonly ActionType _actionType = ActionType.Unknown;

        public override ActionType ActionType => _actionType;

        private FileSystemInfo _pausedFile = null;

        /// <summary>
        /// If a copy is paused during copying, this property gets the file paused, otherwise it returns null.
        /// </summary>
        public FileSystemInfo PausedFile { get => _pausedFile; private set => OnPropertyChanged(nameof(PausedFile), nameof(_pausedFile), value, typeof(CopyProcessInfo)); }

        private string _destPausedFilePath = null;

        public string DestPausedFilePath { get => _destPausedFilePath; set => OnPropertyChanged(nameof(DestPausedFilePath), nameof(_destPausedFilePath), value, typeof(CopyProcessInfo)); }

        private int _pausedIndex = -1;

        public int PausedIndex { get => _pausedIndex; set => OnPropertyChanged(nameof(PausedIndex), nameof(_pausedIndex), value, typeof(CopyProcessInfo)); }

        private readonly long _copiedFiles = 0;

        /// <summary>
        /// Gets the number of copied files.
        /// </summary>
        public long CopiedFiles
        {

            get => _copiedFiles;

            private set => OnPropertyChanged(nameof(CopiedFiles), nameof(_copiedFiles), value, typeof(CopyProcessInfo));

        }

        private readonly bool _overwrite = false;

        public bool Overwrite
        {

            get => _overwrite;

            set => OnPropertyChangedWhenNotBusy(nameof(Overwrite), nameof(_overwrite), value, typeof(CopyProcessInfo), true);

        }

        //TODO : 'internal' ? 'private' ?

        // public bool _Is_A_File_Moving = false;

        // todo : exception : 'exceptions' ? - nullable

        private readonly HowToRetry _how_To_Retry_When_Exception_Occured = HowToRetry.None;

        /// <summary>
        /// Gets or sets a value that indicates how to retry to copy the file system objects.
        /// </summary>
        /// <remarks>If this property is set, this property have to be set to null individually for all the path items.</remarks>
        public HowToRetry HowToRetryWhenExceptionOccured
        {

            get => _how_To_Retry_When_Exception_Occured;

            set => OnPropertyChangedWhenNotBusy(nameof(HowToRetryWhenExceptionOccured), nameof(_how_To_Retry_When_Exception_Occured), value, typeof(CopyProcessInfo), true);

        }

        private readonly bool _isAFileMoving = false;

        //TODO : 'OK' : - simplifier l'utilisation ? - nom / nomenclature du nom ? - voir également commenatire CopyFiles.cs

        // /// <summary>
        // /// Obtient ou défini une valeur qui indique si ce module effectue un déplacement de fichier. True pour déplacer directement les fichiers si le déplacement a lieu sur le même lecteur ou supprimer les originaux si le déplacement s'effectue sur plusieurs lecteurs différents. False pour copier à chaque fois tous les fichiers et conserver les originaux.
        // /// <br /><br />
        // /// Remarque : Renvoi null ou Nothing en VB si le module CopyFiles n'est pas instancié !
        // /// <br /><br />
        // /// Remarque : Si la valeur passée pour l'assignation est null ou Nothing en VB la propriété sera finalement assignée avec false pour ne pas créer une erreur de conversion .
        // /// </summary>

        /// <summary>
        /// Gets or sets a value that indicates if this module represents a file moving. True for moving directly the selected file system objects if the moving is running on the same drive for both the source and destination paths or deleting the original file system objects if the moving is running on multiple drives. False for copying all the file system objects each time and keep the original file system objects.
        /// </summary>
        public bool IsAFileMoving
        {
            get => _isAFileMoving; set

            {

                OnPropertyChangedWhenNotBusy(nameof(IsAFileMoving), nameof(_isAFileMoving), value, typeof(CopyProcessInfo), true);

                OnPropertyChanged(nameof(ActionType), nameof(_actionType), value ? FileProcesses.ActionType.Move : FileProcesses.ActionType.Copy, typeof(CopyProcessInfo));

            }

        }

        //TODO : 'internal' - 'private' ?

        // public string _destPath = "";

        //TODO : ?



        private readonly FileSystemInfo _currentCopiedFile = null;

        // /// <summary>
        // /// Le fichier qui est en cours de copie ou de actuellement. S'il s'agit d'un déplacement et que le fichier est déplacé sur un autre lecteur de celui d'origine, il est d'abord copié, puis supprimé de son emplacement d'origine. Certains noms de fichiers ainsi que tous les noms de dossiers accessibles via cette propriété ne devraient donc pas y rester logntemps avant de laisser leur place aux éléments suivants si le processus est exécuté à une vitesse moyenne. Cette propriété est en lecture seule.
        // /// </summary>

        /// <summary>
        /// Gets the file which is copying or moving.
        /// </summary>
        public FileSystemInfo CurrentCopiedFile
        {

            get => _currentCopiedFile;

            private set => OnPropertyChanged(nameof(CurrentCopiedFile), nameof(_currentCopiedFile), value, typeof(CopyProcessInfo));

        }

        //TODO : ?

        // /// <summary>
        // /// OK - Également accessible depuis le module CopyFiles de ce module-ci quand celui-ci est instancié. Emplacement où sont stockés les fichiers copiés
        // /// <br />
        // /// Remarque : Renvoi null ou Nothing en VB si le module CopyFiles n'est pas instancié !
        // /// </summary>

        /// <summary>
        /// Gets the destination path to copy or moving the files.
        /// </summary>
        public string DestPath { get; } = null;

        private readonly Size _current_Copied_Size = new Size(0, SizeUnit.Byte);

        private readonly Size _current_File_Copied_Size = new Size(0, SizeUnit.Byte);

        /// <summary>
        /// Gets the total current copied size.
        /// </summary>
        public Size CurrentCopiedSize { get => _current_Copied_Size; set => OnPropertyChanged(nameof(CurrentCopiedSize), nameof(_current_Copied_Size), value, typeof(CopyProcessInfo)); }

        /// <summary>
        /// Gets the current file copied size.
        /// </summary>
        public Size CurrentFileCopiedSize { get => _current_File_Copied_Size; set => OnPropertyChanged(nameof(CurrentFileCopiedSize), nameof(_current_File_Copied_Size), value, typeof(CopyProcessInfo)); }

        // todo : utiliser des observable collections "traditionnelles" ou bien des read-only wrapper, afin de respecter la logique

        // #region Exceptions collections

        private readonly System.Collections.ObjectModel.ObservableCollection<FileSystemInfo> _exceptions = new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>();

        /// <summary>
        /// Gets a <see cref="System.Collections.ObjectModel.ReadOnlyObservableCollection{FileSystemInfo}"/> which represents the files for which a <see cref="WinCopies.IO.FileProcesses.Exceptions"/> exception has occurred.
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyObservableCollection<FileSystemInfo> Exceptions { get; } = null;

        /// <summary>
        /// Initializes a new instance of <see cref="CopyProcessInfo"/> which will keep in memory the source files and folders.
        /// </summary>
        /// <param name="destPath">Destination folder path for copying the source files and folders</param>
        /// <param name="isAFileMoving">True if the process is a file moving</param>
        // /// <param name="filesInfoLoader">Files info loader for this <see cref="CopyProcessInfo"/>.</param>
        public CopyProcessInfo(string destPath, bool isAFileMoving)
        {

            DestPath = destPath;

            IsAFileMoving = isAFileMoving;

            Exceptions = new System.Collections.ObjectModel.ReadOnlyObservableCollection<FileSystemInfo>(_exceptions);

        }

        //private void On_exceptions_CollectionItemRemovedAt(int index)

        //{

        //    _exceptions.RemoveAt(index);

        //    _exceptions.Sort(new FileSystemInfoComparer());

        //}



        // TODO : ou bien plutôt mettre directement cela dans le BGWorker

        //TODO: problème de back-slash - private ? vraiment utile en tant que méthode à part ? déplacer la conditionnelle pour ne pas la ré-itérer à chaque fois ?

        public string GetCopyPath(string oldFile, bool rename_Path)
        {

            string path = DestPath + oldFile.Substring(System.IO.Path.GetDirectoryName(FilesInfoLoader.Paths[0].FileSystemInfoProperties.FullName).Length);

#if DEBUG 

            Debug.WriteLine($"DestPath : {path}, oldFile : {oldFile}");
            Debug.WriteLine(oldFile);
            Debug.WriteLine(FilesInfoLoader.Paths[0].FileSystemInfoProperties.FullName);
            Debug.WriteLine(FilesInfoLoader.PathsLoaded[0].FileSystemInfoProperties.FullName);

#endif

            string dest_Path = "";

            if (rename_Path)

                //inutile : le test pour passer le fichier est effectué dans la méthode principale if (oldFile.HowToRetryToProcess == HowToRetryToProcess.Rename)

                dest_Path = Path.RenamePathWithAutomaticNumber(oldFile, System.IO.Path.GetDirectoryName(path));

            else

                dest_Path = path;

            return dest_Path;

        }

        //private void loadFilesInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) =>

        //    // todo : si le disque du répertoire de destination a un espace de stockage insuffisant, mettre une exception.

        //    FilesInfoLoaded(this, new EventArgs());

        // todo : copy tout-court pour le nom ?

        /// <summary>
        /// Starts the copy.
        /// </summary>
        public void StartCopy() => StartCopy(false);

        /// <summary>
        /// Starts the copy with the possibility to copy only the first item.
        /// </summary>
        /// <param name="onlyFirstFile">Define if the process must be copy or move only the first item</param>
        public void StartCopy(bool onlyFirstFile)
        {

            //            CancellationPending = false;

            if (FilesInfoLoader == null)

                throw new ArgumentNullException(nameof(FilesInfoLoader));

            if (FilesInfoLoader.IsBusy)

                //TODO:

                throw new Exception(Generic.LoadingFilesInfoModuleIsRunning);

            if (!FilesInfoLoader.IsLoaded)

                //TODO:

                throw new Exception(Generic.LoadingFilesInfoModuleHasNotRanYet);

            if (onlyFirstFile && (ExceptionsToRetry == FileProcesses.Exceptions.None || HowToRetryWhenExceptionOccured == HowToRetry.Cancel))

                throw new ArgumentException(string.Format(Generic.IncompatibleValues, nameof(onlyFirstFile), nameof(ExceptionsToRetry)));

            /*if (CopyFiles == null)*/ //CopyFiles = new CopyFiles(FilesInfoLoader.pathsLoaded, DestPath);

            //TODO:

            _bgWorker.RunWorkerAsync(onlyFirstFile);

        }

        //private void CopyFiles_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //    CopyProgressChangedEventArgs copyProgressChangedEventArgs = new CopyProgressChangedEventArgs((int)(CopyFiles.copiedFiles / PathsToCopy.Count * 100));

        //    ReportProgress(copyProgressChangedEventArgs);

        //}

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            OnPropertyChanged(nameof(IsBusy), false, true);

            // #if DEBUG 
            //             MessageBox.Show("a");
            // #endif

            // int i = 0;

            // string formated_Path = "";

            // string formated_Copy_Path_Address = "";

            // string dest_Path_2 = "";

            // string dest_Path_3 = "";    



            if (PausedFiles.Count == 0 && HowToRetryWhenExceptionOccured == HowToRetry.Cancel)

            {

                FileSystemInfo item = null;

                while (_exceptions.Count > 0)

                {

                    item = _exceptions[0];

                    item._exception = FileProcesses.Exceptions.None;

                    CopiedFiles += 1;

                    if (item.FileSystemInfoProperties.GetType() == typeof(FileInfo))

                        CurrentCopiedSize += ((FileInfo)item.FileSystemInfoProperties).Length;

                    _exceptions.RemoveAt(0);

                }

                ExceptionsOccurred = false;

                return;

            }



            // List<string> folder_Path = new List<string>(); // Liste des répertoires à supprimer lors du déplacement une fois celui-ci effectué (déplacements uniquement).

            bool onlyFirstFile = (bool)e.Argument;

            void getStartAndLength(Exceptions flag, out int _start, out int _length)

            {

                if (flag == FileProcesses.Exceptions.None)

                {

                    _start = StartItemIndex;

                    _length = FilesInfoLoader.PathsLoaded.Count;

                }

                else

                {

                    _start = _exceptions.IndexOf(_exceptions.First((FileSystemInfo f) => f.Exception == flag));

                    _length = onlyFirstFile ? 1 : _exceptions.IndexOf(_exceptions.Last((FileSystemInfo f) => f.Exception == flag)) + 1;

                }

            }

            int start = 0;

            int length = 0;

            if (PausedFiles.Count > 0)

                this_all_Files_DoWork(PausedIndex, PausedFiles.Count, _PausedFiles, _PausedFiles == _exceptions, true);

            else if (ExceptionsToRetry == FileProcesses.Exceptions.None)
            {

                getStartAndLength(FileProcesses.Exceptions.None, out start, out length);

                this_all_Files_DoWork(start, length, FilesInfoLoader.PathsLoaded);

            }

            else

            {

                Type type = ExceptionsToRetry.GetType();

                Enum enumValue = null;



                foreach (string s in type.GetEnumNames())

                {

                    enumValue = (Enum)Enum.Parse(type, s);



                    if (enumValue.GetNumValue(s).Equals(0)) continue;



                    if (ExceptionsToRetry.HasFlag(enumValue))

                    {

                        getStartAndLength((Exceptions)enumValue, out start, out length);

                        this_all_Files_DoWork(start, length, Exceptions, true);

                        ExceptionsOccurred = _exceptions.Count > 0; // If we are in a re-try process, we check if there are still paths in the exceptions list.

                    }

                }

            }

            // else if (ExceptionsToRetry.HasFlag(Exceptions.None))

            // }

        }

        // todo : nom + le 'i' ? + déplacer les répétitions dans une méthode : avoir un seul do work au final, une méthode pour récupérer les noms des fichiers et passer les collections des fichiers à copier en paramètres

        //private void this_MultipleEtcDoWork(int i)
        //{



        //}

        // ne pas pouvoir ré-appeler après une première exécution sans une remise à 0 des propriétés

        // ou bien alors remettre à 0 les collections sur les exceptions

        // todo : 'items' : files_To_Copy - paths ? 

        private void this_all_Files_DoWork(int start, int length, IList<FileSystemInfo> items, bool isARetry = false, bool isResuming = false)
        {

            void copyFile(ref Exceptions ex, FileSystemInfo _path, string destFilePath, int currentIndex, bool overwrite)

            {

                ex = Copy_Files(_path.FileSystemInfoProperties.FullName, destFilePath, IsAFileMoving, overwrite,
                             (
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            uint dwStreamNumber,
            CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData) =>
                             copyProgressCallback(this, _path, ref destFilePath, items, ref currentIndex, ref totalFileSize, ref totalBytesTransferred, ref streamSize, ref streamBytesTransferred, ref dwStreamNumber, dwCallbackReason, ref hSourceFile, ref hDestinationFile, ref lpData)).ex;

                if (ex == FileProcesses.Exceptions.None)

                {

                    //TODO: ? - Si c'est un déplacement sur le même lecteur, plutôt procéder par nbre de fichiers déplacés / s.

                    if (IsAFileMoving && System.IO.Path.GetPathRoot(FilesInfoLoader.SourcePath) == System.IO.Path.GetPathRoot(DestPath))
                    {

                        long fileLength = ((FileInfo)_path.FileSystemInfoProperties).Length;

                        CurrentFileCopiedSize = (Size)fileLength;

                        CurrentCopiedSize += fileLength;

                    }

                }

            }



            CopyProgressResult copyProgressCallback(CopyProcessInfo cpi,
                        FileSystemInfo currentPath,
                       ref string destFilePath,
                        IList<FileSystemInfo> currentFiles,
                        ref int currentIndex,
                        ref long TotalFileSize,
                      ref long TotalBytesTransferred,
                      ref long StreamSize,
                      ref long StreamBytesTransferred,
                      ref uint dwStreamNumber,
                      CopyProgressCallbackReason dwCallbackReason,
                      ref IntPtr hSourceFile,
                      ref IntPtr hDestinationFile,
                      ref IntPtr lpData)
            {

                // TODO : meilleure gestion ?

                if (dwStreamNumber > 1) return CopyProgressResult.PROGRESS_QUIET;

                //TODO: 'TotalFileSize' : utiliser plutôt path.Length ? renommer Path.Length en Path.Size ?

                Size oldCurrent_File_Copied_Size = cpi._current_File_Copied_Size;

                cpi.CurrentFileCopiedSize = (Size)StreamBytesTransferred;

                cpi.CurrentCopiedSize += StreamBytesTransferred - oldCurrent_File_Copied_Size;

#if DEBUG
                if (cpi.CurrentCopiedSize >= cpi.FilesInfoLoader.TotalSize) Debug.WriteLine(cpi._copiedFiles.ToString() + " " + cpi.CurrentCopiedSize.ToString() + " " + cpi.FilesInfoLoader.TotalSize.ToString());
#endif

                // var fi = new FileInfo(currentPath.FileSystemInfoProperties.FullName);



                //TODO:

                cpi.ReportProgress(0);

                if (cpi.PausePending)

                {

                    cpi.PausedFile = currentPath;

                    cpi.DestPausedFilePath = destFilePath;

                    cpi._PausedFiles = isARetry ? (System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>)currentFiles : new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>(currentFiles);

                    cpi.PausedIndex = currentIndex;



                    cpi.Pause();

                }

                return cpi.CancellationPending ? CopyProgressResult.PROGRESS_CANCEL : CopyProgressResult.PROGRESS_CONTINUE;

            }



            #region pre-processing actions



            DriveInfo driveInfo = null;



            if (isResuming && _pausedFile != null)

            {

                Exceptions ex = FileProcesses.Exceptions.None;

                copyFile(ref ex, _pausedFile, _destPausedFilePath, start, _overwrite || (isARetry && (_pausedFile.HowToRetryToProcess == HowToRetry.Replace || _how_To_Retry_When_Exception_Occured == HowToRetry.Replace)));

                PausedFile = null;

                DestPausedFilePath = null;

                _PausedFiles = new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>();

                PausedIndex = -1;

            }

            else if (!isResuming)

            {

                if (length == 1 && _how_To_Retry_When_Exception_Occured == HowToRetry.None && items[start].HowToRetryToProcess == HowToRetry.None)

                    return;



                ExceptionsOccurred = false;



                // if (isARetry && HowToRetryWhenExceptionOccured == HowToRetry.None) return;



                // We check if the destination drive has enough space on disk if the destination drives are different or if the current process is a copy. 

                driveInfo = new DriveInfo(System.IO.Path.GetPathRoot(DestPath));

                if ((new DriveInfo(System.IO.Path.GetPathRoot(FilesInfoLoader.SourcePath)).RootDirectory != driveInfo.RootDirectory || !_isAFileMoving) && driveInfo.AvailableFreeSpace <= FilesInfoLoader.TotalSize)

                {

                    driveInfo = null;

                    ExceptionsOccurred = true;

                    foreach (FileSystemInfo _path in items)

                    {

                        _path._exception = FileProcesses.Exceptions.NotEnoughSpaceOnDisk;

                        _exceptions.Add(_path);

                    }

                    _exceptions.Sort(new FileSystemInfoComparer());

                    return;

                }

                else driveInfo = null;

                #endregion



            }



            #region Variables initialization

            FileSystemInfo path = null;

            int i = start;

            #endregion

            // _length = onlyFirstFile ? 1 : items.Count;

            // TODO : for ? -- attention alors aux incrémentations manuelles / autre système ? 

            void onException(Exceptions exception)

            {

                ExceptionsOccurred = true;

                if (path._exception != exception)

                {

                    path._exception = exception;

                    if (!isARetry)

                        _exceptions.Add(path);

                }

                ReportProgress(i / length * 100);

            }

            while (i < length && !CancellationPending)
            {

                try

                {

                    #region pre-processing item actions

                    path = items[i];

#if DEBUG

                    Debug.WriteLine($"Fichier à copier : {path.FileSystemInfoProperties.FullName}");

#endif 

                    string destFilePath = "";



                    bool rename = path.Exception == FileProcesses.Exceptions.FileAlreadyExists && (path.HowToRetryToProcess == HowToRetry.Rename || _how_To_Retry_When_Exception_Occured == HowToRetry.Rename);

                    destFilePath = GetCopyPath(path.FileSystemInfoProperties.FullName, rename);

                    if (DestPath.Length > path.FileSystemInfoProperties.FullName.Length && DestPath.Substring(path.FileSystemInfoProperties.FullName.Length).Contains("\\"))

                    {

                        onException(FileProcesses.Exceptions.DestPathIsASubdirectory);

                        return;

                    }

                    if (PausePending)

                    {

                        PausedFile = path;

                        DestPausedFilePath = destFilePath;

                        _PausedFiles = isARetry ? (System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>)items : new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>(items);

                        PausedIndex = i;



                        break;

                    }

                    if (isARetry && path.HowToRetryToProcess != HowToRetry.Retry && _how_To_Retry_When_Exception_Occured != HowToRetry.Retry && path.HowToRetryToProcess != HowToRetry.Rename && _how_To_Retry_When_Exception_Occured != HowToRetry.Rename && path.HowToRetryToProcess != HowToRetry.Replace && _how_To_Retry_When_Exception_Occured != HowToRetry.Replace)

                    {

                        if (path.HowToRetryToProcess != HowToRetry.None || _how_To_Retry_When_Exception_Occured != HowToRetry.None)

                        {

                            _exceptions.RemoveAt(i);

                            // items.RemoveAt(i);    

                            // todo : HowToRetry ...

                            //if (path.HowToRetryToProcess == HowToRetry.None)

                            //{

                            //    onException(path.Exception);

                            //    continue;

                            //}

                            if (path.HowToRetryToProcess == HowToRetry.Ignore || HowToRetryWhenExceptionOccured == HowToRetry.Ignore)

                            {

                                path._exception = FileProcesses.Exceptions.None;

                                CopiedFiles += 1;

                                if (path.FileSystemInfoProperties.GetType() == typeof(FileInfo))

                                    CurrentCopiedSize += ((FileInfo)path.FileSystemInfoProperties).Length;

                            }

                        }

                        // i++;

                        length--;

                        ReportProgress(0);

                        ExceptionsOccurred = i == length && _exceptions.Count > 0;

                        continue;

                    }



                    CurrentFileCopiedSize = (Size)0;

                    CurrentCopiedFile = path;

#if DEBUG

                    Debug.WriteLine($"Fichier de destination : {destFilePath + " " + path.FileSystemInfoProperties.FullName}");

#endif

                    #endregion

                    FileProcesses.Exceptions ex = FileProcesses.Exceptions.None;

                    if (path.FileType == FileTypes.Drive || path.FileType == FileTypes.Folder)
#if DEBUG
                    {

                        Console.WriteLine($"Création du répertoire : {destFilePath}");
#endif

                        Directory.CreateDirectory(destFilePath);

#if DEBUG
                    }
#endif

                    else if (path.FileType == FileTypes.File)
                    {

#if DEBUG
                        Debug.WriteLine($"Copie du fichier : de {path.FileSystemInfoProperties.FullName} vers {destFilePath}");
#endif



                        copyFile(ref ex, path, destFilePath, i, _overwrite || (isARetry && (path.HowToRetryToProcess == HowToRetry.Replace || _how_To_Retry_When_Exception_Occured == HowToRetry.Replace)));

                    }

                    if (ex == FileProcesses.Exceptions.None)

                        CopiedFiles += 1;

                    else

                        onException(ex);

                    //else if (ex == Exceptions.FileAlreadyExists)

                    //{

                    //    _file_Already_Exists.Add(path);

                    //}

                    //else if (ex == Exceptions.DirectoryNotFound)

                    //{

                    //    _directory_Not_Found.Add(path);

                    //}



                    //TODO:

                    // if (start_Item_Index + 1 == pathsToCopy.Count) start_Item_Index = 0; else start_Item_Index += 1;

                    //TODO:

                    ReportProgress(i / length * 100);

                }

                catch (DirectoryNotFoundException)
                {

                    onException(FileProcesses.Exceptions.PathNotFound);

                }
                catch (PathTooLongException)
                {

                    onException(FileProcesses.Exceptions.FileNameTooLong);

                }
                // catch (IOException) { ex = FileProcesses.Exceptions.FileAlreadyExists; } 
                catch (UnauthorizedAccessException)
                {

                    onException(FileProcesses.Exceptions.AccessDenied);

                }

                catch (IOException)

                {

                    bool driveNotReady = false;

                    driveInfo = new DriveInfo(System.IO.Path.GetPathRoot(FilesInfoLoader.SourcePath));

                    if (!driveInfo.IsReady)

                        driveNotReady = true;

                    else

                    {

                        driveInfo = new DriveInfo(System.IO.Path.GetPathRoot(DestPath));

                        if (!driveInfo.IsReady)

                            driveNotReady = true;

                    }

                    driveInfo = null;

                    if (driveNotReady)

                    {

                        ExceptionsOccurred = true;

                        _exceptions.Clear();

                        foreach (FileSystemInfo _path in items)

                        {

                            _path._exception = FileProcesses.Exceptions.DiskNotReady;

                            _exceptions.Add(_path);

                        }

                        break;

                    }

                }

                if (isARetry)

                {

                    _exceptions.RemoveAt(0);

                    length--;

                }

                else

                    i++;

            }

            // todo : maybe more functional with a dictionary which would create a collection corresponding to an exception when it'd be needed (this case would mean no sorting needed)

            _exceptions.Sort(new FileSystemInfoComparer());




#if DEBUG
            Debug.WriteLine("azertyuiop : " + Exceptions.Count((FileSystemInfo f) => { return f.Exception == FileProcesses.Exceptions.FileAlreadyExists; }).ToString());
#endif

        }

        //TODO : ?

        // /// <summary>
        // /// Libère toutes les ressources utilisées par le composant CopyFiles s'il est différent de null ou Nothing en Visual Basic.
        // /// </summary>

        // public void Dispose()
        // {

        //     if (CopyFiles != null) CopyFiles.Dispose();

        // }

        public class FileSystemInfoComparer : IComparer<FileSystemInfo>

        {

            StringComparer sc = StringComparer.Create(CultureInfo.CurrentCulture, true);

            public int Compare(FileSystemInfo x, FileSystemInfo y)
            {

                if (x.Exception < y.Exception) return -1;

                else if (x.Exception == y.Exception) return 0;

                else return 1;

            }

        }

    }

}
