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

    public class ArchiveLoader : FileSystemObjectLoader<ArchiveItemInfoProvider>
    {

        private static Dictionary<InArchiveFormat, string[]> dic = new Dictionary<InArchiveFormat, string[]>();

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

            dic.Add(InArchiveFormat.Elf, new string[] { "", ".axf", ".bin", ".elf", ".o", ".prx", ".puff", ".ko", ".mod", ".so" });

            dic.Add(InArchiveFormat.Fat, null);

            dic.Add(InArchiveFormat.Flv, new string[] { ".flv" });

            dic.Add(InArchiveFormat.GZip, new string[] { ".gz" });

            dic.Add(InArchiveFormat.Hfs, new string[] { ".hfs" });

            dic.Add(InArchiveFormat.Iso, new string[] { ".iso" });

            dic.Add(InArchiveFormat.Lzh, new string[] { ".lzh" });

            dic.Add(InArchiveFormat.Lzma, new string[] { "lzma" });

            dic.Add(InArchiveFormat.Lzma86, null);

            dic.Add(InArchiveFormat.Lzw, new string[] { ".lzw" });

            dic.Add(InArchiveFormat.MachO, new string[] { "", ".o", ".dylib", ".bundle" });

            InArchiveFormats = new ReadOnlyDictionary<InArchiveFormat, string[]>(dic);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public ArchiveLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypes fileTypes) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), fileTypes) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public ArchiveLoader(bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> fileSystemObjectComparer, FileTypes fileTypes) : base(workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer, fileTypes) { }

        protected override void OnPathChanging(BrowsableObjectInfo path)

        {

            // if ((path is ShellObjectInfo && ((ShellObjectInfo)path).FileType == FileTypes.Archive) || path is ArchiveItemInfo)

            // {

            // this.path = path;

            // PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(nameof(Path), null, path));

            // }

            _ = GetOrThrowIfNotType<IArchiveItemInfoProvider>((IBrowsableObjectInfo)path, nameof(path));

            /*else*/
            if (path.FileType != FileType.Archive)

                throw new ArgumentException("'Path' is not an Archive or a Folder.");

            // _Paths = new ObservableCollection<IBrowsableObjectInfo>();

        }

        // protected override void OnProgressChanged(object sender, ProgressChangedEventArgs e) => PathsOverride.Add((ArchiveItemInfo)e.UserState);

        public static bool IsSupportedArchiveFormat(string extension)

        {

            foreach (KeyValuePair<InArchiveFormat, string[]> value in InArchiveFormats)

                if (value.Value != null)

                    foreach (string _extension in value.Value)

                        if (_extension == extension)

                            return true;

            return false;

        }

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
            catch (Exception) { }

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

                        void AddPath(ref PathInfo pathInfo)

                        {

                            if (pathInfo.FileType == FileType.Other || (FileTypes != Util.Util.GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(pathInfo.FileType)))) return;

                            // We only make a normalized path if we add the path to the paths to load.

                            pathInfo.NormalizedPath = pathInfo.Path.RemoveAccents();

                            paths.AddLast(pathInfo);

                        }

                        void AddDirectory(PathInfo pathInfo)

                        {

                            // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                            pathInfo.FileType = FileType.Folder;

                            AddPath(ref pathInfo);

                        }

                        void AddFile(PathInfo pathInfo)

                        {

                            pathInfo.FileType = pathInfo.Path.Substring(pathInfo.Path.Length).EndsWith(".lnk")
                                ? FileType.Link
                                : IsSupportedArchiveFormat(System.IO.Path.GetExtension(pathInfo.Path)) ? FileType.Archive : FileType.File;

                            // We only make a normalized path if we add the path to the paths to load.

                            AddPath(ref pathInfo);

                        }

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

                                path = new PathInfo() { Path = fileName };

                                if (fileName.ToLower() == archiveFileInfo.FileName.ToLower())

                                {

                                    path.ArchiveFileInfo = archiveFileInfo;

                                    if (archiveFileInfo.IsDirectory)

                                        AddDirectory(path);

                                    else if (CheckFilter(archiveFileInfo.FileName))

                                        AddFile(path);

                                }

                                else

                                    AddDirectory(path);

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

                ReportProgress(0, ((IArchiveItemInfoProvider)Path).Factory.GetBrowsableObjectInfo(((IArchiveItemInfoProvider)Path).ArchiveShellObject, path.ArchiveFileInfo, Path.Path + "\\" + path.Path, path.FileType));

                // #if DEBUG

                // Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

                // #endif

            }

#endif

            // this._Paths = new ObservableCollection<IBrowsableObjectInfo>();



            PathInfo path_;



            using (var _paths = pathInfos.GetEnumerator())

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

        public struct PathInfo : IFileSystemObject

        {

            /// <summary>
            /// Gets the path of this <see cref="PathInfo"/>.
            /// </summary>
            public string Path { get; set; }

            public string NormalizedPath { get; set; }

            public ArchiveFileInfo? ArchiveFileInfo { get; set; }

            public string LocalizedName => Path;

            public string Name { get; set; }

            /// <summary>
            /// Gets the file type of this <see cref="PathInfo"/>.
            /// </summary>
            public FileType FileType { get; set; }

        }

    }

}
