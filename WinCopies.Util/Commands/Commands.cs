using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinCopies.Util.Commands
{
    public static class Commands
    {

        public static RoutedCommand CommonCommand { get; } = new RoutedCommand(nameof(CommonCommand), typeof(Commands));

        /// <summary>
        /// A static <see cref="System.Windows.Input.CanExecuteRoutedEventHandler"/> that sets the <see cref="CanExecuteRoutedEventArgs.CanExecute"/> to true. This handler can be used for commands that can always be executed.
        /// </summary>
        public static CanExecuteRoutedEventHandler CanExecuteRoutedEventHandler { get; } = (object sender, CanExecuteRoutedEventArgs e) => { e.CanExecute = true; };

    }
}
