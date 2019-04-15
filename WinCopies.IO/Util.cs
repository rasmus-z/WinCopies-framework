using Microsoft.WindowsAPICodePack.Shell;

using System;
using System.Diagnostics;
using System.Globalization;

using Shell = Microsoft.WindowsAPICodePack.Shell;

namespace WinCopies.IO
{
    /// <summary>
    /// The special folder types.
    /// </summary>
    public enum SpecialFolders
    {

        /// <summary>
        /// A casual file system item.
        /// </summary>
        OtherFolderOrFile = 0,

        UsersFiles,

        /// <summary>
        /// The <see cref="Shell.KnownFolders.Desktop"/> known folder.
        /// </summary>
        Desktop,

        PublicDesktop,

        Documents,

        PublicDocuments,

        Pictures,

        PublicPictures,

        Music,

        PublicMusic,

        Videos,

        PublicVideos,

        Downloads,

        PublicDownloads,

        /// <summary>
        /// The <see cref="Shell.KnownFolders.UsersLibraries"/> known folder.
        /// </summary>
        UsersLibraries,

        Libraries,

        /// <summary>
        /// The <see cref="Shell.KnownFolders.DocumentsLibrary"/> known folder.
        /// </summary>
        DocumentsLibrary,

        PicturesLibrary,

        CameraRollLibrary,

        SavedPicturesLibrary,

        MusicLibrary,

        VideosLibrary,

        RecordedTVLibrary, LocalAppData, LocalAppDataLow, QuickLaunch, SavedSearches, UserPinned, UserProfiles, OtherUsers,

        /// <summary>
        /// The item is the <see cref="Shell.KnownFolders.Computer"/> known folder.
        /// </summary>
        Computer,

        RecycleBin

    }

    /// <summary>
    /// Provides data about file system items.
    /// </summary>
    public interface IFileSystemObject

    {

        /// <summary>
        /// Gets the path of this <see cref="IFileSystemObject"/>.
        /// </summary>
        string Path { get; }

        string LocalizedPath { get; }

        string Name { get; }

        /// <summary>
        /// Gets the file type of this <see cref="IFileSystemObject"/>.
        /// </summary>
        FileType FileType { get; }

    }

    //public class KnownFolder : IShellObject

    //{

    //    public IKnownFolder Path { get; } = null;

    //    object IShellObject.Path => Path;

    //    public string ParsingName => Path.ParsingName;

    //    public KnownFolder(IKnownFolder path) => Path = path;

    //}

    //public class ShellObject : IShellObject

    //{

    //    public Shell.ShellObject Path { get; } = null;

    //    object IShellObject.Path => Path;

    //    public string ParsingName => Path.ParsingName;

    //    public ShellObject(Shell.ShellObject path) => Path = path;

    //}

    //public interface IShellObject

    //{

    //    object Path { get; }

    //    string ParsingName { get; }

    //}

    /// <summary>
    /// Provides static methods to interact with file system items.
    /// </summary>
    public static class Util
    {

        public const string LibrariesName = "Libraries";
        // todo: xml
        public const string LibrariesLocalizedName = "Bibliothèques";
    }
}
