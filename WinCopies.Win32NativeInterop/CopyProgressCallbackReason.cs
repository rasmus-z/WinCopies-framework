using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinCopies.Win32NativeInterop.NativeMethods;

namespace WinCopies.Win32NativeInterop
{
    /// <summary>
    /// The reason that <see cref="CopyProgressRoutine"/> was called.
    /// </summary>
    public enum CopyProgressCallbackReason : uint
    {
        /// <summary>
        /// Another part of the data file was copied.
        /// </summary>
        CALLBACK_CHUNK_FINISHED = 0x00000000,

        /// <summary>
        /// Another stream was created and is about to be copied. This is the callback reason given when the callback routine is first invoked. 
        /// </summary>
        CALLBACK_STREAM_SWITCH = 0x00000001
    }
}
