using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WinCopies.Util;

namespace WinCopies.IO.FileProcesses
{

    /// <summary>
    /// A class that provides static methods to copy files. The doc is from the following Microsoft's doc page: https://docs.microsoft.com/en-us/windows/desktop/api/winbase/nc-winbase-lpprogress_routine.
    /// </summary>
    public static class Copy
    {

        //TODO: pbCancel ==> bool ?

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

        /// <summary>
        /// Flags that specify how the file is to be copied.
        /// </summary>
        [System.Flags]
        public enum CopyFileFlags : uint
        {

            /// <summary>
            /// The copy operation fails immediately if the target file already exists.
            /// </summary>
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,

            /// <summary>
            /// Progress of the copy is tracked in the target file in case the copy fails. The failed copy can be restarted at a later time by specifying the same values for lpExistingFileName and lpNewFileName as those used in the call that failed. This can significantly slow down the copy operation as the new file may be flushed multiple times during the copy operation.
            /// </summary>
            COPY_FILE_RESTARTABLE = 0x00000002,

            /// <summary>
            /// The file is copied and the original file is opened for write access.
            /// </summary>
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,

            /// <summary>
            /// An attempt to copy an encrypted file will succeed even if the destination copy cannot be encrypted.
            /// </summary>
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008,

            /// <summary>
            /// If the source file is a symbolic link, the destination file is also a symbolic link pointing to the same file that the source symbolic link is pointing to.
            ///
            /// Windows Server 2003 and Windows XP:  This value is not supported.
            /// </summary>
            COPY_FILE_COPY_SYMLINK = 0x00000800, //NT 6.0+

            /// <summary>
            /// The copy operation is performed using unbuffered I/O, bypassing system I/O cache resources. Recommended for very large file transfers.
            ///
            /// Windows Server 2003 and Windows XP:  This value is not supported.
            /// </summary>
            COPY_FILE_NO_BUFFERING = 0x00001000 //NT 6.0+
        }

        //TODO:

        //public static void Copy_Drives_Content(PathInfoBase path, string formated_Path, string formated_Copy_Path_Address, string dest_Path_3)
        //{



        //    System.IO.Directory.CreateDirectory(dest_Path_3);



        //} // end function ( void )

        //TODO:

        //public static void Copy_Folders(bool Is_A_File_Moving, PathInfoBase path, List<string> folder_Path, string formated_Path, string formated_Copy_Path_Address, string dest_Path_2, ref string dest_Path_3)
        //{
        //    //System.Windows.Forms.MessageBox.Show(dest_Path_3 );
        //    System.IO.Directory.CreateDirectory(dest_Path_3);

        //    if (Is_A_File_Moving)
        //    {

        //        folder_Path.Add(path.Path);          //      if (folder_Path.Count == 0) folder_Path.Add(path.Path);

        //        //        else
        //        //        {
        //        //      if (folder_Path[folder_Path.Count - 1].Length < path.Path.Length)
        //        //    {

        //        //    if (folder_Path[folder_Path.Count - 1] + path.Path.Substring(folder_Path[folder_Path.Count - 1].Length) == path.Path)

        //        //       folder_Path.Add(path.Path);

        //        //   else folder_Path.Clear();

        //        //   }

        //        //  else folder_Path.Clear();

        //        //   }

        //    }



        //} // end function ( void )

        //TODO: ? - erreur quand le répertoire de destination existe déjà (avec un fichier dedans) lors de la copie avec le répetoire de destination identique au répertoire de départ + '\' + qqch

