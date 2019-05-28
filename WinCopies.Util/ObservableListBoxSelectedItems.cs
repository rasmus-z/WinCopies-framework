using System.Collections.Specialized;
using System.Windows.Controls;

namespace WinCopies.Collections
{

    // todo: use PushBinding

    /// <summary>
    /// Allows the user to be notified when the selection of a <see cref="System.Windows.Controls.ListBox"/> has changed.
    /// </summary>
    public class ObservableListBoxSelectedItems : INotifyCollectionChanged
    {

        public ListBox ListBox { get; } = null;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableListBoxSelectedItems(ListBox listBox)

        {

            this.ListBox = listBox;

            listBox.SelectionChanged += ListBox_SelectionChanged;

        }

        private protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)

        {

            if (e.AddedItems.Count > 0)    

                CollectionChanged?.Invoke(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.AddedItems));    

            if (e.RemovedItems.Count > 0)    

                CollectionChanged?.Invoke(this, new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.RemovedItems));    

        }

        //private protected void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => OnSelectionChanged(e);

        //public IEnumerator GetEnumerator() => ListBox.SelectedItems.GetEnumerator();

        //public void CopyTo(Array array, int index) => ListBox.SelectedItems.CopyTo(array, index);

    }
}
