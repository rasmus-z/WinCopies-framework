using Microsoft.Win32;

namespace WinCopies.IO
{
    public interface IRegistryItemInfo : IBrowsableObjectInfo
    {

        RegistryItemType RegistryItemType { get; }

        RegistryKey RegistryKey { get; }

    }
}
