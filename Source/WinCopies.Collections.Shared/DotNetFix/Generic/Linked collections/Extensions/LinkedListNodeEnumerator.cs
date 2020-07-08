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

#if !WinCopies2

using System;
using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic.Extensions
{
    [Serializable]
    internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<LinkedListNode<T>>, IEnumerable<LinkedListNode<T>>
    {
        private ILinkedList<T> _list;

        public LinkedListNode<T> Current { get; private set; }

        object IEnumerator.Current => Current;

        public LinkedListNodeEnumerator(ILinkedList<T> list) => _list = list;

        public void Dispose()
        {
            Current = null;

            _list = null;
        }

        private bool _first = true;

        public bool MoveNext()
        {
            if (_list.Count == 0)

                return false;

            if (_first)
            {
                _first = false;

                Current = _list.First;

                return true;
            }

            if (Current.Next == null)
            {
                Current = null;

                return false;
            }

            Current = Current.Next;

            return true;
        }

        public void Reset() { }

        public IEnumerator<LinkedListNode<T>> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

#endif
