using SevenZip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using WinCopies.Util;

using TsudaKageyu;
using Microsoft.WindowsAPICodePack.Shell;

namespace WinCopies.IO
{

    public interface IArchiveItemInfoProvider

    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

        ShellObjectInfo ArchiveShellObject { get; }

    }

    /// <summary>
    /// Provides info to interact with archive items.
    /// </summary>
    public class ArchiveItemInfo : BrowsableObjectInfo, IArchiveItemInfoProvider
    {

        public ShellObjectInfo ArchiveShellObject { get; } = null;

        public sealed override string LocalizedName => ArchiveShellObject.LocalizedName;

        public sealed override string Name => System.IO.Path.GetFileName(Path);

        private Icon TryGetIcon(System.Drawing.Size size)
        {

            if (System.IO.Path.HasExtension(Path))

                if (Path.EndsWith(".exe"))

                    return new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\imageres.dll")).GetIcon(11).Split()?.TryGetIcon(size, 32, true, true);

                else

                {

                    string fileType = Registry.GetFileTypeByExtension(System.IO.Path.GetExtension(Path));

                    Icon icon = null;

                    if (fileType != null)

                        icon = Registry.GetIconVariationsFromFileType(fileType)?.TryGetIcon(size, 32, true, true);

                    return icon ?? new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\SHELL32.dll")).GetIcon(FileType == FileType.Folder ? 3 : 0).Split()?.TryGetIcon(size, 32, true, true);

                }

            else

                return new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\SHELL32.dll")).GetIcon(FileType == FileType.Folder ? 3 : 0).Split()?.TryGetIcon(size, 32, true, true);

        }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public sealed override BitmapSource SmallBitmapSource
        {
            get
            {

                using (Icon icon = TryGetIcon(new System.Drawing.Size(16, 16)))

                    return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }

        }

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public sealed override BitmapSource MediumBitmapSource
        {
            get
            {

                using (Icon icon = TryGetIcon(new System.Drawing.Size(48, 48)))

                    return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }

        }

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public sealed override BitmapSource LargeBitmapSource
        {
            get
            {

                using (Icon icon = TryGetIcon(new System.Drawing.Size(128, 128)))

                    return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromWidthAndHeight(icon.Width, icon.Height));

            }

        }

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public sealed override BitmapSource ExtraLargeBitmapSource
        {
            get
            {

                using (Icon icon = TryGetIcon(new System.Drawing.Size(256, 256)))

                    return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            }

        }

        public sealed override bool IsBrowsable => FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.Archive;

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

        public ArchiveFileInfo? ArchiveFileInfo { get; }

        // public FileTypes FileType { get; } = FileTypes.None;

        //public BrowsableObjectInfoItemsLoader ItemsLoader
        //{

        //    get => ItemsLoader;

        //    set => ItemsLoader = (FolderLoader)value;

        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveItemInfo"/> class.
        /// </summary>
        /// <param name="archiveShellObject">The <see cref="ShellObjectInfo"/> that correspond to the root path of the archive</param>
        /// <param name="archiveFileInfo">The <see cref="SevenZip.ArchiveFileInfo"/> that correspond to this archive item in the archive. Note: leave this parameter null if this <see cref="ArchiveItemInfo"/> represent a folder that exists implicitly in the archive.</param>
        /// <param name="path">The full path to this archive item</param>
        /// <param name="fileType">The file type of this archive item</param>
        public ArchiveItemInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) : base(path, fileType)

        {

            if (fileType == FileType.SpecialFolder)

                // todo:

                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

            if (archiveFileInfo.HasValue && !path.EndsWith(archiveFileInfo.Value.FileName))

                // todo:

                throw new ArgumentException($"'{nameof(path)}' must end with '{nameof(archiveFileInfo.Value.FileName)}'");

            ArchiveShellObject = archiveShellObject;

#if DEBUG 

            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

#endif

            // Path = path;

            if (archiveFileInfo.HasValue)

                ArchiveFileInfo = archiveFileInfo.Value;

            Debug.WriteLine("path: " + path);

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

        public sealed override IBrowsableObjectInfo GetParent() => Path.Length > ArchiveShellObject.Path.Length /*&& Path.Contains("\\")*/ ? GetBrowsableObjectInfo(ArchiveShellObject, null/*archiveParentFileInfo.Value*/, Path.Substring(0, Path.LastIndexOf('\\')), FileType.Folder) : ArchiveShellObject;

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        public sealed override void LoadItems()

        {

            if (ItemsLoader == null)

                LoadItems(true, true, FileTypesFlags.All);

            else

                ItemsLoader.LoadItems();

        }

        public sealed override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes) => LoadItems(new ArchiveLoader(true, true, fileTypes));

        public sealed override void LoadItems(BrowsableObjectInfoItemsLoader browsableObjectInfoItemsLoader)

        {

            ItemsLoader = browsableObjectInfoItemsLoader;

            ItemsLoader.LoadItems();

        }

        public override void Dispose()
        {

            base.Dispose();

            ArchiveShellObject.Dispose();

        }

        public override string ToString() => System.IO.Path.GetFileName(Path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, fileType);

        public sealed override void Rename(string newValue) =>

            // string getNewPath() => System.IO.Path.GetDirectoryName(Path) + "\\" + newValue;

            //SevenZipCompressor a = new SevenZipCompressor();

            //Dictionary<int, string> dico = new Dictionary<int, string>();

            //dico.Add(ArchiveFileInfo.Index, ArchiveFileInfo.FileName);

            //a.ModifyArchive(ArchiveShellObject.Path, dico);

            // todo:

            throw new NotSupportedException("This feature is currently not supported for the content archive items.");

        public sealed override IBrowsableObjectInfo Clone() => GetBrowsableObjectInfo(new ShellObjectInfo(ArchiveShellObject.ShellObject, ArchiveShellObject.Path, ArchiveShellObject.FileType, ArchiveShellObject.SpecialFolder), ArchiveFileInfo, Path, FileType);
    }

}
