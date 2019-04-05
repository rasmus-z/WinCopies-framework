using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Controls
{
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
}
