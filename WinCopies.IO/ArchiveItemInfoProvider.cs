using System;
using System.Collections.Generic;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides interoperability for interacting with browsable items.
    /// </summary>
    public interface IArchiveItemInfoProvider : IBrowsableObjectInfo

    {

        // IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

        /// <summary>
        /// The factory that is used to create the new <see cref="IArchiveItemInfo"/>s.
        /// </summary>
        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        IShellObjectInfo ArchiveShellObject { get; }

    }

    /// <summary>
    /// The base class for <see cref="ArchiveItemInfoProvider"/>s objects.
    /// </summary>
    public abstract class ArchiveItemInfoProvider : BrowsableObjectInfo, IArchiveItemInfoProvider

    {

        //    IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// The factory that is used to create the new <see cref="IArchiveItemInfo"/>s.
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

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="ArchiveItemInfoProvider"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="ArchiveItemInfoProvider"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="ArchiveItemInfoProvider"/>.</param>
        /// <param name="factory">The factory for this <see cref="ArchiveItemInfoProvider"/>. This factory is used to create new <see cref="IArchiveItemInfoProvider"/>s from the current <see cref="ArchiveItemInfoProvider"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
        protected ArchiveItemInfoProvider(string path, FileType fileType, BrowsableObjectInfoFactory factory) : base(path, fileType, factory) { }

        //    protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
        //    {

        //        base.OnDeepClone(browsableObjectInfo);

        //        if (ArchiveItemInfoFactory.UseRecursively)

        //            (((ArchiveItemInfoProvider)browsableObjectInfo).ArchiveItemInfoFactory = ArchiveItemInfoFactory.Clone();

        //    }

    }

}
