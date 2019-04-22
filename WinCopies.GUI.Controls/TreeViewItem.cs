﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using WinCopies.Util;

namespace WinCopies.GUI.Controls
{
    /// <summary>
    /// Implements a selectable item in a System.Windows.Controls.TreeView control.
    /// </summary>
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(TreeViewItem))]
    public class TreeViewItem : System.Windows.Controls.TreeViewItem, ISelectable
    {

        //public static readonly DependencyProperty EnableClickEventProperty = DependencyProperty.Register(nameof(EnableClickEvent), typeof(bool), typeof(TreeViewItem));

        //public bool EnableClickEvent { get => (bool)GetValue(EnableClickEventProperty); set => SetValue(EnableClickEventProperty, value); }

        private static readonly DependencyPropertyKey IsFocusSelectionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsFocusSelection), typeof(bool), typeof(TreeViewItem), new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="IsFocusSelection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFocusSelectionProperty = IsFocusSelectionPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeViewItem"/> is selected by focus.
        /// </summary>
        public bool IsFocusSelection => (bool)GetValue(IsFocusSelectionProperty);

        internal static readonly DependencyPropertyKey IsPreviouslySelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsPreviouslySelectedItem), typeof(bool), typeof(TreeViewItem), new PropertyMetadata());

        /// <summary>
        /// Identifies the <see cref="IsPreviouslySelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPreviouslySelectedItemProperty = IsPreviouslySelectedItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeViewItem"/> 
        /// </summary>
        public bool IsPreviouslySelectedItem => (bool)GetValue(IsPreviouslySelectedItemProperty);

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TreeViewItem));

        /// <summary>
        /// Occurs when the user clicks or push the Enter key on this <see cref="TreeViewItem"/>.
        /// </summary>
        public event RoutedEventHandler Click { add => AddHandler(ClickEvent, value); remove => RemoveHandler(ClickEvent, value); }

        internal TreeView _parentTreeView = null;

        static TreeViewItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class.
        /// </summary>
        public TreeViewItem() => Click += TreeViewItem_Click;

        /// <summary>
        /// Called when this <see cref="TreeViewItem"/> is clicked.
        /// </summary>
        /// <param name="e">The event parameters.</param>
        protected virtual void OnClick(RoutedEventArgs e) =>

            // this.GetParent<TreeView>(false)._isFocusSelection = false;

            SetValue(IsFocusSelectionPropertyKey, false);

        private void TreeViewItem_Click(object sender, RoutedEventArgs e) => OnClick(e);

        /// <summary>
        /// Creates a new <see cref="TreeViewItem"/> to use to display the object.
        /// </summary>
        /// <returns>A new <see cref="TreeViewItem"/> to use to display the object.</returns>
        protected override DependencyObject GetContainerForItemOverride() => new TreeViewItem();

        /// <summary>
        /// Raises the <see cref="System.Windows.Controls.TreeViewItem.Selected"/> routed event when the
        ///     <see cref="System.Windows.Controls.TreeViewItem.IsSelected"/> property changes from <see langword="false"/> to
        ///     <see langword="true"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSelected(RoutedEventArgs e)
        {

            base.OnSelected(e);

            if (_parentTreeView != null)

            {

                SetValue(IsFocusSelectionPropertyKey, _parentTreeView._isFocusSelection);

                // _parentTreeView._isFocusSelection = false;

            }

        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Controls.TreeViewItem.Unselected"/> routed event when
        ///     the <see cref="System.Windows.Controls.TreeViewItem.IsSelected"/> property changes from <see langword="true"/>
        ///     to <see langword="false"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnUnselected(RoutedEventArgs e)
        {

            base.OnUnselected(e);

            SetValue(IsFocusSelectionPropertyKey, false);

        }

        /// <summary>
        /// Provides class handling for a <see cref="UIElement.KeyDown"/> event.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (_parentTreeView != null)

            {

                _parentTreeView._isFocusSelection = e.Key != Key.Enter;

                SetValue(IsFocusSelectionPropertyKey, _parentTreeView._isFocusSelection);

            }

            base.OnKeyDown(e);

        }

        /// <summary>
        /// Invoked when an unhandled System.Windows.Input.Mouse.MouseDown attached event
        ///     reaches an element in its route that is derived from this class. Implement this
        ///     method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> that contains the event data. This
        ///     event data reports details about the mouse button that was pressed and the handled
        ///     state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            base.OnMouseDown(e);

            if (_parentTreeView != null)

                _parentTreeView._isFocusSelection = true;

            CaptureMouse();

        }

        /// <summary>
        /// Invoked when an unhandled System.Windows.Input.Mouse.MouseUp routed event reaches
        ///     an element in its route that is derived from this class. Implement this method
        ///     to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> that contains the event data. The
        ///     event data reports that the mouse button was released.</param>
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
