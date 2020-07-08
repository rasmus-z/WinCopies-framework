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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class StackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected internal Stack<T> InnerStack { get; }

        public int Count => InnerStack.Count;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        public StackCollection() : this(new Stack<T>()) { }

        public StackCollection(in Stack<T> stack) => InnerStack = stack;

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        protected virtual T PopItem() => InnerStack.Pop();

        public T Pop() => PopItem();

        protected virtual void PushItem(T item) => InnerStack.Push(item);

        public void Push(in T item) => PushItem(item);

        public T[] ToArray() => InnerStack.ToArray();

        public void TrimExcess() => InnerStack.TrimExcess();

#if NETCORE

        public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

        protected virtual bool TryPopItem(out T result) => InnerStack.TryPop(out result);

        public bool TryPop(out T result) => TryPopItem(out result);

#endif

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }
}

#endif
