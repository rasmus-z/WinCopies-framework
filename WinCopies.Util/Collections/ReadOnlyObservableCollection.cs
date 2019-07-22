using WinCopies.Util;

namespace WinCopies.Collections
{
    public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, INotifyCollectionChanging
    {

        protected virtual event NotifyCollectionChangingEventHandler CollectionChanging;

        event NotifyCollectionChangingEventHandler INotifyCollectionChanging.CollectionChanging
        {
            add => this.CollectionChanging += value;

            remove => this.CollectionChanging -= value;
        }

        public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanging(e);

        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);
    }
}
