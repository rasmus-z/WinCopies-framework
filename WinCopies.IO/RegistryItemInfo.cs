using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
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
    public class RegistryItemInfo : BrowsableObjectInfo, IRegistryItemInfo
    {

        public static RegistryItemInfoComparer GetDefaultComparer() => new RegistryItemInfoComparer();

        public    override    bool IsRenamingSupported => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        public RegistryItemInfo() : this(new RegistryItemInfoFactory(), (IComparer<IFileSystemObject>) GetDefaultComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo"/> and associated <see cref="RegistryKeyLoader"/> use to create new instances of the <see cref="RegistryItemInfo"/> class.</param>
        public RegistryItemInfo(RegistryItemInfoFactory factory, IComparer<IFileSystemObject> comparer) : base(ShellObject.FromParsingName(KnownFolders.Computer.ParsingName).GetDisplayName(DisplayNameType.Default), FileType.SpecialFolder, comparer)
        {

            Factory = factory;

            Name = Path;

            RegistryItemType = RegistryItemType.RegistryRoot;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey) : this(registryKey, new RegistryItemInfoFactory(), (IComparer<IFileSystemObject>)GetDefaultComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo"/> and associated <see cref="RegistryKeyLoader"/> use to create new instances of the <see cref="RegistryItemInfo"/> class.</param>
        public RegistryItemInfo(RegistryKey registryKey, RegistryItemInfoFactory factory, IComparer<IFileSystemObject> comparer) : base(registryKey.Name, FileType.SpecialFolder, comparer)
        {

            Factory = factory;

            string[] name = registryKey.Name.Split('\\');

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            RegistryKey = registryKey;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="path">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(string path) : this(path, new RegistryItemInfoFactory(), (IComparer<IFileSystemObject>)GetDefaultComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="path">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo"/> and associated <see cref="RegistryKeyLoader"/> use to create new instances of the <see cref="RegistryItemInfo"/> class.</param>
        public RegistryItemInfo(string path, RegistryItemInfoFactory factory, IComparer<IFileSystemObject> comparer) : base(path, FileType.SpecialFolder, comparer)
        {

            Factory = factory;

            string[] name = path.Split('\\');

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            RegistryKey = Registry.OpenRegistryKey(path);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(RegistryKey registryKey, string valueName) : this(registryKey, valueName, new RegistryItemInfoFactory(), (IComparer<IFileSystemObject>)GetDefaultComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKey">The <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo"/> and associated <see cref="RegistryKeyLoader"/> use to create new instances of the <see cref="RegistryItemInfo"/> class.</param>
        public RegistryItemInfo(RegistryKey registryKey, string valueName, RegistryItemInfoFactory factory, IComparer<IFileSystemObject> comparer) : base(registryKey.Name, FileType.Other, comparer)

        {

            Factory = factory;

            Name = valueName;

            RegistryItemType = RegistryItemType.RegistryValue;

            RegistryKey = registryKey;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKeyPath">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        public RegistryItemInfo(string registryKeyPath, string valueName) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, new RegistryItemInfoFactory(), (IComparer<IFileSystemObject>)GetDefaultComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryItemInfo"/> class using a custom factory for <see cref="RegistryItemInfo"/>s.
        /// </summary>
        /// <param name="registryKeyPath">The path of the <see cref="Microsoft.Win32.RegistryKey"/> that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="valueName">The name of the value that the new <see cref="RegistryItemInfo"/> represents.</param>
        /// <param name="factory">The factory this <see cref="RegistryItemInfo"/> and associated <see cref="RegistryKeyLoader"/> use to create new instances of the <see cref="RegistryItemInfo"/> class.</param>
        public RegistryItemInfo(string registryKeyPath, string valueName, RegistryItemInfoFactory factory, IComparer<IFileSystemObject> comparer) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, factory, comparer) { }

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

            int iconIndex = 0;

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryRoot:

                    iconIndex = 15;

                    break;

                case RegistryItemType.RegistryKey:

                    iconIndex = 3;

                    break;

            }

            using (Icon icon = TryGetIcon(iconIndex, "shell32.dll", size))

                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        /// <summary>
        /// The Windows registry item type of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        public RegistryItemType RegistryItemType { get; }

        /// <summary>
        /// The <see cref="Microsoft.Win32.RegistryKey"/> that this <see cref="RegistryItemInfo"/> represents.
        /// </summary>
        public RegistryKey RegistryKey { get; }

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
        public override bool IsBrowsable => RegistryItemType == RegistryItemType.RegistryRoot || RegistryItemType == RegistryItemType.RegistryKey;

        /// <summary>
        /// Gets or sets the factory for this <see cref="RegistryItemInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="RegistryItemInfo"/> and its associated <see cref="ItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public new RegistryItemInfoFactory Factory { get => (RegistryItemInfoFactory)base.Factory; set => base.Factory = value; }

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public override IBrowsableObjectInfo Clone()
        {

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryRoot:

                    return Factory.GetBrowsableObjectInfo();

                case RegistryItemType.RegistryKey:

                    return Factory.GetBrowsableObjectInfo(RegistryKey);

                case RegistryItemType.RegistryValue:

                    return Factory.GetBrowsableObjectInfo(RegistryKey, Name);

                default:

                    return null;

            }

        }

        /// <summary>
        /// Returns the parent of this <see cref="RegistryItemInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="RegistryItemInfo"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryKey:

                    string[] path = RegistryKey.Name.Split('\\');

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
        /// Disposes the current <see cref="BrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public override void Dispose()
        {
            base.Dispose();

            RegistryKey.Dispose();
        }

        /// <summary>
        /// Loads the items of this <see cref="RegistryItemInfo"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new RegistryKeyLoader(workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

        /// <summary>
        /// Loads the items of this <see cref="RegistryItemInfo"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new RegistryKeyLoader(workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

        /// <summary>
        /// Renames or move to a relative path, or both, the current <see cref="RegistryItemInfo"/> with the specified name.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="RegistryItemInfo"/>.</param>
        public override void Rename(string newValue)

        {

            switch (RegistryItemType)

            {

                case RegistryItemType.RegistryRoot:

                    throw new InvalidOperationException("This node is the registry root node and cannot be renamed.");

                case RegistryItemType.RegistryKey:

                    // todo:

                    throw new InvalidOperationException("This feature is currently not supported.");

                case RegistryItemType.RegistryValue:

                    if (RegistryKey.GetValue(newValue) != null)

                        throw new InvalidOperationException("A value with the specified name already exists in this registry key.");

                    object value = RegistryKey.GetValue(Name);

                    RegistryValueKind valueKind = RegistryKey.GetValueKind(Name);

                    RegistryKey.DeleteValue(Name);

                    RegistryKey.SetValue(newValue, value, valueKind);

                    break;

            }

        }
    }
}
