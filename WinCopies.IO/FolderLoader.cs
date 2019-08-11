using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Windows;
using WinCopies.Util;

using static WinCopies.Util.Util;
using IFCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;

namespace WinCopies.IO
{

    public interface IFolderLoader<TPath> : IFileSystemObjectLoader<TPath> where TPath : IShellObjectInfo
    {

        FolderLoaderFileSystemWatcher FileSystemWatcher { get; }

    }

    /// <summary>
    /// Provides a background process that can be used to load items of a folder. See the Remarks section.
    /// </summary>
    /// <remarks>
    /// This loader is not designed for <see cref="ShellObjectInfo"/> that have their <see cref="BrowsableObjectInfo.FileType"/> property set up with an other value than <see cref="FileType.Folder"/>, <see cref="FileType.Drive"/> or <see cref="FileType.SpecialFolder"/>, even if they can be browsable (e.g. <see cref="FileType.Archive"/>). If the file type of the given <see cref="BrowsableObjectInfoLoader{TPath}.Path"/> is not supported by this loader, you'll have to use a specific loader or to inherit from this loader.
    /// </remarks>
    public class FolderLoader : FileSystemObjectLoader<ShellObjectInfo>, IFolderLoader<ShellObjectInfo>
    {

        public override bool NeedsObjectsReconstruction => true;

        protected override BrowsableObjectInfoLoader<ShellObjectInfo> DeepCloneOverride(bool preserveIds) => new FolderLoader(null, FileTypes, WorkerReportsProgress, WorkerSupportsCancellation, (IFileSystemObjectComparer)FileSystemObjectComparer.DeepClone(preserveIds));

        // todo: to turn on ShellObjectWatcher for better compatibility

#pragma warning disable CS0649 // Set up using reflection
        private readonly FolderLoaderFileSystemWatcher _fileSystemWatcher;
#pragma warning restore CS0649

