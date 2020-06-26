using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Win32Native.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var copyProcess = new WinCopies.GUI.IO.CopyProcess(new GUI.IO.PathCollection("Computer", paths), "E:", new GUI.IO.CopyProcessSimulationParameters() { CopyFileExAction = CopyFileEx, CreateDirectoryWAction = s => true, CreatingFileStreamSucceedsAction = (s, pathDirectoryType) => null, DestDriveReady = true, DestDriveTotalFreeSpace = long.MaxValue, DestPathExistsAction = s => false, DestPathRootExists = true, IsDuplicateAction = (sourcePath, destPath, callback) => !callback(), RenameOnDuplicateAction = s => null, SourceDriveReady = true, SourcePathRootExists = true, FileSystemEntryEnumeratorProcessSimulation = new FileSystemEntryEnumeratorProcessSimulation() { EnumerateFunc = new PathInfoFileSystemEntryEnumerator().GetEnumerable }, IOPathInfoToGUIIOPathInfoAction = pathInfo => new GUI.IO.PathInfo(pathInfo.Path, pathInfo.IsDirectory ? (Size?)null : (Size)fileSize++) });
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
