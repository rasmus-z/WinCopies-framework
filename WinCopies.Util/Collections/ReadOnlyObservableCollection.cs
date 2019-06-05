using WinCopies.Util;

namespace WinCopies.Collections
{
    public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, INotifyCollectionChanging
    {

        public event NotifyCollectionChangingEventHandler CollectionChanging;

        public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);
    }
}
