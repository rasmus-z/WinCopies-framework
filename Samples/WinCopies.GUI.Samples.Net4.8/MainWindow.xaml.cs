/* Copyright © Pierre Sprimont, 2019
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinCopies.GUI.Controls.Models;
using WinCopies.GUI.Windows.Dialogs;
using WinCopies.Util;

namespace WinCopies.GUI.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WinCopies.GUI.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ShowHelpButton = true;
        }

        private void Window_HelpButtonClick(object sender, RoutedEventArgs e)
        {

            MessageBox.Show($"The window is currently in help mode: { IsInHelpMode }.");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow[] dialogWindows = { new DialogWindow() { ShowHelpButton = true },
            new DialogWindow() { ShowHelpButton = true },
            new DialogWindow() { ShowHelpButtonAsCommandButton = true },
            new DialogWindow() { ShowHelpButtonAsCommandButton = true },
            new DialogWindow() { DialogButton = DialogButton.YesNoCancel, DefaultButton = DefaultButton.Cancel, ShowHelpButtonAsCommandButton = true },
            new DialogWindow() { DialogButton = null, CustomButtonTemplateSelector = new AttributeDataTemplateSelector(), CustomButtonsSource = new ButtonModel[] { new ButtonModel("Button1"), new ButtonModel("Button2") } } };

            int i = 0;

            dialogWindows[0].Closed += (object _sender, EventArgs _e) => OnDialogWindowClosed(dialogWindows, i);

            dialogWindows[0].Show();
        }

        private void OnDialogWindowClosed(DialogWindow[] dialogWindows, int i)
        {

            i++;

            if (i == dialogWindows.Length)

                return;

            if (i % 2 == 0)

            {

                dialogWindows[i].Closed += (object sender, EventArgs e) => OnDialogWindowClosed(dialogWindows, i);

                dialogWindows[i].Show();

            }

            else

            {

                dialogWindows[i].Closed += (object sender, EventArgs e) => OnDialogWindowClosed(dialogWindows, i);

                dialogWindows[i].ShowDialog();

            }

        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
