using AttachedCommandBehavior;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinCopies.IO;
using WinCopies.Util;
using BackgroundWorker = WinCopies.Util.BackgroundWorker;
using NotifyCollectionChangedEventArgs = System.Collections.Specialized.NotifyCollectionChangedEventArgs;
using Generic = WinCopies.GUI.Explorer.Themes.Generic;

namespace WinCopies.GUI.Explorer
{
    public class ExplorerControl : Control, ICommandSource
    {

        public TreeView TreeView { get; private set; } = null;

        public ListView ListView { get; private set; } = null;

        private TextBox PART_TextBox = null;

        private BackgroundWorker FileOpeningBgWorker = null;

        private List<ShellFile> PathsToOpen = null;

        private readonly FileSystemWatcher fsw = new FileSystemWatcher();

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ExplorerControl), new PropertyMetadata(Commands.Open));

        /// <summary>
        /// Gets or sets the command of this <see cref="ExplorerControl"/>. This command is used to open items in this <see cref="ExplorerControl"/>. This is a dependency property.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="Command"/> property. This property is used to pass to the <see cref="Command"/> property. This is a dependency property.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(ExplorerControl), new PropertyMetadata(null));

        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the text of this <see cref="ExplorerControl"/>. This is a dependency property.
        /// </summary>
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        private static readonly DependencyPropertyKey PathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Path), typeof(IBrowsableObjectInfo), typeof(ExplorerControl), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ExplorerControl)d).PathChanged?.Invoke(d, new ValueChangedEventArgs(e.OldValue, e.NewValue))));

        /// <summary>
        /// Identifies the <see cref="Path"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathProperty = PathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> which represents the path of this <see cref="ExplorerControl"/>. This is a dependency property.
        /// </summary>
        public IBrowsableObjectInfo Path => (IBrowsableObjectInfo)GetValue(PathProperty);

        private static readonly DependencyPropertyKey IsLoadingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsLoading), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsLoading"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty = IsLoadingPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets a value that indicates if this <see cref="ExplorerControl"/> is loading items. The <see cref="FrameworkElement.IsLoaded"/> property and the <see cref="FrameworkElement.Loaded"/> event are not concerned with this property. This is a dependency property.
        /// </summary>
        public bool IsLoading { get => (bool)GetValue(IsLoadingProperty); set => SetValue(IsLoadingPropertyKey, value); }

        /// <summary>
        /// Identifies the <see cref="PreviousPathIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviousPathIconProperty = DependencyProperty.Register(nameof(PreviousPathIcon), typeof(ImageSource), typeof(ExplorerControl), new PropertyMetadata(Properties.Resources.resultset_previous.ToImageSource()));

        /// <summary>
        /// Gets or sets the icon for the 'Previous path' button. This is a dependency property.
        /// </summary>
        public ImageSource PreviousPathIcon { get => (ImageSource)GetValue(PreviousPathIconProperty); set => SetValue(PreviousPathIconProperty, value); }

        /// <summary>
        /// Identifies the <see cref="NextPathIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NextPathIconProperty = DependencyProperty.Register(nameof(NextPathIcon), typeof(ImageSource), typeof(ExplorerControl), new PropertyMetadata(Properties.Resources.resultset_next.ToImageSource()));

        /// <summary>
        /// Gets or sets the icon for the 'Next path' button. This is a dependency property.
        /// </summary>
        public ImageSource NextPathIcon { get => (ImageSource)GetValue(NextPathIconProperty); set => SetValue(NextPathIconProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ParentPathIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentPathIconProperty = DependencyProperty.Register(nameof(ParentPathIcon), typeof(ImageSource), typeof(ExplorerControl), new PropertyMetadata(Properties.Resources.arrow_up.ToImageSource()));

        /// <summary>
        /// Gets or sets the icon for the 'Parent path' button. This is a dependency property.
        /// </summary>
        public ImageSource ParentPathIcon { get => (ImageSource)GetValue(ParentPathIconProperty); set => SetValue(ParentPathIconProperty, value); }

        /// <summary>
        /// Identifies the <see cref="FolderGoIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FolderGoIconProperty = DependencyProperty.Register(nameof(FolderGoIcon), typeof(ImageSource), typeof(ExplorerControl), new PropertyMetadata(Properties.Resources.folder_go.ToImageSource()));

        /// <summary>
        /// Gets or sets the icon for the 'Go to' button. This is a dependency property.
        /// </summary>
        public ImageSource FolderGoIcon { get => (ImageSource)GetValue(FolderGoIconProperty); set => SetValue(FolderGoIconProperty, value); }

        /// <summary>
        /// Identifies the <see cref="FolderCancelLoadingIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FolderCancelLoadingIconProperty = DependencyProperty.Register(nameof(FolderCancelLoadingIcon), typeof(ImageSource), typeof(ExplorerControl), new PropertyMetadata(Properties.Resources.cancel.ToImageSource()));

        /// <summary>
        /// Gets or sets the icon for the 'Go to' button when this button gets the 'Cancel folder loading' state. This is a dependency property.
        /// </summary>
        public ImageSource FolderCancelLoadingIcon { get => (ImageSource)GetValue(FolderCancelLoadingIconProperty); set => SetValue(FolderCancelLoadingIconProperty, value); }

        private static readonly DependencyPropertyKey HeaderPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Header), typeof(string), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = HeaderPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="Path"/> header of this <see cref="ExplorerControl"/>. This is a dependency property.
        /// </summary>
        public string Header => (string)GetValue(HeaderProperty);

        /// <summary>
        /// Identifies the <see cref="AllowMultipleSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowMultipleSelectionProperty = DependencyProperty.Register(nameof(AllowMultipleSelection), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether activate multiple selection. This is dependency property.
        /// </summary>
        public bool AllowMultipleSelection { get => (bool)GetValue(AllowMultipleSelectionProperty); set => SetValue(AllowMultipleSelectionProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ShowItemsCheckBox"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowItemsCheckBoxProperty = DependencyProperty.Register(nameof(ShowItemsCheckBox), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value that indicates whether to show the items check boxes when multi-selection is available. This is a dependency property.
        /// </summary>
        public bool ShowItemsCheckBox { get => (bool)GetValue(ShowItemsCheckBoxProperty); set => SetValue(ShowItemsCheckBoxProperty, value); }

        public static readonly DependencyProperty ShowHiddenItemsProperty = DependencyProperty.Register(nameof(ShowHiddenItems), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(null));

        public bool ShowHiddenItems

        {

            get => (bool)GetValue(ShowHiddenItemsProperty); set => SetValue(ShowHiddenItemsProperty, value);

        }

        public static readonly DependencyProperty ShowSystemItemsProperty = DependencyProperty.Register(nameof(ShowSystemItems), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(null));

        public bool ShowSystemItems

        {

            get => (bool)GetValue(ShowSystemItemsProperty); set => SetValue(ShowSystemItemsProperty, value);

        }

        /// <summary>
        /// Identifies the <see cref="TreeViewSelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TreeViewSelectedItemProperty = DependencyProperty.Register(nameof(TreeViewSelectedItem), typeof(IO.IBrowsableObjectInfo), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the tree view selected item.
        /// </summary>
        public IO.IBrowsableObjectInfo TreeViewSelectedItem { get => (IO.IBrowsableObjectInfo)GetValue(TreeViewSelectedItemProperty); set => SetValue(TreeViewSelectedItemProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ListViewSelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListViewSelectedItemProperty = DependencyProperty.Register(nameof(ListViewSelectedItem), typeof(IO.IBrowsableObjectInfo), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the list view selected item.
        /// </summary>
        public IBrowsableObjectInfo ListViewSelectedItem
        {
            get => (IBrowsableObjectInfo)GetValue(ListViewSelectedItemProperty);

            set => SetValue(ListViewSelectedItemProperty, value);
        }

        private static readonly DependencyPropertyKey ListViewSelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ListViewSelectedItems), typeof(ObservableListBoxSelectedItems), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ListViewSelectedItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ListViewSelectedItemsProperty = ListViewSelectedItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the list view selected items.
        /// </summary>
        public ObservableListBoxSelectedItems ListViewSelectedItems => (ObservableListBoxSelectedItems)GetValue(ListViewSelectedItemsProperty);

        private System.Collections.ObjectModel.ObservableCollection<IHistoryItemData> history = new System.Collections.ObjectModel.ObservableCollection<IHistoryItemData>();

        private static readonly DependencyPropertyKey HistoryPropertyKey = DependencyProperty.RegisterReadOnly(nameof(History), typeof(System.Collections.ObjectModel.ReadOnlyObservableCollection<IHistoryItemData>), typeof(ExplorerControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="History"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HistoryProperty = HistoryPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the history of the browsed paths.
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyObservableCollection<IHistoryItemData> History { get => (System.Collections.ObjectModel.ReadOnlyObservableCollection<IHistoryItemData>)GetValue(HistoryProperty); }

        private static readonly DependencyPropertyKey HistorySelectedIndexPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HistorySelectedIndex), typeof(int), typeof(ExplorerControl), new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="HistorySelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HistorySelectedIndexProperty = HistorySelectedIndexPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the history selected index.
        /// </summary>
        public int HistorySelectedIndex => (int)GetValue(HistorySelectedIndexProperty);

        /// <summary>
        /// Identifies the <see cref="ViewStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewStyleProperty = DependencyProperty.Register(nameof(ViewStyle), typeof(ViewStyles), typeof(ExplorerControl), new PropertyMetadata(ViewStyles.Tiles));

        /// <summary>
        /// Gets or sets the view style of the ListView.
        /// </summary>
        public ViewStyles ViewStyle { get => (ViewStyles)GetValue(ViewStyleProperty); set => SetValue(ViewStyleProperty, value); }

        /// <summary>
        /// Identifies the <see cref="TreeViewItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TreeViewItemsProperty = DependencyProperty.Register(nameof(TreeViewItems), typeof(System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo>), typeof(ExplorerControl), new PropertyMetadata(

            GetDefaultTreeViewItems(), (DependencyObject d, DependencyPropertyChangedEventArgs e) =>

            {

                foreach (IBrowsableObjectInfo item in (ObservableCollection<IBrowsableObjectInfo>)e.OldValue)

                    item.Dispose();

            }

            ));

        public System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo> TreeViewItems { get => (System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo>)GetValue(TreeViewItemsProperty); set => SetValue(TreeViewItemsProperty, value); }

        private static readonly DependencyPropertyKey CanMoveToPreviousPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanMoveToPreviousPath), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="CanMoveToPreviousPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToPreviousPathProperty = CanMoveToPreviousPathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether it is possible to move to the previous path of the history.
        /// </summary>
        public bool CanMoveToPreviousPath => (bool)GetValue(CanMoveToPreviousPathProperty);

        private static readonly DependencyPropertyKey CanMoveToNextPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanMoveToNextPath), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="CanMoveToNextPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToNextPathProperty = CanMoveToNextPathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether it is possible to move to the next path of the history.
        /// </summary>
        public bool CanMoveToNextPath => (bool)GetValue(CanMoveToNextPathProperty);

        private static readonly DependencyPropertyKey CanMoveToParentPathPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanMoveToParentPath), typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="CanMoveToParentPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToParentPathProperty = CanMoveToParentPathPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether it is possible to move to the parent path.
        /// </summary>
        public bool CanMoveToParentPath => (bool)GetValue(CanMoveToParentPathProperty);

        private static readonly DependencyPropertyKey VisibleItemsCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(VisibleItemsCount), typeof(int), typeof(ExplorerControl), new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ExplorerControl)d).VisibleItemsCountChanged?.Invoke(d, new ValueChangedEventArgs(e.OldValue, e.NewValue))));

        /// <summary>
        /// Identifies the <see cref="VisibleItemsCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VisibleItemsCountProperty = VisibleItemsCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the visible items count.
        /// </summary>
        public int VisibleItemsCount => (int)GetValue(VisibleItemsCountProperty);

        public static readonly DependencyProperty ContextMenusProperty = DependencyProperty.Register(nameof(ContextMenus), typeof(IEnumerable<WinCopies.Util.MenuItem>), typeof(ExplorerControl));

        public IEnumerable<Util.MenuItem> ContextMenus { get => (IEnumerable<WinCopies.Util.MenuItem>)GetValue(ContextMenusProperty); set => SetValue(ContextMenusProperty, value); }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(IEnumerable<string>), typeof(ExplorerControl));

        public IEnumerable<string> Filter { get => (IEnumerable<string>)GetValue(FilterProperty); set => SetValue(FilterProperty, value); }

        // public static readonly DependencyProperty BrowsableObjectInfoItemsLoaderProperty = DependencyProperty.Register("BrowsableObjectInfoItemsLoader", typeof(BrowsableObjectInfoItemsLoader), typeof(ExplorerControl), new PropertyMetadata()));

        // private WinCopies.IO.BrowsableObjectInfoItemsLoader BrowsableObjectInfoItemsLoader = null;

        /// <summary>
        /// Occurs when the <see cref="Path"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler PathChanged;

        /// <summary>
        /// Occurs when the <see cref="Text"/> property has changed.
        /// </summary>
        public event TextChangedEventHandler TextChanged;

        /// <summary>
        /// Occurs when the selection has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the visible items count changed.
        /// </summary>
        public event ValueChangedEventHandler VisibleItemsCountChanged;

        static ExplorerControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControl), new FrameworkPropertyMetadata(typeof(ExplorerControl)));

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerControl"/> class.
        /// </summary>
        public ExplorerControl() => Init(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerControl"/> class using a custom path.
        /// </summary>
        public ExplorerControl(string path) : this(new ShellObjectInfo(ShellObject.FromParsingName(path), path)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerControl"/> class using a custom path.
        /// </summary>
        public ExplorerControl(IBrowsableObjectInfo path) => Init(path);

        private void Init(IBrowsableObjectInfo path)

        {

            SetValue(HistoryPropertyKey, new System.Collections.ObjectModel.ReadOnlyObservableCollection<IHistoryItemData>(history));

            if (path == null)

            {

                string _path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                path = new ShellObjectInfo(ShellObject.FromParsingName(_path), _path);

            }

            Open(path );    

            history.CollectionChanged += History_CollectionChanged;

            PathChanged += ExplorerControl_PathChanged;

            TextChanged += ExplorerControl_TextChanged;

            // InputBindings.Add(new MouseBinding(Commands.Open, new MouseGesture(MouseAction.LeftDoubleClick)));

            CommandBindings.Add(new CommandBinding(Commands.Open, Open_Executed, Open_CanExecute));

            #region Comments

            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Command_CanExecute));

            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Command_CanExecute));

            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Command_CanExecute));

            //CommandBindings.Add(new CommandBinding(Commands.CreateShortcut, CreateShortcut_Executed, CanWrite_CanExecute));

            //CommandBindings.Add(new CommandBinding(Commands.Rename, Rename_Executed, CanWrite_CanExecute));

            //CommandBindings.Add(new CommandBinding(Commands.Delete, Delete_Executed, CanWrite_CanExecute));

            //CommandBindings.Add(new CommandBinding(Commands.Properties, Properties_Executed, Command_CanExecute));

            #endregion

        }

        internal void RaiseTextChangedEvent(TextChangedEventArgs e) => TextChanged?.Invoke(this, e);

        internal void RaiseSelectionChangedEvent(SelectionChangedEventArgs e) => SelectionChanged?.Invoke(this, e);

        protected virtual void OnPathChanged(IBrowsableObjectInfo oldValue, IBrowsableObjectInfo newValue)

        {
            // Console.WriteLine((PART_TextBox != null).ToString()+" "+(!PART_TextBox.IsFocused).ToString());
            if (PART_TextBox == null || PART_TextBox != null && !PART_TextBox.IsFocused)

                Text = newValue.Path;

            // if (value != null)

            // if (e.Action == NotifyCollectionChangedAction.Reset)    

            if (oldValue != null)

            {

                // ((INotifyCollectionChanging)oldValue.Items).CollectionChanging -= Path_CollectionChanging;

                // foreach (IBrowsableObjectInfo value in oldValue.Items)

                // {

                // object content = ((ListViewItem)ListView.ItemContainerGenerator.ContainerFromItem(value)).Content;

                // ((ListViewItem)ListView.ItemContainerGenerator.ContainerFromItem(value)).IsVisibleChanged -= Value_IsVisibleChanged;

                // }

                if (oldValue is BrowsableObjectInfo)

                    oldValue.Dispose();

                oldValue.Items.CollectionChanging -= Items_CollectionChanging;

                listViewItems.Clear();

                SetValue(VisibleItemsCountPropertyKey, 0);

            }

            // ((INotifyCollectionChanging)newValue.Items).CollectionChanging += Path_CollectionChanging;

            newValue.Items.CollectionChanging += Items_CollectionChanging;

        }

        // private void Path_CollectionChanging(object sender, WinCopies.Util.NotifyCollectionChangedEventArgs e)

        // {

        // if ((e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Reset) && e.ResetItems != null)

        // foreach (IBrowsableObjectInfo value in e.ResetItems)

        // object value = ((ListViewItem)ListView.ItemContainerGenerator.ContainerFromItem(ListView.ItemContainerGenerator.Items[i]));

        // ((ListViewItem)ListView.ItemContainerGenerator.ContainerFromItem(value)).IsVisibleChanged -= Value_IsVisibleChanged;

        // }

        protected virtual void OnTextChanged(TextChangedEventArgs e)

        {

            // todo: checking if a display path is available, and so showing the list of the available sub-paths

        }

        private void Open(object value) => Open();

        public List<Util.MenuItem> GetDefaultListViewItemContextMenu()

        {

            List<Util.MenuItem> menuItems = new List<Util.MenuItem>();

            Util.MenuItem menuItem = new Util.MenuItem(Generic.Open, null, new DelegateCommand(Open), ListViewSelectedItem, null);

            string extension = System.IO.Path.GetExtension(ListViewSelectedItem.Path);

            if (!(ListViewSelectedItem.FileType == FileTypes.Archive && LoadArchive.IsSupportedArchiveFormat(extension)) && extension.Length > 0)

            {

                string command = Registry.GetCommandByExtension("open", System.IO.Path.GetExtension(ListViewSelectedItem.Path));

                if (command != null) menuItem.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Registry.GetOpenWithSoftwarePathFromCommand(command)).ToBitmap().ToImageSource();

            }

            menuItems.AddRange(new Util.MenuItem[] {menuItem,
                new Util.MenuItem(Generic.Copy, Properties.Resources.page_copy.ToImageSource(), new DelegateCommand(o => Copy(ActionsFromObjects.ListView)), null, null),
                new Util.MenuItem(Generic.Cut, Properties.Resources.cut.ToImageSource(), new DelegateCommand(o => Cut(ActionsFromObjects.ListView)), null, null),
                new Util.MenuItem(Generic.CreateShortcut, null, new DelegateCommand<ShellObjectInfo>(o => new ShellLink(o.Path, System.IO.Path.GetDirectoryName(o.Path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(o.Path) + ".lnk")), ListViewSelectedItem, null),
                new Util.MenuItem(Generic.Rename),
                new Util.MenuItem(Generic.Delete),
                new Util.MenuItem(Generic.Properties) });

            if (System.IO.Path.HasExtension(ListViewSelectedItem.Path))

            {

                menuItem = new Util.MenuItem("Open with");

                menuItems.Insert(1, menuItem);

                WinShellAppInfoInterop winShellAppInfoInterop = new WinShellAppInfoInterop(System.IO.Path.GetExtension(ListViewSelectedItem.Path));

                winShellAppInfoInterop.OpenWithAppInfosLoaded += (object sender, EventArgs e) =>

                {

                    foreach (AppInfo value in winShellAppInfoInterop.OpenWithAppInfos)

                        menuItem.Items.Add(new Util.MenuItem(value.DisplayName, null, null, null, null));

                };

                winShellAppInfoInterop.GetAppInfos();

            }

            return menuItems;

        }

        protected virtual void OnListViewSelectionChanged(System.Windows.Controls.SelectionChangedEventArgs e)

        {

            if (ListView.SelectedItems.Count > 0)

                ContextMenus = GetDefaultListViewItemContextMenu();

        }

        private void History_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Remove)

                foreach (var oldItem in e.OldItems.OfType<HistoryItemData>())

                    ((IBrowsableObjectInfo)oldItem.Path).Dispose();

        }

        private static System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo> GetDefaultTreeViewItems()

        {

            object[][] defaultTreeViewFolders = { new object[] { KnownFolders.Desktop, SpecialFolders.Desktop }, new object[] { KnownFolders.Computer, SpecialFolders.Computer }, new object[] { KnownFolders.Libraries, SpecialFolders.Libraries }, new object[] { KnownFolders.RecycleBin, SpecialFolders.RecycleBin } };

            System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo> oc = new System.Collections.ObjectModel.ObservableCollection<IBrowsableObjectInfo>();

            ShellObject shellObject;

            foreach (object[] defaultTreeViewFolder in defaultTreeViewFolders)

            {

                shellObject = ShellObject.FromParsingName(((IKnownFolder)defaultTreeViewFolder[0]).ParsingName);

                oc.Add(new ShellObjectInfo(
                            shellObject, shellObject.IsFileSystemObject ? shellObject.ParsingName : shellObject.GetDisplayName(DisplayNameType.Default), FileTypes.SpecialFolder, (SpecialFolders)defaultTreeViewFolder[1]));

            };

            return oc;

        }

        private void ExplorerControl_PathChanged(object sender, ValueChangedEventArgs e) => OnPathChanged((IBrowsableObjectInfo)e.OldValue, (IBrowsableObjectInfo)e.NewValue);

        private void ExplorerControl_TextChanged(object sender, TextChangedEventArgs e) => OnTextChanged(e);

        /// <summary>
        /// Is invoked whenever application code or internal processes call <see cref="FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            TreeView = (TreeView)Template.FindName("PART_TreeView", this);

            ListView = (ListView)Template.FindName("PART_ListView", this);

            if (ListView != null)

            {

                ListView.ParentExplorerControl = this;

                ListView.SelectionChanged += (object sender, System.Windows.Controls.SelectionChangedEventArgs e) => OnListViewSelectionChanged(e);

                SetValue(ListViewSelectedItemsPropertyKey, new ObservableListBoxSelectedItems(ListView));

                ListViewSelectedItems.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>

                {

                    IBrowsableObjectInfoInternal _path = (IBrowsableObjectInfoInternal)Path;

                    if (e.Action == NotifyCollectionChangedAction.Add)

                        foreach (var item in e.NewItems.OfType<IBrowsableObjectInfo>())

                            _path.SelectedItems.Add(item);

                    else if (e.Action == NotifyCollectionChangedAction.Remove)

                        foreach (var item in e.OldItems.OfType<IBrowsableObjectInfo>())

                            _path.SelectedItems.Remove(item);

                    else if (e.Action == NotifyCollectionChangedAction.Reset)

                        _path.SelectedItems.Clear();

                };

                ListView.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

            }

            PART_TextBox = (TextBox)Template.FindName("PART_TextBox", this);

        }

        private void Items_CollectionChanging(object sender, Util.NotifyCollectionChangedEventArgs e)

        {

            if (ListView != null && ListView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)

            {

                ListViewItem item = null;

                if (e.OldItems != null)

                    for (int i = 0; i < e.OldItems.Count; i++)

                    {

                        item = (ListViewItem)ListView.ItemContainerGenerator.ContainerFromIndex(e.OldStartingIndex + i);

                        if (listViewItems.Contains(item))

                        {

                            SetValue(VisibleItemsCountPropertyKey, VisibleItemsCount - 1);

                            listViewItems.Remove(item);

                        }

                    }

                if (e.NewItems != null)

                    UpdateVisibleItemsCount();

            }

        }

        private List<ListViewItem> listViewItems = new List<ListViewItem>();

        private void UpdateVisibleItemsCount()

        {

            if (ListView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)

            {

                // int count = 0;

                ListViewItem value = null;

                foreach (object item in ListView.ItemContainerGenerator.Items)

                {

                    value = (ListViewItem)ListView.ItemContainerGenerator.ContainerFromItem(item);

                    if (value == null)

                        return;

                    if (!listViewItems.Contains(value))

                    {

                        listViewItems.Add(value);

                        SetValue(VisibleItemsCountPropertyKey, VisibleItemsCount + 1);

                        // value.IsVisibleChanged += Value_IsVisibleChanged;

                    }

                }

                // SetValue(VisibleItemsCountPropertyKey, count);

            }

        }

        internal void Value_IsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)

        {

            if ((Visibility)e.NewValue == Visibility.Visible)

                SetValue(VisibleItemsCountPropertyKey, VisibleItemsCount + 1);

            else

            {

                ((ListViewItem)d).IsSelected = false;

                SetValue(VisibleItemsCountPropertyKey, VisibleItemsCount - 1);

            }

        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e) => UpdateVisibleItemsCount();

        internal void OnListViewMouseDoubleClickInternal(MouseButtonEventArgs e) => OnListViewMouseDoubleClick(e);

        protected virtual void OnListViewMouseDoubleClick(MouseButtonEventArgs e)

        {

            if (e.ChangedButton == MouseButton.Left)

                Command.TryExecute(CommandParameter, CommandTarget);

        }

        private void FileOpeningBgWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)

        {

            PathsToOpen = null;

            FileOpeningBgWorker = null;

            Window.GetWindow(this).Cursor = Cursors.Arrow;

        }

        private void FileOpeningBgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            for (int i = 0; i <= PathsToOpen.Count - 1; i++)

                Process.Start(PathsToOpen[i].Path);

        }

        internal bool OpenInternal(IBrowsableObjectInfo path)

        {

            if (path is ShellObjectInfo)

                if (path.FileType == FileTypes.Folder || path.FileType == FileTypes.Drive || path.FileType == FileTypes.SpecialFolder || path.FileType == FileTypes.Archive)

                {

                    Navigate(path, true);

                    return true;

                }

                else if (path.FileType == FileTypes.File)

                    if (!(path is IO.ShellObjectInfo) || !(((IO.ShellObjectInfo)path).ShellObject is ShellFile))

                        throw new ArgumentException("path isn't a ShellObjectInfo or its ShellObject property value isn't a ShellFile.");

                    else

                    {

                        OpenFile((ShellFile)((IO.ShellObjectInfo)path).ShellObject);

                        return true;

                    }

            return false;

        }

        internal void OpenLinkInternal(ShellLink path)

        {

            using (ShellObjectInfo _path = new ShellObjectInfo(path.TargetShellObject, path.TargetLocation))

                if (_path.FileType == FileTypes.Folder || _path.FileType == FileTypes.Drive || _path.FileType == FileTypes.SpecialFolder || _path.FileType == FileTypes.Archive)

                    Navigate(_path, true);

                else if (_path.FileType == FileTypes.File)

                    if (!(_path is IO.ShellObjectInfo) || !(_path.ShellObject is ShellFile))

                        throw new ArgumentException("path isn't a ShellObjectInfo or its ShellObject property value isn't a ShellFile.");

                    else

                        OpenFile((ShellFile)_path.ShellObject);

        }

        /// <summary>
        /// Opens a path.
        /// </summary>
        /// <param name="path">The <see cref="IBrowsableObjectInfo"/> path as to open.</param>
        public void Open(IBrowsableObjectInfo path)

        {

#if DEBUG

            Debug.WriteLine("ExplorerControl Open");

            Debug.WriteLine((!(path is IO.ShellObjectInfo)).ToString() + " " + ((IO.ShellObjectInfo)path).ShellObject.GetType().ToString());

#endif

            if (!OpenInternal(path))

                if (path.FileType == FileTypes.Link)

                    if (!(path is IO.ShellObjectInfo) || !((IO.ShellObjectInfo)path).ShellObject.IsLink)

                        throw new ArgumentException("path isn't a ShellObjectInfo or path isn't a link.");

                    else

                    {

                        ShellLink shellLink = (ShellLink)ShellObject.FromParsingName(((IO.ShellObjectInfo)path).ShellObject.ParsingName);

                        if (shellLink.TargetShellObject.IsLink)

                            throw new InvalidOperationException("Shell link target shell object is also a link.");

                        else

                            OpenLinkInternal(shellLink);

                    }

        }

        public void Open()

        {

            bool areFoldersSelected = false;

            bool areFilesSelected = false;

            bool areOtherObjectsSelected = false;

            foreach (var item in ListViewSelectedItems.ListBox.SelectedItems)

                if (item is IBrowsableObjectInfo)

                    if (((IBrowsableObjectInfo)item).FileType == FileTypes.Folder || ((IBrowsableObjectInfo)item).FileType == FileTypes.Drive || ((IBrowsableObjectInfo)item).FileType == FileTypes.SpecialFolder || ((IBrowsableObjectInfo)item).FileType == FileTypes.Archive)

                        areFoldersSelected = true;

                    else if (((IBrowsableObjectInfo)item).FileType == FileTypes.File)

                        areFilesSelected = true;

                    else

                        areOtherObjectsSelected = true;

            if (!(areFoldersSelected != areFilesSelected != areOtherObjectsSelected))

                return;

            for (int i = 0; i <= ListViewSelectedItems.ListBox.SelectedItems.Count - 1; i++)

                Open((IBrowsableObjectInfo)ListViewSelectedItems.ListBox.SelectedItems[i]);

        }

        public void Navigate(IBrowsableObjectInfo path, bool addPathToHistory) => Navigate(path, addPathToHistory, null);

        public void Navigate(IBrowsableObjectInfo path, bool addPathToHistory, BrowsableObjectInfoItemsLoader browsableObjectInfoItemsLoader)

        {

            // SetValue(PathPropertyKey, path);

            if (Path != null)

                if (Path.ItemsLoader.IsBusy)

                    // if (Path.ItemsLoader.WorkerSupportsCancellation)

                    Path.ItemsLoader.Cancel();

            // else

            // throw new InvalidOperationException("This worker doesn't supports cancellation.");

            SetValue(IsLoadingPropertyKey, true);

            // if (path is ShellObjectInfo && (((ShellObjectInfo)path).FileType == FileTypes.Folder || ((ShellObjectInfo)path).FileType == FileTypes.Drive))

            // if (BrowsableObjectInfoItemsLoader == null || BrowsableObjectInfoItemsLoader.GetType() != typeof(LoadFolder))

            // BrowsableObjectInfoItemsLoader = new LoadFolder((ShellObjectInfo)path);

            SetValue(HeaderPropertyKey, ((ShellObjectInfo)path).ShellObject.GetDisplayName(DisplayNameType.Default));


            // loadFolder.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>

            SetValue(PathPropertyKey, path);

            SetValue(CanMoveToParentPathPropertyKey, path.Parent != null);

            if (browsableObjectInfoItemsLoader == null)

                path.LoadItems();

            else

                path.LoadItems(browsableObjectInfoItemsLoader);

            if (addPathToHistory)

                if (ListView == null)

                    history.Insert(0, new HistoryItemData(Header, path, new ScrollViewerOffset(0, 0), null));

                else

                    history.Insert(0, new HistoryItemData(Header, path, new ScrollViewerOffset(ListView.ScrollHost.HorizontalOffset, ListView.ScrollHost.VerticalOffset), null));

            else

            {

                bool currentPathIsInHistory = false;

                for (int i = 0; i < history.Count; i++)

                {

                    var historyItem = history[i];

                    if (historyItem is HistoryItemData && path.Path == ((HistoryItemData)historyItem).Path.Path)

                    {

                        SetValue(HistorySelectedIndexPropertyKey, HistorySelectedIndex + 1);

                        currentPathIsInHistory = true;

                    }

                }

                if (!currentPathIsInHistory)

                {

                    history.RemoveRange(0, HistorySelectedIndex + 1);

                    SetValue(HistorySelectedIndexPropertyKey, 0);

                }

            }

            SetValue(CanMoveToPreviousPathPropertyKey, history.Count > 1 && HistorySelectedIndex < history.Count - 1);

            SetValue(CanMoveToNextPathPropertyKey, history.Count > 1 && HistorySelectedIndex > 0);

        }

        /// <summary>
        /// Opens a <see cref="ShellFile"/> asynchronously.
        /// </summary>
        /// <param name="path">The <see cref="ShellFile"/> to open.</param>
        public void OpenFile(ShellFile path)

        {

            if (FileOpeningBgWorker == null)

            {

                FileOpeningBgWorker = new BackgroundWorker();

                FileOpeningBgWorker.DoWork += FileOpeningBgWorker_DoWork;

                FileOpeningBgWorker.RunWorkerCompleted += FileOpeningBgWorker_RunWorkerCompleted;

            }

            if (PathsToOpen == null)

                PathsToOpen = new List<ShellFile>();

            PathsToOpen.Add(path);

            if (!FileOpeningBgWorker.IsBusy)

                FileOpeningBgWorker.RunWorkerAsync();

            Window.GetWindow(this).Cursor = Cursors.Wait;

        }

        protected virtual void OnOpening() => Open();

        internal void OnOpeningInternal() => OnOpening();

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e) => OnOpening();

        private StringCollection GetFileDropList(ActionsFromObjects copyFrom)

        {

            StringCollection sc = null;

            if (copyFrom == ActionsFromObjects.TreeView)

                sc = new StringCollection() { ((IO.ShellObjectInfo)TreeViewSelectedItem).Path };

            else if (copyFrom == ActionsFromObjects.ListView)

            {

                sc = new StringCollection();

                foreach (IO.ShellObjectInfo shellObjectInfo in ListViewSelectedItems.ListBox.SelectedItems)

                    sc.Add(shellObjectInfo.Path);

                Clipboard.SetFileDropList(sc);

            }

            return sc;

        }

        public void Copy(ActionsFromObjects copyFrom) => Clipboard.SetFileDropList(GetFileDropList(copyFrom));

        //public void Copy()
        //{

        //    if (PART_TreeView.IsFocused)

        //        Copy(ActionsFromObjects.FromTreeView);

        //    else if (PART_ListView.IsFocused)

        //        Copy(ActionsFromObjects.FromListView);

        //}

        public void Cut(ActionsFromObjects cutFrom)
        {

            var sc = GetFileDropList(cutFrom);

            byte moveEffect = (byte)DragDropEffects.Move; // new byte[] { 2, 0, 0, 0 };


            MemoryStream dropEffect = new MemoryStream();
            dropEffect.WriteByte(moveEffect);

            DataObject data = new DataObject();
            data.SetFileDropList(sc);
            data.SetData("Preferred DropEffect", dropEffect);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

        }

        //public void Cut()

        //{

        //    if (PART_TreeView.IsFocused)

        //        Cut(ActionsFromObjects.FromTreeView);

        //    else if (PART_ListView.IsFocused)

        //        Cut(ActionsFromObjects.FromListView);

        //} 

        protected virtual void OnPaste(bool isAFileMoving, StringCollection sc, string destPath)

        {

#if DEBUG
            Debug.WriteLine("Is a file moving: " + isAFileMoving.ToString());
#endif

            Process process = new Process();

            string args = null;

            args += isAFileMoving ? "\"FileMoving\" " : "\"Copy\" ";

            foreach (string s in sc)

                args += "\"" + s + "\" ";

            args += "\"" + destPath + "\"";

#if DEBUG
            Debug.WriteLine(args);
#endif

            process.StartInfo = new ProcessStartInfo(Generic.WinCopiesProcessesManagerPath, args);

            process.Start();

        }

        public void Paste(ActionsFromObjects pasteTo)

        {

            // if (!PART_TreeView.IsFocused && !PART_ListView.IsFocused) return;

            if (!Clipboard.ContainsFileDropList()) return;

            string path = null;

            if (pasteTo == ActionsFromObjects.TreeView)

                path = TreeViewSelectedItem.Path;

            else if (pasteTo == ActionsFromObjects.ListView)

                path = Path.Path;

            StringCollection sc = Clipboard.GetFileDropList();

            bool isAFileMoving = false;

            if (Clipboard.ContainsData("Preferred DropEffect"))

            {

                var dropEffect = (MemoryStream)Clipboard.GetData("Preferred DropEffect");

                var moveEffect = (DragDropEffects)dropEffect.ReadByte();

                isAFileMoving = moveEffect == DragDropEffects.Move;

            }

        }

        //public void Paste()

        //{

        //    if (PART_TreeView.IsFocused)

        //        Paste(ActionsFromObjects.FromTreeView);

        //    else if (PART_ListView.IsFocused)

        //        Paste(ActionsFromObjects.FromListView);

        //}

        private void CanWrite_CanExecute(object sender, CanExecuteRoutedEventArgs e)

        {

            // todo

        }

        private void CreateShortcut_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            // todo

        }

        private void Rename_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            // todo

        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            // todo

        }

        private void Properties_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            // todo

        }

    }

    public static class Commands

    {

        public static RoutedUICommand Open { get; } = new RoutedUICommand(Generic.Open, nameof(Open), typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.Enter) });

        public static RoutedUICommand EditProperty { get; } = new RoutedUICommand(Generic.EditProperty, nameof(EditProperty), typeof(Commands));

        // todo:

        public static DelegateCommand TrucMuche { get; } = new DelegateCommand((object obj) =>
          {
              var _obj = (IBrowsableObjectInfo)obj;

              if (!_obj.AreItemsLoaded)

                  _obj.LoadItems(true, false, FileTypesFlags.Folder | FileTypesFlags.Drive | FileTypesFlags.Archive);

              // MessageBox.Show(obj.ToString());
          });

        // todo:

        public static RoutedUICommand SelectFile { get; } = new RoutedUICommand(nameof(SelectFile), nameof(SelectFile), typeof(Commands));

        //public static readonly RoutedUICommand OpenInNewTab = new RoutedUICommand((string)Application.Current.Resources["OpenInNewTab"], "OpenInNewTab", typeof(Commands));

        //public static readonly RoutedUICommand OpenInNewWindow = new RoutedUICommand((string)Application.Current.Resources["OpenInNewWindow"], "OpenInNewWindow", typeof(Commands));

        //public static readonly RoutedUICommand OpenInNewInstanceWindow = new RoutedUICommand((string)Application.Current.Resources["OpenInNewInstanceWindow"], "OpenInNewInstanceWindow", typeof(Commands));

        //public static readonly RoutedUICommand CreateShortcut = new RoutedUICommand((string)Application.Current.Resources["CreateShortcut"], "CreateShortcut", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.L, ModifierKeys.Control) });

        //public static readonly RoutedUICommand PasteShortcut = new RoutedUICommand((string)Application.Current.Resources["PasteShortcut"], "PasteShortcut", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Alt) });

        //public static readonly RoutedUICommand Rename = new RoutedUICommand((string)Application.Current.Resources["Rename"], "Rename", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.F2) });

        //public static readonly RoutedUICommand Delete = new RoutedUICommand((string)Application.Current.Resources["Delete"], "Delete", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.Delete) });

        //public static readonly RoutedUICommand Properties = new RoutedUICommand((string)Application.Current.Resources["Properties"], "Properties", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.Enter, ModifierKeys.Alt) });

    }
}
