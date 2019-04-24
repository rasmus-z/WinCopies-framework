﻿using System;
using System.ComponentModel;

namespace WinCopies.IO
{

    /// <summary>
    /// Provides info to interact with file system browsable items.
    /// </summary>
    public interface IBrowsableObjectInfo : IFileSystemObject, INotifyPropertyChanged, IDisposable
    {

        /// <summary>
        /// Gets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is browsable.
        /// </summary>
        bool IsBrowsable { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is disposing.
        /// </summary>
        bool IsDisposing { get; }

        /// <summary>
        /// Gets the items of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        WinCopies.Util.ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; }

        // todo: really needed? :

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="IBrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        bool AreItemsLoaded { get; }

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="IBrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        IBrowsableObjectInfo Parent { get; }

        /// <summary>
        /// Gets or sets the items loader for this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        BrowsableObjectInfoItemsLoader ItemsLoader { get; set; }

        // IBrowsableObjectInfo GetBrowsableObjectInfo(IBrowsableObjectInfo browsableObjectInfo);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously.
        /// </summary>
        void LoadItems();

        void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes);

        /// <summary>
        /// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /// </summary>
        /// <param name="itemsLoader"></param>
        void LoadItems(BrowsableObjectInfoItemsLoader itemsLoader);

        void Rename(string newValue);

        // string ToString();

    }

}