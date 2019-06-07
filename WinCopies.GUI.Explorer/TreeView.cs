using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinCopies.Util;
using WinCopies.Util.Commands;
using ApplicationCommands = System.Windows.Input.ApplicationCommands;

namespace WinCopies.GUI.Explorer
{
    public class TreeView : WinCopies.GUI.Controls.TreeView
    {

        internal ExplorerControl ParentExplorerControl { get; set; }

        private static readonly DependencyPropertyKey CanCopyPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanCopy), typeof(bool), typeof(TreeView), new PropertyMetadata(false));

        public static readonly DependencyProperty CanCopyProperty = CanCopyPropertyKey.DependencyProperty;

        public bool CanCopy => (bool)GetValue(CanCopyProperty);

        private static readonly DependencyPropertyKey CanCutPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanCut), typeof(bool), typeof(TreeView), new PropertyMetadata(false));

        public static readonly DependencyProperty CanCutProperty = CanCutPropertyKey.DependencyProperty;

        public bool CanCut => (bool)GetValue(CanCutProperty);

        private static readonly DependencyPropertyKey CanRenamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanRename), typeof(bool), typeof(TreeView), new PropertyMetadata(false));

        public static readonly DependencyProperty CanRenameProperty = CanRenamePropertyKey.DependencyProperty;

        public bool CanRename => (bool)GetValue(CanRenameProperty);

        private static readonly DependencyPropertyKey CanDeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CanDelete), typeof(bool), typeof(TreeView), new PropertyMetadata(false));

        public static readonly DependencyProperty CanDeleteProperty = CanDeletePropertyKey.DependencyProperty;

        public bool CanDelete => (bool)GetValue(CanDeleteProperty);

        // todo: to add a default implementation:

        public static readonly DependencyProperty RenameActionProperty = DependencyProperty.Register(nameof(RenameAction), typeof(RenameHandler), typeof(TreeView));

        public RenameHandler RenameAction { get => (RenameHandler)GetValue(RenameActionProperty); set => SetValue(RenameActionProperty, value); }

        // todo: to add a default implementation:

        public static readonly DependencyProperty DeleteActionProperty = DependencyProperty.Register(nameof(DeleteAction), typeof(DeleteHandler), typeof(TreeView));

        public DeleteHandler DeleteAction { get => (DeleteHandler)GetValue(DeleteActionProperty); set => SetValue(DeleteActionProperty, value); }

        public TreeView()
        {

            SelectedItemChanged += TreeView_SelectedItemChanged;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_CanExecute));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_CanExecute));

            CommandBindings.Add(new CommandBinding(FileSystemCommands.Rename, Rename_Executed, Rename_CanExecute));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_CanExecute));

        }

        protected override DependencyObject GetContainerForItemOverride() => new TreeViewItem(ParentExplorerControl);

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            if (SelectedItem is IBrowsableObjectInfo browsableObjectInfo)

            {

                SetValue(CanRenamePropertyKey, true);

                SetValue(CanDeletePropertyKey, true);

                if (browsableObjectInfo is ShellObjectInfo shellObjectInfo && shellObjectInfo.ShellObject.IsFileSystemObject)

                {

                    SetValue(CanCopyPropertyKey, true);

                    SetValue(CanCutPropertyKey, true);

                }

            }

        }

        protected virtual void OnCopyCanExecute(CanExecuteRoutedEventArgs e) => e.CanExecute = CanCopy;//e.ContinueRouting = false;//e.Handled = true;//raise

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnCopyCanExecute(e);

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e) => (sender as TreeView)?.GetParent<ExplorerControl>(false)?.Copy(ActionsFromObjects.TreeView);

        protected virtual void OnCutCanExecute(CanExecuteRoutedEventArgs e) => e.CanExecute = CanCut;

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnCutCanExecute(e);

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e) => (sender as ExplorerControl)?.GetParent<ExplorerControl>(false)?.Cut(ActionsFromObjects.TreeView);

        protected virtual void OnRenameCanExecute(CanExecuteRoutedEventArgs e) => e.CanExecute = CanRename;

        private void Rename_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnRenameCanExecute(e);

        private void Rename_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            ExplorerControl explorerControl = (sender as ExplorerControl)?.GetParent<ExplorerControl>(false);

            explorerControl?.RenameAction?.Invoke(explorerControl.TreeView.SelectedItem as IBrowsableObjectInfo, e.Parameter as string);

        }

        protected virtual void OnDeleteCanExecute(CanExecuteRoutedEventArgs e) => e.CanExecute = CanDelete;

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnDeleteCanExecute(e);

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            ExplorerControl explorerControl = (sender as ExplorerControl)?.GetParent<ExplorerControl>(false);

            explorerControl?.DeleteAction?.Invoke(new IBrowsableObjectInfo[] { explorerControl.TreeView.SelectedItem as IBrowsableObjectInfo });

        }

    }
}
