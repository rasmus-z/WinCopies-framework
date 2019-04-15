using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WinCopies.Util;
using static WinCopies.Util.Util;
using static WinCopies.Util.Generic;

namespace WinCopies.IO
{
    // todo: check if registry keys are the authorized ones.

    /// <summary>
    /// Provides some static methods to interact with the Windows registry.
    /// </summary>
    public static class Registry
    {

        /// <summary>
        /// Gets the Windows registry open-with file type for a given extension.
        /// </summary>
        /// <param name="extension">The file extension from which look for the associated file type.</param>
        /// <returns>If found, the Windows registry open-with file type associated to the given extension, otherwise null.</returns>
        public static string GetFileTypeByExtension(string extension)

        {

            if (extension == null)

                throw new ArgumentNullException(nameof(extension));

            if (string.IsNullOrEmpty(extension) || string.IsNullOrWhiteSpace(extension))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(extension)));

            RegistryKey value = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes\\" + extension) ?? Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension);

            string valueName = "";

            if (value == null)

                value = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension) ?? Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\" + extension);

            else

            {

                value = value.OpenSubKey("UserChoice");

                if (value == null)

                    value = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension) ?? Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\" + extension);

                else

                    valueName = "ProgId";

            }



            return value != null && value.GetValue(valueName) != null && value.GetValue(valueName) is string valueAsString ? valueAsString
                : null;
        }

        /// <summary>
        /// Gets the Windows registry command associated to a given extension.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="extension">The extension from which look for the associated command.</param>
        /// <returns>The Windows registry command associated to the given extension.</returns>
        public static string GetCommandByExtension(string commandName, string extension)

        {

            // todo: public method to avoid argument test for each call of the GetCommandByExtension and GetFileTypeByExtension methods.

            string key = null;

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, /*EqualityComparer<object>.Default, GetCommonPredicate<object>(),*/ null, GetKeyValuePair(nameof(commandName), (object)commandName), GetKeyValuePair(nameof(extension), (object)extension)))

                throw new ArgumentNullException(key);

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, true, GetKeyValuePair(nameof(commandName), IsNullEmptyOrWhiteSpace(commandName)), GetKeyValuePair(nameof(extension), IsNullEmptyOrWhiteSpace(extension))))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, key));

            string fileType = GetFileTypeByExtension(extension);

            return fileType == null
                ? (string)null
                : GetFileTypeRegistryKey(fileType).OpenSubKey("shell\\" + commandName + "\\command")?.GetValue("") is string valueAsString ? valueAsString : null;
        }

        public static string GetCommandByFileType(string commandName, string fileType)

        {

            string key = null;

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, null, GetKeyValuePair(nameof(commandName), commandName), GetKeyValuePair(nameof(commandName), fileType)))

                throw new ArgumentNullException(key);

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, true, GetKeyValuePair(nameof(commandName), IsNullEmptyOrWhiteSpace(commandName)), GetKeyValuePair(nameof(fileType), IsNullEmptyOrWhiteSpace(fileType))))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, key));

            RegistryKey fileTypeRegistryKey = GetFileTypeRegistryKey(fileType)?.OpenSubKey("shell\\" + commandName + "\\command");

            return fileTypeRegistryKey?.GetValue("") is string valueAsString ? valueAsString : null;

        }

        public static string GetOpenWithSoftwarePathFromCommand(string command)

        {

            if (command == null)

                throw new ArgumentNullException(nameof(command));

            if (IsNullEmptyOrWhiteSpace(command))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(command)));

            return command.Count(c => c == '"') > 1 && command.StartsWith("\"") ? command.Substring(1, command.Substring(1).IndexOf('"')) : command.Contains(' ') ? command.Substring(0, command.IndexOf(' ')) : command;

        }

        public static string GetOpenWithSoftwareArgumentsFromCommand(string command)

        {

            if (command == null)

                throw new ArgumentNullException(nameof(command));

            if (IsNullEmptyOrWhiteSpace(command))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(command)));

            if (command.Count(c => c == '"') > 2 && command.StartsWith("\""))

            {

                int value = command.Substring(1).IndexOf('"') + 1;

                if (value < command.Length)

                {

                    string _command = command.Substring(value);

                    return _command.StartsWith(" ") ? _command.Length > 1 ? _command.Substring(1) : "" : _command;

                }

                else

                    return "";

            }

            else if (command.Contains(' '))

            {

                int value = command.IndexOf(' ') + 1;

                return value < command.Length ? command.Substring(value) : "";

            }

            else

                return "";

        }

        public static ProcessStartInfo GetOpenWithSoftwareProcessStartInfoFromCommand(string command, string fileName)

        {

            string key = null;

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, null, GetKeyValuePair(nameof(command), command), GetKeyValuePair(nameof(fileName), fileName)))

                throw new ArgumentNullException(key);

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, true, GetKeyValuePair(nameof(command), IsNullEmptyOrWhiteSpace(command)), GetKeyValuePair(nameof(fileName), IsNullEmptyOrWhiteSpace(fileName))))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, key));

            string softwareFileName = command.Count(c => c == '"') > 2 && command.StartsWith("\"") ? command.Substring(1, command.Substring(1).IndexOf('"')) : command.Contains(' ') ? command.Substring(0, command.IndexOf(' ')) : command;

            string commandLineArguments = null;

            if (command.Length > softwareFileName.Length)

            {

                commandLineArguments = command.Substring(softwareFileName.Length);

                if (commandLineArguments.StartsWith(" "))

                    commandLineArguments = commandLineArguments.Substring(1);

                commandLineArguments = commandLineArguments.Replace("%1", fileName);

            }

            else

                commandLineArguments = "";

            return new ProcessStartInfo(softwareFileName, commandLineArguments);

        }

        // todo: add gesture for the 'HKEY_LOCAL_MACHINE\SOFTWARE\Clients' key (ex.: HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Media\Windows Media Player\Capabilities\FileAssociations, HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Media\Windows Media Player\Capabilities\MimeAssociations, HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Media\Windows Media Player\Capabilities\URLAssociations, ...).

        public static List<DesktopAppInfo> GetAppInfoByExtension(string extension)

        {

            if (extension == null)

                throw new ArgumentNullException(nameof(extension));

            if (string.IsNullOrEmpty(extension) || string.IsNullOrWhiteSpace(extension))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(extension)));

            RegistryKey[] subKeys = { Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes\\"), Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\"), Microsoft.Win32.Registry.ClassesRoot, Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\") };

            RegistryKey _registryKey ;

            List<DesktopAppInfo> fileTypes = new List<DesktopAppInfo>();

            void checkAndAddFileTypeIfSucceeded(string fileType, RegistryKey registryKey)

            {

                if (GetFileTypeRegistryKey(fileType, registryKey)?.OpenSubKey("shell\\open\\command")?.GetValue("") is string)

                {

                    DesktopAppInfo desktopAppInfo = new DesktopAppInfo(fileType);

                    foreach (DesktopAppInfo _desktopAppInfo in fileTypes)

                        if (GetOpenWithSoftwarePathFromCommand(_desktopAppInfo.Command) == GetOpenWithSoftwarePathFromCommand(desktopAppInfo.Command) && GetOpenWithSoftwareArgumentsFromCommand(_desktopAppInfo.Command) == GetOpenWithSoftwareArgumentsFromCommand(desktopAppInfo.Command)) return;

                    fileTypes.Add(desktopAppInfo);

                }

            }

            void onOpenWithList(RegistryKey mainRegistryKey, RegistryKey registryKey, string extensionOrPerceivedType)

            {

                registryKey = registryKey?.OpenSubKey("OpenWithList");

                if (registryKey == null) return;

                // object obj = registryKey.GetValueKind("MRUList");

                object valueAsObject = registryKey.GetValue("MRUList");

                string[] keyValueNames = registryKey.GetValueNames();

                if (!(valueAsObject == null || registryKey.GetValueKind("MRUList") != RegistryValueKind.String || IsNullEmptyOrWhiteSpace((string)valueAsObject)))

                {

                    string _value = null;

                    while (((string)valueAsObject).Length > 0)

                    {

                        _value = ((string)valueAsObject).Substring(0, 1);

                        valueAsObject = ((string)valueAsObject).Substring(1);

                        if (keyValueNames.Contains(_value) && registryKey.GetValue(_value) is string valueAsString)

                            checkAndAddFileTypeIfSucceeded(valueAsString, mainRegistryKey);

                    }

                }

                registryKey = mainRegistryKey.OpenSubKey(extensionOrPerceivedType + "\\OpenWithList");

                keyValueNames = registryKey?.GetSubKeyNames();

                foreach (string ___value in keyValueNames)

                    checkAndAddFileTypeIfSucceeded(___value, mainRegistryKey.Name.EndsWith("\\SystemFileAssociations") ? OpenRegistryKey(mainRegistryKey.Name.Substring(0, mainRegistryKey.Name.LastIndexOf("\\SystemFileAssociations"))) : mainRegistryKey);

            }

            foreach (RegistryKey registryKey in subKeys)

            {

                _registryKey = registryKey.OpenSubKey(extension);

                if (_registryKey == null) continue;

                #region Progids

                _registryKey = _registryKey.OpenSubKey("OpenWithProgids");

                if (_registryKey != null)

                {

                    foreach (string value in _registryKey.GetValueNames())

                    {

                        if (value == "") continue;

                        checkAndAddFileTypeIfSucceeded(value, registryKey);

                    }

                }

                #endregion

                #region OpenWith

                _registryKey = registryKey.OpenSubKey(extension);

                onOpenWithList(registryKey, _registryKey, extension);

                #endregion

                #region PerceivedType

                _registryKey = registryKey.OpenSubKey(extension);

                object valueAsObject = _registryKey.GetValue("PerceivedType");

                if (valueAsObject == null || _registryKey.GetValueKind("PerceivedType") != RegistryValueKind.String || (string)valueAsObject == "") continue;

                // checkAndAddFileTypeIfSucceeded((string)valueAsObject, registryKey, true);

                onOpenWithList(_registryKey.OpenSubKey("SystemFileAssociations\\") ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("SystemFileAssociations\\"), _registryKey.OpenSubKey("SystemFileAssociations\\" + (string)valueAsObject) ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("SystemFileAssociations\\" + (string)valueAsObject), (string)valueAsObject);

                #endregion

            }

            return fileTypes;

        }

        public static RegistryKey GetFileTypeRegistryKey(string fileType)

        {

            if (fileType == null)

                throw new ArgumentNullException(nameof(fileType));

            if (IsNullEmptyOrWhiteSpace(fileType))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(fileType)));

            return Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes\\" + fileType) ?? Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes\\Applications" + fileType) ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileType) ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Applications\\" + fileType) ?? Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Classes\\" + fileType) ?? Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Classes\\Applications\\" + fileType) ?? null;

        }

        public static RegistryKey GetFileTypeRegistryKey(string fileType, RegistryKey registryKey)

        {

            string key = null;

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out key, null, GetKeyValuePair(nameof(fileType), (object)fileType), GetKeyValuePair(nameof(registryKey), (object)registryKey)))

                throw new ArgumentNullException(key);

            if (IsNullEmptyOrWhiteSpace(fileType))

                throw new ArgumentException(string.Format(StringParameterEmptyOrWhiteSpaces, nameof(fileType)));

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, registryKey.Name, "HKEY_CURRENT_USER\\Software\\Classes", "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts", "HKEY_CLASSES_ROOT", "HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes"))

            {

                if (registryKey.Name == "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts")

                    registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Classes");

                return registryKey.Name.StartsWith("HKEY_CLASSES_ROOT")
                    ? registryKey.OpenSubKey(fileType) ?? registryKey.OpenSubKey("Applications\\" + fileType) ?? null
                    : registryKey.OpenSubKey(fileType) ?? registryKey.OpenSubKey("Applications\\" + fileType) ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileType) ?? Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Applications\\" + fileType) ?? null;
            }

            else throw new ArgumentException(string.Format(Generic.NotValidRegistryKey, registryKey));



        }

        public static RegistryKey OpenRegistryKey(string path)

        {

            RegistryKey registryKey ;

            int result ;

            string registryKeyName ;

            if (path.Contains("\\", out result))

            {

                registryKeyName = path.Substring(0, result);

                path = path.Length == 1 ? "" : path.Substring(result + 1);

            }

            else

            {

                registryKeyName = path;

                path = "";

            }

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equals, out registryKey, registryKeyName, GetKeyValuePair(Microsoft.Win32.Registry.ClassesRoot, Microsoft.Win32.Registry.ClassesRoot.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.CurrentConfig, Microsoft.Win32.Registry.CurrentConfig.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.CurrentUser, Microsoft.Win32.Registry.CurrentUser.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.DynData, Microsoft.Win32.Registry.DynData.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.LocalMachine, Microsoft.Win32.Registry.LocalMachine.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.PerformanceData, Microsoft.Win32.Registry.PerformanceData.Name),
                GetKeyValuePair(Microsoft.Win32.Registry.Users, Microsoft.Win32.Registry.Users.Name)))

                return path.Length > 0 ? registryKey.OpenSubKey(path) : registryKey;

            else throw new IOException(string.Format(Generic.RegistryKeyNotExists, path));

        }

    }
}
