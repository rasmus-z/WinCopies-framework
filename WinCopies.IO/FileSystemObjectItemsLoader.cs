using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public abstract class FileSystemObjectItemsLoader : BrowsableObjectInfoItemsLoader
    {

        private FileTypes _fileTypes = FileTypes.All;

        public FileTypes FileTypes

        {

            get => _fileTypes;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException("The BackgroundWorker is busy.");

                _fileTypes = value;

            }

        }

        public FileSystemObjectItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, FileTypes fileTypes) : base(workerReportsProgress, workerSupportsCancellation) => FileTypes = fileTypes;

        public override bool CheckFilter(string directory)

        {

            if (Filter == null) return true;

            foreach (string filter in Filter)

                if (!IO.Path.MatchToFilter(directory, filter)) return false;

            return true;

        }

    }
}
