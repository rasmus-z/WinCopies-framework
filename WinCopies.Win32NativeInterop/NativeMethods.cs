using System;
using System.Runtime.InteropServices;

namespace WinCopies.Win32NativeInterop
{
    public static class NativeMethods
    {

        // [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        //public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

        //TODO: pbCancel ==> bool ?

        // todo: to move in the win32interop dll and check comments

        /// <summary>
        /// <para>Copies an existing file to a new file, notifying the application of its progress through a callback function.</para>
        ///
        /// <para>To perform this operation as a transacted operation, use the CopyFileTransacted function.</para>
        /// </summary>
        /// <param name="lpExistingFileName"><para>The name of an existing file.</para>
        ///
        /// <para>In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\?" to the path.For more information, see Naming a File.</para>
        /// <para>Tip Starting in Windows 10, version 1607, for the unicode version of this function (CopyFileExW), you can opt-in to remove the MAX_PATH character limitation without prepending "\\?\". See the "Maximum Path Limitation" section of Naming Files, Paths, and Namespaces for details.</para>
        ///
        ///<para>If lpExistingFileName does not exist, the <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/> function fails, and the <see cref="Marshal. GetLastWin32Error"/> function returns <see cref="NativeWin32FilesProcessesErrorCodes.ERROR_FILE_NOT_FOUND"/>.</para></param>
        /// <param name="lpNewFileName"><para>The name of the new file.</para>
        ///
        /// <para>In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\?" to the path.For more information, see Naming a File.</para>
        ///
        /// <para>Tip Starting inWindows 10, version 1607, for the unicode version of this function (CopyFileExW), you can opt-in to remove the MAX_PATH character limitation without prepending "\\?\". See the "Maximum Path Limitation" section of Naming Files, Paths, and Namespaces for details.</para></param>
        /// <param name="lpProgressRoutine">The address of a callback function of type <see cref="CopyProgressRoutine"/> that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>. For more information on the progress callback function, see the <see cref="CopyProgressRoutine"/> function.</param>
        /// <param name="lpData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
        /// <param name="pbCancel">If this flag is set to <see langword="true"/> during the copy operation, the operation is canceled. Otherwise, the copy operation will continue to completion.</param>
        /// <param name="dwCopyFlags">Flags that specify how the file is to be copied. This parameter can be a combination of the <see cref="CopyFileFlags"/> enum.</param>
        /// <returns><para>If the function succeeds, the return value is nonzero.</para>
        ///
        /// <para>If the function fails, the return value is zero.To get extended error information call <see cref="Marshal.GetLastWin32Error"/>.</para>
        ///
        /// <para>If lpProgressRoutine returns <see cref="CopyProgressResult.PROGRESS_CANCEL"/> due to the user canceling the operation, <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/> will return zero and <see cref="Marshal.GetLastWin32Error"/> will return <see cref="WinCopies.Util.Win32ErrorCodes.ERROR_REQUEST_ABORTED"/>. In this case, the partially copied destination file is deleted.</para>
        ///
        /// <para>If lpProgressRoutine returns <see cref="CopyProgressResult.PROGRESS_STOP"/> due to the user stopping the operation, <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/> will return zero and <see cref="Marshal.GetLastWin32Error"/> will return <see cref="WinCopies.Util.Win32ErrorCodes.ERROR_REQUEST_ABORTED"/>. In this case, the partially copied destination file is left intact.</para></returns>
        /// <remarks><para>This function preserves extended attributes, OLE structured storage, NTFS file system alternate data streams, security resource attributes, and file attributes.</para>
        ///
        /// <para>Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  Security resource attributes(ATTRIBUTE_SECURITY_INFORMATION) for the existing file are not copied to the new file until Windows 8 and Windows Server 2012.</para>
        ///
        /// <para>The security resource properties(ATTRIBUTE_SECURITY_INFORMATION) for the existing file are copied to the new file.</para>
        ///
        /// <para>Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  Security resource properties for the existing file are not copied to the new file until Windows 8 and Windows Server 2012.</para>
        ///
        /// <para>This function fails with <see cref="NativeWin32FilesProcessesErrorCodes.ERROR_ACCESS_DENIED"/> if the destination file already exists and has the <see cref="System.IO.FileAttributes.Hidden"/> or <see cref="System.IO.FileAttributes.ReadOnly"/> attribute set.</para>
        ///
        /// <para>When encrypted files are copied using <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/>, the function attempts to encrypt the destination file with the keys used in the encryption of the source file.If this cannot be done, this function attempts to encrypt the destination file with default keys.If both of these methods cannot be done, <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/> fails with an <see cref="WinCopies.Util.Win32ErrorCodes.ERROR_ENCRYPTION_FAILED"/> error code. If you want <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/> to complete the copy operation even if the destination file cannot be encrypted, include the <see cref="CopyFileFlags.COPY_FILE_ALLOW_DECRYPTED_DESTINATION"/> as the value of the dwCopyFlags parameter in your call to <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/>.</para>
        ///
        /// If <see cref="CopyFileFlags.COPY_FILE_COPY_SYMLINK"/> is specified, the following rules apply:
        ///
        /// If the source file is a symbolic link, the symbolic link is copied, not the target file.
        /// If the source file is not a symbolic link, there is no change in behavior.
        /// If the destination file is an existing symbolic link, the symbolic link is overwritten, not the target file.
        /// If <see cref="CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS"/> is also specified, and the destination file is an existing symbolic link, the operation fails in all cases.
        ///
        /// If <see cref="CopyFileFlags.COPY_FILE_COPY_SYMLINK"/> is not specified, the following rules apply:
        ///
        /// If <see cref="CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS"/> is also specified, and the destination file is an existing symbolic link, the operation fails only if the target of the symbolic link exists.
        /// If <see cref="CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS"/> is not specified, there is no change in behavior.
        ///
        /// Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  If you are writing an application that is optimizing file copy operations across a LAN, consider using the TransmitFile function from Windows Sockets(Winsock). TransmitFile supports high-performance network transfers and provides a simple interface to send the contents of a file to a remote computer.To use TransmitFile, you must write a Winsock client application that sends the file from the source computer as well as a Winsock server application that uses other Winsock functions to receive the file on the remote computer.
        ///
        /// In Windows 8 and Windows Server 2012, this function is supported by the following technologies.
        /// Technology Supported
        /// Server Message Block (SMB) 3.0 protocol Yes
        /// SMB 3.0 Transparent Failover (TFO) Yes
        /// SMB 3.0 with Scale-out File Shares (SO) Yes
        /// Cluster Shared Volume File System (CsvFS) Yes
        /// Resilient File System (ReFS) Yes </remarks>

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
   CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel,
   CopyFileFlags dwCopyFlags);

