using System;
using System.Text;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;
using System.Management;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;
using System.Globalization;
using WinCopies.Util;

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

    public class WMIItemInfo<T> : BrowsableObjectInfo<T>, IWMIItemInfo, IBrowsableObjectInfo<T> where T : IWMIItemInfoFactory
    {

        public static WMIItemInfoComparer<IWMIItemInfo> GetDefaultWMIItemInfoComparer() => new WMIItemInfoComparer<IWMIItemInfo>();

        // public override bool IsRenamingSupported => false;

        private const string RootPath = @"\\.\ROOT:__NAMESPACE";
        private const string NamespacePath = ":__NAMESPACE";
        private const string NameConst = "Name";
        private const string RootNamespace = "root:__namespace";
        private const string ROOT = "ROOT";
        private readonly DeepClone<ManagementBaseObject> _managementObjectDelegate;

        /// <summary>
        /// Gets the <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{T}"/> represents.
        /// </summary>
        public ManagementBaseObject ManagementObject { get; private set; }

        /// <summary>
        /// Gets the name of the given <see cref="ManagementBaseObject"/>.
        /// </summary>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> for which get the name.</param>
        /// <param name="wmiItemType">The <see cref="IO.WMIItemType"/> of <paramref name="managementObject"/>.</param>
        /// <returns>The name of the given <see cref="ManagementBaseObject"/>.</returns>
        public static string GetName(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            (managementObject as ManagementClass)?.Get();

            const string name = NameConst;

            return wmiItemType == WMIItemType.Namespace ? (string)managementObject[name] : managementObject.ClassPath.ClassName;

        }

        /// <summary>
        /// Gets the path of the given <see cref="ManagementBaseObject"/>.
        /// </summary>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> for which get the path.</param>
        /// <param name="wmiItemType">The <see cref="IO.WMIItemType"/> of <paramref name="managementObject"/>.</param>
        /// <returns>The path of the given <see cref="ManagementBaseObject"/>.</returns>
        public static string GetPath(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            string path = @IO.Path.PathSeparator + managementObject.ClassPath.Server + IO.Path.PathSeparator + managementObject.ClassPath.NamespacePath;

            string name = GetName(managementObject, wmiItemType);

            if (name != null)

                path += IO.Path.PathSeparator + name;

            path += ":" + managementObject.ClassPath.ClassName;

            return path;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{T}"/> class as the root WMI item.
        /// </summary>
        public WMIItemInfo() : this(default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{T}"/> class as the root WMI item using a custom factory.
        /// </summary>
        public WMIItemInfo( T factory) : this(RootPath, WMIItemType.Namespace, (bool? preserveIds) => new ManagementClass(RootPath), null, factory) => IsRootNode = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{T}"/> class. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo()"/> constructor.
        /// </summary>
        /// <param name="path">The path of this <see cref="WMIItemInfo{T}"/></param>.
        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{T}"/>.</param>
        /// <param name="managementObjectDelegate">The delegate that will be used to get a new <see cref="ManagementBaseObject"/> and, if <paramref name="managementObject"/> is null, to initialize this instance of <see cref="WMIItemInfo{T}"/> (see <paramref name="managementObject"/>).</param>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{T}"/> represents. Leave this parameter null for initializing this instance with a value provided by <paramref name="managementObjectDelegate"/>.</param>
        public WMIItemInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObjectDelegate, ManagementBaseObject managementObject) : this(path, wmiItemType, managementObjectDelegate, managementObject, default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{T}"/> class using a custom <see cref="IWMIItemInfoFactory"/>. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo()"/> constructor.
        /// </summary>
        /// <param name="path">The path of this <see cref="WMIItemInfo{T}"/></param>.
        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{T}"/>.</param>
        /// <param name="managementObjectDelegate">The delegate that will be used to get a new <see cref="ManagementBaseObject"/> and, if <paramref name="managementObject"/> is null, to initialize this instance of <see cref="WMIItemInfo{T}"/> (see <paramref name="managementObject"/>).</param>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{T}"/> represents. Leave this parameter null for initializing this instance with a value provided by <paramref name="managementObjectDelegate"/>.</param>
        /// <param name="factory">The factory this <see cref="WMIItemInfo{T}"/> and associated <see cref="WMILoader{T}"/> use to create new instances of the <see cref="WMIItemInfo{T}"/> class.</param>
        public WMIItemInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObjectDelegate, ManagementBaseObject managementObject, T factory) : base(path, FileType.SpecialFolder, object.Equals(factory, null) ? (T) (IWMIItemInfoFactory) new WMIItemInfoFactory() : factory)

        {

            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

            _managementObjectDelegate = managementObjectDelegate;

            ManagementObject = managementObject ?? managementObjectDelegate(null);

            if (wmiItemType != WMIItemType.Instance)

                Name = GetName(ManagementObject, wmiItemType);

            WMIItemType = wmiItemType;

            if (wmiItemType == WMIItemType.Namespace && Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

                IsRootNode = true;

        }

        /// <summary>
        /// Gets a new <see cref="WMIItemInfo{T}"/> that corresponds to the given server name and relative path.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        /// <param name="serverRelativePath">The server relative path.</param>
        /// <returns>A new <see cref="WMIItemInfo{T}"/> that corresponds to the given server name and relative path.</returns>
        /// <seealso cref="WMIItemInfo()"/>
        /// <seealso cref="WMIItemInfo(string, WMIItemType, DeepClone{ManagementBaseObject}, ManagementBaseObject)"/>
        public static WMIItemInfo<T> GetWMIItemInfo(string serverName, string serverRelativePath) => GetWMIItemInfo(serverName, serverRelativePath, (T) (IWMIItemInfoFactory) new WMIItemInfoFactory());

        /// <summary>
        /// Gets a new <see cref="WMIItemInfo{T}"/> that corresponds to the given server name and relative path using a custom <see cref="WMIItemInfoFactory"/>.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        /// <param name="serverRelativePath">The server relative path.</param>
        /// <param name="factory">A custom factory.</param>
        /// <returns>A new <see cref="WMIItemInfo{T}"/> that corresponds to the given server name and relative path.</returns>
        /// <seealso cref="WMIItemInfo()"/>
        /// <seealso cref="WMIItemInfo(string, WMIItemType, DeepClone{ManagementBaseObject}, ManagementBaseObject, T)"/>
        public static WMIItemInfo<T> GetWMIItemInfo(string serverName, string serverRelativePath, T factory)

        {

            var stringBuilder = new StringBuilder();

            _ = stringBuilder.Append(@IO.Path.PathSeparator);

            _ = stringBuilder.Append(@IO.Path.PathSeparator);

            _ = stringBuilder.Append(serverName);

            _ = stringBuilder.Append(IO.Path.PathSeparator);

            _ = stringBuilder.Append(IsNullEmptyOrWhiteSpace(serverRelativePath) ? ROOT : serverRelativePath);

            _ = stringBuilder.Append(NamespacePath);

            string path = stringBuilder.ToString();

            return new WMIItemInfo<T>(path, WMIItemType.Namespace, (bool? preserveIds) => new ManagementClass(path), null, factory);

        }

        //public static WMIItemInfo GetWMIItemInfo(string computerName, string serverClassRelativeName, string classMemberName)

        //{

        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append(@IO.Path.PathSeparator);

        //    stringBuilder.Append(computerName);

        //    stringBuilder.Append(IO.Path.PathSeparator);

        //    stringBuilder.Append(WinCopies.Util.Util.IsNullEmptyOrWhiteSpace(serverClassRelativeName) ? "ROOT" : serverClassRelativeName);

        //    stringBuilder.Append(":");

        //    stringBuilder.Append(classMemberName);

        //    return new WMIItemInfo(stringBuilder.ToString(), WMIItemType.Class);

        //}

        private BitmapSource TryGetBitmapSource(System.Drawing.Size size)

        {

            int iconIndex = 0;

            if (IsRootNode)

                iconIndex = 15;

            else if (WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class)

                iconIndex = 3;

            using (Icon icon = TryGetIcon(iconIndex, "shell32.dll", size))

                return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo{T}"/> represents a root node.
        /// </summary>
        public bool IsRootNode { get; }

        /// <summary>
        /// Gets the localized path of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override string LocalizedName => Name;

        /// <summary>
        /// Gets the name of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo{T}"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class;

        public WMIItemType WMIItemType { get; }

        //public new WMIItemInfoFactory Factory { get => (WMIItemInfoFactory)base.Factory; set => base.Factory = value; }

        protected override BrowsableObjectInfo<T> DeepCloneOverride(bool? preserveIds) => IsRootNode ? new WMIItemInfo<T>() : new WMIItemInfo<T>(Path, WMIItemType, _managementObjectDelegate, null, (T)Factory.DeepClone(preserveIds));

        public override bool NeedsObjectsReconstruction => true;

        protected override IBrowsableObjectInfo GetParent()
        {

            if (IsRootNode) return null;

            string path;

            switch (WMIItemType)

            {

                case WMIItemType.Namespace:

                    path = Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator)) + NamespacePath;

                    return path.EndsWith(RootNamespace, true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(path, WMIItemType.Namespace);

                case WMIItemType.Class:

                    return Path.EndsWith("root:" + Name, true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.IndexOf(':')) + NamespacePath, WMIItemType.Namespace);

                case WMIItemType.Instance:

                    path = Path.Substring(0, Path.IndexOf(':'));

                    path = path.Substring(0, path.LastIndexOf(IO.Path.PathSeparator)) + ':' + path.Substring(path.LastIndexOf(IO.Path.PathSeparator) + 1);

                    return Factory.GetBrowsableObjectInfo(path, WMIItemType.Class);

                default: // We souldn't reach this point.

                    return null;

            }

        }

        private WMILoader<IWMIItemInfo<IWMIItemInfoFactory>> GetDefaultWMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) => new WMILoader<IWMIItemInfo<IWMIItemInfoFactory>>( (IWMIItemInfo < IWMIItemInfoFactory >) this, GetAllEnumFlags<WMIItemTypes>(), workerReportsProgress, workerSupportsCancellation);

#pragma warning disable IDE0067 // Dispose objects before losing scope
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItems(GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation));

        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => LoadItemsAsync(GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation));
