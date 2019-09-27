using Microsoft.WindowsAPICodePack.Shell;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    // todo: create a read-only wrapper for shellobjectinfo

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>s.
    /// </summary>
    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsOrValuesReconstruction => ArchiveItemInfoFactory?.NeedsObjectsOrValuesReconstruction == true;

        public IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        // IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfoFactory"/> class.
        /// </summary>
        public ShellObjectInfoFactory() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="archiveItemInfoFactory">The <see cref="ArchiveItemInfoFactory"/> this factory will use for creating new items.</param>
        public ShellObjectInfoFactory(ArchiveItemInfoFactory archiveItemInfoFactory) : base() => ArchiveItemInfoFactory = archiveItemInfoFactory;

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) => new ShellObjectInfo(path, fileType, specialFolder, shellObject, shellObjectDelegate);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ShellObjectInfoFactory((ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone());

    }

}
