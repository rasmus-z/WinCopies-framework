using System.Windows;

namespace WinCopies.GUI.Controls
{
    public class Label : System.Windows.Controls.Label
    {

        /// <summary>
        /// Identifies the <see cref="RecognizesAccessKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RecognizesAccessKeyProperty = DependencyProperty.Register(nameof(RecognizesAccessKey), typeof(bool), typeof(Label), new PropertyMetadata(true));

        public bool RecognizesAccessKey { get => (bool)GetValue(RecognizesAccessKeyProperty); set => SetValue(RecognizesAccessKeyProperty, value); }



        static Label() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(typeof(Label)));

    }
}
