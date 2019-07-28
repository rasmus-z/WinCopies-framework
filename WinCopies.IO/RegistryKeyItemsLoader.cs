using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Win32;
using WinCopies.Util;
using System.Windows;
using System.Security;
using System.IO;

namespace WinCopies.IO
{

    [Flags]
    public enum RegistryItemTypes

    {

        None = 0,

        RegistryKey = 1,

        RegistryValue = 2

    }

    public class RegistryKeyItemsLoader : BrowsableObjectInfoItemsLoader, IRegistryKeyItemsLoader
    {

        private readonly RegistryItemTypes _registryItemTypes = RegistryItemTypes.None;

        public RegistryItemTypes RegistryItemTypes
        {

            get => _registryItemTypes;

            set
            {

                ThrowOnInvalidRegistryTypesOption();

                _ = this.SetBackgroundWorkerProperty(nameof(RegistryItemTypes), nameof(_registryItemTypes), value, typeof(RegistryKeyItemsLoader), true);

            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyItemsLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="registryItemTypes">The registry item types to load.</param>
        public RegistryKeyItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, RegistryItemTypes registryItemTypes) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), registryItemTypes) => RegistryItemTypes = registryItemTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyItemsLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="registryItemTypes">The registry item types to load.</param>
        public RegistryKeyItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> fileSystemObjectComparer, RegistryItemTypes registryItemTypes) : base(workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer) => _registryItemTypes = registryItemTypes;

        public override bool CheckFilter(string path)

        {

            if (Filter == null) return true;

            foreach (string filter in Filter)

            {

                bool checkFilters(string[] filters)

                {

                    foreach (string _filter in filters)

                    {

                        if (_filter == "") continue;

                        if (path.Length >= _filter.Length && path.Contains(_filter))

                            path = path.Substring(path.IndexOf(_filter) + _filter.Length);

                        else return false;

                    }

                    return true;

                }

                return checkFilters(filter.Split('*'));

            }

            return true;

        }

        protected override void OnPathChanging(BrowsableObjectInfo path) => ThrowOnInvalidRegistryTypesOption();

        private void ThrowOnInvalidRegistryTypesOption()

        {

            if (Path is RegistryItemInfo registryItemInfo)

                if (registryItemInfo.RegistryItemType == RegistryItemType.RegistryRoot && RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryValue))

                    throw new InvalidOperationException("The 'RegistryValue' option is not valid for the registry root path.");

        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            if (RegistryItemTypes == RegistryItemTypes.None)

                return;

            if (Path is RegistryItemInfo registryItemInfo)

            {

                var paths = new ArrayAndListBuilder<IFileSystemObject>();

                PathInfo pathInfo;

                void checkAndAppend(string pathWithoutName, string name, bool isValue)

                {

                    string path = pathWithoutName + '\\' + name;

                    if (CheckFilter(path))

                        _ = paths.AddLast(pathInfo = new PathInfo() { Path = path, Name = name, IsValue = isValue });

                }

                switch (registryItemInfo.RegistryItemType)

                {

                    case RegistryItemType.RegistryRoot:

                        if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

                        {

                            FieldInfo[] _registryKeyFields = typeof(Microsoft.Win32.Registry).GetFields();

                            string name;

                            foreach (FieldInfo fieldInfo in _registryKeyFields)

                            {

                                name = ((RegistryKey)fieldInfo.GetValue(null)).Name;

                                checkAndAppend(name, name, false);

                            }

                        }

                        break;

                    case RegistryItemType.RegistryKey:

                        string[] items;

                        if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

                            try

                            {

                                items = registryItemInfo.RegistryKey.GetSubKeyNames();

                                foreach (string item in items)

                                    checkAndAppend(item.Substring(0, item.LastIndexOf('\\')), item.Substring(item.LastIndexOf('\\') + 1), false);

                            }

                            catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

                        if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryValue))

                            try

                            {

                                items = registryItemInfo.RegistryKey.GetValueNames();

                                foreach (string item in items)

                                    checkAndAppend(registryItemInfo.RegistryKey.Name, item, true);

                            }

                            catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

                        break;

                }



                IEnumerable<PathInfo> pathInfos;



                if (FileSystemObjectComparer == null)

                    pathInfos = (IEnumerable<PathInfo>)paths;

                else

                {

                    var _paths = paths.ToList();

                    _paths.Sort(FileSystemObjectComparer);

                    pathInfos = (IEnumerable<PathInfo>)_paths;

                }



                using (IEnumerator<PathInfo> pathsEnum = pathInfos.GetEnumerator())



                    while (pathsEnum.MoveNext())

                        try

                        {

                            do

                                ReportProgress(0, pathsEnum.Current.IsValue ? registryItemInfo.Factory.GetBrowsableObjectInfo(pathsEnum.Current.Path) : registryItemInfo.Factory.GetBrowsableObjectInfo(pathsEnum.Current.Path.Substring(0, pathsEnum.Current.Path.Length - pathsEnum.Current.Name.Length - 1 /* We remove one more character to remove the backslash between the registry key path and the registry key value name. */ ), pathsEnum.Current.Name));

                            while (pathsEnum.MoveNext());

                        }

                        catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }



            }

        }

        public struct PathInfo : IFileSystemObject
        {
            public string Path { get; set; }

            public string LocalizedName => Name;

            public string Name { get; set; }

            public FileType FileType => FileType.SpecialFolder;

            public bool IsValue { get; set; }
        }
    }
}
