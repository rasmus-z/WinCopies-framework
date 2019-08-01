﻿using SevenZip;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using TsudaKageyu;

using static WinCopies.Util.Util;

namespace WinCopies.IO
{

    public interface IArchiveItemInfo : IBrowsableObjectInfo, IArchiveItemInfoProvider
    {

        ArchiveFileInfo? ArchiveFileInfo { get; }

    }

    /// <summary>
    /// Represents an archive that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
    /// </summary>
    public class ArchiveItemInfo : ArchiveItemInfoProvider, IArchiveItemInfo
    {

        /// <summary>
        /// Gets the localized path of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override string LocalizedName => ArchiveShellObject.LocalizedName;

        /// <summary>
        /// Gets the name of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

        IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => Factory;

        /// <summary>
        /// Gets or sets the factory for this <see cref="ArchiveItemInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ArchiveItemInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public new ArchiveItemInfoFactory Factory { get => (ArchiveItemInfoFactory)base.Factory; set => base.Factory = value; }

        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        public ArchiveFileInfo? ArchiveFileInfo { get; }

        protected override IShellObjectInfo ArchiveShellObjectOverride { get; } = null;

        public IShellObjectInfo ArchiveShellObject => ArchiveShellObjectOverride;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfo">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo"/> represent a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        public ArchiveItemInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) : this(archiveShellObject, archiveFileInfo, path, fileType, new ArchiveItemInfoFactory()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class using a custom factory for <see cref="ArchiveItemInfo"/>s.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfo">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo"/> represent a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        /// <param name="factory">The factory this <see cref="ArchiveItemInfo"/> and associated <see cref="ArchiveLoader"/> use to create new instances of the <see cref="ArchiveItemInfo"/> class.</param>
        public ArchiveItemInfo(IShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType, ArchiveItemInfoFactory factory) : base(path, fileType)

        {

            if (fileType == FileType.SpecialFolder)

                // todo:

                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

            if (archiveFileInfo.HasValue && !path.EndsWith(archiveFileInfo.Value.FileName))

                // todo:

                throw new ArgumentException($"'{nameof(path)}' must end with '{nameof(archiveFileInfo.Value.FileName)}'");

            ArchiveShellObjectOverride = archiveShellObject;

#if DEBUG 

            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

#endif

            // Path = path;

            if (archiveFileInfo.HasValue)

                ArchiveFileInfo = archiveFileInfo.Value;

            Factory = factory;

#if DEBUG

            Debug.WriteLine("path: " + path);

#endif

            #region Comments

            // ArchiveFileInfo? archiveParentFileInfo = null;

            // FileType _fileType = FileType.None;

            // SevenZipExtractor archiveExtractor = new SevenZipExtractor(archiveShellObject.Path);

            // System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

            //foreach (ArchiveFileInfo _archiveFileInfo in archiveFileData)

            //{

            //if (_archiveFileInfo.FileName.Substring(0, path.LastIndexOf('\\')) == parent)

            //{

            // archiveParentFileInfo = _archiveFileInfo;

            //_fileType = _archiveFileInfo.IsDirectory ? FileType.Folder : FileType.File;

            //break;

            //}

            //else

            // todo:

            //throw new Exception("");

            //}

            #endregion

        }

        /// <summary>
        /// Loads the items of this <see cref="ArchiveItemInfo"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new ArchiveLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((IBrowsableObjectInfoLoader<IBrowsableObjectInfo>)new ArchiveLoader(workerReportsProgress, workerSupportsCancellation, GetAllEnumFlags<FileTypes>()));

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        /// <returns>the parent of this <see cref="ArchiveItemInfo"/>.</returns>
        protected override IBrowsableObjectInfo GetParent() => Path.Length > ArchiveShellObject.Path.Length /*&& Path.Contains("\\")*/ ? Factory.GetBrowsableObjectInfo(ArchiveShellObject, null/*archiveParentFileInfo.Value*/, Path.Substring(0, Path.LastIndexOf('\\')), FileType.Folder) : ArchiveShellObject;

        /// <summary>
        /// Currently not implemented.
        /// </summary>
        /// <param name="newValue"></param>
        public override void Rename(string newValue) =>

            // string getNewPath() => System.IO.Path.GetDirectoryName(Path) + "\\" + newValue;

            //SevenZipCompressor a = new SevenZipCompressor();

            //Dictionary<int, string> dico = new Dictionary<int, string>();

            //dico.Add(ArchiveFileInfo.Index, ArchiveFileInfo.FileName);

            //a.ModifyArchive(ArchiveShellObject.Path, dico);

            // todo:

            throw new NotSupportedException("This feature is currently not supported for the content archive items.");

        /// <summary>
        /// Returns an <see cref="ArchiveItemInfo"/> that represents the same item that the current <see cref="ArchiveItemInfo"/>.
        /// </summary>
        /// <returns>An <see cref="ArchiveItemInfo"/> that represents the same item that the current <see cref="ArchiveItemInfo"/>.</returns>
        public override IBrowsableObjectInfo Clone() => Factory.GetBrowsableObjectInfo(new ShellObjectInfo(ArchiveShellObject.ShellObject, ArchiveShellObject.Path, ArchiveShellObject.FileType, ArchiveShellObject.SpecialFolder), ArchiveFileInfo, Path, FileType);

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {

            base.Dispose();

            ArchiveShellObject.Dispose();

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

        // public ArchiveFileInfo ArchiveFileInfo { get; } = null;

        // public bool AreItemsLoaded { get => areItemsLoaded; private set => OnPropertyChanged(nameof(AreItemsLoaded), nameof(areItemsLoaded), value); }

        // private ReadOnlyObservableCollection<IBrowsableObjectInfo> items = null;

        // public event PropertyChangedEventHandler PropertyChanged;

        //public ReadOnlyObservableCollection<IBrowsableObjectInfo> Items
        //{

        //    get => items;

        //    public set

        //    {

        //        OnPropertyChanged(nameof(Items), nameof(items), value);

        //        if (value != null)

        //            AreItemsLoaded = true;

        //    }

        //}

        // public FileTypes FileType { get; } = FileTypes.None;

        //public BrowsableObjectInfoItemsLoader ItemsLoader
        //{

        //    get => ItemsLoader;

        //    set => ItemsLoader = (FolderLoader)value;

        //}

    }

}
