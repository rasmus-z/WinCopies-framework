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

using SevenZip;

using System;
using System.Windows.Media.Imaging;

using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using System.Linq;
using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// Represents an archive item.
    /// </summary>
    public interface IArchiveItemInfo : IArchiveItemInfoProvider
    {

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        ArchiveFileInfo? ArchiveFileInfo { get; }

    }

    /// <summary>
    /// Represents an archive item.
    /// </summary>
    public class ArchiveItemInfo/*<TItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IArchiveItemInfo // where TItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        private IBrowsableObjectInfo _parent;

        #region Properties

        public override bool IsSpecialItem
        {
            get
            {
                if (ArchiveFileInfo.HasValue)
                {
                    var value = (Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes)ArchiveFileInfo.Value.Attributes;

                    return value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Hidden) || value.HasFlag(Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.System);
                }

                else

                    return false;
            }
        }

        public override string LocalizedName => "N/A";

        /// <summary>
        /// Gets the name of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(SmallIconSize);

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(MediumIconSize);

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(LargeIconSize);

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(ExtraLargeIconSize);

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; private set; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject { get; }

        public override string ItemTypeName => GetItemTypeName(System.IO.Path.GetExtension(Path), FileType);

        /// <summary>
        /// Not applicable for this item kind.
        /// </summary>
        public override string Description => "N/A";

        public override Size? Size
        {
            get
            {
                if (ArchiveFileInfo.HasValue)

                    return new Size(ArchiveFileInfo.Value.Size);

                else

                    return null;
            }
        }

#if NETFRAMEWORK

        public override IBrowsableObjectInfo Parent => _parent ?? (_parent=GetParent());

#else

        public override IBrowsableObjectInfo Parent => _parent ??= GetParent();

#endif

        #endregion

        private ArchiveItemInfo(in string path, in FileType fileType, in IShellObjectInfo archiveShellObject, in ArchiveFileInfo? archiveFileInfo/*, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate*/) : base(path, fileType)
        {
            ArchiveShellObject = archiveShellObject;

            ArchiveFileInfo = archiveFileInfo;
        }

        #region Methods

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class using a custom factory for <see cref="ArchiveItemInfo"/>s.
        ///// </summary>
        ///// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        ///// <param name="path">The full path to this archive item</param>
        ///// <param name="fileType">The file type of this archive item</param>
        public static ArchiveItemInfo From(in IShellObjectInfo archiveShellObjectInfo, in ArchiveFileInfo archiveFileInfo)

        {

            ThrowIfNull(archiveShellObjectInfo, nameof(archiveShellObjectInfo));

            string extension = System.IO.Path.GetExtension(archiveFileInfo.FileName);

            return new ArchiveItemInfo(System.IO.Path.Combine(archiveShellObjectInfo.Path, archiveFileInfo.FileName), archiveFileInfo.IsDirectory ? FileType.Folder : extension == ".lnk" ? FileType.Link : extension == ".library.ms" ? FileType.Library : FileType.File, archiveShellObjectInfo, archiveFileInfo);

        }

        public static ArchiveItemInfo From(in IShellObjectInfo archiveShellObjectInfo, in string archiveFilePath)

        {

            ThrowIfNull(archiveShellObjectInfo, nameof(archiveShellObjectInfo));

            return new ArchiveItemInfo(System.IO.Path.Combine(archiveShellObjectInfo.Path, archiveFilePath), FileType.Folder, archiveShellObjectInfo, null);

        }

        private IBrowsableObjectInfo GetParent()
        {

            IBrowsableObjectInfo result;

            if (Path.Length > ArchiveShellObject.Path.Length)

            {

                string path = Path.Substring(0, Path.LastIndexOf(WinCopies.IO.Path.PathSeparator));

                ArchiveFileInfo? archiveFileInfo = null;

                using (var extractor = new SevenZipExtractor(ArchiveShellObject.ArchiveFileStream))

                    archiveFileInfo = extractor.ArchiveFileData.FirstOrDefault(item => string.Compare(item.FileName, path, StringComparison.OrdinalIgnoreCase) == 0);

                result = new ArchiveItemInfo(path, FileType.Folder, ArchiveShellObject, archiveFileInfo/*, _archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(_archiveFileInfo, ArchiveShellObject.Path) archiveParentFileInfo.Value*/);

            }

            else

                result = ArchiveShellObject;

            return result /*&& Path.Contains(IO.Path.PathSeparator)*/;

        }

        public IEnumerable<IBrowsableObjectInfo> GetItems(in Predicate<ArchiveFileInfoEnumeratorStruct> func) => func is null ? throw GetArgumentNullException(nameof(func)) : GetArchiveItemInfoItems(func);

        public override IEnumerable<IBrowsableObjectInfo> GetItems() => GetArchiveItemInfoItems(null);

        private IEnumerable<IBrowsableObjectInfo> GetArchiveItemInfoItems(Predicate<ArchiveFileInfoEnumeratorStruct> func) => new Enumerable<IBrowsableObjectInfo>(() => new ArchiveItemInfoEnumerator(this, func));

        protected override void Dispose(in bool disposing)
        {

            base.Dispose(disposing);

            if (disposing)

                ArchiveFileInfo = null;

        }

        #endregion

    }

}
