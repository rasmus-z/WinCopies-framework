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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.WindowsAPICodePack.Shell;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using WinCopies.Util;
using static WinCopies.IO.Path;
using Microsoft.WindowsAPICodePack.COMNative.Shell;
using System.Linq;
using SevenZip;
using WinCopies.Linq;
using Microsoft.WindowsAPICodePack.PortableDevices;
using WinCopies.Collections;

namespace WinCopies.IO
{
    public struct ShellObjectInfoEnumeratorStruct
    {
        public ShellObject ShellObject { get; }

        public IPortableDevice PortableDevice { get; }

        public ArchiveFileInfoEnumeratorStruct? ArchiveFileInfoEnumeratorStruct { get; }

        internal ShellObjectInfoEnumeratorStruct(ShellObject shellObject)
        {
            ShellObject = shellObject;

            PortableDevice = null;

            ArchiveFileInfoEnumeratorStruct = null;
        }

        internal ShellObjectInfoEnumeratorStruct(IPortableDevice portableDevice)
        {
            ShellObject = null;

            PortableDevice = portableDevice;

            ArchiveFileInfoEnumeratorStruct = null;
        }

        internal ShellObjectInfoEnumeratorStruct(ArchiveFileInfoEnumeratorStruct archiveFileInfoEnumeratorStruct)
        {
            ShellObject = null;

            PortableDevice = null;

            ArchiveFileInfoEnumeratorStruct = archiveFileInfoEnumeratorStruct;
        }
    }

    /// <summary>
    /// Represents an archive item. This struct is used in enumeration methods.
    /// </summary>
    public struct ArchiveFileInfoEnumeratorStruct
    {
        /// <summary>
        /// Gets the path of the current archive item. This property is set only when <see cref="ArchiveFileInfo"/> is <see langword="null"/>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the <see cref="SevenZip.ArchiveFileInfo"/> that represents the cyrrent archive item. This property is set only when <see cref="Path"/> is <see langword="null"/>.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFileInfoEnumeratorStruct"/> struct with the given path.
        /// </summary>
        /// <param name="path">The path of the archive item.</param>
        public ArchiveFileInfoEnumeratorStruct(string path)
        {
            Path = path ?? throw GetArgumentNullException(nameof(path));

            ArchiveFileInfo = null;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ArchiveFileInfoEnumeratorStruct"/> struct with the given <see cref="SevenZip.ArchiveFileInfo"/>.
        ///// </summary>
        ///// <param name="path">The <see cref="SevenZip.ArchiveFileInfo"/> that represents the archive item.</param>
        public ArchiveFileInfoEnumeratorStruct(ArchiveFileInfo archiveFileInfo)
        {
            Path = null;

            ArchiveFileInfo = archiveFileInfo;
        }
    }

    /// <summary>
    /// Represents a file system item.
    /// </summary>
    public class ShellObjectInfo/*<TItems, TArchiveItemInfoItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IShellObjectInfo // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TArchiveItemInfoItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {
        private IBrowsableObjectInfo _parent;

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether the current item has particularities.
        /// </summary>
        public override bool IsSpecialItem
        {
            get
            {
                uint? value = ShellObject.Properties.System.FileAttributes.Value;

                if (value.HasValue)
                {
                    var _value = (Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes)value.Value;

                    return _value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Hidden) || _value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.System);
                }

                else

                    return false;
            }
        }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item. If the current <see cref="ShellObjectInfo"/> represents an archive file, this property returns the current <see cref="ShellObjectInfo"/>, or <see langword="null"/> otherwise.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject => FileType == FileType.Archive ? this : null;

        /// <summary>
        /// Gets a <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that represents this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public ShellObject ShellObject { get; private set; } = null;

        /// <summary>
        /// Gets the localized name of this <see cref="ShellObjectInfo"/> depending the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
        /// </summary>
        public override string LocalizedName => ShellObject.GetDisplayName(DisplayNameType.Default);

        /// <summary>
        /// Gets the name of this <see cref="ShellObjectInfo"/> depending of the associated <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> (see the <see cref="ShellObject"/> property for more details.
        /// </summary>
        public override string Name => ShellObject.Name;

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => ShellObject.Thumbnail.SmallBitmapSource;

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => ShellObject.Thumbnail.MediumBitmapSource;

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => ShellObject.Thumbnail.LargeBitmapSource;

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => ShellObject.Thumbnail.ExtraLargeBitmapSource;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ShellObjectInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => /*(*/ShellObject is IEnumerable<ShellObject>/*) && FileType != FileType.File && FileType != FileType.Link*/; // FileType == FileTypes.Folder || FileType == FileTypes.Drive || (FileType == FileTypes.SpecialFolder && SpecialFolder != SpecialFolders.Computer) || FileType == FileTypes.Archive;

