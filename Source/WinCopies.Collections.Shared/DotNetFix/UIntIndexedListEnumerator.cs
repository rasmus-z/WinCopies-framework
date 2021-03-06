﻿/* Copyright © Pierre Sprimont, 2020
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
using System.Diagnostics;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix
{
    public abstract class UIntIndexedListEnumeratorBase : WinCopies.Util.DotNetFix.IDisposable
    {
        private uint? index = null;

        protected internal uint? Index { get { ThrowIfDisposed(this); return index; } set { ThrowIfDisposed(this); index = value; } }

        private readonly Func<bool> moveNextMethod;

        protected internal Func<bool> MoveNextMethod { get { ThrowIfDisposed(this); return moveNextMethod; } set { ThrowIfDisposed(this); MoveNextMethod = value; } }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Reset();

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        ~UIntIndexedListEnumeratorBase() => Dispose(false);

        public virtual bool MoveNext()
        {
            ThrowIfDisposed(this);

            return MoveNextMethod();
        }

        public virtual void Reset() => Index = null;

        #endregion
    }

    public sealed class UIntIndexedListEnumerator : UIntIndexedListEnumeratorBase, IEnumerator
    {
        private IReadOnlyUIntIndexedList innerList;

        internal IReadOnlyUIntIndexedList InnerList { get { ThrowIfDisposed(this); return innerList; } private set { ThrowIfDisposed(this); innerList = value; } }

        private Func<bool> moveNextToReset;

        public static Func<UIntIndexedListEnumerator, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator e) =>
        {
            if (e.InnerList.Count > 0)
            {
                e.Index = 0;

                e.MoveNextMethod = () =>
                {
                    if (e.Index < e.InnerList.Count - 1)
                    {
                        e.Index++;

                        return true;
                    }

                    else return false;
                };

                return true;
            }

            else return false;
        };

        public object Current
        {
            get
            {
                Debug.Assert(Index.HasValue, $"{nameof(Index)} does not have value.");

                return InnerList[Index.Value];
            }
        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList)
        {
            MoveNextMethod = moveNextToReset = () => DefaultMoveNextMethod(this);

            innerList = uintIndexedList;
        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            innerList = uintIndexedList;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                InnerList = null;

                moveNextToReset = null;

                base.Dispose(disposing);
            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = moveNextToReset;
        }
    }
}

#endif
