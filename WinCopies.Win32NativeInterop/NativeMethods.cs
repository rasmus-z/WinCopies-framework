using System;
using System.Runtime.InteropServices;

namespace WinCopies.Win32NativeInterop
{
    public static class NativeMethods
    {

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
      string pszPath,
      uint dwFileAttributes,
      ref SHFILEINFO psfi,
      uint cbfileInfo,
      SHGFI uFlags);

    }
}
