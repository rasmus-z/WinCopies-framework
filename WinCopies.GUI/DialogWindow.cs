using AttachedCommandBehavior;
using System;
using System.Windows;
using System.Windows.Input;
using WinCopies.Util;
using WinCopies.Util.Commands;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Windows.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour DialogBase.xaml. See the remarks section.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="CommonCommand"/> with the following string parameters: OK, Apply, Cancel, Yes, No. For more information about this behavior/design pattern, see the WinCopies website.
    /// </remarks>
    public partial class DialogWindow : Window, ICommandSource
    {

        /// <summary>
        /// Identifies the <see cref="DialogButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DialogButtonProperty = DependencyProperty.Register(nameof(DialogButton), typeof(DialogButton), typeof(DialogWindow), new PropertyMetadata(DialogButton.OK, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        {
            // MessageBox.Show(e.NewValue.ToString());
            if (((DialogWindow)d).DefaultButton != DefaultButton.None) throw new Exception("DefaultButton must be set to None in order to perform this action.");

        }));

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/>. This is a dependency property.
        /// </summary>
        public DialogButton DialogButton { get => (DialogButton)GetValue(DialogButtonProperty); set => SetValue(DialogButtonProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ButtonAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register(nameof(ButtonAlignment), typeof(HorizontalAlignment), typeof(DialogWindow), new PropertyMetadata(Dialogs.HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets the <see cref="HorizontalAlignment"/> for the button alignment. This is a dependency property.
        /// </summary>
        public HorizontalAlignment ButtonAlignment { get => (HorizontalAlignment)GetValue(ButtonAlignmentProperty); set => SetValue(ButtonAlignmentProperty, value); }

        public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register(nameof(DefaultButton), typeof(DefaultButton), typeof(DialogWindow), new PropertyMetadata(DefaultButton.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        {

            var defaultButton = (DefaultButton)e.NewValue;

            if (defaultButton == DefaultButton.None) return;

            var dialogButton = (DialogButton)d.GetValue(DialogButtonProperty);

            void throwArgumentException() => throw new ArgumentException("DefaultButton must be included in DialogButton value.");

            switch (dialogButton)

            {

                case DialogButton.OK:

                    if (defaultButton != DefaultButton.OK) throwArgumentException();

                    break;

                case DialogButton.OKCancel:

                    if (defaultButton != DefaultButton.OK && defaultButton != DefaultButton.Cancel) throwArgumentException();

                    break;

                case DialogButton.OKApplyCancel:

                    if (defaultButton != DefaultButton.OK && defaultButton != DefaultButton.Apply && defaultButton != DefaultButton.Cancel) throwArgumentException();

                    break;

                case DialogButton.YesNoCancel:

                    if (defaultButton != DefaultButton.Yes && defaultButton != DefaultButton.No && defaultButton != DefaultButton.Cancel) throwArgumentException();

                    break;

                case DialogButton.YesNo:

                    if (defaultButton != DefaultButton.Yes && defaultButton != DefaultButton.No) throwArgumentException();

                    break;

            }

        }));

        public DefaultButton DefaultButton { get => (DefaultButton)GetValue(DefaultButtonProperty); set => SetValue(DefaultButtonProperty, value); }

        /// <summary>
        /// Gets the <see cref="System.Windows.MessageBoxResult"/>.
        /// </summary>
        public MessageBoxResult MessageBoxResult { get; private set; } = MessageBoxResult.None;

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command that will be executed when the command source is invoked.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents a user defined data value that can be passed to the command when it is executed.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents the object that the command is being executed on.
        /// </summary>
        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }



        static DialogWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));



        public DialogWindow() =>
            // InitializeComponent();

            // ActionCommand = new WinCopies.Util.DelegateCommand(ActionCommandMethod);

            Init(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);

        public DialogWindow(string title) => Init(title);

        private void Init(string title)

        {

            Title = title;

            CommandBindings.Add(new CommandBinding(CommonCommand, OnCommandExecuted, OnCommandCanExecute));

        }

        protected virtual void OnCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = e.Parameter is DialogWindowCommandParameters parameter ? If(ComparisonType.Or, Comparison.Equals,  parameter ,DialogWindowCommandParameters.Cancel , DialogWindowCommandParameters.No) || (parameter == DialogWindowCommandParameters.OK && DialogButton == DialogButton.OK ) ||     Command == null ? true : Command.CanExecute(CommandParameter) : true;

        protected virtual void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is DialogWindowCommandParameters d)

                switch (d)

                {

                    case DialogWindowCommandParameters.OK:

                        CloseWindowWithDialogResult(true, MessageBoxResult.OK);

                        break;

                    case DialogWindowCommandParameters.Apply:

                        Command.TryExecute(CommandParameter, CommandTarget);

                        break;

                    case DialogWindowCommandParameters.Cancel:

                        CloseWindowWithDialogResult(false, MessageBoxResult.Cancel);

                        break;

                    case DialogWindowCommandParameters.Yes:

                        CloseWindowWithDialogResult(true, MessageBoxResult.Yes);

                        break;

                    case DialogWindowCommandParameters.No:

                        CloseWindowWithDialogResult(false, MessageBoxResult.No);

                        break;

                }
        }

        protected void CloseWindowWithDialogResult(bool dialogResult, MessageBoxResult messageBoxResult)

        {

            DialogResult = dialogResult;

            MessageBoxResult = messageBoxResult;

            if ( DialogButton != DialogButton.OK &&     dialogResult && Command != null)    

               Command.Execute(CommandParameter, CommandTarget);

            Close();

        }

    }
}
