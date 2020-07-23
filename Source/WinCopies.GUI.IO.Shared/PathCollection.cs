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

using System;
using System.Collections;
using System.Collections.Generic;
using WinCopies.IO;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.IO
{
    public abstract class PathCollection<T> : ICollection<T>, IList<T> where T : WinCopies.IO.IPathInfo
    {
        protected IList<T> InnerList { get; }

        public string Path { get; }

        public int Count => InnerList.Count;

        public bool IsReadOnly => false;

        protected virtual void SetItem(int index, T item)
        {
            ValidatePath(item);

            InnerList[index] = item;
        }

        public T this[int index] { get => InnerList[index]; set => SetItem(index, value); }

        protected abstract Func<T> GetNewEmptyEnumeratorPathInfoDelegate { get; }

        protected abstract Func<T, T> GetNewEnumeratorPathInfoDelegate { get; }

        public abstract Func<T, Size?> GetPathSizeDelegate { get; } 

        public string GetConcatenatedPath(WinCopies.IO.IPathInfo pathInfo) => pathInfo == null ? throw GetArgumentNullException(nameof(pathInfo)) : $"{Path}{WinCopies.IO.Path.PathSeparator}{pathInfo.Path}";

        protected PathCollection(string path) : this(path, new List<T>()) { }

        protected PathCollection(string path, IList<T> list)
        {
            Path = path == null || path.Length == 0 ? string.Empty : System.IO.Path.IsPathRooted(path) ? path : throw new ArgumentException($"{nameof(path)} must be null, empty or rooted.");

            ThrowIfNull(list, nameof(list));

            foreach (T _path in list)

                ValidatePath(_path);

            InnerList = list;
        }

        protected virtual void ValidatePath(T item)
        {
            if ((Path.Length > 1 && System.IO.Path.IsPathRooted(item.Path)) || (Path.Length == 0 && !System.IO.Path.IsPathRooted(item.Path)))

                throw new ArgumentException("The path to add must be relative.");
        }

        public void Add(T item) => InsertItem(Count, item);

        protected virtual void InsertItem(int index, T item)
        {
            ValidatePath(item);

            InnerList.Add(item);
        }

        public void Clear() => ClearItems();

        protected virtual void ClearItems() => InnerList.Clear();

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            int itemIndex = InnerList.IndexOf(item);

            if (itemIndex == -1)

                return false;

            RemoveItemAt(itemIndex);

            return true;
        }

        protected virtual void RemoveItemAt(int index) => InnerList.RemoveAt(index);

        public IEnumerator<T> GetEnumerator() => new PathCollectionEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => InnerList.IndexOf(item);

        public void Insert(int index, T item) => InsertItem(index, item);

        public void RemoveAt(int index) => InnerList.RemoveAt(index);

        public class PathCollectionEnumerator : WinCopies.Collections.Enumerator<T, T>
        {
            private PathCollection<T> _pathCollection;
            private bool _completed = false;

            public PathCollectionEnumerator(PathCollection<T> pathCollection) : base((pathCollection ?? throw GetArgumentNullException(nameof(pathCollection))).InnerList) => _pathCollection = pathCollection;

            protected override void Dispose(bool disposing)
            {
                _pathCollection = null;

                base.Dispose(disposing);
            }

            protected override bool MoveNextOverride()
            {
                if (_completed) return false;

                if (_pathCollection.Count == 0)
                {
                    Current = _pathCollection.GetNewEmptyEnumeratorPathInfoDelegate();

                    _completed = true;

                    return true;
                }

                if (InnerEnumerator.MoveNext())
                {
                    Current = _pathCollection.GetNewEnumeratorPathInfoDelegate(InnerEnumerator.Current);

                    return true;
                }

                _completed = true;

                return false;
            }
        }
    }
}
