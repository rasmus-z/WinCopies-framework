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
    public sealed class PathCollection : ICollection<WinCopies.IO.IPathInfo>, IList<WinCopies.IO.IPathInfo>
    {
        private readonly IList<WinCopies.IO.IPathInfo> _list;

        public string Path { get; }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public WinCopies.IO.IPathInfo this[int index]
        {
            get => _list[index]; set
            {
                ValidatePath(value);

                _list[index] = value;
            }
        }

        public PathCollection(string path) : this(path, new List<WinCopies.IO.IPathInfo>()) { }

        public PathCollection(string path, IList<WinCopies.IO.IPathInfo> list)
        {
            Path = path;

            foreach (WinCopies.IO.IPathInfo _path in list)

                ValidatePath(_path);

            _list = list;
        }

        private void ValidatePath(WinCopies.IO.IPathInfo item)
        {
            if (System.IO.Path.IsPathRooted(item.Path))

                throw new ArgumentException("The path to add must be relative.");
        }

        public void Add(WinCopies.IO.IPathInfo item)
        {
            ValidatePath(item);

            _list.Add(item);
        }

        public void Clear() => _list.Clear();

        public bool Contains(WinCopies.IO.IPathInfo item) => _list.Contains(item);

        public void CopyTo(WinCopies.IO.IPathInfo[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public bool Remove(WinCopies.IO.IPathInfo item) => _list.Remove(item);

        public IEnumerator<WinCopies.IO.IPathInfo> GetEnumerator() => new PathCollectionEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(WinCopies.IO.IPathInfo item) => _list.IndexOf(item);

        public void Insert(int index, WinCopies.IO.IPathInfo item)
        {
            ValidatePath(item);

            _list.Add(item);
        }

        public void RemoveAt(int index) => _list.RemoveAt(index);

        public class PathCollectionEnumerator : Enumerator<WinCopies.IO.IPathInfo, WinCopies.IO.IPathInfo>
        {
            private PathCollection _pathCollection;

            public PathCollectionEnumerator(PathCollection pathCollection) : base((pathCollection ?? throw GetArgumentNullException(nameof(pathCollection)))._list) => _pathCollection = pathCollection;

            protected override void Dispose(bool disposing)
            {
                _pathCollection = null;

                base.Dispose(disposing);
            }

            protected override bool MoveNextOverride()
            {
                if (InnerEnumerator.MoveNext())
                {
                    Current = new WinCopies.IO. PathInfo($"{_pathCollection.Path}{WinCopies.IO.Path.PathSeparator}{InnerEnumerator.Current.Path}", InnerEnumerator.Current.IsDirectory);

                    return true;
                }

                return false;
            }
        }
    }

}
