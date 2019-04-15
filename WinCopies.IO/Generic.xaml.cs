using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinCopies.IO
{
    public partial class Generic : ResourceDictionary
    {

        public static string GetStringResource(object key) => (string)ResourceDictionary[key];

        public static ResourceDictionary ResourceDictionary { get; } = WinCopies.Util.Generic.AddNewDictionary("/WinCopies.IO;component/Generic.xaml");

        public static string NotBrowsableObject => GetStringResource(nameof(NotBrowsableObject));

        public static string FileTypeAndSpecialFolderNotCorrespond => GetStringResource(nameof(FileTypeAndSpecialFolderNotCorrespond));

        public static string BackgroundWorkerIsBusy => GetStringResource(nameof(BackgroundWorkerIsBusy));

        public static string NotValidRegistryKey => GetStringResource(nameof(NotValidRegistryKey));

        public static string RegistryKeyNotExists => GetStringResource(nameof(RegistryKeyNotExists));

    }
}
