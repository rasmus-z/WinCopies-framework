using System.Windows;
using System.Windows.Input;

namespace WinCopies.Util.Commands
{

    /// <summary>
    /// Provides some standard commands for file system gesture.
    /// </summary>
    public static class FileSystemCommands
    {

        /// <summary>
        /// Gets the <b>NewFolder</b> command.
        /// </summary>
        public static RoutedUICommand NewFolder { get; } = new RoutedUICommand(Generic.NewFolder, nameof(NewFolder), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>NewArchive</b> command.
        /// </summary>
        public static RoutedUICommand NewArchive { get; } = new RoutedUICommand(Generic.NewArchive, nameof(NewArchive), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>FileProperties</b> command.
        /// </summary>
        public static RoutedUICommand FileProperties { get; } = new RoutedUICommand(Generic.ShowFileProperties, nameof(FileProperties), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.Enter, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>Rename</b> command.
        /// </summary>
        public static RoutedUICommand Rename { get; } = new RoutedUICommand(Generic.Rename, nameof(Rename), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>DeletePermanently</b> command.
        /// </summary>
        public static RoutedUICommand DeletePermanently { get; } = new RoutedUICommand(Generic.DeletePermanently, nameof(DeletePermanently), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.Delete, ModifierKeys.Shift) });

    }
}
