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

using System.Collections;

namespace WinCopies.Collections.DotNetFix
{
    public interface IUIntIndexedList : IReadOnlyUIntIndexedList, IEnumerable
    {
        /// <summary>
        /// Gets the item at a given index in the list.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at <paramref name="index"/>.</returns>
        new object this[uint index] { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the current list is fixed size.
        /// </summary>
        bool IsFixedSize { get; }

        /// <summary>
        /// Adds a value to the list.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>The index of the new value in the list.</returns>
        uint Add(in object value);

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inserts a value to the list.
        /// </summary>
        /// <param name="index">The index at which to add the value.</param>
        /// <param name="value">The value to add.</param>
        void Insert(in uint index, in object value);

        /// <summary>
        /// Removes a given value from the list.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        void Remove(in object value);

        /// <summary>
        /// Removes a value from the list at a given index.
        /// </summary>
        /// <param name="index">The index of the value to remove.</param>
        void RemoveAt(in uint index);
    }
}

#endif
