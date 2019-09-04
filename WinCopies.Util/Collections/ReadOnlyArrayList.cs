using System;
using System.Collections;

namespace WinCopies.Collections
{
    public interface IReadOnlyList : ICollection, IEnumerable
    {

        object this[int index] { get; }

    }

    public class ReadOnlyArrayList : IReadOnlyList
    {

        private IList innerList = null;

        public ReadOnlyArrayList(IList list) => innerList = list;

        public object this[int index] { get => innerList[index]; }

        public int Count => innerList.Count;

        public object SyncRoot => innerList.SyncRoot;

        public bool IsSynchronized => innerList.IsSynchronized;

        public bool IsReadOnly => true;

        public bool IsFixedSize => true;

        public bool Contains(object value) => innerList.Contains(value);

        public void CopyTo(Array array, int index) => innerList.CopyTo(array, index);

        public IEnumerator GetEnumerator() => innerList.GetEnumerator();

        public int IndexOf(object value) => innerList.IndexOf(value);

    }
}
