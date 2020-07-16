/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.WindowsAPICodePack.PortableDevices;
using System;

namespace WinCopies.IO.ObjectModel
{
    /// <summary>
    /// Provides interoperability for interacting with browsable items.
    /// </summary>
    public interface IArchiveItemInfoProvider : IFileSystemObjectInfo
    {
        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        IShellObjectInfo ArchiveShellObject { get; }
    }

    /// <summary>
    /// The base class for <see cref="ArchiveItemInfoProvider"/>s objects.
    /// </summary>
    public abstract class ArchiveItemInfoProvider/*<TItems, TFactory>*/ : FileSystemObjectInfo/*<TItems, TFactory>*/, IArchiveItemInfoProvider // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo    where TFactory : IBrowsableObjectInfoFactory
    {
        public override FileType FileType { get; }

        /// <summary>
        /// The parent <see cref="IShellObjectInfo"/> of the current archive item.
        /// </summary>
        public abstract IShellObjectInfo ArchiveShellObject { get; }

        protected ArchiveItemInfoProvider(in string path, in FileType fileType) : this(path, fileType, null) { }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="ArchiveItemInfoProvider"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="ArchiveItemInfoProvider"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="ArchiveItemInfoProvider"/>.</param>
        /// <exception cref="InvalidOperationException">The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        protected ArchiveItemInfoProvider(in string path, in FileType fileType, ClientVersion? clientVersion) : base(path, clientVersion) => FileType = fileType;
    }
}
