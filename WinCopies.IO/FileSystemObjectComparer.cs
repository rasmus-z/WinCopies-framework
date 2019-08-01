using System;
using System.Collections.Generic;
using System.Globalization;
using WinCopies.Util;

namespace WinCopies.IO
{
    public class FileSystemObjectComparer : Comparer<IFileSystemObject>

    {

        public StringComparer StringComparer { get; set; }

        public FileSystemObjectComparer() : this(StringComparer.Create(CultureInfo.CurrentCulture, true)) { }

        public FileSystemObjectComparer(StringComparer stringComparer) => StringComparer = stringComparer;

        public override int Compare(IFileSystemObject x, IFileSystemObject y) => x.FileType == y.FileType || (x.FileType == FileType.File && (y.FileType == FileType.Link || y.FileType == FileType.Archive)) || (y.FileType == FileType.File && (x.FileType == FileType.Link || x.FileType == FileType.Archive))
                ? StringComparer.Compare(x.LocalizedName.RemoveAccents(), y.LocalizedName.RemoveAccents())
                : (x.FileType == FileType.Folder || x.FileType == FileType.Drive) && (y.FileType == FileType.File || y.FileType == FileType.Archive || y.FileType == FileType.Link)
                ? -1
                : (x.FileType == FileType.File || x.FileType == FileType.Archive || x.FileType == FileType.Link) && (y.FileType == FileType.Folder || y.FileType == FileType.Drive)
                ? 1
                : 0;

    }
}
