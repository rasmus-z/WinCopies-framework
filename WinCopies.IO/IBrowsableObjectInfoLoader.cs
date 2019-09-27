using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;
using WinCopies.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{

    /// <summary>
    /// Represents a loader that can be used to load <see cref="IBrowsableObjectInfo"/>. Note: An <see cref="InvalidOperationException"/> is thrown when the <see cref="System.IDisposable.Dispose"/> method is called if this <see cref="IBrowsableObjectInfoLoader{T}"/> is busy and does not support cancellation.
    /// </summary>
    public interface IBrowsableObjectInfoLoader : IBackgroundWorker, IDeepCloneable, IDisposable

    {

        IFileSystemObjectComparer<IFileSystemObject> FileSystemObjectComparer { get; set; }

        IEnumerable<string> Filter { get; set; }

        bool CheckFilter(string path);

        void LoadItems();

        void LoadItemsAsync();

    }

    public interface IBrowsableObjectInfoLoader<TPath, TItems> : IBrowsableObjectInfoLoader where TPath : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

    {

        IBrowsableObjectTreeNode<TPath, TItems> Path { get; }

    }

}
