using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using WinCopies.Util;

namespace WinCopies.IO
{
    public interface IWMILoader<TPath> : IBrowsableObjectInfoLoader<TPath> where TPath : IWMIItemInfo
    {

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMILoader"/> is busy.</exception>
        WMIItemTypes WMIItemTypes { get; set; }

    }

    /// <summary>
    /// A class for easier <see cref="ManagementBaseObject"/> items loading.
    /// </summary>
    public class WMILoader : BrowsableObjectInfoLoader<WMIItemInfo>, IWMILoader<WMIItemInfo>
    {

        protected override BrowsableObjectInfoLoader<WMIItemInfo> DeepCloneOverride(bool preserveIds) => new WMILoader(null, WMIItemTypes, WorkerReportsProgress, WorkerSupportsCancellation, (IFileSystemObjectComparer)FileSystemObjectComparer.DeepClone(preserveIds));

        private readonly WMIItemTypes _wmiItemTypes;

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMILoader"/> is busy.</exception>
        public WMIItemTypes WMIItemTypes { get => _wmiItemTypes; set => this.SetBackgroundWorkerProperty(nameof(WMIItemTypes), nameof(_wmiItemTypes), value, typeof(WMILoader), true); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{T}"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMILoader(WMIItemInfo path, WMIItemTypes wmiItemTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, wmiItemTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{T}"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMILoader(WMIItemInfo path, WMIItemTypes wmiItemTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer fileSystemObjectComparer) : base(path, workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer) => _wmiItemTypes = wmiItemTypes;

        public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            var paths = new ArrayAndListBuilder<PathInfo>();

            string _path;

            bool dispose = false;

#pragma warning disable IDE0019 // Pattern Matching
            var managementClass = Path.ManagementObject as ManagementClass;
#pragma warning restore IDE0019 // Pattern Matching

            if (managementClass == null)

            {

                dispose = true;

#pragma warning disable IDE0067 // Dispose objects before losing scope
                managementClass = new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.Factory?.Options?.ObjectGetOptions);
#pragma warning restore IDE0067 // Dispose objects before losing scope

            }

            if (Path.WMIItemType == WMIItemType.Namespace)

            {

                // managementClass = Path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.Factory?.Options?.ObjectGetOptions);

                if (WMIItemTypes.HasFlag(WMIItemTypes.Namespace))

                    try
                    {

                        managementClass.Get();

                        using (ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.Factory?.Options?.EnumerationOptions)).GetEnumerator())
                        {

                            ManagementBaseObject instance;

                            while (instances.MoveNext())

                                try

                                {

                                    do

                                    {

                                        instance = instances.Current;

                                        _path = WMIItemInfo.GetPath(instance, WMIItemType.Namespace);

                                        if (CheckFilter(_path))

                                            _ = paths.AddLast(new PathInfo() { ManagementObject = instance, Path = _path, Name = WMIItemInfo.GetName(instance, WMIItemType.Namespace) });

                                    }

                                    while (instances.MoveNext());

                                }

                                catch (Exception) { }

                        }

                    }
                    catch (Exception)

                    {
                        // MessageBox.Show(ex.Message);
                    }

                if (WMIItemTypes.HasFlag(WMIItemTypes.Class))

                    try

                    {

                        // MessageBox.Show(wmiItemInfo.Path.Substring(0, wmiItemInfo.Path.Length - ":__NAMESPACE".Length));
                        // managementClass = new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path.Substring(0, Path.Path.Length - ":__NAMESPACE".Length)), Path.Factory?.Options?.ObjectGetOptions);

                        //#if DEBUG
                        //                        if (Path.Path.Contains("CIM"))

                        //                            MessageBox.Show(instances.Count.ToString());
                        //#endif

                        ManagementBaseObject instance;

                        using (ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory?.Options?.EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(Path.Factory?.Options?.EnumerationOptions)).GetEnumerator())

                        {

                            while (instances.MoveNext())

                                try

                                {

                                    do

                                    {

                                        instance = instances.Current;

                                        _path = WMIItemInfo.GetPath(instance, WMIItemType.Class);

                                        if (CheckFilter(_path))

                                            _ = paths.AddLast(new PathInfo() { ManagementObject = instance, Path = _path, Name = WMIItemInfo.GetName(instance, WMIItemType.Class) });

                                    } while (instances.MoveNext());

                                }

                                catch (Exception) { }

                        }

                    }

                    catch (Exception) { }

            }

            else if (Path.WMIItemType == WMIItemType.Class && WMIItemTypes.HasFlag(WMIItemTypes.Instance))

            {

                try
                {

                    managementClass.Get();

                    ManagementBaseObject instance;

                    using (ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.Factory?.Options?.EnumerationOptions)).GetEnumerator())

                        while (instances.MoveNext())

                            try

                            {

                                do

                                {

                                    instance = instances.Current;

                                    _path = WMIItemInfo.GetPath(instance, WMIItemType.Instance);

                                    if (CheckFilter(_path))

                                        _ = paths.AddLast(new PathInfo() { ManagementObject = instance, Path = _path, Name = WMIItemInfo.GetName(instance, WMIItemType.Instance) });

                                } while (instances.MoveNext());

                            }
                            catch (Exception) { }

                }

                catch (Exception)

                {
                    // MessageBox.Show(ex.Message);
                }

                if (dispose)

                    managementClass.Dispose();

            }



            IEnumerable<PathInfo> pathInfos;



            if (FileSystemObjectComparer == null)

                pathInfos = paths;

            else

            {

                var _paths = paths.ToList();

                _paths.Sort((IComparer<PathInfo>)FileSystemObjectComparer);

                pathInfos = _paths;

            }



            PathInfo path_;



            using (IEnumerator<PathInfo> _paths = pathInfos.GetEnumerator())



                while (_paths.MoveNext())

                    try

                    {

                        do

                        {

                            path_ = _paths.Current;

                            // new_Path.LoadThumbnail();

                            ReportProgress(0, path_.IsRoot ? Path.Factory.GetBrowsableObjectInfo() : Path.Factory.GetBrowsableObjectInfo( path_.Path, path_.WMIItemType, () =>    path_.ManagementObject));

                        } while (_paths.MoveNext());

                    }
                    catch (Exception) { }

        }

        public struct PathInfo : IFileSystemObject
        {

            /// <summary>
            /// Gets the path of this <see cref="PathInfo"/>.
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Gets the localized name of this <see cref="PathInfo"/>.
            /// </summary>
            public string LocalizedName => Name;

            /// <summary>
            /// Gets the name of this <see cref="PathInfo"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets the <see cref="WinCopies.IO.FileType"/> of this <see cref="PathInfo"/>.
            /// </summary>
            public FileType FileType => FileType.SpecialFolder;

            public ManagementBaseObject ManagementObject { get; set; }

            public bool IsRoot { get; set; }

            public WMIItemType WMIItemType { get; set; }

            public bool Equals(IFileSystemObject fileSystemObject) => ReferenceEquals(this, fileSystemObject)
                    ? true : fileSystemObject is IBrowsableObjectInfo _obj ? FileType == _obj.FileType && Path.ToLower() == _obj.Path.ToLower()
                    : false;

            public int CompareTo(IFileSystemObject fileSystemObject) => BrowsableObjectInfo.GetDefaultComparer().Compare(this, fileSystemObject);

        }
    }

}
