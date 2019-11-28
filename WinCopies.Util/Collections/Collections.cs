/* Copyright © Pierre Sprimont, 2019
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Collections
{

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

}
