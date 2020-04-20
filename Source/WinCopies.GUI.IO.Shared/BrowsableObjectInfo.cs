using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Media.Imaging;
using WinCopies.IO;

namespace WinCopies.GUI.IO
{
    public class ExplorerControlBrowsableObjectInfoViewModel : INotifyPropertyChanged
    {

        public BrowsableObjectInfoViewModel BrowsableObjectInfoViewModel { get; }

        private ObservableCollection<BrowsableObjectInfoViewModel> _treeViewItems;

        public ObservableCollection<BrowsableObjectInfoViewModel> TreeViewItems { get=>_treeViewItems; set { _treeViewItems = value; OnPropertyChanged(nameof(TreeViewItems)); } }

        private ViewStyle _viewStyle = ViewStyle.SizeThree;

        public ViewStyle ViewStyle { get => _viewStyle; set { _viewStyle = value; OnPropertyChanged(nameof(ViewStyle)); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BrowsableObjectInfoViewModel : IBrowsableObjectInfo, INotifyPropertyChanged
    {

        protected IBrowsableObjectInfo InnerBrowsableObjectInfo { get; }

        public BitmapSource SmallBitmapSource => InnerBrowsableObjectInfo.SmallBitmapSource;

        public BitmapSource MediumBitmapSource => InnerBrowsableObjectInfo.MediumBitmapSource;

        public BitmapSource LargeBitmapSource => InnerBrowsableObjectInfo.LargeBitmapSource;

        public BitmapSource ExtraLargeBitmapSource => InnerBrowsableObjectInfo.ExtraLargeBitmapSource;

        public bool IsBrowsable => InnerBrowsableObjectInfo.IsBrowsable;

        IReadOnlyCollection<IBrowsableObjectInfo> IBrowsableObjectInfo.Items => InnerBrowsableObjectInfo.Items;

        public ReadOnlyObservableCollection<IBrowsableObjectInfo> Items { get; }

        IBrowsableObjectInfo IBrowsableObjectInfo.Parent => InnerBrowsableObjectInfo.Parent;

        public BrowsableObjectInfoViewModel Parent { get; }

        public string Path => InnerBrowsableObjectInfo.Path;

        public string LocalizedName => InnerBrowsableObjectInfo.LocalizedName;

        public string Name => InnerBrowsableObjectInfo.Name;

        private bool _isChecked = false;

        public bool IsChecked { get => _isChecked; set { _isChecked = value; OnPropertyChanged(nameof(IsChecked)); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public BrowsableObjectInfoViewModel(IBrowsableObjectInfo browsableObjectInfo)

        {

            InnerBrowsableObjectInfo = browsableObjectInfo;

            Items = new ReadOnlyObservableCollection<IBrowsableObjectInfo>( new ObservableCollection<IBrowsableObjectInfo>( InnerBrowsableObjectInfo.Items));

            Parent = new BrowsableObjectInfoViewModel(InnerBrowsableObjectInfo.Parent);

        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int CompareTo([AllowNull] IFileSystemObject other) => InnerBrowsableObjectInfo.CompareTo(other);

        public bool Equals([AllowNull] IFileSystemObject other) => InnerBrowsableObjectInfo.Equals(other);

        #region IDisposable Support

        public bool IsDisposed => InnerBrowsableObjectInfo.IsDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                InnerBrowsableObjectInfo.Dispose();
        }

        public void Dispose() => Dispose(true);

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
