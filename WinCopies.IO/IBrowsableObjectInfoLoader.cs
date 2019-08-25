using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{

    public interface IBrowsableObjectInfoLoader :    IBackgroundWorker2, IDeepCloneable, IDisposable

    {

        IFileSystemObjectComparer<IFileSystemObject> FileSystemObjectComparer { get; set; }

        IEnumerable<string> Filter { get; set; }

        bool CheckFilter(string path);

        void LoadItems();

        void LoadItemsAsync();

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfoLoader{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">This <see cref="IBrowsableObjectInfoLoader{T}"/> is busy and does not support cancellation.</exception>
        new void Dispose();

        IBrowsableObjectInfo Path { get; }

    }

    public interface IBrowsableObjectInfoLoader<TPath> : IBrowsableObjectInfoLoader where TPath : IBrowsableObjectInfo

    {

        TPath Path { get; set; }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfoLoader{T}"/> and optionally disposes the related <see cref="Path"/>.
        /// </summary>
        /// <param name="disposePath">Whether to dispose the related <see cref="Path"/>. If this parameter is set to <see langword="true"/>, the <see cref="IBrowsableObjectInfo.ItemsLoader"/>s of the parent and childs of the related <see cref="Path"/> will be disposed recursively.</param>
        /// <exception cref="InvalidOperationException">This <see cref="IBrowsableObjectInfoLoader{T}"/> is busy and does not support cancellation.</exception>
        void Dispose(bool disposePath);

    }

}
