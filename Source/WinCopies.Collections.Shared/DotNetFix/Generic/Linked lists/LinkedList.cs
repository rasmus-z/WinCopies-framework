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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using WinCopies.Collections.Resources;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T> : System.Collections.Generic.LinkedList<T>, ILinkedList2<T>
    {
        // todo: remove the explicit interface implementation and make this property public.

        bool ILinkedList2<T>.IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
        /// </summary>
        public LinkedList() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public LinkedList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is serializable with the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        /// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream associated with the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        protected LinkedList(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

#endif
