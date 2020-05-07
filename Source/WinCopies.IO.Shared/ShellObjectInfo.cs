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
using Microsoft.WindowsAPICodePack.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Guids;
using System.Linq;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using SevenZip;
using WinCopies.Linq;

namespace WinCopies.IO
{

    /// <summary>
    /// Represents an archive item. This struct is used in enumeration methods.
    /// </summary>
    public struct ArchiveFileInfoEnumeratorStruct
    {
        /// <summary>
        /// Gets the path of the current archive item. This property is set only when <see cref="ArchiveFileInfo"/> is <see langword="null"/>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the <see cref="SevenZip.ArchiveFileInfo"/> that represents the cyrrent archive item. This property is set only when <see cref="Path"/> is <see langword="null"/>.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFileInfoEnumeratorStruct"/> struct with the given path.
        /// </summary>
        /// <param name="path">The path of the archive item.</param>
        public ArchiveFileInfoEnumeratorStruct(string path)
        {
            Path = path ?? throw GetArgumentNullException(nameof(path));

            ArchiveFileInfo = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFileInfoEnumeratorStruct"/> struct with the given <see cref="SevenZip.ArchiveFileInfo"/>.
        /// </summary>
        /// <param name="path">The <see cref="SevenZip.ArchiveFileInfo"/> that represents the archive item.</param>
        public ArchiveFileInfoEnumeratorStruct(ArchiveFileInfo archiveFileInfo)
        {
            Path = null;

            ArchiveFileInfo = archiveFileInfo;
        }
    }

    /// <summary>
    /// Represents a file system item.
    /// </summary>
    public class ShellObjectInfo/*<TItems, TArchiveItemInfoItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IShellObjectInfo // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TArchiveItemInfoItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        private IBrowsableObjectInfo _parent;

        #region Properties

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether the current item has particularities.
        /// </summary>
        public override bool IsSpecialItem
        {
            get
            {
                uint? value = ShellObject.Properties.System.FileAttributes.Value;

                if (value.HasValue)

                {
                    var _value = (Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes)value.Value;

                    return _value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Hidden) || _value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.System);
                }

                else

                    return false;
            }
        }

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
        public override bool IsBrowsable => /*(*/ShellObject is IEnumerable<ShellObject>/*) && FileType != FileType.File && FileType != FileType.Link*/; // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

        public Stream ArchiveFileStream { get; private set; }          /*new FileStream(_archiveItemInfoProvider.ArchiveShellObject.Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)*/

        public bool IsArchiveOpen => ArchiveFileStream is object;

        public override string ItemTypeName => ShellObject.Properties.System.ItemTypeText.Value;

        public override Size? Size
        {
            get
            {
                ulong? value = ShellObject.Properties.System.Size.Value;

                if (value.HasValue)

                    return new Size(value.Value);

                else

                    return null;
            }
        }

#if NETFRAMEWORK

        public override IBrowsableObjectInfo Parent => _parent ?? (_parent = GetParent());

#else

        public override IBrowsableObjectInfo Parent => _parent ??= GetParent();

#endif

        ///// <summary>
        ///// Gets a <see cref="FileSystemInfo"/> object that provides info for the folders and files. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a folder, a drive or a file.
        ///// </summary>
        ///// <seealso cref="FileSystemObjectInfo.FileType"/>
        //public FileSystemInfo FileSystemInfoProperties { get; private set; } = null;

        ///// <summary>
        ///// Gets a <see cref="DriveInfo"/> object that provides info for drives. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a drive.
        ///// </summary>
        ///// <seealso cref="FileSystemObjectInfo.FileType"/>
        //public DriveInfo DriveInfoProperties { get; private set; } = null;

        ///// <summary>
        ///// Gets a <see cref="IKnownFolder"/> object that provides info for the system known folders. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a system known folder. See the <see cref="FileSystemObjectInfo.FileType"/> property for more details.
        ///// </summary>
        //public IKnownFolder KnownFolderInfo { get; private set; } = null;

