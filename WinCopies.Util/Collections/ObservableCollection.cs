using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WinCopies.Collections
{

    public class NotifyCollectionChangedEventArgs : System.Collections.Specialized.NotifyCollectionChangedEventArgs

    {

        public bool IsChangingEvent { get; } = false;

        public IList ResetItems { get; }

        public NotifyCollectionChangedEventArgs(IList resetItems) : base(NotifyCollectionChangedAction.Reset)

        {

            IsChangingEvent = true;

            ResetItems = resetItems;

        }

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) => IsChangingEvent = isChangingEvent;

    }

    public delegate void NotifyCollectionChangingEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, INotifyCollectionChanging
    {

        public event NotifyCollectionChangingEventHandler CollectionChanging;

        public ObservableCollection()
        {

        }

        public ObservableCollection(IList<T> list) : base(list)
        {

        }

        public ObservableCollection(IEnumerable<T> collection) : base(collection)
        {

        }

    protected override void InsertItem(int index, T item)
    {

        OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Add, item, index));

        base.InsertItem(index, item);

    }

    protected override void MoveItem(int oldIndex, int newIndex)

    {

        OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Move, this[oldIndex], oldIndex, newIndex));

        base.MoveItem(oldIndex, newIndex);

    }

    protected override void SetItem(int index, T item)

    {

        OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Replace, this[index], item));

        base.SetItem(index, item);

    }

    protected override void RemoveItem(int index)

    {

        OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Remove, this[index], index));

        base.RemoveItem(index);

    }

    protected override void ClearItems()

    {

        OnCollectionChanging(new NotifyCollectionChangedEventArgs(Items.ToList()));

        base.ClearItems();

    }

    private void RaiseCollectionChangingEvent(NotifyCollectionChangedEventArgs e)

    {

        if (!e.IsChangingEvent) throw new ArgumentException($"'{nameof(e)}' must have the IsChangingProperty set to true.");

        CollectionChanging(this, e);

    }

    protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)

    {

        if (CollectionChanging != null)

            RaiseCollectionChangingEvent(e);

    }
}
}
