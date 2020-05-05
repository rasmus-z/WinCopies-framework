using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlTreeView:TreeView
    {
        protected override DependencyObject GetContainerForItemOverride() => new ExplorerControlTreeViewItem();
    }
}
