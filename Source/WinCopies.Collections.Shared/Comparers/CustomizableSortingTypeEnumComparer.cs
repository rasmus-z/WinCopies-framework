/* Copyright © Pierre Sprimont, 2019
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

using System;

namespace WinCopies.Collections
{
    public class CustomizableSortingTypeEnumComparer : EnumComparer, Generic.IComparer<Enum>
    {
        public SortingType SortingType { get; set; }

        protected virtual int CompareToObjectOverride(Enum x, object y) => base.CompareToObject(x, y);

        public sealed override int CompareToObject(Enum x, object y)
        {
            int result = CompareToObjectOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }

        protected virtual int CompareToEnumOverride(object x, Enum y) => base.CompareToEnum(x, y);

        public sealed override int CompareToEnum(object x, Enum y)
        {
            int result = CompareToEnumOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }

        protected virtual int CompareOverride(Enum x, Enum y) => base.Compare(x, y);

        public sealed override int Compare(Enum x, Enum y)
        {
            int result = CompareOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }
    }
}
