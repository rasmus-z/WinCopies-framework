using SevenZip;

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
    /// Represents an archive that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
    /// </summary>
    public interface IArchiveItemInfo : IArchiveItemInfoProvider
    {

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        ArchiveFileInfo? ArchiveFileInfo { get; }

    }

    /// <summary>
    /// Represents an archive that can be used with interoperability with the other <see cref="IBrowsableObjectInfo"/> objects.
    /// </summary>
    public class ArchiveItemInfo<T> : ArchiveItemInfoProvider<T>, IArchiveItemInfo, IBrowsableObjectInfo<T> where T : IArchiveItemInfoFactory
    {

        // public override bool IsRenamingSupported => false;

        /// <summary>
        /// Gets the localized path of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override string LocalizedName => ArchiveShellObject.LocalizedName;

        /// <summary>
        /// Gets the name of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="ArchiveItemInfo{T}"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => If(IfCT.Or, IfCM.Logical, IfComp.Equal, FileType, FileType.Folder, FileType.Drive, FileType.Archive);

        ///// <summary>
        ///// Gets or sets the factory for this <see cref="ArchiveItemInfo{T}"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="ArchiveItemInfo{T}"/> and its associated <see cref="BrowsableObjectInfo{T}.ItemsLoader"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo{T}.ItemsLoader"/> is running. OR The given items loader has already been added to a <see cref="BrowsableObjectInfo{T}"/>.</exception>
        ///// <exception cref="ArgumentNullException">value is null.</exception>
        //public new ArchiveItemInfoFactory Factory { get => (ArchiveItemInfoFactory)base.Factory; set => base.Factory = value; }

        /// <summary>
        /// The factory used to create the new <see cref="IArchiveItemInfo"/>s.
        /// </summary>
        public sealed override IArchiveItemInfoFactory ArchiveItemInfoFactory { get => Factory; set => Factory = (T) value; }

        //IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        private readonly DeepClone<ArchiveFileInfo?> _archiveFileInfoDelegate;

        /// <summary>
        /// The <see cref="SevenZip.ArchiveFileInfo"/> that this <see cref="IArchiveItemInfo"/> represents.
        /// </summary>
        public ArchiveFileInfo? ArchiveFileInfo { get; private set; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public override IShellObjectInfo ArchiveShellObject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{T}"/> class.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{T}"/> represent a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate) : this(path, fileType, archiveShellObject, archiveFileInfoDelegate, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo{T}"/> class using a custom factory for <see cref="ArchiveItemInfo{T}"/>s.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="IShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfoDelegate">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo{T}"/> represent a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        /// <param name="factory">The factory this <see cref="ArchiveItemInfo{T}"/> and associated <see cref="ArchiveLoader"/> use to create new instances of the <see cref="ArchiveItemInfo{T}"/> class.</param>
        public ArchiveItemInfo(string path, FileType fileType, IShellObjectInfo archiveShellObject, DeepClone<ArchiveFileInfo?> archiveFileInfoDelegate, IArchiveItemInfoFactory factory) : base(path, fileType, (T) ( factory ?? new ArchiveItemInfoFactory() ) ) 

        {

            if (fileType == FileType.SpecialFolder)

                // todo:

                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

            _archiveFileInfoDelegate = archiveFileInfoDelegate;

            ArchiveFileInfo = archiveFileInfoDelegate(null);

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

            #region Comments

            // ArchiveFileInfo? archiveParentFileInfo = null;

            // FileType _fileType = FileType.None;

            // SevenZipExtractor archiveExtractor = new SevenZipExtractor(archiveShellObject.Path);

            // System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

            //foreach (ArchiveFileInfo _archiveFileInfo in archiveFileData)

            //{

            //if (_archiveFileInfo.FileName.Substring(0, path.LastIndexOf(IO.Path.PathSeparator)) == parent)

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
        /// Loads the items of this <see cref="ArchiveItemInfo{T}"/> using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems((BrowsableObjectInfoLoader)new ArchiveLoader<IArchiveItemInfoProvider>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo{T}"/> asynchronously using custom worker behavior options.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the worker reports progress</param>
        /// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync((BrowsableObjectInfoLoader)new ArchiveLoader<IArchiveItemInfoProvider>(this, GetAllEnumFlags<FileTypes>(), workerReportsProgress, workerSupportsCancellation));

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="ArchiveItemInfo{T}"/>.
        /// </summary>
        /// <returns>the parent of this <see cref="ArchiveItemInfo{T}"/>.</returns>
        protected override IBrowsableObjectInfo GetParent() => Path.Length > ArchiveShellObject.Path.Length /*&& Path.Contains(IO.Path.PathSeparator)*/ ? Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator)), FileType.Folder, ArchiveShellObject, null/*archiveParentFileInfo.Value*/) : ArchiveShellObject;

        ///// <summary>
        ///// Currently not implemented.
        ///// </summary>
        ///// <param name="newValue"></param>
        //public override void Rename(string newValue) =>

        //    // string getNewPath() => System.IO.Path.GetDirectoryName(Path) + IO.Path.PathSeparator + newValue;

        //    //SevenZipCompressor a = new SevenZipCompressor();

        //    //Dictionary<int, string> dico = new Dictionary<int, string>();

        //    //dico.Add(ArchiveFileInfo.Index, ArchiveFileInfo.FileName);

        //    //a.ModifyArchive(ArchiveShellObject.Path, dico);

        //    // todo:

        //    throw new NotSupportedException("This feature is currently not supported for the content archive items.");

        // protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
        // {

        // base.OnDeepClone(browsableObjectInfo);

        //using (var sevenZipExtractor = new SevenZipExtractor(ArchiveShellObject.Path))

        //    browsableObjectInfo.ArchiveFileInfo = sevenZipExtractor.ArchiveFileData.FirstOrDefault(item => item.FileName == Path.Substring(ArchiveShellObject.Path.Length));

        // }

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo{T}"/>. The <see cref="BrowsableObjectInfo{T}.OnDeepClone(BrowsableObjectInfo{T}, bool?)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride(bool?)"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any, or to create new IDs.</param>
        protected override BrowsableObjectInfo<T> DeepCloneOverride(bool? preserveIds) => new ArchiveItemInfo<T>(Path, FileType, (IShellObjectInfo)ArchiveShellObject.DeepClone(preserveIds), _archiveFileInfoDelegate, (ArchiveItemInfoFactory)Factory.DeepClone(preserveIds));

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeOverride(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
        {

            base.DisposeOverride(disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

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

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsReconstruction => true; // True because of the ShellObjectInfo's ShellObject

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
