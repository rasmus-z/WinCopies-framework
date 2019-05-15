using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinCopies.Win32NativeInterop.NativeMethods;

namespace WinCopies.Win32NativeInterop
{
    /// <summary>
    /// The result that is returned by the <see cref="CopyProgressRoutine"/> function.
    /// </summary>
    public enum CopyProgressResult : uint
    {
        /// <summary>
        /// Continue the copy operation.
        /// </summary>
        PROGRESS_CONTINUE = 0,

        /// <summary>
        /// Cancel the copy operation and delete the destination file.
        /// </summary>
        PROGRESS_CANCEL = 1,

        /// <summary>
        /// Stop the copy operation. It can be restarted at a later time.
        /// </summary>
        PROGRESS_STOP = 2,

        /// <summary>
        /// Continue the copy operation, but stop invoking CopyProgressRoutine to report progress.
        /// </summary>
        PROGRESS_QUIET = 3
    }
}
