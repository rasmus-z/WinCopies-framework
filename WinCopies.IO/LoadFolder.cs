#region "Imports"



#region ".NET"

using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
    public class LoadFolder : BrowsableObjectInfoItemsLoader
    {

        // todo: to turn on ShellObjectWatcher for better compatibility

        public LoadFolderFileSystemWatcher FileSystemWatcher { get; private set; } = null;

        private IEnumerable<string> _filter = null;

        public IEnumerable<string> Filter
        {

            get => _filter;

            set
            {

                if (IsBusy)

                    throw new InvalidOperationException("The " + nameof(LoadFolder) + " is busy.");

                _filter = value;

            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        public LoadFolder(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes) : base(workerReportsProgress, workerSupportsCancellation, fileTypes)

        {

        }

        protected override void Init()

        {

            if (Path == null)

                FileSystemWatcher = null;

            else

            {

                if (!(Path is ShellObjectInfo _path))

                    throw new ArgumentException("'Path' isn't a 'ShellObjectInfo'. 'Path' is " + Path.ToString());

                else if (_path.FileType != IO.FileTypes.Folder && _path.FileType != IO.FileTypes.Drive && _path.FileType != IO.FileTypes.SpecialFolder)

                    throw new ArgumentException("'Path' isn't a folder, a drive or a special folder. 'Path': " + _path.ToString());

                if ((((ShellObjectInfo)Path).FileType == IO.FileTypes.Folder || ((ShellObjectInfo)Path).FileType == IO.FileTypes.SpecialFolder) && ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject && FileSystemWatcher == null)

                {

                    try
                    {

                        FileSystemWatcher = GetLoadFolderFileSystemWatcher();

                        if ((((ShellObjectInfo)Path).FileType == IO.FileTypes.Drive && new DriveInfo(((ShellObjectInfo)Path).Path).IsReady) || (((ShellObjectInfo)Path).FileType != IO.FileTypes.Drive && ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject))

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
        /// <para>Gets the <see cref="LoadFolderFileSystemWatcher"/> used to listen to the file system events for the current <see cref="BrowsableObjectInfoItemsLoader.Path"/> property.</para>
        /// <para>When substituted in a derived class, provides a custom <see cref="LoadFolderFileSystemWatcher"/>.</para>
        /// </summary>
        /// <returns>An instance of the <see cref="LoadFolderFileSystemWatcher"/> class.</returns>
        protected virtual LoadFolderFileSystemWatcher GetLoadFolderFileSystemWatcher() => new LoadFolderFileSystemWatcher();

        /// <summary>
        /// Frees all resources used by this <see cref="LoadFolder"/>.
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

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(ShellObject shellObject, string path) => ((ShellObjectInfo)Path).GetBrowsableObjectInfo(shellObject, path);

        protected virtual IBrowsableObjectInfo OnAddingNewBrowsableObjectInfo(PathInfo path) => ((ShellObjectInfo)Path).GetBrowsableObjectInfo(path.ShellObject, path.Path, path.FileType, ShellObjectInfo.GetFileType(path.Path, path.ShellObject).specialFolder);

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

#if DEBUG

            Debug.WriteLine("Dowork event started.");

            Debug.WriteLine(FileTypes);

#endif

            if (FileTypes == FileTypesFlags.None) return;

            else if (FileTypes.HasFlag(FileTypesFlags.All) && FileTypes.HasMultipleFlags())

                throw new InvalidOperationException("FileTypes cannot have the All flag in combination with other flags.");

            List<IFileSystemObject> paths = new List<IFileSystemObject>();

            var comp = LoadFolder.comp.GetInstance();

            void AddPath(ref PathInfo pathInfo)

            {

                // We only make a normalized path if we add the path to the paths to load.

                pathInfo.Normalized_Path = IO.Path.GetNormalizedPath(pathInfo.Path);

                paths.Add(pathInfo);

            }

            void AddDirectory(PathInfo pathInfo)

            {

                if (pathInfo.ShellObject.IsFileSystemObject)

                {

                    if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                        pathInfo.FileType = System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path ? IO.FileTypes.Drive : IO.FileTypes.Folder;

                }

                else

                    pathInfo.FileType = IO.FileTypes.SpecialFolder;

                if (pathInfo.FileType != IO.FileTypes.None)

                    // We only make a normalized path if we add the path to the paths to load.

                    AddPath(ref pathInfo);

            }

            void AddFile(PathInfo pathInfo, bool isLink)

            {

                if (isLink && (FileTypes == FileTypesFlags.All || FileTypes.HasFlag(FileTypesFlags.Link)))

                    pathInfo.FileType = IO.FileTypes.Link;

                else

                {

                    if (LoadArchive.IsSupportedArchiveFormat(System.IO.Path.GetExtension(pathInfo.Path)) && (FileTypes == FileTypesFlags.All || FileTypes.HasFlag(FileTypesFlags.Archive)))

                        pathInfo.FileType = IO.FileTypes.Archive;

                    else if (FileTypes == FileTypesFlags.All || FileTypes.HasFlag(FileTypesFlags.File))

                        pathInfo.FileType = IO.FileTypes.File;

                }

                if (pathInfo.FileType != IO.FileTypes.None)

                    // We only make a normalized path if we add the path to the paths to load.

                    AddPath(ref pathInfo);

            }

            try
            {

#if DEBUG

                Debug.WriteLine("Path == null: " + (Path == null).ToString());

                Debug.WriteLine("Path.Path: " + Path.Path);

                Debug.WriteLine("Path.ShellObject: " + ((ShellObjectInfo)Path).ShellObject.ToString());

#endif    

                if (((ShellObjectInfo)Path).ShellObject.IsFileSystemObject)

                {

                    bool checkFilter(string directory)

                    {

                        if (Filter == null) return true;

                        foreach (string filter in Filter)

                            if (!IO.Path.MatchToFilter(directory, filter)) return false;

                        return true;

                    }

                    string[] directories = Directory.GetDirectories(Path.Path);

                    ShellObject shellObject = null;

                    foreach (string directory in directories)

                        if (checkFilter(directory))

                            AddDirectory(new PathInfo() { Path = directory, ShellObject = ShellObject.FromParsingName(directory) });

                    string[] files = Directory.GetFiles(Path.Path);

                    foreach (string file in files)

                        if (checkFilter(file))

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

                        if (so.GetType() == typeof(ShellFile))

                            AddFile(new PathInfo() { Path = ((ShellFile)so).Path, ShellObject = so }, false);

                        else if (so.GetType() == typeof(ShellLink))

                            AddFile(new PathInfo() { Path = ((ShellLink)so).Path, ShellObject = so }, true);

                        // if (so is FileSystemKnownFolder || so is NonFileSystemKnownFolder || so is ShellNonFileSystemFolder || so is ShellLibrary)

                        // if (File.Exists(_path))

                        // AddFile(pathInfo, so.IsLink);

                        else

                            AddDirectory(new PathInfo() { Path = so.ParsingName, ShellObject = so });

                }

            }

#if DEBUG

            catch (Exception ex) { Debug.WriteLine(ex.Message); }

#else

            catch (IOException) { }    

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

            public string LocalizedPath { get => ShellObject.GetDisplayName(DisplayNameType.Default); }

            public string Name { get; set; }

            public FileTypes FileType { get; set; }

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

            public override int Compare(IFileSystemObject x, IFileSystemObject y) => x.FileType == y.FileType || (x.FileType == IO.FileTypes.File && (y.FileType == IO.FileTypes.Link || y.FileType == IO.FileTypes.Archive)) || (y.FileType == IO.FileTypes.File && (x.FileType == IO.FileTypes.Link || x.FileType == IO.FileTypes.Archive))
                    ? bidule.Compare(IO.Path.GetNormalizedPath(x.LocalizedPath), IO.Path.GetNormalizedPath(y.LocalizedPath))
                    : (x.FileType == IO.FileTypes.Folder || x.FileType == IO.FileTypes.Drive) && (y.FileType == IO.FileTypes.File || y.FileType == IO.FileTypes.Archive || y.FileType == IO.FileTypes.Link)
                    ? -1
                    : (x.FileType == IO.FileTypes.File || x.FileType == IO.FileTypes.Archive || x.FileType == IO.FileTypes.Link) && (y.FileType == IO.FileTypes.Folder || y.FileType == IO.FileTypes.Drive)
                    ? 1
                    : 0;

        }

    }

}
