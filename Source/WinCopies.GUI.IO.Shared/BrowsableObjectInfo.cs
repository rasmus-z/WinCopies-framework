using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Media.Imaging;
using WinCopies.IO;
using WinCopies.Linq;

namespace WinCopies.GUI.IO
{
    public interface IExplorerControlBrowsableObjectInfoViewModel : INotifyPropertyChanged
    {
        BrowsableObjectInfoViewModel BrowsableObjectInfoViewModel { get; }

        ObservableCollection<IBrowsableObjectInfoViewModel> TreeViewItems { get; set; }

        IBrowsableObjectInfoViewModel Path { get; set; }
    }

    public class ExplorerControlBrowsableObjectInfoViewModel : IExplorerControlBrowsableObjectInfoViewModel
    {

        public BrowsableObjectInfoViewModel BrowsableObjectInfoViewModel { get; }

        private ObservableCollection<IBrowsableObjectInfoViewModel> _treeViewItems;

        public ObservableCollection<IBrowsableObjectInfoViewModel> TreeViewItems { get => _treeViewItems; set { _treeViewItems = value; OnPropertyChanged(nameof(TreeViewItems)); } }

        private IBrowsableObjectInfoViewModel _path;

        public IBrowsableObjectInfoViewModel Path { get => _path; set { _path = value; OnPropertyChanged(nameof(Path)); } }

        public ExplorerControlBrowsableObjectInfoViewModel(IBrowsableObjectInfoViewModel path) => Path = path;

        //private ViewStyle _viewStyle = ViewStyle.SizeThree;

        //public ViewStyle ViewStyle { get => _viewStyle; set { _viewStyle = value; OnPropertyChanged(nameof(ViewStyle)); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public interface IBrowsableObjectInfoViewModel : IBrowsableObjectInfo, INotifyPropertyChanged
    {
        bool IsSelected { get; set; }

        ObservableCollection<IBrowsableObjectInfoViewModel> Items { get; }
    }

    public class BrowsableObjectInfoViewModel : IBrowsableObjectInfoViewModel
    {

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
                    InnerBrowsableObjectInfo.GetItems(_browsableObjectInfo => _browsableObjectInfo.IsBrowsable).Select(_browsableObjectInfo => new BrowsableObjectInfoViewModel(_browsableObjectInfo)));

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

        public event PropertyChangedEventHandler PropertyChanged;

        public BrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) => InnerBrowsableObjectInfo = browsableObjectInfo ?? throw Util.Util.GetArgumentNullException(nameof(browsableObjectInfo));

        protected virtual void OnPropertyChanged(in string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
