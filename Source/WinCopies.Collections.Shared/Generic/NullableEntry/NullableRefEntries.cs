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
    public struct NullableRefEntry<T> : INullableRefEntry<T> where T : class
    {
        public T Value { get; }

        public NullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(in NullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in NullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in NullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in NullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>, IEquatable<INullableRefEntry<T>> where T : class, IEquatable<T>
    {
        public T Value { get; }

        public EquatableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public static bool operator ==(in EquatableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableRefEntry<T> : INullableRefEntry<T>, IComparable<T>, IComparable<INullableRefEntry<T>> where T : class, IComparable<T>
    {
        public T Value { get; }

        public ComparableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in ComparableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(in ComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(in ComparableNullableRefEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in ComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(in ComparableNullableRefEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>, IEquatable<INullableRefEntry<T>>, IComparable<T>, IComparable<INullableRefEntry<T>> where T : class, IEquatable<T>, IComparable<T>
    {
        public T Value { get; }

        public EquatableComparableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left < right);

    }
}

#endif
