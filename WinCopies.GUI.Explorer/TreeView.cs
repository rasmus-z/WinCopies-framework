using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util.Commands;

namespace WinCopies.GUI.Explorer
{
    public class TreeView : WinCopies.GUI.Controls.TreeView
    {

        internal ExplorerControl ParentExplorerControl { get; set; }

        internal void TryRaiseCommandsByKeyDown(KeyEventArgs e)
        {

            ICommand[] commands = new ICommand[] { ApplicationCommands.Copy, ApplicationCommands.Cut, ApplicationCommands.Paste, ApplicationCommands.Delete };

            foreach (ICommand command in commands)

                if (KeyCommandHelper.CanRaiseCommand(command, ActionsFromObjects.TreeView, ParentExplorerControl, e))

                    command.Execute(ActionsFromObjects.TreeView);

        }
    }
}
