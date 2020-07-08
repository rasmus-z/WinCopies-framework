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
    public struct NullableValueEntry<T> : INullableValueEntry<T> where T : struct
    {
        public T? Value { get; }

        public NullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(in NullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in NullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in NullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>> where T : struct, IEquatable<T>
    {
        public T? Value { get; }

        public EquatableNullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableValueEntry<T> : INullableValueEntry<T>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IComparable<T>
    {
        public T? Value { get; }

        public ComparableNullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in T? right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in T? right) => !(left < right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IEquatable<T>, IComparable<T>
    {
        public T? Value { get; }

        public EquatableComparableNullableValueEntry(T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left < right);

    }
}

#endif
