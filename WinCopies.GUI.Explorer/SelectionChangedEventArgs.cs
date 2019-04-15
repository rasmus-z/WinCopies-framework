using System.Collections;
using System.Windows;

namespace WinCopies.GUI.Explorer
{
    public class SelectionChangedEventArgs : System.Windows.Controls.SelectionChangedEventArgs
    {
        private ActionsFromObjects SelectionSource { get; }

        private SelectionChangedEventArgs(RoutedEvent id, IList addedItems, IList removedItems) : base(id, addedItems, removedItems) { }

        public SelectionChangedEventArgs(RoutedEvent id, IList addedItems, IList removedItems, ActionsFromObjects selectionSource) : base(id, addedItems, removedItems) => SelectionSource = selectionSource;

    }

    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}
