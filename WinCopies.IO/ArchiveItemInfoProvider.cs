﻿using System;
using System.Collections.Generic;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides interoperability for interacting with browsable items.
    /// </summary>
    public interface IArchiveItemInfoProvider : IFileSystemObjectInfo

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

    //public interface IArchiveItemInfoProvider<T> : IFileSystemObjectInfo<T>, IArchiveItemInfoProvider where T : IFileSystemObjectInfoFactory

    //{



    //}

    /// <summary>
    /// The base class for <see cref="ArchiveItemInfoProvider"/>s objects.
    /// </summary>
    public abstract class ArchiveItemInfoProvider/*<TItems, TFactory>*/ : FileSystemObjectInfo/*<TItems, TFactory>*/, IArchiveItemInfoProvider // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo    where TFactory : IBrowsableObjectInfoFactory

    {

        //    IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// The factory that is used to create the new <see cref="IArchiveItemInfo"/>s.
        /// </summary>
        public abstract IArchiveItemInfoFactory ArchiveItemInfoFactory { get; set; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public abstract IShellObjectInfo ArchiveShellObject { get; }

        //    IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        //    public ArchiveItemInfoProvider(string path, FileType fileType) : this(path, fileType) { }

        //    public ArchiveItemInfoProvider(string path, FileType fileType, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType, ) { }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="ArchiveItemInfoProvider{TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="ArchiveItemInfoProvider{TItems, TFactory}"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="ArchiveItemInfoProvider{TItems, TFactory}"/>.</param>
        /// <param name="factory">The factory for this <see cref="ArchiveItemInfoProvider{TItems, TFactory}"/>. This factory is used to create new <see cref="IArchiveItemInfoProvider"/>s from the current <see cref="ArchiveItemInfoProvider{TItems, TFactory}"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
        /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo{TItems, TFactory}"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        protected ArchiveItemInfoProvider(string path, FileType fileType, TFactory factory) : base(path, fileType, factory) { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            ArchiveShellObject.Dispose();
        }

        //    protected override void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)
        //    {

        //        base.OnDeepClone(browsableObjectInfo);

        //        if (ArchiveItemInfoFactory.UseRecursively)

        //            (((ArchiveItemInfoProvider)browsableObjectInfo).ArchiveItemInfoFactory = ArchiveItemInfoFactory.Clone();

        //    }

    }

}
