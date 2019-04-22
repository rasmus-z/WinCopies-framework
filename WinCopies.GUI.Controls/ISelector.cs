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

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        int SelectedIndex { get; }

    }

    /// <summary>
    /// Provides properties that apply to the UI controls that can select one or more children by the item itself or by the parent selector.
    /// </summary>
    public interface ISettableSelector : ISelector
    {

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        new object SelectedItem { set; }

        /// <summary>
        /// Sets the selected index.
        /// </summary>
        new int SelectedIndex { set; }

    }

    /// <summary>
    /// Provides properties that apply to the UI controls that can be selected.
    /// </summary>
    public interface ISelectable

    {

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
