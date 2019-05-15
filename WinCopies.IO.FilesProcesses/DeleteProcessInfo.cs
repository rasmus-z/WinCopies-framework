using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WinCopies.Util;

namespace WinCopies.IO.FileProcesses
{
    public class DeleteProcessInfo : Process
    {
        /// <summary>
        /// Gets the type of this process.
        /// </summary>
        public override ActionType ActionType => ActionType.Deletion;

        private long _deletedFiles = 0;

        /// <summary>
        /// Gets the number of deleted files.
        /// </summary>
        public long DeletedFiles

        {

            get => _deletedFiles;

            private set => OnPropertyChanged(nameof(DeletedFiles), nameof(_deletedFiles), value, typeof(DeleteProcessInfo));

        }

        public FileSystemInfo _currentDeletedFile = null;

        /// <summary>
        /// Gets the file which is being deleted.
        /// </summary>
        public FileSystemInfo CurrentDeletedFile

        {

            get => _currentDeletedFile;

            set => OnPropertyChanged(nameof(CurrentDeletedFile), nameof(_currentDeletedFile), value, typeof(DeleteProcessInfo));

        }

        private Size _current_Deleted_Size = new Size(0, SizeUnit.Byte);

        /// <summary>
        /// Gets the total deleted size.
        /// </summary>
        public Size CurrentDeletedSize { get => _current_Deleted_Size; set => OnPropertyChanged(nameof(CurrentDeletedSize), nameof(_current_Deleted_Size), value, typeof(DeleteProcessInfo)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProcessInfo"/> class.
        /// </summary>
        public DeleteProcessInfo()

        {



        }

        public void StartDeletion() => StartDeletion(false);

        public void StartDeletion(bool onlyFirstFile)

        {

            if (FilesInfoLoader == null)

                throw new ArgumentNullException(nameof(FilesInfoLoader));

            if (FilesInfoLoader.IsBusy)

                throw new Exception(Generic.LoadingFilesInfoModuleIsRunning);

            if (!FilesInfoLoader.IsLoaded)

                throw new Exception(Generic.LoadingFilesInfoModuleHasNotRanYet);

            if (onlyFirstFile && (ExceptionsToRetry == FileProcesses.Exceptions.None || HowToRetryWhenExceptionOccured == HowToRetry.Cancel))

                throw new ArgumentException(string.Format(Generic.IncompatibleValues, nameof(onlyFirstFile), nameof(ExceptionsToRetry)));

            _bgWorker.RunWorkerAsync(onlyFirstFile);

        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            OnPropertyChanged(nameof(IsBusy), false, true);

            bool onlyFirstFile = (bool)e.Argument;

            if (PausedFiles.Count == 0 && HowToRetryWhenExceptionOccured == HowToRetry.Cancel)

            {

                FileSystemInfo item = null;

                void delete()

                {

                    item = ExceptionsProtected[0];

                    item._exception = FileProcesses.Exceptions.None;

                    DeletedFiles += 1;

                    if (ActionType != ActionType.Recycling && item.FileSystemInfoProperties.GetType() == typeof(FileInfo))

                        CurrentDeletedSize += ((FileInfo)item.FileSystemInfoProperties).Length;

                    ExceptionsProtected.RemoveAt(0);

                }

                if (onlyFirstFile)

                    delete();

                else

                    while (ExceptionsProtected.Count > 0)

                        delete();

                ExceptionsOccurred = ExceptionsProtected.Count == 0;

                return;

            }



            // List<string> folder_Path = new List<string>(); // Liste des répertoires à supprimer lors du déplacement une fois celui-ci effectué (déplacements uniquement).

            void getStartAndLength(Exceptions flag, out int _start, out int _length)

            {

                if (flag == FileProcesses.Exceptions.None)

                {

                    _start = StartItemIndex;

                    _length = FilesInfoLoader.PathsLoaded.Count;

                }

                else

                {

                    _start = ExceptionsProtected.IndexOf(ExceptionsProtected.First((FileSystemInfo f) => f.Exception == flag));

                    _length = onlyFirstFile ? 1 : ExceptionsProtected.IndexOf(ExceptionsProtected.Last((FileSystemInfo f) => f.Exception == flag)) + 1;

                }

            }

            int start;

            int length;

            if (PausedFiles.Count > 0)

                this_all_Files_DoWork(PausedIndex, PausedFiles.Count, _PausedFiles, _PausedFiles == ExceptionsProtected, true);

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

                        ExceptionsOccurred = ExceptionsProtected.Count > 0; // If we are in a re-try process, we check if there are still paths in the exceptions list.

                    }

                }

            }

        }

