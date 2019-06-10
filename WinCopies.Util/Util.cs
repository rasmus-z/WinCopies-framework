using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{
    public static class Util
    {

        public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged

        {

            if (bgWorker.IsBusy)

                if (throwIfBusy)

                    throw new InvalidOperationException("Cannot change property value when BackgroundWorker is busy.");

                else

                    return (false, Extensions.GetField(fieldName, declaringType, bindingFlags).GetValue(bgWorker));

            else

                return bgWorker.SetProperty(propertyName, fieldName, newValue, declaringType, performIntegrityCheck, bindingFlags);

        }

        public static Predicate<T> GetCommonPredicate<T>() => (T value) => true;

        // public static KeyValuePair<TKey, Func<bool>>[] GetIfKeyValuePairPredicateArray<TKey>(params KeyValuePair<TKey, Func<bool>>[] keyValuePairs) => keyValuePairs;

        #region IfAnd, IfOr and IfXor

        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static KeyValuePair<TKey, Func<bool>> GetIfKeyValuePairPredicate<TKey>(TKey key, Func<bool> predicate) => new KeyValuePair<TKey, Func<bool>>(key, predicate);

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

            Equal = 0,

            NotEqual = 1,

            Lesser = 2,

            LesserOrEqual = 3,

            Greater = 4,

            GreaterOrEqual = 5,

            ReferenceEqual = 6

        }

        private static void ThrowIfNotValidEnumValue(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison)

        {

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            comparison.ThrowIfNotValidEnumValue();

        }

        private static bool CheckIfComparison(Comparison comparison, bool predicateResult, int result)
        {
            switch (comparison)

            {

                case Comparison.Equal:
                case Comparison.ReferenceEqual:

                    return predicateResult && result == 0;

                case Comparison.LesserOrEqual:

                    return result <= 0;

                case Comparison.GreaterOrEqual:

                    return result >= 0;

                case Comparison.Lesser:

                    return !predicateResult && result < 0;

                case Comparison.Greater:

                    return !predicateResult && result > 0;

                case Comparison.NotEqual:

                    return !predicateResult && result != 0;

                default:

                    return false;//: comparisonType == ComparisonType.Or ?//(result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||//    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||//    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))

            }
        }

        private static bool CheckEqualityComparison(Comparison comparison, object value, bool predicateResult, bool result)
        {

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            return (predicateResult && result && (comparison == Comparison.Equal || comparison == Comparison.ReferenceEqual)) || (!(predicateResult || result) && comparison == Comparison.NotEqual);

        }

        // todo: factoriser au maximum

        #region Non generic methods

        #region Comparisons without key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, object value, params object[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<object>.Default, GetCommonPredicate<object>(), value, values);

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
        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IComparer<object> comparer, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (object _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (object _value in values)

                    if (CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value), comparer.Compare(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (object _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (object _value in values)

                    if (CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value), comparisonDelegate(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IEqualityComparer<object> equalityComparer, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)}, {nameof(Comparison.NotEqual)} or {nameof(Comparison.ReferenceEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (object _value in values)

                    if (!CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (object _value in values)

                    if (CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value, predicate(__value), comparison == Comparison.Equal ? equalityComparer.Equals(__value, value) : object.ReferenceEquals(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IComparer<object> comparer, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, Func<bool>> _value;

                KeyValuePair<object, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value(), comparer.Compare(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, Func<bool>> _value;

                KeyValuePair<object, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value(), comparisonDelegate(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IEqualityComparer<object> equalityComparer, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, Func<bool>> _value;

                KeyValuePair<object, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Key, __value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Key, value) : object.ReferenceEquals(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

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
        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<object>.Default, GetCommonPredicate<object>(), value, values);

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
        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, IComparer<object> comparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, object> _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, object> _value in values)

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, object> _value;

                KeyValuePair<object, object> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value.Value), comparer.Compare(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, object> _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, object> _value in values)

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, object> _value;

                KeyValuePair<object, object> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value.Value), comparisonDelegate(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, IEqualityComparer<object> equalityComparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, object> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, object> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, object> _value;

                KeyValuePair<object, object> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Value, predicate(__value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Value, value) : object.ReferenceEquals(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, IComparer<object> comparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value.Value(), comparer.Compare(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value.Value(), comparisonDelegate(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

        }

        public static bool If(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out object key, IEqualityComparer<object> equalityComparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            object _key = null;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Value.Key, __value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Value.Key, value) : object.ReferenceEquals(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : null;

                        return result;

                    }

                }

                key = null;

                return result;

            }

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
        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, T value, params T[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

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
        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IComparer<T> comparer, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (T _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (T _value in values)

                    if (CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value), comparer.Compare(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value), comparer.Compare(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, Comparison<T> comparisonDelegate, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (T _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (T _value in values)

                    if (CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value), comparisonDelegate(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value), comparisonDelegate(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IEqualityComparer<T> equalityComparer, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (T _value in values)

                    if (!CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (T _value in values)

                    if (CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value, predicate(_value), comparison == Comparison.Equal ? equalityComparer.Equals(_value, value) : object.ReferenceEquals(_value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, _value, predicate(__value), comparison == Comparison.Equal ? equalityComparer.Equals(__value, value) : object.ReferenceEquals(__value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IComparer<T> comparer, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<T, Func<bool>> _value;

                KeyValuePair<T, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value(), comparer.Compare(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value(), comparer.Compare(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, Comparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<T, Func<bool>> _value;

                KeyValuePair<T, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value(), comparisonDelegate(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value(), comparisonDelegate(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, IEqualityComparer<T> equalityComparer, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<T, Func<bool>> _value;

                KeyValuePair<T, Func<bool>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Key, _value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Key, value) : object.ReferenceEquals(_value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Key, __value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Key, value) : object.ReferenceEquals(__value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        return result;

                    }

                }

                return result;

            }

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
        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<TValue>.Default, GetCommonPredicate<TValue>(), value, values);

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
        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, IComparer<TValue> comparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, TValue> _value;

                KeyValuePair<TKey, TValue> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparer.Compare(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value.Value), comparer.Compare(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, Comparison<TValue> comparisonDelegate, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (!CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, TValue> _value;

                KeyValuePair<TKey, TValue> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, predicate(_value.Value), comparisonDelegate(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, predicate(__value.Value), comparisonDelegate(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, TValue> _value;

                KeyValuePair<TKey, TValue> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Value, predicate(_value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value, value) : object.ReferenceEquals(_value.Value, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Value, predicate(__value.Value), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Value, value) : object.ReferenceEquals(__value.Value, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, IComparer<TValue> comparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparer.Compare(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value.Value(), comparer.Compare(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, Comparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowIfNotValidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == Comparison.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)Comparison.ReferenceEqual, typeof(Comparison));

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (!CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckIfComparison(comparison, _value.Value.Value(), comparisonDelegate(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckIfComparison(comparison, __value.Value.Value(), comparisonDelegate(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, ComparisonMode comparisonMode, Comparison comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            comparisonType.ThrowIfNotValidEnumValue();

            comparisonMode.ThrowIfNotValidEnumValue();

            if (!(comparison == Comparison.Equal || comparison == Comparison.NotEqual || comparison == Comparison.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equal)} or {nameof(Comparison.NotEqual)}");

            if (comparison == Comparison.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class type.");

            TKey _key = default;

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                result = true;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (!CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = false;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                result = false;

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        _key = _value.Key;

                        result = true;

                        if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                    }

                key = _key;

                return result;

            }

            else

            {

                result = false;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    result = true;

                    if (CheckEqualityComparison(comparison, _value.Value.Key, _value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(_value.Value.Key, value) : object.ReferenceEquals(_value.Value.Key, value)))

                    {

                        for (int j = i + 1; j < values.Length; j++)

                        {

                            __value = values[j];

                            if (CheckEqualityComparison(comparison, __value.Value.Key, __value.Value.Value(), comparison == Comparison.Equal ? equalityComparer.Equals(__value.Value.Key, value) : object.ReferenceEquals(__value.Value.Key, value)))

                            {

                                result = false;

                                if (comparisonMode == ComparisonMode.Binary) continue; else /*if (comparisonMode == ComparisonMode.Logical)*/ break;

                            }

                        }

                        key = result ? _value.Key : default;

                        return result;

                    }

                }

                key = default;

                return result;

            }

        }

        #endregion

        #endregion

        #endregion

        public static bool IsNullEmptyOrWhiteSpace(string value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

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

        public static T GetEnumAllFlags<T>() where T : Enum

        {

            Type enumType = typeof(T);

            if (enumType.GetCustomAttributes<FlagsAttribute>().ToList().Count == 0)

                throw new ArgumentException("Enum is not a 'flags' enum.");

            Array array = Enum.GetValues(enumType);

            long values = 0;

            foreach (object value in array)

                values |= (long)Convert.ChangeType(value, TypeCode.Int64);

            return (T)Enum.ToObject(enumType, values);

        }

    }
}
