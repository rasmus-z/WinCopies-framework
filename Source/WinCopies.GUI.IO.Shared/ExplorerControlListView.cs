using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.IO
{
    public enum ViewStyle
    {

        SizeOne = 0,

        SizeTwo = 1,

        SizeThree = 2,

        SizeFour = 3,

        List = 4,

        Details = 5,

        Tiles = 6,

        Content = 7

    }

    public    class ExplorerControlListView:ListView
    {
        public static readonly DependencyProperty ViewStyleProperty = DependencyProperty.Register(nameof(ViewStyle), typeof(ViewStyle), typeof(ExplorerControlListView));

        public ViewStyle ViewStyle { get => (ViewStyle)GetValue(ViewStyleProperty); set => SetValue(ViewStyleProperty, value); }
    }
}
