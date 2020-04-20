/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using SevenZip;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using TsudaKageyu;

using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using System.Linq;
using System.Collections.Generic;
using WinCopies.Util;
using System.IO;
using WinCopies.Collections;
using System.Collections.ObjectModel;
using System.Security;
using static WinCopies.IO.FolderLoader;

namespace WinCopies.IO
{

    /// <summary>
    /// Represents an archive item.
    /// </summary>
    public interface IArchiveItemInfo : IArchiveItemInfoProvider
    {

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        ArchiveFileInfo? ArchiveFileInfo { get; }

    }

    /// <summary>
    /// Represents an archive item.
    /// </summary>
    public class ArchiveItemInfo/*<TItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IArchiveItemInfo // where TItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        // public override bool IsRenamingSupported => false;

        #region Properties

        //public static ArchiveFileInfoDeepClone DefaultArchiveFileInfoDeepClone { get; } = (ArchiveFileInfo? archiveFileInfo, string archivePath) => archiveFileInfo.HasValue
        //        ? (ArchiveFileInfo?)new SevenZipExtractor(archivePath).ArchiveFileData.First(item => item.FileName.ToLower() == archiveFileInfo.Value.FileName)
        //        : null;

        /// <summary>
        /// Gets the localized path of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override string LocalizedName => ArchiveShellObject.LocalizedName;

        /// <summary>
        /// Gets the name of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; private set; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class using a custom factory for <see cref="ArchiveItemInfo"/>s.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo/*, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate*/) : base(path, fileType)

        {

            if (fileType == FileType.SpecialFolder)

                // todo:

                throw new ArgumentException(string.Format(Properties.Resources.SpecialFolderError, nameof(fileType)));

            ArchiveFileInfo = archiveFileInfo;

            if (ArchiveFileInfo.HasValue && !path.EndsWith(ArchiveFileInfo.Value.FileName))

                // todo:

                throw new ArgumentException(string.Format(Properties.Resources.PathMustEndWithFileName, nameof(path), nameof(ArchiveFileInfo.Value.FileName)));

            ArchiveShellObject = archiveShellObject;

#if DEBUG 

            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

#endif

            // Path = path;

#if DEBUG

            Debug.WriteLine("path: " + path);

#endif

        }

        ///// <summary>
        ///// Loads the items of this <see cref="ArchiveItemInfo"/> using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        ///// <summary>
        ///// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        #region Protected methods

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        /// <returns>the parent of this <see cref="ArchiveItemInfo"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

            IBrowsableObjectInfo result;

            if (Path.Length > ArchiveShellObject.Path.Length)

            {

                string path = Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator));

                ArchiveFileInfo? archiveFileInfo = null;

                using (var extractor = new SevenZipExtractor(ArchiveShellObject.Path))

                    foreach (ArchiveFileInfo item in extractor.ArchiveFileData)

                        if (item.FileName.ToLower() == path.ToLower())

                            archiveFileInfo = item;

                result = new ArchiveItemInfo(path, FileType.Folder, ArchiveShellObject, archiveFileInfo/*, _archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(_archiveFileInfo, ArchiveShellObject.Path) archiveParentFileInfo.Value*/);

            }

            else

                result = ArchiveShellObject;

            return result /*&& Path.Contains(IO.Path.PathSeparator)*/;

        }

        protected override IReadOnlyCollection<IBrowsableObjectInfo> GetItems()

        {

            // if (FileTypes == FileTypes.None) return;

            //else if (FileTypes.HasFlag(GetAllEnumFlags<FileTypes>()) && FileTypes.HasMultipleFlags())

            //    throw new InvalidOperationException("FileTypes cannot have the All flag in combination with other flags.");

#if DEBUG

                        // Debug.WriteLine("Dowork event started.");

                        // Debug.WriteLine(FileTypes);

                        try
                        {

                            Debug.WriteLine("Path == null: " + (Path == null).ToString());

                            Debug.WriteLine("Path: " + Path);

                            Debug.WriteLine("ShellObject: " + ArchiveShellObject.ToString());

                        }
#pragma warning disable CA1031 // Do not catch general exception types
                        catch (Exception) { }
#pragma warning restore CA1031 // Do not catch general exception types

#endif

#if DEBUG

                        // Debug.WriteLine("Dowork event started.");

#endif

            //List<FolderLoader.IPathInfo> directories = new List<FolderLoader.IPathInfo>();

            //List<FolderLoader.IPathInfo> files = new List<FolderLoader.IPathInfo>();

            var paths = new ArrayBuilder<PathInfo>();

#if DEBUG

                        Debug.WriteLine("Path == null: " + (Path == null).ToString());

                        Debug.WriteLine("Path: " + Path);

                        Debug.WriteLine("ShellObject: " + ArchiveShellObject.ToString());

#endif

            // ShellObjectInfo archiveShellObject = Path is ShellObjectInfo ? (ShellObjectInfo)Path : ((ArchiveItemInfo)Path).ArchiveShellObject;

            string archiveFileName = ArchiveShellObject.Path;

            try

            {

#if NETFRAMEWORK

                using (var archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))

                using (var archiveExtractor = new SevenZipExtractor(archiveFileStream))

                {

#else

                using var archiveFileStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                using var archiveExtractor = new SevenZipExtractor(archiveFileStream);

#endif

                    //try
                    //{

                    // archiveShellObject.ArchiveFileStream = archiveFileStream;
                    void AddPath(ref string _path, FileType fileType, ref ArchiveFileInfo? archiveFileInfo) =>

                        //if (fileType == FileType.Other || (FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

                        // // We only make a normalized path if we add the path to the paths to load.

                        // string __path = string.Copy(_path);

                        paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), fileType, archiveFileInfo));

                    void AddDirectory(string _path, ArchiveFileInfo? archiveFileInfo) =>

                        // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                        AddPath(ref _path, FileType.Folder, ref archiveFileInfo);

                    void AddFile(string _path, ArchiveFileInfo? archiveFileInfo) =>

                        // We only make a normalized path if we add the path to the paths to load.

                        AddPath(ref _path, _path.Substring(_path.Length).EndsWith(".lnk")
                            ? FileType.Link
                            : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(_path)) ? FileType.Archive : FileType.File, ref archiveFileInfo);

                    System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

                    string fileName = "";

                    string relativePath = Path.Substring(archiveFileName.Length + 1);

                    // PathInfo path;

