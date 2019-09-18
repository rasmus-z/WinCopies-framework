using Microsoft.Win32;
using System;

namespace WinCopies.IO
{
    public interface IRegistryItemInfo : IBrowsableObjectInfo, IComparable<IRegistryItemInfo>, IEquatable<IRegistryItemInfo>
    {

        RegistryItemType RegistryItemType { get; }

        RegistryKey RegistryKey { get; }

    }

    //public interface IRegistryItemInfo<T> : IRegistryItemInfo, IBrowsableObjectInfo<T> where T : IRegistryItemInfoFactory

    //{



    //}
}
