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

    public class RegistryKeyItemsLoader : BrowsableObjectInfoItemsLoader
    {

        private RegistryItemTypes _registryItemTypes = RegistryItemTypes.None;

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

        protected override void InitializePath() => ThrowOnInvalidRegistryTypesOption();

        private void ThrowOnInvalidRegistryTypesOption()

        {

            if (Path is RegistryItemInfo registryItemInfo)

                if (registryItemInfo.RegistryItemType == RegistryItemType.RegistryRoot && RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryValue))

                    throw new InvalidOperationException("The 'RegistryValue' option is not valid for the registry root path.");

        }

        protected override void OnDoWork()
        {

            if (RegistryItemTypes == RegistryItemTypes.None)

                return;

            if (Path is RegistryItemInfo registryItemInfo)

            {

                switch (registryItemInfo.RegistryItemType)

                {

                    case RegistryItemType.RegistryRoot:

                        var _registryKeyFields = new List<FieldInfo>(typeof(Microsoft.Win32.Registry).GetFields());

                        _registryKeyFields.Sort((FieldInfo x, FieldInfo y) => x.Name.CompareTo(y.Name));

                        foreach (FieldInfo fieldInfo in _registryKeyFields)

                            if (CheckFilter(fieldInfo.Name))

                                ReportProgress(0, registryItemInfo.RegistryItemInfoFactory.GetBrowsableObjectInfo((RegistryKey)fieldInfo.GetValue(null)));

                        break;

                    case RegistryItemType.RegistryKey:

                        List<string> items;

                        if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

                            try

                            {

                                items = new List<string>(registryItemInfo.RegistryKey.GetSubKeyNames());

                                items.Sort();

                                foreach (string item in items)

                                    if (CheckFilter(item))

                                        ReportProgress(0, registryItemInfo.RegistryItemInfoFactory.GetBrowsableObjectInfo(registryItemInfo.Path + '\\' + item));

                            }

                            catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

                        if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryValue))

                            try

                            {

                                items = new List<string>(registryItemInfo.RegistryKey.GetValueNames());

                                items.Sort();

                                foreach (string item in items)

                                    if (CheckFilter(item))

                                        ReportProgress(0, registryItemInfo.RegistryItemInfoFactory.GetBrowsableObjectInfo(registryItemInfo.RegistryKey, item));

                            }

                            catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

                        break;

                }

            }

        }
    }
}
