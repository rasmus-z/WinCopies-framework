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
        FileTypes FileType { get; }

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

    public static class Path

    {

        public static bool MatchToFilter(string path, string filter)

        {

            string pathWithoutExtension = System.IO.Path.GetDirectoryName(path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(path);

            string extension = System.IO.Path.GetExtension(path);

            bool checkFilters(string[] filters)

            {

                foreach (string _filter in filters)

                {

                    if (_filter == "") continue;

                    if (pathWithoutExtension.Length >= _filter.Length && pathWithoutExtension.Contains(_filter))

                        pathWithoutExtension = pathWithoutExtension.Substring(pathWithoutExtension.IndexOf(_filter) + _filter.Length);

                    else return false;

                }

                return true;

            }

            if (filter.Contains("."))

            {

                int i = filter.LastIndexOf(".");

                string[] filters1 = filter.Substring(0, i).Split('*');

                string[] filters2 = filter.Length > i + 1 ? filter.Substring(i + 1).Split('*') : null;

                return checkFilters(filters1) && checkFilters(filters2);

            }

            else

                return checkFilters(filter.Split('*'));

        }

        public static string GetNormalizedPath(string path)

        {

            var currentFile_Normalized = string.Empty;

            path = path.Normalize(System.Text.NormalizationForm.FormD);

            foreach (char ch in path)

                if (char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)

                    currentFile_Normalized += ch;

            return currentFile_Normalized.Normalize(System.Text.NormalizationForm.FormC);

        }

        public static ShellObjectInfo GetNormalizedOSPath(string basePath)

        {

            ShellObject shellObject = null;

            FileTypes fileType = FileTypes.None;

            SpecialFolders specialFolder = SpecialFolders.OtherFolderOrFile;

            string path = null;

            void setSpecialFolder(SpecialFolders specialFolder_)

            {

                specialFolder = specialFolder_;

                fileType = FileTypes.SpecialFolder;

            }

            // if (fileType != FileTypes.File && fileType != FileTypes.Folder) throw new ArgumentException("Invalid fileType parameter value. Accepted values are FileTypes.File or FileTypes.Folder.") ; 

#if DEBUG

            Debug.WriteLine("KnownFolders.Libraries.CanonicalName: " + KnownFolders.Downloads.CanonicalName);

            Debug.WriteLine("KnownFolders.Libraries.LocalizedName: " + KnownFolders.Downloads.LocalizedName);

            Debug.WriteLine("KnownFolders.Libraries.Path: " + KnownFolders.Downloads.Path);

            Debug.WriteLine("KnownFolders.Libraries.ParsingName: " + KnownFolders.Downloads.ParsingName);

#endif

            if (basePath.EndsWith("\\"))

                basePath = basePath.Substring(0, basePath.Length - 1);

#if DEBUG 

            Debug.WriteLine(basePath);

#endif 

            if (basePath.EndsWith(":"))

            {

                shellObject = ShellObject.FromParsingName(basePath);

                path = basePath;

                fileType = FileTypes.Drive;

            }

            else if (basePath.Contains(":"))

            {

                shellObject = ShellObject.FromParsingName(basePath);

                if (shellObject is IKnownFolder)

                {

                    string shellObjectDisplayName = shellObject.GetDisplayName(DisplayNameType.Default);

                    if (shellObjectDisplayName == KnownFolders.Libraries.LocalizedName)

                        setSpecialFolder(SpecialFolders.UsersLibraries);

                    else if (shellObjectDisplayName == KnownFolders.Desktop.LocalizedName)

                        setSpecialFolder(SpecialFolders.Desktop);

                    else

                        fileType = FileTypes.Folder;

                }

                else if (shellObject is ShellFile)

                    fileType = FileTypes.File;

                path = basePath;

            }

            else

            {

                if (basePath == Util.LibrariesName || basePath == Util.LibrariesLocalizedName)

                {

                    shellObject = (Shell.ShellObject)KnownFolders.Libraries;

                    setSpecialFolder(SpecialFolders.UsersLibraries);

                    path = KnownFolders.Libraries.Path;

                }

                else if (basePath == KnownFolders.Desktop.CanonicalName || basePath == KnownFolders.Desktop.LocalizedName)

                {

                    shellObject = (Shell.ShellObject)KnownFolders.Desktop;

                    setSpecialFolder(SpecialFolders.Desktop);

                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                }

                else

                    fileType = FileTypes.Folder;

                // else if (basePath.StartsWith(LibrariesName + "\\") || basePath.StartsWith(LibrariesLocalizedName)) { shellObject=(ShellObject)KnownFolders.Libraries KnownFolders.Libraries.Path + basePath.Substring(KnownFolders.Libraries.LocalizedName.Length);

                // else if

            }

#if DEBUG 

            Debug.WriteLine("Path: " + path);

#endif

            return new ShellObjectInfo(shellObject, path, fileType, specialFolder);

        }

        // todo: to check and translate all the methods below:

        public static SpecialFolders GetSpecialFolderFromPath(string path, ShellObject shellObject)

        {

            SpecialFolders specialFolder = SpecialFolders.OtherFolderOrFile;

            if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

#if DEBUG 

            Debug.WriteLine(path);

#endif 

            if (path.EndsWith(":"))

                specialFolder = SpecialFolders.OtherFolderOrFile;

            else if (path.Contains(":"))

                // todo: to add other values

                if (shellObject is IKnownFolder)

                {

                    string shellObjectParsingName = shellObject.ParsingName;

                    if (shellObjectParsingName == KnownFolders.Libraries.ParsingName) specialFolder = SpecialFolders.UsersLibraries;

                    else if (shellObjectParsingName == KnownFolders.Desktop.ParsingName) specialFolder = SpecialFolders.Desktop;

                }

                else

                if (path == KnownFolders.UsersLibraries.ParsingName || path == KnownFolders.UsersLibraries.LocalizedName)

                    specialFolder = SpecialFolders.UsersLibraries;

            // else if (basePath.StartsWith(LibrariesName + "\\") || basePath.StartsWith(LibrariesLocalizedName)) { shellObject=(ShellObject)KnownFolders.Libraries KnownFolders.Libraries.Path + basePath.Substring(KnownFolders.Libraries.LocalizedName.Length);

            // else if

            return specialFolder;

        }

        // TODO : attention : supprimer les répétitions ! - voir : file_Type as FileTypes? 

        public static string RenamePathWithAutomaticNumber(string path, string destPath)

        {

            string newPath = destPath + "\\" + System.IO.Path.GetFileName(path);

            if (!(System.IO.Directory.Exists(newPath) || System.IO.File.Exists(newPath)))

            {

                //if (System.IO.Directory.Exists(path))

                //    System.IO.Directory.Move(path, newPath);

                //else if (System.IO.File.Exists(path))

                //    System.IO.File.Move(path, newPath);

                return newPath;

            }



            long pathParenthesesNumber = -1;

            string getFileNameWithoutParentheses(string fileName, out long parenthesesNumber)

            {

                // We remove, if any, the last parentheses that are in the file name if they contain a number and if this number is lesser than long.MaxValue.

                if (fileName.Contains(" (") && fileName.EndsWith(")"))

                {

                    int index = fileName.LastIndexOf(" (");

                    string parenthesesContent = fileName.Substring(index + 2, fileName.Length - (index + 3));

                    if (parenthesesContent.Length > 0 && long.TryParse(parenthesesContent, out parenthesesNumber) && parenthesesNumber >= 0 && parenthesesNumber < long.MaxValue)

                        fileName = fileName.Substring(0, index);

                    else

                        parenthesesNumber = -1;

                }

                else

                    parenthesesNumber = -1;

                return fileName;

            }

            // Variables initialization

            // long number = 1;

            string _fileNameWithoutExtension = "";

            long _parenthesesNumber = -1;



            // We get all items that are in the same folder as the destPath parameter.

            string[] directories = System.IO.Directory.GetDirectories(destPath);

            string[] files = System.IO.Directory.GetFiles(destPath);



            // Then, we get the file name of the current path without its extension.

            string fileNameWithoutExtension = getFileNameWithoutParentheses(System.IO.Path.GetFileNameWithoutExtension(path), out pathParenthesesNumber);



            foreach (string directory in directories)

            {

                // de nouveau on reprend le nom de l'élément, ici, le dossier, sans son extension éventuelle:

                _fileNameWithoutExtension = getFileNameWithoutParentheses(System.IO.Path.GetFileNameWithoutExtension(directory), out _parenthesesNumber);



                // On fait ensuite une comparaison du nom de l'élément introduit par l'utilisateur avec le nom du dossier véirifé actuellement :

                if (_fileNameWithoutExtension.ToLower() == fileNameWithoutExtension.ToLower() && _parenthesesNumber > pathParenthesesNumber)

#if DEBUG

                {

                    Debug.WriteLine(pathParenthesesNumber.ToString() + " " + _parenthesesNumber.ToString());

#endif

                    pathParenthesesNumber = _parenthesesNumber;

#if DEBUG

                }

#endif

            }



            foreach (string file in files)

            {

                // de nouveau on reprend le nom de l'élément, ici, le dossier, sans son extension éventuelle:

                _fileNameWithoutExtension = getFileNameWithoutParentheses(System.IO.Path.GetFileNameWithoutExtension(file), out _parenthesesNumber);



                // On fait ensuite une comparaison du nom de l'élément introduit par l'utilisateur avec le nom du dossier véirifé actuellement :

                if (_fileNameWithoutExtension.ToLower() == fileNameWithoutExtension.ToLower() && _parenthesesNumber > pathParenthesesNumber)

#if DEBUG

                {

                    // if (long.TryParse(partOfName, out number_2))

                    // {

                    Debug.WriteLine(pathParenthesesNumber.ToString() + " " + _parenthesesNumber.ToString());

#endif

                    pathParenthesesNumber = _parenthesesNumber;

                    // }



#if DEBUG

                }

#endif

            }



            string new_Name = destPath + "\\" + fileNameWithoutExtension + " (" + (pathParenthesesNumber + 1).ToString() + ")" + System.IO.Path.GetExtension(path);



            // TODO : pertinent ? si oui, utiliser WinCopies.IO.FilesProcesses (avec un boolean pour voir s'il faut l'uitliser ou pas) ?



            if (System.IO.Directory.Exists(path))

                // System.IO.Directory.Move(path, new_Name);

                return new_Name;

            // DirectoryInfo.MoveTo(Rename_Window.NewFullName)

            else if (System.IO.File.Exists(path))

                // System.IO.File.Move(path, new_Name);

                return new_Name;

            return null;

            // FileInfo.MoveTo(

            // Case FileTypes.Drive, FileTypes.Folder, FileTypes.File



            // ProcessDialogResult = True

            // Close()

        }

    }
}
