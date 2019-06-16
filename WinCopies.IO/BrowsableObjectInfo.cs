using System;
using System.Linq;
using System.Windows.Media.Imaging;
using WinCopies.Collections;

namespace WinCopies.IO
{
    public abstract class BrowsableObjectInfo : IBrowsableObjectInfo
    {

        /// <summary>
        /// Gets the path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public string Path { get; } = null;

        /// <summary>
        /// When overriden in a derived class, gets the localized path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string LocalizedName { get; }

        /// <summary>
        /// When overriden in a derived class, gets the name of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// When overriden in a derived class, gets the small <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource SmallBitmapSource { get; }

        /// <summary>
        /// When overriden in a derived class, gets the medium <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource MediumBitmapSource { get; }

        /// <summary>
        /// When overriden in a derived class, gets the large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource LargeBitmapSource { get; }

        /// <summary>
        /// When overriden in a derived class, gets the extra large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource ExtraLargeBitmapSource { get; }

        /// <summary>
        /// When overriden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is browsable.
        /// </summary>
        public abstract bool IsBrowsable { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is disposing.
        /// </summary>
        public bool IsDisposing { get; set; }

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        public bool AreItemsLoaded { get; set; }

        internal readonly ObservableCollection<IBrowsableObjectInfo> items = new ObservableCollection<IBrowsableObjectInfo>();

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; } = null;

        private IBrowsableObjectInfo _parent = null;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public IBrowsableObjectInfo Parent
        {
            get

            {

                if (_parent == null)

                    _parent = GetParent();

                return _parent;

            }

            internal set => _parent = value;
        }

        /// <summary>
        /// The file type of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public FileType FileType { get; private set; } = FileType.None;

        private BrowsableObjectInfoItemsLoader itemsLoader = null;

        /// <summary>
        /// Gets or sets the items loader for this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public BrowsableObjectInfoItemsLoader ItemsLoader
        {
            get => itemsLoader;

            set
            {

                if (value == itemsLoader)

                    return;

                itemsLoader = value;

                if (value != null)

                    value.Path = this;

            }

        }

        // private bool _considerAsPathRoot = false;

        // public bool ConsiderAsPathRoot { get => _considerAsPathRoot; set => OnPropertyChanged(nameof(ConsiderAsPathRoot), nameof(_considerAsPathRoot), value, typeof(BrowsableObjectInfo)); }

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
        public BrowsableObjectInfo(string path, FileType fileType)

        {

            Path = path;

            FileType = fileType;

            Items = new ReadOnlyObservableCollection<IBrowsableObjectInfo>(items);

        }

        public abstract IBrowsableObjectInfo GetParent();

        // protected abstract IBrowsableObjectInfo GetBrowsableObjectInfo(string path, FileTypes fileType);

        /// <summary>
        /// When overriden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously.
        /// </summary>
        public abstract void LoadItems();

        /// <summary>
        /// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using the given <see cref="BrowsableObjectInfoItemsLoader"/>.
        /// </summary>
        /// <param name="browsableObjectInfoItemsLoader">Custom loader to load the items of this <see cref="ShellObjectInfo"/>.</param>
        public abstract void LoadItems(BrowsableObjectInfoItemsLoader browsableObjectInfoItemsLoader);

        public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation, FileTypesFlags fileTypes);

        // /// <summary>
        // /// Frees the <see cref="ArchiveFileStream"/> property to unlock the archive referenced by it and makes it <see langword="null"/>. Calling this method will erase all the <see cref="Items"/> of this <see cref="ShellObjectInfo"/> in memory.
        // /// </summary>
        public virtual void Dispose() => Dispose(true);

        private void Dispose(bool disposeParentBrowsableObjectInfo)

        {

            IsDisposing = true;

            if (ItemsLoader != null && ItemsLoader.IsBusy)

                throw new InvalidOperationException("The items loader is busy.");

            else if (ItemsLoader != null)

                ItemsLoader.Dispose();

            foreach (BrowsableObjectInfo browsableObjectInfo in Items.OfType<BrowsableObjectInfo>())

            {

                browsableObjectInfo.Dispose(false);

                if (browsableObjectInfo.ItemsLoader != null)

                    browsableObjectInfo.ItemsLoader.Dispose();

            }

            if (disposeParentBrowsableObjectInfo && Parent != null)

                Parent.Dispose();

            IsDisposing = false;

        }

        /// <summary>
        /// When overriden in a derived class, renames or move to a relative path, or both, the current <see cref="BrowsableObjectInfo"/> with the specified name.
        /// </summary>
        /// <param name="newValue">The new name or relative path for this <see cref="ShellObjectInfo"/>.</param>
        public abstract void Rename(string newValue);

        public override string ToString() => Name;

        public abstract IBrowsableObjectInfo Clone();

        /// <summary>
        /// Determines whether the specified object is equal to the current object by testing the following things, in order: whether <b>obj</b> implements the <see cref="IBrowsableObjectInfo"/> interface and both objects references, and <see cref="FileType"/> and <see cref="Path"/> properties are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IBrowsableObjectInfo _obj)

            {

                if (ReferenceEquals(this, obj)) return true;

                return FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower();

            }

            else return false;
        }

        /// <summary>
        /// Get an hash code for this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The hash codes of the <see cref="FileType"/> and the <see cref="Path"/> property.</returns>
        public override int GetHashCode() => FileType.GetHashCode() ^ Path.ToLower().GetHashCode();
    }
}
