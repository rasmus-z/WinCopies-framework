using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlListViewItem : ListViewItem, ICommandSource
    {

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ExplorerControlListViewItem));

        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ExplorerControlListViewItem));

        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(ExplorerControlListViewItem));

        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }

        //static ExplorerControlListViewItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControlListViewItem), new FrameworkPropertyMetadata(typeof(ExplorerControlListViewItem)));

        private bool TryExecuteCommand() => CommandTarget == null ? Command.TryExecute(CommandParameter) : Command.TryExecute(CommandParameter, CommandTarget);

        /// <summary>
        /// Raises the <see cref="Control.MouseDoubleClick"/> routed event, tries to execute the command and, if succeeded, handles the event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (TryExecuteCommand())

                e.Handled = true;
        }

        /// <summary>
        /// Invoked when an unhandled System.Windows.Input.Keyboard.KeyDown attached event reaches an element in its route that is derived from this class. If the <see cref="KeyEventArgs.Key"/> property of <paramref name="e"/> is defined to <see cref="Key.Enter"/>, tries to execute the command and, if succeeded, handles the event. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter && TryExecuteCommand())

                e.Handled = true;
        }

    }
}
