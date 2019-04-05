using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.GUI.Explorer.Data;
using WinCopies.GUI.Windows.Dialogs;
using BooleanToVisibilityConverter = WinCopies.Util.DataConverters.BooleanToVisibilityConverter;
using WinCopies.Util;

namespace WinCopies.GUI.Explorer.Themes
{
    public partial class Generic
    {

        public static T GetResource<T>(object key) => (T)ResourceDictionary[key];

        #region Resources

        /// <summary>
        /// Gets the <see cref="Open"/> resource.
        /// </summary>
        public static string Open => GetResource<string>(nameof(Open));

        /// <summary>
        /// Gets the <see cref="OpenInNewTab"/> resource.
        /// </summary>
        public static string OpenInNewTab => GetResource<string>(nameof(OpenInNewTab));

        /// <summary>
        /// Gets the <see cref="OpenInNewWindow"/> resource.
        /// </summary>
        public static string OpenInNewWindow => GetResource<string>(nameof(OpenInNewWindow));

        /// <summary>
        /// Gets the <see cref="OpenInNewInstanceWindow"/> resource.
        /// </summary>
        public static string OpenInNewInstanceWindow => GetResource<string>(nameof(OpenInNewInstanceWindow));

        /// <summary>
        /// Gets the <see cref="Copy"/> resource.
        /// </summary>
        public static string Copy => GetResource<string>(nameof(Copy));

        /// <summary>
        /// Gets the <see cref="Cut"/> resource.
        /// </summary>
        public static string Cut => GetResource<string>(nameof(Cut));

        /// <summary>
        /// Gets the <see cref="Paste"/> resource.
        /// </summary>
        public static string Paste => GetResource<string>(nameof(Paste));

        /// <summary>
        /// Gets the <see cref="CreateShortcut"/> resource.
        /// </summary>
        public static string CreateShortcut => GetResource<string>(nameof(CreateShortcut));

        /// <summary>
        /// Gets the <see cref="Rename"/> resource.
        /// </summary>
        public static string Rename => GetResource<string>(nameof(Rename));

        /// <summary>
        /// Gets the <see cref="Delete"/> resource.
        /// </summary>
        public static string Delete => GetResource<string>(nameof(Delete));

        /// <summary>
        /// Gets the <see cref="Properties"/> resource.
        /// </summary>
        public static string Properties => GetResource<string>(nameof(Properties));

        /// <summary>
        /// Gets the CommonProperties resource.
        /// </summary>
        public static string CommonProperties => GetResource<string>(nameof(CommonProperties));

        /// <summary>
        /// Gets the CreationTime resource.
        /// </summary>
        public static string CreationTime => GetResource<string>(nameof(CreationTime));

        /// <summary>
        /// Gets the LastAccessTime resource.
        /// </summary>
        public static string LastAccessTime => GetResource<string>(nameof(LastAccessTime));

        /// <summary>
        /// Gets the LastWriteTime resource.
        /// </summary>
        public static string LastWriteTime => GetResource<string>(nameof(LastWriteTime));

        /// <summary>
        /// Gets the Size resource.
        /// </summary>
        public static string Size => GetResource<string>(nameof(Size));

        /// <summary>
        /// Gets the SpecificProperties resource.
        /// </summary>
        public static string SpecificProperties => GetResource<string>(nameof(SpecificProperties));

        /// <summary>
        /// Gets the EditProperty resource.
        /// </summary>
        public static string EditProperty => GetResource<string>(nameof(EditProperty));

        /// <summary>
        /// Gets the Common resource.
        /// </summary>
        public static string Common => GetResource<string>(nameof(Common));

        /// <summary>
        /// Gets the FileType resource.
        /// </summary>
        public static string FileType => GetResource<string>(nameof(FileType));

        /// <summary>
        /// Gets the OpenWith resource.
        /// </summary>
        public static string OpenWith => GetResource<string>(nameof(OpenWith));

        /// <summary>
        /// Gets the Define resource.
        /// </summary>
        public static string Define => GetResource<string>(nameof(Define));

        /// <summary>
        /// Gets the Path resource.
        /// </summary>
        public static string Path => GetResource<string>(nameof(Path));

        /// <summary>
        /// Gets the SizeOnDisk resource.
        /// </summary>
        public static string SizeOnDisk => GetResource<string>(nameof(SizeOnDisk));

        /// <summary>
        /// Gets the Details resource.
        /// </summary>
        public static string Details => GetResource<string>(nameof(Details));

        /// <summary>
        /// Gets the Property resource.
        /// </summary>
        public static string Property => GetResource<string>(nameof(Property));

        /// <summary>
        /// Gets the Value resource.
        /// </summary>
        public static string Value => GetResource<string>(nameof(Value));

        /// <summary>
        /// Gets the DeletePersonalProperties resource.
        /// </summary>
        public static string DeletePersonalProperties => GetResource<string>(nameof(DeletePersonalProperties));

        /// <summary>
        /// Gets the OpenFolder resource.
        /// </summary>
        public static string OpenFolder => GetResource<string>(nameof(OpenFolder));

