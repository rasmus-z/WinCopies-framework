using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IWMIItemInfo : IBrowsableObjectInfo
    {

        ManagementBaseObject ManagementObject { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo"/> represents a root node.
        /// </summary>
        bool IsRootNode { get; }

        WMIItemType WMIItemType { get; }

        WMIItemInfoFactory WMIItemInfoFactory { get; set; }
    }
}
