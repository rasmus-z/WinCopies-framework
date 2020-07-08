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
using System.Collections.Generic;
using System.Diagnostics;
using WinCopies.Util;

namespace WinCopies.Collections.Generic
{
    public sealed class JoinSubEnumerator<T> : Enumerator<T, T>
    {
        private IEnumerator<T> _joinEnumerator;
        private T _firstValue;
        private bool _completed = false;
        private Func<bool> _moveNext;

        public JoinSubEnumerator(IEnumerable<T> subEnumerable, IEnumerable<T> joinEnumerable) : base(subEnumerable)
        {
#if DEBUG
            Debug.Assert(subEnumerable != null && joinEnumerable != null);
#endif

            _joinEnumerator =
#if !DEBUG
                (
#endif
                joinEnumerable
#if !DEBUG
                ?? throw GetArgumentNullException(nameof(joinEnumerable)))
#endif
                .GetEnumerator();

            InitDelegate();
        }

        private void InitDelegate() => _moveNext = () =>
        {
            if (InnerEnumerator.MoveNext())
            {
                _firstValue = InnerEnumerator.Current;

                _moveNext = () => _MoveNext();

                return _MoveNext();
            }

            else

                return false;
        };

        private bool _MoveNext()
        {
            if (_joinEnumerator.MoveNext())
            {
                Current = _joinEnumerator.Current;

                return true;
            }

            Current = _firstValue;

            _firstValue = default;

            _moveNext = () =>
            {
                if (InnerEnumerator.MoveNext())
                {
                    Current = InnerEnumerator.Current;

                    return true;
                }

                return false;
            };

            return true;
        }

        protected override bool MoveNextOverride()
        {
            if (_completed) return false;

            if (_moveNext()) return true;

            Current = default;

            _moveNext = null;

            _completed = true;

            return false;
        }

        protected override void ResetOverride()
        {
            base.ResetOverride();

            _joinEnumerator.Reset();

            _firstValue = default;

            _completed = false;

            InitDelegate();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _joinEnumerator = null;

            _firstValue = default;

            _moveNext = null;
        }
    }

    public sealed class JoinEnumerator<T> : Enumerator<IEnumerable<T>, T>
    {
        private IEnumerator<T> _subEnumerator;
        private IEnumerable<T> _joinEnumerable;
        private bool _completed = false;
        private Action _updateEnumerator;
        private Func<bool> _moveNext;
        private readonly bool _keepEmptyEnumerables;

        public JoinEnumerator(IEnumerable<IEnumerable<T>> enumerable, bool keepEmptyEnumerables, params T[] join) : base(enumerable)
        {
            _joinEnumerable = join;

            _keepEmptyEnumerables = keepEmptyEnumerables;

            InitDelegates();
        }

        private void InitDelegates()
        {
            _updateEnumerator = () =>
            {
                _subEnumerator = InnerEnumerator.Current.GetEnumerator();

                if (_keepEmptyEnumerables)

                    _updateEnumerator = () => _subEnumerator = _joinEnumerable.AppendValues(InnerEnumerator.Current).GetEnumerator();

                else

                    _updateEnumerator = () => _subEnumerator = new JoinSubEnumerator<T>(InnerEnumerator.Current, _joinEnumerable);
            };

            _moveNext = () =>
            {

                if (_subEnumerator == null)
                {
                    _MoveNext();

                    if (_completed)

                        return false;
                }

                _moveNext = () => __MoveNext();

                return __MoveNext();
            };
        }

        private bool __MoveNext()
        {
            bool moveNext()
            {
                if (_subEnumerator.MoveNext())
                {
                    Current = _subEnumerator.Current;

                    return true;
                }

                return false;
            }

            while (!_completed)
            {
                if (moveNext())

                    return true;

                _MoveNext();
            }

            return false;
        }

        private void _MoveNext()
        {
            if (InnerEnumerator.MoveNext())

                _updateEnumerator();

            _completed = true;

            _subEnumerator = null;
        }

        protected override bool MoveNextOverride() => _completed ? false : _moveNext();

        protected override void ResetOverride()
        {
            base.ResetOverride();

            _subEnumerator = null;

            InitDelegates();

            _completed = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Current = default;

            _joinEnumerable = null;

            _updateEnumerator = null;

            _moveNext = null;

            _subEnumerator = null;
        }
    }
}

#endif
