using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util;

namespace WinCopies.GUI.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : DialogWindow
    {

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(InputBox), new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Orientation"/> of the label and the text box. This is a dependency property.
        /// </summary>
        public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(object), typeof(InputBox));

        /// <summary>
        /// Gets or sets the label text for the user. This is a dependency property.
        /// </summary>
        public object Label { get => GetValue(LabelProperty); set => SetValue(LabelProperty, value); } 

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(InputBox), new PropertyMetadata(null

#if DEBUG 
            , (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {

                Debug.WriteLine("TextProperty value changed : " + e.NewValue);

            }

#endif 

            ));

        /// <summary>
        /// Gets or sets the text of the text box. This is a dependency property.
        /// </summary>
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        /// <summary>
        /// Identifies the <see cref="PlaceholderMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderModeProperty = DependencyProperty.Register(nameof(PlaceholderMode), typeof(Controls.PlaceholderMode), typeof(InputBox), new PropertyMetadata(Controls.PlaceholderMode.OnFocus));

        /// <summary>
        /// Gets or sets the <see cref="Controls.PlaceholderMode"/> for the text box. This is a dependency property.
        /// </summary>
        public Controls.PlaceholderMode PlaceholderMode { get => (Controls.PlaceholderMode)GetValue(PlaceholderModeProperty); set => SetValue(PlaceholderModeProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Placeholder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(Controls.PlaceholderProperties), typeof(InputBox));

        /// <summary>
        /// Gets or sets the <see cref="Controls.PlaceholderProperties"/> for the text box. This is a dependency property.
        /// </summary>
        public Controls.PlaceholderProperties Placeholder { get => (Controls.PlaceholderProperties)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ErrorText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(InputBox), new PropertyMetadata(null));    

        /// <summary>
        /// Gets or sets the error text. This is a dependency property.
        /// </summary>
        public string ErrorText { get => (string)GetValue(ErrorTextProperty); set => SetValue(ErrorTextProperty, value);

        }

        /// <summary>
        /// Identifies the <see cref="TextChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(nameof(TextChanged), RoutingStrategy.Bubble, typeof(TextChangedEventHandler), typeof(InputBox));

        /// <summary>
        /// Occurs when content changes in the text element.
        /// </summary>
        /// <remarks>
        /// This event occurs when the text changes and it does not take care of the formatting.
        /// </remarks>
        public event TextChangedEventHandler TextChanged

        {

            add => AddHandler(TextChangedEvent, value);

            remove => RemoveHandler(TextChangedEvent, value);

        }

        static InputBox() =>
            // DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputBox), new FrameworkPropertyMetadata(typeof(InputBox)));// InputBox.StyleProperty.OverrideMetadata(typeof(InputBox), new FrameworkPropertyMetadata((Style) Application.Current. Resources["abcd"]));

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        public InputBox() => Content = /*new Label { Content = "a" };*/ new Control { Template = (ControlTemplate)ResourcesHelper.Instance.ResourceDictionary["InputBoxTemplate"] };

        // /// <summary>
        // /// Initialize a new instance of the <see cref="InputBox"/> window.
        // /// </summary>
        // public InputBox() =>
        // InitializeComponent();

        // CommandBindings.Add(new CommandBinding(Util.Util.CommonCommand, OnTextChangedInternal));

        /// <summary>
        /// Is called when content in this editing control changes.
        /// </summary>
        /// <param name="e">The arguments that are associated with the <see cref="TextChanged"/> event.</param>
        /// <remarks>
        /// This method raises a <see cref="TextChanged"/> event.
        /// </remarks>
        protected virtual void OnTextChanged(TextChangedEventArgs e)

        {

            Command?.CanExecute(CommandParameter, CommandTarget);

            e.RoutedEvent = TextChangedEvent;

            RaiseEvent(e);    

        } 

        protected override void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)    

        {

            if (e.Parameter is TextChangedEventArgs _e) 

                OnTextChanged(_e); 

            else

                base.OnCommandExecuted(sender, e);

        } 
    }
}
