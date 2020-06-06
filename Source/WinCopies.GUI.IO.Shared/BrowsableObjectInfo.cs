/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WinCopies.IO;
using WinCopies.Linq;
using WinCopies.Util.Commands;
using WinCopies.Util.Data;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.IO
{
    public interface IExplorerControlBrowsableObjectInfoViewModel : IBrowsableObjectInfoViewModelCommon
    {

        ObservableCollection<IBrowsableObjectInfoViewModel> TreeViewItems { get; set; }

        string Text { get; set; }

        IBrowsableObjectInfoViewModel Path { get; set; }

        IBrowsableObjectInfoFactory Factory { get; set; }

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

        private IBrowsableObjectInfoFactory _factory;

        public IBrowsableObjectInfoFactory Factory { get => _factory; set { _factory = value ?? throw GetArgumentNullException(nameof(value)); OnPropertyChanged(nameof(BrowsableObjectInfoFactory)); } }

        protected virtual void OnPathChanged() => Text = _path.Path;

        private bool _isSelected;

        public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } }

        public ExplorerControlBrowsableObjectInfoViewModel(IBrowsableObjectInfoViewModel path) : this(path, new BrowsableObjectInfoFactory()) { } 

        public ExplorerControlBrowsableObjectInfoViewModel(IBrowsableObjectInfoViewModel path, IBrowsableObjectInfoFactory factory)
        {
            _path = path ?? throw GetArgumentNullException(nameof(path));

            ItemClickCommand = new DelegateCommand<IBrowsableObjectInfoViewModel>(browsableObjectInfo => true, browsableObjectInfo =>
            {
                if (browsableObjectInfo.InnerBrowsableObjectInfo is IShellObjectInfo shellObjectInfo && shellObjectInfo.FileType == FileType.File)

                    _ = Process.Start(new ProcessStartInfo(browsableObjectInfo.Path) { UseShellExecute = true });

                else

                    Path = new BrowsableObjectInfoViewModel(browsableObjectInfo);
            });

            _factory = factory ?? throw GetArgumentNullException(nameof(factory));
        }

        public static DelegateCommand<ExplorerControlBrowsableObjectInfoViewModel> GoCommand { get; } = new DelegateCommand<ExplorerControlBrowsableObjectInfoViewModel>(browsableObjectInfo => browsableObjectInfo != null && browsableObjectInfo.OnGoCommandCanExecute(), browsableObjectInfo => browsableObjectInfo.OnGoCommandExecuted());

        protected virtual bool OnGoCommandCanExecute() => true;

        protected virtual void OnGoCommandExecuted() => Path = _factory.GetBrowsableObjectInfoViewModel(_factory.GetBrowsableObjectInfo(Text));

        public DelegateCommand<IBrowsableObjectInfoViewModel> ItemClickCommand { get; }

        //private ViewStyle _viewStyle = ViewStyle.SizeThree;

        //public ViewStyle ViewStyle { get => _viewStyle; set { _viewStyle = value; OnPropertyChanged(nameof(ViewStyle)); } }
    }

    public interface IBrowsableObjectInfoViewModelCommon
    {
        bool IsSelected { get; set; }
    }

    public interface IBrowsableObjectInfoViewModel : IBrowsableObjectInfo, IBrowsableObjectInfoViewModelCommon
    {
        ObservableCollection<IBrowsableObjectInfoViewModel> Items { get; }

        IBrowsableObjectInfo InnerBrowsableObjectInfo { get; }
    }

    public interface IBrowsableObjectInfoFactory
    {
        IBrowsableObjectInfoViewModel GetBrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path);
    }

    public class BrowsableObjectInfoFactory : IBrowsableObjectInfoFactory
    {
        public virtual IBrowsableObjectInfoViewModel GetBrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) => new BrowsableObjectInfoViewModel(browsableObjectInfo);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path)
        {
            if (WinCopies.IO.Path.IsFileSystemPath(path))

                return ShellObjectInfo.From(ShellObject.FromParsingName(path));

            else if (WinCopies.IO.Path.IsRegistryPath(path))

                return new RegistryItemInfo(path);

            throw new ArgumentException("The factory cannot create an object for the given path.");
        }
    }

    //public class TreeViewItemBrowsableObjectInfoViewModelFactory : IBrowsableObjectInfoViewModelFactory
    //{
    //    public IBrowsableObjectInfoViewModel GetBrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) => new BrowsableObjectInfoViewModel(browsableObjectInfo, Predicate) { Factory = this };
    //}

    public class BrowsableObjectInfoViewModel : ViewModel<IBrowsableObjectInfo>, IBrowsableObjectInfoViewModel
    {

        public static Predicate<IBrowsableObjectInfo> Predicate => _browsableObjectInfo => _browsableObjectInfo.IsBrowsable;

        private Predicate<IBrowsableObjectInfo> _filter;

        public Predicate<IBrowsableObjectInfo> Filter { get => _filter; set { _filter = value; OnPropertyChanged(nameof(Filter)); } }

        private IBrowsableObjectInfoFactory _factory;

        public IBrowsableObjectInfoFactory Factory { get => _factory; set { _factory = value; OnPropertyChanged(nameof(_factory)); } }

        public IBrowsableObjectInfo InnerBrowsableObjectInfo => ModelGeneric;

        public bool IsSpecialItem => InnerBrowsableObjectInfo.IsSpecialItem;

        public BitmapSource SmallBitmapSource => InnerBrowsableObjectInfo.SmallBitmapSource;

        public BitmapSource MediumBitmapSource => InnerBrowsableObjectInfo.MediumBitmapSource;

        public BitmapSource LargeBitmapSource => InnerBrowsableObjectInfo.LargeBitmapSource;

        public BitmapSource ExtraLargeBitmapSource => InnerBrowsableObjectInfo.ExtraLargeBitmapSource;

        public bool IsBrowsable => InnerBrowsableObjectInfo.IsBrowsable;

        public string ItemTypeName => InnerBrowsableObjectInfo.ItemTypeName;

        public string Description => InnerBrowsableObjectInfo.Description;

        public Size? Size => InnerBrowsableObjectInfo.Size;

        public bool HasTransparency => InnerBrowsableObjectInfo.IsSpecialItem;

        private ObservableCollection<IBrowsableObjectInfoViewModel> _items;

        private bool _itemsLoaded = false;

        public IEnumerable<IBrowsableObjectInfo> GetItems() => Items;

        public ObservableCollection<IBrowsableObjectInfoViewModel> Items
        {
            get
            {
                if (_itemsLoaded)

                    return _items;

                if (InnerBrowsableObjectInfo.IsBrowsable)

                    try
                    {
                        IEnumerable<IBrowsableObjectInfo> items = _filter == null ? InnerBrowsableObjectInfo.GetItems() : InnerBrowsableObjectInfo.GetItems().WherePredicate(_filter);

                        _items = new ObservableCollection<IBrowsableObjectInfoViewModel>(items
                    .Select(_browsableObjectInfo => _factory == null ? new BrowsableObjectInfoViewModel(_browsableObjectInfo, _filter) : _factory.GetBrowsableObjectInfoViewModel(_browsableObjectInfo)));

                    }
                    catch { }

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

        public BrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo) : base(browsableObjectInfo ?? throw GetArgumentNullException(nameof(browsableObjectInfo))) { }

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
