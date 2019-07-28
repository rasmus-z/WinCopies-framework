using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{

    public interface IWMIItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IWMIItemInfoFactoryOptions Options { get; }

        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(ManagementScope managementScope, ManagementPath managementPath, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

    }

}
