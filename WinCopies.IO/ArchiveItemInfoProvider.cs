using System;
using System.Collections.Generic;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoProvider : IBrowsableObjectInfo

    {

        // IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

            /// <summary>
            /// The factory used to create the new <see cref="IArchiveItemInfo"/>s.
            /// </summary>
        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        IShellObjectInfo ArchiveShellObject { get; }

    }

    public abstract class ArchiveItemInfoProvider : BrowsableObjectInfo, IArchiveItemInfoProvider

    {

        //    IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// The factory used to create the new <see cref="IArchiveItemInfo"/>s.
        /// </summary>
        public abstract ArchiveItemInfoFactory ArchiveItemInfoFactory { get; set; }

        IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public abstract IShellObjectInfo ArchiveShellObject { get; }

        //    IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        //    public ArchiveItemInfoProvider(string path, FileType fileType) : this(path, fileType) { }

        //    public ArchiveItemInfoProvider(string path, FileType fileType, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, ) { }

        protected ArchiveItemInfoProvider(string path, FileType fileType, BrowsableObjectInfoFactory factory) : base(path, fileType, factory) { }

        //    protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
        //    {

        //        base.OnDeepClone(browsableObjectInfo);

        //        if (ArchiveItemInfoFactory.UseRecursively)

        //            (((ArchiveItemInfoProvider)browsableObjectInfo).ArchiveItemInfoFactory = ArchiveItemInfoFactory.Clone();

        //    }

    }
}
