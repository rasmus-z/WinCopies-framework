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
        /// Gets a value that represents the new folder command.
        /// </summary>
        public static RoutedUICommand NewFolder { get; } = new RoutedUICommand(Generic.NewFolder, nameof(NewFolder), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        /// <summary>
        /// Gets a value that represents the new archive command.
        /// </summary>
        public static RoutedUICommand NewArchive { get; } = new RoutedUICommand(Generic.NewArchive, nameof(NewArchive), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets a value that represents the show file properties command.
        /// </summary>
        public static RoutedUICommand FileProperties { get; } = new RoutedUICommand(Generic.ShowFileProperties, nameof(FileProperties), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.Enter, ModifierKeys.Alt) });

        public static RoutedUICommand Rename { get; } = new RoutedUICommand(Generic.Rename, nameof(Rename), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });    

    }
}
