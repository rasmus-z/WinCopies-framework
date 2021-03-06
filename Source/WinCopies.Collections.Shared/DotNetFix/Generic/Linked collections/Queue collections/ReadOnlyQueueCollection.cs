﻿/* Copyright © Pierre Sprimont, 2020
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
using System.Diagnostics.CodeAnalysis;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected Queue<T> InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
        public int Count => InnerQueue.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="queue">The inner <see cref="Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
        public ReadOnlyQueueCollection(in Queue<T> queue) => InnerQueue = queue;

        public ReadOnlyQueueCollection(in QueueCollection<T> queueCollection) : this(queueCollection.InnerQueue) { }

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Generic.Queue{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) => InnerQueue.Contains(item);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
        /// <seealso cref="TryPeek(out T)"/>
        public T Peek() => InnerQueue.Peek();

#if NETCORE

        /// <summary>
        /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
        /// </summary>
        /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
        /// <seealso cref="Peek"/>
        public bool TryPeek([MaybeNullWhen(false)] out T result) => InnerQueue.TryPeek(out result);

#endif

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
    }
}

#endif
