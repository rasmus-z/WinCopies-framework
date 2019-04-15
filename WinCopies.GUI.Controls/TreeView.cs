using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.GUI.Controls
{
    public class TreeView : System.Windows.Controls.TreeView, ISelector
    {

        public int SelectedIndex => ItemsSource == null ? Items.IndexOf(SelectedItem) : ItemsSource.ToList().IndexOf(SelectedItem);

    }
}
