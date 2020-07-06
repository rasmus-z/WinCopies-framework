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

namespace WinCopies.GUI.Samples
{
    /// <summary>
    /// Interaction logic for CopyProcessTest.xaml
    /// </summary>
    public partial class CopyProcessTest : Window
    {
        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ObservableCollection<CopyProcess>), typeof(CopyProcessTest), new PropertyMetadata(new ObservableCollection<CopyProcess>()));

        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public ObservableCollection<CopyProcess> Items => (ObservableCollection<CopyProcess>)GetValue(ItemsProperty);

        public CopyProcessTest()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var addNew = new AddNewCopyProcess();

            if (addNew.ShowDialog() == true)
            {
                var copyProcess = new CopyProcess(new PathCollection(addNew.SourcePath), addNew.DestPath
#if DEBUG
, null
#endif
                    ) { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

                Items.Add(copyProcess);

                copyProcess.RunWorkerAsync();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ObservableCollection<CopyProcess> items = Items;

            while (items.Count > 0)
            {
                items[0].CancelAsync();

                items.RemoveAt(0);
            }
        }
    }
}
