using System;
using System.Globalization;
using WinCopies.Util.DataConverters;

namespace WinCopies.GUI.Explorer.Data
{
    public class PathToItemsConverter : ConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? null : ((IBrowsableObjectInfo)value).Items;// ((ShellObjectInfo)value).PropertyChanged += PathToItemsConverter_PropertyChanged;// if (items != null) ((INotifyCollectionChanged)items).CollectionChanged += PathToItemsConverter_CollectionChanged;

        //private void PathToItemsConverter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    Debug.WriteLine("PathToItemsConverter_PropertyChanged");
        //}
        //private void PathToItemsConverter_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    Debug.WriteLine("PathToItemsConverter_CollectionChanged");
        //}

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }
}
