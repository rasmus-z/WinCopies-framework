using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WinCopies.Collections;

namespace WinCopies.GUI.Explorer
{
    public class SelectionChangedEventArgs : RoutedEventArgs
    {
        public ActionsFromObjects SelectionSource { get; private set; }

        public IReadOnlyList AddedItems { get; private set; }

        public IReadOnlyList RemovedItems { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs"/> class. This constructor will always create a <see cref="ReadOnlyArrayList"/> wrapper. To use directly a <see cref="IReadOnlyList"/>, see the <see cref="SelectionChangedEventArgs(AccessibilitySwitches)"/> constructor.
        /// </summary>
        /// <param name="selectionSource">The source of the selection change</param>
        /// <param name="addedItems">The added items</param>
        /// <param name="removedItems">The removed items.</param>
        public SelectionChangedEventArgs(ActionsFromObjects selectionSource, IList addedItems, IList removedItems) : this(selectionSource, new ReadOnlyArrayList(addedItems), new ReadOnlyArrayList(removedItems)) { }

        public SelectionChangedEventArgs(RoutedEvent routedEvent, ActionsFromObjects selectionSource, IList addedItems, IList removedItems) : this(routedEvent, selectionSource, new ReadOnlyArrayList(addedItems), new ReadOnlyArrayList(removedItems)) { }

        public SelectionChangedEventArgs(RoutedEvent routedEvent, object source, ActionsFromObjects selectionSource, IList addedItems, IList removedItems) : this(routedEvent, source, selectionSource, new ReadOnlyArrayList(addedItems), new ReadOnlyArrayList(removedItems)) { }

        public SelectionChangedEventArgs(ActionsFromObjects selectionSource, IReadOnlyList addedItems, IReadOnlyList removedItems) => Init(selectionSource, addedItems, removedItems);

        public SelectionChangedEventArgs(RoutedEvent routedEvent, ActionsFromObjects selectionSource, IReadOnlyList addedItems, IReadOnlyList removedItems) : base(routedEvent) => Init(selectionSource, addedItems, removedItems);

        public SelectionChangedEventArgs(RoutedEvent routedEvent, object source, ActionsFromObjects selectionSource, IReadOnlyList addedItems, IReadOnlyList removedItems) : base(routedEvent, source) => Init(selectionSource, addedItems, removedItems);

        private void Init(ActionsFromObjects selectionSource, IReadOnlyList addedItems, IReadOnlyList removedItems)
        {

            SelectionSource = selectionSource;

            AddedItems = addedItems;

            RemovedItems = removedItems;

        }

    }

    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
}
