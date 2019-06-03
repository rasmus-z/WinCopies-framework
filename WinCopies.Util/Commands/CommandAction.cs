using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace WinCopies.Util.Commands
{
    public class CommandAction : System.Windows.Interactivity.TriggerAction<DependencyObject>
    {


        public static readonly DependencyProperty CommandProperty =
         DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandAction),
         new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
         DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandAction),
         new PropertyMetadata(null, OnCommandParameterChanged));

        private IDisposable canExecuteChanged;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandChanged(DependencyObject sender,
                        DependencyPropertyChangedEventArgs e)
        {
            if (sender is CommandAction ev)
            {
                ev.canExecuteChanged?.Dispose();

                if (e.NewValue is ICommand command)

                    ev.canExecuteChanged = Observable.FromEventPattern(
                        x => command.CanExecuteChanged += x,
                        x => command.CanExecuteChanged -= x).Subscribe
                        (_ => ev.SynchronizeElementState());
            }
        }

        private static void OnCommandParameterChanged(DependencyObject sender,
                 DependencyPropertyChangedEventArgs e) => (sender as CommandAction)?.SynchronizeElementState();

        private void SynchronizeElementState()
        {
            if (Command != null && AssociatedObject is FrameworkElement associatedObject)

                associatedObject.IsEnabled = Command.CanExecute(CommandParameter);
        }

        protected override void Invoke(object parameter) => Command?.Execute(CommandParameter);


    }
}
