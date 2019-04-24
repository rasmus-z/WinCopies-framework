using System;
using System.IO;

namespace WinCopies.IO
{
    public class FolderLoaderFileSystemWatcher : IDisposable
    {

        /// <summary>
        /// Gets the <see cref="System.IO. FileSystemWatcher"/> used to listen to the file system events of the current <see cref="Path"/> property.
        /// </summary>
        protected virtual FileSystemWatcher FileSystemWatcher { get; } = new FileSystemWatcher();

        public string Path { get => FileSystemWatcher.Path; internal set => FileSystemWatcher.Path = value; }

        public virtual bool IncludeSubdirectories => false;

        public string Filter { get => FileSystemWatcher.Filter; set => FileSystemWatcher.Filter = value; }

        public bool EnableRaisingEvents { get => FileSystemWatcher.EnableRaisingEvents; set => FileSystemWatcher.EnableRaisingEvents = value; }

        public NotifyFilters NotifyFilter { get => FileSystemWatcher.NotifyFilter; set => FileSystemWatcher.NotifyFilter = value; }

        public event FileSystemEventHandler Created;

        public event RenamedEventHandler Renamed;

        public event FileSystemEventHandler Deleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderLoaderFileSystemWatcher"/> class.
        /// </summary>
        public FolderLoaderFileSystemWatcher() => Init();

        protected virtual void Init()

        {

            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

            FileSystemWatcher.Created += (object sender, FileSystemEventArgs e) => Created(this, e);

            FileSystemWatcher.Renamed += (object sender, RenamedEventArgs e) => Renamed(this, e);

            FileSystemWatcher.Deleted += (object sender, FileSystemEventArgs e) => Deleted(this, e);

        }

        /// <summary>
        /// Frees all resources used by the <see cref="FileSystemWatcher"/> property.
        /// </summary>
        public virtual void Dispose() => FileSystemWatcher.Dispose();

    }
}
