using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            Items = new ObservableCollection<IExplorerControlBrowsableObjectInfoViewModel>() { { new ExplorerControlBrowsableObjectInfoViewModel(new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName("C:\\")))) { TreeViewItems = new ObservableCollection<IBrowsableObjectInfoViewModel>() { { new TreeViewBrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName("C:\\Users"))) { IsSelected = true } } }, IsSelected = true, SelectionMode = SelectionMode.Extended, IsCheckBoxVisible = true } } };
        }
    }
}
