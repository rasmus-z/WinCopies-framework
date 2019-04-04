using System;
using System.Globalization;

namespace WinCopies.GUI.Windows.Dialogs
{
    public class CompressionMethodToVisibilityConverter : WinCopies.Util.DataConverters.MultiConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            SevenZip.CompressionMethod currentCompressionMethod = SevenZip.CompressionMethod.Default;

            currentCompressionMethod = (SevenZip.CompressionMethod)EnumToStringStaticResourceConverter.ConvertBackStatic(values[1], "CompressionMethod");

            switch ((SevenZip.OutArchiveFormat)values[0])

            {

                case SevenZip.OutArchiveFormat.Zip:

                    return currentCompressionMethod == SevenZip.CompressionMethod.Copy || currentCompressionMethod == SevenZip.CompressionMethod.Deflate || currentCompressionMethod == SevenZip.CompressionMethod.Deflate64 || currentCompressionMethod == SevenZip.CompressionMethod.BZip2 || currentCompressionMethod == SevenZip.CompressionMethod.Lzma || currentCompressionMethod == SevenZip.CompressionMethod.Ppmd || currentCompressionMethod == SevenZip.CompressionMethod.Default ?

                        System.Windows.Visibility.Visible :

                        System.Windows.Visibility.Collapsed;

                case SevenZip.OutArchiveFormat.SevenZip:

                    return currentCompressionMethod == SevenZip.CompressionMethod.Copy || currentCompressionMethod == SevenZip.CompressionMethod.BZip2 || currentCompressionMethod == SevenZip.CompressionMethod.Lzma || currentCompressionMethod == SevenZip.CompressionMethod.Lzma2 || currentCompressionMethod == SevenZip.CompressionMethod.Ppmd || currentCompressionMethod == SevenZip.CompressionMethod.Default ?

                        System.Windows.Visibility.Visible :

                        System.Windows.Visibility.Collapsed;

                default:

                    return currentCompressionMethod == SevenZip.CompressionMethod.Default ?

                        System.Windows.Visibility.Visible :

                        System.Windows.Visibility.Collapsed;

            }
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
