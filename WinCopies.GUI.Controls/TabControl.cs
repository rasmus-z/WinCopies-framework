using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.Controls
{
    /// <summary>
    /// A <see cref="System.Windows.Controls.TabControl"/> with advanced multiple items gesture.
    /// </summary>
    public class TabControl : System.Windows.Controls.TabControl
    {
        private ScrollViewer PART_ScrollViewer = null;

        private ItemsPresenter PART_ScrollViewer_ItemsPresenter = null;

        private List<TabItem> tabItems = null;

        private static readonly DependencyPropertyKey CanScrollToLeftPropertyKey = DependencyProperty.RegisterReadOnly("CanScrollToLeft", typeof(bool), typeof(TabControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="CanScrollToLeft"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanScrollToLeftProperty = CanScrollToLeftPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value indicating if items can scroll to left. This is a dependency property.
        /// </summary>
        public bool CanScrollToLeft => (bool)GetValue(CanScrollToLeftProperty);

        private static readonly DependencyPropertyKey CanScrollToRightPropertyKey = DependencyProperty.RegisterReadOnly("CanScrollToRight", typeof(bool), typeof(TabControl), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="CanScrollToRight"/> dependencyc property.
        /// </summary>
        public static readonly DependencyProperty CanScrollToRightProperty = CanScrollToRightPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value indicating if items can scroll to right. This is a dependency property.
        /// </summary>
        public bool CanScrollToRight => (bool)GetValue(CanScrollToRightProperty);

        public static readonly DependencyProperty MovingStepOnScrollButtonUserDoubleClickProperty = DependencyProperty.Register("MovingStepOnScrollButtonUserDoubleClick", typeof(int), typeof(TabControl), new PropertyMetadata(10));

        public int MovingStepOnScrollButtonUserDoubleClick

        {

            get => (int)GetValue(MovingStepOnScrollButtonUserDoubleClickProperty);

            set => SetValue(MovingStepOnScrollButtonUserDoubleClickProperty, value);

        }

        // todo: do not reducing this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also MenuItemData for the other use of this class in this project.

        /// <summary>
        /// Identifies the <see cref="TabMenu"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabMenuProperty = DependencyProperty.Register("TabMenu", typeof(PopupMenuButton), typeof(TabControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the menu items for tab items gesture. This is a dependency property.
        /// </summary>
        public PopupMenuButton TabMenu

        {

            get => (PopupMenuButton)GetValue(TabMenuProperty);

            set => SetValue(TabMenuProperty, value);

        }

        static TabControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()

        {

            // OnApplyTemplate();

            ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //ObservableCollection<MenuItemData> menuItems = new ObservableCollection<MenuItemData>();

            //menuItems.Add(new MenuItemData() { Header = ResourcesHelper.ResourceDictionary["RecentlyClosedTabs"], Items = new ObservableCollection<MenuItemData>() });

            //// todo: really needed? :

            //PART_ScrollViewer = (ScrollViewer)Template.FindName("PART_ScrollViewer", this);

            //PART_ScrollViewer_ItemsPresenter = PART_ScrollViewer.Content as ItemsPresenter;

            //if (PART_ScrollViewer != null && PART_ScrollViewer_ItemsPresenter != null)

            //    tabItems = new List<TabItem>();
        }

        protected virtual void OnItemContainerGeneratorItemsChanged(System.Windows.Controls.Primitives.ItemsChangedEventArgs e)

        {

            object o = null;

            switch (e.Action)

            {

                case NotifyCollectionChangedAction.Add:

                    Debug.WriteLine("OnItemsChangedFromItemContainerGenerator add");

                    bool foundVisibleItem = false;

                    bool checkIfScrollingIsAllowed(int index, DependencyPropertyKey dependencyProperty)

                    {

                        o = ItemContainerGenerator.ContainerFromIndex(index);

                        if (o is FrameworkElement)

                        {

                            if (((FrameworkElement)o).IsUserVisible(this))

                            {

#if DEBUG 

                                Debug.WriteLine("((FrameworkElement)o).IsUserVisible(this) = true");

#endif

                                foundVisibleItem = true;

                            }

                            else if (foundVisibleItem)

                            {

#if DEBUG 

                                Debug.WriteLine("((FrameworkElement)o).IsUserVisible(this) = false");

#endif

                                SetValue(dependencyProperty, true);

                                return true;

                            }

                        }

                        return false;

                    }

                    for (int i = ItemContainerGenerator.Items.Count - 1; i >= 0; i--)

                        if (checkIfScrollingIsAllowed(i, CanScrollToLeftPropertyKey))

                            break;

                    foundVisibleItem = false;

                    for (int i = 0; i <= ItemContainerGenerator.Items.Count - 1; i++)

                        if (checkIfScrollingIsAllowed(i, CanScrollToRightPropertyKey))

                            break;

                    break;

                case NotifyCollectionChangedAction.Remove:



                    break;

                case NotifyCollectionChangedAction.Reset:

                    SetValue(CanScrollToLeftPropertyKey, false);

                    SetValue(CanScrollToRightPropertyKey, false);

                    break;

            }

        }

        private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {

            OnItemContainerGeneratorItemsChanged(e);

        }

        //protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    base.OnItemsChanged(e);
        //}

        public void MoveToLeft(int step)

        {

            object o = null;

            object o_previous = null;

            int o_previousIndex = 0;

            for (int i = 1; i <= ItemContainerGenerator.Items.Count - 1; i++)

            {

                o = ItemContainerGenerator.ContainerFromIndex(i);

                if (o is FrameworkElement && ((FrameworkElement)o).IsUserVisible(this))

                {

                    // todo: try to reach the first non-visible FrameworkElement in order to bring it into view instead of this:

                    o_previousIndex = i < step ? 0 : i - step;

                    for (int _i = o_previousIndex; _i <= i - 1; _i++)

                    {

                        o_previous = ItemContainerGenerator.ContainerFromIndex(_i);

                        if (o_previous is FrameworkElement)

                        {

                            ((FrameworkElement)o_previous).BringIntoView();

                            //if ( _i == 0) 

                            //{

                            //    SetValue(CanScrollToLeftPropertyKey, false);

                            //    return;

                            //}

                            //else

                            //{

                            for (int j = _i - 1; j >= 0; j--)

                            {

                                o = ItemContainerGenerator.ContainerFromIndex(j);

                                if (o is FrameworkElement /* && !((FrameworkElement)o).IsUserVisible(this) */ )

                                    return;

                            }

                            SetValue(CanScrollToLeftPropertyKey, false);

                            return;

                            //}

                        }

                        //else

                        //    return;

                    }

                    return;

                }

            }

        }

        protected virtual void OnScrollToLeftButtonClick() => MoveToLeft(1);

        public void OnScrollToLeftButtonClickInternal() => OnScrollToLeftButtonClick();

        protected virtual void OnScrollToLeftButtonMouseDoubleClick() => MoveToLeft(MovingStepOnScrollButtonUserDoubleClick);

        public void OnScrollToLeftButtonMouseDoubleClickInternal() => OnScrollToLeftButtonMouseDoubleClick();

        public void MoveToRight(int step)

        {

            object o = null;

            object o_next = null;

            int o_nextIndex = 0;

            for (int i = ItemContainerGenerator.Items.Count - 2; i >= 0; i--)

            {

                o = ItemContainerGenerator.ContainerFromIndex(i);

                if (o is FrameworkElement && ((FrameworkElement)o).IsUserVisible(this))

                {

                    // todo: try to reach the first non-visible FrameworkElement in order to bring it into view instead of this:

                    o_nextIndex = ItemContainerGenerator.Items.Count - 1 - i < step ? ItemContainerGenerator.Items.Count - 1 : i + step;

                    for (int _i = o_nextIndex; _i >= i + 1; _i--)

                    {

                        o_next = ItemContainerGenerator.ContainerFromIndex(_i);

                        if (o_next is FrameworkElement)

                        {

                            ((FrameworkElement)o_next).BringIntoView();

                            //if (_i == ItemContainerGenerator.Items.Count - 1)

                            //{

                            //    SetValue(CanScrollToRightPropertyKey, false);

                            //    return;

                            //}

                            //else

                            //{

                            for (int j = _i + 1; j <= ItemContainerGenerator.Items.Count - 1; j++)

                            {

                                o = ItemContainerGenerator.ContainerFromIndex(j);

                                if (o is FrameworkElement /* && !((FrameworkElement)o).IsUserVisible(this) */ )

                                    return;

                            }

                            SetValue(CanScrollToLeftPropertyKey, false);

                            return;

                            //}

                        }

                        //else

                        //    return;

                    }

                    return;

                }

            }

        }

        protected virtual void OnScrollToRightButtonClick() => MoveToRight(1);

        public void OnScrollToRightButtonClickInternal() => OnScrollToRightButtonClick();

        protected void OnScrollToRightButtonMouseDoubleClick() => MoveToRight(MovingStepOnScrollButtonUserDoubleClick);

        public void OnScrollToRightButtonMouseDoubleClickInternal() => OnScrollToRightButtonMouseDoubleClick();

    }
}
