using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{

    /// <summary>
    /// Represents a value container. See the <see cref="IValueObject{T}"/> for a generic version of this class.
    /// </summary>
    public interface IValueObject : IEquatable<IValueObject>
    {

        /// <summary>
        /// Gets a value that indicates whether this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        object Value { get; set; }

    }

    /// <summary>
    /// Represents a value container. See the <see cref="IValueObject"/> for a non-generic version of this class.
    /// </summary>
    public interface IValueObject<T> : IValueObject, IEquatable<IValueObject<T>>
    {

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        new T Value { get; set; }

    }

    /// <summary>
    /// Represents a default comparer for <see cref="IValueObject"/>s.
    /// </summary>
    public sealed class ValueObjectEqualityComparer : IEqualityComparer<IValueObject>
    {

        /// <summary>
        /// Checks if two <see cref="IValueObject"/>s are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(IValueObject x, IValueObject y) => x is object && y is object ? EqualityComparer<object>.Default.Equals(x.Value, y.Value) : !(x is object || y is object);

        /// <summary>
        /// Returns the hash code for a given <see cref="IValueObject"/>. If <paramref name="obj"/> has a value, this function returns the hash code of <paramref name="obj"/>'s <see cref="IValueObject.Value"/>, otherwise this function returns the hash code of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The <see cref="IValueObject"/> for which to return the hash code.</param>
        /// <returns>The hash code of <paramref name="obj"/>'s <see cref="IValueObject.Value"/> if <paramref name="obj"/> has a value, otherwise the <paramref name="obj"/>'s hash code.</returns>
        public int GetHashCode(IValueObject obj) => (obj.Value is object ? obj.Value : obj).GetHashCode();

    }

    /// <summary>
    /// Represents a default comparer for <see cref="IValueObject{T}"/>s.
    /// </summary>
    public class ValueObjectEqualityComparer<T> : IEqualityComparer<IValueObject<T>>
    {

        /// <summary>
        /// Checks if two <see cref="IValueObject{T}"/>s are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(IValueObject<T> x, IValueObject<T> y) => x is object && y is object ? EqualityComparer<T>.Default.Equals(x.Value, y.Value) : !(x is object || y is object);

        /// <summary>
        /// Returns the hash code for a given <see cref="IValueObject{T}"/>. If <paramref name="obj"/> has a value, this function returns the hash code of <paramref name="obj"/>'s <see cref="IValueObject{T}.Value"/>, otherwise this function returns the hash code of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The <see cref="IValueObject{T}"/> for which to return the hash code.</param>
        /// <returns>The hash code of <paramref name="obj"/>'s <see cref="IValueObject{T}.Value"/> if <paramref name="obj"/> has a value, otherwise the <paramref name="obj"/>'s hash code.</returns>
        public int GetHashCode(IValueObject<T> obj) => obj.Value is object ? obj.Value.GetHashCode() : obj.GetHashCode();

    }

}
