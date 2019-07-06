﻿using System.ComponentModel;
using System.Threading;

namespace WinCopies.Util
{
    public interface IBackgroundWorker : IComponent
    {

        /// <summary>
        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
        /// </summary>
        ApartmentState ApartmentState { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the thread must try to cancel before finished the background tasks.
        /// </summary>
        bool CancellationPending { get; }

        /// <summary>
        /// Gets a value that indicates whether the thread is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets a value that indicates whether the working is cancelled.
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// Gets the current progress of the working in percent.
        /// </summary>
        int Progress { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether the thread can notify of the progress.
        /// </summary>
        bool WorkerReportsProgress { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the thread supports cancellation.
        /// </summary>
        bool WorkerSupportsCancellation { get; set; }

        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        void CancelAsync();

        /// <summary>
        /// Cancels the working.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        void ReportProgress(int percentProgress);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        void ReportProgress(int percentProgress, object userState);

        /// <summary>
        /// Suspends the current thread.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resumes the current thread.
        /// </summary>
        void Resume();

        /// <summary>
        /// <para>Called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        event DoWorkEventHandler DoWork;

        /// <summary>
        /// <para>Called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// <para>Called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        event RunWorkerCompletedEventHandler RunWorkerCompleted;

    }
}
