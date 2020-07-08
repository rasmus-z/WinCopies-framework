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

namespace WinCopies.Collections.DotNetFix
{
    public class StackCollection : IEnumerable, ICollection, ICloneable
    {
        protected internal Stack InnerStack { get; }

        public int Count => InnerStack.Count;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public StackCollection() : this(new Stack()) { }

        public StackCollection(in Stack stack) => InnerStack = stack;

        public object Clone() => InnerStack.Clone();

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        protected virtual object PopItem() => InnerStack.Pop();

        public object Pop() => PopItem();

        protected virtual void PushItem(object item) => InnerStack.Push(item);

        public void Push(in object item) => PushItem(item);

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public IEnumerator GetEnumerator() => InnerStack.GetEnumerator();
    }
}

#endif
