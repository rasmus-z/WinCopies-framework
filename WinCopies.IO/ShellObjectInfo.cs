using Microsoft.WindowsAPICodePack.Shell;

using SevenZip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    /// <summary>
    /// Provides info to interact with file system items.
    /// </summary>
    public class ShellObjectInfo : BrowsableObjectInfo, IArchiveItemInfoProvider
    {

        /// <summary>
        /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public ShellObject ShellObject { get; private set; } = null;

        ShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => this;

        /// <summary>
        /// Gets the localized name of this <see cref="ShellObjectInfo"/> depending the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details..
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
        public override bool IsBrowsable => (ShellObject is IEnumerable<ShellObject> || FileType == FileType.Archive) && (FileType != FileType.File && FileType != FileType.Link); // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

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
        /// The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="SpecialFolders.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.
        /// </summary>
        public SpecialFolders SpecialFolder { get; private set; } = SpecialFolders.OtherFolderOrFile;

        public static (FileType fileType, SpecialFolders specialFolder) GetFileType(string path, ShellObject shellObject)

        {

            FileType fileType;

            if (// shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.FileSystemKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.IKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.NonFileSystemKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.ShellNonFileSystemFolder
                // !path.Contains(":\\")
                    !shellObject.IsFileSystemObject
                    )

                fileType = FileType.SpecialFolder;

            // todo: checking the special folder type and assign it to the specialFolder variable

            // todo : making a function for both fileType and specialFolder variables

            else if (shellObject is FileSystemKnownFolder && ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) && shellObject is ShellFile)

                fileType = FileType.Archive;

            else if (shellObject is ShellFile)

            {

                // var fileType = FileTypes.None;

                // if (shellObject.GetType() == typeof(ShellFolder))

                // fileType = FileTypes.Folder;

                // else

                if (path.EndsWith(".lnk"))

                    fileType = FileType.Link;

                // todo: add other 'in' archive supported formats

                else if (ArchiveLoader.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)))

                    fileType = FileType.Archive;

                else

                    fileType = FileType.File;

            }

            else

                fileType = System.IO.Path.GetPathRoot(path) == path ? FileType.Drive : FileType.Folder;

            return (fileType, GetSpecialFolderType(shellObject));

        }

        public static SpecialFolders GetSpecialFolderType(ShellObject shellObject)

        {

            SpecialFolders value;

            // todo: to add the other known folder types:

            if (shellObject.ParsingName == KnownFolders.DocumentsLibrary.ParsingName)

                value = SpecialFolders.DocumentsLibrary;

            else if (shellObject.ParsingName == KnownFolders.MusicLibrary.ParsingName)

                value = SpecialFolders.MusicLibrary;

            else if (shellObject.ParsingName == KnownFolders.PicturesLibrary.ParsingName)

                value = SpecialFolders.PicturesLibrary;

            else if (shellObject.ParsingName == KnownFolders.CameraRollLibrary.ParsingName)

                value = SpecialFolders.CameraRollLibrary;

            else if (shellObject.ParsingName == KnownFolders.SavedPicturesLibrary.ParsingName)

                value = SpecialFolders.SavedPicturesLibrary;

            else if (shellObject.ParsingName == KnownFolders.RecordedTVLibrary.ParsingName)

                value = SpecialFolders.RecordedTVLibrary;

            else if (shellObject.ParsingName == KnownFolders.VideosLibrary.ParsingName)

                value = SpecialFolders.VideosLibrary;

            else if (shellObject.ParsingName == KnownFolders.UsersLibraries.ParsingName)

                value = SpecialFolders.UsersLibraries;

            else if (shellObject.ParsingName == KnownFolders.Libraries.ParsingName)

                value = SpecialFolders.Libraries;

            else if (shellObject.ParsingName == KnownFolders.Computer.ParsingName)

                value = SpecialFolders.Computer;

            else

                value = SpecialFolders.OtherFolderOrFile;

            return value;

        }

        public ShellObjectInfo(ShellObject shellObject, string path) : base(path, GetFileType(path, shellObject).fileType) =>

            //#if DEBUG

            //            Debug.WriteLine("ShellObjectInfo(ShellObject shellObject, string path): shellObject == null: " + (shellObject == null).ToString());

            //#endif

            // void checkFileType()

            // {

            // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;

            // PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

            // if (specialFolder == SpecialFolders.OtherFolderOrFile)

            // {

            // }

            // }

            // checkFileType();

            // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;

            // PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

            // todo:

            // }

            Init(shellObject, nameof(FileType), GetFileType(path, shellObject).specialFolder);

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolders.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public ShellObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolders specialFolder) : base(path, fileType) =>

            //#if DEBUG

            //            Debug.WriteLine("ShellObjectInfo(ShellObject shellObject, string path, FileTypes fileType, WinCopies.IO.SpecialFolders specialFolder): shellObject == null: " + (shellObject == null).ToString());

            //#endif

            Init(shellObject, nameof(fileType), specialFolder);// string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;// PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        private void Init(ShellObject shellObject, string fileTypeParameterName, SpecialFolders specialFolder)

        {

            if (shellObject.ParsingName != Path)

                Debug.WriteLine("");

            if ((FileType == FileType.SpecialFolder && specialFolder == SpecialFolders.OtherFolderOrFile) || (FileType != FileType.SpecialFolder && specialFolder != SpecialFolders.OtherFolderOrFile))

                throw new ArgumentException(string.Format(Generic.FileTypeAndSpecialFolderNotCorrespond, fileTypeParameterName, nameof(specialFolder), FileType, specialFolder));

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

        public override IBrowsableObjectInfo GetParent()
        {

            IBrowsableObjectInfo parent;

            if (FileType == FileType.Folder || FileType == FileType.Archive || (FileType == FileType.SpecialFolder && ShellObject.IsFileSystemObject))

            {

                DirectoryInfo parentDirectoryInfo = FileType == FileType.Archive ? new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) : Directory.GetParent(Path);

                if (parentDirectoryInfo != null)

                {

                    string _parent = parentDirectoryInfo.FullName;

                    parent = GetBrowsableObjectInfo(ShellObject.FromParsingName(_parent), _parent);

                }

                else parent = null;

            }

            else parent = FileType == FileType.Drive
                ? GetBrowsableObjectInfo(ShellObject.Parent, KnownFolders.Computer.Path, FileType.SpecialFolder, SpecialFolders.Computer)
                : FileType == FileType.SpecialFolder && SpecialFolder != SpecialFolders.Computer
                ? GetBrowsableObjectInfo(ShellObject.Parent, KnownFolderHelper.FromParsingName(ShellObject.Parent.ParsingName).Path)
                : null;

            return parent;

        }

        /// <summary>
        /// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        /// </summary>
        public override void LoadItems()

        {

            if (ItemsLoader == null)

                LoadItems(true, true, FileTypesFlags.All);

            else

                ItemsLoader.LoadItems();

        }

        /// <summary>
        /// Loads the items of this <see cref="ShellObjectInfo"/> asynchronously.
        /// </summary>
        /// <param name="workerReportsProgress">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will report progress.</param>
        /// <param name="workerSupportsCancellation">A value that indicates whether the <see cref="BrowsableObjectInfoItemsLoader"/> will supports cancellation.</param>
        /// <param name="fileTypes">A value that indicates which have to be loaded.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.SpecialFolder)

                    LoadItems(new FolderLoader(true, true, fileTypes));

                else if (FileType == FileType.Archive)

                    LoadItems(new ArchiveLoader(true, true, fileTypes));

            }

            else

                LoadItems(new FolderLoader(true, true, fileTypes));

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
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given <see cref="BrowsableObjectInfoItemsLoader"/>.
        /// </summary>
        /// <param name="browsableObjectInfoItemsLoader">A custom loader to use to load the items of this <see cref="ShellObjectInfo"/>.</param>
        public override void LoadItems(BrowsableObjectInfoItemsLoader browsableObjectInfoItemsLoader)

        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            ItemsLoader = browsableObjectInfoItemsLoader ?? throw new ArgumentNullException(nameof(browsableObjectInfoItemsLoader));

            ItemsLoader.LoadItems();

        }

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo"/> in memory.
        // /// </summary>
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

        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path) => new ShellObjectInfo(shellObject, path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolders specialFolder) => new ShellObjectInfo(shellObject, path, fileType, specialFolder);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

        /// <summary>
        /// Renames or move to a relative path, or both, the current <see cref="ShellObjectInfo"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo"/>.</param>
        public override void Rename(string newValue)

        {

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.DoesNotEqual, out string key, FileType, GetKeyValuePair(nameof(FileType.Folder), FileType.Folder), GetKeyValuePair(nameof(FileType.File), FileType.File), GetKeyValuePair(nameof(FileType.Drive), FileType.Drive)))

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

    }

}
