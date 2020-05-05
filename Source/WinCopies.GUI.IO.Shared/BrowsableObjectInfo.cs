using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WinCopies.IO;
using WinCopies.Linq;
using WinCopies.Util.Commands;
using WinCopies.Util.Data;

namespace WinCopies.GUI.IO
{
    public interface IExplorerControlBrowsableObjectInfoViewModel : IBrowsableObjectInfoViewModelCommon, INotifyPropertyChanged
    {

        ObservableCollection<IBrowsableObjectInfoViewModel> TreeViewItems { get; set; }

        string Text { get; set; }

        IBrowsableObjectInfoViewModel Path { get; set; }

        SelectionMode SelectionMode { get; set; }

        bool IsCheckBoxVisible { get; set; }
    }

    public class ExplorerControlBrowsableObjectInfoViewModel : ViewModelBase, IExplorerControlBrowsableObjectInfoViewModel
    {

        //protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue) => OnPropertyChanged(new WinCopies.Util.Data.PropertyChangedEventArgs(propertyName, oldValue, newValue));

        private SelectionMode _selectionMode = SelectionMode.Extended;

        private bool _isCheckBoxVisible;

        public bool IsCheckBoxVisible { get => _isCheckBoxVisible; set { if (value && _selectionMode == SelectionMode.Single) throw new ArgumentException("Cannot apply the true value for the IsCheckBoxVisible when SelectionMode is set to Single.", nameof(value)); _isCheckBoxVisible = value; OnPropertyChanged(nameof(IsCheckBoxVisible)); } }

        public SelectionMode SelectionMode { get => _selectionMode; set { _selectionMode = value; OnPropertyChanged(nameof(SelectionMode)); } }

        private string _text;

        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }

        private ObservableCollection<IBrowsableObjectInfoViewModel> _treeViewItems;

        public ObservableCollection<IBrowsableObjectInfoViewModel> TreeViewItems { get => _treeViewItems; set { _treeViewItems = value; OnPropertyChanged(nameof(TreeViewItems)); } }

        private IBrowsableObjectInfoViewModel _path;

        public IBrowsableObjectInfoViewModel Path { get => _path; set { _path = value; OnPropertyChanged(nameof(Path)); OnPathChanged(); } }

        protected virtual void OnPathChanged() => Text = _path.Path;

        private bool _isSelected;

