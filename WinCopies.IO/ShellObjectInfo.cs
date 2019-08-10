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

namespace WinCopies.IO
{

    /// <summary>
    /// Represents a file system item that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
    /// </summary>
    public class ShellObjectInfo : ArchiveItemInfoProvider, IShellObjectInfo
    {

        private ArchiveItemInfoFactory _archiveItemInfoFactory;

        /// <summary>
        /// Gets or sets the factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy.</exception>
        /// <exception cref="ArgumentNullException">The given value is null.</exception>
        public override ArchiveItemInfoFactory ArchiveItemInfoFactory
        {
            get => _archiveItemInfoFactory; set
            {

                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

                _archiveItemInfoFactory.Path = null;

                value.Path = this;

                _archiveItemInfoFactory = value;

            }
        }

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
        /// Gets a <see cref="FileSystemInfo"/> object that provides info for the folders and files. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a folder, drive or file. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        public FileSystemInfo FileSystemInfoProperties { get; private set; } = null;

        /// <summary>
        /// Gets a <see cref="DriveInfo"/> object that provides info for drives. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a drive. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        public DriveInfo DriveInfoProperties { get; private set; } = null;

        /// <summary>
        /// Gets a <see cref="IKnownFolder"/> object that provides info for the system known folders. This property returns <see langword="null"/> when this <see cref="ShellObjectInfo"/> is not a system known folder. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        public IKnownFolder KnownFolderInfo { get; private set; } = null;

        //private FileStream _archiveFileStream = null;

        ///// <summary>
        ///// The <see cref="FileStream"/> for this <see cref="ShellObjectInfo"/> when it represents an archive file system item. See the remarks section.
        ///// </summary>
        ///// <remarks>
        ///// This field is only used by the <see cref="ShellObjectInfo"/>, <see cref="FolderLoader"/> and the <see cref="ArchiveLoader"/> classes in order to lock the file that the <see cref="ShellObjectInfo"/> represents when the items of the archive are loaded.
        ///// </remarks>
        //public FileStream ArchiveFileStream { get => _archiveFileStream; internal set => OnPropertyChanged(nameof(ArchiveFileStream), nameof(_archiveFileStream), value, typeof(ShellObjectInfo)); }

        /// <summary>
        /// Gets the special folder type of this <see cref="ShellObjectInfo"/>. <see cref="SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.
        /// </summary>
        public SpecialFolder SpecialFolder { get; private set; } = SpecialFolder.OtherFolderOrFile;

        ///// <summary>
        ///// Gets the <see cref="IO.FileType"/> and <see cref="IO.SpecialFolder"/> for a given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
        ///// </summary>
        ///// <param name="path">The path from which to get the associated <see cref="FileType"/>.</param>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to get the associated <see cref="FileType"/>.</param>
        ///// <returns>The <see cref="FileType"/> and <see cref="IO.SpecialFolder"/> for the given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
        //public static (FileType fileType, SpecialFolder specialFolder) GetFileType(string path, ShellObject shellObject) => (!shellObject.IsFileSystemObject
        //        ? FileType.SpecialFolder
        //        : shellObject is FileSystemKnownFolder && ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) && shellObject is ShellFile
        //        ? FileType.Archive
        //        : shellObject is ShellFile
        //        ? shellObject.IsLink
        //            ? FileType.Link
        //            : ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) ? FileType.Archive : FileType.File
        //        : System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder, GetSpecialFolderType(shellObject));

        ///// <summary>
        ///// Returns the <see cref="IO.SpecialFolder"/> value for a given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
        ///// </summary>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to return a <see cref="IO.SpecialFolder"/> value.</param>
        ///// <returns>A <see cref="IO.SpecialFolder"/> value that correspond to the given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
        //public static SpecialFolder GetSpecialFolderType(ShellObject shellObject)

        //{

        //    SpecialFolder? value = null;

        //    PropertyInfo[] knownFoldersProperties = typeof(KnownFolders).GetProperties();

        //    for (int i = 1 ; i < knownFoldersProperties.Length; i++)

        //        try
        //        {

        //            for (; i < knownFoldersProperties.Length; i++)

        //                if (shellObject.ParsingName == (knownFoldersProperties[i].GetValue(null) as IKnownFolder)?.ParsingName)

        //                    value = (SpecialFolder)typeof(SpecialFolder).GetField(knownFoldersProperties[i].Name).GetValue(null);

        //            break;

        //        }

        //        catch (ShellException) { i++; }

        //    #region Comments

        //    //    else if (shellObject.ParsingName == KnownFolders.MusicLibrary.ParsingName)

        //    //    value = SpecialFolder.MusicLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.PicturesLibrary.ParsingName)

        //    //    value = SpecialFolder.PicturesLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.CameraRollLibrary.ParsingName)

        //    //    value = SpecialFolder.CameraRollLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.SavedPicturesLibrary.ParsingName)

        //    //    value = SpecialFolder.SavedPicturesLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.RecordedTVLibrary.ParsingName)