        /// <summary>
        /// Gets the OpenFile resource.
        /// </summary>
        public static string OpenFile => GetResource<string>(nameof(OpenFile));

        /// <summary>
        /// Gets the OpenFiles resource.
        /// </summary>
        public static string OpenFiles => GetResource<string>(nameof(OpenFiles));

        /// <summary>
        /// Gets the Save resource.
        /// </summary>
        public static string Save => GetResource<string>(nameof(Save));

        /// <summary>
        /// Gets a resource that represents the path to the WinCopies Processes manager application.
        /// </summary>
        public static string WinCopiesProcessesManagerPath => GetResource<string>(nameof(WinCopiesProcessesManagerPath));

        public static BooleanToVisibilityConverter BooleanToVisibilityConverter => GetResource<BooleanToVisibilityConverter>(nameof(BooleanToVisibilityConverter));

        public static FileSystemInfoAttributesToVisibilityConverter FileSystemInfoAttributesToVisibilityConverter => GetResource<FileSystemInfoAttributesToVisibilityConverter>(nameof(FileSystemInfoAttributesToVisibilityConverter));

        public static FileSystemInfoAttributesToOpacityConverter FileSystemInfoAttributesToOpacityConverter => GetResource<FileSystemInfoAttributesToOpacityConverter>(nameof(FileSystemInfoAttributesToOpacityConverter));

        #endregion

        public static ResourceDictionary ResourceDictionary { get; } = null;

        static Generic() => ResourceDictionary = Util.Generic.AddNewDictionary("/WinCopies.GUI.Explorer;component/Themes/Generic.xaml");

        private void PreviousButton_Click(object sender, RoutedEventArgs e)

        {

            var historyItem = ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).History[((ExplorerControl)((FrameworkElement)sender).TemplatedParent).HistorySelectedIndex + 1];

            if (historyItem is HistoryItemData)

                ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Navigate((IBrowsableObjectInfo)((HistoryItemData)historyItem).Path, false);

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)

        {

            var historyitem = ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).History[((ExplorerControl)((FrameworkElement)sender).TemplatedParent).HistorySelectedIndex - 1];

            if (historyitem is IHistoryItemData)

                ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Navigate((IBrowsableObjectInfo)((HistoryItemData)historyitem).Path, false);

        }

        private void ListView_MouseDoubleClick(object sender, RoutedEventArgs e)

        {

            if (((ExplorerControl)((FrameworkElement)sender).TemplatedParent).ListViewSelectedItem is IO.IBrowsableObjectInfo)

                ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).OnOpeningInternal();

        }

        private void Navigate(object sender) => ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Open(new ShellObjectInfo(Microsoft.WindowsAPICodePack.Shell.ShellObject.FromParsingName(
                ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Text)
                , ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Text)
                );

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)

                Navigate(sender);
            // SelectedItem.ExplorerControl.Open(WinCopies.IO.Util.GetNormalizedOSPath(SelectedItem.ExplorerControl.CurrentPath));)
            // new WinCopies.IO.ShellObjectInfo(Microsoft.WindowsAPICodePack.Shell.ShellObject.FromParsingName(WinCopies.IO.Util.GetNormalizedOSPath(ExplorerControl.CurrentPath)), WinCopies.IO.Util.GetNormalizedOSPath(ExplorerControl.CurrentPath), WinCopies.IO.SpecialFolders)
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Navigate(sender);

        // todo:

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            // if (!IsLoaded || DataContext == null) return;

            // ((ExplorerControl)sender).RaisePropertyChangedEvent("SelectedItems");

#if DEBUG
            Debug.WriteLine(string.Format("Added items: {0}, Removed items: {1}", e.AddedItems.Count, e.RemovedItems.Count));

            if (((ExplorerControl)((FrameworkElement)sender).TemplatedParent).ListViewSelectedItems != null)

            {

                Debug.WriteLine(((ExplorerControl)((FrameworkElement)sender).TemplatedParent).ListViewSelectedItems.ListBox.SelectedItems.Count.ToString());

                foreach (object @object in ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).ListViewSelectedItems.ListBox.SelectedItems)

                    Debug.WriteLine("@object.GetType().ToString(): " + @object.GetType().ToString());

            }

#endif

            ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).RaiseSelectionChangedEvent(new SelectionChangedEventArgs(e.RoutedEvent, e.AddedItems, e.RemovedItems, ActionsFromObjects.ListView));

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) => ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Text = ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).Path.Path;

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e) => ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).RaiseTextChangedEvent(e);

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ((ExplorerControl)((FrameworkElement)sender).TemplatedParent).OnListViewMouseDoubleClickInternal(e);

        private void ListViewItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

            ListView listView = (ListView)((FrameworkElement)sender).GetParent(typeof(ListView), false);

            ((ExplorerControl)listView.GetParent(typeof(ExplorerControl), false)).OnItemsControlContextMenuOpening( (FrameworkElement) sender, listView, e);    

        }
    }

}
