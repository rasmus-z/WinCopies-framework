using Microsoft.WindowsAPICodePack.Shell;

namespace WinCopies.IO
{

    /// <summary>
    /// A factory to create new <see cref="ShellObjectInfo"/>'s.
    /// </summary>
    public class ShellObjectInfoFactory : BrowsableObjectInfoFactory, IShellObjectInfoFactory
    {

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path) => new ShellObjectInfo(shellObject, path);

        /// <summary>
        /// Gets a new <see cref="ShellObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo"/>. <see cref="WinCopies.IO.SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo"/> is a casual file system item.</param>
        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(ShellObject shellObject, string path, FileType fileType, SpecialFolder specialFolder) => new ShellObjectInfo(shellObject, path, fileType, specialFolder);

    }

}
