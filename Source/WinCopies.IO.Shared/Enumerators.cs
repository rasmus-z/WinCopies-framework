using Microsoft.WindowsAPICodePack.PortableDevices.EventSystem;
using SevenZip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security;
using System.Text;
using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    [Serializable]
    public sealed class ArchiveItemInfoEnumerator : IEnumerator<ArchiveItemInfo>, IEnumerable<ArchiveItemInfo>
    {
        private int _index = -1;

        private IArchiveItemInfoProvider _archiveItemInfoProvider;

        private Queue<IFileSystemObject> _paths = new Queue<IFileSystemObject>();

        private ArchiveItemInfo _current;

        public ArchiveItemInfo Current => disposedValue ? throw GetExceptionForDispose(false) : _current;

        public Predicate<ArchiveFileInfoEnumeratorStruct> Func { get; }

        public ArchiveItemInfoEnumerator(IArchiveItemInfoProvider archiveItemInfoProvider, Predicate<ArchiveFileInfoEnumeratorStruct> func)
        {
            _archiveItemInfoProvider = archiveItemInfoProvider;

            Func = func;
        }

        public IEnumerator<ArchiveItemInfo> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        object IEnumerator.Current => Current;

        public bool MoveNext()

        {

            #region Old

            // if (FileTypes == FileTypes.None) return;

            //else if (FileTypes.HasFlag(GetAllEnumFlags<FileTypes>()) && FileTypes.HasMultipleFlags())

            //    throw new InvalidOperationException("FileTypes cannot have the All flag in combination with other flags.");

            //        #if DEBUG

            //                                // Debug.WriteLine("Dowork event started.");

            //                                // Debug.WriteLine(FileTypes);

            //                                try
            //                                {

            //                                    Debug.WriteLine("Path == null: " + (Path == null).ToString());

            //                                    Debug.WriteLine("Path: " + Path);

            //                                    Debug.WriteLine("ShellObject: " + ArchiveShellObject.ToString());

            //                                }
            //        #pragma warning disable CA1031 // Do not catch general exception types
            //                                catch (Exception) { }
            //#pragma warning restore CA1031 // Do not catch general exception types

            //#endif

            //#if DEBUG

            //Debug.WriteLine("Dowork event started.");

            //#endif

            //List<FolderLoader.IPathInfo> directories = new List<FolderLoader.IPathInfo>();

            //List<FolderLoader.IPathInfo> files = new List<FolderLoader.IPathInfo>();

            // var paths = new ArrayBuilder<ArchiveItemInfo>();

            //#if DEBUG

            //Debug.WriteLine("Path == null: " + (Path == null).ToString());

            //                                Debug.WriteLine("Path: " + Path);

            //                                Debug.WriteLine("ShellObject: " + ArchiveShellObject.ToString());

            //        #endif

            // ShellObjectInfo archiveShellObject = Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject;

            #endregion

            try

            {

#if NETFRAMEWORK

                using (var archiveExtractor = new SevenZipExtractor(_archiveItemInfoProvider.ArchiveShellObject.ArchiveFileStream))

                {

#else

                using var archiveExtractor = new SevenZipExtractor(_archiveItemInfoProvider.ArchiveShellObject.ArchiveFileStream);

#endif

                //try
                //{

                // archiveShellObject.ArchiveFileStream = archiveFileStream;
                void AddArchiveFileInfo(in ArchiveFileInfo _archiveFileInfo) =>

                    //if (fileType == FileType.Other || (FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

                    // // We only make a normalized path if we add the path to the paths to load.

                    // string __path = string.Copy(_path);

                    _current = ArchiveItemInfo.From(_archiveItemInfoProvider.ArchiveShellObject, _archiveFileInfo);

                void AddPath(in string archiveFilePath) =>

                    _current = ArchiveItemInfo.From(_archiveItemInfoProvider.ArchiveShellObject, archiveFilePath);

                //void AddDirectory(string _path, ArchiveFileInfo? archiveFileInfo) =>

                // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                //AddPath(ref _path, FileType.Folder, ref archiveFileInfo);

                //void AddFile(string _path, ArchiveFileInfo? archiveFileInfo) =>

                // We only make a normalized path if we add the path to the paths to load.

                //AddPath(ref _path, _path.Substring(_path.Length).EndsWith(".lnk")
                //    ? FileType.Link
                //    : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(_path)) ? FileType.Archive : FileType.File, ref archiveFileInfo);

                System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

                string fileName;

                string relativePath = _archiveItemInfoProvider.Path.Substring(_archiveItemInfoProvider.ArchiveShellObject.Path.Length + 1);

                // PathInfo path;

                //#if DEBUG

                //                foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                //                    Debug.WriteLine(archiveFileInfo.FileName);

                //#endif

                bool addPath(ArchiveFileInfoEnumeratorStruct archiveFileInfoEnumeratorStruct)

                {

                    if (Func is object && !Func(archiveFileInfoEnumeratorStruct))

                        return false;

                    foreach (IFileSystemObject pathInfo in _paths)

                        if (pathInfo.Path == fileName)

                            return false;

                    return true;

                }

                ArchiveFileInfo archiveFileInfo;

                for (int i = _index; i < archiveFileData.Count; i++)

                {
                    archiveFileInfo = archiveFileData[i];

                    // _path = archiveFileInfo.FileName.Replace('/', IO.Path.PathSeparator);

                    if (archiveFileInfo.FileName.StartsWith(relativePath, StringComparison.OrdinalIgnoreCase) && archiveFileInfo.FileName.Length > relativePath.Length)

                    {

                        fileName = archiveFileInfo.FileName.Substring(relativePath.Length);

                        if (fileName.StartsWith(WinCopies.IO.Path.PathSeparator))

                            fileName = fileName.Substring(1);

                        if (fileName.Contains(WinCopies.IO.Path.PathSeparator, StringComparison.OrdinalIgnoreCase))

                            fileName = fileName.Substring(0, fileName.IndexOf(WinCopies.IO.Path.PathSeparator
#if !NETFRAMEWORK
                                , StringComparison.OrdinalIgnoreCase
#endif
                                ));

                        /*if (!archiveFileInfo.FileName.Substring(archiveFileInfo.FileName.Length).Contains(IO.Path.PathSeparator))*/

                        // {

                        Action addValue;

                        ArchiveFileInfoEnumeratorStruct archiveFileInfoEnumeratorStruct;

                        if (fileName.ToUpperInvariant() == archiveFileInfo.FileName.ToUpperInvariant())

                        {

                            archiveFileInfoEnumeratorStruct = new ArchiveFileInfoEnumeratorStruct(archiveFileInfo);

                            addValue = () => AddArchiveFileInfo(archiveFileInfoEnumeratorStruct.ArchiveFileInfo.Value);

                        }

                        else

                        {

                            archiveFileInfoEnumeratorStruct = new ArchiveFileInfoEnumeratorStruct(fileName);

                            addValue = () => AddPath(archiveFileInfoEnumeratorStruct.Path);

                        }

                        if (addPath(archiveFileInfoEnumeratorStruct))
                        {

                            //if (archiveFileInfo.IsDirectory)

                            //    AddDirectory(fileName, archiveFileInfo);

                            //else /*if (CheckFilter(archiveFileInfo.FileName))*/

                            //    AddFile(fileName, archiveFileInfo);

                            addValue();

                            _index = i;

                            _paths.Enqueue(Current);

                            return true;

                        }

                        // }

                    }

                }

#if NETFRAMEWORK

                        }

#endif

            }

            catch (Exception ex) when (ex.Is(false, typeof(IOException), typeof(SecurityException), typeof(UnauthorizedAccessException), typeof(SevenZipException))) { }

            return false;

        }

        public void Reset()
        {
            _index = -1;

            _current = null;

            _paths.Clear();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public void Dispose()
        {
            if (!disposedValue)
            {
                Reset();

                _archiveItemInfoProvider = null;

                _paths = null;

                disposedValue = true;
            }
        }
        #endregion

        //private void Load()

        //{



        // for (int i = 0; i < paths.Count; i++)

        // {

        // PathInfo directory = (PathInfo)paths[i];

        // string CurrentFile_Normalized = "";

        // CurrentFile_Normalized = Util.GetNormalizedPath(directory.Path);

        // directory.Normalized_Path = CurrentFile_Normalized;

        // paths[i] = directory;

        // }

        // IEnumerable<PathInfo> pathInfos;

        //if (FileSystemObjectComparer == null)

        //    pathInfos = (IEnumerable<PathInfo>)paths;

        //else

        //{

        //    var sortedPaths = paths.ToList();

        //    sortedPaths.Sort(FileSystemObjectComparer);

        //    pathInfos = (IEnumerable<PathInfo>)paths;

        //}

        // pathInfos = paths;

        // for (int i = 0; i < files.Count; i++)

        // {

        // var file = (PathInfo)files[i];

        // string CurrentFile_Normalized = "";

        // CurrentFile_Normalized = FolderLoader.PathInfo.NormalizePath(file.Path);

        // file.Normalized_Path = CurrentFile_Normalized;

        // files[i] = file;

        // }

        // files.Sort(comp);
    }

    [Serializable]
    public sealed class WMIItemInfoEnumerator : IEnumerator<WMIItemInfo>, IEnumerable<WMIItemInfo>
    {
        private IEnumerator<ManagementBaseObject> _enumerator;

        private bool _resetInnerEnumerator = false;

        private WMIItemInfo _current;

        public WMIItemInfo Current { get { if (_disposedValue) throw GetExceptionForDispose(false); return _current; } }

        object IEnumerator.Current => Current;

        private Func<bool> Func { get; }

        public WMIItemType ItemWMIItemType { get; }

        public WMIItemInfoEnumerator(IEnumerable<ManagementBaseObject> enumerator, bool resetEnumerator, WMIItemType itemWMIItemType, bool catchExceptions)
        {
            _enumerator = enumerator.GetEnumerator();

            _resetInnerEnumerator = resetEnumerator;

            ItemWMIItemType = itemWMIItemType;

            if (catchExceptions)

                Func = () =>
                  {
                      while (_enumerator.MoveNext())
                          try
                          {
                              _MoveNext();

                              return true;
                          }
                          catch (Exception) { }

                      return false;
                  };

            else

                Func = () =>
                  {
                      if (_enumerator.MoveNext())
                      {
                          _MoveNext();

                          return true;
                      }

                      return false;
                  };
        }

        public IEnumerator<WMIItemInfo> GetEnumerator() => this;

        private void _MoveNext()
        {
            ManagementBaseObject current = _enumerator.Current;

            // if (CheckFilter(_path))

            _current = new WMIItemInfo(ItemWMIItemType, current);
        }

        public bool MoveNext() => Func();

        public void Reset()
        {
            _current = null;

            if (_resetInnerEnumerator)

                _enumerator.Reset();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        public void Dispose()
        {
            if (!_disposedValue)
            {
                Reset();

                _enumerator = null;

                _disposedValue = true;
            }
        }
        #endregion
    }

    [Serializable]
    public sealed class FileSystemEntryEnumerable : IEnumerable<IPathInfo>, IEnumerator<IPathInfo>
    {
        private IEnumerator<string> _directoryEnumerator;

        private IEnumerator<string> _fileEnumerator;

        private bool _completed = false;

        public IPathInfo Current { get; private set; }

        object IEnumerator.Current => Current;

        public FileSystemEntryEnumerable(IEnumerator<string> directoryEnumerator, IEnumerator<string> fileEnumerator)
        {
            _directoryEnumerator = directoryEnumerator;

            _fileEnumerator = fileEnumerator;
        }

        public IEnumerator<IPathInfo> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            if (_completed) return false;

            if (_fileEnumerator == null)
            {
                if (_directoryEnumerator.MoveNext())
                {
                    Current = new PathInfo(_directoryEnumerator.Current, true);

                    return true;
                }

                _directoryEnumerator = null;

                _completed = true;

                return false;
            }

            if (_fileEnumerator.MoveNext())
            {
                Current = new PathInfo(_fileEnumerator.Current, false);

                return true;
            }

            _fileEnumerator = null;

            _completed = true;

            return false;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose()
        {
            if (_completed)
            {
                Current = null;

                return;
            }

            if (_directoryEnumerator != null)
            {
                _directoryEnumerator.Dispose();

                _directoryEnumerator = null;
            }

            if (_fileEnumerator != null)
            {
                _fileEnumerator.Dispose();

                _fileEnumerator = null;
            }

            Current = null;
        }
    }

    [Serializable]
    public sealed class FileSystemEntryEnumerator : IEnumerable<IPathInfo>, IEnumerator<IPathInfo>
    {
        private IEnumerator<IPathInfo> _paths;

        private Stack<FileSystemEntryEnumerable> _stack;

        private Queue<IPathInfo> _directories;

        private Queue<IPathInfo> _files;

        public IPathInfo Current { get; private set; }

        object IEnumerator.Current => Current;

        public FileSystemEntryEnumerator(IEnumerable<IPathInfo> paths, string searchPattern, SearchOption? searchOption
#if NETCORE
            , System.IO.EnumerationOptions enumerationOptions
#endif
            )
        {
            _paths = paths.GetEnumerator();

            if (string.IsNullOrEmpty(searchPattern) && searchOption == null
#if NETCORE
                     && enumerationOptions == null
#endif
                    )
            {
                _enumerateDirectoriesFunc = _path => System.IO.Directory.EnumerateDirectories(_path);

                _enumerateFilesFunc = _path => System.IO.Directory.EnumerateFiles(_path);
            }

            else if (searchPattern != null && searchOption == null
#if NETCORE
                    && enumerationOptions == null
#endif
                    )
            {
                _enumerateDirectoriesFunc = _path => System.IO.Directory.EnumerateDirectories(_path, searchPattern);

                _enumerateFilesFunc = _path => System.IO.Directory.EnumerateFiles(_path, searchPattern);
            }

#if NETCORE
            else if (searchOption == null)
            {
                if (searchPattern == null)

                    searchPattern = "";

                _enumerateDirectoriesFunc = path => System.IO.Directory.EnumerateDirectories(path, searchPattern, enumerationOptions);

                _enumerateFilesFunc = path => System.IO.Directory.EnumerateFiles(path, searchPattern, enumerationOptions);
            }
#endif

            else
            {
                if (searchPattern == null)

                    searchPattern = "";

                _enumerateDirectoriesFunc = _path => System.IO.Directory.EnumerateDirectories(_path, searchPattern, searchOption.Value);

                _enumerateFilesFunc = _path => System.IO.Directory.EnumerateFiles(_path, searchPattern, searchOption.Value);
            }
        }

        public void Dispose() { }

        public IEnumerator<IPathInfo> GetEnumerator() => this;

        private bool _firstLaunch = true;
        private bool _completed = false;
        private readonly Func<string, IEnumerable<string>> _enumerateDirectoriesFunc;
        private readonly Func<string, IEnumerable<string>> _enumerateFilesFunc;

        private void _markAsCompleted()
        {
            _stack = null;

            _completed = true;
        }

        public bool MoveNext()
        {
            if (_completed)
            {
                Current = null;

                return false;
            }

            bool dequeueDirectory()
            {

                FileSystemEntryEnumerable enumerator;

                void push() => _stack.Push(new FileSystemEntryEnumerable(_enumerateDirectoriesFunc(Current.Path).GetEnumerator(), _enumerateFilesFunc(Current.Path).GetEnumerator()));

                while (true)
                {

                    if (_stack.Count == 0)

                    {
                        if (_directories.Count == 0)
                        {
                            _directories = null;

                            _markAsCompleted();

                            return false;
                        }

                        Current = _directories.Dequeue();

                        push();

                        if (_directories.Count == 0)

                            _directories = null;

                        return true;
                    }

                    enumerator = _stack.Peek();

                    if (enumerator.MoveNext())
                    {
                        Current = enumerator.Current;

                        if (Current.IsDirectory)

                            push();

                        return true;
                    }

                    _ = _stack.Pop();

                }

            }

            if (_firstLaunch)
            {
                _firstLaunch = false;

                _directories = new Queue<IPathInfo>();

                _files = new Queue<IPathInfo>();

                IPathInfo path;

                while (_paths.MoveNext())
                {
                    path = _paths.Current;

                    (path.IsDirectory ? _directories : _files).Enqueue(path);
                }

                _paths = null;

                if (_files.Count == 0)
                {
                    _files = null;

                    if (_directories.Count == 0)
                    {
                        _directories = null;

                        _markAsCompleted();

                        return false;
                    }

                    _ = dequeueDirectory();

                    return true;
                }

                if (_directories.Count == 0)

                    _directories = null;

                Current = _files.Dequeue();

                if (_files.Count == 0)

                    _files = null;

                return true;
            }

            if (_files == null)
            {
                if (_directories == null && _stack.Count == 0)
                {
                    _markAsCompleted();

                    return false;
                }

                if (dequeueDirectory())

                    return true;

                _markAsCompleted();

                return false;
            }

            Current = _files.Dequeue();

            if (_files.Count == 0)

                _files = null;

            return true;
        }

        public void Reset() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