        public static (Exceptions ex, int file, string fileName) Copy_Files(string oldFile, string newFile, bool Is_A_File_Moving, /* ref FileStream fs, ref FileStream fs2, */ bool overwrite, CopyProgressRoutine progressCallback)
        {

            //fs = null;

            //fs2 = null;

            // TODO :

            Directory.CreateDirectory(new FileInfo(newFile).DirectoryName);

            //TODO: ?

            // CopyProgressRoutine cpr = progressCallback;

            #region Comments
            // {

            // System.Windows.Forms.MessageBox.Show($"CopyFileEx : CopyProgressRoutine: {TotalFileSize.ToString()} {TotalBytesTransferred.ToString()}"

            // + $" { StreamSize.ToString()} {StreamBytesTransferred.ToString()} {dwStreamNumber.ToString()}"

            // + " " + dwCallbackReason.ToString());

            //Current_Copied_SizeProperty += fs.Position - Current_File_Copied_SizeProperty;



            //Current_File_Copied_SizeProperty = fs.Position;

            //return CopyProgressResult.PROGRESS_CONTINUE;

            // System.Windows.Forms.MessageBox.Show("");

            // };

            #endregion

            // Exceptions ex = Exceptions.None;

            // int error_Code = 0;

            int pbCancel = 0;

            // TODO : meilleure valeur ? --> no buffuring ?

            // TODO : rationnaliser des processus tels que celui-ci dans le processus général de la copie

            CopyFileFlags copyFileFlag = CopyFileFlags.COPY_FILE_NO_BUFFERING | CopyFileFlags.COPY_FILE_RESTARTABLE;

                    if (overwrite) copyFileFlag |= CopyFileFlags.COPY_FILE_OPEN_SOURCE_FOR_WRITE;

                    else copyFileFlag |= CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;

#if DEBUG

            Debug.WriteLine(Is_A_File_Moving);

            Debug.WriteLine(copyFileFlag.ToString());

#endif 

            //TODO:

            if (Is_A_File_Moving)
            {

                if (System.IO.Path.GetPathRoot(oldFile) != System.IO.Path.GetPathRoot(newFile))
                {

                    //fs = System.IO.File.OpenRead(newFile);

                    //fs2 = System.IO.File.OpenWrite(oldFile + @"\\" + dest_Path_2);

                    //fs.CopyTo(fs2);

                    (Exceptions ex, int file, string fileName) = CopyFile(oldFile, newFile, progressCallback, IntPtr.Zero, ref pbCancel, copyFileFlag);

                    // todo: to implement handlers for the other exceptions

                    if (ex == Exceptions.None)

                    {

                        try

                        {

                            File.Delete(oldFile);

                        }

                        catch (DirectoryNotFoundException)
                        {

                            ex = Exceptions.PathNotFound;

                        }

                        catch (PathTooLongException)

                        {

                            ex = Exceptions.FileNameTooLong;

                        }

                        catch (IOException)

                        {

                            ex = Exceptions.FileCheckedOut;

                        }

                        catch (UnauthorizedAccessException)

                        {

                            ex = Exceptions.AccessDenied;

                        }

                        // Assuming that when using this function, oldFile and newFile are both really files -- and not folders --, if ex has not the None value,
                        // that means that an exception has occurred. So, we need to try to remove the new file created, assuming this can cause a lot of waste time,
                        // but for logical reasons, we need to do not confuse the user. However, if the removal of the new file created and copied fail as well,
                        // we don't continue the process, also for logical reasons, because this process does not track this kind of exceptions.

                        if (ex != Exceptions.None)

                            try

                            {

                                File.Delete(newFile);

                            }

                            catch { }

                    }

                    return (ex, file, fileName);

                }


                else
                {

                    // todo : deux fonctions distinctes afin que celle-ci puisse retourner une valeur ? - retourner une valeur ou déclencher une exception lors d'un problème avec la copie ? - voir des apis et les utiliser aussi directement aussi pour un déplacement

                    //    System.Windows.Forms.MessageBox.Show("a1");

                    Exceptions ex = Exceptions.None;

                    int file = -1;

                    try
                    {

                        File.Move(oldFile, newFile);

                    }

                    catch (DirectoryNotFoundException)
                    {

                        ex = Exceptions.PathNotFound;

                        file = File.Exists(oldFile) ? 0 : 1;

                    }

                    catch (PathTooLongException)

                    {

                        ex = Exceptions.FileNameTooLong;

                        file = 1;

                    }

                    catch (IOException)

                    {

                        // This exception is more important than the other, so it is tested first.

                        if (!File.Exists(oldFile) && !Directory.Exists(oldFile))
                        {
                            ex = Exceptions.FileNotFound;

                            file = 0;
                        }

                        // This api does not tell us where the exception has appeared: on the old file or the new file, so we'll manage this with just an else:

                        else /* if (System.IO.File.Exists(newFile) || System.IO.Directory.Exists(newFile)) */
                        {
                            ex = Exceptions.FileAlreadyExists;

                            file = 1;

                        }

                    }

                    catch (UnauthorizedAccessException)

                    {

                        ex = Exceptions.AccessDenied;

                        file = File.Exists(oldFile) ? 0 : 1;

                    }

                    //   System.Windows.Forms.MessageBox.Show("b2");

                    return (ex, file, file == -1 ? null : (file == 0 ? oldFile : newFile));

                }

            }

            else
            {

                (Exceptions ex, int file, string fileName) = CopyFile(oldFile, newFile, progressCallback, IntPtr.Zero, ref pbCancel, copyFileFlag);

#if DEBUG

                if (ex == Exceptions.None)

                    Debug.WriteLine($"Fichier copié avec succès : {oldFile}");

                else

                    Debug.WriteLine($"Erreur lors de la copie du fichier : {oldFile}");

                // TODO : supprimer la référence à system.windows.forms

                Debug.WriteLine(copyFileFlag.ToString());

#endif 

                return (ex, file, fileName);

            }



        } // end function ( void )

        public static (Exceptions ex, int file, string fileName) CopyFile(string lpExistingFileName, string lpNewFileName,
   CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel,
   CopyFileFlags dwCopyFlags)

