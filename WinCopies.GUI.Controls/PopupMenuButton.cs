using System.Windows;

namespace WinCopies.GUI.Controls
{
    public class PopupMenuButton : System.Windows.Controls.MenuItem
    {
        //public static readonly DependencyProperty IsMenuOpenProperty = DependencyProperty.Register("IsMenuOpen", typeof(bool), typeof(PopupMenuButton), new PropertyMetadata(false));

        //public bool IsMenuOpen

        //{

        //    get => (bool)GetValue(IsMenuOpenProperty);

        //    set => SetValue(IsMenuOpenProperty, value);

        //}

        static PopupMenuButton() => DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupMenuButton), new FrameworkPropertyMetadata(typeof(PopupMenuButton)));

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is System.Windows.Controls.MenuItem)

                ((System.Windows.Controls.MenuItem)element).Click += PopupMenuButtonMenuItem_Click;
        }

        protected virtual void OnPopupMenuButtonMenuItemClick(RoutedEventArgs e)

        {

            if (!StaysOpenOnClick)

                IsSubmenuOpen = false;

            // todo: IsMouseCaptured etc.

        }

        private void PopupMenuButtonMenuItem_Click(object sender, RoutedEventArgs e) => OnPopupMenuButtonMenuItemClick(e);

        //public PopupMenuButton()

        //{

        //    ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;

        //}

        //protected virtual void OnItemContainerGeneratorItemsChanged(System.Windows.Controls.Primitives.ItemsChangedEventArgs e)

        //{

        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)

        //    {

        //        Debug.WriteLine("PopupMenuButton message: OnItemContainerGeneratorItemsChanged e.Action = Add");

        //        for (int i = e.Position.)

        //    }

        //}

        //private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        //{

        //    OnItemContainerGeneratorItemsChanged(e);

        //}
    }
}
