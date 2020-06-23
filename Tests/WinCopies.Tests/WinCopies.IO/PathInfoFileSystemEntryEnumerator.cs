using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WinCopies.Collections;
using WinCopies.IO;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.Tests
{
    public class PathInfo : IPathInfo
    {
        public string Name { get; }

        public FileType FileType { get; }

        public IReadOnlyList<PathInfo> SubPaths { get; }

        string IPathInfo.Path => Name;

        private bool? _isDirectory;

        bool IPathInfo.IsDirectory => (_isDirectory ?? (_isDirectory = If(ComparisonType.Or, ComparisonMode.Logical, Util.Util.Comparison.Equal, FileType, FileType.Folder, FileType.Drive, FileType.KnownFolder))).Value;

        public PathInfo(string relativePath, FileType fileType, params PathInfo[] subPaths)
        {
            Name = relativePath;

            FileType = fileType;

            SubPaths = new ReadOnlyCollection<PathInfo>(subPaths);
        }
    }

    [TestClass]
    public class PathInfoFileSystemEntryEnumerator
    {
        internal static PathInfo[] _paths = {
                new PathInfo("C:", FileType.Drive,
                    new PathInfo("A", FileType.Folder,
                        new PathInfo("B", FileType.File )
                    ),
                    new PathInfo("C", FileType.File),
                    new PathInfo("D", FileType.Folder,
                        new PathInfo("E", FileType.File)
                    ),
                    new PathInfo("F", FileType.Folder)
                ),
                new PathInfo("D:", FileType.Drive,
                    new PathInfo("G", FileType.File)
                )
            };

        internal static IPathInfo[] _joinedPaths =
        {
                new PathInfo("C:", FileType.Drive),
                new PathInfo("C:\\A", FileType.Folder),
                new PathInfo("C:\\A\\B", FileType.File),
                new PathInfo("C:\\C", FileType.File),
                new PathInfo("C:\\D", FileType.Folder),
                new PathInfo("C:\\D\\E", FileType.File),
                new PathInfo("C:\\F", FileType.Folder),
                new PathInfo("D:", FileType.Drive),
                new PathInfo("D:\\G", FileType.File)
            };

        [TestMethod]
        public void TestPathEnumerator()
        {

            var enumerator = new IO.PathInfoFileSystemEntryEnumerator(_paths, null, null,
#if NETCORE
                null,
#endif
                new FileSystemEntryEnumeratorProcessSimulation() { EnumerateFunc = GetEnumerable });

            for (int i = 0; enumerator.MoveNext(); i++)

                Assert.IsTrue(_joinedPaths[i].Path == enumerator.Current.Path && _joinedPaths[i].IsDirectory == enumerator.Current.IsDirectory, $"Occurrence: {i}; Joined path: {_joinedPaths[i].Path}; Current enumeration path: {enumerator.Current.Path}; Joined path is directory: {_joinedPaths[i].IsDirectory}; Current enumeration path is directory: {enumerator.Current.IsDirectory}.");

            enumerator.Dispose();
        }

        internal static IEnumerable<string> GetEnumerable(string path, PathType pathType)
        {
            Queue<string> _paths = path.SplitToQueue(false, Path.PathSeparator);

            IReadOnlyList<PathInfo> __paths = PathInfoFileSystemEntryEnumerator._paths;

            var _path = new System.Collections.Generic.LinkedList<string>();

            string dequeue()
            {
                _ = _path.AddLast(_paths.Peek());

                return _paths.Dequeue();
            }

            PathInfo getFirst() => __paths.FirstOrDefault(p => p.Name == dequeue()) ?? throw new InvalidOperationException($"Cannot find path from given parameter. Name: { _path.Last}; path: { _path.Join(true, "\\")}");

            int length = _paths.Count - 1;

            for (int i = 0; i < length; i++)

                __paths = getFirst().SubPaths;

            return getFirst().SubPaths.Select(p => p.Name);
        }
    }
}
