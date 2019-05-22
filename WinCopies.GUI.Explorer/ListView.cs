using System.Windows;
using System.Windows.Input;
using WinCopies.Util.Commands;

namespace WinCopies.GUI.Explorer
{
    public class ListView : WinCopies.GUI.Controls.ListView
    {

        public ExplorerControl ParentExplorerControl { get; internal set; } = null;

        //public override void OnApplyTemplate()

        //{

        //    base.OnApplyTemplate();



        //}    

        protected override DependencyObject GetContainerForItemOverride() => new ListViewItem(ParentExplorerControl);

        //internal void TryRaiseCommandsByKeyDown(KeyEventArgs e)
        //{

        //    ICommand[] commands = new ICommand[] { ApplicationCommands.Copy, ApplicationCommands.Cut, ApplicationCommands.Paste, FileSystemCommands.Rename, ApplicationCommands.Delete, FileSystemCommands.PermanentlyDelete };

        //    foreach (ICommand command in commands)

        //        if (KeyCommandHelper.CanRaiseCommand(command, ActionsFromObjects.ListView, ParentExplorerControl, e))

        //            command.Execute(ActionsFromObjects.ListView);

        //}

    }
}
