using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Util;

namespace WinCopies.IO
{

    public interface IWMIItemInfoFactory : IBrowsableObjectInfoFactory
    {

        IWMIItemInfoFactoryOptions Options { get; }

        /// <summary>
        /// Gets a new instance of the <see cref="IBrowsableObjectInfo"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="IBrowsableObjectInfo"/> class.</returns>
        IBrowsableObjectInfo GetBrowsableObjectInfo();

        IBrowsableObjectInfo GetBrowsableObjectInfo(IWMIItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObject);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObject, IWMIItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementScope> managementScope, DeepClone<ManagementPath> managementPath);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementScope> managementScope, DeepClone<ManagementPath> managementPath, IWMIItemInfoFactory factory);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, IWMIItemInfoFactory factory);

    }

}
