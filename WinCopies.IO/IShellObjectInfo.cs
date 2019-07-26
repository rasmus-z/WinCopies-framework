using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.IO;

namespace WinCopies.IO
{
    public interface IShellObjectInfo : IBrowsableObjectInfo, IArchiveItemInfoProvider
    {

        /// <summary>
        /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="IShellObjectInfo"/>.
        /// </summary>
        ShellObject ShellObject { get; }

        /// <summary>
        /// Gets a <see cref="FileSystemInfo"/> object that provides info for the folders and files. This property returns <see langword="null"/> when this <see cref="IShellObjectInfo"/> is not a folder, drive or file. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        FileSystemInfo FileSystemInfoProperties { get; }

        /// <summary>
        /// Gets a <see cref="DriveInfo"/> object that provides info for drives. This property returns <see langword="null"/> when this <see cref="IShellObjectInfo"/> is not a drive. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        DriveInfo DriveInfoProperties { get; }

        /// <summary>
        /// Gets a <see cref="IKnownFolder"/> object that provides info for the system known folders. This property returns <see langword="null"/> when this <see cref="IShellObjectInfo"/> is not a system known folder. See the <see cref="BrowsableObjectInfo.FileType"/> property for more details.
        /// </summary>
        IKnownFolder KnownFolderInfo { get; }

        //private FileStream _archiveFileStream = null;

        ///// <summary>
        ///// The <see cref="FileStream"/> for this <see cref="IShellObjectInfo"/> when it represents an archive file system item. See the remarks section.
        ///// </summary>
        ///// <remarks>
        ///// This field is only used by the <see cref="IShellObjectInfo"/>, <see cref="FolderLoader"/> and the <see cref="ArchiveLoader"/> classes in order to lock the file that the <see cref="IShellObjectInfo"/> represents when the items of the archive are loaded.
        ///// </remarks>
        //public FileStream ArchiveFileStream { get => _archiveFileStream; internal set => OnPropertyChanged(nameof(ArchiveFileStream), nameof(_archiveFileStream), value, typeof(IShellObjectInfo)); }

        /// <summary>
        /// Gets the special folder type of this <see cref="IShellObjectInfo"/>. <see cref="SpecialFolder.OtherFolderOrFile"/> if this <see cref="IShellObjectInfo"/> is a casual file system item.
        /// </summary>
        SpecialFolder SpecialFolder { get; }

        /// <summary>
        /// Gets or sets the factory this <see cref="IShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent casual file system items.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy.</exception>
        /// <exception cref="ArgumentNullException">The given value is null.</exception>
        IShellObjectInfoFactory ShellObjectInfoFactory { get; set; }

    }
}
