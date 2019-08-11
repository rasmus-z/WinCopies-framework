using Microsoft.WindowsAPICodePack.Shell;
using System;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>'s.
    /// </summary>
    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        public override bool NeedsObjectsReconstruction => ArchiveItemInfoFactory?.NeedsObjectsReconstruction == true;

        ArchiveItemInfoFactory _archiveItemInfoFactory;

        ArchiveItemInfoFactory ArchiveItemInfoFactory { get => _archiveItemInfoFactory; set { ThrowOnInvalidPropertySet(Path); _archiveItemInfoFactory = value; } }

        IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        public ShellObjectInfoFactory() : this(false, null) { }

        public ShellObjectInfoFactory(bool useRecursively, ArchiveItemInfoFactory archiveItemInfoFactory) : base(useRecursively) => _archiveItemInfoFactory = archiveItemInfoFactory;

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, Func<ShellObject> shellObjectDelegate, ShellObject shellObject) => UseRecursively ? GetBrowsableObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, (ShellObjectInfoFactory)this.DeepClone(false), Path is ShellObjectInfo shellObjectInfo && shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)shellObjectInfo.ArchiveItemInfoFactory.DeepClone(false)
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(false) ?? new ArchiveItemInfoFactory()) : new ShellObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, new ShellObjectInfoFactory(), Path is ShellObjectInfo _shellObjectInfo && _shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)_shellObjectInfo.ArchiveItemInfoFactory.DeepClone(false)
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(false) ?? new ArchiveItemInfoFactory());

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, Func<ShellObject> shellObjectDelegate, ShellObject shellObject, ShellObjectInfoFactory factory, ArchiveItemInfoFactory archiveItemInfoFactory) => new ShellObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, factory, archiveItemInfoFactory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new ShellObjectInfoFactory(UseRecursively, (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(preserveIds));

    }

}
