using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{

    /// <summary>
    /// <para>FR: Représente un BackgroundWorker qui s'exécute par défaut dans un thread MTA et qui arrête automatiquement l'exécution en arrière-plan lors d'un rapport du progrès.</para>
    /// <para>EN: Represents a BackgroundWorker that runs in a MTA thread by default and automatically stops on background when reports progress.</para>
    /// </summary>
    public class BackgroundWorker : Component
    {

        /// <summary>
        /// <para>FR: Cet evènement se produit lorsque le thread d'arrière plan démarre. Placez votre code de traitement ici.</para>
        /// <para>Gestionnaire d'événement exécuté dans le thread d'arrière plan.</para>
        /// <para>EN: This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        /// <summary>
        /// <para>FR: Cet évènement se produit lorsque le thread d'arrière plan notifie de la progression.</para>
        /// <para>Gestionnaire d'événement exécuté dans le thread principal.</para>
        /// <para>EN: This event is called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// <para>FR: Cet évènement se produit lorsque le thread d'arrière plan est terminé.</para>
        /// <para>Gestionnaire d'événement exécuté dans le thread d'arrière plan.</para>
        /// <para>EN: This event is called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;



        private Thread _Thread = null;

        private ApartmentState _ApartmentState = ApartmentState.MTA;

        private SynchronizationContext _SyncContext;

        private ManualResetEvent _event = new ManualResetEvent(true);



        /// <summary>
        /// <para>FR: Obtient une valeur indiquant si le traitement a été annulé.</para>
        /// <para>EN: Gets a value that indicates if the working is cancelled.</para>
        /// </summary>
        public bool IsCancelled { get; private set; } = false;

        /// <summary>
        /// <para>FR: Obtient une valeur indiquant si le thread doit essayer de se terminer avant la fin de ses tâches en arrière-plan.</para>
        /// <para>EN: Gets a value that indicates if the thread must try to cancel before finished the background tasks.</para>
        /// </summary>
        public bool CancellationPending { get; private set; } = false;

        /// <summary>
        /// <para>FR: Obtient une valeur indiquant si le thread est occupé.</para>
        /// <para>EN: Gets a value that indicates if the thread is busy.</para>
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        private bool workerReportsProgress = false;

        /// <summary>
        /// <para>FR: Obtient ou définit une valeur indiquant si le thread peut notifier de l'avancement.</para>
        /// <para>EN: Gets or sets a value that indicates if the thread can notify of the progress.</para>
        /// </summary>
        public bool WorkerReportsProgress
        {

            get => workerReportsProgress;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException(BackgroundWorkerIsBusy);

                else

                    workerReportsProgress = value;

            }

        }

        private bool workerSupportsCancellation = false;

        /// <summary>
        /// <para>FR: Obtient ou définit une valeur indiquant si le thread supporte l'annulation.</para>
        /// <para>EN: Gets or sets a value that indicates if the thread supports the cancellation.</para>
        /// </summary>
        public bool WorkerSupportsCancellation

        {

            get => workerSupportsCancellation;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException(BackgroundWorkerIsBusy);

                else

                    workerSupportsCancellation = true;

            }

        }

        /// <summary>
        /// <para>FR: Obtient le progrès actuel du traitement en pourcents.</para>
        /// <para>EN: Gets the current progress of the working in percent.</para>
        /// </summary>
        public int Progress { get; private set; } = 0;

        /// <summary>
        /// <para>FR: Obtient l'<see cref="System.Threading. ApartmentState"/> de ce thread.</para>
        /// <para>EN: Gets the <see cref="System.Threading.ApartmentState"/> of this thread.</para>
        /// </summary>
        public ApartmentState ApartmentState
        {

            get => _ApartmentState;

            set
            {

                if (IsBusy)

                    // todo:

                    throw new InvalidOperationException("Le thread est déjà en cours d'exécution ; son ApartmentState ne peut pas encore changer.");

                else if (_ApartmentState == value)

                    return;

                // todo:

                else if (value == ApartmentState.Unknown)

                    throw new ArgumentException(string.Format("La valeur {0} ne peut pas être sélectionnée pour un ApartmentState d'un {1}.", ApartmentState.Unknown.GetType().FullName, typeof(BackgroundWorker).FullName));

                else

                    _ApartmentState = value;

            }

        }



        /// <summary>
        /// <para>FR: Construit un <see cref="BackgroundWorker"/>.</para>
        /// <para>EN: Initializes a new instance of the <see cref="BackgroundWorker"/> class.</para>
        /// </summary>
        public BackgroundWorker() => _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        // {

        // Reset()

        // _CanCancel = True

        // _ReportProgress = True

        // }

        /// <summary>
        /// <para>FR: Construit un <see cref="BackgroundWorker"/> avec un <see cref="System.Threading.ApartmentState"/> donné.</para>
        /// <para>EN: Initializes a new instance of the <see cref="BackgroundWorker"/> class with a given <see cref="System.Threading.ApartmentState"/>.</para>
        /// </summary>
        /// <param name="apartmentState">
        /// <para>FR: <see cref="System.Threading.ApartmentState"/> dans lequel initialiser le thread.</para>
        /// <para>EN: <see cref="System.Threading.ApartmentState"/> in which initialize the thread.</para>
        /// </param>
        public BackgroundWorker(ApartmentState apartmentState)
        {
            _ApartmentState = apartmentState;

            // Reset()

            // _CanCancel = True

            // _ReportProgress = True

            _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }





        /// <summary>
        /// <para>FR: Ré-initialise les variables locales.</para>
        /// <para>EN: Re-initializes the local variables.</para>
        /// </summary>
        private void Reset(bool isCancelled)
        {
            CancellationPending = false;

            IsCancelled = isCancelled;

            IsBusy = false;

            if (!isCancelled)

                Progress = 0;

            _Thread = null;
        }

        // DoWorkEventArgs e = null;

        /// <summary>
        /// <para>FR: Démarre le traitement.</para>
        /// <para>EN: Starts the working.</para>
        /// </summary>
        public void RunWorkerAsync() => RunWorkerAsync(null);

        /// <summary>
        /// <para>FR: Démarre le traitement avec un argument personnalisé.</para>
        /// <para>EN: Starts the working with a custom parameter.</para>
        /// </summary>
        /// <param name="argument">
        /// <para>FR: Argument passé au traitement.</para>
        /// <para>EN: Argument given for the working.</para>
        /// </param>
        public void RunWorkerAsync(object argument)
        {

            if (IsBusy)

                // todo:

                throw new InvalidOperationException("Le BackgroundWorker est déjà en cours d'exécution.");

            Reset(false);

            IsBusy = true;



            //Exception error = null;

            var e = new DoWorkEventArgs(argument);

            _Thread = new Thread(() => ThreadStart(e));

            _Thread.IsBackground = true;

            _Thread.SetApartmentState(_ApartmentState);

            _Thread.Start();

        }

        /// <summary>
        /// <para>FR: Point d'entré du thread.</para>
        /// <para>EN: Entry point of the thread.</para>
        /// </summary>
        // /// <param name="argument">Argument du thread.</param>
        private void ThreadStart(DoWorkEventArgs e)
        {

#if DEBUG
            Console.WriteLine("Début");
#endif



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

#if DEBUG

                Debug.WriteLine(ex.GetType() + ": " + ex.Message);

#endif

            }

            _SyncContext.Send(ThreadCompleted, new ValueTuple<Object, Exception, bool>(e.Result, error, IsCancelled || CancellationPending || e.Cancel));

        }

        /// <summary>
        /// <para>FR: Cette méthode est appelée lorsque le thread est terminé.</para>
        /// <para>EN: This method is called when the thread has finished.</para>
        /// </summary>
        // /// <param name="result">Objet résultat du traitement.</param>
        // /// <param name="error">Erreur éventuelle. <c>null</c> si pas d'erreur.</param>
        // /// <param name="cancel">Indique si le thread à été annulé ou non.</param>
        private void ThreadCompleted(object args)
        {

            (object result, Exception ex, bool isCancelled) = (ValueTuple<object, Exception, bool>)args;

            RunWorkerCompletedEventArgs e = new RunWorkerCompletedEventArgs(result, ex, isCancelled);

#if DEBUG

            Debug.WriteLine("Fin");

#endif

            // IsBusy = false;

            // CancellationPending = false;

            // IsCancelled = r.Item3;    

            Reset(isCancelled);

            RunWorkerCompleted?.Invoke(this, e);

        }

        /// <summary>
        /// <para>FR: Annule le traitement en cours de manière asynchrone.</para>
        /// <para>EN: Cancels the working asynchronously.</para>
        /// </summary>
        public void CancelAsync() => Cancel(false);

        /// <summary>
        /// <para>FR: Annule le traitement en cours.</para>
        /// <para>EN: Cancels the working.</para>
        /// </summary>
        public void Cancel() => Cancel(true);



        private void Cancel(bool abort)
        {

            if (!WorkerSupportsCancellation)

                throw new InvalidOperationException("Ce traitement ne supporte pas l'annulation.");



            if (_Thread == null || !IsBusy)

                return;



            CancellationPending = true;

            if (abort)
            {

                _Thread.Abort();

                ThreadCompleted(new ValueTuple<object, Exception, bool>(null, null, true));

            }

        }

        /// <summary>
        /// <para>FR: Délégué pour rapporter de la progression.</para>
        /// <para>EN: Delegate for progress reportting.</para>
        /// </summary>
        /// <param name="args">
        /// <para>FR: Argument de l'évènement.</para>
        /// <para>EN: Event argument.</para>
        /// </param>
        private void OnProgressChanged(object args) => ProgressChanged?.Invoke(this, args as ProgressChangedEventArgs);

        /// <summary>
        /// <para>FR: Notifie de la progession.</para>
        /// <para>EN: Notifies of the progress.</para>
        /// </summary>
        /// <param name="percentProgress">
        /// <para>FR: Pourcentage de progression.</para>
        /// <para>EN: Progress percentage.</para>
        /// </param>
        public void ReportProgress(int percentProgress) => ReportProgress(percentProgress, null);

        /// <summary>
        /// <para>FR: Notifie de la pogression.</para>
        /// <para>EN: Notifies of the progress.</para>
        /// </summary>
        /// <param name="percentProgress">
        /// <para>FR: Pourcentage de progression.</para>
        /// <para>EN: Progress percentage.</para>
        /// </param>
        /// <param name="userState">
        /// <para>FR: Objet utilisateur.</para>
        /// <para>EN: User object.</para>
        /// </param>
        public void ReportProgress(int percentProgress, object userState)
        {

            if (!WorkerReportsProgress)

                throw new InvalidOperationException("Ce BackgroundWorker ne permet pas de notifier de la progression.");

            Progress = percentProgress;

            ProgressChangedEventArgs e = new ProgressChangedEventArgs(Progress, userState);

            _SyncContext.Send(OnProgressChanged, e);

        }

        public void Suspend()

        {

            // to suspend thread.
            _event.Reset();

            _event.WaitOne();

        }

        public void Resume() =>

            //to resume thread
            _event.Set();

        protected override void Dispose(bool disposing)

        {

            base.Dispose(disposing);

            _event.Dispose();

        }

    }

}
