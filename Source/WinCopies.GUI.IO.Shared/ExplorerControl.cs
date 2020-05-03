using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.IO;
using WinCopies.Util;
using WinCopies.Util.Data;

namespace WinCopies.GUI.IO
{
    public class ExplorerControl : Control
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(ExplorerControl), new PropertyMetadata((DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ExplorerControl)d).OnPathChanged((string)e.OldValue, (string)e.NewValue)));

        public string Path { get => (string)GetValue(PathProperty); set => SetValue(PathProperty, value); }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(ExplorerControl));

        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        public static readonly DependencyProperty TreeViewStyleProperty = DependencyProperty.Register(nameof(TreeViewStyle), typeof(Style), typeof(ExplorerControl));

        public Style TreeViewStyle { get => (Style)GetValue(TreeViewStyleProperty); set => SetValue(TreeViewStyleProperty, value); }

        public static readonly DependencyProperty ListViewStyleProperty = DependencyProperty.Register(nameof(ListViewStyle), typeof(Style), typeof(ExplorerControl));

        public Style ListViewStyle { get => (Style)GetValue(ListViewStyleProperty); set => SetValue(ListViewStyleProperty, value); }

        public static readonly DependencyProperty IsCheckBoxVisibleProperty = DependencyProperty.Register(nameof(IsCheckBoxVisible), typeof(bool), typeof(ExplorerControl));

        public bool IsCheckBoxVisible { get => (bool)GetValue(IsCheckBoxVisibleProperty); set => SetValue(IsCheckBoxVisibleProperty, value); } 

        public static readonly DependencyProperty GoButtonCommandProperty = DependencyProperty.Register(nameof(GoButtonCommand), typeof(ICommand), typeof(ExplorerControl));

        public ICommand GoButtonCommand { get => (ICommand)GetValue(GoButtonCommandProperty); set => SetValue(GoButtonCommandProperty, value); }

        public static readonly DependencyProperty GoButtonCommandParameterProperty = DependencyProperty.Register(nameof(GoButtonCommandParameter), typeof(object), typeof(ExplorerControl));

        public object GoButtonCommandParameter { get => GetValue(GoButtonCommandParameterProperty); set => SetValue(GoButtonCommandParameterProperty, value); }

        public static readonly RoutedEvent PathChangedEvent = EventManager.RegisterRoutedEvent(nameof(PathChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler<ValueChangedEventArgs>), typeof(ExplorerControl));

        public event RoutedEventHandler<ValueChangedEventArgs> PathChanged
        {
            add => AddHandler(PathChangedEvent, value);

            remove => RemoveHandler(PathChangedEvent, value);
        }

        //public static readonly DependencyProperty TreeViewItemTemplateProperty = DependencyProperty.Register(nameof(TreeViewItemTemplate), typeof(DataTemplate), typeof(ExplorerControl));

        //public DataTemplate TreeViewItemTemplate { get => (DataTemplate)GetValue(TreeViewItemTemplateProperty); set => SetValue(TreeViewItemTemplateProperty, value); }

        //public static readonly DependencyProperty ListViewItemTemplateProperty = DependencyProperty.Register(nameof(ListViewItemTemplate), typeof(DataTemplate), typeof(ExplorerControl));

        //public DataTemplate ListViewItemTemplate { get => (DataTemplate)GetValue(ListViewItemTemplateProperty); set => SetValue(ListViewItemTemplateProperty, value); }

        //public static readonly DependencyProperty TreeViewItemsProperty = DependencyProperty.Register(nameof(TreeViewItems), typeof(IEnumerable), typeof(ExplorerControl));

        //public IEnumerable TreeViewItems { get => (IEnumerable)GetValue(TreeViewItemsProperty); set => SetValue(TreeViewItemsProperty, value); }

        //public static readonly DependencyProperty ListViewItemsProperty = DependencyProperty.Register(nameof(ListViewItems), typeof(IEnumerable), typeof(ExplorerControl));

        //public IEnumerable ListViewItems { get => (IEnumerable)GetValue(ListViewItemsProperty); set => SetValue(ListViewItemsProperty, value); }

        static ExplorerControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerControl), new FrameworkPropertyMetadata(typeof(ExplorerControl)));

        protected virtual void OnPathChanged(string oldValue, string newValue) => RaiseEvent(new RoutedEventArgs<ValueChangedEventArgs>(PathChangedEvent, new ValueChangedEventArgs(oldValue, newValue)));

        //public ExplorerControl() => OnApplyCommandBindings();

        //protected virtual void OnApplyCommandBindings()
        //{
        //    CommandBindings.Add(new System.Windows.Input.CommandBinding(WinCopies.Util.Commands.Commands.CommonCommand, (object sender, ExecutedRoutedEventArgs e) => OnPathChange(e), (object sender, CanExecuteRoutedEventArgs e) => OnPathChange(e)));
        //}

        //protected virtual void OnPathChange(CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        //protected virtual void OnPathChange(ExecutedRoutedEventArgs e) => path.Path = new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName(browsableObjectInfo.Text)));

        //protected virtual void OnPathChanged(IBrowsableObjectInfoViewModel oldPath, IBrowsableObjectInfoViewModel newPath ) =>            browsableObjectInfo.Text = Path; 
    }
}
