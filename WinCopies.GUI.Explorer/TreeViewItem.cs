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

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {

            base.OnMouseDown(e);

            //MessageBox.Show("");



            CaptureMouse();

        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {

            base.OnMouseUp(e);

            if (IsMouseCaptured)

            {

                ReleaseMouseCapture();

                // if (IsMouseOver && ParentExplorerControl.OpenMode == OpenMode.OnFirstClick)

                    // TryExecuteCommand();

            }

        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {

            base.OnKeyUp(e);

            // if (e.Key == Key.Enter && !IsMouseCaptured)

                // TryExecuteCommand();

        }

        //private void TryExecuteCommand() => ParentExplorerControl.Command?.TryExecute(ParentExplorerControl.CommandParameter ?? this.GetParent<ListView>(false).SelectedItems, ParentExplorerControl.CommandTarget);

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {

            base.OnPreviewMouseDoubleClick(e);

            // if (ParentExplorerControl.OpenMode == OpenMode.OnDoubleClick)

                // TryExecuteCommand();

        }

    }
}
