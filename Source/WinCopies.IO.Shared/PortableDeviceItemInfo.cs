using Microsoft.WindowsAPICodePack.PortableDevices;
using Microsoft.WindowsAPICodePack.PropertySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using WinCopies.Linq;

namespace WinCopies.IO
{
    public interface IPortableDeviceItemInfo : IFileSystemObjectInfo
    {
        IPortableDeviceObject PortableDeviceObject { get; }
    }

    public class PortableDeviceItemInfo : FileSystemObjectInfo, IPortableDeviceItemInfo
    {
        public IPortableDeviceObject PortableDeviceObject { get; }

        private bool? _isSpecialItem;

        public override bool IsSpecialItem
        {
            get
            {
                if (_isSpecialItem.HasValue)

                    return _isSpecialItem.Value;

                bool result = (PortableDeviceObject.Properties.TryGetValue(Microsoft.WindowsAPICodePack.PortableDevices.PropertySystem.Properties.Legacy.Object.Common.IsSystem, out Property value) && value.TryGetValue(out bool _value) && _value) || (PortableDeviceObject.Properties.TryGetValue(Microsoft.WindowsAPICodePack.PortableDevices.PropertySystem.Properties.Legacy.Object.Common.IsHidden, out Property __value) && __value.TryGetValue(out bool ___value) && ___value);

                _isSpecialItem = result;

                return result;
            }
        }

        public override BitmapSource SmallBitmapSource => TryGetBitmapSource(SmallIconSize);

        public override BitmapSource MediumBitmapSource => TryGetBitmapSource(MediumIconSize);

        public override BitmapSource LargeBitmapSource => TryGetBitmapSource(LargeIconSize);

        public override BitmapSource ExtraLargeBitmapSource => TryGetBitmapSource(ExtraLargeIconSize);

        private bool? _isBrowsable;

        public override bool IsBrowsable
        {
            get
            {
                if (_isBrowsable.HasValue)

                    return _isBrowsable.Value;

                bool result = PortableDeviceObject is IEnumerablePortableDeviceObject;

                _isBrowsable = result;

                return result;
            }
        }

        public override string ItemTypeName => GetItemTypeName(System.IO.Path.GetExtension(Path), FileType);

        public override string Description => "N/A";

        private bool _isSizeLoaded;

        private Size? _size;

        public override Size? Size
        {
            get
            {
                if (_isSizeLoaded)

                    return _size;

                if (PortableDeviceObject.Properties.TryGetValue(Microsoft.WindowsAPICodePack.PortableDevices.PropertySystem.Properties.Legacy.Object.Common.Size, out Property value) && value.TryGetValue(out ulong _value))

                    _size = new Size(_value);

                _isSizeLoaded = true;

                return _size;
            }
        }

        public override IBrowsableObjectInfo Parent { get; }

        public override string LocalizedName => "N/A";

        private bool _isNameLoaded;

        private string _name;

        public override string Name
        {
            get
            {
                if (_isNameLoaded)

                    return _name;

                _name = PortableDeviceObject.Name;

                _isNameLoaded = true;

                return _name;
            }
        }

        public override FileType FileType { get; }

        private static FileType GetFileType(in PortableDeviceFileType portableDeviceFileType, in string path)
        {
            string extension = System.IO.Path.GetExtension(path);

            return portableDeviceFileType == PortableDeviceFileType.Folder ? FileType.Folder : extension == ".lnk" ? FileType.Link : extension == ".library.ms" ? FileType.Library : FileType.File;
        }

        internal PortableDeviceItemInfo(in IPortableDeviceObject portableDeviceObject, in IPortableDeviceInfo parentPortableDevice) : base($"{parentPortableDevice.Path}{WinCopies.IO.Path.PathSeparator}{portableDeviceObject.Name}")
        {
            PortableDeviceObject = portableDeviceObject;

            FileType = GetFileType(portableDeviceObject.FileType, Path);

            Parent = parentPortableDevice;
        }

        private PortableDeviceItemInfo(in IPortableDeviceObject portableDeviceObject, in IPortableDeviceItemInfo parent) : base($"{parent.Path}{WinCopies.IO.Path.PathSeparator}{portableDeviceObject.Name}")
        {
            PortableDeviceObject = portableDeviceObject;

            FileType = GetFileType(portableDeviceObject.FileType, Path);

            Parent = parent;
        }

        public override IEnumerable<IBrowsableObjectInfo> GetItems() => throw new NotImplementedException();

        public IEnumerable<IBrowsableObjectInfo> GetItems(Predicate<IPortableDeviceObject> predicate)
        {
            if (PortableDeviceObject is IEnumerablePortableDeviceObject enumerablePortableDeviceObject)

                return (predicate == null ? enumerablePortableDeviceObject : enumerablePortableDeviceObject).Where(predicate).Select(portableDeviceObject => new PortableDeviceItemInfo(portableDeviceObject, this));

            return null;
        }
    }
}
