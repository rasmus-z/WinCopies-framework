using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinCopies.GUI.Icons
{
    // todo: to put in WinCopies.Util.Desktop

    public class IconToImageSourceConverter : Util.Data.ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is Bitmap _value ? ToImageSource(_value) : null;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    //public static ImageSource ToImageSource(this Icon icon)

    //{

    //    IntPtr hIcon = icon.Handle;

    //    BitmapSource wpfIcon = Imaging.CreateBitmapSourceFromHIcon(
    //        hIcon,
    //        Int32Rect.Empty,
    //        BitmapSizeOptions.FromEmptyOptions());

    //    //if (!Util.DeleteObject(hIcon))

    //    //    throw new Win32Exception();

    //    //using (MemoryStream memoryStream = new MemoryStream())

    //    //{

    //    //    icon.ToBitmap().Save(memoryStream, ImageFormat.Png);

    //    //    IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

    //    //    return (ImageSource) new ImageSourceConverter().ConvertFrom( iconBitmapDecoder);

    //    //}

    //    ImageSource imageSource;

    //    // Icon icon = Icon.ExtractAssociatedIcon(path);

    //    using (Bitmap bmp = icon.ToBitmap())
    //    {
    //        var stream = new MemoryStream();
    //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
    //        imageSource = BitmapFrame.Create(stream);
    //    }

    //    return imageSource;

    //    return icon.ToBitmap().ToImageSource();

    //    return wpfIcon;

    //}

    //CS7

    /// <summary>
    /// Converts a <see cref="Bitmap"/> to an <see cref="ImageSource"/>.
    /// </summary>
    /// <param name="bitmap">The <see cref="Bitmap"/> to convert.</param>
    /// <returns>The <see cref="ImageSource"/> obtained from the given <see cref="Bitmap"/>.</returns>
    public static ImageSource ToImageSource(/*this*/ Bitmap bitmap)

    {

        bitmap.MakeTransparent();

        IntPtr hBitmap = bitmap.GetHbitmap();

        ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
        
        if (!Microsoft.WindowsAPICodePack.Win32Native.GDI.GDI.DeleteObject(hBitmap))

            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

        return wpfBitmap ; 

        //            //using (MemoryStream stream = new MemoryStream())
        //            //{
        //            //    bitmap.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

        //            //    stream.Position = 0;
        //            //    BitmapImage result = new BitmapImage();
        //            //    result.BeginInit();
        //            //    // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
        //            //    // Force the bitmap to load right now so we can dispose the stream.
        //            //    result.CacheOption = BitmapCacheOption.OnLoad;
        //            //    result.StreamSource = stream;
        //            //    result.EndInit();
        //            //    result.Freeze();
        //            //    return result;
        //            //}

        //            return wpfBitmap;

    }

    //#endif
    }
}
