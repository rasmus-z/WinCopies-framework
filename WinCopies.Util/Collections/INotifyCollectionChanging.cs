namespace WinCopies.Collections
{
    public interface INotifyCollectionChanging
    {
        event NotifyCollectionChangingEventHandler CollectionChanging;
    }

    public interface INotifyCollectionChanged : INotifyCollectionChanging, System.Collections.Specialized.INotifyCollectionChanged
    {
        
    }
}
