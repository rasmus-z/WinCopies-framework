using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using WinCopies.Collections;
using WinCopies.IO;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.IO
{

    public sealed class PathCollection : ICollection<IPathInfo>, IList<IPathInfo>
    {
        private readonly IList<IPathInfo> _list;

        public string Path { get; }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public IPathInfo this[int index]
        {
            get => _list[index]; set
            {
                ValidatePath(value);

                _list[index] = value;
            }
        }

        public PathCollection(string path) : this(path, new List<IPathInfo>()) { }

        public PathCollection(string path, IList<IPathInfo> list)
        {
            Path = path;

            foreach (IPathInfo _path in list)

                ValidatePath(_path);

            _list = list;
        }

        private void ValidatePath(IPathInfo item)
        {
            if (System.IO.Path.IsPathRooted(item.Path))

                throw new ArgumentException("The path to add must be relative.");
        }

        public void Add(IPathInfo item)
        {
            ValidatePath(item);

            _list.Add(item);
        }

        public void Clear() => _list.Clear();

        public bool Contains(IPathInfo item) => _list.Contains(item);

        public void CopyTo(IPathInfo[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public bool Remove(IPathInfo item) => _list.Remove(item);

        public IEnumerator<IPathInfo> GetEnumerator() => new PathCollectionEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(IPathInfo item) => _list.IndexOf(item);

        public void Insert(int index, IPathInfo item)
        {
            ValidatePath(item);

            _list.Add(item);
        }

        public void RemoveAt(int index) => _list.RemoveAt(index);

        public struct PathCollectionEnumerator : IEnumerator<IPathInfo>, WinCopies.Util.DotNetFix.IDisposable
        {
            private PathCollection _pathCollection;

            private IEnumerator<IPathInfo> _innerEnumerator;

            private IPathInfo _current;

            public IPathInfo Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            object IEnumerator.Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            public bool IsDisposed { get; private set; }

            public PathCollectionEnumerator(PathCollection pathCollection)
            {
                _pathCollection = pathCollection;

                _innerEnumerator = pathCollection._list.GetEnumerator();

                _current = null;

                IsDisposed = false;
            }

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                _innerEnumerator.Dispose();

                _innerEnumerator = null;

                _pathCollection = null;

                IsDisposed = true;
            }

            public bool MoveNext()
            {
                ThrowIfDisposed(this);

                if (_innerEnumerator.MoveNext())
                {
                    _current = new PathInfo($"{_pathCollection.Path}{WinCopies.IO.Path.PathSeparator}{_innerEnumerator.Current.Path}", _innerEnumerator.Current.Size);

                    return true;
                }

                return false;
            }

            public void Reset()
            {
                ThrowIfDisposed(this);

                _innerEnumerator.Reset();

                _current = null;
            }
        }
    }

}
