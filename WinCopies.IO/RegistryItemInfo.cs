﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// The Windows registry item type.
    /// </summary>
    public enum RegistryItemType
    {

        /// <summary>
        /// The current instance represents the Windows registry root node.
        /// </summary>
        RegistryRoot,

        /// <summary>
        /// The current instance represents a Windows registry key.
        /// </summary>
        RegistryKey,

        /// <summary>
        /// The current instance represents a Windows registry value.
        /// </summary>
        RegistryValue

    }

    /// <summary>
    /// Represents a Windows registry item that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
    /// </summary>
    public class RegistryItemInfo<T> : BrowsableObjectInfo<T>, IRegistryItemInfo, IBrowsableObjectInfo<T> where T : IRegistryItemInfoFactory
    {

        /// <summary>
        /// Gets a default comparer for <see cref="FileSystemObject"/>s.
        /// </summary>
        /// <returns>A default comparer for <see cref="FileSystemObject"/>s.</returns>
        public static RegistryItemInfoComparer<IRegistryItemInfo> GetDefaultRegistryItemInfoComparer() => new RegistryItemInfoComparer<IRegistryItemInfo>();

        // public override bool IsRenamingSupported => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        public RegistryItemInfo() : this(default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo{T}"/> and associated <see cref="RegistryKeyLoader{T}"/> use to create new instances of the <see cref="RegistryItemInfo{T}"/> class.</param>
        public RegistryItemInfo(T factory) : base(ShellObject.FromParsingName(KnownFolders.Computer.ParsingName).GetDisplayName(DisplayNameType.Default), FileType.SpecialFolder, (T)(object.Equals(factory, null) ? (IRegistryItemInfoFactory)new RegistryItemInfoFactory() : factory))
        {

            Name = Path;

            RegistryItemType = RegistryItemType.RegistryRoot;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey) : this(registryKey, default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo{T}"/> and associated <see cref="RegistryKeyLoader{T}"/> use to create new instances of the <see cref="RegistryItemInfo{T}"/> class.</param>
        public RegistryItemInfo(RegistryKey registryKey, T factory) : base(registryKey.Name, FileType.SpecialFolder, (T)(object.Equals(factory, null) ? (IRegistryItemInfoFactory)new RegistryItemInfoFactory() : factory))
        {

            string[] name = registryKey.Name.Split(IO.Path.PathSeparator);

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            _registryKey = registryKey;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="path">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        public RegistryItemInfo(string path) : this(path, default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="path">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo{T}"/> and associated <see cref="RegistryKeyLoader{T}"/> use to create new instances of the <see cref="RegistryItemInfo{T}"/> class.</param>
        public RegistryItemInfo(string path, T factory) : base(path, FileType.SpecialFolder, (T)(object.Equals(factory, null) ? (IRegistryItemInfoFactory)new RegistryItemInfoFactory() : factory))
        {

            string[] name = path.Split(IO.Path.PathSeparator);

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            _registryKey = Registry.OpenRegistryKey(path);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey, string valueName) : this(registryKey, valueName, default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo{T}"/> and associated <see cref="RegistryKeyLoader{T}"/> use to create new instances of the <see cref="RegistryItemInfo{T}"/> class.</param>
        public RegistryItemInfo(RegistryKey registryKey, string valueName, T factory) : base(registryKey.Name, FileType.Other, (T)(object.Equals(factory, null) ? (IRegistryItemInfoFactory)new RegistryItemInfoFactory() : factory))

        {

            Name = valueName;

            RegistryItemType = RegistryItemType.RegistryValue;

            _registryKey = registryKey;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKeyPath">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        public RegistryItemInfo(string registryKeyPath, string valueName) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo{T}"/> class using a custom factory for <see cref="RegistryItemInfo{T}"/>s.
        /// </summary>
        /// <param name="registryKeyPath">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo{T}"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo{T}"/> and associated <see cref="RegistryKeyLoader{T}"/> use to create new instances of the <see cref="RegistryItemInfo{T}"/> class.</param>
        public RegistryItemInfo(string registryKeyPath, string valueName, T factory) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, (T)(object.Equals(factory, null) ? (IRegistryItemInfoFactory)new RegistryItemInfoFactory() : factory)) { }

        private const int FileIcon = 0;
        private const int ComputerIcon = 15;
        private const int FolderIcon = 3;

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

            int iconIndex = FileIcon;

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryRoot:

                    iconIndex = ComputerIcon;

                    break;

                case RegistryItemType.RegistryKey:

                    iconIndex = FolderIcon;

                    break;

            }

            using (Icon icon = TryGetIcon(iconIndex, "shell32.dll", size))

                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        /// <summary>
        /// The Windows registry item type of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public RegistryItemType RegistryItemType { get; }

        private RegistryKey _registryKey;

        /// <summary>
        /// The <see cref="Microsoft.Win32.RegistryKey"/> that this <see cref="RegistryItemInfo{T}"/> represents.
        /// </summary>
        public RegistryKey RegistryKey

        {

            get

            {

                if (_registryKey is null && RegistryItemType == RegistryItemType.RegistryKey)

                    OpenRegistryKey();

                return _registryKey;

            }

        }

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo{T}"/> represents.
        /// </summary>
        public void OpenRegistryKey()
        {

            if (RegistryItemType != RegistryItemType.RegistryKey)

                throw new InvalidOperationException("This item does not represent a registry key.");

            _registryKey = Registry.OpenRegistryKey(Path);

        }

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo{T}"/> represents using custom <see cref="RegistryKeyPermissionCheck"/> and <see cref="RegistryRights"/>.
        /// </summary>
        /// <param name="registryKeyPermissionCheck">Specifies whether security checks are performed when opening the registry key and accessing its name/value pairs.</param>
        /// <param name="registryRights">Specifies the access control rights that can be applied to the registry objects in registry key's scope.</param>
        public void OpenRegistryKey(RegistryKeyPermissionCheck registryKeyPermissionCheck, RegistryRights registryRights) => _registryKey = Registry.OpenRegistryKey(Path, registryKeyPermissionCheck, registryRights);

        /// <summary>
        /// Opens the <see cref="RegistryKey"/> that this <see cref="RegistryItemInfo{T}"/> represents with a <see cref="bool"/> value that indicates whether the registry key has to be opened with write-rights.
        /// </summary>
        /// <param name="writable">A <see cref="bool"/> value that indicates whether the registry key has to be opened with write-rights</param>
        public void OpenRegistryKey(bool writable) => _registryKey = Registry.OpenRegistryKey(Path, writable);

        /// <summary>
        /// Gets the localized path of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override string LocalizedName => Name;

        /// <summary>
        /// Gets the name of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="RegistryItemInfo{T}"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => RegistryItemType == RegistryItemType.RegistryRoot || RegistryItemType == RegistryItemType.RegistryKey;

        ///// <summary>
        ///// Gets or sets the factory for this <see cref="RegistryItemInfo{T}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="RegistryItemInfo{T}"/> and its associated <see cref="BrowsableObjectInfo{T}.ItemsLoader"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{T}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{T}"/>.</exception>
        ///// <exception cref="ArgumentNullException">value is null.</exception>
        //public new RegistryItemInfoFactory Factory { get => (RegistryItemInfoFactory)base.Factory; set => base.Factory = value; }

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo{T}"/>.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo{T}"/>.</returns>
        protected override BrowsableObjectInfo<T> DeepCloneOverride(bool? preserveIds)
        {

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryRoot:

                    return new RegistryItemInfo<T>((T)Factory.DeepClone(preserveIds));

                case RegistryItemType.RegistryKey:

                    return new RegistryItemInfo<T>(Path, (T)Factory.DeepClone(preserveIds));

                case RegistryItemType.RegistryValue:

                    return new RegistryItemInfo<T>(Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator)), Path.Substring(Path.LastIndexOf(IO.Path.PathSeparator) + 1), (T)Factory.DeepClone(preserveIds));

                default:

                    throw new InvalidOperationException(string.Format(WinCopies.Util.Generic.NoValidEnumValue, nameof(RegistryItemType), "WinCopies.IO.RegistryItemType"));

            }

        }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsReconstruction => base.NeedsObjectsReconstruction || !(_registryKey is null); // If _registryKey is null, reconstructing registry does not make sense, so we return false.

        /// <summary>
        /// Returns the parent of this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="RegistryItemInfo{T}"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryKey:

                    string[] path = RegistryKey.Name.Split(IO.Path.PathSeparator);

                    if (path.Length == 1)

                        return Factory.GetBrowsableObjectInfo();

                    var stringBuilder = new StringBuilder();

                    for (int i = 0; i < path.Length - 1; i++)

                        _ = stringBuilder.Append(path);

                    return Factory.GetBrowsableObjectInfo(stringBuilder.ToString());

                case RegistryItemType.RegistryValue:

                    return Factory.GetBrowsableObjectInfo(RegistryKey);

                default:

                    return null;

            }

        }

        /// <summary>
        /// Disposes the current <see cref="RegistryItemInfo{T}"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo{T}.ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected override void DisposeOverride(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
        {
            base.DisposeOverride(disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

            _registryKey?.Dispose();
        }

        /// <summary>
        /// Loads the items of this <see cref="RegistryItemInfo{T}"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((BrowsableObjectInfoLoader)new RegistryKeyLoader<IRegistryItemInfo<IRegistryItemInfoFactory>>((IRegistryItemInfo<IRegistryItemInfoFactory>)this, workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

        /// <summary>
        /// Loads the items of this <see cref="RegistryItemInfo{T}"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((BrowsableObjectInfoLoader)new RegistryKeyLoader<IRegistryItemInfo<IRegistryItemInfoFactory>>((IRegistryItemInfo<IRegistryItemInfoFactory>)this, workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

        ///// <summary>
        ///// Renames or move to a relative path, or both, the current <see cref="RegistryItemInfo{T}"/> with the specified name.
        ///// </summary>
        ///// <param name="newValue">The new name or relative path for this <see cref="RegistryItemInfo{T}"/>.</param>
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

        /// <summary>
        /// Determines whether the specified object is equal to the current object by calling <see cref="FileSystemObject.Equals(object)"/> and then, if the result is <see langword="true"/>, by testing the following things, in order: <paramref name="obj"/> implements the <see cref="IRegistryItemInfo"/> interface and <see cref="RegistryItemType"/> are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => base.Equals(obj) && (obj is IRegistryItemInfo _obj ? RegistryItemType == _obj.RegistryItemType : false);

        /// <summary>
        /// Compares the current object to a given <see cref="FileSystemObject"/>.
        /// </summary>
        /// <param name="registryItemInfo">The <see cref="FileSystemObject"/> to compare with.</param>
        /// <returns>The comparison result. See <see cref="IComparable{T}.CompareTo(T)"/> for more details.</returns>
        public int CompareTo(IRegistryItemInfo registryItemInfo) => GetDefaultRegistryItemInfoComparer().Compare(this, registryItemInfo);

        /// <summary>
        /// Determines whether the specified <see cref="IRegistryItemInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
        /// </summary>
        /// <param name="registryItemInfo">The <see cref="IRegistryItemInfo"/> to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public bool Equals(IRegistryItemInfo registryItemInfo) => Equals(registryItemInfo as object);

        /// <summary>
        /// Gets an hash code for this <see cref="RegistryItemInfo{T}"/>.
        /// </summary>
        /// <returns>The hash code returned by the <see cref="FileSystemObject.GetHashCode"/> and the hash code of the <see cref="RegistryItemType"/>.</returns>
        public override int GetHashCode() => base.GetHashCode() ^ RegistryItemType.GetHashCode();

    }
}
