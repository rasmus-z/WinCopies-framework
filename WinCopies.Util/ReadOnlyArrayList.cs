using System;
using System.Collections;

namespace WinCopies.Collections
{
    public interface IReadOnlyList : IList
    {

        object this[int index] { get; }

    }

    public class ReadOnlyArrayList : IEnumerable, IList, ICollection, IReadOnlyList
    {

        private IList innerList = null;

        public ReadOnlyArrayList(IList list) => innerList = list;

        public object this[int index] { get => innerList[index]; }

        object IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

        public int Count => innerList.Count;

        public object SyncRoot => innerList.SyncRoot;

        public bool IsSynchronized => innerList.IsSynchronized;

        public bool IsReadOnly => true;

        public bool IsFixedSize => true;

        int IList.Add(object value) => throw new NotImplementedException();

        void IList.Clear() => throw new NotImplementedException();

        public bool Contains(object value) => innerList.Contains(value);

        public void CopyTo(Array array, int index) => innerList.CopyTo(array, index);

        public IEnumerator GetEnumerator() => innerList.GetEnumerator();

        public int IndexOf(object value) => innerList.IndexOf(value);

        void IList.Insert(int index, object value) => throw new NotImplementedException();

        void IList.Remove(object value) => throw new NotImplementedException();

        void IList.RemoveAt(int index) => throw new NotImplementedException();

    }
}
