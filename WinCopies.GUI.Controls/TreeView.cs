using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util;

// todo: to add command input gesture for mouse actions

namespace WinCopies.GUI.Controls
{

    /// <summary>
    /// Represents a control that displays hierarchical data in a tree structure that
    /// has items that can expand and collapse.
    /// </summary>
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(TreeViewItem))]
    public class TreeView : System.Windows.Controls.TreeView, ISelector, ICommandSource
    {

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TreeView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command of this <see cref="TreeView"/>. This is a dependency property.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(TreeView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="Command"/> property. This is a dependency property.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(TreeView), new PropertyMetadata(null));

        /// <summary>
        /// The object that the command is being executed on.
        /// </summary>
        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }

        /// <summary>
        /// Gets the selected index.
        /// </summary>
        public int SelectedIndex => ItemsSource == null ? Items.IndexOf(SelectedItem) : ItemsSource.ToList().IndexOf(SelectedItem);

        internal static readonly DependencyPropertyKey PreviouslySelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PreviouslySelectedItem), typeof(TreeViewItem), typeof(TreeView), new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="PreviouslySelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviouslySelectedItemProperty = PreviouslySelectedItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the previously selected <see cref="TreeViewItem"/> when a <see cref="TreeViewItem"/> item is selected by focus.
        /// </summary>
        public TreeViewItem PreviouslySelectedItem => (TreeViewItem)GetValue(PreviouslySelectedItemProperty);

        // internal TreeViewItem _selectedTreeViewItem = null;

        internal bool _isFocusSelection = false;

        //public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TreeView));

        //public event RoutedEventHandler Click { add => AddHandler(ClickEvent, value); remove => RemoveHandler(ClickEvent, value); }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView() =>

            // MouseDoubleClick += TreeView_MouseDoubleClick;

            EventManager.RegisterClassHandler(typeof(TreeView), TreeViewItem.ClickEvent, new RoutedEventHandler(TreeView_Click));

        /// <summary>
        /// Called when a child <see cref="TreeViewItem"/> is clicked.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected virtual void OnClick(RoutedEventArgs e)
        {
            if (Command != null && Command.CanExecute(CommandParameter, CommandTarget))

            {

                OnCommandExecute();

                Command.Execute(CommandParameter, CommandTarget);

            }
        }

        protected virtual void OnCommandExecute()

        {

            if (PreviouslySelectedItem == null) return;

            PreviouslySelectedItem.SetValue(TreeViewItem.IsPreviouslySelectedItemPropertyKey, false);

            SetValue(PreviouslySelectedItemPropertyKey, null);

        }

        private void TreeView_Click(object sender, RoutedEventArgs e) => OnClick(e);

        /// <summary>
        /// Creates a <see cref="TreeViewItem"/> to use to display content.
        /// </summary>
        /// <returns>A new <see cref="TreeViewItem"/> to use as a container for content.</returns>
        protected override DependencyObject GetContainerForItemOverride() => new TreeViewItem() { ParentTreeView = this };

        //private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //    if (Command == null) return;

        //    Command.TryExecute(CommandParameter, CommandTarget);

        //}

        /// <summary>
        /// Provides class handling for the <see cref="UIElement.KeyDown"/> event for a
        ///     <see cref="TreeView"/>.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {

            base.OnKeyDown(e);

            if (e.Key == Key.Enter) OnClick(e);

            else if (Command != null && KeyDownCommandHelper.CanRaiseCommand(this, e))

            {

                OnCommandExecute();

                Command.Execute(CommandParameter, CommandTarget);

            }

        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {

            base.OnSelectedItemChanged(e);

            //if (_isFocusSelection)

            //{

            _isFocusSelection = false;

        }
    }
}
