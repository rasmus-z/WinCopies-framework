using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public abstract class PathInfo : FileSystemObject
    {

        public string NormalizedPath { get; }

        protected PathInfo(string path, string normalizedPath, FileType fileType) : base(path, fileType) => NormalizedPath = normalizedPath;

    }
}
