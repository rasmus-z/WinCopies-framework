using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Collections
{

    public interface IList : System.Collections. IList, ICollection, IEnumerable

    {



    }

    public interface IList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

    {
        
        new void Clear();

        new int Count { get; }

        //new IEnumerator<T> GetEnumerator();

        //new bool IsReadOnly { get; }

        new void RemoveAt(int index);

        new T this[int index] { get; set; }

    }

    public interface IReadOnlyList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

    {

        new void Clear();

        new int Count { get; }

        //new IEnumerator<T> GetEnumerator();

        //new bool IsReadOnly { get; }

        new void RemoveAt(int index);

        new T this[int index] { get; set; }

    }

    public class ReadOnlyCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IReadOnlyList<T>

    {

        public ReadOnlyCollection(System.Collections.Generic.IList<T> list) : base(list) { } 

        T IReadOnlyList<T>.this[int index] { get => this[index]; set => throw new NotSupportedException("This collection is read-only.") ; } 

        // int IReadOnlyCollection<T>.Count => Count ; 

        // bool IReadOnlyList<T>.IsReadOnly => true ; 

        void IReadOnlyList<T>.Clear() => throw new NotSupportedException("This collection is read-only.") ; 

        // IEnumerator<T> IReadOnlyCollection<T>.GetEnumerator() => throw new NotImplementedException();

        void IReadOnlyList<T>.RemoveAt(int index) => throw new NotSupportedException("This collection is read-only.") ; 

    }

}
