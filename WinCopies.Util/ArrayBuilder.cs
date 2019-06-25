using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public class ArrayBuilder<T> : ISerializable, IDeserializationCallback
    {

        protected LinkedList<T> InnerList { get; }

        public ArrayBuilder() => InnerList = new LinkedList<T>();

        public ArrayBuilder(IEnumerable<T> enumerable) => InnerList = new LinkedList<T>(enumerable);

        protected ArrayBuilder(LinkedList<T> innerList) => InnerList = innerList;

        /// <summary>
        /// Gets the last node of the <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <returns>The last <see cref="System.Collections.Generic.LinkedListNode{T}"/> of the <see cref="System.Collections.Generic.LinkedList{T}"/>.</returns>
        public LinkedListNode<T> Last => InnerList.Last;

        //
        // Summary:
        //     Gets the first node of the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The first System.Collections.Generic.LinkedListNode`1 of the System.Collections.Generic.LinkedList`1.
        public LinkedListNode<T> First => InnerList.First;

        //
        // Summary:
        //     Gets the number of nodes actually contained in the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The number of nodes actually contained in the System.Collections.Generic.LinkedList`1.
        public int Count => InnerList.Count;

        //
        // Summary:
        //     Adds a new node containing the specified value after the specified existing node
        //     in the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The System.Collections.Generic.LinkedListNode`1 after which to insert a new System.Collections.Generic.LinkedListNode`1
        //     containing value.
        //
        //   value:
        //     The value to add to the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The new System.Collections.Generic.LinkedListNode`1 containing value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null.
        //
        //   T:System.InvalidOperationException:
        //     node is not in the current System.Collections.Generic.LinkedList`1.
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        //
        // Summary:
        //     Adds the specified new node after the specified existing node in the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The System.Collections.Generic.LinkedListNode`1 after which to insert newNode.
        //
        //   newNode:
        //     The new System.Collections.Generic.LinkedListNode`1 to add to the System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null. -or- newNode is null.
        //
        //   T:System.InvalidOperationException:
        //     node is not in the current System.Collections.Generic.LinkedList`1. -or- newNode
        //     belongs to another System.Collections.Generic.LinkedList`1.
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);
        //
        // Summary:
        //     Adds a new node containing the specified value before the specified existing
        //     node in the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The System.Collections.Generic.LinkedListNode`1 before which to insert a new
        //     System.Collections.Generic.LinkedListNode`1 containing value.
        //
        //   value:
        //     The value to add to the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The new System.Collections.Generic.LinkedListNode`1 containing value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null.
        //
        //   T:System.InvalidOperationException:
        //     node is not in the current System.Collections.Generic.LinkedList`1.
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);
        //
        // Summary:
        //     Adds the specified new node before the specified existing node in the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The System.Collections.Generic.LinkedListNode`1 before which to insert newNode.
        //
        //   newNode:
        //     The new System.Collections.Generic.LinkedListNode`1 to add to the System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null. -or- newNode is null.
        //
        //   T:System.InvalidOperationException:
        //     node is not in the current System.Collections.Generic.LinkedList`1. -or- newNode
        //     belongs to another System.Collections.Generic.LinkedList`1.
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);
        //
        // Summary:
        //     Adds a new node containing the specified value at the start of the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   value:
        //     The value to add at the start of the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The new System.Collections.Generic.LinkedListNode`1 containing value.
        public LinkedListNode<T> AddFirst(T value) => InnerList.AddFirst(value);
        //
        // Summary:
        //     Adds the specified new node at the start of the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The new System.Collections.Generic.LinkedListNode`1 to add at the start of the
        //     System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null.
        //
        //   T:System.InvalidOperationException:
        //     node belongs to another System.Collections.Generic.LinkedList`1.
        public void AddFirst(LinkedListNode<T> node) => InnerList.AddFirst(node);
        //
        // Summary:
        //     Adds a new node containing the specified value at the end of the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   value:
        //     The value to add at the end of the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The new System.Collections.Generic.LinkedListNode`1 containing value.
        public LinkedListNode<T> AddLast(T value) => InnerList.AddLast(value);
        //
        // Summary:
        //     Adds the specified new node at the end of the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The new System.Collections.Generic.LinkedListNode`1 to add at the end of the
        //     System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null.
        //
        //   T:System.InvalidOperationException:
        //     node belongs to another System.Collections.Generic.LinkedList`1.
        public void AddLast(LinkedListNode<T> node) => InnerList.AddLast(node);

        #region AddRange methods

        public LinkedListNode<T>[] AddRangeFirst(params T[] values)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            if (values.Length != 0)

            {

                LinkedListNode<T> node = AddFirst(values[0]);

                result.AddLast(node);

                for (int i = 1; i < values.Length; i++)

                {

                    node = AddAfter(node, values[i]);

                    result.AddLast(node);

                }

            }

            return result.ToArray<LinkedListNode<T>>();

        }

        public LinkedListNode<T>[] AddRangeFirst(IEnumerable<T> array) => AddRangeFirst(array as T[] ?? array.ToArray<T>());

        public void AddRangeFirst(params LinkedListNode<T>[] values)

        {

            if (values.Length == 0) return;

            LinkedListNode<T> node = values[0];

            AddFirst(node);

            for (int i = 1; i < values.Length; i++)

            {

                AddAfter(node, values[i]);

                node = values[i];

            }

        }

        public void AddRangeFirst(IEnumerable<LinkedListNode<T>> array) => AddRangeFirst(array as LinkedListNode<T>[] ?? array.ToArray<LinkedListNode<T>>());

        public LinkedListNode<T>[] AddRangeLast(params T[] values) => AddRangeLast((IEnumerable<T>)values);

        public LinkedListNode<T>[] AddRangeLast(IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                result.AddLast(AddLast(item));

            return result.ToArray<LinkedListNode<T>>();

        }

        public void AddRangeLast(params LinkedListNode<T>[] values) => AddRangeLast((IEnumerable<LinkedListNode<T>>)values);

        public void AddRangeLast(IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddLast(item);

        }

        public LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, params T[] values) => AddRangeBefore(node, (IEnumerable<T>)values);

        public LinkedListNode<T>[] AddRangeBefore(LinkedListNode<T> node, IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            foreach (T item in array)

                result.AddLast(AddBefore(node, item));

            return result.ToArray<LinkedListNode<T>>();

        }

        public void AddRangeBefore(LinkedListNode<T> node, params LinkedListNode<T>[] values) => AddRangeBefore(node, (IEnumerable<LinkedListNode<T>>)values);

        public void AddRangeBefore(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            foreach (LinkedListNode<T> item in array)

                AddBefore(node, item);

        }

        public LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, params T[] values) => AddRangeAfter(node, (IEnumerable<T>)values);

        public LinkedListNode<T>[] AddRangeAfter(LinkedListNode<T> node, IEnumerable<T> array)

        {

            LinkedList<LinkedListNode<T>> result = new LinkedList<LinkedListNode<T>>();

            LinkedListNode<T> _node = node;

            foreach (T item in array)

            {

                _node = AddAfter(_node, item);

                result.AddLast(_node);

            }

            return result.ToArray<LinkedListNode<T>>();

        }

        public void AddRangeAfter(LinkedListNode<T> node, params LinkedListNode<T>[] values) => AddRangeAfter(node, (IEnumerable<LinkedListNode<T>>)values);

        public void AddRangeAfter(LinkedListNode<T> node, IEnumerable<LinkedListNode<T>> array)

        {

            LinkedListNode<T> _node = node;

            foreach (LinkedListNode<T> item in array)

            {

                AddAfter(_node, item);

                _node = item;

            }

        }

        #endregion

        //
        // Summary:
        //     Removes all nodes from the System.Collections.Generic.LinkedList`1.
        public void Clear() => InnerList.Clear();
        //
        // Summary:
        //     Determines whether a value is in the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   value:
        //     The value to locate in the System.Collections.Generic.LinkedList`1. The value
        //     can be null for reference types.
        //
        // Returns:
        //     true if value is found in the System.Collections.Generic.LinkedList`1; otherwise,
        //     false.
        public bool Contains(T value) => InnerList.Contains(value);
        //
        // Summary:
        //     Copies the entire System.Collections.Generic.LinkedList`1 to a compatible one-dimensional
        //     System.Array, starting at the specified index of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements copied
        //     from System.Collections.Generic.LinkedList`1. The System.Array must have zero-based
        //     indexing.
        //
        //   index:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     array is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than zero.
        //
        //   T:System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.LinkedList`1
        //     is greater than the available space from index to the end of the destination
        //     array.
        public void CopyTo(T[] array, int index) => InnerList.Contains(array, index);
        //
        // Summary:
        //     Finds the first node that contains the specified value.
        //
        // Parameters:
        //   value:
        //     The value to locate in the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The first System.Collections.Generic.LinkedListNode`1 that contains the specified
        //     value, if found; otherwise, null.
        public LinkedListNode<T> Find(T value) => InnerList.Find(value);
        //
        // Summary:
        //     Finds the last node that contains the specified value.
        //
        // Parameters:
        //   value:
        //     The value to locate in the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     The last System.Collections.Generic.LinkedListNode`1 that contains the specified
        //     value, if found; otherwise, null.
        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);
        //
        // Summary:
        //     Returns an enumerator that iterates through the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     An System.Collections.Generic.LinkedList`1.Enumerator for the System.Collections.Generic.LinkedList`1.
        public LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();
        //
        // Summary:
        //     Implements the System.Runtime.Serialization.ISerializable interface and returns
        //     the data needed to serialize the System.Collections.Generic.LinkedList`1 instance.
        //
        // Parameters:
        //   info:
        //     A System.Runtime.Serialization.SerializationInfo object that contains the information
        //     required to serialize the System.Collections.Generic.LinkedList`1 instance.
        //
        //   context:
        //     A System.Runtime.Serialization.StreamingContext object that contains the source
        //     and destination of the serialized stream associated with the System.Collections.Generic.LinkedList`1
        //     instance.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     info is null.
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);
        //
        // Summary:
        //     Implements the System.Runtime.Serialization.ISerializable interface and raises
        //     the deserialization event when the deserialization is complete.
        //
        // Parameters:
        //   sender:
        //     The source of the deserialization event.
        //
        // Exceptions:
        //   T:System.Runtime.Serialization.SerializationException:
        //     The System.Runtime.Serialization.SerializationInfo object associated with the
        //     current System.Collections.Generic.LinkedList`1 instance is invalid.
        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
        //
        // Summary:
        //     Removes the first occurrence of the specified value from the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   value:
        //     The value to remove from the System.Collections.Generic.LinkedList`1.
        //
        // Returns:
        //     true if the element containing value is successfully removed; otherwise, false.
        //     This method also returns false if value was not found in the original System.Collections.Generic.LinkedList`1.
        public bool Remove(T value) => InnerList.Remove(value);
        //
        // Summary:
        //     Removes the specified node from the System.Collections.Generic.LinkedList`1.
        //
        // Parameters:
        //   node:
        //     The System.Collections.Generic.LinkedListNode`1 to remove from the System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     node is null.
        //
        //   T:System.InvalidOperationException:
        //     node is not in the current System.Collections.Generic.LinkedList`1.
        public void Remove(LinkedListNode<T> node) => InnerList.Remove(node);
        //
        // Summary:
        //     Removes the node at the start of the System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Collections.Generic.LinkedList`1 is empty.
        public void RemoveFirst() => InnerList.RemoveFirst();
        //
        // Summary:
        //     Removes the node at the end of the System.Collections.Generic.LinkedList`1.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The System.Collections.Generic.LinkedList`1 is empty.
        public void RemoveLast() => InnerList.RemoveLast();

        public T[] ToArray()

        {

            T[] result = new T[InnerList.Count];

            int i = 0;

            foreach (T item in InnerList)

                result[i++] = item;

            return result;

        }

        public ArrayList ToArrayList() => new ArrayList(ToArray());

        public List<T> ToList()

        {

            List<T> result = new List<T>(Count);

            foreach (T item in this)

                result.Add(item);

            return result;

        }

    }
}
