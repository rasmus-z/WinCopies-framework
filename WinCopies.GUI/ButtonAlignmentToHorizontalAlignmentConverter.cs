using System;
using System.Globalization;
using System.Windows.Data;
using WinCopies.Util.Data;

namespace WinCopies.GUI.Windows.Dialogs
{
    [ValueConversion(typeof(HorizontalAlignment), typeof(System.Windows.HorizontalAlignment))]
    public class ButtonAlignmentToHorizontalAlignmentConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HorizontalAlignment _value)

                switch (_value)

                {

                    case HorizontalAlignment.Left:

                        return System.Windows.HorizontalAlignment.Left;

                    case HorizontalAlignment.Right:

                        return System.Windows.HorizontalAlignment.Right;

                    default:

                        throw new ArgumentException("Invalid value for HorizontalAlignment.");

                }

            else

                return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.HorizontalAlignment _value)

                switch (_value)

                {

                    case System.Windows.HorizontalAlignment.Left:

                        return HorizontalAlignment.Left;

                    case System.Windows.HorizontalAlignment.Right:

                        return HorizontalAlignment.Right;

                    default:

                        throw new ArgumentException("Invalid value for HorizontalAlignment.");

                }

            else

                return null;
        }
    }
}
