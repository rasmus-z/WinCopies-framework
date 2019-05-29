using System.Windows;
using System.Windows.Input;
using WinCopies.GUI.Controls;
using WinCopies.Util;

namespace WinCopies.GUI.Explorer
{
    public class ListViewItem : System.Windows.Controls.ListViewItem
    {

        public ExplorerControl ParentExplorerControl { get; } = null;

        public static readonly DependencyProperty IsCheckBoxEnabledProperty = DependencyProperty.Register(nameof(IsCheckBoxEnabled), typeof(bool), typeof(ListViewItem), new PropertyMetadata(false

            //    , (DependencyObject d, DependencyPropertyChangedEventArgs e) =>

            //{

            //    object a = e.NewValue;

            //}

            ));

        public bool IsCheckBoxEnabled { get => (bool)GetValue(IsCheckBoxEnabledProperty); set => SetValue(IsCheckBoxEnabledProperty, value); }

        public ListViewItem(ExplorerControl parentExplorerControl)
        {

            ParentExplorerControl = parentExplorerControl;//InputBindings.Add(new InputBinding(Commands.OpenOnLeftClick, new MouseGesture(MouseAction.LeftClick)));

            //CommandBindings.Add(new CommandBinding(parentExplorerControl.Command, Open_Executed, Open_CanExecute));

            //InputBindings.Add(new InputBinding(parentExplorerControl.Command, new KeyGesture(Key.Enter)));

        }

        static ListViewItem() => VisibilityProperty.OverrideMetadata(typeof(ListViewItem), new PropertyMetadata(VisibilityProperty.DefaultMetadata.DefaultValue, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>

                                 {

                                     VisibilityProperty.DefaultMetadata.PropertyChangedCallback?.Invoke(d, e);

                                     ((ListViewItem)d).ParentExplorerControl.Value_IsVisibleChanged(d, e);

                                 }));
    }
}
