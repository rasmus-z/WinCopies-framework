using System;
using System.Collections.Generic;

namespace WinCopies.IO
{
    public interface IArchiveItemInfoProvider : IBrowsableObjectInfo

    {

        // IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObjectInfo archiveShellObject, ArchiveFileInfo? archiveFileInfo, string path, FileType fileType);

        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        IShellObjectInfo ArchiveShellObject { get; }

    }

    public abstract class ArchiveItemInfoProvider : BrowsableObjectInfo, IArchiveItemInfoProvider

    {

        private ArchiveItemInfoFactory _archiveItemInfoFactory;

        IArchiveItemInfoFactory IArchiveItemInfoProvider.ArchiveItemInfoFactory => _archiveItemInfoFactory;

        /// <summary>
        /// Gets or sets the factory this <see cref="ShellObjectInfo"/> and associated <see cref="FolderLoader"/>'s and <see cref="ArchiveLoader"/>'s use to create new objects that represent archive items.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy.</exception>
        /// <exception cref="ArgumentNullException">The given value is null.</exception>
        public ArchiveItemInfoFactory ArchiveItemInfoFactory
        {
            get => _archiveItemInfoFactory; set
            {

                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

                _archiveItemInfoFactory.Path = null;

                value.Path = this;

                _archiveItemInfoFactory = value;

            }
        }

        protected abstract IShellObjectInfo ArchiveShellObjectOverride { get; }

        IShellObjectInfo IArchiveItemInfoProvider.ArchiveShellObject => ArchiveShellObjectOverride;

        public ArchiveItemInfoProvider(string path, FileType fileType) : this(path, fileType, new ArchiveItemInfoFactory()) { }

        public ArchiveItemInfoProvider(string path, FileType fileType, ArchiveItemInfoFactory archiveItemInfoFactory) : base(path, fileType) => _archiveItemInfoFactory = archiveItemInfoFactory;

    }
}
