using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public sealed class Enumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _enumeratorFunc;

        public Enumerable(Func<IEnumerator<T>> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public IEnumerator<T> GetEnumerator() => _enumeratorFunc();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class Enumerator<TSource, TDestination> : IEnumerator<TDestination>, Util.DotNetFix.IDisposable
    {
        public bool IsDisposed { get; private set; }

        private IEnumerator<TSource> _innerEnumerator;

        protected IEnumerator<TSource> InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

        private TDestination _current;

        public TDestination Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _current; protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object IEnumerator.Current => Current;

        public Enumerator(IEnumerable<TSource> enumerable) => _innerEnumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride()) return true;

            _current = default;

            return false;
        }

        protected abstract bool MoveNextOverride();

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            ResetOverride();
        }

        protected virtual void ResetOverride()
        {
            _current = default;

            InnerEnumerator.Reset();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                _innerEnumerator = null;

            IsDisposed = true;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(disposing: true);

                GC.SuppressFinalize(this);
            }
        }
    }

    // todo: moved to WinCopies.Util

    public class PausableBackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        public bool PausePending { get; private set; }

        private bool _workerSupportsPausing = false;

        public bool WorkerSupportsPausing { get => _workerSupportsPausing; set => _workerSupportsPausing = IsBusy ? throw new InvalidOperationException("The BackgroundWorker is running.") : value; }

        public void PauseAsync()
        {
            if (!_workerSupportsPausing)

                throw new InvalidOperationException("The BackgroundWorker does not support pausing.");

            if (IsBusy)

                PausePending = true;
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);

            PausePending = false;
        }
    }

    public static class Extensions// todo: replace by WinCopies.Util's implementation.
    {
        public static string Join(this IEnumerable<string> enumerable, in bool keepEmptyValues, params char[] join) => Join(enumerable, keepEmptyValues, new string(join));

        public static string Join(this IEnumerable<string> enumerable, in bool keepEmptyValues, in string join, StringBuilder stringBuilder = null)
        {
            IEnumerator<string> enumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

#if CS7
            if (stringBuilder == null)

                stringBuilder = new StringBuilder();
#else
            stringBuilder ??= new StringBuilder();
#endif

            try
            {
                void append() => _ = stringBuilder.Append(enumerator.Current);

                bool moveNext() => enumerator.MoveNext();

                if (moveNext())

                    append();

                while (moveNext() && (keepEmptyValues || enumerator.Current.Length > 0))
                {
                    _ = stringBuilder.Append(join);

                    append();
                }
            }
            finally
            {
                enumerator.Dispose();
            }

            return stringBuilder.ToString();
        }
    }

    //public class ReadOnlyObservableQueueCollection<T, U> : INotifyPropertyChanged where T : ObservableQueueCollection<U> // todo: remove when CopyProcessQueueCollection has been updated.
    //{
    //    private readonly T _innerCollection;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public ReadOnlyObservableQueueCollection(T innerCollection)
    //    {
    //        _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

    //        innerCollection.PropertyChanged += PropertyChanged;
    //    }

    //    public U Peek() => _innerCollection.Peek();
    //}

    //public class ReadOnlyCopyProcessQueueCollection // todo: remove when CopyProcessQueueCollection has been updated.
    //{
    //    private readonly ProcessQueueCollection _innerCollection;

    //    public Size Size => _innerCollection.Size;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public ReadOnlyCopyProcessQueueCollection(ProcessQueueCollection innerCollection)
    //    {
    //        _innerCollection = innerCollection ?? throw GetArgumentNullException(nameof(innerCollection));

    //        innerCollection.PropertyChanged += PropertyChanged;
    //    }

    //    public IPathInfo Peek() => _innerCollection.Peek();
    //}
}
