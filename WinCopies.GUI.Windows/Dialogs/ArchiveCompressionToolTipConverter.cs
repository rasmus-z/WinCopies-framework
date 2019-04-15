using System;
using System.Globalization;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class ArchiveCompressionToolTipConverter : Util.DataConverters.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {

            if ((bool?)value == true)

                switch ((string)parameter)

                {

                    case "ArchiveFormat":

                        return /*new Label { Content = ((string) */ResourcesHelper.Instance.ResourceDictionary["ArchiveFormatDescription"]/*).Replace("&#x0a;", "\n") }*/ ;

                    case "ArchiveCompressionMethod":

                        return ResourcesHelper.Instance.ResourceDictionary["ArchiveCompressionMethodDescription"];

                    case "ArchiveCompressionLevel":

                        return ResourcesHelper.Instance.ResourceDictionary["ArchiveCompressionLevelDescription"];

                    case "ArchiveCompressionMode":

                        return ResourcesHelper.Instance.ResourceDictionary["ArchiveCompressionModeDescription"];

                    default:

                        return null;

                }

            else

                return null;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
