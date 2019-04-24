#region "Imports"



#region ".NET"

using Microsoft.WindowsAPICodePack.Shell;
using System;
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
    public class FolderLoader : BrowsableObjectInfoItemsLoader
    {

        // todo: to turn on ShellObjectWatcher for better compatibility

        public FolderLoaderFileSystemWatcher FileSystemWatcher { get; private set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        public FolderLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes) : base(workerReportsProgress, workerSupportsCancellation, fileTypes)

        {

        }

        protected override void Init()

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

                    Path.items.Add(OnAddingNewBrowsableObjectInfo(ShellObject.FromParsingName(path), path));

                }
#if DEBUG
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
#else
            catch { }
#endif

                Path.items.Sort(comp.GetInstance());
            }

        }

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo)

        {

            if (browsableObjectInfo is BrowsableObjectInfo _browsableObjectInfo)

                _browsableObjectInfo.Parent = Path;

            return browsableObjectInfo;

        }

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(ShellObject shellObject, string path) => OnAddingNewBrowsableObjectInfo(((ShellObjectInfo)Path).GetBrowsableObjectInfo(shellObject, path));

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(PathInfo path) => OnAddingNewBrowsableObjectInfo(((ShellObjectInfo)Path).GetBrowsableObjectInfo(path.ShellObject, path.Path, path.FileType, ShellObjectInfo.GetFileType(path.Path, path.ShellObject).specialFolder));

        protected virtual void OnShellObjectRenamed(string oldPath, string newPath)

        {

            if (!Application.Current.Dispatcher.CheckAccess())

                Application.Current.Dispatcher.InvokeAsync(() => OnShellObjectRenamed(oldPath, newPath));

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

        protected override void OnDoWork(object sender, DoWorkEventArgs e)
        {

            base.OnDoWork(sender, e);

            List<IFileSystemObject> paths = new List<IFileSystemObject>();

            var comp = FolderLoader.comp.GetInstance();

            void AddPath(ref PathInfo pathInfo)

            {

                if (pathInfo.Path.EndsWith(".lnk"))

                    Debug.WriteLine("");

                if (pathInfo.FileType == FileType.None || (pathInfo.FileType != FileType.SpecialFolder && FileTypes != FileTypesFlags.All && !FileTypes.HasFlag(FileTypeToFileTypeFlags(pathInfo.FileType)))) return;

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

                pathInfo.Normalized_Path = IO.Path.GetNormalizedPath(pathInfo.Path);

                paths.Add(pathInfo);

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
                    ? IO.FileType.Link
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

#if DEBUG

            catch (IOException ex)
            {

                Debug.WriteLine(ex.GetType().ToString() + " " + ex.Message);

                throw;

            }

#endif

            paths.Sort(comp);



            void reportProgressAndAddNewPathToObservableCollection(PathInfo path_)

            {

#if DEBUG

                Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
                Debug.WriteLine("path_.Path: " + path_.Path);
                Debug.WriteLine("path_.Normalized_Path: " + path_.Normalized_Path);
                Debug.WriteLine("path_.Shell_Object: " + path_.ShellObject);

#endif

                var new_Path = path_.ShellObject;

                // new_Path.LoadThumbnail();

                ReportProgress(0, OnAddingNewBrowsableObjectInfo(path_));

#if DEBUG

                Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

#endif

            }

            foreach (PathInfo path_ in paths)

                reportProgressAndAddNewPathToObservableCollection(path_);

        }

        public struct PathInfo : IFileSystemObject

        {

            public string Path { get; set; }

            public string Normalized_Path { get; set; }

            public ShellObject ShellObject { get; set; }

            public string LocalizedName { get => ShellObject.GetDisplayName(DisplayNameType.Default); }

            public string Name { get; set; }

            public FileType FileType { get; set; }

        }

        public class comp : Comparer<IFileSystemObject> // Variable locale pour stocker une référence vers l'instance

        {


            private static comp instance = null;

            private static readonly object mylock = new object();

            public StringComparer bidule { get; set; } = StringComparer.Create(CultureInfo.CurrentCulture, true);

            // Le constructeur est Private
            private comp()
            {
                //
            }

            // La méthode GetInstance doit être Shared
            public static comp GetInstance()
            {
                if (instance == null)

                    lock (mylock)

                        // Si pas d'instance existante on en crée une...
                        if (instance == null)

                            instance = new comp();

                // On retourne l'instance de Singleton
                return instance;

            }

            public override int Compare(IFileSystemObject x, IFileSystemObject y) => x.FileType == y.FileType || (x.FileType == IO.FileType.File && (y.FileType == IO.FileType.Link || y.FileType == IO.FileType.Archive)) || (y.FileType == IO.FileType.File && (x.FileType == IO.FileType.Link || x.FileType == IO.FileType.Archive))
                    ? bidule.Compare(IO.Path.GetNormalizedPath(x.LocalizedName), IO.Path.GetNormalizedPath(y.LocalizedName))
                    : (x.FileType == IO.FileType.Folder || x.FileType == IO.FileType.Drive) && (y.FileType == IO.FileType.File || y.FileType == IO.FileType.Archive || y.FileType == IO.FileType.Link)
                    ? -1
                    : (x.FileType == IO.FileType.File || x.FileType == IO.FileType.Archive || x.FileType == IO.FileType.Link) && (y.FileType == IO.FileType.Folder || y.FileType == IO.FileType.Drive)
                    ? 1
                    : 0;

        }

    }

}
