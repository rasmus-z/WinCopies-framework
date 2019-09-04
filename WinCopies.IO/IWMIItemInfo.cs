using System;
using System.Management;

namespace WinCopies.IO
{
    public interface IWMIItemInfo : IBrowsableObjectInfo, IComparable<IFileSystemObject>, IEquatable<IFileSystemObject>
    {

        ManagementBaseObject ManagementObject { get; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> represents a root node.
        /// </summary>
        bool IsRootNode { get; }

        WMIItemType WMIItemType { get; }

    }

    public interface IWMIItemInfo<T> : IWMIItemInfo, IBrowsableObjectInfo<T> where T : IWMIItemInfoFactory

    {



    }
}
