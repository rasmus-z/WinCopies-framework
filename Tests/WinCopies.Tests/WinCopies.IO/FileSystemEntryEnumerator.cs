using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WinCopies.IO;
using WinCopies.Util;

namespace WinCopies.Tests
{
    public class PathInfo : IPathInfo
    {
        public string Name { get; }

        public FileType FileType { get; }

        public IReadOnlyList<PathInfo> SubPaths { get; }

        string IPathInfo.Path => Name;

        bool IPathInfo.IsDirectory => FileType == FileType.Folder || FileType == FileType.Drive || FileType == FileType.KnownFolder;

        public PathInfo(string relativePath, FileType fileType, params PathInfo[] subPaths)
        {
            Name = relativePath;

            FileType = fileType;

            SubPaths = new ReadOnlyCollection<PathInfo>(subPaths);
        }
    }

    [TestClass]
    public class FileSystemEntryEnumerator
    {
        [TestMethod]
        public void Test()
        {
            PathInfo[] paths = {
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

            IEnumerable<string> getEnumerable(string path, PathType pathType)
            {
                Queue<string> _paths = path.SplitToQueue(false, '\\');

                IReadOnlyList<PathInfo> __paths =paths;

                var _path = new LinkedList<string>();

                string dequeue()
                {
                    _path.AddLast(_paths.Peek());

                    return _paths.Dequeue();
                }

                PathInfo getFirst () => __paths.FirstOrDefault(p => p.Name == dequeue()) ??                throw new InvalidOperationException($"Cannot find path from given parameter. Name: { _path.Last}; path: { _path.Join(") ; 

                int length = _paths.Count - 1;

                for (int i = 0;i<length;i++)
                
                    __paths = getFirst() .SubPaths ;

                return getFirst().SubPaths.Select(p => p.Name);
            }

            WinCopies.IO.PathInfoFileSystemEntryEnumerator enumerator = new IO.PathInfoFileSystemEntryEnumerator(paths, null, null, null, new FileSystemEntryEnumeratorProcessSimulation() { EnumerateFunc =  });
        }
    }
}
