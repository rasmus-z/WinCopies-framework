﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Win32NativeInterop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int left, top, right, bottom;
    }
}
