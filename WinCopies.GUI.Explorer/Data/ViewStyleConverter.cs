using System;
using System.Globalization;
using WinCopies.Util.DataConverters;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Explorer.Data
{
    public class ViewStyleConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        //#if DEBUG
        //            Console.WriteLine("ViewStyleConverter value: "+value);
        //#endif

        If(ComparisonType.Or, Comparison.Equals, (ViewStyles)value, ViewStyles.SizeOne,
                ViewStyles.SizeTwo,
                ViewStyles.SizeThree,
                ViewStyles.SizeFour) ? "SizeOneToFour" : (ViewStyles)value == ViewStyles.List ? "ListItems" : null;

        //#if DEBUG//                    Console.WriteLine("ViewStyleConverter return value: " + "SizeOneToFour");//#endif//#if DEBUG//            Console.WriteLine("ViewStyleConverter return value: " + "ListItems");//#endif

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
