using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WinCopies.Util.Generic;

namespace WinCopies.Util.Commands
{

    /// <summary>
    /// Provides some standard commands for application commands.
    /// </summary>
    public static class ApplicationCommands
    {

        /// <summary>
        /// Gets the <b>NewTab</b> command.
        /// </summary>
        public static RoutedUICommand NewTab { get; } = new RoutedUICommand(Generic.NewTab, nameof(NewTab), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>NewWindow</b> command.
        /// </summary>
        public static RoutedUICommand NewWindow { get; } = new RoutedUICommand(Generic.NewWindow, nameof(NewWindow), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>NewWindowInNewInstance</b> command.
        /// </summary>
        public static RoutedUICommand NewWindowInNewInstance { get; } = new RoutedUICommand(Generic.NewWindowInNewInstance, nameof(NewWindowInNewInstance), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

        /// <summary>
        /// Gets the <b>CloseTab</b> command.
        /// </summary>
        public static RoutedUICommand CloseTab { get; } = new RoutedUICommand(Generic.CloseTab, nameof(CloseTab), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>CloseAllTabs</b> command.
        /// </summary>
        public static RoutedUICommand CloseAllTabs { get; } = new RoutedUICommand(Generic.CloseAllTabs, nameof(CloseAllTabs), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>CloseWindow</b> command.
        /// </summary>
        public static RoutedUICommand CloseWindow { get; } = new RoutedUICommand(Generic.CloseWindow, nameof(CloseWindow), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>DeselectAll</b> command.
        /// </summary>
        public static RoutedUICommand DeselectAll { get; } = new RoutedUICommand(Generic.DeselectAll, nameof(DeselectAll), typeof(System.Windows.Input.ApplicationCommands));

        /// <summary>
        /// Gets the <b>ReverseSelection</b> command.
        /// </summary>
        public static RoutedUICommand ReverseSelection { get; } = new RoutedUICommand(Generic.ReverseSelection, nameof(ReverseSelection), typeof(System.Windows.Input.ApplicationCommands));

    }
}