        public Stream ArchiveFileStream { get; private set; }          /*new FileStream(_archiveItemInfoProvider.ArchiveShellObject.Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None)*/

        public bool IsArchiveOpen => ArchiveFileStream is object;

        /// <summary>
        /// Gets the type name of the current <see cref="ShellObjectInfo"/>. This value corresponds to the description of the file's extension.
        /// </summary>
        public override string ItemTypeName => ShellObject.Properties.System.ItemTypeText.Value;

        public override string Description => ShellObject.Properties.System.FileDescription.Value;

        public override Size? Size
        {
            get
            {
                ulong? value = ShellObject.Properties.System.Size.Value;

                if (value.HasValue)

                    return new Size(value.Value);

                else

                    return null;
            }
        }

#if NETFRAMEWORK

        public override IBrowsableObjectInfo Parent => _parent ?? (_parent = GetParent());

#else

        public override IBrowsableObjectInfo Parent => _parent ??= GetParent();

#endif

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ShellObjectInfo"/> class with a given <see cref="FileType"/> and <see cref="SpecialFolder"/> using custom factories for <see cref="ShellObjectInfo"/>s and <see cref="ArchiveItemInfo"/>s.
        ///// </summary>
        ///// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        ///// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        ///// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.None"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        ///// <param name="shellObject">The <see cref="Microsoft.WindowsAPICodePack.Shell.ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        private ShellObjectInfo(string path, FileType fileType, ShellObject shellObject) : base(path, fileType) => ShellObject = shellObject;

        #region Methods

        public static ShellObjectInfo From(ShellObject shellObject)
        {
            if ((shellObject ?? throw GetArgumentNullException(nameof(shellObject))) is ShellFolder shellFolder)
            {
                if (shellObject is ShellFileSystemFolder shellFileSystemFolder)
                {
                    (string path, FileType fileType) = shellFileSystemFolder is FileSystemKnownFolder ? (shellObject.ParsingName, FileType.KnownFolder) : (shellFileSystemFolder.Path, FileType.Folder);

                    return System.IO.Directory.GetParent(path) is null
                        ? new ShellObjectInfo(path, FileType.Drive, shellObject)
                        : new ShellObjectInfo(path, fileType, shellObject);
                }

                switch (shellObject)
                {
                    case NonFileSystemKnownFolder nonFileSystemKnownFolder:
                        return new ShellObjectInfo(nonFileSystemKnownFolder.Path, FileType.KnownFolder, shellObject);
                    case ShellNonFileSystemFolder _:
                        return new ShellObjectInfo(shellObject.ParsingName, FileType.Folder, shellObject);
                }
            }

            if (shellObject is ShellLink shellLink)

                return new ShellObjectInfo(shellLink.Path, FileType.Link, shellObject);

            if (shellObject is ShellFile shellFile)

                return new ShellObjectInfo(shellFile.Path, IsSupportedArchiveFormat(System.IO.Path.GetExtension(shellFile.Path)) ? FileType.Archive : shellFile.IsLink ? FileType.Link : System.IO.Path.GetExtension(shellFile.Path) == ".library-ms" ? FileType.Library : FileType.File, shellObject);

            throw new ArgumentException($"The given {nameof(Microsoft.WindowsAPICodePack.Shell.ShellObject)} is not supported.");
        }

        public void OpenArchive(Stream stream)
        {
            CloseArchive();

            FileType.ThrowIfInvalidEnumValue(true, FileType.Archive);

            ArchiveFileStream = stream;
        }

        public void CloseArchive()
        {
            ArchiveFileStream.Flush();

            ArchiveFileStream.Dispose();

            ArchiveFileStream = null;
        }

