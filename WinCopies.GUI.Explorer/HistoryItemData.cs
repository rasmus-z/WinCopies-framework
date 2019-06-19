using System.Collections.Generic;

namespace WinCopies.GUI.Explorer
{
    public struct ScrollViewerOffset
    {

        public double HorizontalOffset { get; }

        public double VerticalOffset { get; }

        public ScrollViewerOffset(double horizontalOffset, double verticalOffset)

        {

            HorizontalOffset = horizontalOffset;

            VerticalOffset = verticalOffset;

        }

    }

    /// <summary>
    /// Provides properties to interact with <see cref="ExplorerControl"/> history items.
    /// </summary>
    public interface IHistoryItemData

    {

        /// <summary>
        /// Gets the header of this history item.
        /// </summary>
        string Header { get; }

        /// <summary>
        /// Gets a value that indicates whether this history item is an empty history item. This property can be used to notify the user that the history is empty.
        /// </summary>
        bool IsEmptyHistoryUserIndication { get; }

    }

    /// <summary>
    /// Represents an empty history item. This struct can be used to notify the user that the history is empty.
    /// </summary>
    public struct HistoryItemDataEmptyHistoryUserIndication : IHistoryItemData

    {

        // todo:

        /// <summary>
        /// Gets the header of this history item.
        /// </summary>
        public string Header => "(Empty)";

        /// <summary>
        /// Gets a value that indicates whether this history item is an empty history item. When implemented in this struct, this property is always <see langword="true"/>.
        /// </summary>
        public bool IsEmptyHistoryUserIndication => true;

    }

    /// <summary>
    /// Represents an history item.
    /// </summary>
    public struct HistoryItemData : IHistoryItemData
    {

        /// <summary>
        /// Gets the header of this history item.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Gets a value that indicates whether this history item is an empty history item. When implemented in this struct, this property is always <see langword="false"/>.
        /// </summary>
        public bool IsEmptyHistoryUserIndication => false;

        /// <summary>
        /// Gets the path of this history item.
        /// </summary>
        public IO.IBrowsableObjectInfo Path { get; }

        /// <summary>
        /// Gets the <see cref="WinCopies.GUI.Explorer. ScrollViewerOffset"/> of this history item.
        /// </summary>
        public ScrollViewerOffset ScrollViewerOffset { get; }

        /// <summary>
        /// Gets the selected items of this history item.
        /// </summary>
        public IEnumerable<Explorer.IBrowsableObjectInfo> SelectedItems { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryItemData"/> class.
        /// </summary>
        /// <param name="header">The header of this history item.</param>
        /// <param name="path">The path of this history item.</param>
        /// <param name="scrollViewerOffset">The <see cref="WinCopies.GUI.Explorer. ScrollViewerOffset"/> of this history item.</param>
        /// <param name="selectedItems">The selected items of this history item.</param>
        public HistoryItemData(string header, IO.IBrowsableObjectInfo path, ScrollViewerOffset scrollViewerOffset, IEnumerable<Explorer.IBrowsableObjectInfo> selectedItems)

        {

            Header = header;

            Path = path;

            ScrollViewerOffset = scrollViewerOffset;

            SelectedItems = selectedItems;

        }
    }
}
