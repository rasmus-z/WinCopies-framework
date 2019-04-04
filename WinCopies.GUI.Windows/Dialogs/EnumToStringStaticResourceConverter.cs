using System;
using System.Globalization;
using System.Windows;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class EnumToStringStaticResourceConverter : WinCopies.Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)parameter)

            {

                case "CompressionLevel":
                case "CompressionMethod":
                case "CompressionMode":

                    string _value = value.ToString();
                    _value = (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary[string.Format("{0}s", (string)parameter)])[_value];
                    return _value;

                default:

                    return null;

            }
        }

        // todo: really needed with this approach?

        public static object ConvertBackStatic(object value, object parameter)

        {

            if (value is string)
            {
                switch ((string)parameter)

                {

                    case "CompressionLevel":

                        return (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["None"] ? SevenZip.CompressionLevel.None :

                            (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["Fast"] ? SevenZip.CompressionLevel.Fast :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["Low"] ? SevenZip.CompressionLevel.Low :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["Normal"] ? SevenZip.CompressionLevel.Normal :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["High"] ? SevenZip.CompressionLevel.High :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionLevels"])["Ultra"] ? (object)SevenZip.CompressionLevel.Ultra :

                        null;

                    case "CompressionMethod":

                        return (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Copy"] ? SevenZip.CompressionMethod.Copy :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Deflate"] ? SevenZip.CompressionMethod.Deflate :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Deflate64"] ? SevenZip.CompressionMethod.Deflate64 :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["BZip2"] ? SevenZip.CompressionMethod.BZip2 :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Lzma"] ? SevenZip.CompressionMethod.Lzma :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Lzma2"] ? SevenZip.CompressionMethod.Lzma2 :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Ppmd"] ? SevenZip.CompressionMethod.Ppmd :

                        (string)value == (string)((ResourceDictionary)ResourcesHelper.Instance.ResourceDictionary["CompressionMethods"])["Default"] ? (object)SevenZip.CompressionMethod.Default :

                        null;

                    case "CompressionMode":

                        return (string)value == (string) ((ResourceDictionary) ResourcesHelper.Instance.ResourceDictionary["CompressionModes"])["Append"] ? SevenZip.CompressionMode.Append :

                        (string)value == (string) ((ResourceDictionary) ResourcesHelper.Instance.ResourceDictionary["CompressionModes"])["Create"] ? (object)SevenZip.CompressionMode.Create :

                        null;

                    default:

                        return null;

                }

            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertBackStatic(value, parameter);
    }
}
