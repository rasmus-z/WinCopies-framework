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
    public enum SpecialFolder
    {

        /// <summary>
        /// A casual file system item.
        /// </summary>
        OtherFolderOrFile = 0,

        Contacts,

        /// <summary>
        /// The <see cref="KnownFolders.Desktop"/> known folder.
        /// </summary>
        Desktop,

        /// <summary>
        /// The <see cref="KnownFolders.Documents"/> known folder.
        /// </summary>
        Documents,

        /// <summary>
        /// The <see cref="KnownFolders.Downloads"/> known folder.
        /// </summary>
        Downloads,

        Favorites,

        GameTasks,

        Links,

        /// <summary>
        /// The <see cref="KnownFolders.Music"/> known folder.
        /// </summary>
        Music,

        SampleMusic,

        Playlists,

        SamplePlaylists,

        Ringtones,

        /// <summary>
        /// The <see cref="KnownFolders.Pictures"/> known folder.
        /// </summary>
        Pictures,

        SamplePictures,

        SavedGames,

        SavedSearches,

        /// <summary>
        /// The <see cref="KnownFolders.Videos"/> known folder.
        /// </summary>
        Videos,

        SampleVideos,

        UsersLibraries,

        /// <summary>
        /// The <see cref="KnownFolders.Libraries"/> known folder.
        /// </summary>
        Libraries,

        /// <summary>
        /// The <see cref="KnownFolders.DocumentsLibrary"/> known folder.
        /// </summary>
        DocumentsLibrary,

        /// <summary>
        /// The <see cref="KnownFolders.PicturesLibrary"/> known folder.
        /// </summary>
        PicturesLibrary,

        /// <summary>
        /// The <see cref="KnownFolders.CameraRollLibrary"/> known folder.
        /// </summary>
        CameraRollLibrary,

        /// <summary>
        /// The <see cref="KnownFolders.SavedPicturesLibrary"/> known folder.
        /// </summary>
        SavedPicturesLibrary,

        /// <summary>
        /// The <see cref="KnownFolders.MusicLibrary"/> known folder.
        /// </summary>
        MusicLibrary,

        /// <summary>
        /// The <see cref="KnownFolders.VideosLibrary"/> known folder.
        /// </summary>
        VideosLibrary,

        RecordedTVLibrary,

        UsersFiles,

        Profile,

        LocalAppData,

        LocalAppDataLow,

        RoamingAppData,

        AdminTools,

        CDBurning,

        Cookies,

        History,

        ImplicitAppShortcuts,

        InternetCache,

        NetHood,

        PrintHood,

        Programs,

        QuickLaunch,

        Recent,

        SendTo,

        StartMenu,

        Startup,

        Templates,

        UserPinned,

        UserProgramFiles,

        UserProgramFilesCommon,

        /// <summary>
        /// The <see cref="KnownFolders.PublicDesktop"/> known folder.
        /// </summary>
        PublicDesktop,

        /// <summary>
        /// The <see cref="KnownFolders.PublicDocuments"/> known folder.
        /// </summary>
        PublicDocuments,

        /// <summary>
        /// The <see cref="KnownFolders.PublicDownloads"/> known folder.
        /// </summary>
        PublicDownloads,

        PublicGameTasks,

        /// <summary>
        /// The <see cref="KnownFolders.PublicMusic"/> known folder.
        /// </summary>
        PublicMusic,

        PublicRingtones,

        /// <summary>
        /// The <see cref="KnownFolders.PublicPictures"/> known folder.
        /// </summary>
        PublicPictures,

        /// <summary>
        /// The <see cref="KnownFolders.PublicVideos"/> known folder.
        /// </summary>
        PublicVideos,

        Public,

        UserProfiles,

        CommonStartMenu,

        Fonts,

        ProgramFiles,

        ProgramFilesCommon,

        ProgramFilesX86,

        ProgramFilesCommonX86,

        ProgramFilesX64,

        ProgramFilesCommonX64,

        ProgramData,

        CommonAdminTools,

        CommonPrograms,

        CommonStartup,

        CommonTemplates,

        DeviceMetadataStore,

        Windows,

        ResourceDir,

        SystemX86,

        System,

        Computer,

        Connections,

        ControlPanel,

        Internet,

        Network,

        OtherUsers,

        Printers,

        RecycleBin,

        SearchCsc,

        SearchHome,

        SearchMapi,

        AddNewPrograms,

        AppUpdates,

        ChangeRemovePrograms,

        CommonOemLinks,

        Conflict,

        Games,

        LocalizedResourcesDir,

        OriginalImages,

        PhotoAlbums,

        SidebarDefaultParts,

        SidebarParts,

        SyncManager,

        SyncResults,

        SyncSetup,

        TreeProperties

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

        string LocalizedName { get; }

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
}
