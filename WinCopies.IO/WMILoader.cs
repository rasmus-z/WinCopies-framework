using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Threading;
using WinCopies.Util;

namespace WinCopies.IO
{
    public interface IWMILoader<T> : IBrowsableObjectInfoLoader<T> where T : IWMIItemInfo
    {

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="IWMILoader{TPath}"/> is busy.</exception>
        WMIItemTypes WMIItemTypes { get; set; }

    }

    /// <summary>
    /// A class for easier <see cref="ManagementBaseObject"/> items loading.
    /// </summary>
    public class WMILoader<TPath, TItems, TFactory> : BrowsableObjectInfoLoader<TPath, TItems, TFactory>, IWMILoader<TPath> where TPath : BrowsableObjectInfo<TItems, TFactory>, IWMIItemInfo where TItems : BrowsableObjectInfo<TItems, TFactory>, IWMIItemInfo where TFactory : BrowsableObjectInfoFactory, IWMIItemInfoFactory
    {

        protected override BrowsableObjectInfoLoader<TPath, TItems, TFactory> DeepCloneOverride() => new WMILoader<TPath, TItems, TFactory>(null, WMIItemTypes, WorkerReportsProgress, WorkerSupportsCancellation, (IFileSystemObjectComparer<IFileSystemObject>)FileSystemObjectComparer.DeepClone());

        private readonly WMIItemTypes _wmiItemTypes;

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMILoader{TPath, TItems, TFactory}"/> is busy.</exception>
        public WMIItemTypes WMIItemTypes { get => _wmiItemTypes; set => this.SetBackgroundWorkerProperty(nameof(WMIItemTypes), nameof(_wmiItemTypes), value, typeof(WMILoader<TPath, TItems, TFactory>), true); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TFactory}"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMILoader(TPath path, WMIItemTypes wmiItemTypes, bool workerReportsProgress, bool workerSupportsCancellation) : this(path, wmiItemTypes, workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer<IFileSystemObject>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoLoader{TPath, TItems, TFactory}"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMILoader(TPath path, WMIItemTypes wmiItemTypes, bool workerReportsProgress, bool workerSupportsCancellation, IFileSystemObjectComparer<IFileSystemObject> fileSystemObjectComparer) : base((TPath)path, workerReportsProgress, workerSupportsCancellation, (IFileSystemObjectComparer<IFileSystemObject>)fileSystemObjectComparer) => _wmiItemTypes = wmiItemTypes;

        //public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            var paths = new ArrayAndListBuilder<PathInfo>();

            string _path;

            bool dispose = false;

// #pragma warning disable IDE0019 // Pattern Matching
            var managementClass = Path.ManagementObject as ManagementClass;
// #pragma warning restore IDE0019 // Pattern Matching

            if (managementClass == null)

            {

                dispose = true;

// #pragma warning disable IDE0067 // Dispose objects before losing scope
                managementClass = new ManagementClass(new ManagementScope(Path.Path, Path.Factory.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.Factory.Options?.ObjectGetOptions);
// #pragma warning restore IDE0067 // Dispose objects before losing scope

            }

            if (Path.WMIItemType == WMIItemType.Namespace)

            {

                // managementClass = Path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, Path.Factory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.Factory?.Options?.ObjectGetOptions);

                if (WMIItemTypes.HasFlag(WMIItemTypes.Namespace))

                    try
                    {

                        managementClass.Get();

                        using (ManagementObjectCollection.ManagementObjectEnumerator instances = (Path.Factory.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.Factory.Options?.EnumerationOptions)).GetEnumerator())
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

                                            _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Namespace), instance, WMIItemType.Namespace));

                                    }

                                    while (instances.MoveNext());

                                }

// #pragma warning disable CA1031 // Do not catch general exception types
                                catch (Exception ex) when (!(ex is ThreadAbortException)) { }
// #pragma warning restore CA1031 // Do not catch general exception types

                        }

                    }

// #pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
// #pragma warning restore CA1031 // Do not catch general exception types

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

                                            _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Class), instance, WMIItemType.Class) { });

                                    } while (instances.MoveNext());

                                }

#pragma warning disable CA1031 // Do not catch general exception types
                                catch (Exception ex) when (!(ex is ThreadAbortException)) { }
#pragma warning restore CA1031 // Do not catch general exception types

                        }

                    }

#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
#pragma warning restore CA1031 // Do not catch general exception types

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

                                        _ = paths.AddLast(new PathInfo(_path, _path.RemoveAccents(), WMIItemInfo.GetName(instance, WMIItemType.Instance), FileType.Other, managementObject => WMIItemInfo.DefaultManagementObjectDeepClone( (ManagementObject) managementObject, null), instance, WMIItemType.Instance));

                                } while (instances.MoveNext());

                            }

#pragma warning disable CA1031 // Do not catch general exception types
                            catch (Exception ex) when (!(ex is ThreadAbortException)) { }
#pragma warning restore CA1031 // Do not catch general exception types

                }

#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex) when (!(ex is ThreadAbortException)) { }
#pragma warning restore CA1031 // Do not catch general exception types

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

                            ReportProgress(0, Path.Factory.GetBrowsableObjectInfo(path_.Path, path_.WMIItemType, path_.ManagementObject, path_.ManagementObjectDelegate /*managementObject => WMIItemInfo.DefaultManagementObjectDeepClone( (ManagementObject) path_.ManagementObject, null )*/));

                        } while (_paths.MoveNext());

                    }

#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex) when (!(ex is ThreadAbortException)) { }
#pragma warning restore CA1031 // Do not catch general exception types

        }

        protected class PathInfo : IO.PathInfo
        {

            /// <summary>
            /// Gets the localized name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string LocalizedName => Name;

            /// <summary>
            /// Gets the name of this <see cref="PathInfo"/>.
            /// </summary>
            public override string Name { get; }

            public DeepClone<ManagementBaseObject> ManagementObjectDelegate { get; }

            public ManagementBaseObject ManagementObject { get; }

            public WMIItemType WMIItemType { get; }

            public PathInfo(string path, string normalizedPath, string name, DeepClone<ManagementBaseObject> managementObjectDelegate, ManagementBaseObject managementObject, WMIItemType wmiItemType) : base(path, normalizedPath)
            {

                Name = name;

                ManagementObjectDelegate = managementObjectDelegate;

                ManagementObject = managementObject;

                WMIItemType = wmiItemType;

            }

        }

    }

}
