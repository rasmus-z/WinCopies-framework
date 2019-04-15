using System.Collections.ObjectModel;

namespace WinCopies.GUI.Explorer
{
    /// <summary>
    /// Provides properties to interact with graphical <see cref="WinCopies.IO.IBrowsableObjectInfo"/> objects.
    /// </summary>
    public interface IBrowsableObjectInfo : WinCopies.IO.IBrowsableObjectInfo
    {

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="IBrowsableObjectInfo"/> is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the selected item of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        IBrowsableObjectInfo SelectedItem { get; set; }

        /// <summary>
        /// Gets a collection that represents the selected items of this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        ReadOnlyObservableCollection<IBrowsableObjectInfo> SelectedItems { get; }

        bool IsCheckBoxEnabled { get; set; }

    }

    internal interface IBrowsableObjectInfoInternal
    {

        ObservableCollection<IBrowsableObjectInfo> SelectedItems { get; set; }

    }

    internal interface IBrowsableObjectInfoHelper
    {

        ReadOnlyObservableCollection<IBrowsableObjectInfo> SelectedItems { set; }

    }
}
