using Microsoft.WindowsAPICodePack.Shell;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    // todo: create a read-only wrapper for shellobjectinfo

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>s.
    /// </summary>
    public class ShellObjectInfoFactory<TItems, TArchiveItemInfoItems> : BrowsableObjectInfoFactory, IShellObjectInfoFactory where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TArchiveItemInfoItems : BrowsableObjectInfo, IArchiveItemInfo
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsOrValuesReconstruction => ArchiveItemInfoFactory?.NeedsObjectsOrValuesReconstruction == true;

        public IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        // IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfoFactory{TItems, TArchiveItemInfoItems}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ShellObjectInfoFactory() : this(null, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="archiveItemInfoFactory">The <see cref="ArchiveItemInfoFactory{TItems}"/> this factory will use for creating new items.</param>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="ShellObjectInfoFactory{TItems, TArchiveItemInfoItems}"/>.</param>
        public ShellObjectInfoFactory(ArchiveItemInfoFactory<TArchiveItemInfoItems> archiveItemInfoFactory, bool useRecursively) : base(useRecursively) => ArchiveItemInfoFactory = archiveItemInfoFactory;

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate) => UseRecursively ? GetBrowsableObjectInfo(path, fileType, specialFolder, shellObject, shellObjectDelegate, (ShellObjectInfoFactory<TItems, TArchiveItemInfoItems>)this.DeepClone(), ArchiveItemInfoFactory?.UseRecursively == true
                ? (ArchiveItemInfoFactory<TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone() : new ArchiveItemInfoFactory<TArchiveItemInfoItems>()) : new ShellObjectInfo<TItems, TArchiveItemInfoItems, IShellObjectInfoFactory>(path, fileType, specialFolder, shellObject, shellObjectDelegate, new ShellObjectInfoFactory<TItems, TArchiveItemInfoItems>(), ArchiveItemInfoFactory?.UseRecursively == true
                ? (ArchiveItemInfoFactory<TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone() : new ArchiveItemInfoFactory<TArchiveItemInfoItems>());

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by the <see cref="BrowsableObjectInfo.DeepClone()"/> method by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{TItems, TArchiveItemInfoItems, TFactory}"/> represents. Leave this parameter <see langword="null"/> to use <paramref name="shellObjectDelegate"/> instead.</param>
        /// <param name="factory">A custom factory.</param>
        /// <param name="archiveItemInfoFactory">A custom factory for creating new <see cref="IArchiveItemInfo"/> items.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate, IShellObjectInfoFactory factory, IArchiveItemInfoFactory archiveItemInfoFactory) => new ShellObjectInfo<TItems, TArchiveItemInfoItems, IShellObjectInfoFactory>(path, fileType, specialFolder, shellObject, shellObjectDelegate, factory, archiveItemInfoFactory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ShellObjectInfoFactory<TItems, TArchiveItemInfoItems>((ArchiveItemInfoFactory<TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone(), UseRecursively);

    }

}
