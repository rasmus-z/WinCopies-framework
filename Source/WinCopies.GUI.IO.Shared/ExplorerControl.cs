using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.IO
{
    public class ExplorerControl : Control
    {

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(ExplorerControl));

        public string Path { get => (string)GetValue(PathProperty); set => SetValue(PathProperty, value); }

        public static readonly DependencyProperty TreeViewStyleProperty = DependencyProperty.Register(nameof(TreeViewStyle), typeof(Style), typeof(ExplorerControl));

        public Style TreeViewStyle { get => (Style)GetValue(TreeViewStyleProperty); set => SetValue(TreeViewStyleProperty, value); }

        public static readonly DependencyProperty ListViewStyleProperty = DependencyProperty.Register(nameof(ListViewStyle), typeof(Style), typeof(ExplorerControl));

        public Style ListViewStyle { get => (Style)GetValue(ListViewStyleProperty); set => SetValue(ListViewStyleProperty, value); }

        static ExplorerControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControl), new FrameworkPropertyMetadata(typeof(ExplorerControl)));
    }
}
