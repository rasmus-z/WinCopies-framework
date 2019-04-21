using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WinCopies.GUI.Controls
{
    public class TreeViewItem : System.Windows.Controls.TreeViewItem, ISelectable
    {

        //public static readonly DependencyProperty EnableClickEventProperty = DependencyProperty.Register(nameof(EnableClickEvent), typeof(bool), typeof(TreeViewItem));

        //public bool EnableClickEvent { get => (bool)GetValue(EnableClickEventProperty); set => SetValue(EnableClickEventProperty, value); }

        private static readonly DependencyPropertyKey IsFocusSelectionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsFocusSelection), typeof(bool), typeof(TreeViewItem), new PropertyMetadata());

        public static readonly DependencyProperty IsFocusSelectionProperty = IsFocusSelectionPropertyKey.DependencyProperty;

        public bool IsFocusSelection => (bool)GetValue(IsFocusSelectionProperty);

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TreeViewItem));

        public event RoutedEventHandler Click { add => AddHandler(ClickEvent, value); remove => RemoveHandler(ClickEvent, value); }

        static TreeViewItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));

        public TreeViewItem() => Click += TreeViewItem_Click;

        protected virtual void OnClick(RoutedEventArgs e) => SetValue(IsFocusSelectionPropertyKey, false);

        private void TreeViewItem_Click(object sender, RoutedEventArgs e) => OnClick(e);

        protected override DependencyObject GetContainerForItemOverride() => new TreeViewItem();

        protected override void OnSelected(RoutedEventArgs e)
        {

            base.OnSelected(e);

            SetValue(IsFocusSelectionPropertyKey, true);

        }

        protected override void OnUnselected(RoutedEventArgs e)
        {

            base.OnUnselected(e);

            SetValue(IsFocusSelectionPropertyKey, false);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            base.OnKeyDown(e);

            if (e.Key == Key.Enter)

                SetValue(IsFocusSelectionPropertyKey, false);

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            base.OnMouseDown(e);

            CaptureMouse();

        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

            base.OnMouseUp(e);

            if (IsMouseCaptured)

            {

                RaiseEvent(new RoutedEventArgs(ClickEvent));

                ReleaseMouseCapture();

            }
        }
    }
}
