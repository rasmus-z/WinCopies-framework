using System;
using System.ComponentModel;
using System.Threading;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{

    ///// <para>FR: Représente un BackgroundWorker qui s'exécute par défaut dans un thread MTA et qui arrête automatiquement l'exécution en arrière-plan lors d'un rapport du progrès.</para>
    /// <summary>
    /// Represents a BackgroundWorker that runs in a MTA thread by default and automatically stops on background when reports progress.
    /// </summary>
    public class BackgroundWorker : Component, IBackgroundWorker
    {

        ///// <para>FR: Cet evènement se produit lorsque le thread d'arrière plan démarre. Placez votre code de traitement ici.</para>
        ///// <para>Gestionnaire d'événement exécuté dans le thread d'arrière plan.</para>
        /// <summary>
        /// <para>This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        ///// <para>FR: Cet évènement se produit lorsque le thread d'arrière plan notifie de la progression.</para>
        ///// <para>Gestionnaire d'événement exécuté dans le thread principal.</para>
        /// <summary>
        /// <para>This event is called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        ///// <para>FR: Cet évènement se produit lorsque le thread d'arrière plan est terminé.</para>
        ///// <para>Gestionnaire d'événement exécuté dans le thread d'arrière plan.</para>
        /// <summary>
        /// <para>This event is called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;



        private Thread _Thread = null;

        private readonly ApartmentState _ApartmentState = ApartmentState.MTA;

        private readonly SynchronizationContext _SyncContext;

        private readonly ManualResetEvent _event = new ManualResetEvent(true);



        ///// <para>FR: Obtient une valeur indiquant si le traitement a été annulé.</para>
        /// <summary>
        /// Gets a value that indicates whether the working has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; } = false;

        ///// <para>FR: Obtient une valeur indiquant si le thread doit essayer de se terminer avant la fin de ses tâches en arrière-plan.</para>
        /// <summary>
        /// Gets a value that indicates whether the thread must try to cancel before finished the background tasks.
        /// </summary>
        public bool CancellationPending { get; private set; } = false;

        ///// <para>FR: Obtient une valeur indiquant si le thread est occupé.</para>
        /// <summary>
        /// Gets a value that indicates whether the thread is busy.
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        private readonly bool workerReportsProgress = false;

        ///// <para>FR: Obtient ou définit une valeur indiquant si le thread peut notifier de l'avancement.</para>
        /// <summary>
        /// Gets or sets a value that indicates whether the thread can notify of the progress.
        /// </summary>
        public bool WorkerReportsProgress
        {

            get => workerReportsProgress;

            set => this.SetBackgroundWorkerProperty(nameof(WorkerReportsProgress), nameof(workerReportsProgress), value, typeof(BackgroundWorker), true);

        }

        private readonly bool workerSupportsCancellation = false;

        ///// <para>FR: Obtient ou définit une valeur indiquant si le thread supporte l'annulation.</para>
        /// <summary>
        /// Gets or sets a value that indicates whether the thread supports the cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation

        {

            get => workerSupportsCancellation;

            set => this.SetBackgroundWorkerProperty(nameof(WorkerReportsProgress), nameof(workerSupportsCancellation), value, typeof(BackgroundWorker), true);

        }

        ///// <para>FR: Obtient le progrès actuel du traitement en pourcents.</para>
        /// <summary>
        /// Gets the current progress of the working in percent.
        /// </summary>
        public int Progress { get; private set; } = 0;

        ///// <para>FR: Obtient l'<see cref="System.Threading. ApartmentState"/> de ce thread.</para>
        /// <summary>
        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
        /// </summary>
        public ApartmentState ApartmentState
        {

            get => _ApartmentState;

            set
            {

                if (_ApartmentState == value)

                    return;

                else if (value == ApartmentState.Unknown)

                    // todo:

                    //La valeur {0} ne peut pas être sélectionnée pour un ApartmentState d'un {1}.

                    throw new ArgumentException(string.Format("The {0} value is not valid for the {1} class.", nameof(ApartmentState.Unknown), nameof(BackgroundWorker)));

                _ = this.SetBackgroundWorkerProperty(nameof(ApartmentState), nameof(_ApartmentState), value, typeof(BackgroundWorker), true);

            }

        }



        ///// <para>FR: Construit un <see cref="BackgroundWorker"/>.</para>
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker"/> class.
        /// </summary>
        public BackgroundWorker() => _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        // {

        // Reset()

        // _CanCancel = True

        // _ReportProgress = True

        // }

        ///// <para>FR: Construit un <see cref="BackgroundWorker"/> avec un <see cref="System.Threading.ApartmentState"/> donné.</para>
        ///// <para>FR: <see cref="System.Threading.ApartmentState"/> dans lequel initialiser le thread.</para>
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker"/> class with a given <see cref="System.Threading.ApartmentState"/>.
        /// </summary>
        /// <param name="apartmentState">
        /// The <see cref="System.Threading.ApartmentState"/> in which to initialize the thread.
        /// </param>
        public BackgroundWorker(ApartmentState apartmentState)
        {
            _ApartmentState = apartmentState;

            // Reset()

            // _CanCancel = True

            // _ReportProgress = True

            _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }





        ///// <para>FR: Ré-initialise les variables locales.</para>
        /// <summary>
        /// Re-initializes the local variables.
        /// </summary>
        private void Reset(bool isCancelled)
        {
            CancellationPending = false;

            IsCancelled = isCancelled;

            if (!isCancelled)

                Progress = 0;

            _Thread = null;

            IsBusy = false;
        }

        // DoWorkEventArgs e = null;

        ///// <para>FR: Démarre le traitement.</para>
        /// <summary>
        /// Starts the working.
        /// </summary>
        public void RunWorkerAsync() => RunWorkerAsync(null);

        ///// <para>FR: Démarre le traitement avec un argument personnalisé.</para>
        ///// <para>FR: Argument passé au traitement.</para>
        /// <summary>
        /// Starts the working with a custom parameter.
        /// </summary>
        /// <param name="argument">
        /// Argument given for the working.
        /// </param>
        public void RunWorkerAsync(object argument)
        {

            if (IsBusy)

                // todo:

                throw new InvalidOperationException(BackgroundWorkerIsBusy);

            Reset(false);

            IsBusy = true;



            //Exception error = null;

            _Thread = new Thread(() => ThreadStart(new DoWorkEventArgs(argument)));

            _Thread.IsBackground = true;

            _Thread.SetApartmentState(_ApartmentState);

            _Thread.Start();

        }

        ///// <para>FR: Point d'entré du thread.</para>
        /// <summary>
        /// Entry point of the thread.
        /// </summary>
        // /// <param name="argument">Argument du thread.</param>
        private void ThreadStart(DoWorkEventArgs e)
        {



            // Dim cancelled As Boolean = False

            Exception error = null;



            try
            {

                DoWork?.Invoke(this, e);

            }

            // _IsRunning = False

            // _IsCancelling = False

            catch (Exception ex)
            {

                error = ex;

            }

            bool isCancelled = IsCancelled || CancellationPending || e.Cancel;

            Reset(isCancelled);

            _SyncContext.Send(ThreadCompleted, new ValueTuple<object, Exception, bool>(e.Result, error, isCancelled));

        }

        ///// <para>FR: Cette méthode est appelée lorsque le thread est terminé.</para>
        /// <summary>
        /// The method that is called when the thread has finished.
        /// </summary>
        // /// <param name="result">Objet résultat du traitement.</param>
        // /// <param name="error">Erreur éventuelle. <c>null</c> si pas d'erreur.</param>
        // /// <param name="cancel">Indique si le thread à été annulé ou non.</param>
        private void ThreadCompleted(object args)
        {

            (object result, Exception ex, bool isCancelled) = (ValueTuple<object, Exception, bool>)args;

            var e = new RunWorkerCompletedEventArgs(result, ex, isCancelled);

            // IsBusy = false;

            // CancellationPending = false;

            // IsCancelled = r.Item3;    

            RunWorkerCompleted?.Invoke(this, e);

        }

        ///// <para>FR: Annule le traitement en cours de manière asynchrone.</para>
        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        public void CancelAsync() => Cancel(false);

        ///// <para>FR: Annule le traitement en cours.</para>
        /// <summary>
        /// Cancels the working.
        /// </summary>
        public void Cancel() => Cancel(true);

        private void Cancel(bool abort)
        {

            if (!WorkerSupportsCancellation)

                // todo:

                //Ce traitement ne supporte pas l'annulation.

                throw new InvalidOperationException("This BackgroundWorker does not support cancellation.");



            if (_Thread == null || !IsBusy)

                return;



            if (abort)

                _Thread.Abort();

            //ThreadCompleted(new ValueTuple<object, Exception, bool>(null, null, true));

            else

                CancellationPending = true;

        }

        ///// <para>FR: Délégué pour rapporter de la progression.</para>
        ///// <para>FR: Argument de l'évènement.</para>
        /// <summary>
        /// Delegate for progress reportting.
        /// </summary>
        /// <param name="args">
        /// Event argument.
        /// </param>
        private void OnProgressChanged(object args) => ProgressChanged?.Invoke(this, args as ProgressChangedEventArgs);

        ///// <para>FR: Notifie de la progession.</para>
        ///// <para>FR: Pourcentage de progression.</para>
        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        public void ReportProgress(int percentProgress) => ReportProgress(percentProgress, null);

        ///// <para>FR: Notifie de la pogression.</para>
        ///// <para>FR: Pourcentage de progression.</para>
        ///// <para>FR: Objet utilisateur.</para>
        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        public void ReportProgress(int percentProgress, object userState)
        {

            if (!WorkerReportsProgress)

                //Ce BackgroundWorker ne permet pas de notifier de la progression.

                throw new InvalidOperationException("This BackgroundWorker does not support progression notification.");

            Progress = percentProgress;

            var e = new ProgressChangedEventArgs(Progress, userState);

            _SyncContext.Send(OnProgressChanged, e);

        }

        /// <summary>
        /// Suspends the current thread.
        /// </summary>
        public void Suspend()

        {

            _ = _event.Reset();

            _ = _event.WaitOne();

        }

        /// <summary>
        /// Resumes the current thread.
        /// </summary>
        public void Resume() =>

            _event.Set();

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="BackgroundWorker"/> is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Releases resources used by the <see cref="BackgroundWorker"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public new void Dispose() => base. Dispose();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="BackgroundWorker"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        protected override void Dispose(bool disposing)

        {

            if (IsBusy)

                //    throw new InvalidOperationException(BackgroundWorkerIsBusy);

                Cancel(true);

            base.Dispose(disposing);

            if (IsDisposed)

                return;

            _event.Dispose();

            IsDisposed = true;

        }

    }

}
