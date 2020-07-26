using SevenZip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Text;
using WinCopies.Util;

namespace WinCopies.GUI.IO.Process
{
    public abstract class ArchiveProcess<T> : PathInfoPathToPathProcess<T> where T : WinCopies.IO.IPathInfo
    {
        protected ArchiveProcess(in PathCollection<T> pathsToExtract, in string destPath) : base(pathsToExtract, destPath) { }

        protected virtual ProcessError OnPreProcess(DoWorkEventArgs e) => CheckIfDrivesAreReady(

#if DEBUG
                null
#endif
                );

        protected virtual bool OnFileProcessStarted(in int percentDone)
        {
            if (CheckIfPauseOrCancellationPending())

                return true;

            CurrentPath = _Paths.Peek();

            if (WorkerReportsProgress)

                ReportProgress(percentDone);

            return false;
        }

        protected virtual bool OnFileProcessCompleted()
        {
            _ = _Paths.Dequeue();

            return false;
        }

        protected abstract ProcessError OnProcess(DoWorkEventArgs e);

        protected override ProcessError OnProcessDoWork(DoWorkEventArgs e)
        {
            if (CheckIfPauseOrCancellationPending())

                return Error;

            ProcessError error = OnPreProcess(e);

            if (error != ProcessError.None)

                return error;

            try
            {
                return OnProcess(e);
            }

            catch (SecurityException)
            {
                return ProcessError.AccessDenied;
            }

            catch (Exception ex) when (ex.Is(false, typeof(System.IO.IOException), typeof(SevenZipException)))
            {
                return ProcessError.UnknownError;
            }
        }
    }
}
