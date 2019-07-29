namespace WinCopies.IO
{
    public interface IRegistryKeyLoader : IBrowsableObjectInfoLoader
    {

        RegistryItemTypes RegistryItemTypes { get; set; }

    }
}
