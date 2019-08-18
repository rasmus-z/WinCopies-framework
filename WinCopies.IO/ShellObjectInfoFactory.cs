using Microsoft.WindowsAPICodePack.Shell;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>s.
    /// </summary>
    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsReconstruction => ArchiveItemInfoFactory?.NeedsObjectsReconstruction == true;

        ArchiveItemInfoFactory _archiveItemInfoFactory;

        ArchiveItemInfoFactory ArchiveItemInfoFactory { get => _archiveItemInfoFactory; set { ThrowOnInvalidPropertySet(Path); _archiveItemInfoFactory = value; } }

        IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfoFactory"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ShellObjectInfoFactory() : this(null, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="archiveItemInfoFactory">The <see cref="IO.ArchiveItemInfoFactory"/> this factory will use for creating new items.</param>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="ShellObjectInfoFactory"/>.</param>
        public ShellObjectInfoFactory(ArchiveItemInfoFactory archiveItemInfoFactory, bool useRecursively) : base(useRecursively) => _archiveItemInfoFactory = archiveItemInfoFactory;

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by this factory and the new item's <see cref="IDeepCloneable.DeepClone(bool)"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents. Leave this parameter <see langword="null"/> to use <paramref name="shellObjectDelegate"/> instead.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, Func<ShellObject> shellObjectDelegate, ShellObject shellObject) => UseRecursively ? GetBrowsableObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, (ShellObjectInfoFactory)this.DeepClone(false), Path is ShellObjectInfo shellObjectInfo && shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)shellObjectInfo.ArchiveItemInfoFactory.DeepClone(false)
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(false) ?? new ArchiveItemInfoFactory()) : new ShellObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, new ShellObjectInfoFactory(), Path is ShellObjectInfo _shellObjectInfo && _shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)_shellObjectInfo.ArchiveItemInfoFactory.DeepClone(false)
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(false) ?? new ArchiveItemInfoFactory());

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by this factory and the new item's <see cref="IDeepCloneable.DeepClone(bool)"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents. Leave this parameter <see langword="null"/> to use <paramref name="shellObjectDelegate"/> instead.</param>
        /// <param name="factory">A custom factory.</param>
        /// <param name="archiveItemInfoFactory">A custom factory for creating new <see cref="IArchiveItemInfo"/> items.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, Func<ShellObject> shellObjectDelegate, ShellObject shellObject, ShellObjectInfoFactory factory, ArchiveItemInfoFactory archiveItemInfoFactory) => new ShellObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, factory, archiveItemInfoFactory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride(bool preserveIds) => new ShellObjectInfoFactory((ArchiveItemInfoFactory)ArchiveItemInfoFactory?.DeepClone(preserveIds), UseRecursively);

    }

}
