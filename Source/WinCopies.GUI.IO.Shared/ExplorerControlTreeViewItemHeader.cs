using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlTreeViewItemHeader : Control
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ExplorerControlTreeViewItemHeader));

        public ImageSource Icon { get => (ImageSource)GetValue(IconProperty); set => SetValue(IconProperty, value); }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(nameof(ItemName), typeof(string), typeof(ExplorerControlTreeViewItemHeader));

        public string ItemName { get => (string)GetValue(ItemNameProperty); set => SetValue(ItemNameProperty, value); }

        static ExplorerControlTreeViewItemHeader() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControlTreeViewItemHeader), new FrameworkPropertyMetadata(typeof(ExplorerControlTreeViewItemHeader)));
    }
}
