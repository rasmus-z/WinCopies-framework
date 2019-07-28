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

        public ManagementBaseObject ManagementObject { get; }

        private static string GetName(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            (managementObject as ManagementClass)?.Get();

            try
            {

                return wmiItemType == WMIItemType.Namespace ? (string)managementObject["Name"] : managementObject.ClassPath.ClassName;

            }
            catch (Exception) { return managementObject.ClassPath.ClassName; }

        }

        private static string GetPath(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            string path = @"\\" + managementObject.ClassPath.Server + @"\" + managementObject.ClassPath.NamespacePath;

            string name = GetName(managementObject, wmiItemType);

            if (name != null)

                path += @"\" + name;

            path += ":" + managementObject.ClassPath.ClassName;

            return path;

        }

        public WMIItemInfo() : this(new ManagementClass(@"\\.\ROOT:__NAMESPACE"), WMIItemType.Namespace) => IsRootNode = true;

        public WMIItemInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType) : this(managementObject, wmiItemType, new WMIItemInfoFactory()) { }

        public WMIItemInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType, WMIItemInfoFactory wmiItemInfoFactory) : base(GetPath(managementObject, wmiItemType), FileType.SpecialFolder)

        {

            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

            ManagementObject = managementObject;

            if (wmiItemType != WMIItemType.Instance)

                Name = GetName(managementObject, wmiItemType);

            WMIItemType = wmiItemType;

            if (wmiItemType == WMIItemType.Namespace && Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

                IsRootNode = true;

            Factory = wmiItemInfoFactory;

        }

        public static WMIItemInfo GetWMIItemInfo(string computerName, string serverClassRelativePath)

        {

            var stringBuilder = new StringBuilder();

            _ = stringBuilder.Append(@"\\");

            _ = stringBuilder.Append(computerName);

            _ = stringBuilder.Append(@"\");

            _ = stringBuilder.Append(IsNullEmptyOrWhiteSpace(serverClassRelativePath) ? "ROOT" : serverClassRelativePath);

            _ = stringBuilder.Append(":__NAMESPACE");

            return new WMIItemInfo(new ManagementClass(stringBuilder.ToString()), WMIItemType.Namespace);

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

        public new WMIItemInfoFactory Factory        {            get => (WMIItemInfoFactory)base.Factory;            set => base.Factory = value;        }

        public override IBrowsableObjectInfo Clone() => Factory.GetBrowsableObjectInfo((ManagementBaseObject)ManagementObject.Clone(), WMIItemType);

        protected override IBrowsableObjectInfo GetParent()
        {

            if (IsRootNode) return null;

            string path;

            switch (WMIItemType)

            {

                case WMIItemType.Namespace:

                    path = Path.Substring(0, Path.LastIndexOf('\\')) + ":__NAMESPACE";

                    return path.EndsWith("root:__namespace", true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(path, WMIItemType.Namespace);

                case WMIItemType.Class:

                    return Path.EndsWith("root:" + Name, true, CultureInfo.InvariantCulture)
                        ? Factory.GetBrowsableObjectInfo()
                        : Factory.GetBrowsableObjectInfo(Path.Substring(0, Path.IndexOf(':')) + ":__NAMESPACE", WMIItemType.Namespace);

                case WMIItemType.Instance:

                    path = Path.Substring(0, Path.IndexOf(':'));

                    path = path.Substring(0, path.LastIndexOf('\\')) + ':' + path.Substring(path.LastIndexOf('\\') + 1);

                    return Factory.GetBrowsableObjectInfo(path, WMIItemType.Class);

                default: // We souldn't reach this point.

                    return null;

            }

        }

        private WMIItemsLoader GetDefaultWMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) => (new WMIItemsLoader(workerReportsProgress, workerSupportsCancellation, WMIItemTypes.Namespace | WMIItemTypes.Class | WMIItemTypes.Instance) { Path = this });

        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation).LoadItems();

        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => GetDefaultWMIItemsLoader(workerReportsProgress, workerSupportsCancellation).LoadItemsAsync();

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="newValue"></param>
        public override void Rename(string newValue) => throw new NotImplementedException();
    }
}
