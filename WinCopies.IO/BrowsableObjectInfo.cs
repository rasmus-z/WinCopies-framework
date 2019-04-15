using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinCopies.Util;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace WinCopies.IO
{
    public abstract class BrowsableObjectInfo : IBrowsableObjectInfo
    {

        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType)

        {

            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Gets the path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public string Path { get; } = null;

        /// <summary>
        /// When overriden in a derived class, gets the localized path of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract string LocalizedPath { get; }

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

        private bool isDisposing = false;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is disposing.
        /// </summary>
        public bool IsDisposing { get => isDisposing; private set => OnPropertyChanged(nameof(IsDisposing), nameof(isDisposing), value, typeof(BrowsableObjectInfo)); }

        private bool areItemsLoaded = false;

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        public bool AreItemsLoaded { get => areItemsLoaded; internal set => OnPropertyChanged(nameof(AreItemsLoaded), nameof(areItemsLoaded), value, typeof(BrowsableObjectInfo)); }

        public WinCopies.Util.ObservableCollection<IBrowsableObjectInfo> items = new WinCopies.Util.ObservableCollection<IBrowsableObjectInfo>();

        /// <summary>
        /// Gets the items of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public WinCopies.Util.ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; } = null;

        /// <summary>
        /// When overriden in a derived class, gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public abstract IBrowsableObjectInfo Parent { get; protected set; }

        /// <summary>
        /// The file type of this <see cref="ShellObjectInfo"/>.
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

                BrowsableObjectInfoItemsLoader previousValue = itemsLoader;

                itemsLoader = value;

                if (value != null)

                    value.Path = this;

                PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(nameof(ItemsLoader), previousValue, value));

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        /// <param name="path">The path of this <see cref="BrowsableObjectInfo"/>.</param>
        /// <param name="fileType">The <see cref="FileType"/> of this <see cref="BrowsableObjectInfo"/>.</param>
        public BrowsableObjectInfo(string path, FileType fileType)

        {

            Path = path;

            FileType = fileType;

            Items = new WinCopies.Util.ReadOnlyObservableCollection<IBrowsableObjectInfo>(items);

        }

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

    }
}
