using System;
using System.Globalization;

namespace WinCopies.GUI.Controls
{
    public class ProgressRectangleConverter : Util.DataConverters.MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => (ProcessStatus)values[0] == ProcessStatus.Indeterminate ? 0d : (((double)values[1] / (double)values[2]) * (double)values[3]);// double size = // return 

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException(); 
    }
}