        public FolderLoaderFileSystemWatcher FileSystemWatcher
        {
            get => _fileSystemWatcher; private set
            {

                _fileSystemWatcher.Created += FileSystemWatcher_Created;

                _fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

                _fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;

                this.SetBackgroundWorkerProperty(nameof(FileSystemWatcher), nameof(_fileSystemWatcher), typeof(FolderLoader), true);

                if (value is null) return;

                value.Created += FileSystemWatcher_Created;

                value.Renamed += FileSystemWatcher_Renamed;

                value.Deleted += FileSystemWatcher_Deleted;

            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public FolderLoader(ShellObjectInfo path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, fileTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public FolderLoader(ShellObjectInfo path, FileTypes fileTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer fileSystemObjectComparer) : base(path, fileTypes, workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer) { }

        protected override void OnPathChanging(ShellObjectInfo path)
        {

            FileSystemWatcher.EnableRaisingEvents = false;

            FileSystemWatcher.Path = null;

            if (Path is null) return;

            if (If(IFCT.Or, IfCM.Logical, IfComp.NotEqual, path.FileType, FileType.Folder, FileType.Drive, FileType.SpecialFolder))

                throw new ArgumentException("'Path' isn't a folder, a drive or a special folder. 'Path': " + path.ToString());

            if ((Path.FileType == FileType.Folder || Path.FileType == FileType.SpecialFolder) && Path.ShellObject.IsFileSystemObject && FileSystemWatcher == null)

            {

                try
                {

                    FileSystemWatcher = GetFolderLoaderFileSystemWatcher();

                    if ((Path.FileType == FileType.Drive && new DriveInfo(Path.Path).IsReady) || (Path.FileType != FileType.Drive && Path.ShellObject.IsFileSystemObject))

                    {

                        FileSystemWatcher.Path = Path.Path;

                        FileSystemWatcher.EnableRaisingEvents = true;

                    }

                }

                catch { }

            }

        }

        /// <summary>
        /// <para>Gets the <see cref="FolderLoaderFileSystemWatcher"/> used to listen to the file system events for the current <see cref="BrowsableObjectInfoLoader.Path"/> property.</para>
        /// <para>When overridden in a derived class, provides a custom <see cref="FolderLoaderFileSystemWatcher"/>.</para>
        /// </summary>
        /// <returns>An instance of the <see cref="FolderLoaderFileSystemWatcher"/> class.</returns>
        protected virtual FolderLoaderFileSystemWatcher GetFolderLoaderFileSystemWatcher() => new FolderLoaderFileSystemWatcher();

        /// <summary>
        /// Frees all resources used by this <see cref="FolderLoader"/>.
        /// </summary>
        protected override void DisposeOverride(bool disposePath)

        {

            base.DisposeOverride(disposePath);

            if (FileSystemWatcher != null)

                FileSystemWatcher.Dispose();

        }

        protected virtual void OnNewShellObjectCreated(string path)

        {

            // todo:

            if (!Application.Current.Dispatcher.CheckAccess())

                Application.Current.Dispatcher.InvokeAsync(() => OnNewShellObjectCreated(path));

            else
            {

                try
                {

                    // todo: may not work with ShellObjectWatcher

                    Path.items.Add(Path.Factory.GetBrowsableObjectInfo(path, FileType.File, SpecialFolder.OtherFolderOrFile, () =>    ShellObject.FromParsingName(path), null));

                }
#if DEBUG
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
#else
            catch { }
#endif

                if (FileSystemObjectComparer != null)

                    Path.items.Sort(FileSystemObjectComparer);
            }

        }

        protected virtual void OnShellObjectRenamed(string oldPath, string newPath)

        {

            if (!Application.Current.Dispatcher.CheckAccess())

                _ = Application.Current.Dispatcher.InvokeAsync(() => OnShellObjectRenamed(oldPath, newPath));

            else
            {

                OnShellObjectDeleted(oldPath);

                OnNewShellObjectCreated(newPath);

            }

        }

        protected virtual void OnShellObjectDeleted(string path)

        {

            if (!Application.Current.Dispatcher.CheckAccess())

                _ = Application.Current.Dispatcher.InvokeAsync(() => OnShellObjectDeleted(path));

            else

                for (int i = 0; i < Path.items.Count; i++)

                    if (Path.items[i].Path == path)

                    {

                        Path.items.RemoveAt(i);

                        return;

                    }

        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e) => OnNewShellObjectCreated(e.FullPath);

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e) => OnShellObjectRenamed(e.OldFullPath, e.FullPath);

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e) => OnShellObjectDeleted(e.FullPath);

        internal static bool HandleIOException(Exception ex) => ex.Is(false, typeof(IOException), typeof(UnauthorizedAccessException), typeof(SecurityException));

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

            var paths = new ArrayAndListBuilder<IFileSystemObject>();

            void AddPath(ref PathInfo pathInfo)

            {

#if DEBUG

                if (pathInfo.Path.EndsWith(".lnk"))

                    Debug.WriteLine("");

#endif

                if (pathInfo.FileType == FileType.Other || (pathInfo.FileType != FileType.SpecialFolder && FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(pathInfo.FileType)))) return;

                // We only make a normalized path if we add the path to the paths to load.

                if (pathInfo.Path.StartsWith("::"))

                {

                    //try

                    //{

                    //    using (IKnownFolder knownFolder = KnownFolderHelper.FromKnownFolderId(Guid.Parse(pathInfo.Path.Substring(3, pathInfo.Path.IndexOf('}') - 3))))

                    //    {

                    //        string path = pathInfo.Path;

                    //        path = knownFolder.Path;

                    //        if (pathInfo.Path.Contains("\\"))

                    //            path += pathInfo.Path.Substring(pathInfo.Path.IndexOf('\\')) + 1;

                    //    }

                    //}

                    //catch (ShellException) { }

                    string path = null /*= Path.Name*/;

                    var browsableObjectInfo = (IBrowsableObjectInfo)Path;

                    IBrowsableObjectInfo newPath;

                    path = browsableObjectInfo.Name;

                    browsableObjectInfo = browsableObjectInfo.Parent;

                    while (browsableObjectInfo != null)

                    {

                        path = browsableObjectInfo.Name + "\\" + path;

                        newPath = browsableObjectInfo.Parent;

                        browsableObjectInfo.Dispose();

                    }

                    pathInfo.Path = path;

                }

                //PropertyInfo[] props = typeof(KnownFolders).GetProperties();

                //string path = pathInfo.Path;

                //if (path.Contains("\\"))

                //    path = path.Substring(0, path.IndexOf("\\"));

                //foreach (PropertyInfo prop in props)

                //    using (IKnownFolder knownFolder = (IKnownFolder)prop.GetValue(null))
                //    {

                //        string displayPath = 

                //        if (path == knownFolder.LocalizedName)

                //            ok = true;

                //        else

                //            using (ShellObject shellObject = ShellObject.FromParsingName(knownFolder.ParsingName))

                //                if (path == shellObject.Name)

                //                    ok = true;

                //        if (ok)

                //    }

                pathInfo.NormalizedPath = pathInfo.Path.RemoveAccents();

                paths.AddLast(pathInfo);

            }

            void AddDirectory(PathInfo pathInfo)

            {

                // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                pathInfo.FileType = pathInfo.ShellObject.IsFileSystemObject ? System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path ? FileType.Drive : FileType.Folder : FileType.SpecialFolder;

                AddPath(ref pathInfo);

            }