#if DEBUG

                                    foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                                        Debug.WriteLine(archiveFileInfo.FileName);

#endif

                    void addPath(ArchiveFileInfo archiveFileInfo)

                    {

                        if (archiveFileInfo.FileName.StartsWith(relativePath) && archiveFileInfo.FileName.Length > relativePath.Length)

                        {

                            fileName = archiveFileInfo.FileName.Substring(relativePath.Length);

                            if (fileName.StartsWith(IO.Path.PathSeparator))

                                fileName = fileName.Substring(1);

                            if (fileName.Contains(IO.Path.PathSeparator))

                                fileName = fileName.Substring(0, fileName.IndexOf(IO.Path.PathSeparator));

                            /*if (!archiveFileInfo.FileName.Substring(archiveFileInfo.FileName.Length).Contains(IO.Path.PathSeparator))*/

                            // {

                            foreach (IFileSystemObject pathInfo in (IEnumerable<IFileSystemObject>)paths)

                                if (pathInfo.Path == fileName)

                                    return;

                            if (fileName.ToLower() == archiveFileInfo.FileName.ToLower())

                            {

                                if (archiveFileInfo.IsDirectory)

                                    AddDirectory(fileName, archiveFileInfo);

                                else /*if (CheckFilter(archiveFileInfo.FileName))*/

                                    AddFile(fileName, archiveFileInfo);

                            }

                            else

                                AddDirectory(fileName, archiveFileInfo);

                            // }

                        }

                    }

                    foreach (ArchiveFileInfo archiveFileInfo in archiveFileData)

                        // _path = archiveFileInfo.FileName.Replace('/', IO.Path.PathSeparator);

                        addPath(archiveFileInfo);

#if NETFRAMEWORK

                }

