using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlListViewItemContent : Control
    {
        public static readonly DependencyProperty IsCheckBoxVisibleProperty = DependencyProperty.Register(nameof(IsCheckBoxVisible), typeof(bool), typeof(ExplorerControlListViewItemContent));

        public bool IsCheckBoxVisible { get => (bool)GetValue(IsCheckBoxVisibleProperty); set => SetValue(IsCheckBoxVisibleProperty, value); }

        public static readonly DependencyProperty SmallIconProperty = DependencyProperty.Register(nameof(SmallIcon), typeof(ImageSource), typeof(ExplorerControlListViewItemContent));

        public ImageSource SmallIcon { get => (ImageSource)GetValue(SmallIconProperty); set => SetValue(SmallIconProperty, value); }

        public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof(LargeIcon), typeof(ImageSource), typeof(ExplorerControlListViewItemContent));

        public ImageSource LargeIcon { get => (ImageSource)GetValue(LargeIconProperty); set => SetValue(LargeIconProperty, value); }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(nameof(ItemName), typeof(string), typeof(ExplorerControlListViewItemContent));

        public ImageSource ItemName { get => (ImageSource)GetValue(ItemNameProperty); set => SetValue(ItemNameProperty, value); }

        //public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(nameof(Properties), typeof(IEnumerable<KeyValuePair<string, string>>), typeof(ExplorerControlListViewItemContent));

        //public IEnumerable<KeyValuePair<string, string>> Properties { get => (IEnumerable<KeyValuePair<string, string>>)GetValue(PropertiesProperty); set => SetValue(PropertiesProperty, value); }

        public static readonly DependencyProperty HasTransparencyProperty = DependencyProperty.Register(nameof(HasTransparency), typeof(bool), typeof(ExplorerControlListViewItemContent));

        public bool HasTransparency { get => (bool)GetValue(HasTransparencyProperty); set => SetValue(HasTransparencyProperty, value); }

        static ExplorerControlListViewItemContent() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControlListViewItemContent), new FrameworkPropertyMetadata(typeof(ExplorerControlListViewItemContent)));
    }
}
