using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Util;
using WinCopies.Util;
using System.Management;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;

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

    public class WMIItemInfo : BrowsableObjectInfo
    {

        public ManagementBaseObject ManagementObject { get; }

        public WMIItemInfo() : this(new ManagementClass(@"\\.\ROOT:__NAMESPACE"), WMIItemType.Namespace) => IsRootNode = true;

        private static string GetName(ManagementBaseObject managementObject, WMIItemType wmiItemType)

        {

            (managementObject as ManagementClass)?.Get();

            return wmiItemType == WMIItemType.Namespace ? (string)managementObject["Name"] : managementObject.ClassPath.ClassName;

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

        public WMIItemInfo(ManagementBaseObject managementObject, WMIItemType wmiItemType) : base(GetPath(managementObject, wmiItemType), FileType.SpecialFolder)

        {

            ThrowOnEnumNotValidEnumValue(wmiItemType, WMIItemType.Namespace, WMIItemType.Class);

            ManagementObject = managementObject;

            Name = GetName(managementObject, wmiItemType);

            WMIItemType = wmiItemType;

            if (Path.ToUpper().EndsWith("ROOT:__NAMESPACE"))

                IsRootNode = true;

        }

        public static WMIItemInfo GetWMIItemInfo(string computerName, string serverClassRelativePath)

        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"\\");

            stringBuilder.Append(computerName);

            stringBuilder.Append(@"\");

            stringBuilder.Append(WinCopies.Util.Util.IsNullEmptyOrWhiteSpace(serverClassRelativePath) ? "ROOT" : serverClassRelativePath);

            stringBuilder.Append(":__NAMESPACE");

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

        public bool IsRootNode { get; }

        public override string LocalizedName => Name;

        public override string Name { get; }

        // todo:

        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(new System.Drawing.Size(16, 16));

        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(new System.Drawing.Size(48, 48));

        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(128, 128));

        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(new System.Drawing.Size(256, 256));

        public override bool IsBrowsable => WMIItemType == WMIItemType.Namespace || WMIItemType == WMIItemType.Class;

        public WMIItemType WMIItemType { get; }

        public override IBrowsableObjectInfo Clone() => throw new NotImplementedException();

        public override IBrowsableObjectInfo GetParent() => throw new NotImplementedException();

        public override void LoadItems(bool workerReportsProgress, bool workerSupportsCancellation) => (new WMIItemsLoader(workerReportsProgress, workerSupportsCancellation) { Path = this }).LoadItems();

        public override void LoadItemsAsync(bool workerReportsProgress, bool workerSupportsCancellation) => (new WMIItemsLoader(workerReportsProgress, workerSupportsCancellation) { Path = this }).LoadItemsAsync();

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="newValue"></param>
        public override void Rename(string newValue) => throw new NotImplementedException();
    }
}
