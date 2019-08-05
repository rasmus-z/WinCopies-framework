using AttachedCommandBehavior;
using System;
using System.Windows;
using System.Windows.Input;
using WinCopies.Util;
using WinCopies.Util.Commands;
using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using System.Windows.Controls;

namespace WinCopies.GUI.Windows.Dialogs
{
    /// <summary>
    /// Represents a common dialog window for WPF.
    /// </summary>
    [TemplatePart( Name = PART_OkButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ApplyButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_YesButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NoButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
    public partial class DialogWindow : Window, ICommandSource
    {

        private const string PART_OkButton = "PART_OkButton";
        private const string PART_ApplyButton = "PART_ApplyButton";
        private const string PART_YesButton = "PART_YesButton";
        private const string PART_NoButton = "PART_NoButton";
        private const string PART_CancelButton = "PART_CancelButton";

        /// <summary>
        /// Identifies the <see cref="DialogButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DialogButtonProperty = DependencyProperty.Register(nameof(DialogButton), typeof(DialogButton), typeof(DialogWindow), new PropertyMetadata(DialogButton.OK, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        {
            // MessageBox.Show(e.NewValue.ToString());
            if (((DialogWindow)d).DefaultButton != DefaultButton.None) throw new InvalidOperationException($"{nameof(DefaultButton)} must be set to {nameof(DefaultButton.None)} in order to perform this action.");

            // ((DialogWindow)d).DefaultButton = DefaultButton.None;

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



        public DialogWindow() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name) { }
            // InitializeComponent();

            // ActionCommand = new WinCopies.Util.DelegateCommand(ActionCommandMethod);

        public DialogWindow(string title) => Title = title;

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            switch (DialogButton)

            {

                case DialogButton.OK:

                    _ = (GetTemplateChild(PART_OkButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.OKCancel:

                    _ = (GetTemplateChild(PART_OkButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.OKApplyCancel:

                    _ = (GetTemplateChild(PART_OkButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_ApplyButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesNo:

                    _ = (GetTemplateChild(PART_YesButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_NoButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesNoCancel:

                    _ = (GetTemplateChild(PART_YesButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_NoButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

                    break;

            }

        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnCommandCanExecute(e);

        protected virtual void OnCommandCanExecute(CanExecuteRoutedEventArgs e) => e.CanExecute = e.Parameter is DialogWindowCommandParameters parameter ? If(IfCT.Or, IfCM.Logical, IfComp.Equal, parameter, DialogWindowCommandParameters.Cancel, DialogWindowCommandParameters.No) || (parameter == DialogWindowCommandParameters.OK && DialogButton == DialogButton.OK) || Command == null ? true : Command.CanExecute(CommandParameter) : true;

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e) => OnCommandExecuted(e);

        protected virtual void OnCommandExecuted(ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is DialogWindowCommandParameters d)

                switch (d)

                {

                    case DialogWindowCommandParameters.OK:

                        CloseWindowWithDialogResult(true, MessageBoxResult.OK);

                        break;

                    case DialogWindowCommandParameters.Apply:

                        _ = Command.TryExecute(CommandParameter, CommandTarget);

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

            if (DialogButton != DialogButton.OK && dialogResult && Command != null)

                Command.Execute(CommandParameter, CommandTarget);

            Close();

        }

    }
}
