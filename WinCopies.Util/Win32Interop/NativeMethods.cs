using System;
using System.Runtime.InteropServices;

namespace WinCopies.Util.Win32Interop
{
    public static class NativeMethods
    {

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

    }
}
