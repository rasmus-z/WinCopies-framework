using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Win32Interop;
using static WinCopies.Win32Interop.NativeMethods;
// using static WinCopies.Win32Interop.Icon.Icon;

namespace WinCopies.Win32Interop.Icon
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Media;
    using WinCopies.Win32NativeInterop;

    /// <summary>
    /// Internals are mostly from here: http://www.codeproject.com/Articles/2532/Obtaining-and-managing-file-and-folder-icons-using
    /// Caches all results.
    /// </summary>
    public class Icon
    {

        /// <summary>Maximal Length of unmanaged Windows-Path-strings</summary>
        private const int MAX_PATH = 260;
        /// <summary>Maximal Length of unmanaged Typename</summary>
        private const int MAX_TYPE = 80;

        public const int FILE_ATTRIBUTE_NORMAL = 0x80;

        private static readonly Dictionary<string, Icon> _smallIconCache = new Dictionary<string, Icon>();
        private static readonly Dictionary<string, Icon> _mediumIconCache = new Dictionary<string, Icon>();
        private static readonly Dictionary<string, Icon> _largeIconCache = new Dictionary<string, Icon>();
        private static readonly Dictionary<string, Icon> _extraLargeIconCache = new Dictionary<string, Icon>();

        public static ReadOnlyDictionary<string, Icon> SmallIconCache { get; } = new ReadOnlyDictionary<string, Icon>(_smallIconCache);
        public static ReadOnlyDictionary<string, Icon> MediumIconCache { get; } = new ReadOnlyDictionary<string, Icon>(_mediumIconCache);
        public static ReadOnlyDictionary<string, Icon> LargeIconCache { get; } = new ReadOnlyDictionary<string, Icon>(_largeIconCache);
        public static ReadOnlyDictionary<string, Icon> ExtraLargeIconCache { get; } = new ReadOnlyDictionary<string, Icon>(_extraLargeIconCache);

        public static Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

        /*
 * @method
 *        getFileIcon()
 * @param
 *        int       : icon type--FILE_ICON_SMALL                     0     16x16
 *                                         FILE_ICON_MEDIUM                  1     32x32
 *                                         FILE_ICON_LARGE                      2      48x48
 *                                         FILE_ICON_EXTRALARGE        3      256x256
 */
        /// <summary>
        /// Get associated file icon from the extension name
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="type"></param>
        /// <returns>the return icon</returns>
        public static IntPtr getFileIcon(string ext, IconSize type)
        {
            IntPtr hIcon;
            SHFILEINFO sfi = new SHFILEINFO();
            SHGFI flag = SHGFI.Icon | SHGFI.UseFileAttributes;

            switch (type)
            {
                case IconSize.Small: flag |= SHGFI.SmallIcon; break;
                case IconSize.Medium: flag |= SHGFI.LargeIcon; break;
                case IconSize.Large:
                case IconSize.ExtraLarge: flag |= SHGFI.SysIconIndex; break;
            }
            HResult hr = (HResult)Marshal.ReadInt32(SHGetFileInfo(ext.ToLowerInvariant(), FILE_ATTRIBUTE_NORMAL, ref sfi, (uint)Marshal.SizeOf(sfi), flag));
            //if (hr == HResult.Ok)
            //{
            if (type == IconSize.Large || type == IconSize.ExtraLarge)
            {
                // Retrieve the system image list.
                IImageList imageList;
                hr = SHGetImageList(((type == IconSize.ExtraLarge) ? SHIL.JUMBO : SHIL.EXTRALARGE), ref IID_IImageList, out imageList);

                if (hr == HResult.Ok)

                {

                    // Get the icon we need from the list. Note that the HIMAGELIST we retrieved
                    // earlier needs to be casted to the IImageList interface before use.
                    hr = ((IImageList)imageList).GetIcon(sfi.iIcon, ILD_FLAGS.ILD_TRANSPARENT, out hIcon);

                }

                else

                    throw new Win32Exception((int)hr);
            }
            else
            {
                hIcon = sfi.hIcon;
            }
            //}

            //else

            //    throw new Win32Exception((int)hr);

            return hIcon;
        }

        /// <summary>
        /// Retrieves COM <see cref="IImageList"/> Interface which contains Image List.
        /// </summary>
        /// <param name="iImageList">The image type contained in the list.</param>
        /// <param name="riid">Reference to the image list interface identifier, normally IID_IImageList.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IImageList.</param>
        /// <returns>If this function succeeds, it returns <see cref="HResult.Ok"/>. Otherwise, it returns an <see cref="HResult"/> error code.</returns>
        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static HResult SHGetImageList(SHIL iImageList, ref Guid riid, out IImageList ppv);

        /// <summary>
        /// Destroys an icon and frees any memory the icon occupied. See the Remarks section.
        /// </summary>
        /// <param name="hIcon">A handle to the icon to be destroyed. The icon must not be in use.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        /// <remarks>It is only necessary to call DestroyIcon for icons and cursors created with the following functions: CreateIconFromResourceEx (if called without the LR_SHARED flag), CreateIconIndirect, and CopyIcon. Do not use this function to destroy a shared icon. A shared icon is valid as long as the module from which it was loaded remains in memory. The following functions obtain a shared icon.
        /// LoadIcon
        /// LoadImage(if you use the LR_SHARED flag)
        /// CopyImage(if you use the LR_COPYRETURNORG flag and the hImage parameter is a shared icon)
        /// CreateIconFromResource
        /// CreateIconFromResourceEx(if you use the LR_SHARED flag)</remarks>
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// Get an icon for a given filename
        /// </summary>
        /// <param name="fileName">any filename</param>
        /// <param name="large">16x16 or 32x32 icon</param>
        /// <returns>null if path is null, otherwise - an icon</returns>
        public static ImageSource FindIconForFilename(string fileName, bool large)
        {
            string extension = Path.GetExtension(fileName);
            // todo: exception
            if (extension == null)
                return null;
            Dictionary<string, ImageSource> cache = large ? _largeIconCache : _smallIconCache;
            ImageSource icon;
            if (cache.TryGetValue(extension, out icon))
                return icon;
            icon = IconReader.GetFileIcon(fileName, large ? IconSize.Large : IconSize.Small, false).ToImageSource();
            cache.Add(extension, icon);
            return icon;
        }
        /// <summary>
        /// http://stackoverflow.com/a/6580799/1943849
        /// </summary>
        static ImageSource ToImageSource(this Icon icon)
        {
            var imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return imageSource;
        }
        /// <summary>
        /// Provides static methods to read system icons for both folders and files.
        /// </summary>
        /// <example>
        /// <code>IconReader.GetFileIcon("c:\\general.xls");</code>
        /// </example>
        static class IconReader
        {
            /// <summary>
            /// Returns an icon for a given file - indicated by the name parameter.
            /// </summary>
            /// <param name="name">Pathname for file.</param>
            /// <param name="size">Large or small</param>
            /// <param name="linkOverlay">Whether to include the link icon</param>
            /// <returns>System.Drawing.Icon</returns>
            public static Icon GetFileIcon(string name, IconSize size, bool linkOverlay)
            {
                var shfi = new Shell32.Shfileinfo();
                var flags = Shell32.ShgfiIcon | Shell32.ShgfiUsefileattributes;
                if (linkOverlay) flags += Shell32.ShgfiLinkoverlay;
                /* Check the size specified for return. */
                if (IconSize.Small == size)
                    flags += Shell32.ShgfiSmallicon;
                else
                    flags += Shell32.ShgfiLargeicon;
                Shell32.SHGetFileInfo(name,
                    Shell32.FileAttributeNormal,
                    ref shfi,
                    (uint)Marshal.SizeOf(shfi),
                    flags);
                // Copy (clone) the returned icon to a new object, thus allowing us to clean-up properly
                var icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
                User32.DestroyIcon(shfi.hIcon);     // Cleanup
                return icon;
            }
        }
        private const int MaxPath = 256;
        ///// <summary>
        ///// Wraps necessary Shell32.dll structures and functions required to retrieve Icon Handles using SHGetFileInfo. Code
        ///// courtesy of MSDN Cold Rooster Consulting case study.
        ///// </summary>
        //static class Shell32
        //{
        //    //[StructLayout(LayoutKind.Sequential)]
        //    //public struct Shfileinfo
        //    //{
        //    //    private const int Namesize = 80;
        //    //    public readonly IntPtr hIcon;
        //    //    private readonly int iIcon;
        //    //    private readonly uint dwAttributes;
        //    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
        //    //    private readonly string szDisplayName;
        //    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Namesize)]
        //    //    private readonly string szTypeName;
        //    //};
        //    //public const uint ShgfiIcon = 0x000000100;     // get icon
        //    //public const uint ShgfiLinkoverlay = 0x000008000;     // put a link overlay on icon
        //    //public const uint ShgfiLargeicon = 0x000000000;     // get large icon
        //    //public const uint ShgfiSmallicon = 0x000000001;     // get small icon
        //    //public const uint ShgfiUsefileattributes = 0x000000010;     // use passed dwFileAttribute
        //    //public const uint FileAttributeNormal = 0x00000080;
        //    //[DllImport("Shell32.dll")]
        //    //public static extern IntPtr SHGetFileInfo(
        //    //    string pszPath,
        //    //    uint dwFileAttributes,
        //    //    ref Shfileinfo psfi,
        //    //    uint cbFileInfo,
        //    //    uint uFlags
        //    //    );
        //}
    }

    public enum IconSize
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        ExtraLarge = 3
    }

    
}
