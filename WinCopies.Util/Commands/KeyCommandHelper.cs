using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.Util.Commands
{
    public static class KeyCommandHelper
    {

        public static bool CanRaiseCommand(ICommandSource commandSource, KeyEventArgs e) => CanRaiseCommand(commandSource.Command, commandSource.CommandParameter, commandSource.CommandTarget, e);

        public static bool CanRaiseCommand(ICommand command, object commandParameter, IInputElement commandTarget, KeyEventArgs e)

        {

            if (command is RoutedCommand routedCommand)

            {

                if (routedCommand.InputGestures == null) return false;

                foreach (object inputGesture in routedCommand.InputGestures)

                    if (inputGesture is KeyGesture keyGesture && e.Key == keyGesture.Key && e.KeyboardDevice.Modifiers == keyGesture.Modifiers)

                        return command.CanExecute(commandParameter, commandTarget);

            }

            return false;

        }

    }
}
