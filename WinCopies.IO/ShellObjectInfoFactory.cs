using Microsoft.WindowsAPICodePack.Shell;
using System;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/>s.
    /// </summary>
    public class ShellObjectInfoFactory<TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems> : BrowsableObjectInfoFactory, IShellObjectInfoFactory where TParent : class, IShellObjectInfo where TItems : class, IFileSystemObjectInfo where TParentArchiveItemInfo : class, IArchiveItemInfoProvider where TArchiveItemInfoItems : class, IArchiveItemInfo
    {

        /// <summary>
        /// Gets a value that indicates whether this object needs to reconstruct objects on deep cloning.
        /// </summary>
        public override bool NeedsObjectsOrValuesReconstruction => ArchiveItemInfoFactory?.NeedsObjectsOrValuesReconstruction == true;

        ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems> _archiveItemInfoFactory;

        ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems> ArchiveItemInfoFactory { get => _archiveItemInfoFactory; set { ThrowOnInvalidPropertySet(Path); _archiveItemInfoFactory = value; } }

        IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellObjectInfoFactory{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems}"/> class and sets the <see cref="BrowsableObjectInfoFactory.UseRecursively"/> property to <see langword="true"/>.
        /// </summary>
        public ShellObjectInfoFactory() : this(null, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoFactory"/> class.
        /// </summary>
        /// <param name="archiveItemInfoFactory">The <see cref="ArchiveItemInfoFactory{TParent, TItems}"/> this factory will use for creating new items.</param>
        /// <param name="useRecursively">Whether to add a clone of the new <see cref="BrowsableObjectInfoFactory"/> to all the new objects created from the new <see cref="ShellObjectInfoFactory{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems}"/>.</param>
        public ShellObjectInfoFactory(ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems> archiveItemInfoFactory, bool useRecursively) : base(useRecursively) => _archiveItemInfoFactory = archiveItemInfoFactory;

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/> represents.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject) => UseRecursively ? GetBrowsableObjectInfo(path, fileType, specialFolder, shellObjectDelegate, shellObject, (ShellObjectInfoFactory<TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems>)this.DeepClone(), Path is IShellObjectInfo shellObjectInfo && shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>)shellObjectInfo.ArchiveItemInfoFactory.DeepClone()
                : (ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone() ?? new ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>()) : new ShellObjectInfo< TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, IShellObjectInfoFactory>(path, fileType, specialFolder, shellObjectDelegate, shellObject, new ShellObjectInfoFactory<TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems>(), Path is IShellObjectInfo _shellObjectInfo && _shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>)_shellObjectInfo.ArchiveItemInfoFactory.DeepClone()
                : (ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone() ?? new ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>());

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/>.</param>
        /// <param name="fileType">The file type of the new item.</param>
        /// <param name="specialFolder">The special folder type of the new item.</param>
        /// <param name="shellObjectDelegate">The delegate that will be used by this factory and the new item's <see cref="IDeepCloneable.DeepClone"/> method for creating new items.</param>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, TFactory}"/> represents. Leave this parameter <see langword="null"/> to use <paramref name="shellObjectDelegate"/> instead.</param>
        /// <param name="factory">A custom factory.</param>
        /// <param name="archiveItemInfoFactory">A custom factory for creating new <see cref="IArchiveItemInfo"/> items.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject, IShellObjectInfoFactory factory, IArchiveItemInfoFactory archiveItemInfoFactory) => new ShellObjectInfo< TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems, IShellObjectInfoFactory>(path, fileType, specialFolder, shellObjectDelegate, shellObject, factory, archiveItemInfoFactory);

        protected override BrowsableObjectInfoFactory DeepCloneOverride() => new ShellObjectInfoFactory<TParent, TItems, TParentArchiveItemInfo, TArchiveItemInfoItems>((ArchiveItemInfoFactory<TParentArchiveItemInfo, TArchiveItemInfoItems>)ArchiveItemInfoFactory?.DeepClone(), UseRecursively);

    }

}
