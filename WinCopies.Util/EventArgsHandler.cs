/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code:  Marlon Grech */

//using System;
//using System.Windows;
//using System.Windows.Controls;

//namespace CommandForEventSample
//{
//    public class EventArgsHandler
//    {
//        #region LastEventArgs

//        public static readonly DependencyProperty LastMouseEventArgsProperty =
//            DependencyProperty.RegisterAttached("LastMouseEventArgs", typeof(EventArgs), typeof(EventArgsHandler),
//                new FrameworkPropertyMetadata((EventArgs)null));

//        public static EventArgs GetLastEventArgs(DependencyObject d) => (EventArgs)d.GetValue(LastEventArgsProperty);

//        public static void SetLastEventArgs(DependencyObject d, EventArgs value) => d.SetValue(LastEventArgsProperty, value);

//        #endregion

//        #region HandleEventHandler

//        public static readonly DependencyProperty HandleEventHandlerProperty =
//            DependencyProperty.RegisterAttached("HandleEventHandler", typeof(bool), typeof(EventHandler),
//                new FrameworkPropertyMetadata(false,
//                    new PropertyChangedCallback(OnEventHandlerChanged)));

//        public static bool GetHandleEventHandlerClick(DependencyObject d) => (bool)d.GetValue(HandleEventHandlerProperty);

//        public static void SetHandleMouseDoubleClick(DependencyObject d, bool value) => d.SetValue(HandleEventHandlerProperty, value);

//        /// <summary>
//        /// Handles changes to the HandleMouseDoubleClick property.
//        /// </summary>
//        private static void OnEventHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            if (d is Control control)
//            {
//                if ((bool)e.NewValue)
//                    control.GetType().GetEvent("").AddEventHandler(control, trucmuche);
//                else
//                    control.MouseDoubleClick -= ControlMouseDoubleClick;
//            }
//        }

//        public static EventHandler trucmuche = new EventHandler((_d, _e) => { ControlMouseDoubleClick((DependencyObject)_d, _e); });

//        static void ControlMouseDoubleClick(object sender, EventArgs e)
//        {
//            SetLastEventArgs((DependencyObject)sender, e);
//        }

//        #endregion
//    }
//}
