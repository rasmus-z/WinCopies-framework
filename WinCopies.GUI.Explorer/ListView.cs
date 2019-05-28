using System.Windows;
using System.Windows.Input;
using WinCopies.Util.Commands;

namespace WinCopies.GUI.Explorer
{
    public class ListView : Controls.ListView
    {



        public ExplorerControl ParentExplorerControl { get; internal set; } = null;

        //public override void OnApplyTemplate()

        //{

        //    base.OnApplyTemplate();



        //}    

        public ListView()

        {

            // InputBindings.Add(new InputBinding(Commands.OpenOnLeftClick, new MouseGesture(MouseAction.LeftClick)));

        }

        protected override DependencyObject GetContainerForItemOverride()
        {

            ListViewItem listViewItem = new ListViewItem(ParentExplorerControl);

            bool alreadyFoundCommandBinding = false;

            //foreach (object commandBinding in listViewItem.CommandBindings)

            //    if (((CommandBinding)commandBinding).Command == Commands.Open)

            //    {

            //        alreadyFoundCommandBinding = true;

            //        break;

            //    }

            //if (!alreadyFoundCommandBinding)

            foreach (object inputBinding in listViewItem.InputBindings)

                if (((InputBinding)inputBinding).Command == Commands.Open)

                {

                    alreadyFoundCommandBinding = true;

                    break;

                }

            // listViewItem.CommandBindings.Add(new CommandBinding(Commands.Open, ParentExplorerControl.Open_Executed, ParentExplorerControl.Open_CanExecute));

            if (!alreadyFoundCommandBinding)

                if (ParentExplorerControl.OpenMode == OpenMode.OnFirstClick)

                    listViewItem.InputBindings.Add(new InputBinding(Commands.Open, new MouseGesture(MouseAction.LeftClick)));

                else if (ParentExplorerControl.OpenMode == OpenMode.OnDoubleClick)

                    listViewItem.InputBindings.Add(new InputBinding(Commands.Open, new MouseGesture(MouseAction.LeftDoubleClick)));

            return listViewItem;

        }

        //internal void TryRaiseCommandsByKeyDown(KeyEventArgs e)
        //{

        //    ICommand[] commands = new ICommand[] { ApplicationCommands.Copy, ApplicationCommands.Cut, ApplicationCommands.Paste, FileSystemCommands.Rename, ApplicationCommands.Delete, FileSystemCommands.DeletePermanently };

        //    foreach (ICommand command in commands)

        //        if (KeyCommandHelper.CanRaiseCommand(command, ActionsFromObjects.ListView, ParentExplorerControl, e))

        //            command.Execute(ActionsFromObjects.ListView);

        //}

    }
}