        public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } }

        public ExplorerControlBrowsableObjectInfoViewModel(IBrowsableObjectInfoViewModel path)
        {
            Path = path;

            ItemClickCommand = new DelegateCommand<IBrowsableObjectInfoViewModel>(browsableObjectInfo => true, browsableObjectInfo => Path = new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName(browsableObjectInfo.Path))));
        }

        public static DelegateCommand<ExplorerControlBrowsableObjectInfoViewModel> GoCommand { get; } = new DelegateCommand<ExplorerControlBrowsableObjectInfoViewModel>(browsableObjectInfo => browsableObjectInfo != null &&     browsableObjectInfo.OnGoCommandCanExecute(), browsableObjectInfo =>browsableObjectInfo.OnGoCommandExecuted());

        protected virtual bool OnGoCommandCanExecute() => true;

        protected virtual void OnGoCommandExecuted() => Path = new BrowsableObjectInfoViewModel(ShellObjectInfo.From(ShellObject.FromParsingName(Text)));

        public DelegateCommand<IBrowsableObjectInfoViewModel> ItemClickCommand { get; } 

        //private ViewStyle _viewStyle = ViewStyle.SizeThree;

        //public ViewStyle ViewStyle { get => _viewStyle; set { _viewStyle = value; OnPropertyChanged(nameof(ViewStyle)); } }
    }

    public interface IBrowsableObjectInfoViewModelCommon
    {
        bool IsSelected { get; set; }
    }

    public interface IBrowsableObjectInfoViewModel : IBrowsableObjectInfo, IBrowsableObjectInfoViewModelCommon, INotifyPropertyChanged
    {
        ObservableCollection<IBrowsableObjectInfoViewModel> Items { get; }
    }

    public interface IBrowsableObjectInfoViewModelFactory
    {
        IBrowsableObjectInfoViewModel GetBrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo);
    }

    public class TreeViewItemBrowsableObjectInfoViewModelFactory : IBrowsableObjectInfoViewModelFactory
    {
        public static Predicate<IBrowsableObjectInfo> Predicate => _browsableObjectInfo => _browsableObjectInfo.IsBrowsable;

        public IBrowsableObjectInfoViewModel GetBrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) => new BrowsableObjectInfoViewModel(browsableObjectInfo, Predicate) { Factory = this }; 
    }

    public class BrowsableObjectInfoViewModel : ViewModelBase, IBrowsableObjectInfoViewModel
    {

        private Predicate<IBrowsableObjectInfo> _filter;

        public Predicate<IBrowsableObjectInfo> Filter { get => _filter; set { _filter = value; OnPropertyChanged(nameof(Filter)); } }

        private IBrowsableObjectInfoViewModelFactory _factory;

        public IBrowsableObjectInfoViewModelFactory Factory { get => _factory; set { _factory = value; OnPropertyChanged(nameof(_factory)); } } 

        protected IBrowsableObjectInfo InnerBrowsableObjectInfo { get; }

        public bool IsSpecialItem => InnerBrowsableObjectInfo.IsSpecialItem;

        public BitmapSource SmallBitmapSource => InnerBrowsableObjectInfo.SmallBitmapSource;

        public BitmapSource MediumBitmapSource => InnerBrowsableObjectInfo.MediumBitmapSource;

        public BitmapSource LargeBitmapSource => InnerBrowsableObjectInfo.LargeBitmapSource;

        public BitmapSource ExtraLargeBitmapSource => InnerBrowsableObjectInfo.ExtraLargeBitmapSource;

        public bool IsBrowsable => InnerBrowsableObjectInfo.IsBrowsable;

        public string ItemTypeName => InnerBrowsableObjectInfo.ItemTypeName;

        public Size? Size => InnerBrowsableObjectInfo.Size;

        public bool HasTransparency => InnerBrowsableObjectInfo.IsSpecialItem;

        private ObservableCollection<IBrowsableObjectInfoViewModel> _items;

        private bool _itemsLoaded = false;

        public IEnumerable<IBrowsableObjectInfo> GetItems() => Items;

        public IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<IBrowsableObjectInfo> func) => Items.Where(func);

        public ObservableCollection<IBrowsableObjectInfoViewModel> Items
        {
            get
            {
                if (_itemsLoaded)

                    return _items;

                if (InnerBrowsableObjectInfo.IsBrowsable)

                    try
                    {

                        _items = new ObservableCollection<IBrowsableObjectInfoViewModel>(
                    (_filter == null ? InnerBrowsableObjectInfo.GetItems() : InnerBrowsableObjectInfo.GetItems(_filter)).Select(_browsableObjectInfo => _factory == null ? new BrowsableObjectInfoViewModel(_browsableObjectInfo) : _factory.GetBrowsableObjectInfoViewModel(_browsableObjectInfo)));

                    }
                    catch (ShellException) { }

                _itemsLoaded = true;

                return _items;
            }
        }

        private IBrowsableObjectInfoViewModel _parent;

        private bool _parentLoaded = false;

        IBrowsableObjectInfo IBrowsableObjectInfo.Parent => InnerBrowsableObjectInfo.Parent;

        public IBrowsableObjectInfoViewModel Parent
        {
            get
            {
                if (_parentLoaded)

                    return _parent;

                if (InnerBrowsableObjectInfo.Parent is object)

                    _parent = new BrowsableObjectInfoViewModel(InnerBrowsableObjectInfo.Parent);

                _parentLoaded = true;

                return _parent;
            }
        }

        public string Path => InnerBrowsableObjectInfo.Path;

        public string LocalizedName => InnerBrowsableObjectInfo.LocalizedName;

        public string Name => InnerBrowsableObjectInfo.Name;

        private bool _isSelected = false;

        public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } }

        public BrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) => InnerBrowsableObjectInfo = browsableObjectInfo ?? throw Util.Util.GetArgumentNullException(nameof(browsableObjectInfo));

        public BrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo, Predicate<IBrowsableObjectInfo> filter) : this(browsableObjectInfo) => _filter = filter;

        public int CompareTo(
#if !NETFRAMEWORK
            [AllowNull]
        #endif
        IFileSystemObject other) => InnerBrowsableObjectInfo.CompareTo(other);

        public bool Equals(
#if !NETFRAMEWORK
            [AllowNull]
        #endif
        IFileSystemObject other) => InnerBrowsableObjectInfo.Equals(other);

        #region IDisposable Support

        public bool IsDisposed => InnerBrowsableObjectInfo.IsDisposed;

        protected virtual void Dispose(in bool disposing)
        {
            if (disposing)

                InnerBrowsableObjectInfo.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public override bool Equals(object obj) => ReferenceEquals(this, obj) ? true : obj is null ? false : InnerBrowsableObjectInfo.Equals(obj);

        public override int GetHashCode() => InnerBrowsableObjectInfo.GetHashCode();

        public static bool operator ==(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => !(left == right);

        public static bool operator <(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => left is null ? right is object : left.CompareTo(right) < 0;

        public static bool operator <=(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => left is object && left.CompareTo(right) > 0;

        public static bool operator >=(BrowsableObjectInfoViewModel left, BrowsableObjectInfoViewModel right) => left is null ? right is null : left.CompareTo(right) >= 0;
        #endregion
    }
}
