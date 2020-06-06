using Microsoft.WindowsAPICodePack.PortableDevices;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WinCopies.Linq;

namespace WinCopies.IO
{
    public interface IPortableDeviceInfo:IFileSystemObjectInfo
    {
        IPortableDevice PortableDevice { get; }
    }

    public class PortableDeviceInfo : FileSystemObjectInfo, IPortableDeviceInfo
    {
        private const int PortableDeviceIcon = 42;

        public IPortableDevice PortableDevice { get; }

        public override FileType FileType => FileType.Folder;

        public override bool IsSpecialItem => false;

        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(SmallIconSize);

        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(MediumIconSize);

        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(LargeIconSize);

        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(ExtraLargeIconSize);

        public override bool IsBrowsable => false;

        public override string ItemTypeName => "Portable device";

        public override string Description => "N/A";

        public override Size? Size => null;

        public override IBrowsableObjectInfo Parent => ShellObjectInfo.From(ShellObject.FromParsingName(KnownFolders.Computer.ParsingName));

        public override string LocalizedName => "N/A";

        public override string Name => PortableDevice.DeviceFriendlyName;

        public PortableDeviceInfo(in IPortableDevice portableDevice) : base(portableDevice.DeviceFriendlyName) => PortableDevice = portableDevice;

        private BitmapSource TryGetBitmapSource(int size)

        {

#if NETFRAMEWORK

            using (Icon icon = TryGetIcon(PortableDeviceIcon, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, new System.Drawing.Size(size, size)))

#else

            using Icon icon = TryGetIcon(PortableDeviceIcon, Microsoft.WindowsAPICodePack.Win32Native.Consts.DllNames.Shell32, new System.Drawing.Size(size, size));

#endif

            return icon == null ? null : Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        public override IEnumerable<IBrowsableObjectInfo> GetItems() => GetItems(null);

        public IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<IPortableDeviceObject> predicate) => (predicate == null ? PortableDevice : PortableDevice.Where(predicate)).Select(portableDeviceObject => new PortableDeviceItemInfo(portableDeviceObject, this));
    }
}
