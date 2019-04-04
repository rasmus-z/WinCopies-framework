using Microsoft.WindowsAPICodePack.Shell;

using SevenZip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    /// <summary>
    /// Provides info to interact with file system items.
    /// </summary>
    public class ShellObjectInfo : BrowsableObjectInfo, IArchiveItemInfoProvider
    {

        /// <summary>
        /// Gets a <see cref="ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public ShellObject ShellObject { get; private set; } = null;

        ShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => this;

        public override string LocalizedPath => ShellObject.GetDisplayName(DisplayNameType.Default);

        public override string Name => ShellObject.Name;

        public override bool IsBrowsable => (ShellObject is IEnumerable<ShellObject> || FileType == FileTypes.Archive) && (FileType != FileTypes.File && FileType != FileTypes.Link); // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="ShellObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public override IBrowsableObjectInfo Parent { get; protected set; } = null;

        /// <summary>
        /// Provides info for the folders and files.
        /// </summary>
        public FileSystemInfo FileSystemInfoProperties { get; private set; } = null;

        /// <summary>
        /// Provides info for drives.
        /// </summary>
        public DriveInfo DriveInfoProperties { get; private set; } = null;

        public IKnownFolder KnownFolderMetadata { get; private set; } = null;

        private FileStream _archiveFileStream = null;

        /// <summary>
        /// The <see cref="FileStream"/> for this <see cref="ShellObjectInfo"/> when it represents an archive file system item. See the remarks section.
        /// </summary>
        /// <remarks>
        /// This field is only used by the <see cref="ShellObjectInfo"/>, <see cref="LoadFolder"/> and the <see cref="LoadArchive"/> classes in order to lock the file that the <see cref="ShellObjectInfo"/> represents when the items of the archive are loaded.
        /// </remarks>
        public FileStream ArchiveFileStream
        {

            get => _archiveFileStream;

            internal set => OnPropertyChanged(nameof(ArchiveFileStream), nameof(_archiveFileStream), value, typeof(ShellObjectInfo));

        }

        /// <summary>
        /// The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="SpecialFolders.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.
        /// </summary>
        public SpecialFolders SpecialFolder { get; private set; } = SpecialFolders.OtherFolderOrFile;

        public static (FileTypes fileType, SpecialFolders specialFolder) GetFileType(string path, ShellObject shellObject)

        {

            FileTypes fileType;

            if (// shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.FileSystemKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.IKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.NonFileSystemKnownFolder
                // || shellObject.Parent is Microsoft.WindowsAPICodePack.Shell.ShellNonFileSystemFolder
                // !path.Contains(":\\")
                    !shellObject.IsFileSystemObject
                    )

                fileType = FileTypes.SpecialFolder;

            // todo: checking the special folder type and assign it to the specialFolder variable

            // todo : making a function for both fileType and specialFolder variables

            else if (shellObject is FileSystemKnownFolder && LoadArchive.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)) && shellObject is ShellFile)

                fileType = FileTypes.Archive;

            else if (shellObject is ShellFile)

            {

                // var fileType = FileTypes.None;

                // if (shellObject.GetType() == typeof(ShellFolder))

                // fileType = FileTypes.Folder;

                // else

                if (path.EndsWith(".lnk"))

                    fileType = FileTypes.Link;

                // todo: add other 'in' archive supported formats

                else if (LoadArchive.IsSupportedArchiveFormat(System.IO.Path.GetExtension(path)))

                    fileType = FileTypes.Archive;

                else

                    fileType = FileTypes.File;

            }

            else

                fileType = System.IO.Path.GetPathRoot(path) == path ? FileTypes.Drive : FileTypes.Folder;

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

            else

                value = SpecialFolders.OtherFolderOrFile;

            return value;

        }

        public ShellObjectInfo(ShellObject shellObject, string path) : base(path, GetFileType(path, shellObject).fileType)

        {

#if DEBUG

            Debug.WriteLine("ShellObjectInfo(ShellObject shellObject, string path) : shellObject == null: " + (shellObject == null).ToString());

#endif

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

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfo"/> class.
        /// </summary>
        /// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolders.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public ShellObjectInfo(ShellObject shellObject, string path, FileTypes fileType, SpecialFolders specialFolder) : base(path, fileType)

        {

#if DEBUG

            Debug.WriteLine("ShellObjectInfo(ShellObject shellObject, string path, FileTypes fileType, WinCopies.IO.SpecialFolders specialFolder) : shellObject == null: " + (shellObject == null).ToString());

#endif

            Init(shellObject, nameof(fileType), specialFolder);

            // string _path = ((Microsoft.WindowsAPICodePack.Shell.ShellFileSystemFolder)shellObject.Parent).ParsingName;

            // PathInfo pathInfo = new PathInfo() { Path = _path, Normalized_Path = null, Shell_Object = so };

        }

        private void Init(ShellObject shellObject, string fileTypeParameterName, SpecialFolders specialFolder)

        {

            if ((FileType == FileTypes.SpecialFolder && specialFolder == SpecialFolders.OtherFolderOrFile) || (FileType != FileTypes.SpecialFolder && specialFolder != SpecialFolders.OtherFolderOrFile))

                throw new ArgumentException(string.Format(Generic.FileTypeAndSpecialFolderNotCorrespond, fileTypeParameterName, nameof(specialFolder), FileType, specialFolder));

            ShellObject = shellObject;

            // LocalizedPath = shellObject.GetDisplayName(DisplayNameType.RelativeToDesktop);

            // NormalizedPath = Util.GetNormalizedPath(path);

            IBrowsableObjectInfo parent = null;

            if ((FileType == FileTypes.Folder || FileType == FileTypes.Archive || (FileType == FileTypes.SpecialFolder && shellObject.IsFileSystemObject)))

            {

                DirectoryInfo parentDirectoryInfo = FileType == FileTypes.Archive ? new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) : Directory.GetParent(Path);

                if (parentDirectoryInfo != null)

                {

                    string _parent = parentDirectoryInfo.FullName;

                    parent = GetBrowsableObjectInfo(ShellObject.FromParsingName(_parent), _parent);

                }

            }

            else if (FileType == FileTypes.Drive)

                parent = GetBrowsableObjectInfo(shellObject.Parent, KnownFolders.Computer.Path, FileTypes.SpecialFolder, SpecialFolders.Computer);

            else if (FileType == FileTypes.SpecialFolder && specialFolder != SpecialFolders.Computer)

                parent = GetBrowsableObjectInfo(shellObject.Parent, KnownFolderHelper.FromParsingName(shellObject.Parent.ParsingName).Path);

            Parent = parent;

            SpecialFolder = specialFolder;

            if (FileType == FileTypes.Folder || (FileType == FileTypes.SpecialFolder && shellObject.IsFileSystemObject))

                FileSystemInfoProperties = new DirectoryInfo(Path);

            else if (FileType == FileTypes.File || FileType == FileTypes.Archive || FileType == FileTypes.Link)

                FileSystemInfoProperties = new FileInfo(Path);

            else if (FileType == FileTypes.Drive)

            {

                FileSystemInfoProperties = new DirectoryInfo(Path);

                DriveInfoProperties = new DriveInfo(Path);

            }

            else if (FileType == FileTypes.SpecialFolder)

                KnownFolderMetadata = KnownFolderHelper.FromParsingName(shellObject.ParsingName);

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

        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes)
        {

            if (!IsBrowsable)

                throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject, FileType.ToString(), ToString()));

            if (ShellObject.IsFileSystemObject)

            {

                if (FileType == FileTypes.Folder || FileType == FileTypes.Drive || FileType == FileTypes.SpecialFolder)

                    LoadItems(new LoadFolder(true, true, fileTypes));

                else if (FileType == FileTypes.Archive)

                    LoadItems(new LoadArchive(true, true, fileTypes));

            }

            else

                LoadItems(new LoadFolder(true, true, fileTypes));

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
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using the given <see cref="BrowsableObjectInfoItemsLoader"/>.
        /// </summary>
        /// <param name="browsableObjectInfoItemsLoader">Custom loader to load the items of this <see cref="ShellObjectInfo"/>.</param>
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

            if (ArchiveFileStream != null)

            {

                ArchiveFileStream.Dispose();

                ArchiveFileStream.Close();

            }

        }

        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path) => new ShellObjectInfo(shellObject, path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileTypes fileType, SpecialFolders specialFolder) => new ShellObjectInfo(shellObject, path, fileType, specialFolder);

        public IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType);

        /// <summary>
        /// Renames or move to a relative path (or both) the current <see cref="ShellObjectInfo"/> with the specified name. See the doc of the <see cref="Directory.Move(string, string)"/>, <see cref="File.Move(string, string)"/> and <see cref="DriveInfo.VolumeLabel"/> for the possible exceptions.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo"/></param>.
        public override void Rename(string newValue)

        {

            string key;

            if (If(ComparisonType.Or, Comparison.DoesNotEqual, out key, FileType, GetKeyValuePair(nameof(FileTypes.Folder), FileTypes.Folder), GetKeyValuePair(nameof(FileTypes.File), FileTypes.File), GetKeyValuePair(nameof(FileTypes.Drive), FileTypes.Drive)))

                throw new InvalidOperationException($"{nameof(FileType)} must have one of the following values: {nameof(FileTypes.Folder)}, {nameof(FileTypes.File)} or {nameof(FileTypes.Drive)}. The value was {key}.");

            string getNewPath() => System.IO.Path.GetDirectoryName(Path) + "\\" + newValue;

            if (ShellObject.IsFileSystemObject)

                if (FileType == FileTypes.Folder)

                    Directory.Move(Path, getNewPath());

                else if (FileType == FileTypes.File)

                    File.Move(Path, getNewPath());

                else if (FileType == FileTypes.Drive)

                    DriveInfoProperties.VolumeLabel = newValue;

        }

    }

}
