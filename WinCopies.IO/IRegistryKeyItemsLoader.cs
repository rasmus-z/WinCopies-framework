namespace WinCopies.IO
{
    public interface IRegistryKeyItemsLoader : IBrowsableObjectInfoItemsLoader
    {

        RegistryItemTypes RegistryItemTypes { get; set; }

    }
}
