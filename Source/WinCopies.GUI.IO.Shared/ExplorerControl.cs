using System;
using System.Collections;
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

        //public static readonly DependencyProperty TreeViewItemTemplateProperty = DependencyProperty.Register(nameof(TreeViewItemTemplate), typeof(DataTemplate), typeof(ExplorerControl));

        //public DataTemplate TreeViewItemTemplate { get => (DataTemplate)GetValue(TreeViewItemTemplateProperty); set => SetValue(TreeViewItemTemplateProperty, value); }

        //public static readonly DependencyProperty ListViewItemTemplateProperty = DependencyProperty.Register(nameof(ListViewItemTemplate), typeof(DataTemplate), typeof(ExplorerControl));

        //public DataTemplate ListViewItemTemplate { get => (DataTemplate)GetValue(ListViewItemTemplateProperty); set => SetValue(ListViewItemTemplateProperty, value); }

        //public static readonly DependencyProperty TreeViewItemsProperty = DependencyProperty.Register(nameof(TreeViewItems), typeof(IEnumerable), typeof(ExplorerControl));

        //public IEnumerable TreeViewItems { get => (IEnumerable)GetValue(TreeViewItemsProperty); set => SetValue(TreeViewItemsProperty, value); }

        //public static readonly DependencyProperty ListViewItemsProperty = DependencyProperty.Register(nameof(ListViewItems), typeof(IEnumerable), typeof(ExplorerControl));

        //public IEnumerable ListViewItems { get => (IEnumerable)GetValue(ListViewItemsProperty); set => SetValue(ListViewItemsProperty, value); }

        static ExplorerControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControl), new FrameworkPropertyMetadata(typeof(ExplorerControl)));
    }
}
