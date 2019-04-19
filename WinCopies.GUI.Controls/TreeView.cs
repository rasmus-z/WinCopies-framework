using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.Controls
{
    public class TreeView : System.Windows.Controls.TreeView, ISelector, ICommandSource
    {

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TreeView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command of this <see cref="TreeView"/>. This is a dependency property.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(TreeView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="Command"/> property. This is a dependency property.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(TreeView), new PropertyMetadata(null));

        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }

        public int SelectedIndex => ItemsSource == null ? Items.IndexOf(SelectedItem) : ItemsSource.ToList().IndexOf(SelectedItem);

        public TreeView()
        {
            MouseDoubleClick += TreeView_MouseDoubleClick;

            KeyDown += TreeView_KeyDown;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Command == null) return;

            Command.TryExecute(CommandParameter, CommandTarget);
        }

        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {

            if (Command == null) return;

            KeyDownCommandHelper.TryRaiseCommand(this, e);

        }

    }
}
