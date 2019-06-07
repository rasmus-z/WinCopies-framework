using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.Explorer
{
    public class TreeViewItem : WinCopies.GUI.Controls.TreeViewItem
    {

        public ExplorerControl ParentExplorerControl { get; } = null;

        public TreeViewItem(ExplorerControl parentExplorerControl) => ParentExplorerControl = parentExplorerControl;

    }
}
