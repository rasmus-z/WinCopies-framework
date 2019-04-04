using System.Windows;
using System.Windows.Input;

namespace WinCopies.GUI.Controls.Themes
{
    public partial class Generic
    {
        private void ScrollToLeftButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is TabControl) ((TabControl)sender). OnScrollToLeftButtonClickInternal();

        }

        private void ScrollToLeftButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)

        {

            if (sender is TabControl) ((TabControl)sender). OnScrollToLeftButtonMouseDoubleClickInternal();

        }

        private void ScrollToRightButton_Click(object sender, RoutedEventArgs e)

        {

            if (sender is TabControl) ((TabControl)sender). OnScrollToRightButtonClickInternal();

        }

        private void ScrollToRightButton_MouseDoubleClick(object sender, RoutedEventArgs e)

        {

            if (sender is TabControl) ((TabControl)sender).OnScrollToRightButtonMouseDoubleClickInternal();

        }
    }
}
