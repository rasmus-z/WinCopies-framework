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

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType, ManagementBaseObject managementObject, DeepClone<ManagementBaseObject> managementObjectDelegate);

        IBrowsableObjectInfo GetBrowsableObjectInfo(string path, WMIItemType wmiItemType);

    }

}
