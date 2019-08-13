using System;
using System.Text;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;
using System.Management;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;
using System.Globalization;

namespace WinCopies.IO
{

    public enum WMIItemType
    {

        Namespace,

        Class,

        Instance

    }

    [Flags]
    public enum WMIItemTypes
    {

        None = 0,

        Namespace = 1,

        Class = 2,

        Instance = 4

    }

    public class WMIItemInfo : BrowsableObjectInfo, IWMIItemInfo
    {

        public static WMIItemInfoComparer<IWMIItemInfo> GetDefaultWMIItemInfoComparer() => new WMIItemInfoComparer<IWMIItemInfo>();

        // public override bool IsRenamingSupported => false;

        private const string RootPath = @"\\.\ROOT:__NAMESPACE";
        private const string Namespace = ":__NAMESPACE";
        private readonly Func<ManagementBaseObject> _managementObjectDelegate;

        public ManagementBaseObject ManagementObject { get; private set; }

        public static string GetName(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            (managementObject as ManagementClass)?.Get();

            const string name = "Name";

            return wmiItemType == WMIItemType.Namespace ? (string)managementObject[name] : managementObject.ClassPath.ClassName;

        }

        public static string GetPath(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            string path = @"\\" + managementObject.ClassPath.Server + @"\" + managementObject.ClassPath.NamespacePath;

            string name = GetName(managementObject, wmiItemType);

            if (name != null)

                path += @"\" + name;

            path += ":" + managementObject.ClassPath.ClassName;

            return path;

        }

        public WMIItemInfo() : this(RootPath, WMIItemType.Namespace, () => new ManagementClass(RootPath), new WMIItemInfoFactory()) => IsRootNode = true;

        public WMIItemInfo(string path, WMIItemType wmiItemType, Func<ManagementBaseObject> managementObjectDelegate) : this(path, wmiItemType, managementObjectDelegate, new WMIItemInfoFactory()) { }

        public WMIItemInfo(string path, WMIItemType wmiItemType, Func<ManagementBaseObject> managementObjectDelegate, WMIItemInfoFactory factory) : base(path, FileType.SpecialFolder, factory)

        {

            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

            _managementObjectDelegate = managementObjectDelegate;

            ManagementObject = managementObjectDelegate();

            if (wmiItemType != WMIItemType.Instance)

                Name = GetName(ManagementObject, wmiItemType);

            WMIItemType = wmiItemType;

            if (wmiItemType == WMIItemType.Namespace && Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

                IsRootNode = true;

        }

        public static WMIItemInfo GetWMIItemInfo(string serverName, string serverClassRelativePath) => GetWMIItemInfo(serverName, serverClassRelativePath, new WMIItemInfoFactory());

        public static WMIItemInfo GetWMIItemInfo(string serverName, string serverClassRelativePath, WMIItemInfoFactory factory)

        {

            var stringBuilder = new StringBuilder();

            _ = stringBuilder.Append(@"\\");

            _ = stringBuilder.Append(serverName);

            _ = stringBuilder.Append(@"\");

            _ = stringBuilder.Append(IsNullEmptyOrWhiteSpace(serverClassRelativePath) ? "ROOT" : serverClassRelativePath);

            _ = stringBuilder.Append(Namespace);

            string path = stringBuilder.ToString();

            return new WMIItemInfo(path, WMIItemType.Namespace, () => new ManagementClass(path), factory);

        }

        //public static WMIItemInfo GetWMIItemInfo(string computerName, string serverClassRelativeName, string classMemberName)

        //{

        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append(@"\\");

        //    stringBuilder.Append(computerName);

        //    stringBuilder.Append(@"\");

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
        /// Gets a value that indicates whether this <see cref="WMIItemInfo"/> represents a root node.
        /// </summary>
        public bool IsRootNode { get; }

        /// <summary>
        /// Gets the localized path of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override string LocalizedName => Name;

        /// <summary>
        /// Gets the name of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override string Name { get; }

        /// <summary>
        /// Gets the small <see cref="BitmapSource"/> of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        /// <summary>
        /// Gets the medium <see cref="BitmapSource"/> of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        /// <summary>
        /// Gets the large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        /// <summary>
        /// Gets the extra large <see cref="BitmapSource"/> of this <see cref="WMIItemInfo"/>.
        /// </summary>
        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        /// <summary>
        /// Gets a value that indicates whether this <see cref="WMIItemInfo"/> is browsable.
        /// </summary>
        public override bool IsBrowsable => WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class;

        public WMIItemType WMIItemType { get; }

        public new WMIItemInfoFactory Factory { get => (WMIItemInfoFactory)base.Factory; set => base.Factory = value; }

        protected override BrowsableObjectInfo DeepCloneOverride(bool preserveIds) => IsRootNode ? new WMIItemInfo() : new WMIItemInfo(Path, WMIItemType, _managementObjectDelegate);

        public override bool NeedsObjectsReconstruction => true;

        protected override IBrowsableObjectInfo GetParent()
        {

            if (IsRootNode) return null;

            string path;

            switch (WMIItemType)

            {

                case WMIItemType.Namespace:

                    path = Path.Substring(0, Path.LastIndexOf('\\')) + Namespace;

                    return path.EndsWith("root:__namespace", true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(path, WMIItemType.Namespace);

                case WMIItemType.Class:

                    return Path.EndsWith("root:" + Name, true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.IndexOf(':')) + Namespace, WMIItemType.Namespace);

                case WMIItemType.Instance:

                    path = Path.Substring(0, Path.IndexOf(':'));

                    path = path.Substring(0, path.LastIndexOf('\\')) + ':' + path.Substring(path.LastIndexOf('\\') + 1);

                    return Factory.GetBrowsableObjectInfo(path, WMIItemType.Class);

                default: // We souldn't reach this point.

                    return null;

            }

        }

        private WMILoader GetDefaultWMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) => (new WMILoader(this, GetAllEnumFlags<WMIItemTypes>(), workerReportsProgress, workerSupportsCancellation) { Path = this });

#pragma warning disable IDE0067 // Dispose objects before losing scope
        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation).LoadItems();

        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation).LoadItemsAsync();
#pragma warning restore IDE0067 // Dispose objects before losing scope

        ///// <summary>
        ///// Not implemented.
        ///// </summary>
        ///// <param name="newValue"></param>
        //public override void Rename(string newValue) => throw new NotImplementedException();

        public override bool Equals(object obj) => ReferenceEquals(this, obj)
                ? true : obj is IWMIItemInfo _obj ? WMIItemType == _obj.WMIItemType && Path.ToLower() == _obj.Path.ToLower()
                : false;

        public int CompareTo( IWMIItemInfo other ) => GetDefaultWMIItemInfoComparer().Compare(this, other);

        public bool Equals( IWMIItemInfo other ) => Equals(other as object);

        public override int GetHashCode() => base.GetHashCode() ^ WMIItemType.GetHashCode();

        protected override void DisposeOverride( bool disposing, bool disposeItemsLoader, bool disposeParent, bool disposeItems, bool recursively)
        {

            base.DisposeOverride( disposing, disposeItemsLoader, disposeParent, disposeItems, recursively);

            ManagementObject.Dispose();

        }

    }

}