#endif

            }

            catch (Exception ex) when (ex.Is(false, typeof(IOException), typeof(SecurityException), typeof(UnauthorizedAccessException), typeof(SevenZipException))) { return null; }

            // for (int i = 0; i < paths.Count; i++)

            // {

            // PathInfo directory = (PathInfo)paths[i];

            // string CurrentFile_Normalized = "";

            // CurrentFile_Normalized = Util.GetNormalizedPath(directory.Path);

            // directory.Normalized_Path = CurrentFile_Normalized;

            // paths[i] = directory;

            // }

            IEnumerable<PathInfo> pathInfos;

            //if (FileSystemObjectComparer == null)

            //    pathInfos = (IEnumerable<PathInfo>)paths;

            //else

            //{

            //    var sortedPaths = paths.ToList();

            //    sortedPaths.Sort(FileSystemObjectComparer);

            //    pathInfos = (IEnumerable<PathInfo>)paths;

            //}

            pathInfos = paths;

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
                            Debug.WriteLine("path_.Path: " + Path);
                            //Debug.WriteLine("path_.Normalized_Path: " + path.NormalizedPath);
                            // Debug.WriteLine("path_.Shell_Object: " + path.ArchiveShellObject);

                            // var new_Path = ((ArchiveItemInfo)Path).ArchiveShellObject;
                            // new_Path.LoadThumbnail();

                            ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>(    (TItems)Path.Factory.GetBrowsableObjectInfo(Path.Value.Path + IO.Path.PathSeparator + path.Path, path.FileType, Path.Value.ArchiveShellObject, path.ArchiveFileInfo, archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(archiveFileInfo, Path.Value.ArchiveShellObject.Path)), (TItemsFactory) Path.Factory.DeepClone()));

                            // #if DEBUG

                            // Debug.WriteLine("Ceci est un " + new_Path.GetType().ToString());

                            // #endif

                        }

#endif

            // this._Paths = new ObservableCollection<IBrowsableObjectInfo>();



            PathInfo path_;

            IList<IBrowsableObjectInfo> result = new List<IBrowsableObjectInfo>(paths.Count);



#if NETFRAMEWORK

            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())

#else

            using IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator();

#endif

                while (_paths.MoveNext())

                    try

                    {

                        do

                        {

                            path_ = _paths.Current;

#if DEBUG

                                        reportProgress(path_);

#else

                            result.Add(new ArchiveItemInfo($"{Path }{IO.Path.PathSeparator }{path_.Path}", path_.FileType, ArchiveShellObject, path_.ArchiveFileInfo));

#endif

                        } while (_paths.MoveNext());

                    }
                    catch (Exception ex) when (HandleIOException(ex)) { }

            //foreach (FolderLoader.PathInfo path_ in files)

            //    reportProgressAndAddNewPathToObservableCollection(path_);

            return new ReadOnlyCollection<IBrowsableObjectInfo>(result);

        }

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);

            if (disposing)

                ArchiveFileInfo = null;

        }

        #endregion

        // public override string ToString() => System.IO.Path.GetFileName(Path);

        #region Private methods

        private Icon TryGetIcon(System.Drawing.Size size) =>

            // if (System.IO.Path.HasExtension(Path))

            Microsoft.WindowsAPICodePack.Shell.FileOperation.GetFileInfo(System.IO.Path.GetExtension(Path), Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Normal, Microsoft.WindowsAPICodePack.Win32Native.Shell.GetFileInfoOptions.Icon | Microsoft.WindowsAPICodePack.Win32Native.Shell.GetFileInfoOptions.UseFileAttributes).Icon?.TryGetIcon(size, 32, true, true) ?? TryGetIcon(FileType == FileType.Folder ? 3 : 0, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, size);// else// return TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

#if NETFRAMEWORK

            using (Icon icon = TryGetIcon(size))

#else

            using Icon icon = TryGetIcon(size);

#endif

                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        #endregion

        protected class PathInfo : IO.PathInfo

        {

            public FileType FileType { get; }

            public ArchiveFileInfo? ArchiveFileInfo { get; }

            //public DeepClone<ArchiveFileInfo?> ArchiveFileInfoDelegate { get; }

            /// <summary>
            /// Gets the localized name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string LocalizedName => Name;

            /// <summary>
            /// Gets the name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string Name => System.IO.Path.GetFileName(Path);

            public PathInfo(string path, string normalizedPath, FileType fileType, ArchiveFileInfo? archiveFileInfo) : base(path, normalizedPath)
            {

                ArchiveFileInfo = archiveFileInfo;

                //ArchiveFileInfoDelegate = archiveFileInfoDelegate;

                FileType = fileType;

            }

            //public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
            //        ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
            //        : false;

            //public int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

        }

    }

}
