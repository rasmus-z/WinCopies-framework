﻿/* Copyright © Pierre Sprimont, 2019
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

using Microsoft.WindowsAPICodePack.Shell;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WinCopies.GUI.IO;
using WinCopies.IO;

namespace WinCopies.GUI.Samples
{
    /// <summary>
    /// Interaction logic for ExplorerControlTest.xaml
    /// </summary>
    public partial class ExplorerControlTest : Window
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IEnumerable<IExplorerControlBrowsableObjectInfoViewModel>), typeof(ExplorerControlTest));

        public IEnumerable<IExplorerControlBrowsableObjectInfoViewModel> Items { get => (IEnumerable<IExplorerControlBrowsableObjectInfoViewModel>)GetValue(ItemsProperty); set => SetValue(ItemsProperty, value); }

        public ExplorerControlTest()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Items = new ObservableCollection<IExplorerControlBrowsableObjectInfoViewModel>() { { new ExplorerControlBrowsableObjectInfoViewModel(new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName("C:\\")))) { TreeViewItems = new ObservableCollection<IBrowsableObjectInfoViewModel>() { { new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName("C:\\Users")), TreeViewItemBrowsableObjectInfoViewModelFactory.Predicate) { IsSelected = true, Factory = new TreeViewItemBrowsableObjectInfoViewModelFactory() } }, { new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName(KnownFolders.RecycleBin.ParsingName))) } }, IsSelected = true, SelectionMode = SelectionMode.Extended, IsCheckBoxVisible = true } } };
        }
    }
}