        {
            Win32ErrorCodes error_Code = 0;

            Exceptions ex = Exceptions.None;

            int file = -1;

            bool copy_result = CopyFileEx(lpExistingFileName, lpNewFileName, lpProgressRoutine, lpData, ref pbCancel, dwCopyFlags);

            if (!copy_result)
            {

                error_Code = (Win32ErrorCodes)Marshal.GetLastWin32Error();

#if DEBUG
                Debug.WriteLine("error_Code: " + error_Code.ToString());
#endif 

                if (error_Code != 0)

                    // todo: to implement handlers for the other error codes 

                    switch (error_Code)

                    {

                        case Win32ErrorCodes.ERROR_FILE_NOT_FOUND:

                            ex = Exceptions.FileNotFound;

                            break;

                        case Win32ErrorCodes.ERROR_PATH_NOT_FOUND:

                            ex = Exceptions.PathNotFound;

                            break;

                        case Win32ErrorCodes.ERROR_ACCESS_DENIED:

                            if (Directory.Exists(lpNewFileName))
                                ex = Exceptions.FileAlreadyExists;

                            else

                                ex = Exceptions.AccessDenied;

                            break;

                        case Win32ErrorCodes.ERROR_CURRENT_DIRECTORY:

                            ex = Exceptions.DirectoryCannotBeRemoved;

                            break;

                        case Win32ErrorCodes.ERROR_WRITE_PROTECT:

                            ex = Exceptions.WriteProtect;

                            break;

                        case Win32ErrorCodes.ERROR_INVALID_DRIVE:
                        case Win32ErrorCodes.ERROR_BAD_UNIT:
                        case Win32ErrorCodes.ERROR_SEEK:
                        case Win32ErrorCodes.ERROR_NOT_DOS_DISK:
                        case Win32ErrorCodes.ERROR_SECTOR_NOT_FOUND:
                        case Win32ErrorCodes.ERROR_WRITE_FAULT:
                        case Win32ErrorCodes.ERROR_READ_FAULT:
                        case Win32ErrorCodes.ERROR_WRONG_DISK:

                            ex = Exceptions.ExceptionOnDeviceUnit;

                            break;

                        case Win32ErrorCodes.ERROR_NOT_READY:

                            ex = Exceptions.DiskNotReady;

                            break;


                        case WinCopies.Util.Win32ErrorCodes.ERROR_ALREADY_EXISTS:
                        case Win32ErrorCodes.ERROR_FILE_EXISTS:

                            ex = Exceptions.FileAlreadyExists;

                            break;

                        case Win32ErrorCodes.ERROR_BUFFER_OVERFLOW:
                        case Win32ErrorCodes.ERROR_FILENAME_EXCED_RANGE:

                            ex = Exceptions.FileNameTooLong;

                            break;

                        case Win32ErrorCodes.ERROR_DISK_FULL:
                        case Win32ErrorCodes.ERROR_HANDLE_DISK_FULL:

                            ex = Exceptions.DiskFull;

                            break;

                        case Win32ErrorCodes.ERROR_INVALID_NAME:
                        case Win32ErrorCodes.ERROR_DIRECTORY:

                            ex = Exceptions.InvalidName;

                            break;

                        case Win32ErrorCodes.ERROR_FILE_CHECKED_OUT:
                        case Win32ErrorCodes.ERROR_SHARING_VIOLATION:
                        case Win32ErrorCodes.ERROR_LOCK_VIOLATION:

                            ex = Exceptions.FileCheckedOut;

                            break;

                        case Win32ErrorCodes.ERROR_FILE_TOO_LARGE:

                            ex = Exceptions.FileTooLarge;

                            break;

                        case Win32ErrorCodes.ERROR_DISK_TOO_FRAGMENTED:

                            ex = Exceptions.DiskTooFragmented;

                            break;

                        case Win32ErrorCodes.ERROR_DELETE_PENDING:

                            ex = Exceptions.DeletePending;

                            break;

                        case Win32ErrorCodes.ERROR_NOT_ALLOWED_ON_SYSTEM_FILE:

                            ex = Exceptions.NotAllowedOnSystemFile;

                            break;

                        case Win32ErrorCodes.ERROR_REQUEST_ABORTED:

                            ex = Exceptions.RequestCancelled;

                            break;

                        default:

                            ex = Exceptions.Unknown;

                            break;

                    }

                bool handleValid = false;

                try
                {

                    UnmanagedFileLoader loader = new UnmanagedFileLoader(lpExistingFileName);

                    handleValid = loader.Handle != null;

                    loader.Handle.Close();

                    loader.Handle.Dispose();

                }

#if DEBUG 

                catch (Exception _ex)

                {

                    Debug.WriteLine(string.Format("Unable to create file handle : {0} ({1}). Exception message : {2}", lpExistingFileName, _ex.GetType().ToString(), _ex.Message));

                }

#else

                finally 

                {

#endif 

                file = handleValid ? 1 : 0;

#if !DEBUG

                }

#endif 

            }

            return (ex, file, file == -1 ? null : (file == 0 ? lpExistingFileName : lpNewFileName));

        }

    } // end class

} // end namespace