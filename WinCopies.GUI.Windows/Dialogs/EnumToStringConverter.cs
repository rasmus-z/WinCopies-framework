using System;
using System.Globalization;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class EnumToStringConverter : WinCopies.Util.Data.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.ToString();

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)

                return 0;

            else

            {

                // todo : to add a 'new' Enum.TryParse() method in order to avoid types compatibility problems.

                try

                {

                    return Enum.Parse(targetType, (string)value);

                }

                catch (Exception)

                {

                    return 0;

                }

            }

        }
    }
}
