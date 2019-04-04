using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace WinCopies.GUI.Explorer.Data
{
    public class HistorySelectedItemConverter : Util.DataConverters.MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => ((ReadOnlyObservableCollection<IHistoryItemData>)values[1]).IndexOf((IHistoryItemData)values[0]) == (int)values[2];

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
