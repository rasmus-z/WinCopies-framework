using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WinCopies.Util;

namespace WinCopies.Collections
{

    public interface IReadOnlyObservableCollection<T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyCollectionChanging

    {



    }

    public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, IReadOnlyObservableCollection<T>
    {

        protected virtual event NotifyCollectionChangingEventHandler CollectionChanging;

        event NotifyCollectionChangingEventHandler INotifyCollectionChanging.CollectionChanging
        {
            add => CollectionChanging += value;

            remove => CollectionChanging -= value;
        }

        public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanging(e);

        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);

        void IReadOnlyList<T>.Clear() => ((IReadOnlyList<T>)this).Clear();

        void IReadOnlyList<T>.RemoveAt(int index) => ((IReadOnlyList<T>)this).RemoveAt(index);

        T IReadOnlyList<T>.this[int index] { get => this[index]; set => ((IReadOnlyList<T>)this)[index] = value; }

    }
}
