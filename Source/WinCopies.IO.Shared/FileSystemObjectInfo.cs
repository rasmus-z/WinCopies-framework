﻿/* Copyright © Pierre Sprimont, 2020
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

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using TsudaKageyu;

using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    // public interface IFileSystemObjectInfoFactory : IBrowsableObjectInfoFactory { }

    public interface IFileSystemObjectInfo : IBrowsableObjectInfo
    {
        /// <summary>
        /// Gets the <see cref="WinCopies.IO.FileType"/> of this <see cref="IFileSystemObject"/>.
        /// </summary>
        FileType FileType { get; }
    }

    public abstract class FileSystemObjectInfo/*<TItems, TFactory>*/ : BrowsableObjectInfo/*<TItems, TFactory>*/, IFileSystemObjectInfo // where TItems : BrowsableObjectInfo, IFileSystemObjectInfo where TFactory : BrowsableObjectInfoFactory
    {
        /// <summary>
        /// Gets a default comparer for <see cref="FileSystemObjectInfo"/>s.
        /// </summary>
        /// <returns>A default comparer for <see cref="FileSystemObjectInfo"/>s.</returns>
        public static FileSystemObjectInfoComparer<IFileSystemObjectInfo> GetDefaultComparer() => new FileSystemObjectInfoComparer<IFileSystemObjectInfo>();

        /// <summary>
        /// Compares the current object to a given <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <param name="fileSystemObjectInfo">The <see cref="FileSystemObjectInfo"/> to compare with.</param>
        /// <returns>The comparison result. See <see cref="IComparable{T}.CompareTo(T)"/> for more details.</returns>
        public virtual int CompareTo(IFileSystemObjectInfo fileSystemObjectInfo) => GetDefaultComparer().Compare(this, fileSystemObjectInfo);

        /// <summary>
        /// Determines whether the specified <see cref="IFileSystemObjectInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
        /// </summary>
        /// <param name="fileSystemObjectInfo">The <see cref="IFileSystemObjectInfo"/> to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public virtual bool Equals(IFileSystemObjectInfo fileSystemObjectInfo) => Equals(fileSystemObjectInfo as object);

        /// <summary>
        /// Determines whether the specified object is equal to the current object by testing the following things, in order: whether both objects's references are equal, <paramref name="obj"/> implements the <see cref="IFileSystemObjectInfo"/> interface and <see cref="FileType"/> and <see cref="FileSystemObject.Path"/> properties are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is null ? false : ReferenceEquals(this, obj)
                ? true : obj is IFileSystemObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower() // TODO: make better string comparison
                : false;

        /// <summary>
        /// Gets an hash code for this <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <returns>The hash code of the <see cref="FileType"/> and the <see cref="FileSystemObject.Path"/> property.</returns>
        public override int GetHashCode() => FileType.GetHashCode() ^ Path.ToLower().GetHashCode();

        /// <summary>
        /// Gets a string representation of this <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="FileSystemObject.LocalizedName"/> of this <see cref="FileSystemObjectInfo"/>.</returns>
        public override string ToString() => IsNullEmptyOrWhiteSpace(LocalizedName) ? Path : LocalizedName;

        /// <summary>
        /// Checks if two <see cref="FileSystemObjectInfo"/>s are equal.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the two <see cref="FileSystemObjectInfo"/>s are equal.</returns>
        public static bool operator ==(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => left is null ? right is null : left.Equals(right);

        /// <summary>
        /// Checks if two <see cref="FileSystemObjectInfo"/>s are different.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the two <see cref="FileSystemObjectInfo"/>s are different.</returns>
        public static bool operator !=(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => !(left == right);

        /// <summary>
        /// Checks if a given <see cref="FileSystemObjectInfo"/> is lesser than an other <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the given <see cref="FileSystemObjectInfo"/> is lesser than the <see cref="FileSystemObjectInfo"/> to compare with.</returns>
        public static bool operator <(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => left is null ? right is object : left.CompareTo(right) < 0;

        /// <summary>
        /// Checks if a given <see cref="FileSystemObjectInfo"/> is lesser or equal to an other <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the given <see cref="FileSystemObjectInfo"/> is lesser or equal to the <see cref="FileSystemObjectInfo"/> to compare with.</returns>
        public static bool operator <=(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => left is null || left.CompareTo(right) <= 0;

        /// <summary>
        /// Checks if a given <see cref="FileSystemObjectInfo"/> is greater than an other <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the given <see cref="FileSystemObjectInfo"/> is greater than the <see cref="FileSystemObjectInfo"/> to compare with.</returns>
        public static bool operator >(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => left is object && left.CompareTo(right) > 0;

        /// <summary>
        /// Checks if a given <see cref="FileSystemObjectInfo"/> is greater or equal to an other <see cref="FileSystemObjectInfo"/>.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the given <see cref="FileSystemObjectInfo"/> is greater or equal to the <see cref="FileSystemObjectInfo"/> to compare with.</returns>
        public static bool operator >=(in FileSystemObjectInfo left, in FileSystemObjectInfo right) => left is null ? right is null : left.CompareTo(right) >= 0;

        /// <summary>
        /// The file type of this <see cref="FileSystemObject"/>.
        /// </summary>
        public abstract FileType FileType { get; }

        public static string GetItemTypeName(string extension, FileType fileType) => fileType == FileType.Folder
                    ? FileOperation.GetFileInfo("", Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Directory, GetFileInfoOptions.TypeName).TypeName
                    : FileOperation.GetFileInfo(extension, Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Normal, GetFileInfoOptions.TypeName).TypeName;

        private static Icon TryGetIcon(in int index, in System.Drawing.Size size) => TryGetIcon(index, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, size);

        public static Icon TryGetIcon(in string extension, in FileType fileType, in System.Drawing.Size size) =>

           // if (System.IO.Path.HasExtension(Path))

           fileType == FileType.Folder ? TryGetIcon(3, size) : Microsoft.WindowsAPICodePack.Shell.FileOperation.GetFileInfo(extension, Microsoft.WindowsAPICodePack.Win32Native.Shell.FileAttributes.Normal, Microsoft.WindowsAPICodePack.Win32Native.Shell.GetFileInfoOptions.Icon | Microsoft.WindowsAPICodePack.Win32Native.Shell.GetFileInfoOptions.UseFileAttributes).Icon?.TryGetIcon(size, 32, true, true) ?? TryGetIcon(0, size);// else// return TryGetIcon(FileType == FileType.Folder ? 3 : 0, "SHELL32.dll", size);

        public Icon TryGetIcon(in int size) => TryGetIcon(System.IO.Path.GetExtension(Path), FileType, new System.Drawing.Size(size, size));

        public static BitmapSource TryGetBitmapSource(in string extension, in FileType fileType, in System.Drawing.Size size)
        {
#if NETFRAMEWORK

            using (Icon icon = TryGetIcon(extension, fileType, size))

#else

            using Icon icon = TryGetIcon(extension, fileType, size);

#endif

            return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public BitmapSource TryGetBitmapSource(in int size) => TryGetBitmapSource(System.IO.Path.GetExtension(Path), FileType, new System.Drawing.Size(size, size));

        // /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
        protected FileSystemObjectInfo(in string path) : base(path) { }
    }
}
