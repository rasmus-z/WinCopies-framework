using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlTreeViewItemContent : Control
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ExplorerControlTreeViewItemContent));

        public ImageSource Icon { get => (ImageSource)GetValue(IconProperty); set => SetValue(IconProperty, value); }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(nameof(ItemName), typeof(string), typeof(ExplorerControlTreeViewItemContent));

        public string ItemName { get => (string)GetValue(ItemNameProperty); set => SetValue(ItemNameProperty, value); }

        static ExplorerControlTreeViewItemContent() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControlTreeViewItemContent), new FrameworkPropertyMetadata(typeof(ExplorerControlTreeViewItemContent)));
    }
}
