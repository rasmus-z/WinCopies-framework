using System;
using System.Globalization;
using System.Windows.Controls;
using WinCopies.Util.DataConverters;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Explorer.Data
{
    public class ViewStyleConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //#if DEBUG
            //            Console.WriteLine("ViewStyleConverter value: "+value);
            //#endif

            string result = If(ComparisonType.Or, Comparison.Equals, (ViewStyles)value, ViewStyles.SizeOne,
                ViewStyles.SizeTwo,
                ViewStyles.SizeThree,
                ViewStyles.SizeFour,
                ViewStyles.Tiles) ? "SizeOneToFourOrTiles" : (ViewStyles)value == ViewStyles.List ? "ListItems" : null;

            string _parameter = (string)parameter;

            return _parameter == "ScrollBarsVisibility" ? result : _parameter == "WrapPanelOrientation" ? result == "SizeOneToFourOrTiles" ? Orientation.Horizontal : result == "ListItems" ? (object)Orientation.Vertical : null : null;

        }

        //#if DEBUG//                    Console.WriteLine("ViewStyleConverter return value: " + "SizeOneToFour");//#endif//#if DEBUG//            Console.WriteLine("ViewStyleConverter return value: " + "ListItems");//#endif

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
