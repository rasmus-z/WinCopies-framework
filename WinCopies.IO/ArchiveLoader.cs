using SevenZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using WinCopies.Util;
using static WinCopies.Util.Util;
using static WinCopies.IO.FolderLoader;

namespace WinCopies.IO
{

    // todo: does not work with encrypted archives

    public class ArchiveLoader : FileSystemObjectLoader<ArchiveItemInfoProvider>
    {

        protected override BrowsableObjectInfoLoader<ArchiveItemInfoProvider> DeepCloneOverride(bool preserveIds) => new ArchiveLoader(null, FileTypes, WorkerReportsProgress, WorkerSupportsCancellation, (IFileSystemObjectComparer<IArchiveItemInfo>)FileSystemObjectComparer.DeepClone(preserveIds));

        private static readonly Dictionary<InArchiveFormat, string[]> dic = new Dictionary<InArchiveFormat, string[]>();

        public static ReadOnlyDictionary<InArchiveFormat, string[]> InArchiveFormats { get; }

        // public new event PropertyChangedEventHandler PropertyChanged;

        static ArchiveLoader()

        {

            // todo: to add the other 'in' archive formats

            dic.Add(InArchiveFormat.Zip, new string[] { ".zip" });

            dic.Add(InArchiveFormat.SevenZip, new string[] { ".7z" });

            dic.Add(InArchiveFormat.Arj, new string[] { ".arj" });

            dic.Add(InArchiveFormat.BZip2, new string[] { ".bz2", ".tar", ".xz" });

            dic.Add(InArchiveFormat.Cab, new string[] { ".cab" });

            dic.Add(InArchiveFormat.Chm, new string[] { ".chm" });

            dic.Add(InArchiveFormat.Compound, new string[] { ".cfb" });

            dic.Add(InArchiveFormat.Cpio, new string[] { ".cpio" });

            dic.Add(InArchiveFormat.CramFS, null);

            dic.Add(InArchiveFormat.Deb, new string[] { ".deb", ".udeb" });

            dic.Add(InArchiveFormat.Dmg, new string[] { ".dmg" });

            dic.Add(InArchiveFormat.Elf, new string[] { ".axf", ".bin", ".elf", ".o", ".prx", ".puff", ".ko", ".mod", ".so" });

            dic.Add(InArchiveFormat.Fat, null);

            dic.Add(InArchiveFormat.Flv, new string[] { ".flv" });

            dic.Add(InArchiveFormat.GZip, new string[] { ".gz" });

            dic.Add(InArchiveFormat.Hfs, new string[] { ".hfs" });

            dic.Add(InArchiveFormat.Iso, new string[] { ".iso" });

            dic.Add(InArchiveFormat.Lzh, new string[] { ".lzh" });

            dic.Add(InArchiveFormat.Lzma, new string[] { "lzma" });

            dic.Add(InArchiveFormat.Lzma86, new string[] { ".lzma86" });

            dic.Add(InArchiveFormat.Lzw, new string[] { ".lzw" });

            dic.Add(InArchiveFormat.MachO, new string[] { ".o", ".dylib", ".bundle" });

            dic.Add(InArchiveFormat.Mbr, new string[] { ".mbr" });

            dic.Add(InArchiveFormat.Msi, new string[] { ".msi", ".msp" });

            dic.Add(InArchiveFormat.Mslz, new string[] { ".mslz" });

            dic.Add(InArchiveFormat.Mub, new string[] { ".mub" });

            dic.Add(InArchiveFormat.Nsis, new string[] { ".exe" });

            dic.Add(InArchiveFormat.Ntfs, null);

            dic.Add(InArchiveFormat.PE, new string[] { ".dll", ".ocx", ".sys", ".scr", ".drv", ".efi" });

            dic.Add(InArchiveFormat.Ppmd);

            dic.Add(InArchiveFormat.Rar, null);

            dic.Add(InArchiveFormat.Rar4, null);

            dic.Add(InArchiveFormat.Rpm, null);

            dic.Add(InArchiveFormat.Split, null);

            dic.Add(InArchiveFormat.SquashFS, null);

            dic.Add(InArchiveFormat.Swf, null);

            dic.Add(InArchiveFormat.Swfc, null);

            dic.Add(InArchiveFormat.Tar, null);

            dic.Add(InArchiveFormat.TE, null);

            dic.Add(InArchiveFormat.Udf, null);

            dic.Add(InArchiveFormat.UEFIc, null);

            dic.Add(InArchiveFormat.UEFIs, null);

            dic.Add(InArchiveFormat.Vhd, null);

            dic.Add(InArchiveFormat.Wim, null);

            dic.Add(InArchiveFormat.Xar, null);

            dic.Add(InArchiveFormat.XZ, null);

            InArchiveFormats = new ReadOnlyDictionary<InArchiveFormat, string[]>(dic);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public ArchiveLoader(ArchiveItemInfoProvider path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IArchiveItemInfo>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public ArchiveLoader(ArchiveItemInfoProvider path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IArchiveItemInfo> fileSystemObjectComparer) : base(path, fileTypes, workerReportsProgress, workerSupportsCancellation, (IFileSystemObjectComparer<IFileSystemObject>) fileSystemObjectComparer) { }

        protected override void OnPathChanging(ArchiveItemInfoProvider path)

        {

            // if ((path is ShellObjectInfo && ((ShellObjectInfo)path).FileType == FileTypes.Archive) || path is ArchiveItemInfo)

            // {

            // this.path = path;

            // PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(nameof(Path), null, path));

            // }

            /*else*/
            if (path.FileType != FileType.Archive)

                throw new ArgumentException("'Path' is not an Archive or a Folder.");

            // _Paths = new ObservableCollection<IBrowsableObjectInfo>();

        }

        // protected override void OnProgressChanged(object sender, ProgressChangedEventArgs e) => PathsOverride.Add((ArchiveItemInfo)e.UserState);

        protected override void OnDoWork(DoWorkEventArgs e)

        {

            if (FileTypes == FileTypes.None) return;

            else if (FileTypes.HasFlag(GetAllEnumFlags<FileTypes>()) && FileTypes.HasMultipleFlags())

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
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) { }
#pragma warning restore CA1031 // Do not catch general exception types

#endif

#if DEBUG

            Debug.WriteLine("Dowork event started.");

#endif

            //List<FolderLoader.IPathInfo> directories = new List<FolderLoader.IPathInfo>();

            //List<FolderLoader.IPathInfo> files = new List<FolderLoader.IPathInfo>();

            var paths = new ArrayAndListBuilder<IFileSystemObject>();

#if DEBUG

            Debug.WriteLine("Path == null: " + (Path == null).ToString());

            Debug.WriteLine("Path.Path: " + Path.Path);

            if (Path is ShellObjectInfo) Debug.WriteLine("Path.ShellObject: " + ((ShellObjectInfo)Path).ShellObject.ToString());

#endif

            // ShellObjectInfo archiveShellObject = Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject;

            string archiveFileName = (Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject).Path;

            try

            {

                using (var archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {

                    //try
                    //{

                    // archiveShellObject.ArchiveFileStream = archiveFileStream;

                    using (var archiveExtractor = new SevenZipExtractor(archiveFileStream))
                    {

                        void AddPath(ref string _path, FileType fileType, ref ArchiveFileInfo? archiveFileInfo)

                        {

                            if (fileType == FileType.Other || (FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

                            // We only make a normalized path if we add the path to the paths to load.

                            paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), fileType, archiveFileInfo));

                        }

                        void AddDirectory(string _path, ArchiveFileInfo? archiveFileInfo) =>

                            // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                            AddPath(ref _path, FileType.Folder, ref archiveFileInfo);

                        void AddFile(string _path, ArchiveFileInfo? archiveFileInfo) =>

                            // We only make a normalized path if we add the path to the paths to load.

                            AddPath(ref _path, _path.Substring(_path.Length).EndsWith(".lnk")
                                ? FileType.Link
                                : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(_path)) ? FileType.Archive : FileType.File, ref archiveFileInfo);

                        ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

                        string fileName = "";

                        string relativePath = Path is ShellObjectInfo ? "" : Path.Path.Substring(archiveFileName.Length + 1);

                        PathInfo path;

#if DEBUG

                        foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                            Debug.WriteLine(archiveFileInfo.FileName);

#endif

                        void addPath(ArchiveFileInfo archiveFileInfo)

                        {

                            if (archiveFileInfo.FileName.StartsWith(relativePath) && archiveFileInfo.FileName.Length > relativePath.Length)

                            {

                                fileName = archiveFileInfo.FileName.Substring(relativePath.Length);

                                if (fileName.StartsWith("\\"))

                                    fileName = fileName.Substring(1);

                                if (fileName.Contains("\\"))

                                    fileName = fileName.Substring(0, fileName.IndexOf("\\"));

                                /*if (!archiveFileInfo.FileName.Substring(archiveFileInfo.FileName.Length).Contains("\\"))*/

                                // {

                                foreach (IFileSystemObject pathInfo in paths)

                                    if (pathInfo.Path == fileName)

                                        return;

                                if (fileName.ToLower() == archiveFileInfo.FileName.ToLower())

                                {

                                    if (archiveFileInfo.IsDirectory)

                                        AddDirectory(fileName, archiveFileInfo);

                                    else if (CheckFilter(archiveFileInfo.FileName))

                                        AddFile(fileName, archiveFileInfo);

                                }

                                else

                                    AddDirectory(fileName, archiveFileInfo);

                                // }

                            }

                        }

                        foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                            // _path = archiveFileInfo.FileName.Replace('/', '\\');

                            addPath(archiveFileInfo);

                        //if (Path is ArchiveItemInfo)

                        //{

                        //    if (relativePath != "")

                        //        relativePath = "\\";

                        //    relativePath += ((ArchiveItemInfo)Path).Path/*.Replace('/', '\\')*/;

                        //}

#if DEBUG

                        Debug.WriteLine(relativePath);

#endif

                    }

                    //}

                    //catch (Exception)

                    //{

                    //    paths = null;

                    //    return;

                    //}
                }

            }
            catch (Exception ex) when (ex.Is(false, typeof(IOException), typeof(SecurityException), typeof(UnauthorizedAccessException), typeof(SevenZipException))) { return; }

            // for (int i = 0; i < paths.Count; i++)

            // {

            // PathInfo directory = (PathInfo)paths[i];

            // string CurrentFile_Normalized = "";

            // CurrentFile_Normalized = Util.GetNormalizedPath(directory.Path);

            // directory.Normalized_Path = CurrentFile_Normalized;

            // paths[i] = directory;

            // }

            IEnumerable<PathInfo> pathInfos;

            if (FileSystemObjectComparer == null)

                pathInfos = (IEnumerable<PathInfo>)paths;

            else

            {

                var sortedPaths = paths.ToList();

                sortedPaths.Sort(FileSystemObjectComparer);

                pathInfos = (IEnumerable<PathInfo>)paths;

            }

            // for (int i = 0; i < files.Count; i++)

            // {

            // var file = (PathInfo)files[i];

            // string CurrentFile_Normalized = "";

            // CurrentFile_Normalized = FolderLoader.PathInfo.NormalizePath(file.Path);

            // file.Normalized_Path = CurrentFile_Normalized;

            // files[i] = file;

            // }

            // files.Sort(comp);



#if DEBUG

            void reportProgress(PathInfo path)

            {

                Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
                Debug.WriteLine("path_.Path: " + path.Path);
                Debug.WriteLine("path_.Normalized_Path: " + path.NormalizedPath);
                // Debug.WriteLine("path_.Shell_Object: " + path.ArchiveShellObject);

                // var new_Path = ((ArchiveItemInfo)Path).ArchiveShellObject;
                // new_Path.LoadThumbnail();

                ReportProgress(0, Path.ArchiveItemInfoFactory.GetBrowsableObjectInfo(Path.Path + "\\" + path.Path, path.FileType, Path.ArchiveShellObject, () => path.ArchiveFileInfo));

                // #if DEBUG

                // Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

                // #endif

            }

#endif

            // this._Paths = new ObservableCollection<IBrowsableObjectInfo>();



            PathInfo path_;



            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())

                while (_paths.MoveNext())

                    try

                    {

                        do

                        {

                            path_ = _paths.Current;

#if DEBUG

                            reportProgress(path_);

#else

                ReportProgress(0, ((IArchiveItemInfoProvider)Path).Factory.GetBrowsableObjectInfo(((IArchiveItemInfoProvider)Path).ArchiveShellObject, path.ArchiveFileInfo, Path.Path + "\\" + path.Path, path.FileType));

#endif

                        } while (_paths.MoveNext());

                    }
                    catch (Exception ex) when (HandleIOException(ex)) { }

            //foreach (FolderLoader.PathInfo path_ in files)

            //    reportProgressAndAddNewPathToObservableCollection(path_);

        }

        // todo: really needed? :

        protected class PathInfo : IO.PathInfo

        {

            public ArchiveFileInfo? ArchiveFileInfo { get; }

            public override string LocalizedName => Name;

            public override string Name => System.IO.Path.GetFileName(Path);

            public PathInfo(string path, string normalizedPath, FileType fileType, ArchiveFileInfo? archiveFileInfo) : base(path, normalizedPath, fileType) => ArchiveFileInfo = archiveFileInfo;

            //public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
            //        ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
            //        : false;

            //public int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

        }

    }

}
