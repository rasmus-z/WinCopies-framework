using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using WinCopies.Util;
using static WinCopies.Util.Generic;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;

namespace WinCopies.Util
{

    /// <summary>
    /// Delegate for a non-generic comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns>An <see cref="int"/> which is lesser than 0 if x is lesser than y, 0 if x is equal to y and greater than 0 if x is greater than y.</returns>
    public delegate int Comparison(object x, object y);

    /// <summary>
    /// Delegate for a non-generic equality comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
    public delegate bool EqualityComparison(object x, object y);

    /// <summary>
    /// Delegate for a generic equality comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
    public delegate bool EqualityComparison<in T>(T x, T y);

    /// <summary>
    /// Delegate for a non-generic predicate.
    /// </summary>
    /// <param name="value">The value to test</param>
    /// <returns><see langword="true"/> if the predicate success, otherwise <see langword="false"/>.</returns>
    public delegate bool Predicate(object value);

    public delegate void ActionParams(params object[] args);

    public delegate void ActionParams<in T>(params T[] args);

    public delegate object Func();

    public delegate object FuncParams(params object[] args);

    public delegate TResult FuncParams<in TParams, out TResult>(params TParams[] args);

    /// <summary>
    /// Provides some static helper methods.
    /// </summary>
    public static class Util
    {

        public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.SetBackgroundWorkerProperty method overloads.")]
        public static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged => bgWorker.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException("Cannot change property value when BackgroundWorker is busy.") : (false, Extensions.GetField(fieldName, declaringType, bindingFlags).GetValue(bgWorker))
                : bgWorker.SetProperty(propertyName, fieldName, newValue, declaringType, true, bindingFlags);

        /// <summary>
        /// Provides a <see cref="Predicate"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate GetCommonPredicate() => (object value) => true;

        /// <summary>
        /// Provides a <see cref="Predicate{T}"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate<T> GetCommonPredicate<T>() => (T value) => true;

        // todo: key-value pairs to raise an argument exception

        public static void ThrowOnNotValidEnumValue(params Enum[] values)

        {

            foreach (Enum value in values)

                value.ThrowIfNotValidEnumValue();

        }

        public static void ThrowOnEnumNotValidEnumValue(Enum value, params Enum[] values)

        {

            foreach (Enum _value in values)

                if (_value == value)

                    throw new InvalidOperationException($"'{_value.ToString()}' is not an expected value.");

        }

        // public static KeyValuePair<TKey, Func<bool>>[] GetIfKeyValuePairPredicateArray<TKey>(params KeyValuePair<TKey, Func<bool>>[] keyValuePairs) => keyValuePairs;

        #region 'If' methods

        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static KeyValuePair<TKey, Func<bool>> GetIfKeyValuePairPredicate<TKey>(TKey key, Func<bool> predicate) => new KeyValuePair<TKey, Func<bool>>(key, predicate);

        #region Enums

        /// <summary>
        /// Comparison types for the If functions.
        /// </summary>
        public enum ComparisonType

        {

            /// <summary>
            /// Check if all conditions are checked.
            /// </summary>
            And = 0,

            /// <summary>
            /// Check if at least one condition is checked.
            /// </summary>
            Or = 1,

            /// <summary>
            /// Check if exactly one condition is checked.
            /// </summary>
            Xor = 2

        }

        /// <summary>
        /// Comparison modes for the If functions.
        /// </summary>
        public enum ComparisonMode
        {

            /// <summary>
            /// Use a binary comparison
            /// </summary>
            Binary = 0,

            /// <summary>
            /// Use a logical comparison
            /// </summary>
            Logical = 1

        }

        /// <summary>
        /// Comparison to perform.
        /// </summary>
        public enum Comparison

        {

            /// <summary>
            /// Check for values equality
            /// </summary>
            Equal = 0,

            /// <summary>
            /// Check for values non-equality
            /// </summary>
            NotEqual = 1,

            /// <summary>
            /// Check if a value is lesser than a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            Lesser = 2,

            /// <summary>
            /// Check if a value is lesser than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            LesserOrEqual = 3,

            /// <summary>
            /// Check if a value is greater than a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            Greater = 4,

            /// <summary>
            /// Check if a value is greater than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            GreaterOrEqual = 5,

            /// <summary>
            /// Check if an object reference is equal to a given object reference. This field only works for methods that use equality comparers (not lesser/greater ones).
            /// </summary>
            ReferenceEqual = 6

        }

