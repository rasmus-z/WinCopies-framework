using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{
    /// <summary>
    /// A factory to create new <see cref="IBrowsableObjectInfo"/>s.
    /// </summary>
    public interface IShellObjectInfoFactory : IBrowsableObjectInfoFactory
    {

        IArchiveItemInfoFactory ArchiveItemInfoFactory { get; }

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the given <see cref="ShellObject"/> and path.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{T}"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{T}"/>.</param>
        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject);

        /// <summary>
        /// Gets a new <see cref="IBrowsableObjectInfo"/> that represents the given <see cref="ShellObject"/>, path, <see cref="FileType"/> and <see cref="SpecialFolder"/>.
        /// </summary>
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo{T}"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo{T}"/>.</param>
        /// <param name="fileType">The file type of this <see cref="ShellObjectInfo{T}"/>.</param>
        /// <param name="specialFolder">The special folder type of this <see cref="ShellObjectInfo{T}"/>. <see cref="SpecialFolder.OtherFolderOrFile"/> if this <see cref="ShellObjectInfo{T}"/> is a casual file system item.</param>
        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, DeepClone<ShellObject> shellObjectDelegate, ShellObject shellObject, IShellObjectInfoFactory factory, IArchiveItemInfoFactory archiveItemInfoFactory);

    }
}
