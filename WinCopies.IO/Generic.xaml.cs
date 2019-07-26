using System.Windows;

namespace WinCopies.IO
{
    public partial class Generic : ResourceDictionary
    {

        public static string GetStringResource(object key) => (string)ResourceDictionary[key];

        public static ResourceDictionary ResourceDictionary { get; } = WinCopies.Util.Generic.AddNewDictionary("/WinCopies.IO;component/Generic.xaml");

        /// <summary>
        /// Gets the NotBrowsableObject resource.
        /// </summary>
        public static string NotBrowsableObject => GetStringResource(nameof(NotBrowsableObject));

        /// <summary>
        /// Gets the FileTypeAndSpecialFolderNotCorrespond resource.
        /// </summary>
        public static string FileTypeAndSpecialFolderNotCorrespond => GetStringResource(nameof(FileTypeAndSpecialFolderNotCorrespond));

        /// <summary>
        /// Gets the BackgroundWorkerIsBusy resource.
        /// </summary>
        public static string BackgroundWorkerIsBusy => GetStringResource(nameof(BackgroundWorkerIsBusy));

        /// <summary>
        /// Gets the NotValidRegistryKey resource.
        /// </summary>
        public static string NotValidRegistryKey => GetStringResource(nameof(NotValidRegistryKey));

        /// <summary>
        /// Gets the RegistryKeyNotExists resource.
        /// </summary>
        public static string RegistryKeyNotExists => GetStringResource(nameof(RegistryKeyNotExists));

    }
}
