using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using System;
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

    public class RegistryItemInfo : BrowsableObjectInfo, IRegistryItemInfo
    {

        public RegistryItemInfo() : this(new RegistryItemInfoFactory()) { }

        public RegistryItemInfo(RegistryItemInfoFactory factory) : base(ShellObject.FromParsingName(KnownFolders.Computer.ParsingName).GetDisplayName(DisplayNameType.Default), FileType.SpecialFolder)
        {

            Factory = factory;

            Name = Path;

            RegistryItemType = RegistryItemType.RegistryRoot;

        }

        public RegistryItemInfo(RegistryKey registryKey) : this(registryKey, new RegistryItemInfoFactory()) { }

        public RegistryItemInfo(RegistryKey registryKey, RegistryItemInfoFactory factory) : base(registryKey.Name, FileType.SpecialFolder)
        {

            Factory = factory;

            string[] name = registryKey.Name.Split('\\');

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            RegistryKey = registryKey;

        }

        public RegistryItemInfo(string path) : this(path, new RegistryItemInfoFactory()) { }

        public RegistryItemInfo(string path, RegistryItemInfoFactory factory) : base(path, FileType.SpecialFolder)
        {

            Factory = factory;

            string[] name = path.Split('\\');

            Name = name[name.Length - 1];

            RegistryItemType = RegistryItemType.RegistryKey;

            try

            {

                RegistryKey = Registry.OpenRegistryKey(path);

            }

            catch (Exception ex) when (ex.Is(false, typeof(SecurityException), typeof(IOException), typeof(UnauthorizedAccessException))) { }

        }

        public RegistryItemInfo(RegistryKey registryKey, string valueName) : this(registryKey, valueName, new RegistryItemInfoFactory()) { }

        public RegistryItemInfo(RegistryKey registryKey, string valueName, RegistryItemInfoFactory factory) : base(registryKey.Name, FileType.Other)

        {

            Factory = factory;

            Name = valueName;

            RegistryItemType = RegistryItemType.RegistryValue;

            RegistryKey = registryKey;

        }

        public RegistryItemInfo(string registryKeyPath, string valueName) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, new RegistryItemInfoFactory()) { }

        public RegistryItemInfo(string registryKeyPath, string valueName, RegistryItemInfoFactory factory) : this(Registry.OpenRegistryKey(registryKeyPath), valueName, factory) { }

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

        public RegistryItemType RegistryItemType { get; }

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

        public new RegistryItemInfoFactory Factory        {            get => (RegistryItemInfoFactory)base.Factory; set => base.Factory = value;        }

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

        public override void Dispose()
        {
            base.Dispose();

            RegistryKey.Dispose();
        }

        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems(new RegistryKeyItemsLoader(workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync(new RegistryKeyItemsLoader(workerReportsProgress, workerSupportsCancellation, RegistryItemType == RegistryItemType.RegistryRoot ? RegistryItemTypes.RegistryKey : RegistryItemTypes.RegistryKey | RegistryItemTypes.RegistryValue));

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
