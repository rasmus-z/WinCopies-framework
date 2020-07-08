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
    public interface IReadOnlyUIntIndexedList : IUIntIndexedCollection, IEnumerable
    {
        /// <summary>
        /// Gets or sets an item at a given index in the list.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at <paramref name="index"/>.</returns>
        object this[uint index] { get; }

        /// <summary>
        /// Checks if the list contains a given value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if the list contains <paramref name="value"/>, otherwise <see langword="false"/>.</returns>
        bool Contains(in object value);

        /// <summary>
        /// Returns the index of a given value in the list.
        /// </summary>
        /// <param name="value">The value for which return the index.</param>
        /// <returns>The index of <paramref name="value"/> if it was found, or <see langword="null"/> otherwise.</returns>
        uint? IndexOf(in object value);
    }
}

#endif