        /// <summary>
        /// An application-defined callback function used with the <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/>, MoveFileTransacted, and MoveFileWithProgress functions. It is called when a portion of a copy or move operation is completed. The LPPROGRESS_ROUTINE type defines a pointer to this callback function. CopyProgressRoutine is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="TotalFileSize">The total size of the file, in bytes.</param>
        /// <param name="TotalBytesTransferred">The total number of bytes transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="StreamSize">The total size of the current file stream, in bytes.</param>
        /// <param name="StreamBytesTransferred">The total number of bytes in the current stream that have been transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="dwStreamNumber">A handle to the current stream. The first time CopyProgressRoutine is called, the stream number is 1.</param>
        /// <param name="dwCallbackReason">The reason that <see cref="CopyProgressRoutine"/> was called. This parameter can be one of the values of the <see cref="CopyProgressCallbackReason"/> enum.</param>
        /// <param name="hSourceFile">A handle to the source file.</param>
        /// <param name="hDestinationFile">A handle to the destination file.</param>
        /// <param name="lpData">Argument passed to CopyProgressRoutine by <see cref="CopyFileEx(string, string, CopyProgressRoutine, IntPtr, ref int, CopyFileFlags)"/>, MoveFileTransacted, or MoveFileWithProgress.</param>
        /// <returns>The CopyProgressRoutine function should return one of the values of the <see cref="CopyProgressResult"/> enum.</returns>
        /// <remarks>An application can use this information to display a progress bar that shows the total number of bytes copied as a percent of the total file size.</remarks>
        public delegate CopyProgressResult CopyProgressRoutine(
            long TotalFileSize,
            long TotalBytesTransferred,
            long StreamSize,
            long StreamBytesTransferred,
            uint dwStreamNumber,
            CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

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
