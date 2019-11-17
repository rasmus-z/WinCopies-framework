/* Copyright © Pierre Sprimont, 2019
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public interface IObservableTreeNode<TValue, TItems> : ITreeNode<TValue, TItems>, System.Collections.Specialized.INotifyCollectionChanged, INotifyPropertyChanged
    {



    }

    public interface IReadOnlyObservableTreeNode<TValue, TItems> : IReadOnlyTreeNode<TValue, TItems>, System.Collections.Specialized.INotifyCollectionChanged, INotifyPropertyChanged
    {



    }

}
