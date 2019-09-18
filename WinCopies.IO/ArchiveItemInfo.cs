﻿using SevenZip;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using TsudaKageyu;

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

    public delegate ArchiveFileInfo? ArchiveFileInfoDeepClone(ArchiveFileInfo? obj, string archivePath);

    /// <summary>
    /// Represents an archive item.
    /// </summary>
    public class ArchiveItemInfo/*<TItems, TFactory>*/ : ArchiveItemInfoProvider/*<TItems, TFactory>*/, IArchiveItemInfo // where TItems : BrowsableObjectInfo, IArchiveItemInfo where TFactory : BrowsableObjectInfoFactory, IArchiveItemInfoFactory
    {

        // public override bool IsRenamingSupported => false;

        public static ArchiveFileInfoDeepClone DefaultArchiveFileInfoDeepClone { get; } = (ArchiveFileInfo? archiveFileInfo, string archivePath) => archiveFileInfo.HasValue
                ? (ArchiveFileInfo?)new SevenZipExtractor(archivePath).ArchiveFileData.First(item => item.FileName.ToLower() == archiveFileInfo.Value.FileName)
                : null;

        /// <summary>
        /// Gets the localized path of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override string LocalizedName => ArchiveShellObject.LocalizedName;

        /// <summary>
        /// Gets the name of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo{TItems, TFactory}"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

        /// <summary>
        /// The factory used to create the new <see cref="IArchiveItemInfo"/>s.
        /// </summary>
        public sealed override IArchiveItemInfoFactory ArchiveItemInfoFactory { get => Factory; set => Factory = (TFactory)value; }

        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        private DeepClone<ArchiveFileInfo?> _archiveFileInfoDelegate;

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; private set; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{TItems, TFactory}"/> represents a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) : this(path, fileType, archiveShellObject, archiveFileInfo, archiveFileInfoDelegate, default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class using a custom factory for <see cref="ArchiveItemInfo{TItems, TFactory}"/>s.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{TItems, TFactory}"/> represents a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        /// <param name="factory">The factory this <see cref="ArchiveItemInfo{TItems, TFactory}"/> and associated <see cref="ArchiveLoader{TPath, TItems, TFactory}"/> use to create new instances of the <see cref="ArchiveItemInfo{TItems, TFactory}"/> class.</param>
        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate, IArchiveItemInfoFactory factory) : base(path, fileType, (TFactory)(factory ?? new ArchiveItemInfoFactory<TItems>()))

        {

            if (fileType == FileType.SpecialFolder)

                // todo:

                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

            _archiveFileInfoDelegate = archiveFileInfoDelegate;

            ArchiveFileInfo = archiveFileInfo;

            if (ArchiveFileInfo.HasValue && !path.EndsWith(ArchiveFileInfo.Value.FileName))

                // todo:

                throw new ArgumentException($"'{nameof(path)}' must end with '{nameof(ArchiveFileInfo.Value.FileName)}'");

            ArchiveShellObject = archiveShellObject;

#if DEBUG 

            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

#endif

            // Path = path;

#if DEBUG

            Debug.WriteLine("path: " + path);

#endif

        }

        /// <summary>
        /// Loads the items of this <see cref="ArchiveItemInfo{TItems, TFactory}"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo<TItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo{TItems, TFactory}"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((IBrowsableObjectInfoLoader)new ArchiveLoader<ArchiveItemInfo<TItems, TFactory>, TItems, TFactory>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.
        /// </summary>
        /// <returns>the parent of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>.</returns>
        protected override IBrowsableObjectInfo GetParent()
        {

            IBrowsableObjectInfo result;

            if (Path.Length > ArchiveShellObject.Path.Length)

            {

                string path = Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator));

                ArchiveFileInfo? archiveFileInfo = null;

                using (var extractor = new SevenZipExtractor(ArchiveShellObject.Path))

                    foreach (ArchiveFileInfo item in extractor.ArchiveFileData)

                        if (item.FileName.ToLower() == path.ToLower())

                            archiveFileInfo = item;

                result = Factory.GetBrowsableObjectInfo(path, FileType.Folder, ArchiveShellObject, archiveFileInfo, _archiveFileInfo => ArchiveItemInfo.DefaultArchiveFileInfoDeepClone(_archiveFileInfo, ArchiveShellObject.Path) /*archiveParentFileInfo.Value*/);

            }

            else

                result = ArchiveShellObject;

            return result /*&& Path.Contains(IO.Path.PathSeparator)*/;

        }

        /// <summary>
        /// Gets a deep clone of this <see cref="ArchiveItemInfo{TItems, TFactory}"/>. The <see cref="BrowsableObjectInfo.OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        protected override BrowsableObjectInfo DeepCloneOverride() => new ArchiveItemInfo<TItems, TFactory>(Path, FileType, (IShellObjectInfo)ArchiveShellObject.DeepClone(), _archiveFileInfoDelegate(ArchiveFileInfo), _archiveFileInfoDelegate, (ArchiveItemInfoFactory<TItems>)Factory.DeepClone());

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);

            if (disposing)

            {

                ArchiveFileInfo = null;

                _archiveFileInfoDelegate = null;

            }

        }

        // public override string ToString() => System.IO.Path.GetFileName(Path);

        private Icon TryGetIcon(System.Drawing.Size size) =>

            // if (System.IO.Path.HasExtension(Path))

            Microsoft.WindowsAPICodePack.Shell.FileOperation.GetFileInfo(System.IO.Path.GetExtension(Path), Microsoft.WindowsAPICodePack.Shell.FileAttributes.Normal, Microsoft.WindowsAPICodePack.Shell.GetFileInfoOptions.Icon | Microsoft.WindowsAPICodePack.Shell.GetFileInfoOptions.UseFileAttributes).Icon?.TryGetIcon(size, 32, true, true) ?? TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);// else// return TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

            using (Icon icon = TryGetIcon(size))

                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsOrValuesReconstruction => true; // True because of the ShellObjectInfo's ShellObject

    }

}
