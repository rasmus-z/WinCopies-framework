using SevenZip;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IArchiveItemInfoProvider

    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileType fileType);

        ShellObjectInfo ArchiveShellObject { get; }

    }

    /// <summary>
    /// Provides info to interact with archive items.
    /// </summary>
    public class ArchiveItemInfo : BrowsableObjectInfo, IArchiveItemInfoProvider
    {

        public ShellObjectInfo ArchiveShellObject { get; } = null;

        public override string LocalizedPath => ArchiveShellObject.LocalizedPath;

        public override string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => Imaging.CreateBitmapSourceFromHIcon(WinCopies.Win32Interop.Icon.Icon.getFileIcon(".txt", Win32Interop.Icon.IconSize.Small, true, true).Ptr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => Imaging.CreateBitmapSourceFromHIcon(WinCopies.Win32Interop.Icon.Icon.getFileIcon(".txt", Win32Interop.Icon.IconSize.Medium, true, true).Ptr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource { get

            {

                IntPtr ptr = WinCopies.Win32Interop.Icon.Icon.getFileIcon(".txt", Win32Interop.Icon.IconSize.Large, true, true).Ptr;

                Icon icon = Icon.FromHandle(ptr);

               return Imaging.CreateBitmapSourceFromHIcon( ptr, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());

            }

        }

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="ArchiveItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => Imaging.CreateBitmapSourceFromHIcon(WinCopies.Win32Interop.Icon.Icon.getFileIcon(".txt", Win32Interop.Icon.IconSize.ExtraLarge, true, true).Ptr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        public override bool IsBrowsable => FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.Archive;

        public string ArchiveItemRelativePath { get; } = null;

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

        public ArchiveFileInfo ArchiveFileInfo { get; }

        public override IBrowsableObjectInfo Parent { get; protected set; } = null;

        // public FileTypes FileType { get; } = FileTypes.None;

        //public BrowsableObjectInfoItemsLoader ItemsLoader
        //{

        //    get => ItemsLoader;

        //    set => ItemsLoader = (LoadFolder)value;

        //}

        public ArchiveItemInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileType fileType) : base(path, fileType)

        {
            
            if (fileType == FileType.SpecialFolder)

                throw new ArgumentException("'fileType' can't be a SpecialFolder.");

            ArchiveShellObject = archiveShellObject;

#if DEBUG 

            Debug.WriteLine("shellObject == null: " + (archiveShellObject == null).ToString());

#endif

            // Path = path;

            ArchiveItemRelativePath = archiveItemRelativePath;

            if (path.Contains("\\"))

            {

                string parent = path.Substring(0, path.LastIndexOf('\\'));

                ArchiveFileInfo? archiveParentFileInfo = null;

                FileType _fileType = FileType.None;

                SevenZipExtractor archiveExtractor = new SevenZipExtractor(archiveFileInfo.FileName);

                System.Collections.ObjectModel.ReadOnlyCollection<ArchiveFileInfo> archiveFileData = archiveExtractor.ArchiveFileData;

                foreach (ArchiveFileInfo _archiveFileInfo in archiveFileData)

                {

#if DEBUG 

                    Debug.WriteLine("_archiveFileInfo.FileName: " + _archiveFileInfo.FileName);

#endif 

                    if (_archiveFileInfo.FileName == parent)

                    {

                        archiveParentFileInfo = _archiveFileInfo;

                        if (_archiveFileInfo.IsDirectory)

                            _fileType = FileType.Folder;

                        else

                            _fileType = FileType.File;

                    }

                }

                Parent = GetBrowsableObjectInfo(archiveShellObject, archiveParentFileInfo.Value, path, parent, _fileType);

            }

            else

                Parent = archiveShellObject;

        }

        // public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo) => browsableObjectInfo;

        public override void LoadItems()

        {

            if (ItemsLoader == null)

                LoadItems(true, true, FileTypesFlags.All);

            else

                ItemsLoader.LoadItems();

        }

        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes) => LoadItems(new LoadArchive(true, true, fileTypes));

        public override void LoadItems(BrowsableObjectInfoItemsLoader browsableObjectInfoItemsLoader)

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

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileType fileType) =>

            new ArchiveItemInfo(archiveShellObject, archiveFileInfo, path, archiveItemRelativePath, fileType);

        public override void Rename(string newValue)

        {

            // string getNewPath() => System.IO.Path.GetDirectoryName(Path) + "\\" + newValue;

            SevenZipCompressor a = new SevenZipCompressor();

            Dictionary<int, string> dico = new Dictionary<int, string>();

            dico.Add(ArchiveFileInfo.Index, ArchiveFileInfo.FileName);

            a.ModifyArchive(ArchiveShellObject.Path, dico);

        }
    }

}
