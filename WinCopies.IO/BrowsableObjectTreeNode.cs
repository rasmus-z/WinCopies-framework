using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;

namespace WinCopies.IO
{

    public interface IBrowsableObjectTreeNode<TValue, TItems> where TValue : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo
    {

        /// <summary>
        /// Gets a value that indicates whether the items of this <see cref="IBrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        bool AreItemsLoaded { get; }

        IBrowsableObjectInfoLoader<TValue, TItems> ItemsLoader { get; }

        ///// <summary>
        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/>.
        ///// </summary>
        //void LoadItems();

        ///// <summary>
        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/> using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        /////// <summary>
        /////// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using a given items loader.
        /////// </summary>
        /////// <param name="itemsLoader">A custom items loader.</param>
        ////void LoadItems(IBrowsableObjectInfoLoader itemsLoader);

        ///// <summary>
        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously.
        ///// </summary>
        //void LoadItemsAsync();

        ///// <summary>
        ///// Loads the items of this <see cref="IBrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

    }

    // ///<remarks>The <see cref="Parent"/> property is set directly on first call.</remarks>
    public class BrowsableObjectTreeNode<TValue, TItems, TFactory> : ReadOnlyTreeNode<TValue, TItems>, IBrowsableObjectTreeNode<TValue, TItems> where TValue : BrowsableObjectInfo where TItems : BrowsableObjectInfo where TFactory : BrowsableObjectInfoFactory
    {

        protected internal new System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>> Items => base.Items;

        public IBrowsableObjectInfoLoader<TValue, TItems> ItemsLoader { get; internal set; }

        private ITreeNode _parent;

        private Func<ITreeNode> _getParentDelegate;

        public override ITreeNode Parent
        {

            get
            {

                if (_parent is null)

                {
                    _parent = _getParentDelegate();

                    _getParentDelegate = null;

                }

                return _parent;

            }

            protected set => _parent = value;

        }

        private TFactory _factory;

        ///// <summary>
        ///// Gets or sets the factory for this <see cref="BrowsableObjectInfo"/>. This factory is used to create new <see cref="IBrowsableObjectInfo"/>s from the current <see cref="BrowsableObjectInfo"/> and its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.
        ///// </summary>
        ///// <exception cref="InvalidOperationException">The old <see cref="BrowsableObjectInfo.ItemsLoader"/> is running. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        ///// <exception cref="ArgumentNullException">value is null.</exception>
        public TFactory Factory
        {

            get => _factory;

            set

            {

                ThrowOnInvalidFactoryUpdateOperation(value, nameof(value));

                _factory = value;

            }

        }

        /// <summary>
        /// Checks if an <see cref="IBrowsableObjectInfoFactory"/> can be added to this <see cref="BrowsableObjectInfo"/> and throw an exception if the validation failed.
        /// </summary>
        /// <param name="newFactory">The new factory to use in this <see cref="BrowsableObjectInfo"/> and in its associated <see cref="BrowsableObjectInfo.ItemsLoader"/>.</param>
        /// <param name="paramName">The parameter name to include in error messages.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo.ItemsLoader"/> is busy. OR The given factory has already been added to a <see cref="BrowsableObjectInfo"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newFactory"/> is null.</exception>
        protected virtual void ThrowOnInvalidFactoryUpdateOperation(IBrowsableObjectInfoFactory newFactory, string paramName)

        {

            if (ItemsLoader?.IsBusy == true)

                throw new InvalidOperationException($"The {nameof(ItemsLoader)} is busy.");

            if (newFactory is null)

                throw new ArgumentNullException(paramName);

        }

        /// <summary>
        /// Gets a value that indicates if the items of this <see cref="BrowsableObjectInfo"/> are currently loaded.
        /// </summary>
        public bool AreItemsLoaded { get; internal set; }

        public BrowsableObjectTreeNode(TValue value) : base(value)
        {
        }

        public BrowsableObjectTreeNode(TValue value, System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>> items) : base(value, items)
        {
        }

        ///// <summary>
        ///// Loads the items of this <see cref="BrowsableObjectInfo"/>.
        ///// </summary>
        //public virtual void LoadItems()

        //{

        //if (ItemsLoader == null)

        //    LoadItems(true, true);

        //else

        //        ItemsLoader.LoadItems();

        //}

        ///// <summary>
        ///// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //public abstract void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation);

        ///// <summary>
        ///// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        ///// </summary>
        ///// <param name="itemsLoader">A custom items loader.</param>
        //public virtual void LoadItems(IBrowsableObjectInfoLoader itemsLoader)

        //{

        //    if (!IsBrowsable)

        //        throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject));

        //    ItemsLoader = itemsLoader;

        //    ItemsLoader.LoadItems();

        //}

        ///// <summary>
        ///// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously.
        ///// </summary>
        //public virtual void LoadItemsAsync()

        //{

        //    if (ItemsLoader == null)

        //        LoadItemsAsync(true, true);

        //    else

        //        ItemsLoader.LoadItemsAsync();

        //}

        ///// <summary>
        ///// When overridden in a derived class, loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using custom worker behavior options.
        ///// </summary>
        ///// <param name="workerReportsProgress">Whether the worker reports progress</param>
        ///// <param name="workerSupportsCancellation">Whether the worker supports cancellation.</param>
        //public abstract void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation);

        ///// <summary>
        ///// Loads the items of this <see cref="BrowsableObjectInfo"/> asynchronously using a given items loader.
        ///// </summary>
        ///// <param name="itemsLoader">A custom items loader.</param>
        //public virtual void LoadItemsAsync(IBrowsableObjectInfoLoader itemsLoader)

        //{

        //    if (!IsBrowsable)

        //        throw new InvalidOperationException(string.Format(Generic.NotBrowsableObject));

        //    ItemsLoader = itemsLoader;

        //    ItemsLoader.LoadItemsAsync();

        //}

    }

}
