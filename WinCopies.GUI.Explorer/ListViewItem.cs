using System.Windows;

namespace WinCopies.GUI.Explorer
{
    public class ListViewItem : System.Windows.Controls.ListViewItem
    {

        public ExplorerControl ParentExplorerControl { get; } = null;

        public ListViewItem(ExplorerControl parentExplorerControl)
        {

            ParentExplorerControl = parentExplorerControl;    

        }

        static ListViewItem() => System.Windows.Controls.ListViewItem.VisibilityProperty.OverrideMetadata(typeof(ListViewItem), new System.Windows.PropertyMetadata(System.Windows.Controls.ListViewItem.VisibilityProperty.DefaultMetadata.DefaultValue, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>

                                 {

                                     System.Windows.Controls.ListViewItem.VisibilityProperty.DefaultMetadata.PropertyChangedCallback?.Invoke(d, e);

                                     ((ListViewItem)d).ParentExplorerControl.Value_IsVisibleChanged(d, e);

                                 }));

    }
}
