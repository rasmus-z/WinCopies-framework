

// todo: to add descriptive text for commands:

using System.Windows.Input;

namespace WinCopies.GUI.Windows.Dialogs
{

    public static class Commands

    {

        public static RoutedUICommand SelectPath { get; } =     new RoutedUICommand(nameof(SelectPath), nameof(SelectPath), typeof(Commands));

    }

}