        /// <summary>
        /// Gets a string representation of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="ShellObjectInfo"/>.</returns>
        public override string ToString() => string.IsNullOrEmpty(Path) ? ShellObject.GetDisplayName(DisplayNameType.Default) : System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Returns the parent of this <see cref="ShellObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="ShellObjectInfo"/>.</returns>
        private IBrowsableObjectInfo GetParent()
        {
            //#if !NETFRAMEWORK

            //            static

            //#endif

            //    (FileType, SpecialFolder) getFileType(ShellObject _shellObject)

            //{

            //    SpecialFolder specialFolder = IO.Path.GetSpecialFolder(_shellObject);

            //    FileType fileType = specialFolder == SpecialFolder.None ? FileType.Folder : FileType.KnownFolder;

            //    return (fileType, specialFolder);

            //}

            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Archive) || (FileType == FileType.KnownFolder && ShellObject.IsFileSystemObject) || (FileType == FileType.KnownFolder && ((IKnownFolder)ShellObject).FolderId.ToString() != Microsoft.WindowsAPICodePack.Shell.Guids.KnownFolders.Computer))

                return From(ShellObject.Parent);

            else if (FileType == FileType.Drive)

                return new ShellObjectInfo(Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer.Path, FileType.KnownFolder, ShellObject.FromParsingName(Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer.ParsingName));

            else return null;
        }

        protected override void Dispose(in bool disposing)
        {
            base.Dispose(disposing);

            ShellObject.Dispose();

            if (IsArchiveOpen)

                CloseArchive();

            if (disposing)

                ShellObject = null;
        }

        public virtual IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<ShellObjectInfoEnumeratorStruct> func)
        {
            ThrowIfNull(func, nameof(func));

            if (IsBrowsable)

                switch (FileType)
                {
                    case FileType.Archive:

                        return GetItems(item => func(new ShellObjectInfoEnumeratorStruct(item)));

                    default:

                        IEnumerable<IBrowsableObjectInfo> shellObjects = ((IEnumerable<ShellObject>)ShellObject).WherePredicate(item => func(new ShellObjectInfoEnumeratorStruct(item))).Select(shellObject => From(shellObject));

                        if (ShellObject.ParsingName == Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer.ParsingName)
                        {
                            var portableDeviceManager = new PortableDeviceManager();

                            portableDeviceManager.GetDevices();

                            IEnumerable<IBrowsableObjectInfo> portableDevices = portableDeviceManager.PortableDevices.WherePredicate(item => func(new ShellObjectInfoEnumeratorStruct(item))).Select(portableDevice => new PortableDeviceInfo(portableDevice));

                            if (shellObjects == null) return portableDevices;

                            else if (portableDevices == null) return shellObjects;

                            return shellObjects.AppendValues(portableDevices);
                        }

                        else return shellObjects;
                }

            else return null;
        }

        public virtual IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<ArchiveFileInfoEnumeratorStruct> func)
        {
            ThrowIfNull(func, nameof(func));

#if NETFRAMEWORK

            switch (FileType)
            {
                case FileType.Archive:
                    return GetArchiveItemInfoItems(func);
                default:
                    return null;
            }

#else

            return FileType switch
            {
                FileType.Archive => GetArchiveItemInfoItems(func),
                _ => null,
            };

#endif
        }

        public override IEnumerable<IBrowsableObjectInfo> GetItems() => GetItems((Predicate<ShellObjectInfoEnumeratorStruct>)(obj => true));

        private IEnumerable<IBrowsableObjectInfo> GetArchiveItemInfoItems(Predicate<ArchiveFileInfoEnumeratorStruct> func) => new Enumerable<IBrowsableObjectInfo>(() => new ArchiveItemInfoEnumerator(this, func));

        ///// <summary>
        ///// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        ///// </summary>
        ///// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
        //protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo) => base.OnDeepClone(browsableObjectInfo);

        ///// <summary>
        ///// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        ///// </summary>
        //protected override BrowsableObjectInfo DeepCloneOverride() => new ShellObjectInfo(Path, FileType, SpecialFolder, _shellObjectDelegate(ShellObject), _shellObjectDelegate);

        //private void SetFileSystemInfoProperties(ShellObject shellObject)
        //{

        //    if (FileType == FileType.Folder || (FileType == FileType.KnownFolder && shellObject.IsFileSystemObject))

        //        FileSystemInfoProperties = new DirectoryInfo(Path);

        //    else if (FileType == FileType.File || FileType == FileType.Archive || FileType == FileType.Link)

        //        FileSystemInfoProperties = new FileInfo(Path);

        //    else if (FileType == FileType.Drive)

        //    {

        //        FileSystemInfoProperties = new DirectoryInfo(Path);

        //        DriveInfoProperties = new DriveInfo(Path);

        //    }

        //    else if (FileType == FileType.KnownFolder)

        //        KnownFolderInfo = KnownFolderHelper.FromParsingName(shellObject.ParsingName);

        //}

        #endregion
    }
}
