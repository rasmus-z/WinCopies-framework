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

namespace WinCopies.Collections.Generic
{
    public static class NullableValueEntryHelper
    {

        public static bool Equals<T>(in INullableValueEntry<T> value, in object obj) where T : struct
        {
            if (value == null) return obj == null;

            return value.Value.HasValue ? value.Value.Equals(obj) : obj == null;
        }

        public static bool Equals<T>(in INullableValueEntry<T> left, in T right) where T : struct, IEquatable<T> => left == null ? false : left.Value.HasValue && left.Value.Equals(right);

        public static bool Equals<T>(in INullableValueEntry<T> left, in T? right) where T : struct, IEquatable<T> => right.HasValue ? Equals(left, right.Value) : left == null || !left.Value.HasValue;

        public static bool Equals<T>(in INullableValueEntry<T> left, in INullableValueEntry<T> right) where T : struct, IEquatable<T> => right == null ? left == null || !left.Value.HasValue : Equals(left, right.Value);

        public static int Compare<T>(in INullableValueEntry<T> left, in T right) where T : struct, IComparable<T> => left != null && left.Value.HasValue ? left.Value.Value.CompareTo(right) : -1;

        public static int Compare<T>(in T? left, in T? right) where T : struct, IComparable<T>
        {
            if (left.HasValue) return right.HasValue ? left.Value.CompareTo(right.Value) : 1;

            return right.HasValue ? -1 : 0;
        }

        public static int Compare<T>(in INullableValueEntry<T> left, in T? right) where T : struct, IComparable<T>
        {
            if (right.HasValue) return Compare(left, right.Value);

            if (left == null) return 0;

            return left.Value.HasValue ? 1 : 0;
        }

        public static int Compare<T>(in INullableValueEntry<T> left, in INullableValueEntry<T> right) where T : struct, IComparable<T>
        {
            if (right == null)
            {
                if (left == null) return 0;

                return left.Value.HasValue ? 1 : 0;
            }

            return Compare(left, right.Value);
        }
    }

    public static class NullableRefEntryHelper
    {
        public static bool Equals<T>(in INullableRefEntry<T> value, in object obj) where T : class => value == null || value.Value == null ? obj == null : value.Value.Equals(obj);

        public static bool Equals<T>(in INullableRefEntry<T> left, in T right) where T : class, IEquatable<T> => left == null || left.Value == null ? right == null : left.Value.Equals(right);

        public static bool Equals<T>(in INullableRefEntry<T> left, in INullableRefEntry<T> right) where T : class, IEquatable<T> => right == null ? left == null || left.Value == null : Equals(left, right.Value);

        public static int Compare<T>(in INullableRefEntry<T> left, in T right) where T : class, IComparable<T> => left == null || left.Value == null ? -1 : left.Value.CompareTo(right);

        public static int Compare<T>(in INullableRefEntry<T> left, in INullableRefEntry<T> right) where T : class, IComparable<T>
        {
            if (right == null) return left == null || left.Value == null ? 0 : 1;

            return Compare(left, right.Value);
        }
    }
}

#endif
