using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Diagnostics;
using System.Globalization;

namespace WinCopies.IO
{
    public class ShellPropertyConverter : WinCopies.Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ShellPropertyContainer _value = (ShellPropertyContainer)value;

            switch ((string)parameter)

            {

                case "Visibility":

                    return !string.IsNullOrEmpty(_value.Description.DisplayName)
                        ? System.Windows.Visibility.Visible
                        : (object)System.Windows.Visibility.Collapsed;

                case "IsReadOnly":

                    Debug.WriteLine(_value.CanonicalName + " " + _value.Description.DisplayName + " " + _value.Description.TypeFlags.HasFlag(PropertyTypeOptions.IsInnate));

                    return _value.Description.TypeFlags.HasFlag(PropertyTypeOptions.IsInnate);

                case "IsEnum":

                    Debug.WriteLine(_value.Description.DisplayName);

                    return _value.Description.DisplayType == PropertyDisplayType.Enumerated;

                case "EnumValues":

                    return _value.Description.PropertyEnumTypes;

                case "IsArray":

                    return _value.ValueAsObject is Array;

                default:

                    return null;

            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
