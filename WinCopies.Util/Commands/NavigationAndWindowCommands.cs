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
    /// Provides some standard commands for navigation inside windows.
    /// </summary>
    public static class NavigationAndWindowCommands
    {

        /// <summary>
        /// Gets a value that represents the new tab command.
        /// </summary>
        public static RoutedUICommand NewTab { get; } = new RoutedUICommand(Generic.NewTab, nameof(NewTab), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control) });

        /// <summary>
        /// Gets a value that represents the new window command.
        /// </summary>
        public static RoutedUICommand NewWindow { get; } = new RoutedUICommand(Generic.NewWindow, nameof(NewWindow), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Alt) });

        /// <summary>
        /// Gets a value that represents the new window in new instance command.
        /// </summary>
        public static RoutedUICommand NewWindowInNewInstance { get; } = new RoutedUICommand(Generic.NewWindowInNewInstance, nameof(NewWindowInNewInstance), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

        /// <summary>
        /// Gets a value that represents the close tab command.
        /// </summary>
        public static RoutedUICommand CloseTab { get; } = new RoutedUICommand(Generic.CloseTab, nameof(CloseTab), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        /// <summary>
        /// Gets a value that represents the close all tabs command.
        /// </summary>
        public static RoutedUICommand CloseAllTabs { get; } = new RoutedUICommand(Generic.CloseAllTabs, nameof(CloseAllTabs), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets a value that represents the close window command.
        /// </summary>
        public static RoutedUICommand CloseWindow { get; } = new RoutedUICommand(Generic.CloseWindow, nameof(CloseWindow), typeof(NavigationAndWindowCommands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });

    }
}
