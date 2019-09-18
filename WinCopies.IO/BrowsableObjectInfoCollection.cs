using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;
using IList = System.Collections.IList;

namespace WinCopies.IO
{

    // todo: paths must have a protected virtual method to create their items collection. the path's constructor must call this method. using this implementation, a Unregister method does not make sense for the IBrowsableObjectInfoCollection interface.

    //internal class BrowsableObjectInfoCollectionInternal<TOwner, TItems> : Collection<IBrowsableObjectInfoModifier<TOwner, TItems>> where TOwner : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo
    //{

    //    internal new IList<IBrowsableObjectInfoModifier<TOwner, TItems>> Items => base.Items;

    //}

    public interface IBrowsableObjectInfoCollection<TItems> :     WinCopies.Collections.IList<TItems>     where TItems : IBrowsableObjectInfo

    {

        IBrowsableObjectInfo Owner { get; }

        void Remove(TItems item);

    }

    public interface IBrowsableObjectInfoCollection<TOwner, TItems> : IBrowsableObjectInfoCollection<TItems> /*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/ where TOwner : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

    {

        TOwner Owner { get; }

        // void RegisterOwner(IPathModifier<TOwner, TItems> modifier);

    }

    public interface IReadOnlyBrowsableObjectInfoCollection<TItems> : WinCopies.Collections.IReadOnlyList<TItems>/*, WinCopies.Collections.ICollection<IBrowsableObjectInfoModifier<IBrowsableObjectInfo>>*/
    {

        new IBrowsableObjectInfo Owner { get; }

    }

    public class BrowsableObjectInfoCollection<TOwner, TItems> : Collection<TItems>, IBrowsableObjectInfoCollection<TOwner, TItems> where TOwner : BrowsableObjectInfo where TItems : BrowsableObjectInfo
    {

        //public void RegisterOwner(IPathModifier<TOwner, TItems> modifier)

        //{

        //    // if (object.ReferenceEquals(modifier.Owner, accessor.Owner))

        //    if (object.ReferenceEquals(this, modifier.Accessor.ItemCollection))

        //    {

        //        _modifier = modifier;

        //        // _innerList = new BrowsableObjectInfoCollectionInternal<TOwner, TItems>();

        //    }

        //    else

        //        throw new ArgumentException("Invalid owner.");

        //    // else

        //    // throw new ArgumentException("Invalid owner.");

        //}

        public BrowsableObjectInfoCollection(TOwner owner) : this(new List<TItems>(), owner) { }

        public BrowsableObjectInfoCollection(List<TItems> items, TOwner owner) : base(items) => Owner = !(Owner is null) ? owner : throw new InvalidOperationException("This collection already has an owner.");

        // todo: check if is registered

        public TOwner Owner { get; }

        IBrowsableObjectInfo IBrowsableObjectInfoCollection<TItems>.Owner => Owner;

        // private IPathModifier<TOwner, TItems> _modifier;

        public virtual bool IsReadOnly => false;

        public void Sort(int index, int count, System.Collections.Generic.IComparer<TItems> comparer) => ((List<TItems>)Items).Sort(index, count, comparer);

        protected override void SetItem(int index, TItems item)
        {

            if (item.HasParent)

                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

            this[index].Parent = null;

            this[index].HasParent = false;

            base.SetItem(index, item);

            this[index].Parent = Owner;

            this[index].HasParent = true;

        }

        protected override void InsertItem(int index, TItems item)
        {

            if (item.HasParent)

                throw new InvalidOperationException("item is already added to an IBrowsableObjectInfoCollection.");

            base.InsertItem(index, item);

            item.Parent = Owner;

            item.HasParent = true;

        }

        protected override void RemoveItem(int index)

        {

            this[index].Parent = null;

            this[index].HasParent = false;

            base.RemoveItem(index);

        }

        void IBrowsableObjectInfoCollection<TItems>.Remove(TItems item) => Remove(item);

        protected override void ClearItems()

        {

            for (int i = 0; i < Count; i++)

            {

                this[i].Parent = null;

                this[i].HasParent = false;

            }

            base.ClearItems();

            Owner.AreItemsLoaded = false;

        }

    }

    public class ReadOnlyBrowsableObjectInfoCollection<TItems> : System.Collections.ObjectModel.ReadOnlyCollection<TItems>, IReadOnlyBrowsableObjectInfoCollection<TItems> where TItems : BrowsableObjectInfo

    {

        public ReadOnlyBrowsableObjectInfoCollection(IBrowsableObjectInfoCollection<TItems> items) : base(items) { }

        // todo: test

        public IBrowsableObjectInfo Owner => ((BrowsableObjectInfoCollection<BrowsableObjectInfo, TItems>)Items).Owner;

        TItems Collections.IReadOnlyList<TItems>.this[int index] { get => this[index]; set => ((IList)this)[index] = value; }

        void Collections.IReadOnlyList<TItems>.Clear() => ((IList)this).Clear();

        void Collections.IReadOnlyList<TItems>.RemoveAt(int index) => ((IList)this).RemoveAt(index);

    }

}
