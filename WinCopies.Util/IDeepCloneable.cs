using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{

    /// <summary>
    /// Provides a delegate to indicate to a method (e.g. a constructor) how to create a deep clone of an object or value. All Ids should be preserved, if any. See the Remarks section.
    /// </summary>
    /// <typeparam name="T">The type of the object to deep-clone.</typeparam>
    /// <param name="obj">The object or value to deep-clone. If this parameter is <see langword="null"/>, a new object is returned.</param>
    /// <returns>A deep clone of <paramref name="obj"/>.</returns>
    /// <remarks>This delegate can be used in constructors of classes or structures that implement the <see cref="IDeepCloneable"/> interface in order to pass an instance of this delegate to a parameter instead of passing directly an argument that needs reconstruction on a deep cloning operation.</remarks>
    /// <seealso cref="IDeepCloneable"/>
    public delegate T DeepClone<T>(T obj);

    /// <summary>
    /// Provides a delegate to indicate to a method (e.g. a constructor) how to create a deep clone of an object or value. See the Remarks section.
    /// </summary>
    /// <typeparam name="T">The type of the object to deep-clone.</typeparam>
    /// <param name="preserveIds">A <see cref="bool"/> value that indicates whether to preserve IDs, if any.</param>
    /// <param name="obj">The object or value to deep-clone. If this parameter is <see langword="null"/>, a new object is returned.</param>
    /// <returns>A deep clone of <paramref name="obj"/>.</returns>
    /// <remarks>This delegate can be used in constructors of classes or structures that implement the <see cref="IDeepCloneable"/> interface in order to pass an instance of this delegate to a parameter instead of passing directly an argument that needs reconstruction on a deep cloning operation.</remarks>
    /// <seealso cref="IDeepCloneable"/>
    public delegate T DeepCloneUsingIdsOptions<T>(T obj, bool preserveIds);

    /// <summary>
    /// Represents an object or a value that can be deep cloned. Note that if <see cref="NeedsObjectsOrValuesReconstruction"/> is <see langword="true"/>, the result of the <see cref="DeepClone"/> method might not be a complete deep-clone of the current object or value.
    /// </summary>
    /// <seealso cref="NeedsObjectsOrValuesReconstruction"/>
    public interface IDeepCloneable // This interface is not generic because if it was, we could specify a type that might not be a type available in the inheritance of the one that implemented the interface.
    {

        /// <summary>
        /// Gets a value that indicates whether the current object or value has to reconstruct objects or values on deep cloning. If the current object or value contains objects or values that all implement this interface, this property should be <see langword="true"/>, unless at least one of these objects or values contains itself one or more objects and/or values that does not implement this interface.
        /// </summary>
        bool NeedsObjectsOrValuesReconstruction { get; }

        /// <summary>
        /// Creates a deep clone of the current object or value.
        /// </summary>
        /// <returns>A deep clone of the current object or value.</returns>
        object DeepClone();

    }

    /// <summary>
    /// Represents an object or a value that can be deep cloned. Note that if <see cref="IDeepCloneable.NeedsObjectsOrValuesReconstruction"/> is <see langword="true"/>, the result of the <see cref="IDeepCloneable.DeepClone"/> <see cref="DeepClone(bool)"/> methods might not be a complete deep-clone of the current object or value.
    /// </summary>
    public interface IIDObjectDeepCloneable : IDeepCloneable // This interface is not generic because if it was, we could specify a type that might not be a type available in the inheritance of the one that implemented the interface.
    {

        /// <summary>
        /// Creates a deep clone of the current object or value.
        /// </summary>
        /// <param name="preserveIds">Whether to preserve IDs, if any.</param>
        /// <returns>A deep clone of the current object or value.</returns>
        object DeepClone(bool preserveIds);

    }

}
