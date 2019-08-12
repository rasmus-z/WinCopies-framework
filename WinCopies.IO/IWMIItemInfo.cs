using System;
using System.Management;

namespace WinCopies.IO
{
    public interface IWMIItemInfo : IBrowsableObjectInfo, IComparable<IFileSystemObject>, IEquatable<IFileSystemObject>
    {

        ManagementBaseObject ManagementObject { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo"/> represents a root node.
        /// </summary>
        bool IsRootNode { get; }

        WMIItemType WMIItemType { get; }
    }
}
