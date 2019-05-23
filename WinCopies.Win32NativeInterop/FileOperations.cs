//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.Win32NativeInterop
//{
//    [Flags]
//    public enum FileOperations
//    {
//        /// <summary>
//        /// <para>Preserve undo information, if possible.</para>
//        /// <para>Prior to Windows Vista, operations could be undone only from the same process that performed the original operation.</para>
//        /// <para>In Windows Vista and later systems, the scope of the undo is a user session. Any process running in the user session can undo another operation. The undo state is held in the Explorer.exe process, and as long as that process is running, it can coordinate the undo functions.</para>
//        /// <para>If the source file parameter does not contain fully qualified path and file names, this flag is ignored.</para>
//        /// </summary>
//        FOF_ALLOWUNDO = 0x0040,

//        /// <summary>
//        /// Perform the operation only on files (not on folders) if a wildcard file name (.) is specified.
//        /// </summary>
//        FOF_FILESONLY = 0x0080,

//        /// <summary>
//        /// Respond with <b>Yes to All</b> for any dialog box that is displayed.
//        /// </summary>
//        FOF_NOCONFIRMATION = 0x0010,

//        /// <summary>
//        /// Do not confirm the creation of a new folder if the operation requires one to be created.
//        /// </summary>
//        FOF_NOCONFIRMMKDIR = 0x0200,

//        /// <summary>
//        /// Do not move connected items as a group. Only move the specified files.
//        /// </summary>
//        FOF_NO_CONNECTED_ELEMENTS = 0x2000,

//        /// <summary>
//        /// Do not copy the security attributes of the item.
//        /// </summary>
//        FOF_NOCOPYSECURITYATTRIBS = 0x0800,

//        /// <summary>
//        /// Do not display a message to the user if an error occurs. If this flag is set without <see cref="FOFX_EARLYFAILURE"/>, any error is treated as if the user had chosen <b>Ignore</b> or <b>Continue</b> in a dialog box. It halts the current action, sets a flag to indicate that an action was aborted, and proceeds with the rest of the operation.
//        /// </summary>
//        FOF_NOERRORUI = 0x0400,

//        /// <summary>
//        /// Only operate in the local folder. Do not operate recursively into subdirectories.
//        /// </summary>
//        FOF_NORECURSION = 0x1000,

//        /// <summary>
//        /// Give the item being operated on a new name in a move, copy, or rename operation if an item with the target name already exists.
//        /// </summary>
//        FOF_RENAMEONCOLLISION = 0x0008,

//        /// <summary>
//        /// Do not display a progress dialog box.
//        /// </summary>
//        FOF_SILENT = 0x0004,

//        /// <summary>
//        /// Send a warning if a file or folder is being destroyed during a delete operation rather than recycled. This flag partially overrides <see cref="FOF_NOCONFIRMATION"/>.
//        /// </summary>
//        FOF_WANTNUKEWARNING = 0x4000
//    }
//}
