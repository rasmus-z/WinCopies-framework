using System;

namespace WinCopies.IO
{
    /// <summary>
    /// The file type of an I/O object.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Undefined file type.
        /// </summary>
        Other,

        /// <summary>
        /// The item is a folder.
        /// </summary>
        Folder,

        /// <summary>
        /// The item is a file.
        /// </summary>
        File,

        /// <summary>
        /// The item is a drive.
        /// </summary>
        Drive,

        /// <summary>
        /// The item is a link.
        /// </summary>
        Link,

        /// <summary>
        /// The item is an archive.
        /// </summary>
        Archive,

        /// <summary>
        /// The item is a special folder (system folder or virtual folder).
        /// </summary>
        SpecialFolder
    }

    /// <summary>
    /// File types to load in the <see cref="FolderLoader"/> and <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> classes.
    /// </summary>
    [Flags]
    public enum FileTypes
    {
        /// <summary>
        /// Do not load any item type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Load folders.
        /// </summary>
        Folder = 1,

        /// <summary>
        /// Load files.
        /// </summary>
        File = 2,

        /// <summary>
        /// Load drives.
        /// </summary>
        Drive = 4,

        /// <summary>
        /// Load links.
        /// </summary>
        Link = 8,

        /// <summary>
        /// Load archives.
        /// </summary>
        Archive = 16
    }
}
