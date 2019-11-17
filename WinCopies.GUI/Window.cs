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

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.ShellExtensions;
using Microsoft.WindowsAPICodePack.ShellExtensions.Interop;
using Microsoft.WindowsAPICodePack.Win32Native.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WinCopies.GUI.Windows
{
    public class Window : System.Windows.Window
    {

        /// <summary>
        /// Identifies the <see cref="ShowHelpButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowHelpButtonProperty = DependencyProperty.Register(nameof(ShowHelpButton), typeof(bool), typeof(Window), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => d.SetValue(IsInHelpModePropertyKey, (bool?)e.NewValue == true ? (bool?)false : null)));

        public bool ShowHelpButton { get => (bool)GetValue(ShowHelpButtonProperty); set => SetValue(ShowHelpButtonProperty, value); }

        private static readonly DependencyPropertyKey IsInHelpModePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsInHelpMode), typeof(bool?), typeof(Window), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="IsInHelpMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInHelpModeProperty = IsInHelpModePropertyKey.DependencyProperty;

        public bool? IsInHelpMode => (bool?)GetValue(IsInHelpModeProperty);

        public static readonly DependencyProperty NotInHelpModeCursorProperty = DependencyProperty.Register(nameof(NotInHelpModeCursor), typeof(Cursor), typeof(Window), new PropertyMetadata(Cursors.Arrow));

        public Cursor NotInHelpModeCursor { get => (Cursor)GetValue(NotInHelpModeCursorProperty); set => SetValue(NotInHelpModeCursorProperty, value); }

        /// <summary>
        /// Identifies the <see cref="HelpButtonClick"/> routed event.
        /// </summary>
        public static RoutedEvent HelpButtonClickEvent = EventManager.RegisterRoutedEvent(nameof(HelpButtonClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Window));

        public event RoutedEventHandler HelpButtonClick

        {

            add => AddHandler(HelpButtonClickEvent, value);

            remove => RemoveHandler(HelpButtonClickEvent, value);

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (ShowHelpButton)
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;

                WindowUtilities.SetWindow(hwnd, IntPtr.Zero, 0, 0, 0, 0, (WindowStyles)(((uint)WindowUtilities.GetWindowStyles(hwnd) & 0xFFFFFFFF) ^ ((uint)WindowStyles.MinimizeBox | (uint)WindowStyles.MaximizeBox)), (WindowStylesEx)((uint)WindowUtilities.GetWindowStylesEx(hwnd) | (uint)WindowStylesEx.ContextHelp), SetWindowPositionOptions.NoMove | SetWindowPositionOptions.NoSize | SetWindowPositionOptions.NoZOrder | SetWindowPositionOptions.FrameChanged);

                ((HwndSource)PresentationSource.FromVisual(this)).AddHook(OnHelpButtonClickHook);

                //IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                //uint styles = GetWindowLongPtr(hwnd, GWL_STYLE);
                //styles &= 0xFFFFFFFF ^ (WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                //SetWindowLongPtr(hwnd, GWL_STYLE, styles);
                //styles = GetWindowLongPtr(hwnd, GWL_EXSTYLE);
                //styles |= WS_EX_CONTEXTHELP;
                //SetWindowLongPtr(hwnd, GWL_EXSTYLE, styles);
                //SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
                //((HwndSource)PresentationSource.FromVisual(this)).AddHook(OnHelpButtonClickHook);
            }
        }

        // todo: really needed?

        protected void RaiseHelpButtonClickEvent() => RaiseEvent(new RoutedEventArgs(HelpButtonClickEvent));

        protected virtual void OnHelpButtonClick()

        {

            if (IsInHelpMode.HasValue)

            {

                SetValue(IsInHelpModePropertyKey, !(bool)IsInHelpMode);

                Cursor = (bool)IsInHelpMode ? Cursors.Help : Cursors.Arrow;

            }

            RaiseHelpButtonClickEvent();

        }

        protected virtual IntPtr OnHelpButtonClickHook(IntPtr hwnd,
                int msg,
                IntPtr wParam,
                IntPtr lParam,
                ref bool handled)
        {
            if (msg == (int)WindowMessage.SystemCommand &&
                    ((int)wParam & 0xFFF0) == (int)SystemCommand.ContextHelp)
            {
                OnHelpButtonClick();
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
