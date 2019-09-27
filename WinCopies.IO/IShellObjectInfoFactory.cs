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
        /// <param name="shellObject">The <see cref="ShellObject"/> that this <see cref="ShellObjectInfo"/> represents.</param>
        /// <param name="path">The path of this <see cref="ShellObjectInfo"/>.</param>
        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileType fileType, SpecialFolder specialFolder, ShellObject shellObject, DeepClone<ShellObject> shellObjectDelegate);

    }
}
