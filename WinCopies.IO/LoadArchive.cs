#region "Imports"



#region ".NET"

using SevenZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

#endregion

#region "WinCopies"

// using WinCopies.IO;

#endregion

#endregion

namespace WinCopies.IO
{

    public class LoadArchive : BrowsableObjectInfoItemsLoader, INotifyPropertyChanged
    {

        private static Dictionary<InArchiveFormat, string[]> dic = new Dictionary<InArchiveFormat, string[]>();

        public static ReadOnlyDictionary<InArchiveFormat, string[]> InArchiveFormats { get; }

        // public new event PropertyChangedEventHandler PropertyChanged;

        static LoadArchive()

        {

            // todo: to add the other 'in' archive formats

            dic.Add(InArchiveFormat.Zip, new string[] { ".zip" });

            dic.Add(InArchiveFormat.SevenZip, new string[] { ".7z" });

            dic.Add(InArchiveFormat.Arj, new string[] { ".arj" });

            dic.Add(InArchiveFormat.BZip2, new string[] { ".bz2" });

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
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        public LoadArchive(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes) : base(workerReportsProgress, workerSupportsCancellation, fileTypes) { }

        protected override void Init()

        {

            // if ((path is ShellObjectInfo && ((ShellObjectInfo)path).FileType == FileTypes.Archive) || path is ArchiveItemInfo)

            // {

            // this.path = path;

            // PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(nameof(Path), null, path));

            // }

            IBrowsableObjectInfo _path = Path as ShellObjectInfo;

            if (_path == null)

                _path = Path as ArchiveItemInfo;

            if (_path == null)

                throw new ArgumentException("'Path' is null or isn't a ShellObjectInfo or an ArchiveItemInfo.");

            else if (_path.FileType != IO.FileType.Archive)

                throw new ArgumentException("'Path' isn't an Archive.");

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

        protected override void OnDoWork(object sender, DoWorkEventArgs e)
        {

#if DEBUG

            Debug.WriteLine("Dowork event started.");

#endif

            //List<LoadFolder.IPathInfo> directories = new List<LoadFolder.IPathInfo>();

            //List<LoadFolder.IPathInfo> files = new List<LoadFolder.IPathInfo>();

            List<IFileSystemObject> paths = new List<IFileSystemObject>();

            LoadFolder.comp comp = LoadFolder.comp.GetInstance();

#if DEBUG

            Debug.WriteLine("Path == null: " + (Path == null).ToString());

            Debug.WriteLine("Path.Path: " + Path.Path);

            if (Path is ShellObjectInfo) Debug.WriteLine("Path.ShellObject: " + ((ShellObjectInfo)Path).ShellObject.ToString());

#endif

            ShellObjectInfo archiveShellObject = Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject;

            string archiveFileName = archiveShellObject.Path;

            using (FileStream archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {

                //try
                //{

                // archiveShellObject.ArchiveFileStream = archiveFileStream;

                using (SevenZipExtractor archiveExtractor = new SevenZipExtractor(archiveFileStream))
                {

                    void AddPath(ref PathInfo pathInfo)

                    {

                        if (pathInfo.FileType == IO.FileType.None || (FileTypes != FileTypesFlags.All && !FileTypes.HasFlag(FileTypeToFileTypeFlags(pathInfo.FileType)))) return;

                        // We only make a normalized path if we add the path to the paths to load.

                        pathInfo.Normalized_Path = IO.Path.GetNormalizedPath(pathInfo.Path);

                        paths.Add(pathInfo);

                    }

                    void AddDirectory(PathInfo pathInfo)

                    {

                        // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                        pathInfo.FileType = IO.FileType.Folder;

                        AddPath(ref pathInfo);

                    }

                    void AddFile(PathInfo pathInfo, bool isLink)

                    {

                        pathInfo.FileType = isLink
                            ? IO.FileType.Link
                            : IsSupportedArchiveFormat(System.IO.Path.GetExtension(pathInfo.Path)) ? IO.FileType.Archive : IO.FileType.File;

                        // We only make a normalized path if we add the path to the paths to load.

                        AddPath(ref pathInfo);

                    }

                    ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

                    string _path = "";

                    string fileName = "";

                    string relativePath = "";

                    foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                    {

                        _path = archiveFileInfo.FileName.Replace('/', '\\');

                        if (_path.StartsWith(relativePath) && _path.Length > relativePath.Length)

                        {

                            fileName = _path.Substring(_path.Length);

                            if (!fileName.Contains("\\"))

                            {

                                if (CheckFilter(_path))

                                {

                                    PathInfo path = new PathInfo() { Path = _path };

                                    if (archiveFileInfo.IsDirectory)

                                        AddDirectory(path);

                                    else

                                        AddFile(path, fileName.EndsWith(".lnk"));

                                }

                            }

                        }

                    }

                    if (Path is ArchiveItemInfo)

                    {

                        if (relativePath != "")

                            relativePath = "\\";

                        relativePath += ((ArchiveItemInfo)Path).ArchiveItemRelativePath.Replace('/', '\\');

                    }

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

            // for (int i = 0; i < paths.Count; i++)

            // {

            // PathInfo directory = (PathInfo)paths[i];

            // string CurrentFile_Normalized = "";

            // CurrentFile_Normalized = Util.GetNormalizedPath(directory.Path);

            // directory.Normalized_Path = CurrentFile_Normalized;

            // paths[i] = directory;

            // }

            paths.Sort(comp);

            // for (int i = 0; i < files.Count; i++)

            // {

            // var file = (PathInfo)files[i];

            // string CurrentFile_Normalized = "";

            // CurrentFile_Normalized = LoadFolder.PathInfo.NormalizePath(file.Path);

            // file.Normalized_Path = CurrentFile_Normalized;

            // files[i] = file;

            // }

            // files.Sort(comp);



            void reportProgressAndAddNewPathToObservableCollection(PathInfo path)

            {

#if DEBUG

                Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
                Debug.WriteLine("path_.Path: " + path.Path);
                Debug.WriteLine("path_.Normalized_Path: " + path.Normalized_Path);
                // Debug.WriteLine("path_.Shell_Object: " + path.ArchiveShellObject);

#endif

                // var new_Path = ((ArchiveItemInfo)Path).ArchiveShellObject;
                // new_Path.LoadThumbnail();

                ReportProgress(0, OnAddingNewBrowsableObjectInfo(path));

                // #if DEBUG

                // Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

                // #endif

            }

            // this._Paths = new ObservableCollection<IBrowsableObjectInfo>();

            foreach (PathInfo path_ in paths)

                reportProgressAndAddNewPathToObservableCollection(path_);

            //foreach (LoadFolder.PathInfo path_ in files)

            //    reportProgressAndAddNewPathToObservableCollection(path_);
            
        }

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(PathInfo path) =>

            ((IArchiveItemInfoProvider)Path).GetBrowsableObjectInfo(((IArchiveItemInfoProvider)Path).ArchiveShellObject, path.ArchiveFileInfo, path.Path, Path.Path, path.FileType);

        // todo: really needed? :

        public struct PathInfo : IFileSystemObject

        {

            /// <summary>
            /// Gets the path of this <see cref="PathInfo"/>.
            /// </summary>
            public string Path { get; set; }

            public string Normalized_Path { get; set; }

            public ArchiveFileInfo ArchiveFileInfo { get; set; }

            public string LocalizedPath => Path;

            public string Name { get; set; }

            /// <summary>
            /// Gets the file type of this <see cref="PathInfo"/>.
            /// </summary>
            public FileType FileType { get; set; }

        }

    }

}
