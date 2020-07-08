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

#if !WinCopies2

using System;
using System.Collections;
using static WinCopies.Util.Util;

namespace WinCopies.Collections
{
    public abstract class Enumerator : IEnumerator, WinCopies.Util.DotNetFix.IDisposable
    {
        private object _current;
        private bool _enumerationStarted = false;

        public bool IsDisposed { get; private set; }

        public object Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _enumerationStarted ? _current : throw new InvalidOperationException("The enumeration has not been started or has completed."); protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                _enumerationStarted = true;

                return true;
            }

            _current = default;

            _enumerationStarted = false;

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

            _enumerationStarted = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

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



    public abstract class _Enumerator : IEnumerator, WinCopies.Util.DotNetFix.IDisposable
    {
        public bool IsDisposed { get; private set; }

        private IEnumerator _innerEnumerator;

        protected IEnumerator InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

        private object _current;

        public object Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _current; protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object IEnumerator.Current => Current;

        internal _Enumerator(IEnumerable enumerable) => _innerEnumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

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
}

#endif