        #endregion

        #region 'Throw' methods

        private static void ThrowOnInvalidIfMethodArg(IfCT comparisonType, IfCM comparisonMode, IfComp comparison)

        {

            ThrowOnNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)IfComp.ReferenceEqual, typeof(IfComp));

        }

        private static void ThrowOnInvalidEqualityIfMethodEnumValue(IfCT comparisonType, IfCM comparisonMode, IfComp comparison)

        {

            ThrowOnNotValidEnumValue(comparisonType, comparisonMode);

            if (!(comparison == IfComp.Equal || comparison == IfComp.NotEqual || comparison == IfComp.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(IfComp.Equal)}, {nameof(IfComp.NotEqual)} or {nameof(IfComp.ReferenceEqual)}");

        }

        private static void ThrowOnInvalidEqualityIfMethodArg(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, Type valueType, EqualityComparison comparisonDelegate)

        {

            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !valueType.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

        }

        private static void ThrowOnInvalidEqualityIfMethodArg<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate)

        {

            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !typeof(T).IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

        }

        #endregion

        #region 'Check comparison' methods

        private static bool CheckIfComparison(IfComp comparison, bool predicateResult, int result)
        {
            switch (comparison)

            {

                case IfComp.Equal:
                case IfComp.ReferenceEqual:

                    return predicateResult && result == 0;

                case IfComp.LesserOrEqual:

                    return result <= 0;

                case IfComp.GreaterOrEqual:

                    return result >= 0;

                case IfComp.Lesser:

                    return !predicateResult && result < 0;

                case IfComp.Greater:

                    return !predicateResult && result > 0;

                case IfComp.NotEqual:

                    return !predicateResult && result != 0;

                default:

                    return false;//: comparisonType == ComparisonType.Or ?//(result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||//    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||//    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))

            }
        }

        private static bool CheckEqualityComparison(IfComp comparison, object value, object valueToCompare, bool predicateResult, EqualityComparison comparisonDelegate)
        {

            if (comparison == IfComp.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

            switch (comparison)

            {

                case IfComp.Equal:

                    return predicateResult && comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:

                    return !predicateResult && !comparisonDelegate(value, valueToCompare);

                case IfComp.ReferenceEqual:

#pragma warning disable IDE0002
                    return object.ReferenceEquals(value, valueToCompare);
#pragma warning restore IDE0002

                default:

                    return false;

            }

        }

        private static bool CheckEqualityComparison<T>(IfComp comparison, T value, T valueToCompare, bool predicateResult, EqualityComparison<T> comparisonDelegate)
        {

            // Because we've already checked that for the 'T' type in the 'If' method and assuming that 'T' is the base type of all the values to test, if 'T' is actually a class, we don't need to check here if the type of the current value is actually a class when comparison is set to ReferenceEqual.

            switch (comparison)

            {

                case IfComp.Equal:

                    return predicateResult && comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:

                    return !predicateResult && !comparisonDelegate(value, valueToCompare);

                case IfComp.ReferenceEqual:

#pragma warning disable IDE0002
                    return object.ReferenceEquals(value, valueToCompare);
#pragma warning restore IDE0002

                default:

                    return false;

            }

        }

        private delegate bool CheckIfComparisonDelegate(object value, Func<bool> predicate);

        private delegate bool CheckIfComparisonDelegate<T>(T value, Func<bool> predicate);

        #endregion

        #region Enumerables

        private interface IIfValuesEnumerable
        {

            Array Array { get; }

            KeyValuePair<object, Func<bool>> GetValue(int index);

        }

        private class IfValuesEnumerable : IIfValuesEnumerable
        {

            private static KeyValuePair<object, Func<bool>> GetValue(object[] array, int index, Predicate predicate)

            {

                object result = array[index];

                return new KeyValuePair<object, Func<bool>>(result, () => predicate(result));

            }

            public object[] Array { get; }

            Array IIfValuesEnumerable.Array => Array;

            public Predicate Predicate { get; }

            public IfValuesEnumerable(object[] array, Predicate predicate)
            {

                Array = array;

                Predicate = predicate;

            }

            public KeyValuePair<object, Func<bool>> GetValue(int index) => GetValue(Array, index, Predicate);

        }

        private class IfKeyValuePairEnumerable : IIfValuesEnumerable
        {

            public KeyValuePair<object, Func<bool>>[] Array { get; }

            Array IIfValuesEnumerable.Array => Array;

            public IfKeyValuePairEnumerable(KeyValuePair<object, Func<bool>>[] array) => Array = array;

            public KeyValuePair<object, Func<bool>> GetValue(int index) => Array[index];

        }

        private interface IIfKeyValuesEnumerable
        {

            Array Array { get; }

            KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(int index);

        }

        private class IfKeyValuesEnumerable : IIfKeyValuesEnumerable
        {

            private static KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(KeyValuePair<object, object>[] array, int index, Predicate predicate)

            {

                KeyValuePair<object, object> result = array[index];

                return new KeyValuePair<object, KeyValuePair<object, Func<bool>>>(result.Key, new KeyValuePair<object, Func<bool>>(result.Value, () => predicate(result.Value)));

            }

            public KeyValuePair<object, object>[] Array { get; }

            Array IIfKeyValuesEnumerable.Array => Array;

            public Predicate Predicate { get; }

            public IfKeyValuesEnumerable(KeyValuePair<object, object>[] array, Predicate predicate)
            {

                Array = array;

                Predicate = predicate;

            }

            public KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(int index) => GetValue(Array, index, Predicate);

        }

        private class IfKeyKeyValuePairEnumerable : IIfKeyValuesEnumerable
        {

            public KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] Array { get; }

            Array IIfKeyValuesEnumerable.Array => Array;

            public IfKeyKeyValuePairEnumerable(KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] array) => Array = array;

            public KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(int index) => Array[index];

        }

        private interface IIfValuesEnumerable<T>
        {

            Array Array { get; }

            KeyValuePair<T, Func<bool>> GetValue(int index);

        }

        private class IfValuesEnumerable<T> : IIfValuesEnumerable<T>
        {

            private static KeyValuePair<T, Func<bool>> GetValue(T[] array, int index, Predicate<T> predicate)

            {

                T result = array[index];

                return new KeyValuePair<T, Func<bool>>(result, () => predicate(result));

            }

            public T[] Array { get; }

            Array IIfValuesEnumerable<T>.Array => Array;

            public Predicate<T> Predicate { get; }

            public IfValuesEnumerable(T[] array, Predicate<T> predicate)
            {

                Array = array;

                Predicate = predicate;

            }

            public KeyValuePair<T, Func<bool>> GetValue(int index) => GetValue(Array, index, Predicate);

        }

        private class IfKeyValuePairEnumerable<T> : IIfValuesEnumerable<T>
        {

            public KeyValuePair<T, Func<bool>>[] Array { get; }

            Array IIfValuesEnumerable<T>.Array => Array;

            public IfKeyValuePairEnumerable(KeyValuePair<T, Func<bool>>[] array) => Array = array;

            public KeyValuePair<T, Func<bool>> GetValue(int index) => Array[index];

        }

        private interface IIfKeyValuesEnumerable<TKey, TValue>
        {

            Array Array { get; }

            KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(int index);

        }

        private class IfKeyValuesEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
        {

            private static KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(KeyValuePair<TKey, TValue>[] array, int index, Predicate<TValue> predicate)

            {

                KeyValuePair<TKey, TValue> result = array[index];

                return new KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>(result.Key, new KeyValuePair<TValue, Func<bool>>(result.Value, () => predicate(result.Value)));

            }

            public KeyValuePair<TKey, TValue>[] Array { get; }

            Array IIfKeyValuesEnumerable<TKey, TValue>.Array => Array;

            public Predicate<TValue> Predicate { get; }

            public IfKeyValuesEnumerable(KeyValuePair<TKey, TValue>[] array, Predicate<TValue> predicate)
            {

                Array = array;

                Predicate = predicate;

            }

            public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(int index) => GetValue(Array, index, Predicate);

        }

        private class IfKeyKeyValuePairEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
        {

            public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] Array { get; }

            Array IIfKeyValuesEnumerable<TKey, TValue>.Array => Array;

            public IfKeyKeyValuePairEnumerable(KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] array) => Array = array;

            public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(int index) => Array[index];

        }

        #endregion

        private static bool IfInternal(IfCT comparisonType, IfCM comparisonMode, CheckIfComparisonDelegate comparisonDelegate, IIfValuesEnumerable values)

        {

            bool checkIfComparison(KeyValuePair<object, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (!checkIfComparison(values.GetValue(i)))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return false;

                    }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return true;

                    }

                return false;

            }

            // We check the comparison type for the 'xor' comparison.

            else

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))

                    {

                        for (i++; i < values.Array.Length; i++)

                            if (checkIfComparison(values.GetValue(i)))

                            {

                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i));

                                return false;

                            }

                        return true;

                    }

                return false;

            }

        }

        private static bool IfInternal(IfCT comparisonType, IfCM comparisonMode, CheckIfComparisonDelegate comparisonDelegate, out object key, IIfKeyValuesEnumerable values)

        {

            bool checkIfComparison(KeyValuePair<object, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (!checkIfComparison(_value.Value))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return true;

                    }

                }

                key = null;

                return false;

            }

            // We check the comparison type for the 'xor' comparison.

            else

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))

                    {

                        for (i++; i < values.Array.Length; i++)

                        {

                            _value = values.GetValue(i);

                            if (checkIfComparison(_value.Value))

                            {

                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i).Value);

                                key = _value.Key;

                                return false;

                            }

                        }

                        key = _value.Key;

                        return true;

                    }

                }

                key = null;

                return false;

            }

        }

        private static bool IfInternal<T>(IfCT comparisonType, IfCM comparisonMode, CheckIfComparisonDelegate<T> comparisonDelegate, IIfValuesEnumerable<T> values)

        {

            bool checkIfComparison(KeyValuePair<T, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (!checkIfComparison(values.GetValue(i)))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return false;

                    }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return true;

                    }

                return false;

            }

            // We check the comparison type for the 'xor' comparison.

            else

            {

                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))

                    {

                        for (i++; i < values.Array.Length; i++)

                            if (checkIfComparison(values.GetValue(i)))

                            {

                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i));

                                return false;

                            }

                        return true;

                    }

                return false;

            }

        }

        private static bool IfInternal<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, CheckIfComparisonDelegate<TValue> comparisonDelegate, out TKey key, IIfKeyValuesEnumerable<TKey, TValue> values)

        {

            bool checkIfComparison(KeyValuePair<TValue, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (!checkIfComparison(_value.Value))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))

                    {

                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return true;

                    }

                }

                key = default;

                return false;

            }

            // We check the comparison type for the 'xor' comparison.

            else

            {

                for (int i = 0; i < values.Array.Length; i++)

                {

                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))

                    {

                        for (i++; i < values.Array.Length; i++)

                        {

                            _value = values.GetValue(i);

                            if (checkIfComparison(_value.Value))

                            {

                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i).Value);

                                key = _value.Key;

                                return false;

                            }

                        }

                        key = _value.Key;

                        return true;

                    }

                }

                key = default;

                return false;

            }

        }

        #region Non generic methods

        #region Comparisons without key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer{T}"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, IComparer, Predicate, object, params object[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IComparer comparer, Predicate<object> predicate, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IComparer comparer, Predicate predicate, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="Comparison{T}"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Util.Comparison, Predicate, object, params object[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Util.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="WinCopies.Util.Comparison"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, WinCopies.Util.Comparison comparisonDelegate, Predicate predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), new IfValuesEnumerable(values, predicate));

        }

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="equalityComparer">The equality comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, IEqualityComparer, Predicate, object, params object[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IEqualityComparer equalityComparer, Predicate<object> predicate, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, equalityComparer, new Predicate(o => predicate(o)), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="equalityComparer">The equality comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IEqualityComparer equalityComparer, Predicate predicate, object value, params object[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="EqualityComparison"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison comparisonDelegate, Predicate predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), new IfValuesEnumerable(values, predicate));

        }

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IComparer comparer, object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Util.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Util.Comparison, object, params KeyValuePair<object, Func<bool>>[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Util.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, WinCopies.Util.Comparison comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), new IfKeyValuePairEnumerable(values));

        }

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IEqualityComparer equalityComparer, object value, params KeyValuePair<object, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), new IfKeyValuePairEnumerable(values));

        }

        #endregion

        #region Comparisons with key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IComparer comparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IComparer comparer, Predicate predicate, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Util.Comparison, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Util.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Util.Comparison comparisonDelegate, Predicate predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), out key, new IfKeyValuesEnumerable(values, predicate));

        }

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IEqualityComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IEqualityComparer equalityComparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, equalityComparer, new Predicate(o => predicate(o)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IEqualityComparer equalityComparer, Predicate predicate, object value, params KeyValuePair<object, object>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, EqualityComparison comparisonDelegate, Predicate predicate, object value, params KeyValuePair<object, object>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), out key, new IfKeyValuesEnumerable(values, predicate));

        }

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IComparer comparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Util.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Util.Comparison, object, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[])")]
        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Util.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Util.Comparison comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), out key, new IfKeyKeyValuePairEnumerable(values));

        }

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, IEqualityComparer equalityComparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

        public static bool If(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out object key, EqualityComparison comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (object _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), out key, new IfKeyKeyValuePairEnumerable(values));

        }

        #endregion

        #endregion

        #region Generic methods

        #region Comparisons without key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, T value, params T[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IComparer<T> comparer, Predicate<T> predicate, T value, params T[] values) => If(comparisonType, comparisonMode, comparison, (T x, T y) => comparer.Compare(x, y), predicate, value, values);

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, Comparison<T> comparisonDelegate, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (T _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), new IfValuesEnumerable<T>(values, predicate));

        }

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IEqualityComparer<T> equalityComparer, Predicate<T> predicate, T value, params T[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate, Predicate<T> predicate, T value, params T[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (T _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), new IfValuesEnumerable<T>(values, predicate));

        }

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IComparer<T> comparer, T value, params KeyValuePair<T, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, (T x, T y) => comparer.Compare(x, y), value, values);

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, Comparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (T _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), new IfKeyValuePairEnumerable<T>(values));

        }

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, IEqualityComparer<T> equalityComparer, T value, params KeyValuePair<T, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), value, values);

        public static bool If<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (T _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), new IfKeyValuePairEnumerable<T>(values));

        }

        #endregion

        #region Comparisons with key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<TValue>.Default, GetCommonPredicate<TValue>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, IComparer<TValue> comparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => comparer.Compare(x, y), predicate, value, values);

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, Comparison<TValue> comparisonDelegate, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (TValue _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), out key, new IfKeyValuesEnumerable<TKey, TValue>(values, predicate));

        }

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, EqualityComparison<TValue> comparisonDelegate, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (TValue _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), out key, new IfKeyValuesEnumerable<TKey, TValue>(values, predicate));

        }

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, IComparer<TValue> comparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => comparer.Compare(x, y), value, values);

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, Comparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (TValue _value, Func<bool> _predicate) => CheckIfComparison(comparison, _predicate(), comparisonDelegate(value, _value)), out key, new IfKeyKeyValuePairEnumerable<TKey, TValue>(values));

        }

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), value, values);

        public static bool If<TKey, TValue>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, out TKey key, EqualityComparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (TValue _value, Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate(), comparisonDelegate), out key, new IfKeyKeyValuePairEnumerable<TKey, TValue>(values));

        }

        #endregion

        #endregion

        #endregion

        public static bool IsNullEmptyOrWhiteSpace(string value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

        public static void ThrowIfNullEmptyOrWhiteSpace(string value)

        {

            if (IsNullEmptyOrWhiteSpace(value))

                throw new InvalidOperationException($"The given value is null, empty or white space. The given value is: '{value ?? ""}'");

        }

        public static void ThrowIfNullEmptyOrWhiteSpace(string value, string argumentName)

        {

            if (IsNullEmptyOrWhiteSpace(value))

                throw new ArgumentException($"The given value is null, empty or white space. The given value is: '{value ?? ""}'", argumentName);

        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different tables to concatenate.</param>
        /// <returns></returns>
        public static T[] Concatenate<T>(params T[][] arrays)

        {
            // /// <param name="elementType">The type of the items inside the tables.</param>

            T[] newArray;

            int totalArraysLength = 0;

            int totalArraysIndex = 0;

            foreach (T[] array in arrays)

            {

                // todo : in a newer version, instead, get the maximum rank of arrays in params Array[] arrays and add a gesture of this in the process (also for the ConcatenateLong method) ; and not forgetting to change the comments of the xmldoc about this.

                if (array.Rank != 1) throw new ArgumentException(ArrayWithMoreThanOneDimension);

                totalArraysLength += array.Length;

            }

            newArray = new T[totalArraysLength];

            for (int i = 0; i < arrays.Length - 1; i++)

            {

                T[] array = arrays[i];

                array.CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += array.Length;

            }

            arrays[arrays.Length - 1].CopyTo(newArray, totalArraysIndex);

            return newArray;

        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type using the <see cref="Array.LongLength"/> length property. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different tables to concatenate.</param>
        /// <returns></returns>
        public static T[] ConcatenateLong<T>(params T[][] arrays)

        {

            // /// <param name="elementType">The type of the items inside the tables.</param>

            T[] newArray;

            long totalArraysLength = 0;

            long totalArraysIndex = 0;

            foreach (T[] array in arrays)

            {

                // todo:

                if (array.Rank != 1) throw new ArgumentException("Arrays must have only one dimension.");

                totalArraysLength += array.LongLength;

            }

            newArray = new T[totalArraysLength];

            for (long i = 0; i < arrays.LongLength - 1; i++)

            {

                T[] array = arrays[i];

                array.CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += array.LongLength;

            }

            arrays[arrays.LongLength - 1].CopyTo(newArray, totalArraysIndex);

            return newArray;

        }

        /// <summary>
        /// Checks if a object is numeric.
        /// </summary>
        /// <remarks>This function makes a check at the object type. For a string-parsing-checking for numerical value, look at the <see cref="IsNumeric(string, out decimal)"/> function.</remarks>
        /// <param name="value">The object to check</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the object given is a numerical type.</returns>
        public static bool IsNumber(object value) => value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;

        /// <summary>
        /// Checks if a <see cref="string"/> is a numerical value.
        /// </summary>
        /// <remarks>This function tries to parse a <see cref="string"/> value to a <see cref="decimal"/> value. Given that <see cref="decimal"/> type is the greatest numerical type in the .NET framework, all the numbers can be supported in the .NET framework can be set in a <see cref="decimal"/> object.</remarks>
        /// <param name="s">The <see cref="string"/> to check</param>
        /// <param name="d">The <see cref="decimal"/> in which one set the <see cref="decimal"/> value</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the <see cref="string"/> given is a <see cref="decimal"/>.</returns>
        public static bool IsNumeric(string s, out decimal d) => decimal.TryParse(s, out d);

        /// <summary>
        /// Get all the flags in a flags enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>All the flags in the given enum type.</returns>
        public static T GetAllEnumFlags<T>() where T : Enum

        {

            Type enumType = typeof(T);

            if (enumType.GetCustomAttribute<FlagsAttribute>() == null)

                throw new ArgumentException("Enum is not a 'flags' enum.");

            Array array = Enum.GetValues(enumType);

            long values = 0;

            foreach (object value in array)

                values |= (long)Convert.ChangeType(value, TypeCode.Int64);

            return (T)Enum.ToObject(enumType, values);

        }

        /// <summary>
        /// Gets the numeric value for a field in an enum.
        /// </summary>
        /// <param name="enumType">The enum type in which to look for the specified enum field value.</param>
        /// <param name="fieldName">The enum field to look for.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue(Type enumType, string fieldName) => enumType.IsEnum ? Convert.ChangeType(enumType.GetField(fieldName).GetValue(null), Enum.GetUnderlyingType(enumType)) : throw new ArgumentException("'enumType' is not an enum type.");

        public static void ThrowIfNull(object obj, string argumentName)
        {

            if (obj is null)

                throw new ArgumentNullException(argumentName);

        }

        private static void ThrowIfNotTypeInternal<T>(object obj, string argumentName) => throw (obj is null ? new ArgumentNullException(argumentName) : new ArgumentException($"{argumentName} must be {typeof(T).ToString()}. {argumentName} is {obj.GetType().ToString()}", argumentName));

        public static void ThrowIfNotType<T>(object obj, string argumentName)

        {

            if (!(obj is T))

                ThrowIfNotTypeInternal<T>(obj, argumentName);

        }

        public static T GetOrThrowIfNotType<T>(object obj, string argumentName)

        {

            if (obj is T _obj)

                return _obj;

            else

            {

                ThrowIfNotTypeInternal<T>(obj, argumentName);

                // We shouldn't reach this point.

                return default;

            }

        }

        public static object GetIf(object x, object y, WinCopies.Util.Comparison comparison, Func lower, Func greater, Func equals)

        {

            if (If<string, Func>(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();

        }

        public static TResult GetIf<TValues, TResult>(TValues x, TValues y, Comparison<TValues> comparison, Func<TResult> lower, Func<TResult> greater, Func<TResult> equals)

        {

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();

        }

        public static object GetIf(object x, object y, IComparer comparer, Func lower, Func greater, Func equals)

        {

            if (If<string, Func>(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparer.Compare(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();

        }

        public static TResult GetIf<TValues, TResult>(TValues x, TValues y, WinCopies.Util.Comparison comparison, Func<TResult> lower, Func<TResult> greater, Func<TResult> equals)

        {

            if (If(ComparisonType.Or, ComparisonMode.Logical, Comparison.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();

        }

    }

}
