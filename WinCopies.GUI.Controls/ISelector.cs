using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Controls
{

    // todo: to add this interface to the other FrameworkElements to which they applies.

    /// <summary>
    /// Provides properties that apply to the UI controls that can select one or more children.
    /// </summary>
    public interface ISelector
    {

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        object SelectedItem { get; }

    }

    public interface ISingleSelector : ISelector
    {

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        int SelectedIndex { get; }

    }

    /// <summary>
    /// Provides properties that apply to the UI controls that can select one or more children by the item itself or by the parent selector.
    /// </summary>
    public interface ISettableSelector
    {

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        object SelectedItem { get; set; }

    }

    public interface ISingleSettableSelector : ISettableSelector
    {

        /// <summary>
        /// Sets the selected index.
        /// </summary>
        int SelectedIndex { get; set; }

    }

    // todo: to implement this interface in the other selectable controls 'listbox, listview, ...)

    /// <summary>
    /// Provides properties that apply to the UI controls that can be selected.
    /// </summary>
    public interface ISelectable

    {

        bool IsSelected { get; set; }

        /// <summary>
        /// Gets a value that indicates whether this item is selected by focus.
        /// </summary>
        bool IsFocusSelection { get; }

        /// <summary>
        /// Gets a value that indicates whether this item was selected before a new selection by focus.
        /// </summary>
        bool IsPreviouslySelectedItem { get; }

    }
}