        ///// <summary>
        ///// Gets the special folder type of this <see cref="ShellObjectInfo"/>. <see cref="SpecialFolder.None"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.
        ///// </summary>
        //public SpecialFolder SpecialFolder { get; private set; } = SpecialFolder.None;

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo"/>s and <see cref="ArchiveItemInfo"/>s.
        ///// </summary>
        ///// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        ///// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        ///// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.None"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        private ShellObjectInfo(string path, FileType fileType, ShellObject shellObject) : base(path, fileType) => ShellObject = shellObject;

        // Init(specialFolder, shellObjectDelegate, shellObject, nameof(fileType), archiveItemInfoFactory); // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        // private void Init(SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject, string fileTypeParameterName, IArchiveItemInfoFactory archiveItemInfoFactory)

        //{

        //#if DEBUG

        //            if (shellObject.ParsingName != Path)

        //                Debug.WriteLine("");

        //#endif

        //if ((FileType == FileType.KnownFolder && specialFolder == SpecialFolder.None) || (FileType != FileType.KnownFolder && specialFolder != SpecialFolder.None))

        //    throw new ArgumentException(string.Format(Properties.Resources.FileTypeAndSpecialFolderDoNotCorrespond, nameof(fileType), nameof(specialFolder), FileType, specialFolder));



        // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

        // NormalizedPath = Util.GetNormalizedPath(path);

        //SpecialFolder = specialFolder;

        //SetFileSystemInfoProperties(shellObject);

        //}

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

        #region Methods

        public static ShellObjectInfo From(ShellObject shellObject)
        {

            if ((shellObject ?? throw GetArgumentNullException(nameof(shellObject))) is ShellFolder shellFolder)

            {

                if (shellObject is ShellFileSystemFolder shellFileSystemFolder)

                {

                    (string path, FileType fileType) = shellFileSystemFolder is FileSystemKnownFolder ? (shellObject.ParsingName, FileType.KnownFolder) : (shellFileSystemFolder.Path, FileType.Folder);

                    if (Directory.GetParent(path) is null)

                        return new ShellObjectInfo(path, FileType.Drive, shellObject);

                    return new ShellObjectInfo(path, fileType, shellObject);

                }

                if (shellObject is NonFileSystemKnownFolder nonFileSystemKnownFolder)

                    return new ShellObjectInfo(nonFileSystemKnownFolder.Path, FileType.KnownFolder, shellObject);

                else if (shellObject is ShellNonFileSystemFolder)

                    return new ShellObjectInfo(shellObject.ParsingName, FileType.Folder, shellObject);

            }

            if (shellObject is ShellLink shellLink)

                return new ShellObjectInfo(shellLink.Path, FileType.Link, shellObject);

            if (shellObject is ShellFile shellFile)

                return new ShellObjectInfo(shellFile.Path, IsSupportedArchiveFormat(System.IO.Path.GetExtension(shellFile.Path)) ? FileType.Archive : shellFile.IsLink ? FileType.Link : System.IO.Path.GetExtension(shellFile.Path) == ".library-ms" ? FileType.Library : FileType.File, shellObject);

            throw new ArgumentException($"The given {nameof(Microsoft.WindowsAPICodePack.Shell.ShellObject)} is not supported.");

        }

        public void OpenArchive(Stream stream)
        {
            CloseArchive();

            FileType.ThrowIfInvalidEnumValue(true, FileType.Archive);

            ArchiveFileStream = stream;
        }

        public void CloseArchive()
        {
            ArchiveFileStream.Flush();

            ArchiveFileStream.Dispose();

            ArchiveFileStream = null;
        }

        /// <summary>
        /// Gets a string representation of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="ShellObjectInfo"/>.</returns>
        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Returns the parent of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="ShellObjectInfo"/>.</returns>
        private IBrowsableObjectInfo GetParent()
        {

            //#if !NETFRAMEWORK

            //            static

            //#endif

            //    (FileType, SpecialFolder) getFileType(ShellObject _shellObject)

            //{

            //    SpecialFolder specialFolder = IO.Path.GetSpecialFolder(_shellObject);

            //    FileType fileType = specialFolder == SpecialFolder.None ? FileType.Folder : FileType.KnownFolder;

            //    return (fileType, specialFolder);

            //}

            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Archive) || (FileType == FileType.KnownFolder && ShellObject.IsFileSystemObject) || (FileType == FileType.KnownFolder && ((IKnownFolder)ShellObject).FolderId.ToString() != Microsoft.WindowsAPICodePack.Shell.Guids.KnownFolders.Computer))

