using System.Collections.ObjectModel;
using System.ComponentModel;
using WinCopies.IO;

namespace WinCopies.GUI.Explorer
{
    /// <summary>
    /// Provides properties to interact with graphical <see cref="WinCopies.IO.Explorer.IBrowsableObjectInfo"/> objects.
    /// </summary>
    public interface IBrowsableObjectInfo : IO.IBrowsableObjectInfo
    {

        /// <summary>
        /// Gets or sets a value that indicates whether this <see cref="Explorer.IBrowsableObjectInfo"/> is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the selected item of this <see cref="Explorer.IBrowsableObjectInfo"/>.
        /// </summary>
        Explorer.IBrowsableObjectInfo SelectedItem { get; set; }

        /// <summary>
        /// Gets a collection that represents the selected items of this <see cref="Explorer.IBrowsableObjectInfo"/>.
        /// </summary>
        ReadOnlyObservableCollection<Explorer.IBrowsableObjectInfo> SelectedItems { get; }

        bool IsCheckBoxEnabled { get; set; }

    }

    public interface IBrowsableObjectInfoViewModel : Explorer.IBrowsableObjectInfo, INotifyPropertyChanged
    {

        IBrowsableObjectInfo Model { get; }

    }

    internal interface IBrowsableObjectInfoInternal
    {

        ObservableCollection<Explorer.IBrowsableObjectInfo> SelectedItems { get; set; }

    }

    internal interface IBrowsableObjectInfoHelper
    {

        ReadOnlyObservableCollection<Explorer.IBrowsableObjectInfo> SelectedItems { set; }

    }
}
