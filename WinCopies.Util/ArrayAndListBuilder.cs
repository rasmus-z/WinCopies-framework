using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    /// <summary>
    /// Builds arrays and lists by sizing them only when required. This class can be used to initialize your array or list before adding or removing values to it.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public class ArrayAndListBuilder<T> : IEnumerable<T>, ISerializable, IDeserializationCallback
    {

        /// <summary>
        /// Gets the <see cref="LinkedList{T}"/> that is used to build the arrays and collections.
        /// </summary>
        protected LinkedList<T> InnerList { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAndListBuilder{T}"/> class.
        /// </summary>
        public ArrayAndListBuilder() => InnerList = new LinkedList<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAndListBuilder{T}"/> class with a given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable"></param>
        public ArrayAndListBuilder(IEnumerable<T> enumerable) => InnerList = new LinkedList<T>(enumerable);

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayAndListBuilder{T}"/> class using a custom <see cref="LinkedList{T}"/> to build the arrays and collections.
        /// </summary>
        /// <param name="innerList"></param>
        protected ArrayAndListBuilder(LinkedList<T> innerList) => InnerList = innerList;

        /// <summary>
        /// Gets the last node of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <returns>The last <see cref="LinkedListNode{T}"/> of the <see cref="LinkedList{T}"/>.</returns>
        public virtual LinkedListNode<T> Last => InnerList.Last;

        /// <summary>
        /// Gets the first node of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <returns>The first <see cref="LinkedListNode{T}"/> of the <see cref="LinkedList{T}"/>.</returns>
        public virtual LinkedListNode<T> First => InnerList.First;

        /// <summary>
        /// Gets the number of nodes actually contained in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <returns>The number of nodes actually contained in the <see cref="LinkedList{T}"/>.</returns>
        public virtual int Count => InnerList.Count;

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> after which to insert a new <see cref="LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="LinkedList{T}"/>.</exception>
        public virtual LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> after which to insert newNode.</param>
        /// <param name="newNode">The new <see cref="LinkedListNode{T}"/> to add to the <see cref="LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null. -or- newNode is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="LinkedList{T}"/>. -or- newNode belongs to another <see cref="LinkedList{T}"/>.</exception>
        public virtual void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> before which to insert a new <see cref="LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="LinkedList{T}"/>.</exception>
        public virtual LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> before which to insert newNode.</param>
        /// <param name="newNode">The new <see cref="LinkedListNode{T}"/> to add to the <see cref="LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null. -or- newNode is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="LinkedList{T}"/>. -or- newNode belongs to another <see cref="LinkedList{T}"/>.</exception>
        public virtual void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds a new node containing the specified value at the start of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        public virtual LinkedListNode<T> AddFirst(T value) => InnerList.AddFirst(value);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The new <see cref="LinkedListNode{T}"/> to add at the start of the <see cref="LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node belongs to another <see cref="LinkedList{T}"/>.</exception>
        public virtual void AddFirst(LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The new <see cref="LinkedListNode{T}"/> containing value.</returns>
        public virtual LinkedListNode<T> AddLast(T value) => InnerList.AddLast(value);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The new <see cref="LinkedListNode{T}"/> to add at the end of the <see cref="LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node belongs to another <see cref="LinkedList{T}"/>.</exception>
        public virtual void AddLast(LinkedListNode<T> node) => InnerList.AddLast(node);

        #region AddRange methods

        /// <summary>
        /// Add multiple values at the top of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="values">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeFirst(params T[] values) => InnerList.First == null ? AddRangeLast(values) : AddRangeBefore(InnerList.First, values);

        /// <summary>
        /// Add multiple values at the top of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeFirst(IEnumerable<T> array) => InnerList.First == null ? AddRangeLast(array) : AddRangeBefore(InnerList.First, array);

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the top of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeFirst(params LinkedListNode<T>[] nodes) { if (InnerList.First == null) AddRangeLast(nodes); else AddRangeBefore(InnerList.First, nodes); }

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the top of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeFirst(IEnumerable<LinkedListNode<T>> array) { if (InnerList.First == null) AddRangeLast(array); else AddRangeBefore(InnerList.First, array); }

        /// <summary>
        /// Add multiple values at the end of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="values">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeLast(params T[] values)
        {

            var result = new LinkedListNode<T>[values.Length];

            for (int i = 0; i < values.Length; i++)

                result[i] = AddLast(values[i]);

            return result;

        }

        /// <summary>
        /// Add multiple values at the end of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeLast(IEnumerable<T> array)

        {

            if (array is T[] _array) return AddRangeLast(_array);

            LinkedListNode<T>[] result;

            int i = 0;

            if (array is IList<T> __array)

            {

                result = new LinkedListNode<T>[__array.Count];

                for (; i < __array.Count; i++)

                    result[i] = AddLast(__array[i]);

                return result;

            }

            var values = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                _ = values.AddLast(AddLast(item));

            result = new LinkedListNode<T>[values.Count];

            foreach (LinkedListNode<T> item in values)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the end of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual void AddRangeLast(params LinkedListNode<T>[] nodes) => AddRangeLast((IEnumerable<LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple <see cref="LinkedListNode{T}"/>'s at the end of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="array">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeLast(IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddLast(item);

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="values">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, params T[] values)
        {

            var result = new LinkedListNode<T>[values.Length];

            for (int i = 0; i < values.Length; i++)

                result[i] = AddBefore(node, values[i]);

            return result;

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, IEnumerable<T> array)

        {

            var values = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                _ = values.AddLast(AddBefore(node, item));

            var result = new LinkedListNode<T>[values.Count];

            int i = 0;

            foreach (LinkedListNode<T> item in values)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="nodes">The <see cref="LinkedListNode{T}"/>'s to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeBefore(LinkedListNode<T> node, params LinkedListNode<T>[] nodes) => AddRangeBefore(node, (IEnumerable<LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeBefore(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddBefore(node, item);

        }

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="values">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, params T[] values) => node.Next == null ? AddRangeLast(values) : AddRangeBefore(node.Next, values);

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        /// <returns>The added <see cref="LinkedListNode{T}"/>'s.</returns>
        public virtual LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, IEnumerable<T> array) => node.Next == null ? AddRangeLast(array) : AddRangeBefore(node.Next, array);

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="nodes">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeAfter(LinkedListNode<T> node, params LinkedListNode<T>[] nodes) { if (node.Next == null) AddRangeLast(nodes); else AddRangeBefore(node.Next, nodes); }

        /// <summary>
        /// Add multiple values after a specified node in this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to this <see cref="ArrayAndListBuilder{T}"/></param>
        public virtual void AddRangeAfter(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array) { if (node.Next == null) AddRangeLast(array); else AddRangeBefore(node.Next, array); }

        #endregion

        /// <summary>
        /// Removes all nodes from the <see cref="LinkedList{T}"/>.
        /// </summary>
        public virtual void Clear() => InnerList.Clear();

        /// <summary>
        /// Determines whether a value is in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="LinkedList{T}"/>. The value can be null for reference types.</param>
        /// <returns>true if value is found in the <see cref="LinkedList{T}"/>; otherwise, false.</returns>
        public virtual bool Contains(T value) => InnerList.Contains(value);

        /// <summary>
        /// Copies the entire <see cref="LinkedList{T}"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="LinkedList{T}"/>. The System.Array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="LinkedList{T}"/> is greater than the available space from index to the end of the destination array.</exception>
        public virtual void CopyTo(T[] array, int index) => InnerList.Contains(array, index);

        /// <summary>
        /// Finds the first node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The first <see cref="LinkedListNode{T}"/> that contains the specified value, if found; otherwise, null.</returns>
        public virtual LinkedListNode<T> Find(T value) => InnerList.Find(value);

        /// <summary>
        /// Finds the last node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="LinkedList{T}"/>.</param>
        /// <returns>The last <see cref="LinkedListNode{T}"/> that contains the specified value, if found; otherwise, null.</returns>
        public virtual LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <returns>An <see cref="LinkedList{T}"/>.Enumerator for the <see cref="LinkedList{T}"/>.</returns>
        public virtual LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        /// <summary>
        /// Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize the <see cref="LinkedList{T}"/> instance.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> object that contains the information required to serialize the <see cref="LinkedList{T}"/> instance.</param>
        /// <param name="context">A <see cref="StreamingContext"/> object that contains the source and destination of the serialized stream associated with the <see cref="LinkedList{T}"/> instance.</param>
        /// <exception cref="ArgumentNullException">info is null.</exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        /// <summary>
        /// Implements the System.Runtime.Serialization.ISerializable interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="SerializationException">The <see cref="SerializationInfo"/> object associated with the current <see cref="LinkedList{T}"/> instance is invalid.</exception>
        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        /// <summary>
        /// Removes the first occurrence of the specified value from the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value to remove from the <see cref="LinkedList{T}"/>.</param>
        /// <returns><see langword="true"/> if the element containing value is successfully removed; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if value was not found in the original <see cref="LinkedList{T}"/>.</returns>
        public virtual bool Remove(T value) => InnerList.Remove(value);

        /// <summary>
        /// Removes the specified node from the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The <see cref="LinkedListNode{T}"/> to remove from the <see cref="LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        /// <exception cref="InvalidOperationException">node is not in the current <see cref="LinkedList{T}"/>.</exception>
        public virtual void Remove(LinkedListNode<T> node) => InnerList.Remove(node);

        /// <summary>
        /// Removes the node at the start of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="LinkedList{T}"/> is empty.</exception>
        public virtual void RemoveFirst() => InnerList.RemoveFirst();

        /// <summary>
        /// Removes the node at the end of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="LinkedList{T}"/> is empty.</exception>
        public virtual void RemoveLast() => InnerList.RemoveLast();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        /// <summary>
        /// Returns an array with the items of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <returns>An array with the items of this <see cref="ArrayAndListBuilder{T}"/>.</returns>
        public virtual T[] ToArray()

        {

            var result = new T[InnerList.Count];

            int i = 0;

            foreach (T item in InnerList)

                result[i++] = item;

            return result;

        }

        /// <summary>
        /// Returns an <see cref="ArrayList"/> with the items of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <returns>An <see cref="ArrayList"/> with the items of this <see cref="ArrayAndListBuilder{T}"/>.</returns>
        public virtual ArrayList ToArrayList()
        {

            var result = new ArrayList(InnerList.Count);

            foreach (T item in InnerList)

                _ = result.Add(item);

            return result;

        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> with the items of this <see cref="ArrayAndListBuilder{T}"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> with the items of this <see cref="ArrayAndListBuilder{T}"/>.</returns>
        public virtual List<T> ToList()

        {

            var result = new List<T>(Count);

            foreach (T item in InnerList)

                result.Add(item);

            return result;

        }
    }
}