                return From(ShellObject.Parent);

            else if (FileType == FileType.Drive)

                return new ShellObjectInfo(Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer.Path, FileType.KnownFolder, ShellObject.FromParsingName(Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer.ParsingName));

            else return null;

        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);

            ShellObject.Dispose();

            if (IsArchiveOpen)

                CloseArchive();

            if (disposing)

                ShellObject = null;

        }

        public virtual IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<ShellObject> func)
        {
            ThrowIfNull(func, nameof(func));

            switch (FileType)
            {
                case FileType.Drive:
                case FileType.Folder:
                case FileType.KnownFolder:
                case FileType.Library:
                    return ((IEnumerable<ShellObject>)ShellObject).Where(func).Select(shellObject => From(shellObject));
                default:
                    return null;
            }
        }

        public virtual IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<ArchiveFileInfoEnumeratorStruct> func)
        {
            ThrowIfNull(func, nameof(func));

#if NETFRAMEWORK

            switch (FileType)
            {
                case FileType.Archive:
                    return GetArchiveItemInfoItems(func);
                default:
                    return null;
            }

#else

            return FileType switch
            {
                FileType.Archive => GetArchiveItemInfoItems(func),
                _ => null,
            };

#endif
        }

        public override IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<IBrowsableObjectInfo> func) => IsBrowsable
                ? func is null ? ((IEnumerable<ShellObject>)ShellObject).Select(shellObject => From(shellObject)) : ((IEnumerable<ShellObject>)ShellObject).Select(shellObject => From(shellObject)).Where(func)
                : null;

        private IEnumerable<IBrowsableObjectInfo> GetArchiveItemInfoItems(Predicate<ArchiveFileInfoEnumeratorStruct> func)
        {
#if NETCORE
            using var enumerator = new ArchiveItemInfoEnumerator(this, func);
#else
            using (var enumerator = new ArchiveItemInfoEnumerator(this, func))
#endif

            while (enumerator.MoveNext())

                yield return enumerator.Current;
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

        //private void SetFileSystemInfoProperties(ShellObject shellObject)
        //{

        //    if (FileType == FileType.Folder || (FileType == FileType.KnownFolder && shellObject.IsFileSystemObject))

        //        FileSystemInfoProperties = new DirectoryInfo(Path);

        //    else if (FileType == FileType.File || FileType == FileType.Archive || FileType == FileType.Link)

        //        FileSystemInfoProperties = new FileInfo(Path);

        //    else if (FileType == FileType.Drive)

        //    {

        //        FileSystemInfoProperties = new DirectoryInfo(Path);

        //        DriveInfoProperties = new DriveInfo(Path);

        //    }

        //    else if (FileType == FileType.KnownFolder)

        //        KnownFolderInfo = KnownFolderHelper.FromParsingName(shellObject.ParsingName);

        //}

#endregion

        //{

        //            if (fileTypes == FileTypes.None) return new System.Collections.ObjectModel.ReadOnlyCollection<IBrowsableObjectInfo>(new List<IBrowsableObjectInfo>());

        //            //#if DEBUG

        //            //            Debug.WriteLine("Dowork event started.");

        //            //            try
        //            //            {

        //            //                Debug.WriteLine("Path == null: " + (Path == null).ToString());

        //            //                Debug.WriteLine("Path.Path: " + Path);

        //            //                Debug.WriteLine("ShellObject: " + ShellObject.ToString());

        //            //            }
        //            //#pragma warning disable CA1031 // Do not catch general exception types
        //            //            catch (Exception) { }
        //            //#pragma warning restore CA1031 // Do not catch general exception types

        //            //#endif

        //            var paths = new ArrayBuilder<PathInfo>();

        //            void AddPath(ref string _path, FileType fileType, ShellObject _shellObject)

        //            {

        //#if DEBUG

        //                if (_path.EndsWith(".lnk"))

        //                    Debug.WriteLine("");

        //#endif

        //                if (fileType == FileType.Other || (fileType != FileType.KnownFolder && FileTypes != GetAllEnumFlags<FileTypes>() && !FileTypes.HasFlag(FileTypeToFileTypeFlags(fileType)))) return;

        //                // We only make a normalized path if we add the path to the paths to load.

        //                if (_path.StartsWith("::"))

        //                {

        //                    try

        //                    {

        //                        using (IKnownFolder knownFolder = KnownFolderHelper.FromKnownFolderId(Guid.Parse(pathInfo.Path.Substring(3, pathInfo.Path.IndexOf('}') - 3))))

        //                        {

        //                            string __path = pathInfo.Path;

        //                            __path = knownFolder.Path;

        //                            if (pathInfo.Path.Contains(IO.Path.PathSeparator))

        //                                __path += pathInfo.Path.Substring(pathInfo.Path.IndexOf(IO.Path.PathSeparator)) + 1;

        //                        }

        //                    }

        //                    catch (ShellException) { }

        //                    var browsableObjectInfo = (IBrowsableObjectInfo)Path;

        //                    IBrowsableObjectInfo newPath;

        //                    _path = browsableObjectInfo.Name;

        //                    browsableObjectInfo = browsableObjectInfo.Parent;

        //                    while (browsableObjectInfo != null)

        //                    {

        //                        _path = browsableObjectInfo.Name + IO.Path.PathSeparator + _path;

        //                        newPath = browsableObjectInfo.Parent;

        //                        browsableObjectInfo.Dispose();

        //                    }

        //                }

        //                PropertyInfo[] props = typeof(KnownFolders).GetProperties();

        //                string path = pathInfo.Path;

        //                if (path.Contains(IO.Path.PathSeparator))

        //                    path = path.Substring(0, path.IndexOf(IO.Path.PathSeparator));

        //                foreach (PropertyInfo prop in props)

        //                    using (IKnownFolder knownFolder = (IKnownFolder)prop.GetValue(null))
        //                    {

        //                        // string displayPath =

        //                        if (path == knownFolder.LocalizedName)

        //                            ok = true;

        //                        else

        //                            using (ShellObject shellObject = ShellObject.FromParsingName(knownFolder.ParsingName))

        //                                if (path == shellObject.Name)

        //                                    ok = true;

        //                        // if (ok)

        //                    }

        //                paths.AddLast(new PathInfo(path, path.RemoveAccents(), fileType, _shellObject, ShellObjectInfo.DefaultShellObjectDeepClone));

        //            }

        //            void AddDirectory(string path, ShellObject _shellObject) =>

        //                // if (FileTypes.HasFlag(FileTypesFlags.All) || (FileTypes.HasFlag(FileTypesFlags.Folder) && System.IO.Path.GetPathRoot(pathInfo.Path) != pathInfo.Path) || (FileTypes.HasFlag(FileTypesFlags.Drive) && System.IO.Path.GetPathRoot(pathInfo.Path) == pathInfo.Path))

        //                AddPath(ref path, _shellObject.IsFileSystemObject ? System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder : FileType.KnownFolder, _shellObject);

        //            void AddFile(string path, ShellObject _shellObject) =>

        //                // We only make a normalized path if we add the path to the paths to load.

        //                AddPath(ref path, _shellObject.IsLink
        //                    ? FileType.Link
        //                    : IO.Path.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) ? FileType.Archive : FileType.File, _shellObject);

        //            try
        //            {

        //                if (ShellObject.IsFileSystemObject)

        //                {

        //                    string[] directories = Directory.GetDirectories(Path);

        //                    ShellObject shellObject = null;

        //                    foreach (string directory in directories)

        //                        if (CheckFilter(directory))

        //                            AddDirectory(directory, ShellObject.FromParsingName(directory));

        //                    string[] files = Directory.GetFiles(Path);

        //                    foreach (string file in files)

        //                        if (CheckFilter(file))

        //                            AddFile(file, (shellObject = ShellObject.FromParsingName(file)));

        //                }

        //                else

        //                {

        //                    //string _path = null;

        //                    //PathInfo pathInfo;

        //                    foreach (ShellObject so in (ShellContainer)ShellObject)

        //                        //#if DEBUG

        //                        //                    {

        //                        //                        Debug.WriteLine(Path.Path + ": " + ((ShellObjectInfo)Path).ShellObject.IsFileSystemObject.ToString());

        //                        //                        Debug.WriteLine(so.ParsingName + ": " + so.IsFileSystemObject.ToString());

        //                        //                        Debug.WriteLine(so.GetType().ToString());

        //                        //                        Debug.WriteLine((so is ShellFolder).ToString());

        //                        //#endif

        //                        if (so is ShellFile shellFile)

        //                            AddFile(shellFile.Path, shellFile);

        //                        else if (so is ShellLink shellLink)

        //                            AddFile(shellLink.Path, shellLink);

        //                        // if (so is FileSystemKnownFolder || so is NonFileSystemKnownFolder || so is ShellNonFileSystemFolder || so is ShellLibrary)

        //                        // if (File.Exists(_path))

        //                        // AddFile(pathInfo, so.IsLink);

        //                        else

        //                            AddDirectory(so.ParsingName, so);

        //                }

        //            }

        //            catch (Exception ex) when (HandleIOException(ex))
        //            {

        //#if DEBUG

        //                Debug.WriteLine(ex.GetType().ToString() + " " + ex.Message);

        //#endif

        //            }



        //            // Debug.WriteLine("cFileTypes: " + FileTypes.ToString());



        //            IEnumerable<PathInfo> pathInfos;



        //            if (FileSystemObjectComparer == null)

        //                pathInfos = (IEnumerable<PathInfo>)paths;

        //            else

        //            {

        //                var _paths = paths.ToList();

        //                _paths.Sort(FileSystemObjectComparer);

        //                pathInfos = (IEnumerable<PathInfo>)_paths;

        //            }



        //            PathInfo path_;



        //            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())



        //                while (_paths.MoveNext())

        //                    try

        //                    {

        //                        do

        //                        {

        //                            path_ = _paths.Current;

        //#if DEBUG

        //                            Debug.WriteLine("Current thread is background: " + System.Threading.Thread.CurrentThread.IsBackground);
        //                            Debug.WriteLine("path_.Path: " + path_.Path);
        //                            Debug.WriteLine("path_.Normalized_Path: " + path_.NormalizedPath);
        //                            Debug.WriteLine("path_.Shell_Object: " + path_.ShellObject);
        //                            // Debug.WriteLine("Path.Factory is null: " + (Path.Factory is null).ToString());

        //#endif

        //                            // new_Path.LoadThumbnail();

        //                            // Debug.WriteLine("FileTypes: " + FileTypes.ToString());

        //                            ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>((TItems)Path.Factory.GetBrowsableObjectInfo(path_.Path, path_.FileType, IO.Path.GetSpecialFolder(path_.ShellObject), path_.ShellObject, ShellObjectInfo.DefaultShellObjectDeepClone), (TItemsFactory)Path.Factory.DeepClone()));

        //                        } while (_paths.MoveNext());

        //                    }
        //                    catch (Exception ex) when (HandleIOException(ex)) { }

        //}

        //protected class PathInfo : IO.PathInfo

        //{

        //    public FileType FileType { get; }

        //    public ShellObject ShellObject { get; }

        //    public DeepClone<ShellObject> ShellObjectDelegate { get; }

        //    /// <summary>
        //    /// Gets the localized name of this <see cref="PathInfo"/>.
        //    /// </summary>
        //    public override string LocalizedName => Name;

        //    /// <summary>
        //    /// Gets the name of this <see cref="PathInfo"/>.
        //    /// </summary>
        //    public override string Name => ShellObject.GetDisplayName(DisplayNameType.Default);

        //    public PathInfo(string path, string normalizedPath, FileType fileType, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) : base(path, normalizedPath)
        //    {

        //        ShellObject = shellObject;

        //        ShellObjectDelegate = shellObjectDelegate;

        //        FileType = fileType;

        //    }

        //    //public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
        //    //        ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
        //    //        : false;

        //    //public int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

        //}

    }

}
