namespace WinCopies.IO
{
    public interface IFolderLoader : IFileSystemObjectItemsLoader
    {

        FolderLoaderFileSystemWatcher FileSystemWatcher { get; }

    }
}
