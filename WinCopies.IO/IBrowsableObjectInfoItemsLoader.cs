using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides common properties and methods for the <see cref="IBrowsableObjectInfo"/> items loaders.
    /// </summary>
    public interface IBrowsableObjectInfoItemsLoader : IBackgroundWorker, IDisposable
    {

        IEnumerable<string> Filter { get; set; }

        IBrowsableObjectInfo Path { get; }

        bool CheckFilter(string path);

        void LoadItems();

        void LoadItemsAsync();

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfoItemsLoader"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">This <see cref="IBrowsableObjectInfoItemsLoader"/> is busy and does not support cancellation.</exception>
        new void Dispose();

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfoItemsLoader"/> and optionally disposes the related <see cref="Path"/>.
        /// </summary>
        /// <param name="disposePath">Whether to dispose the related <see cref="Path"/>. If this parameter is set to <see langword="true"/>, the <see cref="IBrowsableObjectInfo.ItemsLoader"/>s of the parent and childs of the related <see cref="Path"/> will be disposed recursively.</param>
        /// <exception cref="InvalidOperationException">This <see cref="IBrowsableObjectInfoItemsLoader"/> is busy and does not support cancellation.</exception>
        void Dispose(bool disposePath);

    }

}
