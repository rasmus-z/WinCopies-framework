using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WinCopies.GUI.Controls
{

    public static class GridViewColumns
    {

        public static readonly DependencyProperty ColumnCollectionBehaviourProperty =
            DependencyProperty.RegisterAttached("ColumnCollectionBehaviour", typeof(GridViewColumnCollectionBehaviour), typeof(GridViewColumns), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.RegisterAttached("ColumnsSource", typeof(object), typeof(GridViewColumns), new UIPropertyMetadata(null, ColumnsSourceChanged));

        public static readonly DependencyProperty DisplayMemberFormatMemberProperty =
            DependencyProperty.RegisterAttached("DisplayMemberFormatMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null, DisplayMemberFormatMemberChanged));

        public static readonly DependencyProperty DisplayMemberMemberProperty =
            DependencyProperty.RegisterAttached("DisplayMemberMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null, DisplayMemberMemberChanged));

        public static readonly DependencyProperty HeaderTextMemberProperty =
            DependencyProperty.RegisterAttached("HeaderTextMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null, HeaderTextMemberChanged));

        public static readonly DependencyProperty WidthMemberProperty =
            DependencyProperty.RegisterAttached("WidthMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null, WidthMemberChanged));

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static GridViewColumnCollectionBehaviour GetColumnCollectionBehaviour(DependencyObject obj) => (GridViewColumnCollectionBehaviour)obj.GetValue(ColumnCollectionBehaviourProperty);

        public static void SetColumnCollectionBehaviour(DependencyObject obj, GridViewColumnCollectionBehaviour value) => obj.SetValue(ColumnCollectionBehaviourProperty, value);

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static object GetColumnsSource(DependencyObject obj) => obj.GetValue(ColumnsSourceProperty);

        public static void SetColumnsSource(DependencyObject obj, object value) => obj.SetValue(ColumnsSourceProperty, value);

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetDisplayMemberFormatMember(DependencyObject obj) => (string)obj.GetValue(DisplayMemberFormatMemberProperty);

        public static void SetDisplayMemberFormatMember(DependencyObject obj, string value) => obj.SetValue(DisplayMemberFormatMemberProperty, value);

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetDisplayMemberMember(DependencyObject obj) => (string)obj.GetValue(DisplayMemberMemberProperty);

        public static void SetDisplayMemberMember(DependencyObject obj, string value) => obj.SetValue(DisplayMemberMemberProperty, value);

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetHeaderTextMember(DependencyObject obj) => (string)obj.GetValue(HeaderTextMemberProperty);

        public static void SetHeaderTextMember(DependencyObject obj, string value) => obj.SetValue(HeaderTextMemberProperty, value);

        [AttachedPropertyBrowsableForType(typeof(GridView))]
        public static string GetWidthMember(DependencyObject obj) => (string)obj.GetValue(WidthMemberProperty);

        public static void SetWidthMember(DependencyObject obj, string value) => obj.SetValue(WidthMemberProperty, value);

        private static void ColumnsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => GetOrCreateColumnCollectionBehaviour(sender).ColumnsSource = e.NewValue;

        private static void DisplayMemberFormatMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => GetOrCreateColumnCollectionBehaviour(sender).DisplayMemberFormatMember = e.NewValue as string;

        private static void DisplayMemberMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => GetOrCreateColumnCollectionBehaviour(sender).DisplayMemberMember = e.NewValue as string;

        private static void HeaderTextMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => GetOrCreateColumnCollectionBehaviour(sender).HeaderTextMember = e.NewValue as string;

        private static void WidthMemberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => GetOrCreateColumnCollectionBehaviour(sender).WidthMember = e.NewValue as string;

        private static GridViewColumnCollectionBehaviour GetOrCreateColumnCollectionBehaviour(DependencyObject source)
        {

            GridViewColumnCollectionBehaviour behaviour = GetColumnCollectionBehaviour(source);

            if (behaviour == null)
            {

                GridView typedSource = source as GridView;

                if (typedSource == null)

                    // todo:

                    throw new Exception("This property can only be set on controls deriving GridView");

                behaviour = new GridViewColumnCollectionBehaviour(typedSource);

                SetColumnCollectionBehaviour(typedSource, behaviour);

            }

            return behaviour;

        }

    }

    public class GridViewColumnCollectionBehaviour
    {

        private object columnsSource;

        private GridView gridView;

        public GridViewColumnCollectionBehaviour(GridView gridView) => this.gridView = gridView;

        public object ColumnsSource
        {
            get => columnsSource;

            set
            {
                // todo:

                object oldValue = columnsSource;
                columnsSource = value;
                ColumnsSourceChanged(oldValue, columnsSource);
            }
        }

        public string DisplayMemberFormatMember { get; set; }

        public string DisplayMemberMember { get; set; }

        public string HeaderTextMember { get; set; }

        public string WidthMember { get; set; }

        private void AddHandlers(ICollectionView collectionView) => collectionView.CollectionChanged += ColumnsSource_CollectionChanged;

        private void ColumnsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // ICollectionView view = sender as ICollectionView;

            if (gridView == null)

                return;

            switch (e.Action)
            {

                case NotifyCollectionChangedAction.Add:

                    for (int i = 0; i < e.NewItems.Count; i++)

                        gridView.Columns.Insert(e.NewStartingIndex + i, CreateColumn(e.NewItems[i]));

                    break;

                case NotifyCollectionChangedAction.Move:

                    List<GridViewColumn> columns = new List<GridViewColumn>();

                    for (int i = 0; i < e.OldItems.Count; i++)

                        columns.Add(gridView.Columns[e.OldStartingIndex + i]);

                    for (int i = 0; i < e.NewItems.Count; i++)

                        gridView.Columns.Insert(e.NewStartingIndex + i, columns[i]);

                    break;

                case NotifyCollectionChangedAction.Remove:

                    for (int i = 0; i < e.OldItems.Count; i++)

                        gridView.Columns.RemoveAt(e.OldStartingIndex);

                    break;

                case NotifyCollectionChangedAction.Replace:

                    for (int i = 0; i < e.NewItems.Count; i++)

                        gridView.Columns[e.NewStartingIndex + i] = CreateColumn(e.NewItems[i]);

                    break;

                case NotifyCollectionChangedAction.Reset:

                    gridView.Columns.Clear();

                    CreateColumns(sender as ICollectionView);

                    break;

                default:

                    break;

            }

        }

        private void ColumnsSourceChanged(object oldValue, object newValue)
        {
            if (gridView != null)
            {

                gridView.Columns.Clear();

                if (oldValue != null)
                {

                    ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);

                    if (view != null)

                        RemoveHandlers(view);

                }

                if (newValue != null)
                {

                    ICollectionView view = CollectionViewSource.GetDefaultView(newValue);

                    if (view != null)
                    {

                        AddHandlers(view);

                        CreateColumns(view);

                    }

                }

            }

        }

        private GridViewColumn CreateColumn(object columnSource)
        {

            GridViewColumn column = new GridViewColumn();

            if (!string.IsNullOrEmpty(HeaderTextMember))

                column.Header = GetPropertyValue(columnSource, HeaderTextMember);

            if (!string.IsNullOrEmpty(DisplayMemberMember))
            {

                string propertyName = GetPropertyValue(columnSource, DisplayMemberMember) as string;

                string format = null;

                if (!string.IsNullOrEmpty(DisplayMemberFormatMember))

                    format = GetPropertyValue(columnSource, DisplayMemberFormatMember) as string;

                if (string.IsNullOrEmpty(format))

                    format = "{0}";

                column.DisplayMemberBinding = new Binding(propertyName) { StringFormat = format };

            }

            if (!string.IsNullOrEmpty(WidthMember))

                column.Width = (double)GetPropertyValue(columnSource, WidthMember);

            return column;
        }

        private void CreateColumns(ICollectionView collectionView)
        {

            foreach (object item in collectionView)

                gridView.Columns.Add(CreateColumn(item));

        }

        private object GetPropertyValue(object obj, string propertyName) => obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null);

        private void RemoveHandlers(ICollectionView collectionView) => collectionView.CollectionChanged -= ColumnsSource_CollectionChanged;

    }

    //public static class GridViewColumns
    //{

    //    [AttachedPropertyBrowsableForType(typeof(GridView))]
    //    public static object GetColumnsSource(DependencyObject obj) => obj.GetValue(ColumnsSourceProperty);

    //    public static void SetColumnsSource(DependencyObject obj, object value) => obj.SetValue(ColumnsSourceProperty, value);

    //    // Using a DependencyProperty as the backing store for ColumnsSource.  This enables animation, styling, binding, etc...
    //    public static readonly DependencyProperty ColumnsSourceProperty =
    //            DependencyProperty.RegisterAttached(
    //                "ColumnsSource",
    //                typeof(object),
    //                typeof(GridViewColumns),
    //                new UIPropertyMetadata(
    //                    null,
    //                    ColumnsSourceChanged));


    //    [AttachedPropertyBrowsableForType(typeof(GridView))]
    //    public static string GetHeaderTextMember(DependencyObject obj) => (string)obj.GetValue(HeaderTextMemberProperty);

    //    public static void SetHeaderTextMember(DependencyObject obj, string value) => obj.SetValue(HeaderTextMemberProperty, value);

    //    // Using a DependencyProperty as the backing store for HeaderTextMember.  This enables animation, styling, binding, etc...
    //    public static readonly DependencyProperty HeaderTextMemberProperty =
    //        DependencyProperty.RegisterAttached("HeaderTextMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null));


    //    [AttachedPropertyBrowsableForType(typeof(GridView))]
    //    public static string GetDisplayMemberMember(DependencyObject obj) => (string)obj.GetValue(DisplayMemberMemberProperty);

    //    public static void SetDisplayMemberMember(DependencyObject obj, string value) => obj.SetValue(DisplayMemberMemberProperty, value);

    //    // Using a DependencyProperty as the backing store for DisplayMember.  This enables animation, styling, binding, etc...
    //    public static readonly DependencyProperty DisplayMemberMemberProperty =
    //        DependencyProperty.RegisterAttached("DisplayMemberMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null));

    //    private static void ColumnsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    //    {

    //        GridView gridView = obj as GridView;

    //        if (gridView != null)
    //        {

    //            gridView.Columns.Clear();

    //            if (e.OldValue != null)
    //            {

    //                ICollectionView view = CollectionViewSource.GetDefaultView(e.OldValue);

    //                if (view != null)

    //                    RemoveHandlers(gridView, view);

    //            }

    //            if (e.NewValue != null)
    //            {

    //                ICollectionView view = CollectionViewSource.GetDefaultView(e.NewValue);

    //                if (view != null)
    //                {

    //                    AddHandlers(gridView, view);

    //                    CreateColumns(gridView, view);

    //                }

    //            }

    //        }

    //    }

    //    private static IDictionary<ICollectionView, List<GridView>> _gridViewsByColumnsSource =
    //        new Dictionary<ICollectionView, List<GridView>>();

    //    private static List<GridView> GetGridViewsForColumnSource(ICollectionView columnSource)
    //    {

    //        List<GridView> gridViews;

    //        if (!_gridViewsByColumnsSource.TryGetValue(columnSource, out gridViews))
    //        {

    //            gridViews = new List<GridView>();

    //            _gridViewsByColumnsSource.Add(columnSource, gridViews);

    //        }

    //        return gridViews;
    //    }

    //    private static void AddHandlers(GridView gridView, ICollectionView view)
    //    {

    //        GetGridViewsForColumnSource(view).Add(gridView);

    //        view.CollectionChanged += ColumnsSource_CollectionChanged;

    //    }

    //    private static void CreateColumns(GridView gridView, ICollectionView view)
    //    {

    //        foreach (object item in view)

    //            gridView.Columns.Add(CreateColumn(gridView, item));

    //    }

    //    private static void RemoveHandlers(GridView gridView, ICollectionView view)
    //    {

    //        view.CollectionChanged -= ColumnsSource_CollectionChanged;

    //        GetGridViewsForColumnSource(view).Remove(gridView);

    //    }

    //    private static void ColumnsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {

    //        ICollectionView view = sender as ICollectionView;

    //        List<GridView> gridViews = GetGridViewsForColumnSource(view);

    //        if (gridViews == null || gridViews.Count == 0) return;

    //        switch (e.Action)
    //        {

    //            case NotifyCollectionChangedAction.Add:

    //                foreach (GridView gridView in gridViews)

    //                    for (int i = 0; i < e.NewItems.Count; i++)

    //                        gridView.Columns.Insert(e.NewStartingIndex + i, CreateColumn(gridView, e.NewItems[i]));

    //                break;

    //            case NotifyCollectionChangedAction.Move:

    //                List<GridViewColumn> columns;

    //                foreach (GridView gridView in gridViews)
    //                {

    //                    columns = new List<GridViewColumn>();

    //                    for (int i = 0; i < e.OldItems.Count; i++)

    //                        columns.Add(gridView.Columns[e.OldStartingIndex + i]);

    //                    for (int i = 0; i < e.NewItems.Count; i++)

    //                        gridView.Columns.Insert(e.NewStartingIndex + i, columns[i]);

    //                }

    //                break;

    //            case NotifyCollectionChangedAction.Remove:

    //                foreach (GridView gridView in gridViews)

    //                    for (int i = 0; i < e.OldItems.Count; i++)

    //                        gridView.Columns.RemoveAt(e.OldStartingIndex);

    //                break;

    //            case NotifyCollectionChangedAction.Replace:

    //                foreach (GridView gridView in gridViews)

    //                    for (int i = 0; i < e.NewItems.Count; i++)

    //                        gridView.Columns[e.NewStartingIndex + i] = CreateColumn(gridView, e.NewItems[i]);

    //                break;

    //            case NotifyCollectionChangedAction.Reset:

    //                foreach (GridView gridView in gridViews)
    //                {

    //                    gridView.Columns.Clear();

    //                    CreateColumns(gridView, sender as ICollectionView);

    //                }

    //                break;

    //            default:

    //                break;

    //        }

    //    }

    //    private static GridViewColumn CreateColumn(GridView gridView, object columnSource)
    //    {

    //        GridViewColumn column = new GridViewColumn();

    //        string headerTextMember = GetHeaderTextMember(gridView);

    //        string displayMemberMember = GetDisplayMemberMember(gridView);

    //        if (!string.IsNullOrEmpty(headerTextMember))

    //            column.Header = GetPropertyValue(columnSource, headerTextMember);

    //        if (!string.IsNullOrEmpty(displayMemberMember))

    //            column.DisplayMemberBinding = new Binding(GetPropertyValue(columnSource, displayMemberMember) as string);

    //        return column;

    //    }

    //    private static object GetPropertyValue(object obj, string propertyName) => obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null);

    //}

}
