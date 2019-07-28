using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    /// <summary>
    /// Provides data about file system items.
    /// </summary>
    public interface IFileSystemObject

    {

        /// <summary>
        /// Gets the path of this <see cref="IFileSystemObject"/>.
        /// </summary>
        string Path { get; }

        string LocalizedName { get; }

        string Name { get; }

        /// <summary>
        /// Gets the file type of this <see cref="IFileSystemObject"/>.
        /// </summary>
        FileType FileType { get; }

    }
}
