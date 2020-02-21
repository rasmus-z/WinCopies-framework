/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IDisposable = WinCopies.Util.DotNetFix.IDisposable;

namespace WinCopies.Collections.DotNetFix
{
    public interface IUIntIndexedCollection : IEnumerable
    {
        uint Count { get; }

        object SyncRoot { get; }

        bool IsSynchronized { get; }

        void CopyTo(Array array, uint index);
    }

    public interface IUIntIndexedList : IReadOnlyUIntIndexedList, IEnumerable
    {
        object this[uint index] { get; set; }

        bool IsFixedSize { get; }

        uint Add(object value);

        void Clear();

        void Insert(uint index, object value);

        void Remove(object value);

        void RemoveAt(uint index);
    }

    public interface IReadOnlyUIntIndexedList : IUIntIndexedCollection, IEnumerable
    {
        object this[uint index] { get; }

        bool Contains(object value);

        uint? IndexOf(object value);
    }

    public interface IUIntIndexedCollection<T> : IReadOnlyUIntIndexedCollection<T>, IEnumerable<T>, IEnumerable
    {
        void Add(T item);

        void Clear();

        bool Contains(T item);

        void CopyTo(T[] array, uint arrayIndex);

        bool Remove(T item);
    }

    public interface IReadOnlyUIntIndexedCollection<out T> : IEnumerable<T>, IEnumerable
    {
        uint Count { get; }
    }

    public interface IUIntIndexedList<T> : IUIntIndexedCollection<T>, IReadOnlyUIntIndexedList<T>, IEnumerable<T>, IEnumerable
    {
        T this[uint index] { get; set; }

        uint? IndexOf(T item);

        void Insert(uint index, T item);

        void RemoveAt(uint index);
    }

    public interface IReadOnlyUIntIndexedList<out T> : IReadOnlyUIntIndexedCollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[uint index] { get; }
    }

    // todo: check if the given collection implements the WinCopies.DotNetFix.IDisposable (or WinCopies.IDisposable) interface and, if yes, check the given collection is not disposed (or disposing) in the Current property and in the MoveNext method.

    public abstract class UIntIndexedListEnumeratorBase : IDisposable

    {
        protected internal uint? Index { get; set; } = null;
        protected internal Func<bool> MoveNextMethod { get; internal set; }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        public virtual void Dispose()

        {

            if (!IsDisposed)

            {

                Reset();

                IsDisposed = true;

            }

        }

        public virtual bool MoveNext() => MoveNextMethod();

        public virtual void Reset() => Index = null;
        #endregion
    }

    public sealed class UIntIndexedListEnumerator : UIntIndexedListEnumeratorBase, IEnumerator

    {

        protected internal IReadOnlyUIntIndexedList InnerList { get; private set; }

        public static Func<UIntIndexedListEnumerator, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator e) =>
        {

            if (e.InnerList.Count > 0)

            {

                e.Index = 0;

                e.MoveNextMethod = () =>
                {

                    if (e.Index < e.InnerList.Count - 1)

                    {

                        e.Index++;

                        return true;

                    }

                    else return false;

                };

                return true;

            }

            else return false;

        };

        public object Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return InnerList[Index.Value];
            }
        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList)
        {

            MoveNextMethod = () => DefaultMoveNextMethod(this);

            InnerList = uintIndexedList;

        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            InnerList = uintIndexedList;
        }

        public override void Dispose()
        {
            if (!IsDisposed)

            {

                InnerList = null;

                base.Dispose();

            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = () => DefaultMoveNextMethod(this);
        }
    }

    public sealed class UIntIndexedListEnumerator<T> : UIntIndexedListEnumeratorBase, IEnumerator<T>

    {

        protected internal IReadOnlyUIntIndexedList<T> InnerList { get; private set; }

        public static Func<UIntIndexedListEnumerator<T>, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator<T> e) =>
        {

            if (e.InnerList.Count > 0)

            {

                e.Index = 0;

                e.MoveNextMethod = () =>
                {

                    if (e.Index < e.InnerList.Count - 1)

                    {

                        e.Index++;

                        return true;

                    }

                    else return false;

                };

                return true;

            }

            else return false;

        };

        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return ((IUIntIndexedList<T>)InnerList)[Index.Value];
            }
        }

        object IEnumerator.Current => Current;

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList)
        {

            MoveNextMethod = () => DefaultMoveNextMethod(this);

            InnerList = uintIndexedList;

        }

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            InnerList = uintIndexedList;
        }

        public override void Dispose()
        {
            if (!IsDisposed)

            {

                InnerList = null;

                base.Dispose();

            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = () => DefaultMoveNextMethod(this);
        }
    }
}

