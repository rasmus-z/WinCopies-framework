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
using System.Security;

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

    public delegate ManagementObject ManagementObjectDeepClone(ManagementObject managementObject, SecureString password);

    public delegate ManagementClass ManagementClassDeepClone(ManagementClass managementClass, SecureString password);

    public delegate ConnectionOptions ConnectionOptionsDeepClone(ConnectionOptions connectionOptions, SecureString password);

    public delegate ManagementPath ManagementPathDeepClone(ManagementPath managementPath);

    public delegate ObjectGetOptions ObjectGetOptionsDeepClone(ObjectGetOptions objectGetOptions);

    public static class WMIItemInfo
    {

        public const string RootPath = @"\\.\ROOT:__NAMESPACE";
        public const string NamespacePath = ":__NAMESPACE";
        public const string NameConst = "Name";
        public const string RootNamespace = "root:__namespace";
        public const string ROOT = "ROOT";

        public static ConnectionOptionsDeepClone DefaultConnectionOptionsDeepClone { get; } = (ConnectionOptions connectionOptions, SecureString password) =>
        {

            return new ConnectionOptions()
            {
                Locale = connectionOptions.Locale,
                Username = connectionOptions.Username,
                SecurePassword = password,
                Authority = connectionOptions.Authority,
                Impersonation = connectionOptions.Impersonation,
                Authentication = connectionOptions.Authentication,
                EnablePrivileges = connectionOptions.EnablePrivileges,
                Timeout = connectionOptions.Timeout
            };

        };

        public static ManagementPathDeepClone DefaultManagementPathDeepClone { get; } = managementPath =>

        {

            return new ManagementPath() { Path = managementPath.Path, ClassName = managementPath.ClassName, NamespacePath = managementPath.NamespacePath, RelativePath = managementPath.RelativePath, Server = managementPath.Server };

        };

        public static ObjectGetOptionsDeepClone DefaultObjectGetOptionsDeepClone { get; } = objectGetOptions =>

        {

            return new ObjectGetOptions() { Timeout = objectGetOptions.Timeout, UseAmendedQualifiers = objectGetOptions.UseAmendedQualifiers };

        };

        public static ManagementObjectDeepClone DefaultManagementObjectDeepClone { get; } = (ManagementObject managementObject, SecureString password) =>

        {

            var _managementObject = managementObject as ManagementClass ?? managementObject as ManagementObject ?? throw new ArgumentException("managementObject must be a ManagementClass or a ManagementObject.", nameof(managementObject));

            var path = DefaultManagementPathDeepClone(_managementObject.Scope?.Path ?? _managementObject.Path); 

            return _managementObject is ManagementClass managementClass ? DefaultManagementClassDeepCloneDelegate(managementClass, null) :    new ManagementObject(
                new ManagementScope(
    path,
                    _managementObject.Scope?.Options is null ? null : DefaultConnectionOptionsDeepClone(_managementObject.Scope?.Options, password)
                    ), path, _managementObject.Options is null ? null : DefaultObjectGetOptionsDeepClone(_managementObject.Options) );

        };

        public static ManagementClassDeepClone DefaultManagementClassDeepCloneDelegate { get; } = (ManagementClass managementClass, SecureString password) =>

        {

            var path = DefaultManagementPathDeepClone(managementClass.Scope?.Path ?? managementClass.Path);

            return new ManagementClass(
                new ManagementScope(
    path,
                    managementClass?.Scope?.Options is null ? null : DefaultConnectionOptionsDeepClone(managementClass?.Scope?.Options, password)
                    ), path, managementClass.Options is null ? null : DefaultObjectGetOptionsDeepClone(managementClass.Options));

        };

        public static WMIItemInfoComparer<IWMIItemInfo> GetDefaultWMIItemInfoComparer() => new WMIItemInfoComparer<IWMIItemInfo>();

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

            string path = IO.Path.PathSeparator + managementObject.ClassPath.Server + IO.Path.PathSeparator + managementObject.ClassPath.NamespacePath;

            string name = GetName(managementObject, wmiItemType);

            if (name != null)

                path += IO.Path.PathSeparator + name;

            path += ":" + managementObject.ClassPath.ClassName;

            return path;

        }

    }

    public class WMIItemInfo<TParent, TItems, TFactory> : BrowsableObjectInfo<TParent, TItems, TFactory>, IWMIItemInfo where TParent : class, IWMIItemInfo where TItems : class, IWMIItemInfo where TFactory : IWMIItemInfoFactory
    {

        // public override bool IsRenamingSupported => false;

        private readonly DeepClone<ManagementBaseObject> _managementObjectDelegate;

        /// <summary>
        /// Gets the <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> represents.
        /// </summary>
        public ManagementBaseObject ManagementObject { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class as the root WMI item.
        /// </summary>
        public WMIItemInfo() : this(default) { }

        // todo: throw exception if the given factory has not TParent and TItems as generic arguments

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class as the root WMI item using a custom factory.
        /// </summary>
        public WMIItemInfo(TFactory factory) : this( WMIItemInfo. RootPath, WMIItemType.Namespace, (ManagementBaseObject managementObject) => WMIItemInfo. DefaultManagementClassDeepCloneDelegate((ManagementClass)managementObject, null), new ManagementClass(WMIItemInfo.RootPath), factory) => IsRootNode = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo{TParent, TItems, TFactory}()"/> constructor.
        /// </summary>
        /// <param name="path">The path of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/></param>.
        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.</param>
        /// <param name="managementObjectDelegate">The delegate that will be used to get a new <see cref="ManagementBaseObject"/>.</param>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> represents.</param>
        public WMIItemInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObjectDelegate, ManagementBaseObject managementObject) : this(path, wmiItemType, managementObjectDelegate, managementObject, default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class using a custom <see cref="IWMIItemInfoFactory"/>. If you want to initialize this class in order to represent the root WMI item, you can also use the <see cref="WMIItemInfo{TParent, TItems, TFactory}()"/> constructor.
        /// </summary>
        /// <param name="path">The path of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/></param>.
        /// <param name="wmiItemType">The type of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.</param>
        /// <param name="managementObjectDelegate">The delegate that will be used to get a new <see cref="ManagementBaseObject"/>.</param>
        /// <param name="managementObject">The <see cref="ManagementBaseObject"/> that this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> represents.</param>
        /// <param name="factory">The factory this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> and associated <see cref="WMILoader{T}"/> use to create new instances of the <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> class.</param>
        public WMIItemInfo(string path, WMIItemType wmiItemType, DeepClone<ManagementBaseObject> managementObjectDelegate, ManagementBaseObject managementObject, TFactory factory) : base(path, FileType.SpecialFolder, object.Equals(factory, null) ? (TFactory)(IWMIItemInfoFactory)new WMIItemInfoFactory<TParent, TItems>() : factory)

        {

            ThrowIfNull(managementObjectDelegate, nameof(managementObjectDelegate));

            ThrowIfNull(managementObject, nameof(managementObject));

            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

            _managementObjectDelegate = managementObjectDelegate;

            ManagementObject = managementObject;

            if (wmiItemType != WMIItemType.Instance)

                Name = WMIItemInfo. GetName(ManagementObject, wmiItemType);

            WMIItemType = wmiItemType;

            if (wmiItemType == WMIItemType.Namespace && Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

                IsRootNode = true;

        }

        /// <summary>
        /// Gets a new <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> that corresponds to the given server name and relative path.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        /// <param name="serverRelativePath">The server relative path.</param>
        /// <returns>A new <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> that corresponds to the given server name and relative path.</returns>
        /// <seealso cref="WMIItemInfo{TParent, TItems, TFactory}()"/>
        /// <seealso cref="WMIItemInfo{TParent, TItems, TFactory}(string, WMIItemType, DeepClone{ManagementBaseObject}, ManagementBaseObject)"/>
        public static WMIItemInfo<TParent, TItems, TFactory> GetWMIItemInfo(string serverName, string serverRelativePath) => GetWMIItemInfo(serverName, serverRelativePath, (TFactory)(IWMIItemInfoFactory)new WMIItemInfoFactory<TParent, TItems>());

        /// <summary>
        /// Gets a new <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> that corresponds to the given server name and relative path using a custom <see cref="WMIItemInfoFactory{TParent, TItems}"/>.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        /// <param name="serverRelativePath">The server relative path.</param>
        /// <param name="factory">A custom factory.</param>
        /// <returns>A new <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> that corresponds to the given server name and relative path.</returns>
        /// <seealso cref="WMIItemInfo{TParent, TItems, TFactory}()"/>
        /// <seealso cref="WMIItemInfo{TParent, TItems, TFactory}(string, WMIItemType, DeepClone{ManagementBaseObject}, ManagementBaseObject, TFactory)"/>
        public static WMIItemInfo<TParent, TItems, TFactory> GetWMIItemInfo(string serverName, string serverRelativePath, TFactory factory)

        {

            var stringBuilder = new StringBuilder();

            _ = stringBuilder.Append(@IO.Path.PathSeparator);

            _ = stringBuilder.Append(@IO.Path.PathSeparator);

            _ = stringBuilder.Append(serverName);

            _ = stringBuilder.Append(IO.Path.PathSeparator);

            _ = stringBuilder.Append(IsNullEmptyOrWhiteSpace(serverRelativePath) ? WMIItemInfo.ROOT : serverRelativePath);

            _ = stringBuilder.Append(WMIItemInfo.NamespacePath);

            string path = stringBuilder.ToString();

            return new WMIItemInfo<TParent, TItems, TFactory>(path, WMIItemType.Namespace, managementObject => WMIItemInfo. DefaultManagementClassDeepCloneDelegate((ManagementClass)managementObject, null), new ManagementClass(path), factory);

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
        /// Gets a value that indicates whether this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> represents a root node.
        /// </summary>
        public bool IsRootNode { get; }

        /// <summary>
        /// Gets the localized path of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override string LocalizedName => Name;

        /// <summary>
        /// Gets the name of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class;

        public WMIItemType WMIItemType { get; }

        //public new WMIItemInfoFactory Factory { get => (WMIItemInfoFactory)base.Factory; set => base.Factory = value; }

        protected override BrowsableObjectInfo<TParent, TItems, TFactory> DeepCloneOverride() => IsRootNode ? new WMIItemInfo<TParent, TItems, TFactory>() : new WMIItemInfo<TParent, TItems, TFactory>(Path, WMIItemType, _managementObjectDelegate, _managementObjectDelegate(ManagementObject), (TFactory)Factory.DeepClone());

        public override bool NeedsObjectsOrValuesReconstruction => true;

        protected override TParent GetParent()
        {

            if (IsRootNode) return null;

            string path;

            switch (WMIItemType)

            {

                case WMIItemType.Namespace:

                    path = Path.Substring(0, Path.LastIndexOf(IO.Path.PathSeparator)) + WMIItemInfo.NamespacePath;

                    return path.EndsWith(WMIItemInfo.RootNamespace, true, CultureInfo.InvariantCulture)
                        ? (TParent)Factory.GetBrowsableObjectInfo()
                        : (TParent)Factory.GetBrowsableObjectInfo(path, WMIItemType.Namespace);

                case WMIItemType.Class:

                    return Path.EndsWith("root:" + Name, true, CultureInfo.InvariantCulture)
                        ? (TParent)Factory.GetBrowsableObjectInfo()
                        : (TParent)Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.IndexOf(':')) + WMIItemInfo.NamespacePath, WMIItemType.Namespace);

                case WMIItemType.Instance:

                    path = Path.Substring(0, Path.IndexOf(':'));

                    path = path.Substring(0, path.LastIndexOf(IO.Path.PathSeparator)) + ':' + path.Substring(path.LastIndexOf(IO.Path.PathSeparator) + 1);

                    return (TParent)Factory.GetBrowsableObjectInfo(path, WMIItemType.Class);

                default: // We souldn't reach this point.

                    return null;

            }

        }

        private WMILoader<IWMIItemInfo<IWMIItemInfoFactory>> GetDefaultWMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) => new WMILoader<IWMIItemInfo<IWMIItemInfoFactory>>((IWMIItemInfo<IWMIItemInfoFactory>)this, GetAllEnumFlags<WMIItemTypes>(), workerReportsProgress, workerSupportsCancellation);

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

        public int CompareTo(IWMIItemInfo other) => WMIItemInfo. GetDefaultWMIItemInfoComparer().Compare(this, other);

        /// <summary>
        /// Determines whether the specified <see cref="IWMIItemInfo"/> is equal to the current object by calling the <see cref="Equals(object)"/> method.
        /// </summary>
        /// <param name="wmiItemInfo">The <see cref="IWMIItemInfo"/> to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public bool Equals(IWMIItemInfo wmiItemInfo) => Equals(wmiItemInfo as object);

        /// <summary>
        /// Gets an hash code for this <see cref="WMIItemInfo{TParent, TItems, TFactory}"/>.
        /// </summary>
        /// <returns>The hash code returned by the <see cref="FileSystemObject.GetHashCode"/> and the hash code of the <see cref="WMIItemType"/>.</returns>
        public override int GetHashCode() => base.GetHashCode() ^ WMIItemType.GetHashCode();

        /// <summary>
        /// Disposes the current <see cref="WMIItemInfo{TParent, TItems, TFactory}"/> and its parent and items recursively.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BrowsableObjectInfo{TParent, TItems, TFactory}.ItemsLoader"/> is busy and does not support cancellation.</exception>
        protected override void Dispose(bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
        {

            base.Dispose(disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

            ManagementObject.Dispose();

        }

    }

}
