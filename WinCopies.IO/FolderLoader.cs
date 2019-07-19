#region "Imports"



#region ".NET"

using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using WinCopies.Util;

#endregion

#region "WinCopies"

// using WinCopies.IO;

#endregion


#endregion

namespace WinCopies.IO
{

    /// <summary>
    /// Provides a background process that can be used to load items of a folder.
    /// </summary>
    public class FolderLoader : FileSystemObjectItemsLoader
    {

        // todo: to turn on ShellObjectWatcher for better compatibility

#pragma warning disable CS0649 // Set up using reflection.
        private readonly FolderLoaderFileSystemWatcher _folderLoaderFileSystemWatcher;
#pragma warning restore CS0649

        public FolderLoaderFileSystemWatcher FileSystemWatcher { get => _folderLoaderFileSystemWatcher; private set => this.SetBackgroundWorkerProperty(nameof(FileSystemWatcher), nameof(_folderLoaderFileSystemWatcher), typeof(FolderLoader), true); }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public FolderLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypes fileTypes) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), fileTypes) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="fileTypes">The file types to load.</param>
        public FolderLoader(bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> fileSystemObjectComparer, FileTypes fileTypes) : base(workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer, fileTypes) { }

        protected override void InitializePath()

        {

            if (Path == null)

                FileSystemWatcher = null;

            else

            {

                if (!(Path is ShellObjectInfo _path))

                    throw new ArgumentException("'Path' isn't a ShellObjectInfo. 'Path' is " + Path.ToString());

                else if (_path.FileType != FileType.Folder && _path.FileType != FileType.Drive && _path.FileType != FileType.SpecialFolder)

                    throw new ArgumentException("'Path' isn't a folder, a drive or a special folder. 'Path': " + _path.ToString());

                if ((((ShellObjectInfo)Path).FileType == FileType.Folder || ((ShellObjectInfo)Path).FileType == FileType.SpecialFolder) && ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject && FileSystemWatcher == null)

                {

                    try
                    {

                        FileSystemWatcher = GetFolderLoaderFileSystemWatcher();

                        if ((((ShellObjectInfo)Path).FileType == FileType.Drive && new DriveInfo(((ShellObjectInfo)Path).Path).IsReady) || (((ShellObjectInfo)Path).FileType != FileType.Drive && ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject))

                        {

                            FileSystemWatcher.Path = Path.Path;

                            FileSystemWatcher.EnableRaisingEvents = true;

                            FileSystemWatcher.Created += FileSystemWatcher_Created;

                            FileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

                            FileSystemWatcher.Deleted += FileSystemWatcher_Deleted;

                        }

                    }

                    catch { }

                }

            }

        }

        /// <summary>
        /// <para>Gets the <see cref="FolderLoaderFileSystemWatcher"/> used to listen to the file system events for the current <see cref="BrowsableObjectInfoItemsLoader.Path"/> property.</para>
        /// <para>When substituted in a derived class, provides a custom <see cref="FolderLoaderFileSystemWatcher"/>.</para>
        /// </summary>
        /// <returns>An instance of the <see cref="FolderLoaderFileSystemWatcher"/> class.</returns>
        protected virtual FolderLoaderFileSystemWatcher GetFolderLoaderFileSystemWatcher() => new FolderLoaderFileSystemWatcher();

        /// <summary>
        /// Frees all resources used by this <see cref="FolderLoader"/>.
        /// </summary>
        public override void Dispose()

        {

            base.Dispose();

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

                    ((BrowsableObjectInfo)Path).items.Add(((ShellObjectInfo)Path).ShellObjectInfoFactory.GetBrowsableObjectInfo(ShellObject.FromParsingName(path), path));

                }
#if DEBUG
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
#else
            catch { }
#endif

                ((BrowsableObjectInfo)Path).items.Sort(FileSystemObjectComparer);
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

                Application.Current.Dispatcher.InvokeAsync(() => OnShellObjectDeleted(path));

            else

                for (int i = 0; i < ((BrowsableObjectInfo)Path).items.Count; i++)

                    if (((BrowsableObjectInfo)Path).items[i].Path == path)

                    {

                        ((BrowsableObjectInfo)Path).items.RemoveAt(i);

                        return;

                    }

        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e) => OnNewShellObjectCreated(e.FullPath);

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e) => OnShellObjectRenamed(e.OldFullPath, e.FullPath);

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e) => OnShellObjectDeleted(e.FullPath);

        protected override void OnDoWork()

        {

            if (FileTypes == FileTypes.None) return;

            else if (FileTypes.HasFlag(Util.Util.GetAllEnumFlags<FileTypes>()) && FileTypes.HasMultipleFlags())

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

                if (pathInfo.FileType == FileType.None || (pathInfo.FileType != FileType.SpecialFolder && FileTypes != Util.Util.GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(pathInfo.FileType)))) return;

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

                    IBrowsableObjectInfo browsableObjectInfo = Path;

                    path = browsableObjectInfo.Name;

                    browsableObjectInfo = browsableObjectInfo.Parent;

                    while (browsableObjectInfo != null)

                    {

                        path = browsableObjectInfo.Name + "\\" + path;

                        browsableObjectInfo = browsableObjectInfo.Parent;

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
                    : ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(pathInfo.Path)) ? FileType.Archive : FileType.File;

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

                        // if (CheckFilter(directory))

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

            catch (IOException ex)
            {

#if DEBUG

                Debug.WriteLine(ex.GetType().ToString() + " " + ex.Message);

#endif

            }



            List<IFileSystemObject> _paths = paths.ToList();



            _paths.Sort(FileSystemObjectComparer);



            foreach (PathInfo path_ in _paths)

            {

#if DEBUG

                Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
                Debug.WriteLine("path_.Path: " + path_.Path);
                Debug.WriteLine("path_.Normalized_Path: " + path_.NormalizedPath);
                Debug.WriteLine("path_.Shell_Object: " + path_.ShellObject);

#endif

                // new_Path.LoadThumbnail();

                ReportProgress(0, ((ShellObjectInfo)Path).ShellObjectInfoFactory.GetBrowsableObjectInfo(path_.ShellObject, path_.Path, path_.FileType, ShellObjectInfo.GetFileType(path_.Path, path_.ShellObject).specialFolder));

            }

        }

        public struct PathInfo : IFileSystemObject

        {

            public string Path { get; set; }

            public string NormalizedPath { get; set; }

            public ShellObject ShellObject { get; set; }

            public string LocalizedName => ShellObject.GetDisplayName(DisplayNameType.Default);

            public string Name { get; set; }

            public FileType FileType { get; set; }

        }

    }

}
