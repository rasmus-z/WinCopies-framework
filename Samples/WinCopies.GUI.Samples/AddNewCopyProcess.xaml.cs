using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinCopies.GUI.Samples
{
    /// <summary>
    /// Interaction logic for AddNewCopyProcess.xaml
    /// </summary>
    public partial class AddNewCopyProcess : Window
    {
        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(AddNewCopyProcess));

        public string SourcePath => (string)GetValue(SourcePathProperty);

        public static readonly DependencyProperty DestPathProperty = DependencyProperty.Register(nameof(DestPath), typeof(string), typeof(AddNewCopyProcess));

        public string DestPath => (string)GetValue(DestPathProperty);

        public AddNewCopyProcess()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Close();
        }
    }
}
