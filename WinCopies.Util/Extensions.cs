#pragma warning disable IDE0002

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{
    /// <summary>
    /// Provides some static extension methods.
    /// </summary>
    public static class Extensions
    {

        #region Enumerables extension methods

        // todo:

        //#region Contains methods

        //// public static bool Contains(this IEnumerable array, object value) => array.Contains(value, EqualityComparer<object>.Default);

        //public static bool Contains(this IEnumerable array, object value, IEqualityComparer comparer)

        //{

        //    foreach (object _value in array)

        //        if (comparer.Equals(_value, value)) return true;

        //    return false;

        //}

        //public static bool Contains(this IEnumerable array, object value, IComparer comparer) => array.Contains(value, (object x, object y) => comparer.Compare(x, y));

        //public static bool Contains(this IEnumerable array, object value, Comparison comparison)

        //{

        //    foreach (object _value in array)

        //        if (comparison(_value, value) == 0) return true;

        //    return false;

        //}

        //// todo: to add methods for the LongLength's Array property gesture.

        //public static bool Contains(this object[] array, object value, out int index) => array.Contains(value, EqualityComparer<object>.Default, out index);

        //public static bool Contains(this object[] array, object value, IEqualityComparer comparer, out int index)

        //{

        //    for (int i = 0; i < array.Length; i++)

        //        if (comparer.Equals(array[i], value))

        //        {

        //            index = i;

        //            return true;

        //        }

        //    index = -1;

        //    return false;

        //}

        //public static bool Contains(this object[] array, object value, IComparer comparer, out int index) => array.Contains(value, (object x, object y) => comparer.Compare(x, y), out index);

        //public static bool Contains(this object[] array, object value, Comparison comparison, out int index)

        //{

        //    for (int i = 0; i < array.Length; i++)

        //        if (comparison(array[i], value) == 0)

        //        {

        //            index = i;

        //            return true;

        //        }

        //    index = -1;

        //    return false;

        //}

        //public static bool ContainsRange(this IEnumerable array, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IComparer comparer, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IComparer comparer, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IComparer comparer, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IComparer comparer, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, out int index, IEnumerable values);

        //public static bool Contains<T>(this IEnumerable<T> array, T value);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, IEqualityComparer comparer);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, IComparer comparer);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, Comparison comparison);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, out int index);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, IEqualityComparer comparer, out int index);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, IComparer comparer, out int index);

        //public static bool Contains<T>(this IEnumerable<T> array, T value, Comparison comparison, out int index);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, IEqualityComparer comparer, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, IComparer comparer, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, Comparison comparison, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, out int index, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, IEqualityComparer comparer, out int index, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, IComparer comparer, out int index, params T[] values);

        //public static bool ContainsRange<T>(this IEnumerable<T> array, Comparison comparison, out int index, params T[] values);

        //#endregion

        // todo: Add-, Insert-, Remove-If(Not)Contains methods: add parameters like the Contains methods

        #region Add(Range)IfNotContains methods

        /// <summary>
        /// Tries to add a value to an <see cref="IList"/> if it does not contain it already.
        /// </summary>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="value">The value to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static bool AddIfNotContains(this IList collection, object value)

        {

            if (collection.Contains(value)) return false;

            collection.Add(value);

            return true;

        }

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static object[] AddRangeIfNotContains(this IList collection, params object[] values) => collection.AddRangeIfNotContains((IEnumerable)values);

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static object[] AddRangeIfNotContains(this IList collection, IEnumerable values)

        {

            ArrayBuilder<object> addedValues = new ArrayBuilder<object>();

            foreach (object value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Add(value);

                addedValues.AddLast(value);

            }

            return addedValues.ToArray();

        }



        /// <summary>
        /// Tries to add a value to an <see cref="ICollection{T}"/> if it does not contain it already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="value">The value to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> collection, T value)

        {

            if (collection.Contains(value)) return false;

            collection.Add(value);

            return true;

        }

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static T[] AddRangeIfNotContains<T>(this ICollection<T> collection, params T[] values) => collection.AddRangeIfNotContains((IEnumerable<T>)values);

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static T[] AddRangeIfNotContains<T>(this ICollection<T> collection, IEnumerable<T> values)

        {

            ArrayBuilder<T> addedValues = new ArrayBuilder<T>();

            foreach (T value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Add(value);

                addedValues.AddLast(value);

            }

            return addedValues.ToArray();

        }

        #endregion

        #region Insert(Range)IfNotContains methods

        public static bool InsertIfNotContains(this IList collection, int index, object value)

        {

            if (collection.Contains(value)) return false;

            collection.Insert(index, value);

            return true;

        }

        public static object[] InsertRangeIfNotContains(this IList collection, int index, params object[] values) => collection.InsertRangeIfNotContains(index, (IEnumerable)values);

        public static object[] InsertRangeIfNotContains(this IList collection, int index, IEnumerable values)

        {

            ArrayBuilder<object> addedValues = new ArrayBuilder<object>();

            foreach (object value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Insert(index, value);

                addedValues.AddLast(value);

            }

            return addedValues.ToArray();

        }



        public static bool InsertIfNotContains<T>(this IList<T> collection, int index, T value)

        {

            if (collection.Contains(value)) return false;

            collection.Insert(index, value);

            return true;

        }

        public static T[] InsertRangeIfNotContains<T>(this IList<T> collection, int index, params T[] values) => collection.InsertRangeIfNotContains(index, (IEnumerable<T>)values);

        public static T[] InsertRangeIfNotContains<T>(this IList<T> collection, int index, IEnumerable<T> values)

        {

            ArrayBuilder<T> addedValues = new ArrayBuilder<T>();

            foreach (T value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Insert(index, value);

                addedValues.AddLast(value);

            }

            return addedValues.ToArray();

        }

        #endregion

        #region Remove(Range)IfContains methods

        public static bool RemoveIfContains(this IList collection, object value)

        {

            if (collection.Contains(value))

            {

                collection.Remove(value);

                return true;

            }

            return false;

        }

        public static object[] RemoveRangeIfContains(this IList collection, params object[] values) => collection.RemoveRangeIfContains((IEnumerable)values);

        public static object[] RemoveRangeIfContains(this IList collection, IEnumerable values)

        {

            ArrayBuilder<object> removedValues = new ArrayBuilder<object>();

            foreach (object value in values)

                if (collection.Contains(value))

                {

                    // todo: RemoveAt()

                    collection.Remove(value);

                    removedValues.AddLast(value);

                }

            return removedValues.ToArray();

        }



        public static bool RemoveIfContains<T>(this ICollection<T> collection, T value)

        {

            if (collection.Contains(value))

            {

                collection.Remove(value);

                return true;

            }

            return false;

        }

        public static T[] RemoveRangeIfContains<T>(this ICollection<T> collection, params T[] values) => collection.RemoveRangeIfContains((IEnumerable<T>)values);

        public static T[] RemoveRangeIfContains<T>(this ICollection<T> collection, IEnumerable<T> values)

        {

            ArrayBuilder<T> removedValues = new ArrayBuilder<T>();

            foreach (T value in values)

                if (collection.Contains(value))

                {

                    // todo: RemoveAt()

                    collection.Remove(value);

                    removedValues.AddLast(value);

                }

            return removedValues.ToArray();

        }

        #endregion

        #region AddRange methods

        public static void AddRange(this IList collection, params object[] values) => collection.AddRange((IEnumerable)values);

        public static void AddRange(this IList collection, IEnumerable array)

        {

            foreach (object item in array)

                collection.Add(item);

        }

        public static void AddRange(this IList collection, int start, int length, params object[] values)

        {

            for (int i = start; i < length; i++)

                collection.Add(values[i]);

        }

        // todo: to add a version of the methods like this one with a 'contains' check:

        public static void AddRange(this IList collection, IList values, int start, int length)

        {

            for (int i = start; i < length; i++)

                collection.Add(values[i]);

        }

        public static void AddRange(this IList collection, IEnumerable array, int start, int length) => collection.AddRange(array.ToArray(), start, length);



        public static void AddRange<T>(this ICollection<T> collection, params T[] values) => collection.AddRange((IEnumerable<T>)values);

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> array)

        {

            foreach (T item in array)

                collection.Add(item);

        }

        public static void AddRange<T>(this ICollection<T> collection, int start, int length, params T[] values)

        {

            for (int i = start; i < length; i++)

                collection.Add(values[i]);

        }

        public static void AddRange<T>(this ICollection<T> collection, IList<T> values, int start, int length)

        {

            for (int i = start; i < length; i++)

                collection.Add(values[i]);

        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> array, int start, int length) => collection.AddRange(array.ToArray<T>(), start, length);



        public static LinkedListNode<T>[] AddRangeFirst<T>(this LinkedList<T> collection, params T[] values)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            if (values.Length != 0)

            {

                LinkedListNode<T> node = collection.AddFirst(values[0]);

                result.AddLast(node);

                for (int i = 1; i < values.Length; i++)

                {

                    node = collection.AddAfter(node, values[i]);

                    result.AddLast(node);

                }

            }

            return result.ToArray<LinkedListNode<T>>();

        }

        public static LinkedListNode<T>[] AddRangeFirst<T>(this LinkedList<T> collection, IEnumerable<T> array) => collection.AddRangeFirst(array as T[] ?? array.ToArray<T>());

        public static void AddRangeFirst<T>(this LinkedList<T> collection, params LinkedListNode<T>[] values)

        {

            if (values.Length == 0) return;

            LinkedListNode<T> node = values[0];

            collection.AddFirst(node);

            for (int i = 1; i < values.Length; i++)

            {

                collection.AddAfter(node, values[i]);

                node = values[i];

            }

        }

        public static void AddRangeFirst<T>(this LinkedList<T> collection, IEnumerable<LinkedListNode<T>> array) => collection.AddRangeFirst(array as LinkedListNode<T>[] ?? array.ToArray<LinkedListNode<T>>());

        public static LinkedListNode<T>[] AddRangeLast<T>(this LinkedList<T> collection, params T[] values) => collection.AddRangeLast((IEnumerable<T>)values);

        public static LinkedListNode<T>[] AddRangeLast<T>(this LinkedList<T> collection, IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                result.AddLast(collection.AddLast(item));

            return result.ToArray<LinkedListNode<T>>();

        }

        public static void AddRangeLast<T>(this LinkedList<T> collection, params LinkedListNode<T>[] values) => collection.AddRangeLast((IEnumerable<LinkedListNode<T>>)values);

        public static void AddRangeLast<T>(this LinkedList<T> collection, IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                collection.AddLast(item);

        }

        public static LinkedListNode<T>[] AddRangeBefore<T>(this LinkedList<T> collection, LinkedListNode<T> node, params T[] values) => collection.AddRangeBefore(node, (IEnumerable<T>)values);

        public static LinkedListNode<T>[] AddRangeBefore<T>(this LinkedList<T> collection, LinkedListNode<T> node, IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                result.AddLast(collection.AddBefore(node, item));

            return result.ToArray<LinkedListNode<T>>();

        }

        public static void AddRangeBefore<T>(this LinkedList<T> collection, LinkedListNode<T> node, params LinkedListNode<T>[] values) => collection.AddRangeBefore(node, (IEnumerable<LinkedListNode<T>>)values);

        public static void AddRangeBefore<T>(this LinkedList<T> collection, LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                collection.AddBefore(node, item);

        }

        public static LinkedListNode<T>[] AddRangeAfter<T>(this LinkedList<T> collection, LinkedListNode<T> node, params T[] values) => collection.AddRangeAfter(node, (IEnumerable<T>)values);

        public static LinkedListNode<T>[] AddRangeAfter<T>(this LinkedList<T> collection, LinkedListNode<T> node, IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            LinkedListNode<T> _node = node;

            foreach (T item in array)

            {

                _node = collection.AddAfter(_node, item);

                result.AddLast(_node);

            }

            return result.ToArray<LinkedListNode<T>>();

        }

        public static void AddRangeAfter<T>(this LinkedList<T> collection, LinkedListNode<T> node, params LinkedListNode<T>[] values) => collection.AddRangeAfter(node, (IEnumerable<LinkedListNode<T>>)values);

        public static void AddRangeAfter<T>(this LinkedList<T> collection, LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            LinkedListNode<T> _node = node;

            foreach (LinkedListNode<T> item in array)

            {

                collection.AddAfter(_node, item);

                _node = item;

            }

        }

        #endregion

        public static ArrayList ToList(this IEnumerable array) => array.ToList(0, null);

        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to an <see cref="ArrayList"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="ArrayList"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="ArrayList"/>.</returns>
        public static ArrayList ToList(this IEnumerable array, int startIndex, int? length)

        {

            int i = 0;

            if (length == null)

            {

                ArrayBuilder<object> arrayBuilder = new ArrayBuilder<object>();

                foreach (object value in array)

                    if (i < startIndex) i++;

                    else // We don't need to increment i anymore when we are here

                        arrayBuilder.AddLast(value);

                return arrayBuilder.ToArrayList();

            }

            else

            {

                ArrayList arrayList = new ArrayList(length.Value);

                int count = 0;

                foreach (object value in array)

                {

                    if (i < startIndex)

                        i++;

                    else

                    {

                        arrayList.Add(value);

                        count++;

                    }

                    if (count == length)

                        break;

                }

                return arrayList;

            }

        }

        //public static List<T> ToList<T>(this IEnumerable<T> array)

        //{

        //    List<T> arrayList = new List<T>();

        //    foreach (T value in array)

        //        arrayList.Add(value);

        //    return arrayList;

        //}

        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to a <see cref="List{T}"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="List{T}"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="List{T}"/>.</returns>
        public static List<T> ToList<T>(this IEnumerable<T> array, int startIndex, int? length)

        {

            int i = 0;

            if (length == null)

            {

                ArrayBuilder<T> arrayBuilder = new ArrayBuilder<T>();

                foreach (T value in array)

                    if (i < startIndex) i++;

                    else    // We don't need to increment i anymore when we are here

                        arrayBuilder.AddLast(value);

                return arrayBuilder.ToList();

            }

            else

            {

                List<T> arrayList = new List<T>(length.Value);

                int count = 0;

                foreach (T value in array)

                {

                    if (i < startIndex)

                        i++;

                    else

                    {

                        arrayList.Add(value);

                        count++;

                    }

                    if (count == length)

                        break;

                }

                return arrayList;

            }

        }

        public static object[] ToArray(this IEnumerable array)

        {

            LinkedList<object> _array = new LinkedList<object>();

            foreach (object value in array)

                _array.AddLast(value);

            return _array.ToArray<object>();

        }

        //public static T[] ToArray<T>(this IEnumerable<T> array)

        //{

        //    T[] _array = new T[length];

        //    int i = 0;

        //    int count = 0;

        //    foreach (T value in array)

        //    {

        //        if (i < startIndex)

        //            i++;

        //        else

        //            _array[count++] = value;

        //        if (count == length)

        //            break;

        //    }

        //    return _array;

        //}

        public static object[] ToArray(this IEnumerable array, int startIndex, int length)

        {

            object[] _array = new object[length];

            int i = 0;

            int count = 0;

            foreach (object value in array)

            {

                if (i < startIndex)

                    i++;

                else

                    _array[count++] = value;

                if (count == length)

                    break;

            }

            return _array;

        }

        public static T[] ToArray<T>(this IEnumerable<T> array, int startIndex, int length)

        {

            T[] _array = new T[length];

            int i = 0;

            int count = 0;

            foreach (T value in array)

            {

                if (i < startIndex)

                    i++;

                else

                    _array[count++] = value;

                if (count == length)

                    break;

            }

            return _array;

        }

        public static ArrayList ToList(this object[] array, int startIndex, int length)

        {

            ArrayList arrayList = new ArrayList(length);

            int count = startIndex + length;

            int i;

            for (i = startIndex; i < count; i++)

                arrayList.Add(array[i]);

            return arrayList;

        }

        public static List<T> ToList<T>(this T[] array, int startIndex, int length)

        {

            List<T> arrayList = new List<T>(length);

            int count = startIndex + length;

            int i;

            for (i = startIndex; i < count; i++)

                arrayList.Add(array[i]);

            return arrayList;

        }

        public static object[] ToArray(this IList arrayList, int startIndex, int length)

        {

            object[] array = new object[length];

            int i;

            for (i = 0; i < length; i++)

                array[i] = arrayList[i + startIndex];

            return array;

        }

        public static T[] ToArray<T>(this IList<T> arrayList, int startIndex, int length)

        {

            T[] array = new T[length];

            int i;

            for (i = 0; i < length; i++)

                array[i] = arrayList[i + startIndex];

            return array;

        }

        // todo: to add null checks, out-of-range checks, ...

        /// <summary>
        /// Removes multiple items in an <see cref="ICollection"/> collection, from a given start index for a given length.
        /// </summary>
        /// <param name="collection">The collection from which remove the items.</param>
        /// <param name="start">The start index in the collection from which delete the items.</param>
        /// <param name="length">The length to remove.</param>
        public static void RemoveRange(this IList collection, int start, int length)

        {

            for (int i = 0; i < length; i++)

                collection.RemoveAt(start);

        }

        /// <summary>
        /// Appends data to the table. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static object[] Append(this Array array, params Array[] arrays) => Util.Concatenate((object[])array, arrays);

        /// <summary>
        /// Appends data to the table using the <see cref="Array.LongLength"/> length property. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static object[] AppendLong(this Array array, params Array[] arrays) => Util.ConcatenateLong((object[])array, arrays);

        /// <summary>
        /// Sorts an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        public static void Sort<T>(this ObservableCollection<T> oc)

        {

            IList<T> sorted = oc.OrderBy(x => x).ToList<T>();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);

        }

        /// <summary>
        /// Sorts an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> with a user-defined comparer.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> providing comparison for sorting the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</param>
        public static void Sort<T>(this ObservableCollection<T> oc, IComparer<T> comparer)

        {

            IList<T> sorted = oc.OrderBy(x => x, comparer).ToList<T>();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);

        }

        #region Contains methods

        #region Non generic methods

        #region ContainsOneValue overloads

        private static bool ContainsOneValue(IEnumerable array, Func<object, object, bool> comparisonDelegate, out bool containsMoreThanOneValue, object[] values)

        {

            bool matchFound = false;

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        if (matchFound)

                        {

                            containsMoreThanOneValue = true;

                            return false;

                        }

                        matchFound = true;

                    }

            containsMoreThanOneValue = false;

            return matchFound;

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, out bool containsMoreThanOneValue, params object[] values) => ContainsOneValue(array, (object value, object _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, IComparer comparer, out bool containsMoreThanOneValue, params object[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneValue(array, (object value, object _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, Comparison<object> comparison, out bool containsMoreThanOneValue, params object[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneValue(array, (object value, object _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom equality comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, IEqualityComparer equalityComparer, out bool containsMoreThanOneValue, params object[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneValue(array, (object value, object _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values); ;

        }

        #endregion

        #region ContainsOneOrMoreValues with notification whether contains more than one values overloads

        private static bool ContainsOneOrMoreValues(IEnumerable array, Func<object, object, bool> comparisonDelegate, out bool containsMoreThanOneValue, object[] values)

        {

            bool matchFound = false;

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        if (matchFound)

                        {

                            containsMoreThanOneValue = true;

                            return true;

                        }

                        matchFound = true;

                    }

            containsMoreThanOneValue = false;

            return matchFound;

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, out bool containsMoreThanOneValue, params object[] values) => ContainsOneOrMoreValues(array, (object value, object _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IComparer comparer, out bool containsMoreThanOneValue, params object[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, Comparison<object> comparison, out bool containsMoreThanOneValue, params object[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom equality comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IEqualityComparer equalityComparer, out bool containsMoreThanOneValue, params object[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values);

        }

        #endregion

        #region ContainsOneOrMoreValues without notification whether contains more than one values overloads

        private static bool ContainsOneOrMoreValues(IEnumerable array, Func<object, object, bool> comparisonDelegate, object[] values)

        {

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparisonDelegate(value, _value))

                        return true;

            return false;

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, params object[] values) => ContainsOneOrMoreValues(array, (object value, object _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IComparer comparer, params object[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparer.Compare(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, Comparison<object> comparison, params object[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparison(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IEqualityComparer equalityComparer, params object[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => equalityComparer.Equals(value, _value), values);

        }

        #endregion

        #region Contains array overloads

        private static bool Contains(IEnumerable array, Func<object, object, bool> comparisonDelegate, object[] values)

        {

            bool matchFound;

            foreach (object value in array)

            {

                matchFound = false;

                foreach (object _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        matchFound = true;

                        break;

                    }

                if (!matchFound)

                    return false;

            }

            return true;

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, params object[] values) => Contains(array, (object value, object _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, IComparer comparer, params object[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return Contains(array, (object value, object _value) => comparer.Compare(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, Comparison<object> comparison, params object[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return Contains(array, (object value, object _value) => comparison(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, IEqualityComparer equalityComparer, params object[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return Contains(array, (object value, object _value) => equalityComparer.Equals(value, _value), values);

        }

        #endregion

        #endregion

        #region Generic methods

        #region ContainsOneValue overloads

        private static bool ContainsOneValue<T>(IEnumerable<T> array, Func<T, T, bool> comparisonDelegate, out bool containsMoreThanOneValue, T[] values)

        {

            bool matchFound = false;

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        if (matchFound)

                        {

                            containsMoreThanOneValue = true;

                            return false;

                        }

                        matchFound = true;

                    }

            containsMoreThanOneValue = false;

            return matchFound;

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this IEnumerable<T> array, out bool containsMoreThanOneValue, params T[] values) => ContainsOneValue(array, (T value, T _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this IEnumerable<T> array, IComparer<T> comparer, out bool containsMoreThanOneValue, params T[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneValue(array, (T value, T _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this IEnumerable<T> array, Comparison<T> comparison, out bool containsMoreThanOneValue, params T[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneValue(array, (T value, T _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this IEnumerable<T> array, IEqualityComparer<T> equalityComparer, out bool containsMoreThanOneValue, params T[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneValue(array, (T value, T _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values); ;

        }

        #endregion

        #region ContainsOneOrMoreValues with notification whether contains more than one values overloads

        private static bool ContainsOneOrMoreValues<T>(IEnumerable<T> array, Func<T, T, bool> comparisonDelegate, out bool containsMoreThanOneValue, T[] values)

        {

            bool matchFound = false;

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        if (matchFound)

                        {

                            containsMoreThanOneValue = true;

                            return true;

                        }

                        matchFound = true;

                    }

            containsMoreThanOneValue = false;

            return matchFound;

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, out bool containsMoreThanOneValue, params T[] values) => ContainsOneOrMoreValues(array, (T value, T _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, IComparer<T> comparer, out bool containsMoreThanOneValue, params T[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, Comparison<T> comparison, out bool containsMoreThanOneValue, params T[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, IEqualityComparer<T> equalityComparer, out bool containsMoreThanOneValue, params T[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values);

        }

        #endregion

        #region ContainsOneOrMoreValues without notification whether contains more than one values overloads

        private static bool ContainsOneOrMoreValues<T>(IEnumerable<T> array, Func<T, T, bool> comparisonDelegate, T[] values)

        {

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparisonDelegate(value, _value))

                        return true;

            return false;

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, params T[] values) => ContainsOneOrMoreValues(array, (T value, T _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, IComparer<T> comparer, params T[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparer.Compare(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, Comparison<T> comparison, params T[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparison(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this IEnumerable<T> array, IEqualityComparer<T> equalityComparer, params T[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => equalityComparer.Equals(value, _value), values);

        }

        #endregion

        #region Contains array overloads

        private static bool Contains<T>(IEnumerable<T> array, Func<T, T, bool> comparisonDelegate, T[] values)

        {

            bool matchFound;

            foreach (T value in array)

            {

                matchFound = false;

                foreach (T _value in values)

                    if (comparisonDelegate(value, _value))

                    {

                        matchFound = true;

                        break;

                    }

                if (!matchFound)

                    return false;

            }

            return true;

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this IEnumerable<T> array, params T[] values) => Contains(array, (T value, T _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this IEnumerable<T> array, IComparer<T> comparer, params T[] values)

        {

            if (comparer == null)

                throw new ArgumentNullException(nameof(comparer));

            return Contains(array, (T value, T _value) => comparer.Compare(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this IEnumerable<T> array, Comparison<T> comparison, params T[] values)

        {

            if (comparison == null)

                throw new ArgumentNullException(nameof(comparison));

            return Contains(array, (T value, T _value) => comparison(value, _value) == 0, values);

        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this IEnumerable<T> array, IEqualityComparer<T> equalityComparer, params T[] values)

        {

            if (equalityComparer == null)

                throw new ArgumentNullException(nameof(equalityComparer));

            return Contains(array, (T value, T _value) => equalityComparer.Equals(value, _value), values);

        }

        #endregion

        #endregion

        #endregion

        public static string ToString(this IEnumerable array, bool parseSubEnumerables, bool parseStrings = false)

        {

            StringBuilder result = new StringBuilder();

            foreach (object value in array)

                result.Append($"{{{ ((value is string && parseStrings) || (!(value is string) && value is IEnumerable && parseSubEnumerables) ? ((IEnumerable)value).ToString(true) : value?.ToString())}}}, ");

            return result.ToString(0, result.Length - 2);

        }

        #endregion

        /// <summary>
        /// Checks if the current object is assignable from at least one type of a given <see cref="Type"/> array.
        /// </summary>
        /// <param name="obj">The object from which check the type</param>
        /// <param name="typeEquality"><see langword="true"/> to preserve type equality, regardless of the type inheritance, otherwise <see langword="false"/></param>
        /// <param name="types">The types to compare</param>
        /// <returns><see langword="true"/> if the current object is assignable from at least one of the given types, otherwise <see langword="false"/>.</returns>
        public static bool Is(this object obj, bool typeEquality, params Type[] types)

        {

            Type objType = obj.GetType();

            foreach (Type type in types)

                if (typeEquality ? objType == type : objType.IsAssignableFrom(type))

                    return true;

            return false;

        }

        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Key;

        }

        public static IEnumerable<TValue> GetValues<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Value;

        }

        public static bool CheckIntegrity<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            bool predicateByVal(TKey keyA, TKey keyB) => Equals(keyA, keyB);

            bool predicateByRef(TKey keyA, TKey keyB) => ReferenceEquals(keyA, keyB);

            Func<TKey, TKey, bool> predicate = typeof(TKey).IsClass ? predicateByRef : (Func<TKey, TKey, bool>)predicateByVal;

            IEnumerable<TKey> keys = array.GetKeys();

            IEnumerable<TKey> _keys = array.GetKeys();

            bool foundOneOccurrence = false;

            foreach (TKey key in keys)

            {

                if (key == null)

                    throw new ArgumentException("One or more key is null.");

                foreach (TKey _key in _keys)

                {

                    if (predicate(key, _key))

                        if (foundOneOccurrence)

                            return false;

                        else

                            foundOneOccurrence = true;

                }

                foundOneOccurrence = false;

            }

            return true;

        }

        public static bool CheckPropertySetIntegrity(Type propertyObjectType, string propertyName, out string methodName, int skipFramesForStackFrame, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet)

        {

            PropertyInfo property = propertyObjectType.GetProperty(propertyName, bindingFlags);

            if (property == null)

                throw new ArgumentException(string.Format(FieldOrPropertyNotFound, propertyName, propertyObjectType));

            MethodBase method = new StackFrame(skipFramesForStackFrame).GetMethod();

            methodName = method.Name;

            //#if DEBUG 

            //            Debug.WriteLine("Property: " + property.Name + ", " + property.DeclaringType);

            //            Debug.WriteLine("Method: " + method.Name + ", " + method.DeclaringType);

            //#endif 

            return (property.CanWrite && property.GetSetMethod() != null) || property.DeclaringType == method.DeclaringType;

        }

        internal static FieldInfo GetField(string fieldName, Type objectType, BindingFlags bindingFlags)

        {

            BindingFlags flags = bindingFlags;

            // var objectType = obj.GetType(); 

            FieldInfo field = objectType.GetField(fieldName, flags);

            if (field == null)

                throw new ArgumentException(string.Format(FieldOrPropertyNotFound, fieldName, objectType));

            return field;

        }

        internal static PropertyInfo GetProperty(string fieldName, Type objectType, BindingFlags bindingFlags)

        {

            BindingFlags flags = bindingFlags;

            // var objectType = obj.GetType(); 

            PropertyInfo property = objectType.GetProperty(fieldName, flags);

            if (property == null)

                throw new ArgumentException(string.Format(FieldOrPropertyNotFound, fieldName, objectType));

            return property;

        }

        public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet)

        {

            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
            //             BindingFlags.Static | BindingFlags.Instance |
            //             BindingFlags.DeclaredOnly;
            //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

            // if (declaringType == null) 

            // {

            //while (objectType != declaringType && objectType != typeof(object))

            //    objectType = objectType.BaseType;

            //if (objectType != declaringType)

            //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

            // }

            //#if DEBUG

            //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //            foreach (var _field in fields)

            //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

            //#endif

            string methodName;

            // var objectType = obj.GetType();

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            object previousValue = field.GetValue(obj);

            if (performIntegrityCheck && !CheckPropertySetIntegrity(declaringType, propertyName, out methodName, 3, bindingFlags))

                throw new InvalidOperationException(string.Format(DeclaringTypesNotCorrespond, propertyName, methodName));

            if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))

            {

                field.SetValue(obj, newValue);

                return (true, previousValue);

                //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                //             BindingFlags.Static | BindingFlags.Instance |
                //             BindingFlags.DeclaredOnly;
                //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            }

            else

                return (false, previousValue);

        }

        public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet)

        {

            string methodName;

            PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

            object previousValue = property.GetValue(obj);

            if (performIntegrityCheck && !CheckPropertySetIntegrity(declaringType, propertyName, out methodName, 3, bindingFlags))

                throw new InvalidOperationException(string.Format(DeclaringTypesNotCorrespond, propertyName, methodName));

            if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))

            {

                property.SetValue(obj, newValue);

                return (true, previousValue);

            }

            else

                return (false, previousValue);

        }

        public static object GetNumValue(this Enum @enum, string enumName) => Convert.ChangeType(@enum.GetType().GetField(enumName).GetValue(@enum), Enum.GetUnderlyingType(@enum.GetType()));

        // public static object GetNumValue(this Enum @enum) => GetNumValue(@enum, @enum.ToString());

        // todo : to test if Math.Log(Convert.ToInt64(flagsEnum), 2) == 'SomeInt64'; (no float, double ...) would be faster.

        public static bool HasMultipleFlags(this Enum flagsEnum)

        {

            Type type = flagsEnum.GetType();

            if (type.GetCustomAttributes(typeof(FlagsAttribute)).Count() == 0)

                return false; // throw new ArgumentException(string.Format("This enum does not implement the {0} attribute.", typeof(FlagsAttribute).Name));



            bool alreadyFoundAFlag = false;

            Enum enumValue;

            // FieldInfo field = null;



            foreach (string s in type.GetEnumNames())

            {

                enumValue = (Enum)Enum.Parse(type, s);



                if (enumValue.GetNumValue(s).Equals(0)) continue;



                if (flagsEnum.HasFlag(enumValue))

                    if (!alreadyFoundAFlag) alreadyFoundAFlag = true;

                    else return true;

            }

            return false;

        }

        /// <summary>
        /// Determines whether the current enum value is within the enum values range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
        public static bool IsValidEnumValue(this Enum @enum)

        {

            ArrayList values = new ArrayList(@enum.GetType().GetEnumValues());

            values.Sort();

            // object _value = Convert.ChangeType(value, value.GetType().GetEnumUnderlyingType());

            return @enum.CompareTo(values[0]) >= 0 && @enum.CompareTo(values[values.Count - 1]) <= 0;

        }

        public static void ThrowIfNotValidEnumValue(this Enum @enum)

        {

            if (!@enum.IsValidEnumValue()) throw new ArgumentException(string.Format(InvalidEnumValue, @enum.ToString()));

        }

        //public static ImageSource ToImageSource(this Icon icon)

        //{

        //    IntPtr hIcon = icon.Handle;

        //    BitmapSource wpfIcon = Imaging.CreateBitmapSourceFromHIcon(
        //        hIcon,
        //        Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    //if (!Util.DeleteObject(hIcon))

        //    //    throw new Win32Exception();

        //    //using (MemoryStream memoryStream = new MemoryStream())

        //    //{

        //    //    icon.ToBitmap().Save(memoryStream, ImageFormat.Png);

        //    //    IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

        //    //    return (ImageSource) new ImageSourceConverter().ConvertFrom( iconBitmapDecoder);

        //    //}

        //    ImageSource imageSource;

        //    // Icon icon = Icon.ExtractAssociatedIcon(path);

        //    using (Bitmap bmp = icon.ToBitmap())
        //    {
        //        var stream = new MemoryStream();
        //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        imageSource = BitmapFrame.Create(stream);
        //    }

        //    return imageSource;

        //    return icon.ToBitmap().ToImageSource();

        //    return wpfIcon;

        //}

        public static ImageSource ToImageSource(this Bitmap bitmap)

        {

            bitmap.MakeTransparent();

            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!Microsoft.WindowsAPICodePack.Win32Native.Shell.ShellNativeMethods.DeleteObject(hBitmap))

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            //using (MemoryStream stream = new MemoryStream())
            //{
            //    bitmap.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

            //    stream.Position = 0;
            //    BitmapImage result = new BitmapImage();
            //    result.BeginInit();
            //    // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
            //    // Force the bitmap to load right now so we can dispose the stream.
            //    result.CacheOption = BitmapCacheOption.OnLoad;
            //    result.StreamSource = stream;
            //    result.EndInit();
            //    result.Freeze();
            //    return result;
            //}

            return wpfBitmap;

        }

        public static bool Between(this int i, int start, int length) => i >= start && i <= length;

        public static void Execute(this ICommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command is RoutedCommand)

                ((RoutedCommand)command).Execute(commandParameter, commandTarget);

            else

                command.Execute(commandParameter);

        }

        public static bool TryExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand
                ? ((RoutedCommand)command).TryExecute(commandParameter, commandTarget)
                : command.TryExecute(commandParameter);

        /// <summary>
        /// Check if the command can be executed, and executes it if available. See the remarks section.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="commandParameter">The parameter of your command.</param>
        /// <remarks>
        /// This method only evaluates the commands of the common <see cref="ICommand"/> type. To evaluate a command of the <see cref="RoutedCommand"/> type, consider using the <see cref="TryExecute(RoutedCommand, object, IInputElement)"/> method. If you are not sure of the type of your command, so consider using the <see cref="TryExecute(ICommand, object, IInputElement)"/> method.
        /// </remarks>
        public static bool TryExecute(this ICommand command, object commandParameter)

        {

            if (command != null && command.CanExecute(commandParameter))

            {

                command.Execute(commandParameter);

                return true;

            }

            return false;

        }

        public static bool TryExecute(this RoutedCommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command.CanExecute(commandParameter, commandTarget))

            {
                // try
                // {

                command.Execute(commandParameter, commandTarget);

                // }
                // catch (InvalidOperationException ex)
                // {

                // Debug.WriteLine(ex.Message);

                // }

                return true;

            }

            return false;

        }

        public static bool CanExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand routedCommand
                ? routedCommand.CanExecute(commandParameter, commandTarget)
                : command.CanExecute(commandParameter);

        ///// <summary>
        ///// Checks if an object is a <see cref="FrameworkElement.Parent"/> or a <see cref="FrameworkElement.TemplatedParent"/> of an another object.
        ///// </summary>
        ///// <param name="source">The source object</param>
        ///// <param name="obj">The object to search in</param>
        ///// <returns><see langword="true"/> if 'obj' is a parent of the current object, otherwise <see langword="false"/>.</returns>
        //public static bool IsParent(this DependencyObject source, FrameworkElement obj)

        //{

        //    DependencyObject parent = obj.Parent ?? obj.TemplatedParent;

        //    while (parent != null && parent is FrameworkElement)

        //    {

        //        if (parent == source)

        //            return true;

        //        parent = ((FrameworkElement)parent).Parent ?? ((FrameworkElement)parent).TemplatedParent;

        //    }

        //    return false;

        //}

        /// <summary>
        /// Searches for the first parent of an object which is assignable from a given type.
        /// </summary>
        /// <typeparam name="T">The type to search</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="typeEquality">Indicates whether to check for the exact type equality. <see langword="true"/> to only search for objects with same type than the given type, <see langword="false"/> to search for all objects of type for which the given type is assignable from.</param>
        /// <returns>The first object that was found, if any, otherwise null.</returns>
        public static T GetParent<T>(this DependencyObject source, bool typeEquality) where T : DependencyObject

        {

            Type type = typeof(T);

            //if (!typeof(DependencyObject).IsAssignableFrom(type))

            //    throw new InvalidOperationException($"The DependencyObject type must be assignable from the type parameter.");

            do

                source = (source is FrameworkElement frameworkElement ? frameworkElement.Parent ?? frameworkElement.TemplatedParent : null) ?? VisualTreeHelper.GetParent(source);

            while (source != null && (typeEquality ? source.GetType() != type : !type.IsAssignableFrom(source.GetType())));

            return (T)source;

        }

        // todo: add other methods and overloads for StringComparison, IEqualityComparer<char>, Comparer<char>, Comparison<char>, ignore case and CultureInfo parameters

        [Obsolete("This method has been replaced by the Contains(this string, string, IEqualityComparer<char>) method.")]
        public static bool Contains(this string s, IEqualityComparer<char> comparer, string value) => s.Contains(value, comparer);

        public static bool Contains(this string s, string value, IEqualityComparer<char> comparer)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i], value[j]))

                        break;

                    else if (j == value.Length - 1)

                        return true;

            return false;

        }

        [Obsolete("This method has been replaced by arrays-common methods.")]
        public static bool Contains(this string s, char value, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                if (s[i] == value)

                {

                    index = i;

                    return true;

                }

            index = default;

            return false;

        }

        public static bool Contains(this string s, string value, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (s[i] != value[j])

                        break;

                    else if (j == value.Length - 1)

                    {

                        index = i;

                        return true;

                    }

            index = default;

            return false;

        }

        [Obsolete("This method has been replaced by arrays-common methods.")]
        public static bool Contains(this string s, char value, IEqualityComparer<char> comparer, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                if (comparer.Equals(s[i], value))

                {

                    index = i;

                    return true;

                }

            index = default;

            return false;

        }

        public static bool Contains(this string s, string value, IEqualityComparer<char> comparer, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i], value[j]))

                        break;

                    else if (j == value.Length - 1)

                    {

                        index = i;

                        return true;

                    }

            index = default;

            return false;

        }

        public static bool StartsWith(this string s, char value) => s[0] == value;

    }
}
