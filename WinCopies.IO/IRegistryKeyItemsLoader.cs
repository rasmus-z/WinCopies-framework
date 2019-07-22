using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IRegistryKeyItemsLoader : IBrowsableObjectInfoItemsLoader
    {

        RegistryItemTypes RegistryItemTypes { get; set; }

    }
}
