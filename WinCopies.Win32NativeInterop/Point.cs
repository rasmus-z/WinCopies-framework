using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Win32NativeInterop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        int x;
        int y;
    }
}
