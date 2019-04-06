using System.Windows;
using System.Windows.Input;
using WinCopies.GUI.Explorer;
using WinCopies.IO;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Windows.Dialogs
{

    public enum FoldersBrowserDialogMode
    {

        OpenFiles, OpenFolder, Save

    }

    public struct FoldersBrowserDialogFilter
    {

        public string FilterName { get; set; }

        public string Filter { get; set; }

        public override string ToString() => string.Format("{0} ({1})", FilterName, Filter);

        public override bool Equals(object obj) => obj is FoldersBrowserDialogFilter _obj ? _obj.Filter == Filter : false;

    }

    public class FoldersBrowserDialog : Window, ICommandSource
    {

        /// <summary>
        /// Identifies the <see cref="ExplorerControl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExplorerControlProperty = DependencyProperty.Register(nameof(ExplorerControl), typeof(ExplorerControl), typeof(FoldersBrowserDialog), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        {

            ExplorerControl explorerControl = (ExplorerControl)e.NewValue;

            explorerControl.Command = Commands.SelectFile;

            explorerControl.CommandBindings.Add(new CommandBinding(Commands.SelectFile, ((FoldersBrowserDialog)d).SelectFileCommand_Executed, ((FoldersBrowserDialog)d).SelectFileCommand_CanExecute));

        }));

        public ExplorerControl ExplorerControl { get => (ExplorerControl)GetValue(ExplorerControlProperty); set => SetValue(ExplorerControlProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(FoldersBrowserDialog));

        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        /// <summary>
        /// Identifies the <see cref="BottomContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BottomContentProperty = DependencyProperty.Register(nameof(BottomContent), typeof(object), typeof(FoldersBrowserDialog));

        public object BottomContent { get => GetValue(BottomContentProperty); set => SetValue(BottomContentProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Mode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(FoldersBrowserDialogMode), typeof(FoldersBrowserDialog));

        public FoldersBrowserDialogMode Mode { get => (FoldersBrowserDialogMode)GetValue(ModeProperty); set => SetValue(ModeProperty, value); }

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(FoldersBrowserDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command that will be executed when the command source is invoked.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(FoldersBrowserDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents a user defined data value that can be passed to the command when it is executed.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(FoldersBrowserDialog), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents the object that the command is being executed on.
        /// </summary>
        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }



        static FoldersBrowserDialog() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FoldersBrowserDialog), new FrameworkPropertyMetadata(typeof(FoldersBrowserDialog)));



        public FoldersBrowserDialog() => Init(FoldersBrowserDialogMode.OpenFiles, false, System.Reflection.Assembly.GetEntryAssembly().GetName().Name);

        public FoldersBrowserDialog(FoldersBrowserDialogMode mode, bool allowMultipleSelection) => Init(mode, allowMultipleSelection, mode == FoldersBrowserDialogMode.OpenFiles ? allowMultipleSelection ? Explorer.Themes.Generic.OpenFiles : Explorer.Themes.Generic.OpenFile : mode == FoldersBrowserDialogMode.OpenFolder ? Explorer.Themes.Generic.OpenFolder : mode == FoldersBrowserDialogMode.Save ? Explorer.Themes.Generic.Save : System.Reflection.Assembly.GetEntryAssembly().GetName().Name);

        public FoldersBrowserDialog(string title) => Init(FoldersBrowserDialogMode.OpenFiles, false, title);

        public FoldersBrowserDialog(FoldersBrowserDialogMode mode, bool allowMultipleSelection, string title) => Init(mode, allowMultipleSelection, title);

        private void Init(FoldersBrowserDialogMode mode, bool allowMultipleSelection, string title)

        {

            ExplorerControl = new ExplorerControl();

            Mode = mode;

            ExplorerControl.SelectionMode = allowMultipleSelection;

            Title = title;

            CommandBindings.Add(new CommandBinding(CommonCommand, OnCommandExecuted, OnCommandCanExecute));

        }



        protected void OnCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = e.CanExecute = e.Parameter is bool result ? !result || Command == null ? true : Command.CanExecute(CommandParameter) : true;

        protected void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)

        {

            if (e.Parameter is bool result)

            {

                Explorer.IBrowsableObjectInfo selectedItem = ExplorerControl.ListViewSelectedItem;

                if (result && selectedItem != null && (selectedItem.FileType == FileTypes.Folder || (selectedItem.FileType == FileTypes.SpecialFolder && selectedItem is Explorer.ShellObjectInfo so && so.ShellObject.IsFileSystemObject) || selectedItem.FileType == FileTypes.Drive))

                {

                    ExplorerControl.Open(selectedItem);

                    return;

                }

                CloseWindowWithDialogResult(result);

            }

        }

        protected void CloseWindowWithDialogResult(bool dialogResult)

        {

            DialogResult = dialogResult;

            if (dialogResult)

                Command.Execute(CommandParameter, CommandTarget);

            Close();

        }

        private void SelectFileCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void SelectFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)

        {

            if ((If(ComparisonType.Or, Comparison.Equals, Mode, FoldersBrowserDialogMode.OpenFiles, FoldersBrowserDialogMode.Save) && If(ComparisonType.Or, Comparison.Equals, ExplorerControl.ListViewSelectedItem.FileType, FileTypes.Folder, FileTypes.SpecialFolder, FileTypes.Link)) || Mode == FoldersBrowserDialogMode.OpenFolder)

                ExplorerControl.Open();

        }

    }
}
