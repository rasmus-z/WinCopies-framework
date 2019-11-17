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

namespace WinCopies.Collections
{

    public interface IReadOnlyCollection : IEnumerable
    {

        int Count { get; }

    }

    public interface IReadOnlyList : IReadOnlyCollection
    {

        object this[int index] { get; }

    }

    public class ReadOnlyArrayList : IReadOnlyList
    {

        private readonly IList innerList = null;

        public ReadOnlyArrayList(IList list) => innerList = list;

        public object this[int index] => innerList[index];

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