        //    //    value = SpecialFolder.RecordedTVLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.VideosLibrary.ParsingName)

        //    //    value = SpecialFolder.VideosLibrary;

        //    //else if (shellObject.ParsingName == KnownFolders.UsersLibraries.ParsingName)

        //    //    value = SpecialFolder.UsersLibraries;

        //    //else if (shellObject.ParsingName == KnownFolders.Libraries.ParsingName)

        //    //    value = SpecialFolder.Libraries;

        //    //else if (shellObject.ParsingName == KnownFolders.Computer.ParsingName)

        //    //    value = SpecialFolder.Computer;

        //    //else

        //    //    value = SpecialFolder.OtherFolderOrFile;

        //    #endregion

        //    return value ?? SpecialFolder.OtherFolderOrFile;

        //}

        /// <summary>
        /// Returns the <see cref="IO.SpecialFolder"/> value for a given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to return a <see cref="IO.SpecialFolder"/> value.</param>
        /// <returns>A <see cref="IO.SpecialFolder"/> value that correspond to the given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
        public static SpecialFolder GetSpecialFolder(ShellObject shellObject)

        {

            SpecialFolder? value = null;

            PropertyInfo[] knownFoldersProperties = typeof(KnownFolders).GetProperties();

            for (int i = 1; i < knownFoldersProperties.Length; i++)

                try
                {

                    for (; i < knownFoldersProperties.Length; i++)

                        if (shellObject.ParsingName == knownFoldersProperties[i].Name)

                            value = (SpecialFolder)typeof(SpecialFolder).GetField(knownFoldersProperties[i].Name).GetValue(null);

                    break;

                }

                catch (ShellException) { i++; }

            return value ?? SpecialFolder.OtherFolderOrFile;

        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ShellObjectInfo"/> class.
        ///// </summary>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        ///// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        //public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) : this(shellObject, path, fileType, specialFolder, new ShellObjectInfoFactory(), new ArchiveItemInfoFactory()) { }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ShellObjectInfo"/> class using custom factories for <see cref="ShellObjectInfo"/> and <see cref="ArchiveItemInfo"/>.
        ///// </summary>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        ///// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        ///// <param name="shellObjectInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent casual file system items.</param>
        ///// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.</param>
        //public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder, ShellObjectInfoFactory shellObjectInfoFactory, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, archiveItemInfoFactory) =>

        //    Init(shellObject, nameof(FileType), specialFolder, shellObjectInfoFactory);

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public ShellObjectInfo(Func<ShellObject> shellObjectDelegate, string path, FileType fileType, SpecialFolder specialFolder) : this(shellObjectDelegate, path, fileType, specialFolder, new ShellObjectInfoFactory(), new ArchiveItemInfoFactory()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo"/>s and <see cref="ArchiveItemInfo"/>s.
        /// </summary>
        /// <param name="shellObjectDelegate">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        /// <param name="factory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>s and <see cref="ArchiveLoader"/>s use to create new objects that represent casual file system items.</param>
        /// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.</param>
        public ShellObjectInfo(Func<ShellObject> shellObjectDelegate, string path, FileType fileType, SpecialFolder specialFolder, ShellObjectInfoFactory factory, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, archiveItemInfoFactory) =>

            Init(shellObjectDelegate, nameof(fileType), specialFolder, factory); // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        private Func<ShellObject> _shellObjectDelegate;

        private void Init(Func<ShellObject> shellObjectDelegate, string fileTypeParameterName, SpecialFolder specialFolder, ShellObjectInfoFactory shellObjectInfoFactory)

        {

            _shellObjectDelegate = shellObjectDelegate;

            ShellObject shellObject = shellObjectDelegate();

#if DEBUG

            if (shellObject.ParsingName != Path)

                Debug.WriteLine("");

#endif

            if ((FileType == FileType.SpecialFolder && specialFolder == SpecialFolder.OtherFolderOrFile) || (FileType != FileType.SpecialFolder && specialFolder != SpecialFolder.OtherFolderOrFile))

                throw new ArgumentException(string.Format(Generic.FileTypeAndSpecialFolderNotCorrespond, fileTypeParameterName, nameof(specialFolder), FileType, specialFolder));

            Factory = shellObjectInfoFactory;

            ShellObject = shellObject;

            // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

            // NormalizedPath = Util.GetNormalizedPath(path);

            SpecialFolder = specialFolder;

            SetFileSystemInfoProperties(shellObject, false);

        }

        private void SetFileSystemInfoProperties(ShellObject shellObject, bool reinit)
        {

            if (reinit)

            {

                FileSystemInfoProperties = null;

                DriveInfoProperties = null;

                KnownFolderInfo = null;

            }

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

        /// <summary>
        /// Returns the parent of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="ShellObjectInfo"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

            if (FileType == FileType.Folder || FileType == FileType.Archive || (FileType == FileType.SpecialFolder && ShellObject.IsFileSystemObject))

            {

                DirectoryInfo parentDirectoryInfo = FileType == FileType.Archive ? new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) : Directory.GetParent(Path);

                if (parentDirectoryInfo != null)

                {

                    string _parent = parentDirectoryInfo.FullName;

                    Func<ShellObject> shellObjectDelegate = () => ShellObject.FromParsingName(_parent);

                    var shellObject = shellobjectdelegate

                    FileType fileType;

                    SpecialFolder specialFolder;

                    if (shellObject.IsFileSystemObject)

                    {

                        fileType = FileType.Folder;

                        specialFolder = SpecialFolder.OtherFolderOrFile;

                    }

                    else

                    {

                        fileType = FileType.SpecialFolder;

                        specialFolder = GetSpecialFolder(shellObject);

                    }

                    return Factory.GetBrowsableObjectInfo(() => shellObject, _parent, fileType, specialFolder);

                }

                else return null;

            }

            else return FileType == FileType.Drive
                ? Factory.GetBrowsableObjectInfo(ShellObject.Parent, KnownFolders.Computer.Path, FileType.SpecialFolder, SpecialFolder.Computer)
                : FileType == FileType.SpecialFolder && SpecialFolder != SpecialFolder.Computer
                ? Factory.GetBrowsableObjectInfo(ShellObject.Parent, KnownFolderHelper.FromParsingName(ShellObject.Parent.ParsingName).Path)
                : null;

        }

        /// <summary>
        /// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        /// </summary>
        /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{T}"/> will report progress.</param>
        /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{T}"/> will supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

                    LoadItems((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new FolderLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

                else if (FileType == FileType.Archive)

                    LoadItems((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new ArchiveLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            }

            else

                LoadItems((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new FolderLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            //else

            //{

            //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

            //    foreach (ShellObject item in items)

            //    {

            //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

            //    }

            //}

        }

        /// <summary>
        /// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        /// </summary>
        /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{T}"/> will report progress.</param>
        /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoLoader{T}"/> will supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

                    LoadItemsAsync((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new FolderLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

                else if (FileType == FileType.Archive)

                    LoadItemsAsync((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new ArchiveLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            }

            else

                LoadItemsAsync((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new FolderLoader(this, workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            //else

            //{

            //    IEnumerable<ShellObject> items = ShellObject as IEnumerable<ShellObject>;

            //    foreach (ShellObject item in items)

            //    {

            //        this.items.Add(new ShellObjectInfo(item, item.ParsingName));

            //    }

            //}

        }

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo"/> in memory.
        // /// </summary>

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeOverride(bool disposeItemsLoader, bool disposeItems, bool disposeParent, bool recursively)
        {

            base.DisposeOverride(disposeItemsLoader, disposeItems, disposeParent, recursively);

            ShellObject.Dispose();

            //if (ArchiveFileStream != null)

            //{

            //    ArchiveFileStream.Dispose();

            //    ArchiveFileStream.Close();

            //}

        }

        /// <summary>
        /// Gets a string representation of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="ShellObjectInfo"/>.</returns>
        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets or sets the factory for this <see cref="ShellObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ShellObjectInfo"/> and its associated <see cref="ItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public new ShellObjectInfoFactory Factory { get => (ShellObjectInfoFactory)base.Factory; set => base.Factory = value; }

        public override bool IsRenamingSupported => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.File, FileType.Drive, FileType.Archive, FileType.Link);

        /// <summary>
        /// Renames or move to a relative path, or both, the current <see cref="ShellObjectInfo"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo"/>.</param>
        public override void Rename(string newValue)

        {

            if (If(IfCT.Or, IfCM.Logical, IfComp.NotEqual, out string key, FileType, GetKeyValuePair(nameof(FileType.Folder), FileType.Folder), GetKeyValuePair(nameof(FileType.File), FileType.File), GetKeyValuePair(nameof(FileType.Drive), FileType.Drive)))

                throw new InvalidOperationException($"{nameof(FileType)} must have one of the following values: {nameof(FileType.Folder)}, {nameof(FileType.File)} or {nameof(FileType.Drive)}. The value was {key}.");

            string getNewPath() => System.IO.Path.GetDirectoryName(Path) + "\\" + newValue;

            switch (FileType)
            {

                case FileType.Folder:

                    Directory.Move(Path, getNewPath());

                    break;

                case FileType.File:

                    File.Move(Path, getNewPath());

                    break;

                case FileType.Drive:

                    DriveInfoProperties.VolumeLabel = newValue;

                    break;

            }

        }

        public override bool NeedsObjectsReconstruction => true;

        protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo, bool preserveIds)
        {

            base.OnDeepClone(browsableObjectInfo, preserveIds);

            if (ArchiveItemInfoFactory.UseRecursively)

                ((ShellObjectInfo)browsableObjectInfo).ArchiveItemInfoFactory = (ArchiveItemInfoFactory)ArchiveItemInfoFactory.DeepClone(preserveIds);

        }

        protected override BrowsableObjectInfo DeepCloneOverride(bool preserveIds) => new ShellObjectInfo(_shellObjectDelegate, Path, FileType, SpecialFolder);

    }

}
