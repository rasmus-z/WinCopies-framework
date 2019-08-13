using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public abstract class FileSystemObject : IFileSystemObject
    {

        /// <summary>
        /// Gets the path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual string Path { get; }

        /// <summary>
        /// When overridden in a derived class, gets the localized path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string LocalizedName { get; }

        /// <summary>
        /// When overridden in a derived class, gets the name of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The file type of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public virtual FileType FileType { get; private set; } = FileType.Other;

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="FileSystemObject"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="FileSystemObject"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="FileSystemObject"/>.</param>
        protected FileSystemObject(string path, FileType fileType)

        {

            Path = path;

            FileType = fileType;

        }

        public virtual bool Equals(IFileSystemObject fileSystemObject) => Equals(fileSystemObject as object);

        /// <summary>
        /// Determines whether the specified object is equal to the current object by testing the following things, in order: whether <paramref name="obj"/> implements the <see cref="IBrowsableObjectInfo"/> interface and both objects references, and <see cref="FileType"/> and <see cref="Path"/> properties are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => ReferenceEquals(this, obj)
                ? true : obj is IFileSystemObject _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
                : false;

        /// <summary>
        /// Gets an hash code for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The hash codes of the <see cref="FileType"/> and the <see cref="Path"/> property.</returns>
        public override int GetHashCode() => FileType.GetHashCode() ^ Path.ToLower().GetHashCode();

        /// <summary>
        /// Gets a string representation of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The <see cref="LocalizedName"/> of this <see cref="BrowsableObjectInfo"/>.</returns>
        public override string ToString() => IsNullEmptyOrWhiteSpace(LocalizedName) ? Path : LocalizedName;

        public static bool operator ==(FileSystemObject left, FileSystemObject right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(FileSystemObject left, FileSystemObject right) => !(left == right);

        public static bool operator <(FileSystemObject left, FileSystemObject right) => left is null ? right is object : left.CompareTo(right) < 0;

        public static bool operator <=(FileSystemObject left, FileSystemObject right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(FileSystemObject left, FileSystemObject right) => left is object && left.CompareTo(right) > 0;

        public static bool operator >=(FileSystemObject left, FileSystemObject right) => left is null ? right is null : left.CompareTo(right) >= 0;

        public static FileSystemObjectComparer<IFileSystemObject> GetDefaultComparer() => new FileSystemObjectComparer<IFileSystemObject>();

        public virtual int CompareTo(IFileSystemObject fileSystemObject) => GetDefaultComparer().Compare(this, fileSystemObject);

    }
}
