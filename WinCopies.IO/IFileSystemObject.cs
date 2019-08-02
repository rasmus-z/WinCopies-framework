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
    public interface IFileSystemObject : IComparable<IFileSystemObject>

    {

        /// <summary>
        /// Gets the path of this <see cref="IFileSystemObject"/>.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the localized name of this <see cref="IFileSystemObject"/>.
        /// </summary>
        string LocalizedName { get; }

        /// <summary>
        /// Gets the name of this <see cref="IFileSystemObject"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="WinCopies.IO.FileType"/> of this <see cref="IFileSystemObject"/>.
        /// </summary>
        FileType FileType { get; }

    }
}
