//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.Win32NativeInterop
//{
//    //[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
//    //If you use the above you may encounter an invalid memory access exception (when using ANSI
//    //or see nothing (when using unicode) when you use FOF_SIMPLEPROGRESS flag.
//    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
//    public struct SHFILEOPSTRUCT
//    {
//        public IntPtr hwnd;
//        public FileFuncFlags wFunc;
//        [MarshalAs(UnmanagedType.LPWStr)]
//        public string pFrom;
//        [MarshalAs(UnmanagedType.LPWStr)]
//        public string pTo;
//        public FILEOP_FLAGS fFlags;
//        [MarshalAs(UnmanagedType.Bool)]
//        public bool fAnyOperationsAborted;
//        public IntPtr hNameMappings;
//        [MarshalAs(UnmanagedType.LPWStr)]
//        public string lpszProgressTitle;
//    }
//    public enum FileFuncFlags : uint
//    {
//        FO_MOVE = 0x1,
//        FO_COPY = 0x2,
//        FO_DELETE = 0x3,
//        FO_RENAME = 0x4
//    }
//}
