﻿/* Copyright © Pierre Sprimont, 2020
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

using Microsoft.WindowsAPICodePack.Shell;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;
using FileInfo = System.IO.FileInfo;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using WinCopies.Util;
using static WinCopies.IO.Path;
using static WinCopies.IO.ShellObjectInfo;
using Microsoft.WindowsAPICodePack.COMNative.Shell;
using WinCopies.Collections;

namespace WinCopies.IO
{

    /// <summary>
    /// Represents a file system item.
    /// </summary>
    public class ShellObjectInfo/*<TItems, TArchiveItemInfoItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IShellObjectInfo // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TArchiveItemInfoItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        #region Properties

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item. If the current <see cref="ShellObjectInfo"/> represents an archive file, this property returns the current <see cref="ShellObjectInfo"/>, or <see langword="null"/> otherwise.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject => FileType == FileType.Archive ? this : null;

        /// <summary>
        /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public ShellObject ShellObject { get; private set; } = null;

        /// <summary>
        /// Gets the localized name of this <see cref="ShellObjectInfo"/> depending the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
        /// </summary>
        public override string LocalizedName => ShellObject.GetDisplayName(DisplayNameType.Default);

        /// <summary>
        /// Gets the name of this <see cref="ShellObjectInfo"/> depending of the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
        /// </summary>
        public override string Name => ShellObject.Name;

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => ShellObject.Thumbnail.SmallBitmapSource;

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => ShellObject.Thumbnail.MediumBitmapSource;

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => ShellObject.Thumbnail.LargeBitmapSource;

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => ShellObject.Thumbnail.ExtraLargeBitmapSource;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ShellObjectInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => (ShellObject is IEnumerable<ShellObject> || FileType == FileType.Archive) && FileType != FileType.File && FileType != FileType.Link; // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

        /// <summary>
        /// Gets a <see cref="FileSystemInfo"/> object that provides info for the folders and files. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a folder, a drive or a file.
        /// </summary>
        /// <seealso cref="FileSystemObjectInfo.FileType"/>
        public FileSystemInfo FileSystemInfoProperties { get; private set; } = null;

        /// <summary>
        /// Gets a <see cref="DriveInfo"/> object that provides info for drives. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a drive.
        /// </summary>
        /// <seealso cref="FileSystemObjectInfo.FileType"/>
        public DriveInfo DriveInfoProperties { get; private set; } = null;

        /// <summary>
        /// Gets a <see cref="IKnownFolder"/> object that provides info for the system known folders. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a system known folder. See the <see cref="FileSystemObjectInfo.FileType"/> property for more details.
        /// </summary>
        public IKnownFolder KnownFolderInfo { get; private set; } = null;

        /// <summary>
        /// Gets the special folder type of this <see cref="ShellObjectInfo"/>. <see cref="SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.
        /// </summary>
        public SpecialFolder SpecialFolder { get; private set; } = SpecialFolder.OtherFolderOrFile;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo"/>s and <see cref="ArchiveItemInfo"/>s.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        public ShellObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject) : base(path, fileType) // =>

        // Init(specialFolder, shellObjectDelegate, shellObject, nameof(fileType), archiveItemInfoFactory); // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        // private void Init(SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject, string fileTypeParameterName, IArchiveItemInfoFactory archiveItemInfoFactory)

        {

#if DEBUG

            if (shellObject.ParsingName != Path)

                Debug.WriteLine("");

#endif

            if ((FileType == FileType.SpecialFolder && specialFolder == SpecialFolder.OtherFolderOrFile) || (FileType != FileType.SpecialFolder && specialFolder != SpecialFolder.OtherFolderOrFile))

                throw new ArgumentException(string.Format(Properties.Resources.FileTypeAndSpecialFolderDoNotCorrespond, nameof(fileType), nameof(specialFolder), FileType, specialFolder));



            ShellObject = shellObject;

            // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

            // NormalizedPath = Util.GetNormalizedPath(path);

            SpecialFolder = specialFolder;

            SetFileSystemInfoProperties(shellObject);

        }

        ///// <summary>
        ///// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        ///// </summary>
        ///// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will report progress.</param>
        ///// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will supports cancellation.</param>
        //public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation)
        //{

        //    if (!IsBrowsable)

        //        throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

        //    if (ShellObject.IsFileSystemObject)

        //    {

        //        if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

        //            LoadItems((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //        else if (FileType == FileType.Archive)

        //            LoadItems((IBrowsableObjectInfoLoader)new ArchiveLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //    }

        //    else

        //        LoadItems((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //    //else

        //    //{

        //    //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

        //    //    foreach (ShellObject item in items)

        //    //    {

        //    //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

        //    //    }

        //    //}

        //}

        ///// <summary>
        ///// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        ///// </summary>
        ///// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will report progress.</param>
        ///// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TSubItems, TFactory}"/> will supports cancellation.</param>
        //public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation)
        //{

        //    if (!IsBrowsable)

        //        throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

        //    if (ShellObject.IsFileSystemObject)

        //    {

        //        if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

        //            LoadItemsAsync((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //        else if (FileType == FileType.Archive)

        //            LoadItemsAsync((IBrowsableObjectInfoLoader)new ArchiveLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //    }

        //    else

        //        LoadItemsAsync((IBrowsableObjectInfoLoader)new FolderLoader<ShellObjectInfo, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        //    //else

        //    //{

        //    //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

        //    //    foreach (ShellObject item in items)

        //    //    {

        //    //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

        //    //    }

        //    //}

        //}

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> in memory.
        // /// </summary>

        ///// <summary>
        ///// Gets or sets the factory for this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
        ///// <exception cref="ArgumentNullException">value is null.</exception>
        //public new ShellObjectInfoFactory Factory { get => (ShellObjectInfoFactory)base.Factory; set => base.Factory = value; }

        // public override bool IsRenamingSupported => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.File, FileType.Drive, FileType.Archive, FileType.Link);

        ///// <summary>
        ///// Renames or move to a relative path, or both, the current <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
        ///// </summary>
        ///// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
        //public override void Rename(string newValue)

        //{

        //    if (If(IfCT.Or, IfCM.Logical, IfComp.NotEqual, out string key, FileType, GetKeyValuePair(nameof(FileType.Folder), FileType.Folder), GetKeyValuePair(nameof(FileType.File), FileType.File), GetKeyValuePair(nameof(FileType.Drive), FileType.Drive)))

        //        throw new InvalidOperationException($"{nameof(FileType)} must have one of the following values: {nameof(FileType.Folder)}, {nameof(FileType.File)} or {nameof(FileType.Drive)}. The value was {key}.");

        //    string getNewPath() => System.IO.Path.GetDirectoryName(Path) + IO.Path.PathSeparator + newValue;

        //    switch (FileType)
        //    {

        //        case FileType.Folder:

        //            Directory.Move(Path, getNewPath());

        //            break;

        //        case FileType.File:

        //            File.Move(Path, getNewPath());

        //            break;

        //        case FileType.Drive:

        //            DriveInfoProperties.VolumeLabel = newValue;

        //            break;

        //    }

        //}

        /// <summary>
        /// Gets a string representation of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="ShellObjectInfo"/>.</returns>
        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        #region Protected methods

        /// <summary>
        /// Returns the parent of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="ShellObjectInfo"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

#if !NETFRAMEWORK

            static

#endif

                (FileType, SpecialFolder) getFileType(ShellObject _shellObject)

            {

                SpecialFolder specialFolder = IO.Path.GetSpecialFolder(_shellObject);

                FileType fileType = specialFolder == SpecialFolder.OtherFolderOrFile ? FileType.Folder : FileType.SpecialFolder;

                return (fileType, specialFolder);

            }

            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Archive) || (FileType == FileType.SpecialFolder && ShellObject.IsFileSystemObject))

            {

                DirectoryInfo parentDirectoryInfo = FileType == FileType.Archive ? new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) : Directory.GetParent(Path);

                string parent = parentDirectoryInfo.FullName;

                var shellObject = ShellObject.FromParsingName(parent);

                (FileType fileType, SpecialFolder specialFolder) = getFileType(shellObject);

                return new ShellObjectInfo(parent, fileType, specialFolder, shellObject);

            }

            else if (FileType == FileType.Drive)

                return new ShellObjectInfo(KnownFolders.Computer.Path, FileType.SpecialFolder, SpecialFolder.Computer, ShellObject.FromParsingName(KnownFolders.Computer.ParsingName));

            else if (FileType == FileType.SpecialFolder && SpecialFolder != SpecialFolder.Computer)

            {

                ShellObject shellObject = ShellObject.Parent;

                string path = Path;

                if (path.EndsWith(PathSeparator.ToString()))

                    path = path.Remove(path.Length - 1);

                (FileType fileType, SpecialFolder specialFolder) = getFileType(shellObject);

                return new ShellObjectInfo(path.Remove(path.LastIndexOf(PathSeparator)), fileType, specialFolder, shellObject);

            }

            else return null;

        }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);

            ShellObject.Dispose();

            if (disposing)

                ShellObject = null;

            //if (ArchiveFileStream != null)

            //{

            //    ArchiveFileStream.Dispose();

            //    ArchiveFileStream.Close();

            //}

        }

        ///// <summary>
        ///// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        ///// </summary>
        ///// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
        //protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo) => base.OnDeepClone(browsableObjectInfo);

        ///// <summary>
        ///// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        ///// </summary>
        //protected override BrowsableObjectInfo DeepCloneOverride() => new ShellObjectInfo(Path, FileType, SpecialFolder, _shellObjectDelegate(ShellObject), _shellObjectDelegate);

        #endregion

        private void SetFileSystemInfoProperties(ShellObject shellObject)
        {

            if (FileType == FileType.Folder || (FileType == FileType.SpecialFolder && shellObject.IsFileSystemObject))

                FileSystemInfoProperties = new DirectoryInfo(Path);

            else if (FileType == FileType.File || FileType == FileType.Archive || FileType == FileType.Link)

                FileSystemInfoProperties = new FileInfo(Path);

            else if (FileType == FileType.Drive)

            {

                FileSystemInfoProperties = new DirectoryInfo(Path);

                DriveInfoProperties = new DriveInfo(Path);

            }

            else if (FileType == FileType.SpecialFolder)

                KnownFolderInfo = KnownFolderHelper.FromParsingName(shellObject.ParsingName);

        }

        public string ItemTypeName => ShellObject.Properties.System.ItemTypeText.Value;

        public Size Size => Size.Create(ShellObject.Properties.System.Size.Value);

        public void GetItems()

        {

            // if (FileTypes == FileTypes.None) return;

#if DEBUG

            Debug.WriteLine("Dowork event started.");

            try
            {

                Debug.WriteLine("Path == null: " + (Path == null).ToString());

                Debug.WriteLine("Path.Path: " + Path);

                Debug.WriteLine("ShellObject: " + ShellObject.ToString());

            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) { }
#pragma warning restore CA1031 // Do not catch general exception types

#endif

            var paths = new ArrayBuilder<PathInfo>();

            void AddPath(ref string _path, FileType fileType, ShellObject _shellObject)

            {

#if DEBUG

                if (_path.EndsWith(".lnk"))

                    Debug.WriteLine("");

#endif

                if (fileType == FileType.Other || (fileType != FileType.SpecialFolder && FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

                // We only make a normalized path if we add the path to the paths to load.

                if (_path.StartsWith("::"))

                {

                    try

                    {

                        using (IKnownFolder knownFolder = KnownFolderHelper.FromKnownFolderId(Guid.Parse(pathInfo.Path.Substring(3, pathInfo.Path.IndexOf('}') - 3))))

                        {

                            string __path = pathInfo.Path;

                            __path = knownFolder.Path;

                            if (pathInfo.Path.Contains(IO.Path.PathSeparator))

                                __path += pathInfo.Path.Substring(pathInfo.Path.IndexOf(IO.Path.PathSeparator)) + 1;

                        }

                    }

                    catch (ShellException) { }

                    var browsableObjectInfo = (IBrowsableObjectInfo)Path;

                    IBrowsableObjectInfo newPath;

                    _path = browsableObjectInfo.Name;

                    browsableObjectInfo = browsableObjectInfo.Parent;

                    while (browsableObjectInfo != null)

                    {

                        _path = browsableObjectInfo.Name + IO.Path.PathSeparator + _path;

                        newPath = browsableObjectInfo.Parent;

                        browsableObjectInfo.Dispose();

                    }

                }

                PropertyInfo[] props = typeof(KnownFolders).GetProperties();

                string path = pathInfo.Path;

                if (path.Contains(IO.Path.PathSeparator))

                    path = path.Substring(0, path.IndexOf(IO.Path.PathSeparator));

                foreach (PropertyInfo prop in props)

                    using (IKnownFolder knownFolder = (IKnownFolder)prop.GetValue(null))
                    {

                        // string displayPath =

                        if (path == knownFolder.LocalizedName)

                            ok = true;

                        else

                            using (ShellObject shellObject = ShellObject.FromParsingName(knownFolder.ParsingName))

                                if (path == shellObject.Name)

                                    ok = true;

                        // if (ok)

                    }

                paths.AddLast(new PathInfo(path, path.RemoveAccents(), fileType, _shellObject, ShellObjectInfo.DefaultShellObjectDeepClone));

            }

            void AddDirectory(string path, ShellObject _shellObject) =>

                // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

                AddPath(ref path, _shellObject.IsFileSystemObject ? System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder : FileType.SpecialFolder, _shellObject);

            void AddFile(string path, ShellObject _shellObject) =>

                // We only make a normalized path if we add the path to the paths to load.

                AddPath(ref path, _shellObject.IsLink
                    ? FileType.Link
                    : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) ? FileType.Archive : FileType.File, _shellObject);

            try
            {

                if (ShellObject.IsFileSystemObject)

                {

                    string[] directories = Directory.GetDirectories(Path);

                    ShellObject shellObject = null;

                    foreach (string directory in directories)

                        if (CheckFilter(directory))

                            AddDirectory(directory, ShellObject.FromParsingName(directory));

                    string[] files = Directory.GetFiles(Path);

                    foreach (string file in files)

                        if (CheckFilter(file))

                            AddFile(file, (shellObject = ShellObject.FromParsingName(file)));

                }

                else

                {

                    //string _path = null;

                    //PathInfo pathInfo;

                    foreach (ShellObject so in (ShellContainer)ShellObject)

                        //#if DEBUG

                        //                    {

                        //                        Debug.WriteLine(Path.Path + ": " + ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject.ToString());

                        //                        Debug.WriteLine(so.ParsingName + ": " + so.IsFileSystemObject.ToString());

                        //                        Debug.WriteLine(so.GetType().ToString());

                        //                        Debug.WriteLine((so is ShellFolder).ToString());

                        //#endif

                        if (so is ShellFile shellFile)

                            AddFile(shellFile.Path, shellFile);

                        else if (so is ShellLink shellLink)

                            AddFile(shellLink.Path, shellLink);

                        // if (so is FileSystemKnownFolder || so is NonFileSystemKnownFolder || so is ShellNonFileSystemFolder || so is ShellLibrary)

                        // if (File.Exists(_path))

                        // AddFile(pathInfo, so.IsLink);

                        else

                            AddDirectory(so.ParsingName, so);

                }

            }

            catch (Exception ex) when (HandleIOException(ex))
            {

#if DEBUG

                Debug.WriteLine(ex.GetType().ToString() + " " + ex.Message);

#endif

            }



            // Debug.WriteLine("cFileTypes: " + FileTypes.ToString());



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
                            // Debug.WriteLine("Path.Factory is null: " + (Path.Factory is null).ToString());

#endif

                            // new_Path.LoadThumbnail();

                            // Debug.WriteLine("FileTypes: " + FileTypes.ToString());

                            ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>((TItems)Path.Factory.GetBrowsableObjectInfo(path_.Path, path_.FileType, IO.Path.GetSpecialFolder(path_.ShellObject), path_.ShellObject, ShellObjectInfo.DefaultShellObjectDeepClone), (TItemsFactory)Path.Factory.DeepClone()));

                        } while (_paths.MoveNext());

                    }
                    catch (Exception ex) when (HandleIOException(ex)) { }

        }

        protected class PathInfo : IO.PathInfo

        {

            public FileType FileType { get; }

            public ShellObject ShellObject { get; }

            public DeepClone<ShellObject> ShellObjectDelegate { get; }

            /// <summary>
            /// Gets the localized name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string LocalizedName => Name;

            /// <summary>
            /// Gets the name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string Name => ShellObject.GetDisplayName(DisplayNameType.Default);

            public PathInfo(string path, string normalizedPath, FileType fileType, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) : base(path, normalizedPath)
            {

                ShellObject = shellObject;

                ShellObjectDelegate = shellObjectDelegate;

                FileType = fileType;

            }

            //public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
            //        ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
            //        : false;

            //public int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

        }

    }

}
