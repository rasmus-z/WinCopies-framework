﻿using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IFileSystemObjectLoader<TPath> : IBrowsableObjectInfoLoader<TPath> where TPath : IBrowsableObjectInfo
    {

        FileTypes FileTypes { get; set; }

    }

    public abstract class FileSystemObjectLoader<TPath> : BrowsableObjectInfoLoader<TPath>, IFileSystemObjectLoader<TPath> where TPath : BrowsableObjectInfo
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectLoader<TPath>), true); }

        public FileSystemObjectLoader( TPath path, bool workerReportsProgress, bool workerSupportsCancellation, FileTypes fileTypes) : this( path, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), fileTypes) { }

        public FileSystemObjectLoader( TPath path, bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> browsableObjectInfoComparer, FileTypes fileTypes) : base( path, workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

        public override bool CheckFilter(string path)

        {

            if (Filter is null) return true;

            foreach (string filter in Filter)

                if (!IO.Path.MatchToFilter(path, filter)) return false;

            return true;

        }

    }
}
