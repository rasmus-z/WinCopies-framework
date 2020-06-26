using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WinCopies.Collections;
using WinCopies.IO;
using WinCopies.Linq;
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
        FileStream fs = new FileStream("log.log", FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.None);
        StreamWriter sw;

        public PathInfoFileSystemEntryEnumerator() => sw = new StreamWriter(fs);

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
                new PathInfo("C:\\C", FileType.File),
                new PathInfo("C:\\A", FileType.Folder),
                new PathInfo("C:\\A\\B", FileType.File),
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
                new FileSystemEntryEnumeratorProcessSimulation() { EnumerateFunc = GetEnumerable, WriteLogAction = s => { sw.WriteLine(s); sw.Flush(); }  });

            for (int i = 0; enumerator.MoveNext(); i++)
            {
                sw.WriteLine($"i: {i}; enumerator.Current.Path: {enumerator.Current.Path}; enumerator.Current.IsDirectory: {enumerator.Current.IsDirectory}");

                sw.Flush();

                Assert.IsTrue(_joinedPaths[i].Path.EndsWith(enumerator.Current.Path) && _joinedPaths[i].IsDirectory == enumerator.Current.IsDirectory, $"Occurrence: {i}; Joined path: {_joinedPaths[i].Path}; Current enumeration path: {enumerator.Current.Path}; Joined path is directory: {_joinedPaths[i].IsDirectory}; Current enumeration path is directory: {enumerator.Current.IsDirectory}.");
            }

            enumerator.Dispose();
        }

        internal IEnumerable<string> GetEnumerable(string path, PathType pathType)
        {
            sw.WriteLine(nameof(GetEnumerable));

            sw.WriteLine($"\tpath: {path}; path type: {pathType}");

            Queue<string> _paths = Temp.SplitToQueue(path, false, IO.Path.PathSeparator);

            Assert.IsNotNull(_paths);

            Assert.IsTrue(_paths.Count > 0, "Queue count error.");

            sw.WriteLine($"\t_paths.Count: {_paths.Count}");

            foreach (string p in _paths)

                sw.WriteLine($"\t{p}");

            sw.Flush();

            IReadOnlyList<PathInfo> __paths = PathInfoFileSystemEntryEnumerator._paths;

            var _path = new System.Collections.Generic.LinkedList<string>();

            string peek()
            {
                sw.WriteLine($"\t{nameof(peek)}");

                sw.Flush();

                string __path = _paths.Peek();

                sw.WriteLine($"\t\t_paths.Dequeue(): {__path}");

                sw.Flush();

                return __path;
            }

            PathInfo getFirst() => __paths.FirstOrDefault(p=>p.Name == peek()  );

            // ?? throw new InvalidOperationException($"Cannot find path from given parameter. Name: { _path.Last?.Value ?? "<Null>"}; joined paths: { _path.Join(true, "\\")}; path: {path}; path type: {pathType}")

            PathInfo pathInfo;

            while (_paths.Count > 1)
            {
                pathInfo = getFirst();

                if (pathInfo == null) return Temp.GetEmptyEnumerable();

                __paths = pathInfo.SubPaths;

                _ = _path.AddLast(_ = _paths.Dequeue());
            }

            sw.WriteLine($"\tLoop ok.");

            sw.Flush();

            pathInfo = getFirst();

            if (pathInfo == null)
            {
                sw.WriteLine($"\t_pathInfo is null.");

                sw.Flush();

                return Temp.GetEmptyEnumerable();
            }

            IEnumerable<PathInfo> result = pathInfo.SubPaths.WherePredicate(p => ((If(ComparisonType.Or, ComparisonMode.Logical, Util.Util.Comparison.Equal, p.FileType, FileType.Folder, FileType.Drive) && pathType == PathType.Directories) || (p.FileType == FileType.File && pathType == PathType.Files)));

            foreach (PathInfo _pathInfo in result ) 

                sw.WriteLine($"\t_pathInfo name: {_pathInfo.Name}; _pathInfo file type: {_pathInfo.FileType}");

            sw.Flush();

            return result .Select(p => $"{path}{WinCopies.IO.Path.PathSeparator}{p.Name}");
        }
    }
}
