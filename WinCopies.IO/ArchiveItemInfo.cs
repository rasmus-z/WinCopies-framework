using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WinCopies.IO
{

    public interface IArchiveItemInfoProvider

    {

        IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType);

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

        public override bool IsBrowsable => FileType == FileTypes.Folder || FileType == FileTypes.Drive || FileType == FileTypes.Archive;

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

        public ArchiveItemInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType) : base(path, fileType)

        {

            if (fileType == FileTypes.SpecialFolder)

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

                FileTypes _fileType = FileTypes.None;

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

                            _fileType = FileTypes.Folder;

                        else

                            _fileType = FileTypes.File;

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

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo archiveFileInfo, string path, string archiveItemRelativePath, FileTypes fileType) =>

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
