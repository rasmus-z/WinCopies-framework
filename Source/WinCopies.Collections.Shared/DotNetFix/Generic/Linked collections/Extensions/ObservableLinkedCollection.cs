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
using System.Collections.Generic;
using System.ComponentModel;

namespace WinCopies.Collections.DotNetFix.Generic.Extensions
{
    [Serializable]
    public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableLinkedCollection() : base() { }

        public ObservableLinkedCollection(in ILinkedList<T> list) : base(list) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in LinkedListNode<T> addedBefore, in LinkedListNode<T> addedAfter, in LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

        protected override void AddFirstItem(LinkedListNode<T> node)
        {
            base.AddFirstItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
        }

        protected override LinkedListNode<T> AddFirstItem(T value)
        {
            LinkedListNode<T> result = base.AddFirstItem(value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, result);

            return result;
        }

        protected override void AddItem(T item)
        {
            base.AddItem(item);

            RaiseCountPropertyChangedEvent();

            // Assumming that items are added to the end of the list.

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, InnerList.Last);
        }

        protected override void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemAfter(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
        }

        protected override LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemAfter(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

            return result;
        }

        protected override void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemBefore(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
        }

        protected override LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemBefore(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

            return result;
        }

        protected override void AddLastItem(LinkedListNode<T> node)
        {
            base.AddLastItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
        }

        protected override LinkedListNode<T> AddLastItem(T value)
        {
            LinkedListNode<T> result = base.AddLastItem(value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, result);

            return result;
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Reset, null, null, null);
        }

        protected override void RemoveFirstItem()
        {
            LinkedListNode<T> node = InnerList.First;

            base.RemoveFirstItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override void RemoveItem(LinkedListNode<T> node)
        {
            base.RemoveItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override bool RemoveItem(T item)
        {
            foreach (LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

                if (node.Value.Equals(item))
                {
                    base.RemoveItem(node); // This is a custom internal enumerator designed to do not throw when its underlying collection change.

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);

                    return true;
                }

            return false;
        }

        protected override void RemoveLastItem()
        {
            LinkedListNode<T> node = InnerList.Last;

            base.RemoveLastItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }
    }
}

#endif