        private void this_all_Files_DoWork(int start, int length, IList<FileSystemInfo> items, bool isARetry = false, bool isResuming = false)
        {

            using (FileOperation fileOperation = new FileOperation())

            {

                fileOperation.Advise(new FileOperationProgressSink());

                if (ActionType == ActionType.Recycling)

                    fileOperation.SetOperationFlags(ShellOperationFlags.FOF_ALLOWUNDO);

                fileOperation.SetOperationFlags(ShellOperationFlags.FOF_SILENT);

                fileOperation.SetOperationFlags(ShellOperationFlags.FOF_NOERRORUI);

                Exceptions deleteFile(FileSystemInfo _path)

                {

                    if (!_path.FileSystemInfoProperties.Exists)

                        return FileProcesses.Exceptions.PathNotFound;

                    fileOperation.DeleteItem(ShellObject.FromParsingName(_path.FileSystemInfoProperties.FullName), null);

                    fileOperation.PerformOperations();

                    return fileOperation.GetAnyOperationsAborted() ? FileProcesses.Exceptions.Unknown : FileProcesses.Exceptions.None;

                }



                if (isResuming && PausedFile != null)

                {

                    Exceptions ex = deleteFile(PausedFile);

                    PausedFile = null;

                    _PausedFiles = new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>();

                    PausedIndex = -1;

                }

                else if (!isResuming)

                {

                    if (length == 1 && HowToRetryWhenExceptionOccured == HowToRetry.None && items[start].HowToRetryToProcess == HowToRetry.None)

                        return;



                    ExceptionsOccurred = false;

                    FileSystemInfo path = null;

                    int i = start;

                    void onException(Exceptions exception)

                    {

                        ExceptionsOccurred = true;

                        if (path._exception != exception)

                        {

                            path._exception = exception;

                            if (!isARetry)

                                ExceptionsProtected.Add(path);

                        }

                        ReportProgress(i / length * 100);

                    }

                    while (i < length && !CancellationPending)

                    {

                        try

                        {

                            #region item pre-processing actions

                            path = items[i];

                            if (PausePending)

                            {

                                PausedFile = path;

                                _PausedFiles = isARetry ? (System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>)items : new System.Collections.ObjectModel.ObservableCollection<FileSystemInfo>(items);

                                PausedIndex = i;



                                break;

                            }

                            if (isARetry && path.HowToRetryToProcess != HowToRetry.Retry && HowToRetryWhenExceptionOccured != HowToRetry.Retry && path.HowToRetryToProcess != HowToRetry.Rename && HowToRetryWhenExceptionOccured != HowToRetry.Rename && path.HowToRetryToProcess != HowToRetry.Replace && HowToRetryWhenExceptionOccured != HowToRetry.Replace)

                            {

                                if (path.HowToRetryToProcess != HowToRetry.None || HowToRetryWhenExceptionOccured != HowToRetry.None)

                                {

                                    ExceptionsProtected.RemoveAt(i);

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

                                        DeletedFiles += 1;

                                        if (path.FileSystemInfoProperties.GetType() == typeof(FileInfo))

                                            CurrentDeletedSize += ((FileInfo)path.FileSystemInfoProperties).Length;

                                    }

                                }

                                // i++;

                                length--;

                                ReportProgress(0);

                                ExceptionsOccurred = i == length && ExceptionsProtected.Count > 0;

                                continue;

                            }



                            CurrentDeletedFile = path;

                            #endregion

                            Exceptions ex = FileProcesses.Exceptions.None;

                            if ((ex = deleteFile(path)) == FileProcesses.Exceptions.None)

                                DeletedFiles += 1;

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

                            if (new DriveInfo(System.IO.Path.GetPathRoot(FilesInfoLoader.SourcePath)).IsReady)

                            {

                                ExceptionsOccurred = true;

                                ExceptionsProtected.Clear();

                                foreach (FileSystemInfo _path in items)

                                {

                                    _path._exception = FileProcesses.Exceptions.DiskNotReady;

                                    ExceptionsProtected.Add(_path);

                                }

                                break;

                            }

                            else

                            onException(FileProcesses.Exceptions.Unknown);

                        }

                        catch (Win32Exception)

                        {

                            onException(FileProcesses.Exceptions.Unknown);

                        }

                        if (isARetry)

                        {

                            ExceptionsProtected.RemoveAt(0);

                            length--;

                        }

                        else

                            i++;

                    }

                    // todo : maybe more functional with a dictionary which would create a collection corresponding to an exception when it'd be needed (this case would mean no sorting needed)

                    ExceptionsProtected.Sort(new FileSystemInfoComparer());

                }

            }

        }
    }
}
