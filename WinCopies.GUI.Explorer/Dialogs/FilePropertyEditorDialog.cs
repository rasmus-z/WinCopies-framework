using System;
using System.Windows;
using System.Windows.Controls;
using WinCopies.GUI.Explorer;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class FilePropertyEditorDialog : DialogWindow
    {

        public static readonly DependencyProperty FilePropertiesProperty = DependencyProperty.Register(nameof(FileProperties), typeof(Array), typeof(FilePropertyEditorDialog));

        public Array FileProperties

        {

            get => (Array)GetValue(FilePropertiesProperty);

            set => SetValue(FilePropertiesProperty, value);

        }

        static FilePropertyEditorDialog() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FilePropertyEditorDialog), new FrameworkPropertyMetadata(typeof(FilePropertyEditorDialog)));

        // /// <summary>
        // /// Initializes a new instance of the <see cref="FilePropertyEditorDialog"/> class.
        // /// </summary>
        // public FilePropertyEditorDialog() => Content = new Control { Template = (ControlTemplate)ResourcesHelper.Instance.ResourceDictionary["FilePropertyEditorDialogTemplate"] };
    }
}