namespace WinCopies.Collections
{

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public interface IUIntIndexedCollection

    {

        object this[uint index] { get; }

        uint Count { get; }

    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public interface IUIntIndexedCollection<T> : IUIntIndexedCollection
    {

        T this[uint index] { get; }

    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public abstract class UIntIndexedCollectionEnumeratorBase : IDisposable

    {

        protected internal IUIntIndexedCollection UIntIndexedCollection { get; private set; }
        protected internal uint? Index { get; set; } = null;
        protected internal Func<bool> MoveNextMethod { get; set; }

        public UIntIndexedCollectionEnumeratorBase(IUIntIndexedCollection uintIndexedCollection)
        {
            UIntIndexedCollection = uintIndexedCollection;

            MoveNextMethod = () => UIntIndexedCollectionEnumerator.MoveNextMethod(this);
        }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {

                    UIntIndexedCollection = null;

                    Index = null;

                }

                IsDisposed = true;
            }
        }

        public void Dispose() => Dispose(true);

        public virtual bool MoveNext() => MoveNextMethod();

        public virtual void Reset()
        {
            Index = null;

            MoveNextMethod = () => UIntIndexedCollectionEnumerator.MoveNextMethod(this);
        }
        #endregion
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public sealed class UIntIndexedCollectionEnumerator : UIntIndexedCollectionEnumeratorBase, IEnumerator

    {

        public static Func<UIntIndexedCollectionEnumeratorBase, bool> MoveNextMethod => (UIntIndexedCollectionEnumeratorBase e) =>
        {

            if (e.UIntIndexedCollection.Count > 0)

            {

                e.Index = 0;

                e.MoveNextMethod = () =>
                {

                    if (e.Index < e.UIntIndexedCollection.Count - 1)

                    {

                        e.Index++;

                        return true;

                    }

                    else return false;

                };

                return true;

            }

            else return false;

        };

        public object Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return UIntIndexedCollection[Index.Value];
            }
        }

        public UIntIndexedCollectionEnumerator(IUIntIndexedCollection uintIndexedCollection) : base(uintIndexedCollection)
        {

        }

    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public sealed class UIntIndexedCollectionEnumerator<T> : UIntIndexedCollectionEnumeratorBase, IEnumerator<T>

    {

        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return ((IUIntIndexedCollection<T>)UIntIndexedCollection)[Index.Value];
            }
        }

        object IEnumerator.Current => Current;

        public UIntIndexedCollectionEnumerator(IUIntIndexedCollection<T> uintIndexedCollection) : base(uintIndexedCollection)
        {

        }
    }
}

//public interface IList : System.Collections. IList, ICollection, IEnumerable

//{



//}

//public interface IList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

//{

//    new void Clear();

//    new int Count { get; }

//    //new IEnumerator<T> GetEnumerator();

//    //new bool IsReadOnly { get; }

//    new void RemoveAt(int index);

//    new T this[int index] { get; set; }

//}

//public interface IReadOnlyList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

//{

//    new void Clear();

//    new int Count { get; }

//    //new IEnumerator<T> GetEnumerator();

//    //new bool IsReadOnly { get; }

//    new void RemoveAt(int index);

//    new T this[int index] { get; set; }

//}

//public class ReadOnlyCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IReadOnlyList<T>

//{

//    public ReadOnlyCollection(System.Collections.Generic.IList<T> list) : base(list) { } 

//    T IReadOnlyList<T>.this[int index] { get => this[index]; /*set => throw new NotSupportedException("This collection is read-only.") ;*/ } 

//    // int IReadOnlyCollection<T>.Count => Count ; 

//    // bool IReadOnlyList<T>.IsReadOnly => true ; 

//    //void IReadOnlyList<T>.Clear() => throw new NotSupportedException("This collection is read-only.") ; 

//    // IEnumerator<T> IReadOnlyCollection<T>.GetEnumerator() => throw new NotImplementedException();

//    //void IReadOnlyList<T>.RemoveAt(int index) => throw new NotSupportedException("This collection is read-only.") ;

//}

//public interface ICollection<T, U> : System.Collections.Generic.ICollection<U> where T : U

//{



//}
