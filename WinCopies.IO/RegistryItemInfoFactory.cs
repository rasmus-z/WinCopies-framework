using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IRegistryItemInfoFactory
    {

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path);

        IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName);

    }

    public class RegistryItemInfoFactory : IRegistryItemInfoFactory
    {

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo() => new RegistryItemInfo();

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey) => new RegistryItemInfo(registryKey);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(string path) => new RegistryItemInfo(path);

        public virtual IBrowsableObjectInfo GetBrowsableObjectInfo(RegistryKey registryKey, string valueName) => new RegistryItemInfo(registryKey, valueName);

    }
}
