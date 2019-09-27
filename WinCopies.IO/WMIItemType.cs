using System;

namespace WinCopies.IO

{

    /// <summary>
    /// The WMI item type.
    /// </summary>
    public enum WMIItemType
    {

        /// <summary>
        /// The WMI item is a namespace.
        /// </summary>
        Namespace,

        /// <summary>
        /// The WMI item is a class.
        /// </summary>
        Class,

        /// <summary>
        /// The WMI item is an instance.
        /// </summary>
        Instance

    }

    /// <summary>
    /// Determines the WMI items to load.
    /// </summary>
    [Flags]
    public enum WMIItemTypes
    {

        /// <summary>
        /// Do not load any items.
        /// </summary>
        None = 0,

        /// <summary>
        /// Load the namespace items.
        /// </summary>
        Namespace = 1,

        /// <summary>
        /// Load the class items.
        /// </summary>
        Class = 2,

        /// <summary>
        /// Load the instance items.
        /// </summary>
        Instance = 4

    }

}