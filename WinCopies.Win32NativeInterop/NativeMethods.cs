using System;
using System.Runtime.InteropServices;

namespace WinCopies.Win32NativeInterop
{
    public static class NativeMethods
    {

        // [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        //public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("Shell32", CharSet = CharSet.Auto)]
        public static extern int ExtractIconEx(
            string lpszFile,
            int nIconIndex,
            IntPtr[] phIconLarge,
            IntPtr[] phIconSmall,
            int nIcons);

        //  [DllImport("shell32.dll")]
        //  public static extern IntPtr SHGetFileInfo(
        //string pszPath,
        //uint dwFileAttributes,
        //ref SHFILEINFO psfi,
        //uint cbfileInfo,
        //SHGFI uFlags);

        ///// <summary>
        ///// Retrieves COM <see cref="IImageList"/> Interface which contains Image List.
        ///// </summary>
        ///// <param name="iImageList">The image type contained in the list.</param>
        ///// <param name="riid">Reference to the image list interface identifier, normally IID_IImageList.</param>
        ///// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IImageList.</param>
        ///// <returns>If this function succeeds, it returns <see cref="HResult.Ok"/>. Otherwise, it returns an <see cref="HResult"/> error code.</returns>
        //[DllImport("shell32.dll", EntryPoint = "#727")]
        //public extern static HResult SHGetImageList(SHIL iImageList, ref Guid riid, out IImageList ppv);

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

    }
}
