/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Threading;
using WinCopies.IO;

namespace WinCopies.Tests
{
    [TestClass]
    public class CopyProcess
    {
        [TestMethod]
        public void TestPathCopy()
        {
            var paths = new List<IPathInfo>(2) { new WinCopies.IO.PathInfo("C:", true), new IO.PathInfo("D:", true) };

            var copyProcess = new WinCopies.GUI.IO.CopyProcess(new GUI.IO.PathCollection(string.Empty, paths), "E:", new GUI.IO.CopyProcessSimulationParameters() { CopyFileExAction = CopyFileEx, CreateDirectoryWAction = s => true, CreatingFileStreamSucceedsAction = (s, pathDirectoryType) => null, DestDriveReady = true, DestDriveTotalFreeSpace = long.MaxValue, DestPathExistsAction = s => false, DestPathRootExists = true, IsDuplicateAction = (sourcePath, destPath, callback) => !callback(), RenameOnDuplicateAction = s => null, SourceDriveReady = true, SourcePathRootExists = true, FileSystemEntryEnumeratorProcessSimulation = new FileSystemEntryEnumeratorProcessSimulation() { EnumerateFunc = new PathInfoFileSystemEntryEnumerator().GetEnumerable }, IOPathInfoToGUIIOPathInfoAction = pathInfo => new GUI.IO.PathInfo(pathInfo.Path, pathInfo.IsDirectory ? (Size?)null : (Size)fileSize++) });
        }

        private int sleep = 1;
        private int fileSize = 0;

        private bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(sleep == 10000 ? sleep /= 10 : sleep *= 10);

                lpProgressRoutine(10, i + 1, 0, 0, 0, CopyProgressCallbackReason.ChunkFinished, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            return true;
        }
    }
}
