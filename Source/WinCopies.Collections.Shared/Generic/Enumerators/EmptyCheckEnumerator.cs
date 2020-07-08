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
using System.Collections.Generic;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.Generic
{
    public sealed class EmptyCheckEnumerator<T> : IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
    {
        #region Fields
        private IEnumerator<T> _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func<T> _current;
        #endregion

        #region Properties
        public bool IsDisposed { get; private set; }

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public T Current
        {
            get
            {
                ThrowIfDisposed();

                return _current();
            }
        }
        #endregion

        public EmptyCheckEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

        #region Methods
        private void ResetCurrent() => _current = () => throw new InvalidOperationException("The enumeration has not been started or has completed.");

        private void ResetMoveNext()
        {
            ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                ResetCurrent();
            }

            bool enumerate()
            {
                if (_enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            _moveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        _current = () => _enumerator.Current;

                        _moveNext = enumerate;

                        return true;
                    }

                    else
                    {
                        resetMoveNext();

                        return false;
                    }
                }

                else
                {
                    _moveNext = enumerate;

                    return enumerate();
                }
            };
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }
        #endregion

        #region Interface implementations
        #region IEnumerator implementation
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _enumerator.Reset();

            _hasItems = null;

            ResetMoveNext();
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            if (IsDisposed) return;

            _enumerator.Dispose();

            ResetMoveNext();

            IsDisposed = true;
        }
        #endregion
        #endregion
    }
}

#endif
