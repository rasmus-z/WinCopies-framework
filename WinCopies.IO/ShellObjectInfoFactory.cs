using Microsoft.WindowsAPICodePack.Shell;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>'s.
    /// </summary>
    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        ArchiveItemInfoFactory _archiveItemInfoFactory;

        ArchiveItemInfoFactory ArchiveItemInfoFactory { get => _archiveItemInfoFactory; set { ThrowOnInvalidPropertySet(); _archiveItemInfoFactory = value; } }

        IArchiveItemInfoFactory IShellObjectInfoFactory.ArchiveItemInfoFactory => ArchiveItemInfoFactory;

        public ShellObjectInfoFactory() : this(false) { }

        public ShellObjectInfoFactory(bool useRecursively) : base(useRecursively) { }

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) => UseRecursively ? GetBrowsableObjectInfo(shellObject, path, fileType, specialFolder, (ShellObjectInfoFactory)this.Clone(), Path is ShellObjectInfo shellObjectInfo && shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)shellObjectInfo.ArchiveItemInfoFactory.Clone()
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.Clone() ?? new ArchiveItemInfoFactory()) : new ShellObjectInfo(shellObject, path, fileType, specialFolder, new ShellObjectInfoFactory(), Path is ShellObjectInfo _shellObjectInfo && _shellObjectInfo.ArchiveItemInfoFactory.UseRecursively == true
                ? (ArchiveItemInfoFactory)_shellObjectInfo.ArchiveItemInfoFactory.Clone()
                : (ArchiveItemInfoFactory)ArchiveItemInfoFactory?.Clone() ?? new ArchiveItemInfoFactory());

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder, ShellObjectInfoFactory factory, ArchiveItemInfoFactory archiveItemInfoFactory) => new ShellObjectInfo(shellObject, path, fileType, specialFolder, factory, archiveItemInfoFactory);

        public override object Clone()
        {

            var factory = (ShellObjectInfoFactory)base.Clone();

            factory._archiveItemInfoFactory = (ArchiveItemInfoFactory)factory.ArchiveItemInfoFactory.Clone();

            return factory;

        }
    }

}
