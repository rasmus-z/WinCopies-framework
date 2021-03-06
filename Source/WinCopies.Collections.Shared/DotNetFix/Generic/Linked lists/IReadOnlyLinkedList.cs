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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IReadOnlyLinkedList<T> : ICollection<T>, ICountableEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
    {
        LinkedListNode<T> Last { get; }

        LinkedListNode<T> First { get; }

        new int Count { get; }

        LinkedListNode<T> Find(T value);

        LinkedListNode<T> FindLast(T value);

        // todo: to remove

        // new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
    }
}

#endif