            void AddFile(PathInfo pathInfo, bool isLink)

            {

                pathInfo.FileType = isLink
                    ? FileType.Link
                    : ArchiveItemInfo. IsSupportedArchiveFormat(System.IO.Path.GetExtension(pathInfo.Path)) ? FileType.Archive : FileType.File;

                // We only make a normalized path if we add the path to the paths to load.

                AddPath(ref pathInfo);

            }

            try
            {

                if (((ShellObjectInfo)Path).ShellObject.IsFileSystemObject)

                {

                    string[] directories = Directory.GetDirectories(Path.Path);

                    ShellObject shellObject = null;

                    foreach (string directory in directories)

                        if (CheckFilter(directory))

                            AddDirectory(new PathInfo() { Path = directory, ShellObject = ShellObject.FromParsingName(directory) });

                    string[] files = Directory.GetFiles(Path.Path);

                    foreach (string file in files)

                        if (CheckFilter(file))

                            AddFile(new PathInfo() { Path = file, ShellObject = shellObject = ShellObject.FromParsingName(file) }, shellObject.IsLink);

                }

                else

                {

                    //string _path = null;

                    //PathInfo pathInfo;

                    foreach (ShellObject so in (ShellContainer)((ShellObjectInfo)Path).ShellObject)

                        //#if DEBUG

                        //                    {

                        //                        Debug.WriteLine(Path.Path + ": " + ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject.ToString());

                        //                        Debug.WriteLine(so.ParsingName + ": " + so.IsFileSystemObject.ToString());

                        //                        Debug.WriteLine(so.GetType().ToString());

                        //                        Debug.WriteLine((so is ShellFolder).ToString());

                        //#endif

                        if (so is ShellFile)

                            AddFile(new PathInfo() { Path = ((ShellFile)so).Path, ShellObject = so }, so.IsLink);

                        else if (so is ShellLink)

                            AddFile(new PathInfo() { Path = ((ShellLink)so).Path, ShellObject = so }, so.IsLink);

                        // if (so is FileSystemKnownFolder || so is NonFileSystemKnownFolder || so is ShellNonFileSystemFolder || so is ShellLibrary)

                        // if (File.Exists(_path))

                        // AddFile(pathInfo, so.IsLink);

                        else

                            AddDirectory(new PathInfo() { Path = so.ParsingName, ShellObject = so });

                }

            }

            catch (Exception ex) when (HandleIOException(ex))
            {

#if DEBUG

                Debug.WriteLine(ex.GetType().ToString() + " " + ex.Message);

#endif

            }



            IEnumerable<PathInfo> pathInfos;



            if (FileSystemObjectComparer == null)

                pathInfos = (IEnumerable<PathInfo>)paths;

            else

            {

                var _paths = paths.ToList();

                _paths.Sort(FileSystemObjectComparer);

                pathInfos = (IEnumerable<PathInfo>)_paths;

            }



            PathInfo path_;



            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())



                while (_paths.MoveNext())

                    try

                    {

                        do

                        {

                            path_ = _paths.Current;

#if DEBUG

                            Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
                            Debug.WriteLine("path_.Path: " + path_.Path);
                            Debug.WriteLine("path_.Normalized_Path: " + path_.NormalizedPath);
                            Debug.WriteLine("path_.Shell_Object: " + path_.ShellObject);

#endif

                            // new_Path.LoadThumbnail();

                            ReportProgress(0, Path.Factory.GetBrowsableObjectInfo(path_.Path, path_.FileType, IO.Path. GetSpecialFolder(path_.ShellObject), () =>    path_.ShellObject, null));

                        } while (_paths.MoveNext());

                    }
                    catch (Exception ex) when (HandleIOException(ex)) { }

        }

        public struct PathInfo : IFileSystemObject

        {

            /// <summary>
            /// Gets the path of this <see cref="PathInfo"/>.
            /// </summary>
            public string Path { get; set; }

            public string NormalizedPath { get; set; }

            public ShellObject ShellObject { get; set; }

            /// <summary>
            /// Gets the localized name of this <see cref="PathInfo"/>.
            /// </summary>
            public string LocalizedName => ShellObject.GetDisplayName(DisplayNameType.Default);

            /// <summary>
            /// Gets the name of this <see cref="PathInfo"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets the <see cref="IO.FileType"/> of this <see cref="PathInfo"/>.
            /// </summary>
            public FileType FileType { get; set; }

            public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
                    ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
                    : false;

            public int CompareTo(IFileSystemObject fileSystemObject) => BrowsableObjectInfo.GetDefaultComparer().Compare(this, fileSystemObject);

        }

    }

}
