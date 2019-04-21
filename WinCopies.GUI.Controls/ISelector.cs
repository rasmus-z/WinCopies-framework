using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Controls
{

    // todo: to add this interface to the other FrameworkElements to which they applies.

    public interface ISelector
    {

        object SelectedItem { get; }

        int SelectedIndex { get; }

    }

    public interface ISettableSelector : ISelector
    {

        object SelectedItem { set; }

        int SelectedIndex { set; }

    }

    public interface ISelectable

    {

        bool IsFocusSelection { get; }

    }
}
