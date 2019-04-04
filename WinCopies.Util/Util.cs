using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{
    public static class Util
    {

        public static RoutedCommand CommonCommand { get; } = new RoutedCommand(nameof(CommonCommand), typeof(Util));

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Predicate<T> GetCommonPredicate<T>() => (T value) => true;

        // public static KeyValuePair<TKey, Func<bool>>[] GetIfKeyValuePairPredicateArray<TKey>(params KeyValuePair<TKey, Func<bool>>[] keyValuePairs) => keyValuePairs;

        #region IfAnd, IfOr and IfXor

        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static KeyValuePair<TKey, Func<bool>> GetIfKeyValuePairPredicate<TKey>(TKey key, Func<bool> predicate) => new KeyValuePair<TKey, Func<bool>>(key, predicate);

        public enum ComparisonType

        {

            And = 0,

            Or = 1,

            Xor = 2

        }

        public enum Comparison

        {

            Equals = 0,

            DoesNotEqual = 1,

            LesserThan = 2,

            LesserOrEquals = 3,

            GreaterThan = 4,

            GreaterOrEquals = 5

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
        public static bool If(ComparisonType comparisonType, Comparison comparison, object value, params object[] values) => If(comparisonType, comparison, EqualityComparer<object>.Default, GetCommonPredicate<object>(), value, values);

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
        public static bool If(ComparisonType comparisonType, Comparison comparison, IComparer<object> comparer, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (object _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = comparer.Compare(_value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (object _value in values)

                    if (predicate(_value))

                    {

                        result = comparer.Compare(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = comparer.Compare(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (object _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = comparisonDelegate(_value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (object _value in values)

                    if (predicate(_value))

                    {

                        result = comparisonDelegate(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = comparisonDelegate(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = comparisonDelegate(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, IEqualityComparer<object> equalityComparer, Predicate<object> predicate, object value, params object[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (object _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = equalityComparer.Equals(_value, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (object _value in values)

                    if (predicate(_value))

                    {

                        result = equalityComparer.Equals(_value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                            return true;

                    }

                return false;

            }

            else

            {

                object _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = equalityComparer.Equals(_value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, IComparer<object> comparer, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    result = comparer.Compare(_value.Key, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (_value.Value())

                    {

                        result = comparer.Compare(_value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                KeyValuePair<object, Func<bool>> _value;

                object __value = null;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        result = comparer.Compare(_value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int i = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    i = comparisonDelegate(_value.Key, value);

                    if (!((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                {

                    if (_value.Value())

                    {

                        i = comparisonDelegate(_value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                }

                return false;

            }

            else

            {

                KeyValuePair<object, Func<bool>> _value;

                object __value = null;

                for (int _i = 0; _i < values.Length; _i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        i = comparisonDelegate(_value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = _i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                i = comparisonDelegate(__value, value);

                                if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, IEqualityComparer<object> equalityComparer, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result = false;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    result = equalityComparer.Equals(_value.Key, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, Func<bool>> _value in values)

                    if (_value.Value())

                    {

                        result = equalityComparer.Equals(_value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                            return true;

                    }

                return false;

            }

            else

            {

                KeyValuePair<object, Func<bool>> _value;

                object __value = null;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        result = equalityComparer.Equals(_value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

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
        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparison, out key, EqualityComparer<object>.Default, GetCommonPredicate<object>(), value, values);

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
        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, IComparer<object> comparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, object> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparer.Compare(_value.Value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, object> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = comparer.Compare(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, object> _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = comparer.Compare(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, Comparison<object> comparisonDelegate, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, object> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparisonDelegate(_value.Value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, object> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = comparisonDelegate(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, object> _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = comparisonDelegate(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = comparisonDelegate(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, IEqualityComparer<object> equalityComparer, Predicate<object> predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, object> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = equalityComparer.Equals(_value.Value, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, object> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = equalityComparer.Equals(_value.Value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, object> _value;

                object __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = equalityComparer.Equals(_value.Value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, IComparer<object> comparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparer.Compare(_value.Value.Key, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (_value.Value.Value())

                    {

                        result = comparer.Compare(_value.Value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                object __value = null;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        result = comparer.Compare(_value.Value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, Comparison<object> comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int i = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    i = comparisonDelegate(_value.Value.Key, value);

                    if (!((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                {

                    if (_value.Value.Value())

                    {

                        i = comparisonDelegate(_value.Value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                object __value = null;

                for (int _i = 0; _i < values.Length; _i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        i = comparisonDelegate(_value.Value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = _i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                i = comparisonDelegate(__value, value);

                                if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

            }

        }

        public static bool If(ComparisonType comparisonType, Comparison comparison, out object key, IEqualityComparer<object> equalityComparer, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result = false;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = equalityComparer.Equals(_value.Value.Key, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = null;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value in values)

                    if (_value.Value.Value())

                    {

                        result = equalityComparer.Equals(_value.Value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = null;

                return false;

            }

            else

            {

                KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

                object __value = null;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        result = equalityComparer.Equals(_value.Value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                {

                                    key = null;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = null;

                return false;

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
        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, T value, params T[] values) => If(comparisonType, comparison, EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

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
        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, IComparer<T> comparer, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (T _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = comparer.Compare(_value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (T _value in values)

                    if (predicate(_value))

                    {

                        result = comparer.Compare(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = comparer.Compare(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, Comparison<T> comparisonDelegate, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (T _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = comparisonDelegate(_value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (T _value in values)

                    if (predicate(_value))

                    {

                        result = comparisonDelegate(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = comparisonDelegate(_value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = comparisonDelegate(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, IEqualityComparer<T> equalityComparer, Predicate<T> predicate, T value, params T[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (T _value in values)

                {

                    if (!predicate(_value))

                        return false;

                    result = equalityComparer.Equals(_value, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (T _value in values)

                    if (predicate(_value))

                    {

                        result = equalityComparer.Equals(_value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                            return true;

                    }

                return false;

            }

            else

            {

                T _value;

                T __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value))

                    {

                        result = equalityComparer.Equals(_value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j];

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, IComparer<T> comparer, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    result = comparer.Compare(_value.Key, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (_value.Value())

                    {

                        result = comparer.Compare(_value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                return false;

            }

            else

            {

                KeyValuePair<T, Func<bool>> _value;

                T __value = default;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        result = comparer.Compare(_value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, Comparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int i = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    i = comparisonDelegate(_value.Key, value);

                    if (!((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                {

                    if (_value.Value())

                    {

                        i = comparisonDelegate(_value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                            return true;

                    }

                }

                return false;

            }

            else

            {

                KeyValuePair<T, Func<bool>> _value;

                T __value = default;

                for (int _i = 0; _i < values.Length; _i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        i = comparisonDelegate(_value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = _i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                i = comparisonDelegate(__value, value);

                                if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

            }

        }

        public static bool If<T>(ComparisonType comparisonType, Comparison comparison, IEqualityComparer<T> equalityComparer, T value, params KeyValuePair<T, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result = false;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                {

                    if (!_value.Value())

                        return false;

                    result = equalityComparer.Equals(_value.Key, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                        return false;

                }

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<T, Func<bool>> _value in values)

                    if (_value.Value())

                    {

                        result = equalityComparer.Equals(_value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                            return true;

                    }

                return false;

            }

            else

            {

                KeyValuePair<T, Func<bool>> _value;

                T __value = default;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value())

                    {

                        result = equalityComparer.Equals(_value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Key;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                    return false;

                            }

                            return true;

                        }

                    }

                }

                return false;

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
        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparison, out key, EqualityComparer<TValue>.Default, GetCommonPredicate<TValue>(), value, values);

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
        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, IComparer<TValue> comparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparer.Compare(_value.Value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = comparer.Compare(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, TValue> _value;

                TValue __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = comparer.Compare(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, Comparison<TValue> comparisonDelegate, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparisonDelegate(_value.Value, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = comparisonDelegate(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, TValue> _value;

                TValue __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = comparisonDelegate(_value.Value, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = comparisonDelegate(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                {

                    if (!predicate(_value.Value))

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = equalityComparer.Equals(_value.Value, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, TValue> _value in values)

                    if (predicate(_value.Value))

                    {

                        result = equalityComparer.Equals(_value.Value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, TValue> _value;

                TValue __value;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (predicate(_value.Value))

                    {

                        result = equalityComparer.Equals(_value.Value, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, IComparer<TValue> comparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int result = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = comparer.Compare(_value.Value.Key, value);

                    if (!((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (_value.Value.Value())

                    {

                        result = comparer.Compare(_value.Value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value = default;

                TValue __value = default;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        result = comparer.Compare(_value.Value.Key, value);

                        if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                result = comparer.Compare(__value, value);

                                if ((result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, Comparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!comparison.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparison), nameof(Comparison)));

            int i = 0;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    i = comparisonDelegate(_value.Value.Key, value);

                    if (!((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                        (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                        (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                {

                    if (_value.Value.Value())

                    {

                        i = comparisonDelegate(_value.Value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

                TValue __value = default;

                for (int _i = 0; _i < values.Length; _i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        i = comparisonDelegate(_value.Value.Key, value);

                        if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                            (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                            (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                        {

                            for (int j = _i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                i = comparisonDelegate(__value, value);

                                if ((i == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||
                                    (i < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||
                                    (i > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals)))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

            }

        }

        public static bool If<TKey, TValue>(ComparisonType comparisonType, Comparison comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            if (!comparisonType.IsValidEnumValue())

                throw new ArgumentException(string.Format(NoValidEnumValue, nameof(comparisonType), nameof(ComparisonType)));

            if (!(comparison == Comparison.Equals || comparison == Comparison.DoesNotEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(Comparison.Equals)} or {nameof(Comparison.DoesNotEqual)}");

            bool result = false;

            // If they are, we check the comparison type for the 'and' comparison.

            if (comparisonType == ComparisonType.And)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                {

                    if (!_value.Value.Value())

                    {

                        key = _value.Key;

                        return false;

                    }

                    result = equalityComparer.Equals(_value.Value.Key, value);

                    if ((!result && comparison == Comparison.Equals) || (result && comparison == Comparison.DoesNotEqual))

                    {

                        key = _value.Key;

                        return false;

                    }

                }

                key = default;

                return true;

            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == ComparisonType.Or)

            {

                foreach (KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value in values)

                    if (_value.Value.Value())

                    {

                        result = equalityComparer.Equals(_value.Value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            key = _value.Key;

                            return true;

                        }

                    }

                key = default;

                return false;

            }

            else

            {

                KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

                TValue __value = default;

                for (int i = 0; i < values.Length; i++)
                {

                    _value = values[i];

                    if (_value.Value.Value())

                    {

                        result = equalityComparer.Equals(_value.Value.Key, value);

                        if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                        {

                            for (int j = i + 1; j < values.Length; j++)

                            {

                                __value = values[j].Value.Key;

                                result = equalityComparer.Equals(__value, value);

                                if ((result && comparison == Comparison.Equals) || (!result && comparison == Comparison.DoesNotEqual))

                                {

                                    key = default;

                                    return false;

                                }

                            }

                            key = _value.Key;

                            return true;

                        }

                    }

                }

                key = default;

                return false;

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

            T[] newArray = null;

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

            T[] newArray = null;

            long totalArraysLength = 0;

            long totalArraysIndex = 0;

            foreach (T[] array in arrays)

            {

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

    }
}
