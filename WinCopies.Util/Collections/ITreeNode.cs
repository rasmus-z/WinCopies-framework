using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.Collections
{

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface ITreeNode : IValueObject
    {

        /// <summary>
        /// Gets the parent of the current node.
        /// </summary>
        ITreeNode Parent { get; }

    }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface ITreeNode<T> : ITreeNode, IValueObject<T>

    {

    }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface ITreeNode<TValue, TItems> : ITreeNode<TValue>, System.Collections.Generic.ICollection<ITreeNode<TItems>> // where TNode : ITreeNode<TNode, TItem>
    {

        bool Contains(TItems item);

    }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface IReadOnlyTreeNode<TValue, TItems> : ITreeNode<TValue>, System.Collections.Generic.IReadOnlyCollection<ITreeNode<TItems>> // where TNode : ITreeNode<TNode, TItem>
    {

        bool Contains(TItems item);

    }

}
