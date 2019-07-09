using Microsoft.WindowsAPICodePack.Shell;
using SevenZip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public static class Path

    {
        public static readonly string[] EnvironmentPathVariables = { "AllUserProfile", "AppData", "CommonProgramFiles", "CommonProgramFiles(x86)", "HomeDrive", "LocalAppData", "ProgramData", "ProgramFiles", "ProgramFiles(x86)", "Public", "SystemDrive", "SystemRoot", "Temp", "UserProfile" };

        public static BrowsableObjectInfo GetBrowsableObjectInfoFromPath(string path)

        {

            path = path.Replace('/', '\\');

            if (path.EndsWith("\\") && !path.EndsWith(":\\") && !path.EndsWith(":\\\\"))

                path = path.Substring(0, path.LastIndexOf("\\"));

            path = GetRealPathFromEnvironmentVariables(path);

            string[] paths = path.Split('\\');

            BrowsableObjectInfo browsableObjectInfo;

            ShellObject shellObject = ShellObject.FromParsingName(paths[0]);

            browsableObjectInfo = Directory.Exists(paths[0])
                ? new ShellObjectInfo(shellObject, paths[0], FileType.Drive, SpecialFolder.OtherFolderOrFile)
                : File.Exists(paths[0])
                ? new ShellObjectInfo(shellObject, paths[0], FileType.File, SpecialFolder.OtherFolderOrFile)
                : new ShellObjectInfo(shellObject, paths[0], FileType.SpecialFolder, ShellObjectInfo.GetSpecialFolderType(shellObject));

            if (paths.Length == 1)

                return browsableObjectInfo;

            bool ok;

            ArchiveLoader ArchiveLoader = null;

            // int archiveSubpathsCount = 0;

            for (int i = 1; i < paths.Length; i++)

            {

                ok = false;

                if (browsableObjectInfo.FileType == FileType.Archive || browsableObjectInfo is ArchiveItemInfo)

                {

                    // archiveSubpathsCount++;

                    if (browsableObjectInfo.FileType == FileType.Archive)

                        ArchiveLoader = new ArchiveLoader(true, false, GetAllEnumFlags<FileTypes>());

                    ArchiveLoader.Path = browsableObjectInfo;

                    ArchiveLoader.LoadItems();

                    string s = paths[i].ToLower();

                    foreach (BrowsableObjectInfo _browsableObjectInfo in browsableObjectInfo.Items)

                        if (_browsableObjectInfo.Path.Substring(_browsableObjectInfo.Path.LastIndexOf('\\') + 1).ToLower() == s)

                        {

                            browsableObjectInfo = _browsableObjectInfo;

                            ok = true;

                            break;

                        }

                    if (ok)

                    {

                        if (browsableObjectInfo.FileType == FileType.Archive && ArchiveLoader != null)

                            throw new IOException("The 'Open archive in archive' feature is currently not supported by the WinCopies framework.");

                    }

                    else

                        throw new FileNotFoundException("The path could not be found.");

                }

                else

                {

                    shellObject = ((ShellObjectInfo)browsableObjectInfo).ShellObject;

                    string s = shellObject.ParsingName;

                    if (!s.EndsWith("\\"))

                        s += "\\";

                    s += paths[i];

                    string _s = s.Replace("\\", "\\\\");

                    if (shellObject.IsFileSystemObject)

                        if (Directory.Exists(_s) || File.Exists(_s)) // We also check the files because the path can be an archive.

                            browsableObjectInfo = new ShellObjectInfo(ShellObject.FromParsingName(s), s);

                        else

                        {

                            foreach (ShellObject _shellObject in (ShellContainer)shellObject)

                            {

                                Debug.WriteLine(_shellObject.GetDisplayName(DisplayNameType.RelativeToParent));

                                if (If(ComparisonType.Or, ComparisonMode.Logical, WinCopies.Util.Util.Comparison.Equal, paths[i], _shellObject.Name, _shellObject.GetDisplayName(DisplayNameType.RelativeToParent)))

                                {

                                    shellObject = _shellObject;

                                    ok = true;

                                    break;

                                }

                            }

                            if (ok)

                                browsableObjectInfo = new ShellObjectInfo(shellObject, s);

                            else

                                throw new FileNotFoundException("The path could not be found.");

                        }

                }

                if (!browsableObjectInfo.IsBrowsable && i < paths.Length - 1)

                    throw new DirectoryNotFoundException("The path isn't a directory.");

            }

            return browsableObjectInfo;

        }

        public static bool MatchToFilter(string path, string filter)

        {

            string pathWithoutExtension = System.IO.Path.GetDirectoryName(path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(path);

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

        //        public static ShellObjectInfo GetNormalizedOSPath(string basePath)

        //        {

        //            ShellObject shellObject = null;

        //            FileType fileType = FileType.None;

        //            SpecialFolders specialFolder = SpecialFolders.OtherFolderOrFile;

        //            string path = null;

        //            void setSpecialFolder(SpecialFolders specialFolder_)

        //            {

        //                specialFolder = specialFolder_;

        //                fileType = FileType.SpecialFolder;

        //            }

        //            // if (fileType != FileTypes.File && fileType != FileTypes.Folder) throw new ArgumentException("Invalid fileType parameter value. Accepted values are FileTypes.File or FileTypes.Folder.") ; 

        //#if DEBUG

        //            Debug.WriteLine("KnownFolders.Libraries.CanonicalName: " + KnownFolders.Downloads.CanonicalName);

        //            Debug.WriteLine("KnownFolders.Libraries.LocalizedName: " + KnownFolders.Downloads.LocalizedName);

        //            Debug.WriteLine("KnownFolders.Libraries.Path: " + KnownFolders.Downloads.Path);

        //            Debug.WriteLine("KnownFolders.Libraries.ParsingName: " + KnownFolders.Downloads.ParsingName);

        //#endif

        //            if (basePath.EndsWith("\\"))

        //                basePath = basePath.Substring(0, basePath.Length - 1);

        //#if DEBUG

        //            Debug.WriteLine(basePath);

        //#endif

        //            if (basePath.EndsWith(":"))

        //            {

        //                shellObject = ShellObject.FromParsingName(basePath);

        //                path = basePath;

        //                fileType = FileType.Drive;

        //            }

        //            else if (basePath.Contains(":"))

        //            {

        //                shellObject = ShellObject.FromParsingName(basePath);

        //                if (shellObject is IKnownFolder)

        //                {

        //                    string shellObjectDisplayName = shellObject.GetDisplayName(DisplayNameType.Default);

        //                    if (shellObjectDisplayName == KnownFolders.Libraries.LocalizedName)

        //                        setSpecialFolder(SpecialFolders.UsersLibraries);

        //                    else if (shellObjectDisplayName == KnownFolders.Desktop.LocalizedName)

        //                        setSpecialFolder(SpecialFolders.Desktop);

        //                    else

        //                        fileType = FileType.Folder;

        //                }

        //                else if (shellObject is ShellFile)

        //                    fileType = FileType.File;

        //                path = basePath;

        //            }

        //            else

        //            {

        //                if (basePath == Util.LibrariesName || basePath == Util.LibrariesLocalizedName)

        //                {

        //                    shellObject = (ShellObject)KnownFolders.Libraries;

        //                    setSpecialFolder(SpecialFolders.UsersLibraries);

        //                    path = KnownFolders.Libraries.Path;

        //                }

        //                else if (basePath == KnownFolders.Desktop.CanonicalName || basePath == KnownFolders.Desktop.LocalizedName)

        //                {

        //                    shellObject = (ShellObject)KnownFolders.Desktop;

        //                    setSpecialFolder(SpecialFolders.Desktop);

        //                    path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        //                }

        //                else

        //                    fileType = FileType.Folder;

        //                // else if (basePath.StartsWith(LibrariesName + "\\") || basePath.StartsWith(LibrariesLocalizedName)) { shellObject=(ShellObject)KnownFolders.Libraries KnownFolders.Libraries.Path + basePath.Substring(KnownFolders.Libraries.LocalizedName.Length);

        //                // else if

        //            }

        //#if DEBUG

        //            Debug.WriteLine("Path: " + path);

        //#endif

        //            return new ShellObjectInfo(shellObject, path, fileType, specialFolder);

        //        }

        // todo: to check and translate all the methods below:

        public static SpecialFolder GetSpecialFolderFromPath(string path, ShellObject shellObject)

        {

            SpecialFolder specialFolder = SpecialFolder.OtherFolderOrFile;

            if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

#if DEBUG

            Debug.WriteLine(path);

#endif

            if (path.EndsWith(":"))

                specialFolder = SpecialFolder.OtherFolderOrFile;

            else if (path.Contains(":"))

                // todo: to add other values

                if (shellObject is IKnownFolder)

                {

                    string shellObjectParsingName = shellObject.ParsingName;

                    if (shellObjectParsingName == KnownFolders.Libraries.ParsingName) specialFolder = SpecialFolder.UsersLibraries;

                    else if (shellObjectParsingName == KnownFolders.Desktop.ParsingName) specialFolder = SpecialFolder.Desktop;

                }

                else

                if (path == KnownFolders.UsersLibraries.ParsingName || path == KnownFolders.UsersLibraries.LocalizedName)

                    specialFolder = SpecialFolder.UsersLibraries;

            // else if (basePath.StartsWith(LibrariesName + "\\") || basePath.StartsWith(LibrariesLocalizedName)) { shellObject=(ShellObject)KnownFolders.Libraries KnownFolders.Libraries.Path + basePath.Substring(KnownFolders.Libraries.LocalizedName.Length);

            // else if

            return specialFolder;

        }

        // TODO : attention : supprimer les répétitions ! - voir : file_Type as FileTypes? 

        public static string RenamePathWithAutomaticNumber(string path, string destPath)

        {

            string newPath = destPath + "\\" + System.IO.Path.GetFileName(path);

            if (!(Directory.Exists(newPath) || File.Exists(newPath)))

                //if (System.IO.Directory.Exists(path))

                //    System.IO.Directory.Move(path, newPath);

                //else if (System.IO.File.Exists(path))

                //    System.IO.File.Move(path, newPath);

                return newPath;



            long pathParenthesesNumber = -1;

            string getFileNameWithoutParentheses(string fileName, out long parenthesesNumber)

            {

                // We remove, if any, the last parentheses that are in the file name if they contain a number and if this number is lesser than long.MaxValue.

                if (fileName.Contains(" (") && fileName.EndsWith(")"))

                {

                    int index = fileName.LastIndexOf(" (");

                    string parenthesesContent = fileName.Substring(index + 2, fileName.Length - (index + 3));

                    if (/*parenthesesContent.Length > 0 &&*/ long.TryParse(parenthesesContent, out parenthesesNumber) && parenthesesNumber >= 0)

                        return fileName.Substring(0, index);

                }

                parenthesesNumber = -1;

                return fileName;

            }

            // Variables initialization

            // long number = 1;

            string _fileNameWithoutExtension = "";

            long _parenthesesNumber = -1;



            // We get all items that are in the same folder as the destPath parameter.

            string[] directories = Directory.GetDirectories(destPath);

            string[] files = Directory.GetFiles(destPath);



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



            return destPath + "\\" + fileNameWithoutExtension + " (" + (pathParenthesesNumber + 1).ToString() + ")" + System.IO.Path.GetExtension(path);


            // string new_Name = destPath + "\\" + fileNameWithoutExtension + " (" + (pathParenthesesNumber + 1).ToString() + ")" + System.IO.Path.GetExtension(path);



            // TODO : pertinent ? si oui, utiliser WinCopies.IO.FilesProcesses (avec un boolean pour voir s'il faut l'uitliser ou pas) ?



            // if (Directory.Exists(path) || File.Exists(path))

            // System.IO.Directory.Move(path, new_Name);


            // DirectoryInfo.MoveTo(Rename_Window.NewFullName)

            // else if (File.Exists(path))

            // System.IO.File.Move(path, new_Name);

            // return new_Name;

            // return null;

            // FileInfo.MoveTo(

            // Case FileTypes.Drive, FileTypes.Folder, FileTypes.File



            // ProcessDialogResult = True

            // Close()

        }

        public static string GetRealPathFromEnvironmentVariables(string path)

        {

            string[] subPaths = path.Split('\\');

            StringBuilder stringBuilder = new StringBuilder();

            int count = 0;

            foreach (string subPath in subPaths)

            {

                count++;

                if (subPath.StartsWith("%"))

                    if (subPath.EndsWith("%"))

                        stringBuilder.Append(Environment.GetEnvironmentVariable(subPath.Substring(1, subPath.Length - 2)));

                    else

                        throw new ArgumentException("'path' is not a well-formatted environment variables path.");

                else

                    stringBuilder.Append(subPath);

                if (count < subPaths.Length)

                    stringBuilder.Append('\\');

            }

            return stringBuilder.ToString();

        }

        public static string GetShortcutPath(string path)

        {

            List<KeyValuePair<string, string>> paths = new List<KeyValuePair<string, string>>();

            foreach (string environmentPathVariable in EnvironmentPathVariables)

            {

                string _path = Environment.GetEnvironmentVariable(environmentPathVariable);

                if (_path != null)

                    paths.Add(new KeyValuePair<string, string>(environmentPathVariable, _path));

            }



            paths.Sort((KeyValuePair<string, string> x, KeyValuePair<string, string> y) => x.Value.Length < y.Value.Length ? 1 : x.Value.Length == y.Value.Length ? 0 : -1);



            foreach (KeyValuePair<string, string> _path in paths)

                if (path.StartsWith(_path.Value))

                {

                    path = "%" + _path.Key + "%" + path.Substring(_path.Value.Length);

                    break;

                }

            return path;

        }

    }
}
