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
        public static readonly DependencyProperty ShowHelpButtonProperty = DependencyProperty.Register(nameof(ShowHelpButton), typeof(bool), typeof(Window), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>

        {

                d.SetValue(IsInHelpModePropertyKey, (bool?)e.NewValue == true ? (bool?)    false :     null);

        }));

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

        private const uint WS_EX_CONTEXTHELP = 0x00000400;
        private const uint WS_MINIMIZEBOX = 0x00020000;
        private const uint WS_MAXIMIZEBOX = 0x00010000;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_CONTEXTHELP = 0xF180;


        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, uint newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (ShowHelpButton)
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                uint styles = GetWindowLong(hwnd, GWL_STYLE);
                styles &= 0xFFFFFFFF ^ (WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                SetWindowLong(hwnd, GWL_STYLE, styles);
                styles = GetWindowLong(hwnd, GWL_EXSTYLE);
                styles |= WS_EX_CONTEXTHELP;
                SetWindowLong(hwnd, GWL_EXSTYLE, styles);
                SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
                ((HwndSource)PresentationSource.FromVisual(this)).AddHook(OnHelpButtonClickHook);
            }
        }

        // todo: really needed?

        protected void RaiseHelpButtonClickEvent() => RaiseEvent(new RoutedEventArgs(HelpButtonClickEvent));

        protected virtual void OnHelpButtonClick()

        {

            if (IsInHelpMode.HasValue)

            {

                SetValue(IsInHelpModePropertyKey, !(bool)IsInHelpMode);

                if ((bool)IsInHelpMode)

                    Cursor = Cursors.Help;

                else

                    Cursor = Cursors.Arrow;    

            }

            RaiseHelpButtonClickEvent();

        }

        protected virtual IntPtr OnHelpButtonClickHook(IntPtr hwnd,
                int msg,
                IntPtr wParam,
                IntPtr lParam,
                ref bool handled)
        {
            if (msg == WM_SYSCOMMAND &&
                    ((int)wParam & 0xFFF0) == SC_CONTEXTHELP)
            {
                OnHelpButtonClick();
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
