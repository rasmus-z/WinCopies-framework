/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
