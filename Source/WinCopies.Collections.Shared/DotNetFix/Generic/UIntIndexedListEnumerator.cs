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
using System.Diagnostics;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public sealed class UIntIndexedListEnumerator<T> : UIntIndexedListEnumeratorBase, IEnumerator<T>
    {
        private IReadOnlyUIntIndexedList<T> innerList;

        internal IReadOnlyUIntIndexedList<T> InnerList { get { ThrowIfDisposed(this); return innerList; } private set { ThrowIfDisposed(this); innerList = value; } }

        private Func<bool> moveNextMethodToReset;

        public static Func<UIntIndexedListEnumerator<T>, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator<T> e) =>
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

        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return InnerList[Index.Value];
            }
        }

        object IEnumerator.Current => Current;

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList)
        {
            MoveNextMethod = moveNextMethodToReset = () => DefaultMoveNextMethod(this);

            innerList = uintIndexedList;
        }

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            innerList = uintIndexedList;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {

                InnerList = null;

                moveNextMethodToReset = null;

                base.Dispose(disposing);
            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = moveNextMethodToReset;
        }
    }
}

#endif
