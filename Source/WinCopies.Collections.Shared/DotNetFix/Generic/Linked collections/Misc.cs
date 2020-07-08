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
using System.Collections.Specialized;

using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<LinkedListNode<T>>, IEnumerable<LinkedListNode<T>>
    {
        private System.Collections.Generic.LinkedList<T> _list;

        public LinkedListNode<T> Current { get; private set; }

        object IEnumerator.Current => Current;

        public LinkedListNodeEnumerator(System.Collections.Generic.LinkedList<T> list) => _list = list;

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

    public class SimpleLinkedCollectionChangedEventArgs<T>
    {
        public NotifyCollectionChangedAction Action { get; }

        public T Item { get; }

        public SimpleLinkedCollectionChangedEventArgs(NotifyCollectionChangedAction action, T item)
        {
            Action = action;

            Item = item;
        }
    }

    public delegate void SimpleLinkedCollectionChangedEventHandler<T>(object sender, SimpleLinkedCollectionChangedEventArgs<T> e);

    public interface INotifySimpleLinkedCollectionChanged<T>
    {
        event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
    }

    public enum LinkedCollectionChangedAction : byte
    {
        AddFirst = 0,

        AddLast = 1,

        AddBefore = 2,

        AddAfter = 3,

        Remove = 4,

        //Replace = 5,

        Move = 6,

        Reset = 7,
    }

    public class LinkedCollectionChangedEventArgs<T>
    {
        public LinkedCollectionChangedAction Action { get; }

        public LinkedListNode<T> AddedBefore { get; }

        public LinkedListNode<T> AddedAfter { get; }

        public LinkedListNode<T> Node { get; }

        public LinkedCollectionChangedEventArgs(LinkedCollectionChangedAction action, LinkedListNode<T> addedBefore, LinkedListNode<T> addedAfter, LinkedListNode<T> node)
        {
            bool check(LinkedCollectionChangedAction _action, LinkedListNode<T> parameter) => (_action == action && parameter == null) || !(_action == action || parameter == null);

            if ((action == LinkedCollectionChangedAction.Reset && (node != null || addedBefore != null || addedAfter != null))
                || (action != LinkedCollectionChangedAction.Reset && node == null)
                || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.AddFirst, LinkedCollectionChangedAction.AddLast) && (addedBefore != null || addedAfter != null))
                || (action == LinkedCollectionChangedAction.Move && addedBefore == null && addedAfter == null)
                || check(LinkedCollectionChangedAction.AddBefore, addedBefore)
                || check(LinkedCollectionChangedAction.AddAfter, addedAfter)
                || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.Remove, LinkedCollectionChangedAction.Reset) && !(addedBefore == null && addedAfter == null)))

                throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)}, {nameof(addedBefore)} or {nameof(addedAfter)}.");

            Action = action;

            AddedBefore = addedBefore;

            AddedAfter = addedAfter;

            Node = node;
        }
    }

    public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

    public interface INotifyLinkedCollectionChanged<T>
    {
        event LinkedCollectionChangedEventHandler<T> CollectionChanged;
    }
}

#endif
