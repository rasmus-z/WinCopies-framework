using System.Windows;

namespace WinCopies.GUI.Explorer
{
    public class ListView : WinCopies.GUI.Controls.ListView
    {

        public ExplorerControl ParentExplorerControl { get; internal set;     } = null;    

        //public override void OnApplyTemplate()

        //{

        //    base.OnApplyTemplate();



        //}    

        protected override DependencyObject GetContainerForItemOverride() => new ListViewItem(ParentExplorerControl);

    }
}
