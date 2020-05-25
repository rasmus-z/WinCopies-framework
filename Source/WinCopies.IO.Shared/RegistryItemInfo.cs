/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.If not, see<https://www.gnu.org/licenses/>. */

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.COMNative.Shell;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WinCopies.Linq;
using WinCopies.Util;
using static WinCopies.IO.RegistryItemInfo;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public struct RegistryItemInfoEnumeratorStruct
    {
        public string Value { get; }

        public RegistryItemType RegistryItemType { get; }

        public RegistryItemInfoEnumeratorStruct(string value, RegistryItemType registryItemType)
        {
            Value = value;

            RegistryItemType = registryItemType;
        }
    }

    /// <summary>
    /// The Windows registry item type.
    /// </summary>
    public enum RegistryItemType
    {

        /// <summary>
        /// The current instance represents the Windows registry root node.
        /// </summary>
        Root,

        /// <summary>
        /// The current instance represents a Windows registry key.
        /// </summary>
        Key,

        /// <summary>
        /// The current instance represents a Windows registry value.
        /// </summary>
        Value

    }

    /// <summary>
    /// Represents a Windows registry item.
    /// </summary>
    public class RegistryItemInfo/*<TItems, TFactory>*/ : BrowsableObjectInfo/*<TItems, TFactory>*/, IRegistryItemInfo // where TItems : BrowsableObjectInfo, IRegistryItemInfo where TFactory : IRegistryItemInfoFactory
    {

        // public override bool IsRenamingSupported => false;

        #region Fields

        private const int FileIcon = 0;
        private const int ComputerIcon = 15;
        private const int FolderIcon = 3;

        private RegistryKey _registryKey;

        #endregion

        #region Properties

        ///// <summary>
        ///// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        ///// </summary>
        //public override bool NeedsObjectsOrValuesReconstruction => _registryKey is object; // If _registryKey is null, reconstructing registry does not make sense, so we return false.

        //public static DeepClone<RegistryKey> DefaultRegistryKeyDeepClone { get; } = registryKey => Registry.OpenRegistryKey(registryKey.Name);

        public override bool IsSpecialItem => false;

        public override Size? Size => null;

        private string _itemTypeName;

        public override string ItemTypeName
        {

            get
            {

                if (string.IsNullOrEmpty(_itemTypeName))

                    switch (RegistryItemType)
                    {
                        case RegistryItemType.Root:
                            _itemTypeName = "Registry root";
                            break;
                        case RegistryItemType.Key:
                            _itemTypeName = "Registry key";
                            break;
                        case RegistryItemType.Value:
                            _itemTypeName = "Registry value";
                            break;
                    }

                return _itemTypeName;

            }

        }

        /// <summary>
        /// The Windows registry item type of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public RegistryItemType RegistryItemType { get; }

        /// <summary>
        /// The <see cref="Microsoft.Win32.RegistryKey"/> that this <see cref="RegistryItemInfo"/> represents.
        /// </summary>
        public RegistryKey RegistryKey

        {

            get

            {

                if (_registryKey == null && RegistryItemType == RegistryItemType.Key)

                    OpenKey();

                return _registryKey;

            }

        }

        /// <summary>
        /// Gets the localized path of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override string LocalizedName => Name;

        /// <summary>
        /// Gets the name of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="RegistryItemInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => RegistryItemType == RegistryItemType.Root || RegistryItemType == RegistryItemType.Key;

        #endregion

        private IBrowsableObjectInfo _parent;

#if NETCORE

        public override IBrowsableObjectInfo Parent => _parent ??= GetParent();

#else

        public override IBrowsableObjectInfo Parent => _parent ?? (_parent = GetParent());

#endif

        #region Constructors

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        ///// </summary>
        public RegistryItemInfo() : base(ShellObject.FromParsingName(KnownFolders.Computer.ParsingName).GetDisplayName(DisplayNameType.Default))
        {

            Name = Path;

            RegistryItemType = RegistryItemType.Root;

        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        ///// </summary>
        ///// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey) : base((registryKey ?? throw GetArgumentNullException(nameof(registryKey))).Name)
        {

            string[] name = registryKey.Name.Split(IO.Path.PathSeparator);

            Name =

#if NETFRAMEWORK

                name[name.Length-1];

#else

                name[^1];

#endif

            RegistryItemType = RegistryItemType.Key;

            _registryKey = registryKey;

        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        ///// </summary>
        ///// <param name="path">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(string path) : base(path)
        {

            ThrowIfNullEmptyOrWhiteSpace(path);

            string[] name = path.Split(IO.Path.PathSeparator);

            Name =

#if NETFRAMEWORK
                
                name[name.Length-1];
                
#else

                name[^1];

#endif

            RegistryItemType = RegistryItemType.Key;

            _registryKey = Registry.OpenRegistryKey(path);

        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        ///// </summary>
        ///// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        ///// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey, string valueName) : base(registryKey.Name)
        {

            Name = valueName;

            RegistryItemType = RegistryItemType.Value;

            _registryKey = registryKey;

        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        ///// </summary>
        ///// <param name="registryKeyPath">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        ///// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(string registryKeyPath, string valueName) : this(Registry.OpenRegistryKey(registryKeyPath), valueName) { }

        #endregion

        #region Public methods

        ///// <summary>
        ///// Gets a default comparer for <see cref="FileSystemObject"/>s.
        ///// </summary>
        ///// <returns>A default comparer for <see cref="FileSystemObject"/>s.</returns>
        //public static RegistryItemInfoComparer<IRegistryItemInfo> GetDefaultRegistryItemInfoComparer() => new RegistryItemInfoComparer<IRegistryItemInfo>();

        ///// <summary>
        ///// Determines whether the specified object is equal to the current object by calling <see cref="FileSystemObject.Equals(object)"/> and then, if the result is <see langword="true"/>, by testing the following things, in order: <paramref name="obj"/> implements the <see cref="IRegistryItemInfo"/> interface and <see cref="RegistryItemType"/> are equal.
        ///// </summary>
        ///// <param name="obj">The object to compare with the current object.</param>
        ///// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        //public override bool Equals(object obj) => base.Equals(obj) && (obj is IRegistryItemInfo _obj ? RegistryItemType == _obj.RegistryItemType : false);

        ///// <summary>
        ///// Compares the current object to a given <see cref="FileSystemObject"/>.
        ///// </summary>
        ///// <param name="registryItemInfo">The <see cref="FileSystemObject"/> to compare with.</param>
        ///// <returns>The comparison result. See <see cref="IComparable{T}.CompareTo(T)"/> for more details.</returns>
        //public int CompareTo(IRegistryItemInfo registryItemInfo) => GetDefaultRegistryItemInfoComparer().Compare(this, registryItemInfo);

        ///// <summary>
        ///// Determines whether the specified <see cref="IRegistryItemInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
        ///// </summary>
        ///// <param name="registryItemInfo">The <see cref="IRegistryItemInfo"/> to compare with the current object.</param>
        ///// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        //public bool Equals(IRegistryItemInfo registryItemInfo) => Equals(registryItemInfo as object);

        ///// <summary>
        ///// Gets an hash code for this <see cref="RegistryItemInfo"/>.
        ///// </summary>
        ///// <returns>The hash code returned by the <see cref="FileSystemObject.GetHashCode"/> and the hash code of the <see cref="RegistryItemType"/>.</returns>
        //public override int GetHashCode() => base.GetHashCode() ^ RegistryItemType.GetHashCode();

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo"/> represents.
        /// </summary>
        public void OpenKey()
        {

            if (RegistryItemType != RegistryItemType.Key)

                throw new InvalidOperationException("This item does not represent a registry key.");

            _registryKey = Registry.OpenRegistryKey(Path);

        }

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo"/> represents using custom <see cref="RegistryKeyPermissionCheck"/> and <see cref="RegistryRights"/>.
        /// </summary>
        /// <param name="registryKeyPermissionCheck">Specifies whether security checks are performed when opening the registry key and accessing its name/value pairs.</param>
        /// <param name="registryRights">Specifies the access control rights that can be applied to the registry objects in registry key's scope.</param>
        public void OpenKey(RegistryKeyPermissionCheck registryKeyPermissionCheck, RegistryRights registryRights) => _registryKey = Registry.OpenRegistryKey(Path, registryKeyPermissionCheck, registryRights);

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo"/> represents with a <see cref="bool"/> value that indicates whether the registry key has to be opened with write-rights.
        /// </summary>
        /// <param name="writable">A <see cref="bool"/> value that indicates whether the registry key has to be opened with write-rights</param>
        public void OpenKey(bool writable) => _registryKey = Registry.OpenRegistryKey(Path, writable);

        #endregion

        #region Protected methods

        /// <summary>
        /// Returns the parent of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="RegistryItemInfo"/>.</returns>
        private IBrowsableObjectInfo GetParent()
        {

            switch (RegistryItemType)

            {

                case RegistryItemType.Key:

                    string[] path = RegistryKey.Name.Split(IO.Path.PathSeparator);

                    if (path.Length == 1)

                        return new RegistryItemInfo();

                    var stringBuilder = new StringBuilder();

                    for (int i = 0; i < path.Length - 1; i++)

                        _ = stringBuilder.Append(path);

                    return new RegistryItemInfo(stringBuilder.ToString());

                case RegistryItemType.Value:

                    return new RegistryItemInfo(RegistryKey);

                default:

                    return null;

            }

        }

        ///// <summary>
        ///// Disposes the current <see cref="RegistryItemInfo"/> and its parent and items recursively.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _registryKey?.Dispose();

            if (disposing)

            {

                _registryKey = null;

            }
        }

        #endregion

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

            int iconIndex = FileIcon;

            switch (RegistryItemType)

            {

                case RegistryItemType.Root:

                    iconIndex = ComputerIcon;

                    break;

                case RegistryItemType.Key:

                    iconIndex = FolderIcon;

                    break;

            }

#if NETFRAMEWORK

            using (Icon icon = TryGetIcon(iconIndex, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, size))

#else

            using Icon icon = TryGetIcon(iconIndex, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, size);

#endif

            return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        //protected override BrowsableObjectInfoLoader<TPath, TItems, TSubItems, TFactory> DeepCloneOverride() => new RegistryKeyLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>(default, RegistryItemTypes, (IFileSystemObjectComparer<IFileSystemObject>)FileSystemObjectComparer.DeepClone(), WorkerReportsProgress, WorkerSupportsCancellation);

        //private readonly RegistryItemTypes _registryItemTypes = RegistryItemTypes.None;

        //public RegistryItemTypes RegistryItemTypes
        //{

        //    get => _registryItemTypes;

        //    set => _ = this.SetBackgroundWorkerProperty(nameof(RegistryItemTypes), nameof(_registryItemTypes), value, typeof(RegistryKeyLoader<TPath, TItems, TSubItems, TFactory, TItemsFactory>), true);

        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryKeyLoader{TPath, TItems, TSubItems, TFactory, TItemsFactory}"/> class.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        ///// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        ///// <param name="registryItemTypes">The registry item types to load.</param>
        //public RegistryKeyLoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, RegistryItemTypes registryItemTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, registryItemTypes, new FileSystemObjectComparer<IFileSystemObject>(), workerReportsProgress, workerSupportsCancellation) => RegistryItemTypes = registryItemTypes;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RegistryKeyLoader{TPath, TItems, TSubItems, TFactory, TItemsFactory}"/> class using a custom comparer.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        ///// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        ///// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        ///// <param name="registryItemTypes">The registry item types to load.</param>
        //public RegistryKeyLoader(BrowsableObjectTreeNode<TPath, TItems, TFactory> path, RegistryItemTypes registryItemTypes, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer, bool workerReportsProgress, bool workerSupportsCancellation) : base(path, (IFileSystemObjectComparer<IFileSystemObject>)fileSystemObjectComparer, workerReportsProgress, workerSupportsCancellation) => _registryItemTypes = registryItemTypes;

        //public override bool CheckFilter(string path)

        //{

        //    if (Filter == null) return true;

        //    foreach (string filter in Filter)

        //    {

        //        bool checkFilters(string[] filters)

        //        {

        //            foreach (string _filter in filters)

        //            {

        //                if ( string.IsNullOrEmpty( _filter ) ) continue;

        //                if (path.Length >= _filter.Length && path.Contains(_filter))

        //                    path = path.Substring(path.IndexOf(_filter) + _filter.Length);

        //                else return false;

        //            }

        //            return true;

        //        }

        //        return checkFilters(filter.Split('*'));

        //    }

        //    return true;

        //}

        public override IEnumerable<IBrowsableObjectInfo> GetItems()
        {
            switch (RegistryItemType)
            {
                case RegistryItemType.Root:

                    return typeof(Microsoft.Win32.Registry).GetFields().Select(f => new RegistryItemInfo((RegistryKey)f.GetValue(null)));

                case RegistryItemType.Key:

                    return GetItems(null, false);

                default:

                    throw new InvalidOperationException("The current item cannot be browsed.");
            }
        }

        public IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<RegistryKey> predicate)
        {
            if (RegistryItemType == RegistryItemType.Root)

                //{

                //if (RegistryItemTypes.HasFlag(RegistryItemTypes.RegistryKey))

                //{

                /*FieldInfo[] _registryKeyFields = */
                return typeof(Microsoft.Win32.Registry).GetFields().Select(f => (RegistryKey)f.GetValue(null)).Where(predicate).Select(item => new RegistryItemInfo(item));

            //string name;

            //foreach (FieldInfo fieldInfo in _registryKeyFields)

            //{  

            //checkAndAppend(name, name, false);

            //}

            //}

            //}

            else

                throw new ArgumentException("The given predicate is not valid for the current RegistryItemInfo.");
        }

        public IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<RegistryItemInfoEnumeratorStruct> predicate, bool catchExceptions)
        {

            //protected override void OnDoWork(DoWorkEventArgs e)
            //{

            //if (RegistryItemTypes == RegistryItemTypes.None)

            //    return;

            // todo: 'if' to remove if not necessary:

            // if (Path is IRegistryItemInfo registryItemInfo)

            // {

            // var paths = new ArrayBuilder<PathInfo>();

            // PathInfo pathInfo;

            //void checkAndAppend(string pathWithoutName, string name, bool isValue)

            //{

            //string path = pathWithoutName + IO.Path.PathSeparator + name;

            //if (CheckFilter(path))

            //    _ = paths.AddLast(pathInfo = new PathInfo(path, path.RemoveAccents(), name, null, RegistryItemInfo.DefaultRegistryKeyDeepClone, isValue));

            //}

            if (RegistryItemType == RegistryItemType.Key)

            {

                //string[] items;

                IEnumerable<RegistryItemInfo> keys;

                IEnumerable<RegistryItemInfo> values;

                void enumerate()
                {

                    if (predicate == null)

                    {

                        keys = RegistryKey.GetSubKeyNames().Select(item => new RegistryItemInfo($"{Path}\\{item}"));

                        values = RegistryKey.GetValueNames().Select(s => new RegistryItemInfo(Path, s));

                    }

                    else

                    {

                        keys = RegistryKey.GetSubKeyNames().Where(func: item => predicate(new RegistryItemInfoEnumeratorStruct(item, RegistryItemType.Key))).Select(item => new RegistryItemInfo($"{Path}\\{item}"));

                        values = RegistryKey.GetValueNames().Where(func: s => predicate(new RegistryItemInfoEnumeratorStruct(s, RegistryItemType.Value))).Select(s => new RegistryItemInfo(Path, s));

                    }

                }

                if (catchExceptions)

                    try

                    {

                        enumerate();

                        // foreach (string item in items)

                        // item.Substring(0, item.LastIndexOf(IO.Path.PathSeparator)), item.Substring(item.LastIndexOf(IO.Path.PathSeparator) + 1), false

                    }

                    catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { keys = null; values = null; }

                else

                    enumerate();

                return values == null ? keys : keys == null ? values : keys.AppendValues(values);

            }

            else

                throw new ArgumentException("The current predicate type is not valid with this RegistryItemInfo.");



            //IEnumerable<PathInfo> pathInfos;



            //if (FileSystemObjectComparer == null)

            //    pathInfos = (IEnumerable<PathInfo>)paths;

            //else

            //{

            //    var _paths = paths.ToList();

            //    _paths.Sort(FileSystemObjectComparer);

            //    pathInfos = (IEnumerable<PathInfo>)_paths;

            //}



            //using (IEnumerator<PathInfo> pathsEnum = pathInfos.GetEnumerator())



            //while (pathsEnum.MoveNext())

            //try

            //{

            //    do

            //ReportProgress(0, new BrowsableObjectTreeNode<TItems, TSubItems, TItemsFactory>((TItems)(pathsEnum.Current.IsValue ? ((IRegistryItemInfoFactory)Path.Factory).GetBrowsableObjectInfo(pathsEnum.Current.Path.Substring(0, pathsEnum.Current.Path.Length - pathsEnum.Current.Name.Length - 1 /* We remove one more character to remove the backslash between the registry key path and the registry key value name. */ ), pathsEnum.Current.Name) : Path.Factory.GetBrowsableObjectInfo(pathsEnum.Current.Path)), (TItemsFactory)Path.Factory.DeepClone()));

            //    while (pathsEnum.MoveNext());

            //}

            //catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }



            // }

        }

        //protected class PathInfo : IO.PathInfo
        //{

        //    /// <summary>
        //    /// Gets the localized name of this <see cref="PathInfo"/>.
        //    /// </summary>
        //    public override string LocalizedName => Name;

        //    /// <summary>
        //    /// Gets the name of this <see cref="PathInfo"/>.
        //    /// </summary>
        //    public override string Name { get; }

        //    public bool IsValue { get; }

        //    public RegistryKey RegistryKey { get; }

        //    public DeepClone<RegistryKey> RegistryKeyDelegate { get; }

        //    public PathInfo(string path, string normalizedPath, string name, RegistryKey registryKey, DeepClone<RegistryKey> registryKeyDelegate, bool isValue) : base(path, normalizedPath)
        //    {

        //        Name = name;

        //        RegistryKey = registryKey;

        //        RegistryKeyDelegate = registryKeyDelegate;

        //        IsValue = isValue;

        //    }
        //}

        ///// <summary>
        ///// Gets or sets the factory for this <see cref="RegistryItemInfo{TItems, TFactory}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="RegistryItemInfo{TItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}"/>.</exception>
        ///// <exception cref="ArgumentNullException">value is null.</exception>
        //public new RegistryItemInfoFactory Factory { get => (RegistryItemInfoFactory)base.Factory; set => base.Factory = value; }

        ///// <summary>
        ///// Renames or move to a relative path, or both, the current <see cref="RegistryItemInfo{TItems, TFactory}"/> with the specified name.
        ///// </summary>
        ///// <param name="newValue">The new name or relative path for this <see cref="RegistryItemInfo{TItems, TFactory}"/>.</param>
        //public override void Rename(string newValue)

        //{

        //    switch (RegistryItemType)

        //    {

        //        case RegistryItemType.RegistryRoot:

        //            throw new InvalidOperationException("This node is the registry root node and cannot be renamed.");

        //        case RegistryItemType.RegistryKey:

        //            // todo:

        //            throw new InvalidOperationException("This feature is currently not supported.");

        //        case RegistryItemType.RegistryValue:

        //            if (RegistryKey.GetValue(newValue) != null)

        //                throw new InvalidOperationException("A value with the specified name already exists in this registry key.");

        //            object value = RegistryKey.GetValue(Name);

        //            RegistryValueKind valueKind = RegistryKey.GetValueKind(Name);

        //            RegistryKey.DeleteValue(Name);

        //            RegistryKey.SetValue(newValue, value, valueKind);

        //            break;

        //    }

        //}

        // public override bool Equals(IFileSystemObject fileSystemObject) => Equals((object)fileSystemObject);

    }
}
