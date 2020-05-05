using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlTreeViewItem : TreeViewItem, ICommandSource
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ExplorerControlTreeViewItem));

        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ExplorerControlTreeViewItem));

        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(ExplorerControlTreeViewItem));

        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }

        static ExplorerControlTreeViewItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControlTreeViewItem), new FrameworkPropertyMetadata(typeof(ExplorerControlTreeViewItem)));

        protected override DependencyObject GetContainerForItemOverride() => new ExplorerControlTreeViewItem();
    }
}
