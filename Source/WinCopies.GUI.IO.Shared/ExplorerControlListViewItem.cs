using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCopies.GUI.IO
{
public    class ExplorerControlListViewItem:ListViewItem
    {
        public static readonly DependencyProperty IsCheckBoxEnabledProperty = DependencyProperty.Register(nameof(IsCheckBoxEnabled), typeof(bool), typeof(ExplorerControlListViewItem));

        public bool IsCheckBoxEnabled { get => (bool)GetValue(IsCheckBoxEnabledProperty); set => SetValue(IsCheckBoxEnabledProperty, value); }

        public static readonly DependencyProperty SmallIconProperty = DependencyProperty.Register(nameof(SmallIcon), typeof(ImageSource), typeof(ExplorerControlListViewItem));

        public ImageSource SmallIcon { get => (ImageSource)GetValue(SmallIconProperty); set => SetValue(SmallIconProperty, value); }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(nameof(ItemName), typeof(string), typeof(ExplorerControlListViewItem));

        public ImageSource ItemName { get => (ImageSource)GetValue(ItemNameProperty); set => SetValue(ItemNameProperty, value); }

        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(nameof(Properties), typeof(IEnumerable<KeyValuePair<string, string>>), typeof(ExplorerControlListViewItem));

        public IEnumerable<KeyValuePair<string, string>> Properties { get => (IEnumerable<KeyValuePair<string, string>>)GetValue(PropertiesProperty); set => SetValue(PropertiesProperty, value); }

        public static readonly DependencyProperty HasTransparencyProperty = DependencyProperty.Register(nameof(HasTransparency), typeof(bool), typeof(ExplorerControlListViewItem));

        public bool HasTransparency { get => (bool)GetValue(HasTransparencyProperty); set => SetValue(HasTransparencyProperty, value); }
    }
}
