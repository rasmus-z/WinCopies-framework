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

namespace WinCopies.Collections.DotNetFix
{
    public interface IUIntIndexedCollection : IEnumerable
    {
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        uint Count { get; }

        object SyncRoot { get; }

        bool IsSynchronized { get; }

        /// <summary>
        /// Copies the items of this collection to a given array starting at a given index.
        /// </summary>
        /// <param name="array">The array to copy the items.</param>
        /// <param name="index">The index in the array from which to start to copy.</param>
        void CopyTo(in Array array, int index);
    }
}

#endif
