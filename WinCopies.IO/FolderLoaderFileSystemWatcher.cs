using System;
using System.IO;
using WinCopies.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{
    public class FolderLoaderFileSystemWatcher : IDeepCloneable, IDisposable
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

        public virtual bool NeedsObjectsReconstruction => true; // True because of the FileSystemWatcher property.

        protected virtual void OnDeepClone(FolderLoaderFileSystemWatcher folderLoaderFileSystemWatcher, bool preserveIds) { }

        protected virtual FolderLoaderFileSystemWatcher DeepCloneOverride(bool preserveIds) => new FolderLoaderFileSystemWatcher();

        public object DeepClone(bool preserveIds)

        {

            ((IDisposable)this).ThrowIfDisposingOrDisposed();

            FolderLoaderFileSystemWatcher folderLoaderFileSystemWatcher = DeepCloneOverride(preserveIds);

            OnDeepClone(folderLoaderFileSystemWatcher, preserveIds);

            return folderLoaderFileSystemWatcher;

        }

        protected virtual void Init()

        {

            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

            FileSystemWatcher.Created += (object sender, FileSystemEventArgs e) => Created(this, e);

            FileSystemWatcher.Renamed += (object sender, RenamedEventArgs e) => Renamed(this, e);

            FileSystemWatcher.Deleted += (object sender, FileSystemEventArgs e) => Deleted(this, e);

        }

        public bool IsDisposing { get; private set; }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Frees all resources used by the <see cref="FileSystemWatcher"/> property.
        /// </summary>
        public void Dispose()

        {

            IsDisposing = true;

            DisposeOverride();

            IsDisposed = true;

            IsDisposing = false;

        }

        protected virtual void DisposeOverride() => FileSystemWatcher.Dispose();

    }
}
