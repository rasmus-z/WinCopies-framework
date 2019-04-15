using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WinCopies.Util;
using WinCopies.Win32NativeInterop;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace WinCopies.IO
{

    public enum CommonClipboardFormats
    {

        None,

        Audio,

        Image,

        Text,

        FileDropList,

        Data,

        All

    }

    /// <summary>
    /// The clipboard formats defined by the system.
    /// </summary>
    public enum StandardClipboardFormats : int
    {

        /// <summary>
        /// A handle to a bitmap (HBITMAP).
        /// </summary>
        CF_BITMAP = 2,

        /// <summary>
        /// A memory object containing a BITMAPINFO structure followed by the bitmap bits.
        /// </summary>
        CF_DIB = 8,

        /// <summary>
        /// A memory object containing a BITMAPV5HEADER structure followed by the bitmap color space information and the bitmap bits.
        /// </summary>
        CF_DIBV5 = 17,

        /// <summary>
        /// Software Arts' Data Interchange Format.
        /// </summary>
        CF_DIF = 5,

        /// <summary>
        /// Bitmap display format associated with a private format. The hMem parameter must be a handle to data that can be displayed in bitmap format in lieu of the privately formatted data.
        /// </summary>
        CF_DSPBITMAP = 0x0082,

        /// <summary>
        /// Enhanced metafile display format associated with a private format. The hMem parameter must be a handle to data that can be displayed in enhanced metafile format in lieu of the privately formatted data.
        /// </summary>
        CF_DSPENHMETAFILE = 0x008E,

        /// <summary>
        /// Metafile-picture display format associated with a private format. The hMem parameter must be a handle to data that can be displayed in metafile-picture format in lieu of the privately formatted data.
        /// </summary>
        CF_DSPMETAFILEPICT = 0x0083,

        /// <summary>
        /// Text display format associated with a private format. The hMem parameter must be a handle to data that can be displayed in text format in lieu of the privately formatted data.
        /// </summary>
        CF_DSPTEXT = 0x0081,

        /// <summary>
        /// A handle to an enhanced metafile (HENHMETAFILE).
        /// </summary>
        CF_ENHMETAFILE = 14,

        /// <summary>
        /// Start of a range of integer values for application-defined GDI object clipboard formats. The end of the range is CF_GDIOBJLAST.
        /// Handles associated with clipboard formats in this range are not automatically deleted using the GlobalFree function when the clipboard is emptied. Also, when using values in this range, the hMem parameter is not a handle to a GDI object, but is a handle allocated by the GlobalAlloc function with the GMEM_MOVEABLE flag.
        /// </summary>
        CF_GDIOBJFIRST = 0x0300,

        /// <summary>
        /// See CF_GDIOBJFIRST.
        /// </summary>
        CF_GDIOBJLAST = 0x03FF,

        /// <summary>
        /// A handle to type HDROP that identifies a list of files. An application can retrieve information about the files by passing the handle to the DragQueryFile function.
        /// </summary>
        CF_HDROP = 15,

        /// <summary>
        /// The data is a handle to the locale identifier associated with text in the clipboard. When you close the clipboard, if it contains CF_TEXT data but no CF_LOCALE data, the system automatically sets the CF_LOCALE format to the current input language. You can use the CF_LOCALE format to associate a different locale with the clipboard text.
        /// An application that pastes text from the clipboard can retrieve this format to determine which character set was used to generate the text.
        /// Note that the clipboard does not support plain text in multiple character sets.To achieve this, use a formatted text data type such as RTF instead.
        /// The system uses the code page associated with CF_LOCALE to implicitly convert from CF_TEXT to CF_UNICODETEXT. Therefore, the correct code page table is used for the conversion.
        /// </summary>
        CF_LOCALE = 16,

        /// <summary>
        /// Handle to a metafile picture format as defined by the METAFILEPICT structure. When passing a CF_METAFILEPICT handle by means of DDE, the application responsible for deleting hMem should also free the metafile referred to by the CF_METAFILEPICT handle.
        /// </summary>
        CF_METAFILEPICT = 3,

        /// <summary>
        /// Text format containing characters in the OEM character set. Each line ends with a carriage return/linefeed (CR-LF) combination. A null character signals the end of the data.
        /// </summary>
        CF_OEMTEXT = 7,

        /// <summary>
        /// Owner-display format. The clipboard owner must display and update the clipboard viewer window, and receive the WM_ASKCBFORMATNAME, WM_HSCROLLCLIPBOARD, WM_PAINTCLIPBOARD, WM_SIZECLIPBOARD, and WM_VSCROLLCLIPBOARD messages. The hMem parameter must be NULL.
        /// </summary>
        CF_OWNERDISPLAY = 0x0080,

        /// <summary>
        /// Handle to a color palette. Whenever an application places data in the clipboard that depends on or assumes a color palette, it should place the palette on the clipboard as well.
        /// If the clipboard contains data in the CF_PALETTE (logical color palette) format, the application should use the SelectPalette and RealizePalette functions to realize (compare) any other data in the clipboard against that logical palette.
        /// When displaying clipboard data, the clipboard always uses as its current palette any object on the clipboard that is in the CF_PALETTE format.
        /// </summary>
        CF_PALETTE = 9,

        /// <summary>
        /// Data for the pen extensions to the Microsoft Windows for Pen Computing.
        /// </summary>
        CF_PENDATA = 10,

        /// <summary>
        /// Start of a range of integer values for private clipboard formats.The range ends with CF_PRIVATELAST. Handles associated with private clipboard formats are not freed automatically; the clipboard owner must free such handles, typically in response to the WM_DESTROYCLIPBOARD message.
        /// </summary>
        CF_PRIVATEFIRST = 0x0200,

        /// <summary>
        /// See CF_PRIVATEFIRST.
        /// </summary>
        CF_PRIVATELAST = 0x02FF,

        /// <summary>
        /// Represents audio data more complex than can be represented in a <see cref="CF_WAVE"/> standard wave format.
        /// </summary>
        CF_RIFF = 11,

        /// <summary>
        /// Microsoft Symbolic Link (SYLK) format.
        /// </summary>
        CF_SYLK = 4,

        /// <summary>
        /// Text format. Each line ends with a carriage return/linefeed (CR-LF) combination. A null character signals the end of the data.Use this format for ANSI text.
        /// </summary>
        CF_TEXT = 1,

        /// <summary>
        /// Tagged-image file format.
        /// </summary>
        CF_TIFF = 6,

        /// <summary>
        /// Unicode text format.Each line ends with a carriage return/linefeed (CR-LF) combination. A null character signals the end of the data.
        /// </summary>
        CF_UNICODETEXT = 13,

        /// <summary>
        /// Represents audio data in one of the standard wave formats, such as 11 kHz or 22 kHz PCM.
        /// </summary>
        CF_WAVE = 12

    }

    public static class Clipboard
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr ownerWindowNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardData(StandardClipboardFormats uFormat);

        public enum GetClipboardDataFlags
        {
            RECO_PASTE = 0,
            RECO_DROP = 1,
            RECO_COPY = 2,
            RECO_CUT = 3,
            RECO_DRAG = 4
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsClipboardFormatAvailable(int format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int EnumClipboardFormats(int format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EmptyClipboard();

        [DllImport("ole32.dll")]
        public static extern uint OleGetClipboard([MarshalAs(UnmanagedType.IUnknown)]out object ppDataObj);

        [DllImport("ole32.dll")]
        public static extern uint OleSetClipboard(IDataObject pDataObj);

        [DllImport("ole32.dll")]
        public static extern int OleFlushClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();

        private static bool ContainsInternal(bool value, out StandardClipboardFormats formatToSet, StandardClipboardFormats format)

        {

            CloseClipboard();

            formatToSet = format;

            return value;

        }

        public static bool Contains(WindowInteropHelper ownerWindow, string format)
        {

            OpenClipboard(ownerWindow.Handle);

            int formatId = DataFormats.GetDataFormat(format).Id;

            CloseClipboard();

            return IsClipboardFormatAvailable(formatId);

        }

        public static bool Contains(WindowInteropHelper ownerWindow, CommonClipboardFormats format, out StandardClipboardFormats standardClipboardFormat)

        {

            format.ThrowIfNotValidEnumValue();

            OpenClipboard(ownerWindow.Handle);

            switch (format)

            {

                case CommonClipboardFormats.None:

                    foreach (object value in typeof(StandardClipboardFormats).GetEnumValues())

                        if (IsClipboardFormatAvailable((int)value))

                            return ContainsInternal(false, out standardClipboardFormat, (StandardClipboardFormats)value);

                    return ContainsInternal(true, out standardClipboardFormat, 0);

                case CommonClipboardFormats.All:

                    Array values = typeof(StandardClipboardFormats).GetEnumValues();

                    foreach (object value in values)

                        if (!IsClipboardFormatAvailable((int)value))

                            return ContainsInternal(false, out standardClipboardFormat, (StandardClipboardFormats)value);

                    return ContainsInternal(true, out standardClipboardFormat, (StandardClipboardFormats)values.ToList()[0]);

            }

            StandardClipboardFormats[] formats;

            switch (format)

            {

                case CommonClipboardFormats.Audio:

                    formats = new StandardClipboardFormats[] { StandardClipboardFormats.CF_RIFF, StandardClipboardFormats.CF_WAVE };

                    break;

                case CommonClipboardFormats.Image:

                    formats = new StandardClipboardFormats[] { StandardClipboardFormats.CF_BITMAP, StandardClipboardFormats.CF_DIB, StandardClipboardFormats.CF_DIBV5, StandardClipboardFormats.CF_DSPBITMAP, StandardClipboardFormats.CF_PALETTE, StandardClipboardFormats.CF_PENDATA, StandardClipboardFormats.CF_TIFF };

                    break;

                case CommonClipboardFormats.Text:

                    formats = new StandardClipboardFormats[] { StandardClipboardFormats.CF_DSPTEXT, StandardClipboardFormats.CF_LOCALE, StandardClipboardFormats.CF_OEMTEXT, StandardClipboardFormats.CF_TEXT, StandardClipboardFormats.CF_UNICODETEXT };

                    break;

                case CommonClipboardFormats.FileDropList:

                    formats = new StandardClipboardFormats[] { StandardClipboardFormats.CF_HDROP };

                    break;

                case CommonClipboardFormats.Data:

                    formats = new StandardClipboardFormats[] { StandardClipboardFormats.CF_DSPENHMETAFILE, StandardClipboardFormats.CF_DSPMETAFILEPICT, StandardClipboardFormats.CF_ENHMETAFILE, StandardClipboardFormats.CF_GDIOBJFIRST, StandardClipboardFormats.CF_GDIOBJLAST, StandardClipboardFormats.CF_METAFILEPICT, StandardClipboardFormats.CF_OWNERDISPLAY, StandardClipboardFormats.CF_PRIVATEFIRST, StandardClipboardFormats.CF_PRIVATELAST, StandardClipboardFormats.CF_SYLK };

                    break;

                default:

                    // We wouldn't reach this code.

                    standardClipboardFormat = 0;

                    return false;

            }

            foreach (StandardClipboardFormats value in formats)

                if (IsClipboardFormatAvailable((int)value))

                    return ContainsInternal(true, out standardClipboardFormat, value);

            return ContainsInternal(false, out standardClipboardFormat, 0);

        }

        public static System.Windows.IDataObject GetDataObject(WindowInteropHelper ownerWindow)

        {

            OpenClipboard(ownerWindow.Handle);

            object oleDataObject;

            uint result = OleGetClipboard(out oleDataObject);

            /*if (closeClipboard) */
            CloseClipboard();

            if (result == (int)ErrorCodes.ERROR_SUCCESS)

                return oleDataObject is System.Windows.IDataObject ? (System.Windows.IDataObject)oleDataObject : oleDataObject != null ? new DataObject(oleDataObject) : null;

            else

                throw new Win32Exception((int)result);

        }

        public static object GetData(WindowInteropHelper ownerWindow, string format) => GetDataObject(ownerWindow)?.GetData(format);

        public static object GetData(WindowInteropHelper ownerWindow, StandardClipboardFormats format) => GetDataObject(ownerWindow)?.GetData(DataFormats.GetDataFormat((int)format).Name);

        public static Stream GetAudioStream(WindowInteropHelper ownerWindow) => GetData(ownerWindow, StandardClipboardFormats.CF_WAVE) as Stream;

        public static StringCollection GetFileDropList(WindowInteropHelper ownerWindow)

        {

            string[] fileDropArray = GetData(ownerWindow, StandardClipboardFormats.CF_HDROP) as string[];

            StringCollection sc = new StringCollection();

            if (fileDropArray != null)

                sc.AddRange(fileDropArray);

            return sc;

        }

        public static BitmapSource GetBitmapSource(WindowInteropHelper ownerWindow) => GetData(ownerWindow, StandardClipboardFormats.CF_BITMAP) as BitmapSource;

        public static string GetText(WindowInteropHelper ownerWindow) => GetData(ownerWindow, StandardClipboardFormats.CF_UNICODETEXT) as string;

        public static string GetText(WindowInteropHelper ownerWindow, TextDataFormat format)

        {

            format.ThrowIfNotValidEnumValue();

            int standardClipboardFormat;

            switch (format)

            {

                case TextDataFormat.Text:

                    standardClipboardFormat = (int)StandardClipboardFormats.CF_TEXT;

                    break;

                case TextDataFormat.UnicodeText:

                    standardClipboardFormat = (int)StandardClipboardFormats.CF_UNICODETEXT;

                    break;

                case TextDataFormat.Rtf:

                    standardClipboardFormat = DataFormats.GetDataFormat(DataFormats.Rtf).Id;

                    break;

                case TextDataFormat.Html:

                    standardClipboardFormat = DataFormats.GetDataFormat(DataFormats.Html).Id;

                    break;

                case TextDataFormat.CommaSeparatedValue:

                    standardClipboardFormat = DataFormats.GetDataFormat(DataFormats.CommaSeparatedValue).Id;

                    break;

                case TextDataFormat.Xaml:

                    standardClipboardFormat = DataFormats.GetDataFormat(DataFormats.Xaml).Id;

                    break;

                default:

                    // We wouldn't reach this code.

                    return null;

            }

            return GetData(ownerWindow, (StandardClipboardFormats)standardClipboardFormat) as string;

        }

        public static void Flush()

        {

            int result = OleFlushClipboard();

            if (result != (int)ErrorCodes.ERROR_SUCCESS)

                throw new Win32Exception(result);

        }
        const uint CLIPBRD_E_CANT_OPEN = 0x800401D0;
        public static void SetDataObject(IDataObject dataObject, bool copy)

        {

            //bool r = OpenClipboard(ownerWindow.Handle);

            // r = EmptyClipboard();

            //if (OpenClipboard(ownerWindow.Handle))

            //{

            bool value = false;
            uint result = 0;

            for (int i = 0; i < 10; i++)
            {
                result = OleSetClipboard(dataObject);
                value = CLIPBRD_E_CANT_OPEN == result;
                if (value)
                    Thread.Sleep(10);
                else
                    break;
            }

            if (result != (int)ErrorCodes.ERROR_SUCCESS)

                throw new Win32Exception((int)result);

            if (copy)

            {

                result = (uint)OleFlushClipboard();

                if (result != (int)ErrorCodes.ERROR_SUCCESS)

                {

                    // CloseClipboard();

                    throw new Win32Exception((int)result);

                }

            }

            // CloseClipboard();

            // }

        }

        public static void SetData(WindowInteropHelper ownerWindow, string format, object data, bool copy) => SetDataObject(data is IDataObject ? (IDataObject)data : new DataObject(format, data), copy);

        public static void SetData(WindowInteropHelper ownerWindow, StandardClipboardFormats format, object data, bool copy) => SetDataObject(data is IDataObject ? (IDataObject)data : new DataObject(DataFormats.GetDataFormat((int)format).Name, data), copy);

        public static void SetAudio(WindowInteropHelper ownerWindow, Stream data, bool copy) => SetData(ownerWindow, StandardClipboardFormats.CF_WAVE, data, copy);

        public static void SetFileDropList(WindowInteropHelper ownerWindow, StringCollection sc, bool copy) => SetData(ownerWindow, StandardClipboardFormats.CF_HDROP, sc.ToList().ToArray(typeof(string)), copy);

    }
}
