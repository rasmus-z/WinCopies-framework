using Microsoft.WindowsAPICodePack.Shell;

using SevenZip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    /// <summary>
    /// Provides info to interact with file system items.
    /// </summary>
    public class ShellObjectInfo : BrowsableObjectInfo, IShellObjectInfo
    {
        private IShellObjectInfoFactory _shellObjectInfoFactory;
        private IArchiveItemInfoFactory _archiveItemInfoFactory;

        /// <summary>
        /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public ShellObject ShellObject { get; private set; } = null;

        ShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => this;

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

        /// <summary>
        /// Gets the <see cref="IO.FileType"/> and <see cref="IO.SpecialFolder"/> for a given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="shellObject"></param>
        /// <returns>The <see cref="FileType"/> and <see cref="IO.SpecialFolder"/> for the given path and <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
        public static (FileType fileType, SpecialFolder specialFolder) GetFileType(string path, ShellObject shellObject) => (!shellObject.IsFileSystemObject
                ? FileType.SpecialFolder
                : shellObject is FileSystemKnownFolder && ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) && shellObject is ShellFile
                ? FileType.Archive
                : shellObject is ShellFile
                ? path.EndsWith(".lnk")
                    ? FileType.Link
                    : ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) ? FileType.Archive : FileType.File
                : System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder, GetSpecialFolderType(shellObject));

        /// <summary>
        /// Returns the <see cref="IO.SpecialFolder"/> value for a given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> from which to return a <see cref="IO.SpecialFolder"/> value.</param>
        /// <returns>A <see cref="IO.SpecialFolder"/> value that correspond to the given <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/>.</returns>
        public static SpecialFolder GetSpecialFolderType(ShellObject shellObject)

        {

            SpecialFolder? value = null;

            PropertyInfo[] knownFoldersProperties = typeof(KnownFolders).GetProperties();

            for (int i = 0; i < knownFoldersProperties.Length; i++)

                try
                {

                    for (; i < knownFoldersProperties.Length; i++)

                        if (shellObject.ParsingName == ((IKnownFolder)knownFoldersProperties[i].GetValue(null)).ParsingName)

                            value = (SpecialFolder)typeof(SpecialFolder).GetField(knownFoldersProperties[i].Name).GetValue(null);

                    break;

                }

                catch (ShellException) { i++; }

            #region Comments

            //    else if (shellObject.ParsingName == KnownFolders.MusicLibrary.ParsingName)

            //    value = SpecialFolder.MusicLibrary;

            //else if (shellObject.ParsingName == KnownFolders.PicturesLibrary.ParsingName)

            //    value = SpecialFolder.PicturesLibrary;

            //else if (shellObject.ParsingName == KnownFolders.CameraRollLibrary.ParsingName)

            //    value = SpecialFolder.CameraRollLibrary;

            //else if (shellObject.ParsingName == KnownFolders.SavedPicturesLibrary.ParsingName)

            //    value = SpecialFolder.SavedPicturesLibrary;

            //else if (shellObject.ParsingName == KnownFolders.RecordedTVLibrary.ParsingName)

            //    value = SpecialFolder.RecordedTVLibrary;

            //else if (shellObject.ParsingName == KnownFolders.VideosLibrary.ParsingName)

            //    value = SpecialFolder.VideosLibrary;

            //else if (shellObject.ParsingName == KnownFolders.UsersLibraries.ParsingName)

            //    value = SpecialFolder.UsersLibraries;

            //else if (shellObject.ParsingName == KnownFolders.Libraries.ParsingName)

            //    value = SpecialFolder.Libraries;

            //else if (shellObject.ParsingName == KnownFolders.Computer.ParsingName)

            //    value = SpecialFolder.Computer;

            //else

            //    value = SpecialFolder.OtherFolderOrFile;

            #endregion

            return value ?? SpecialFolder.OtherFolderOrFile;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        public ShellObjectInfo(ShellObject shellObject, string path) : this(shellObject, path, new ShellObjectInfoFactory(), new ArchiveItemInfoFactory()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class using custom factories for <see cref="ShellObjectInfo"/> and <see cref="ArchiveItemInfo"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="shellObjectInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent casual file system items.</param>
        /// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.</param>
        public ShellObjectInfo(ShellObject shellObject, string path, IShellObjectInfoFactory shellObjectInfoFactory, IArchiveItemInfoFactory archiveItemInfoFactory) : base(path, GetFileType(path, shellObject).fileType) =>

            Init(shellObject, nameof(FileType), GetFileType(path, shellObject).specialFolder, shellObjectInfoFactory, archiveItemInfoFactory);

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) : this(shellObject, path, fileType, specialFolder, new ShellObjectInfoFactory(), new ArchiveItemInfoFactory()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo"/> and <see cref="ArchiveItemInfo"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        /// <param name="shellObjectInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent casual file system items.</param>
        /// <param name="archiveItemInfoFactory">The factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.</param>
        public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder, IShellObjectInfoFactory shellObjectInfoFactory, IArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType) =>

            Init(shellObject, nameof(fileType), specialFolder, shellObjectInfoFactory, archiveItemInfoFactory);// string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        private void Init(ShellObject shellObject, string fileTypeParameterName, SpecialFolder specialFolder, IShellObjectInfoFactory shellObjectInfoFactory, IArchiveItemInfoFactory archiveItemInfoFactory)

        {

#if DEBUG

            if (shellObject.ParsingName != Path)

                Debug.WriteLine("");

#endif

            if ((FileType == FileType.SpecialFolder && specialFolder == SpecialFolder.OtherFolderOrFile) || (FileType != FileType.SpecialFolder && specialFolder != SpecialFolder.OtherFolderOrFile))

                throw new ArgumentException(string.Format(Generic.FileTypeAndSpecialFolderNotCorrespond, fileTypeParameterName, nameof(specialFolder), FileType, specialFolder));

            ShellObjectInfoFactory = shellObjectInfoFactory;

            ArchiveItemInfoFactory = archiveItemInfoFactory;

            ShellObject = shellObject;

            // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

            // NormalizedPath = Util.GetNormalizedPath(path);

            SpecialFolder = specialFolder;

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

                    return ShellObjectInfoFactory.GetBrowsableObjectInfo(ShellObject.FromParsingName(_parent), _parent);

                }

                else return null;

            }

            else return FileType == FileType.Drive
                ? ShellObjectInfoFactory.GetBrowsableObjectInfo(ShellObject.Parent, KnownFolders.Computer.Path, FileType.SpecialFolder, SpecialFolder.Computer)
                : FileType == FileType.SpecialFolder && SpecialFolder != SpecialFolder.Computer
                ? ShellObjectInfoFactory.GetBrowsableObjectInfo(ShellObject.Parent, KnownFolderHelper.FromParsingName(ShellObject.Parent.ParsingName).Path)
                : null;

        }

        /// <summary>
        /// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        /// </summary>
        /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will report progress.</param>
        /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

                    LoadItems(new FolderLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

                else if (FileType == FileType.Archive)

                    LoadItems(new ArchiveLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            }

            else

                LoadItems(new FolderLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

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
        /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will report progress.</param>
        /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

                    LoadItemsAsync(new FolderLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

                else if (FileType == FileType.Archive)

                    LoadItemsAsync(new ArchiveLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

            }

            else

                LoadItemsAsync(new FolderLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

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
        public override void Dispose()
        {

            base.Dispose();

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
        /// Gets or sets the factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent casual file system items.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/></exception>
        public IShellObjectInfoFactory ShellObjectInfoFactory
        {
            get => _shellObjectInfoFactory; set
            {

                if (ItemsLoader.IsBusy)

                    throw new InvalidOperationException($"The {nameof(ItemsLoader)} is running.");

                _shellObjectInfoFactory = value;

            }
        }

        /// <summary>
        /// Gets or sets the factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/></exception>
        public IArchiveItemInfoFactory ArchiveItemInfoFactory
        {
            get => _archiveItemInfoFactory; set
            {

                if (ItemsLoader.IsBusy)

                    throw new InvalidOperationException($"The {nameof(ItemsLoader)} is running.");

                _archiveItemInfoFactory = value;

            }
        }

        /// <summary>
        /// Renames or move to a relative path, or both, the current <see cref="ShellObjectInfo"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo"/>.</param>
        public override void Rename(string newValue)

        {

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.NotEqual, out string key, FileType, GetKeyValuePair(nameof(FileType.Folder), FileType.Folder), GetKeyValuePair(nameof(FileType.File), FileType.File), GetKeyValuePair(nameof(FileType.Drive), FileType.Drive)))

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

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the same item that the current <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>An <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public override IBrowsableObjectInfo Clone() => ShellObjectInfoFactory.GetBrowsableObjectInfo(ShellObject.FromParsingName(ShellObject.ParsingName), Path, FileType, SpecialFolder);

    }

    /// <summary>
    /// A factory to create new <see cref="IBrowsableObjectInfo"/>'s.
    /// </summary>
    public interface IShellObjectInfoFactory
    {

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path);

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder);

    }

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>'s.
    /// </summary>
    public class ShellObjectInfoFactory : IShellObjectInfoFactory
    {

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path) => new ShellObjectInfo(shellObject, path);

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) => new ShellObjectInfo(shellObject, path, fileType, specialFolder);

    }

}
