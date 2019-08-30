using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IFileSystemObjectInfoFactory : IBrowsableObjectInfoFactory { }

    public interface IFileSystemObjectInfo : IBrowsableObjectInfo { }

    public abstract class FileSystemObjectInfo<TParent, TItems, TFactory> : BrowsableObjectInfo<TParent, TItems, TFactory>, IFileSystemObjectInfo where TParent : class, IFileSystemObjectInfo where TItems : class, IFileSystemObjectInfo where TFactory : IFileSystemObjectInfoFactory
    {
        protected FileSystemObjectInfo(string path, FileType fileType, TFactory factory) : base(path, fileType, factory)
        {
        }
    }
}
