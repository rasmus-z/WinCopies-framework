using System;
using System.Diagnostics;
using System.Globalization;

namespace WinCopies.IO
{
    public class ShellPropertyConverter : WinCopies. Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _value = (ShellPropertyContainer) value;

            switch ((string)parameter)

            {

                case "Visibility":

                    if (!string.IsNullOrEmpty(_value.Description.DisplayName))

                        return System.Windows.Visibility.Visible;

                    else

                        return System.Windows.Visibility.Collapsed;

                case "IsReadOnly":

                    Debug.WriteLine(_value.CanonicalName + " " + _value.Description.DisplayName + " " + _value.Description.TypeFlags.HasFlag(Microsoft.WindowsAPICodePack.Shell.PropertySystem.PropertyTypeOptions.IsInnate));

                    return _value.Description.TypeFlags.HasFlag(Microsoft.WindowsAPICodePack.Shell.PropertySystem.PropertyTypeOptions.IsInnate);

                case "IsEnum":

                    Debug.WriteLine(_value.Description.DisplayName);

                    return _value.Description.DisplayType == Microsoft.WindowsAPICodePack.Shell.PropertySystem.PropertyDisplayType.Enumerated;

                case "EnumValues":

                    return _value.Description.PropertyEnumTypes;

                case "IsArray":

                    return _value.ValueAsObject is Array;

                default:

                    return null;

            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
