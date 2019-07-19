using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.IO
{
    public abstract class FileSystemObjectItemsLoader : BrowsableObjectInfoItemsLoader
    {

        private readonly FileTypes _fileTypes = Util.Util.GetAllEnumFlags<FileTypes>();

        public FileTypes FileTypes { get => _fileTypes; set => this.SetBackgroundWorkerProperty(nameof(FileTypes), nameof(_fileTypes), value, typeof(FileSystemObjectItemsLoader), true); }

        public FileSystemObjectItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypes fileTypes) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), fileTypes) { }

        public FileSystemObjectItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> browsableObjectInfoComparer, FileTypes fileTypes) : base(workerReportsProgress, workerSupportsCancellation, browsableObjectInfoComparer) => _fileTypes = fileTypes;

        public override bool CheckFilter(string directory)

        {

            if (Filter == null) return true;

            foreach (string filter in Filter)

                if (!IO.Path.MatchToFilter(directory, filter)) return false;

            return true;

        }

    }
}
