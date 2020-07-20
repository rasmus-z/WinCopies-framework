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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WinCopies.Collections
{
    namespace Generic
    {
        public interface IComparer<in T> : System.Collections.Generic.IComparer<T>
        {
            SortingType SortingType { get; set; }
        }

        public abstract class Comparer<T> : System.Collections.Generic.Comparer<T>, IComparer<T>
        {
            public SortingType SortingType { get; set; }

            protected abstract int CompareOverride(T x, T y);

            public sealed override int Compare(T x, T y)
            {
                int result = CompareOverride(x, y);

                return SortingType == SortingType.Ascending ? result : -result;
            }
        }

        public interface IEqualityComparer<in T> : System.Collections.Generic.IEqualityComparer<T>
        {
            bool Equals(
#if !CS7
                [AllowNull]
#endif
            T x,
#if !CS7
                [AllowNull]
#endif
            object y);
        }

        public abstract class EqualityComparer<T> : System.Collections.Generic.EqualityComparer<T>, IEqualityComparer<T>
        {
            public bool Equals(
#if !CS7
                [AllowNull]
#endif
            T x,
#if !CS7
                [AllowNull]
#endif
            object y) => y is T _y && EqualsOverride(x, _y);

            public sealed override bool Equals(
#if !CS7
                [AllowNull]
#endif
            T x,
#if !CS7
                [AllowNull]
#endif
            T y) => EqualsOverride(x, y);

            protected abstract bool EqualsOverride(
#if !CS7
                [AllowNull]
#endif
            T x,
#if !CS7
                [AllowNull]
#endif
            T y);
        }
    }

    public enum SortingType
    {
        Ascending,

        Descending
    }

    public interface IComparer : System.Collections.IComparer
    {
        SortingType SortingType { get; set; }
    }

    public sealed class Comparer : IComparer
    {
        public static readonly Comparer Default = new Comparer(System.Threading.Thread.CurrentThread.CurrentCulture);

        public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);

        private readonly System.Collections.Comparer _comparer;

        public SortingType SortingType { get; set; }

        public Comparer(CultureInfo cultureInfo) => _comparer = new System.Collections.Comparer(cultureInfo);

        public int Compare(object x, object y)
        {

            int result = _comparer.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }
    }
}