#pragma warning restore IDE0067 // Dispose objects before losing scope

        ///// <summary>
        ///// Not implemented.
        ///// </summary>
        ///// <param name="newValue"></param>
        //public override void Rename(string newValue) => throw new NotImplementedException();

        public override bool Equals(object obj) => ReferenceEquals(this, obj)
                ? true : obj is IWMIItemInfo _obj ? WMIItemType == _obj.WMIItemType && Path.ToLower() == _obj.Path.ToLower()
                : false;

        public int CompareTo(IWMIItemInfo other) => GetDefaultWMIItemInfoComparer().Compare(this, other);

        /// <summary>
        /// Determines whether the specified <see cref="IWMIItemInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
        /// </summary>
        /// <param name="wmiItemInfo">The <see cref="IWMIItemInfo"/> to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public bool Equals(IWMIItemInfo wmiItemInfo) => Equals(wmiItemInfo as object);

        /// <summary>
        /// Gets an hash code for this <see cref="WMIItemInfo{T}"/>.
        /// </summary>
        /// <returns>The hash code returned by the <see cref="FileSystemObject.GetHashCode"/> and the hash code of the <see cref="WMIItemType"/>.</returns>
        public override int GetHashCode() => base.GetHashCode() ^ WMIItemType.GetHashCode();

        /// <summary>
        /// Disposes the current <see cref="WMIItemInfo{T}"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo{T}.ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected override void DisposeOverride(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
        {

            base.DisposeOverride(disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

            ManagementObject.Dispose();

        }

    }

}
