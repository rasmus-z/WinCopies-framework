using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.Controls
{
    public static class KeyDownCommandHelper
    {

        public static bool CanRaiseCommand(ICommandSource commandSource, KeyEventArgs e)

        {

            if (commandSource.Command is RoutedCommand routedCommand)

            {

                if (routedCommand.InputGestures == null) return false;

                foreach (object inputGesture in routedCommand.InputGestures)

                    if (inputGesture is KeyGesture keyGesture && e.Key == keyGesture.Key && e.KeyboardDevice.Modifiers == keyGesture.Modifiers)

                        return commandSource.Command.CanExecute(commandSource.CommandParameter, commandSource.CommandTarget);

            }

            return false;

        }

    }
}
