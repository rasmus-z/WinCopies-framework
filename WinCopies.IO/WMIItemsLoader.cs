using System;
using System.Collections.Generic;
using System.Management;
using System.Windows;
using WinCopies.Util;

namespace WinCopies.IO
{

    /// <summary>
    /// A class for easier <see cref="ManagementBaseObject"/> items loading.
    /// </summary>
    public class WMIItemsLoader : BrowsableObjectInfoItemsLoader, IWMIItemsLoader
    {

        private readonly WMIItemTypes _wmiItemTypes;

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public WMIItemTypes WMIItemTypes { get => _wmiItemTypes; set => this.SetBackgroundWorkerProperty(nameof(WMIItemTypes), nameof(_wmiItemTypes), value, typeof(WMIItemsLoader), true); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, WMIItemTypes wmiItemTypes) : this(workerReportsProgress, workerSupportsCancellation, new FileSystemObjectComparer(), wmiItemTypes) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowsableObjectInfoItemsLoader"/> class using a custom comparer.
        /// </summary>
        /// <param name="workerReportsProgress">Whether the thread can notify of the progress.</param>
        /// <param name="workerSupportsCancellation">Whether the thread supports the cancellation.</param>
        /// <param name="fileSystemObjectComparer">The comparer used to sort the loaded items.</param>
        /// <param name="wmiItemTypes">The WMI item types to load.</param>
        public WMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation, IComparer<IFileSystemObject> fileSystemObjectComparer, WMIItemTypes wmiItemTypes) : base(workerReportsProgress, workerSupportsCancellation, fileSystemObjectComparer) => _wmiItemTypes = wmiItemTypes;

        protected override void OnPathChanging(BrowsableObjectInfo path) => WinCopies.Util.Util.ThrowIfNotType<IWMIItemInfo>(path, nameof(path));

        public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void OnDoWork()
        {

            // We've already checked if Path is actually an IWMIItemInfo in the OnPathChanging method.

            var path = ((WMIItemInfo)Path);

            if ( path .WMIItemType == WMIItemType.Namespace)

            {

                ManagementClass managementClass = path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                ManagementObjectCollection instances;

                var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                List<ManagementBaseObject> sortedInstances;

                try
                {

                    managementClass.Get();

                    instances = path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(path.WMIItemInfoFactory?.Options?.EnumerationOptions);

                    foreach (ManagementBaseObject instance in instances)

                        _ = arrayBuilder.AddLast(instance);

                    sortedInstances = arrayBuilder.ToList();

                    arrayBuilder.Clear();

                    sortedInstances.Sort((ManagementBaseObject x, ManagementBaseObject y) => ((string)x["Name"]).CompareTo((string)y["Name"]));

                    foreach (ManagementBaseObject item in sortedInstances)

                        ReportProgress(0, new WMIItemInfo(item, WMIItemType.Namespace));

                }
                catch (Exception ex)

                {
                    // MessageBox.Show(ex.Message);
                }

                // MessageBox.Show(wmiItemInfo.Path.Substring(0, wmiItemInfo.Path.Length - ":__NAMESPACE".Length));
                managementClass = new ManagementClass(new ManagementScope(Path.Path, path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path.Substring(0, Path.Path.Length - ":__NAMESPACE".Length)), path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                instances = path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(path.WMIItemInfoFactory?.Options?.EnumerationOptions);

#if DEBUG
                if (Path.Path.Contains("CIM"))

                    MessageBox.Show(instances.Count.ToString());
#endif

                foreach (ManagementBaseObject instance in instances)

                    _ = arrayBuilder.AddLast(instance);

                sortedInstances = arrayBuilder.ToList();

                sortedInstances.Sort((ManagementBaseObject x, ManagementBaseObject y) => x.ClassPath.Path.CompareTo(y.ClassPath.Path));

                foreach (ManagementBaseObject item in sortedInstances)

                    try

                    {

                        ReportProgress(0, new WMIItemInfo(item, WMIItemType.Class));

                    }
                    catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ }

            }

            else if (path.WMIItemType == WMIItemType.Class)

            {

                ManagementClass managementClass = path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                ManagementObjectCollection instances;

                var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                List<ManagementBaseObject> sortedInstances;

                try
                {

                    managementClass.Get();

                    instances = path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(path.WMIItemInfoFactory?.Options?.EnumerationOptions);

                    foreach (ManagementBaseObject instance in instances)

                        _ = arrayBuilder.AddLast(instance);

                    sortedInstances = arrayBuilder.ToList();

                    arrayBuilder.Clear();

                    sortedInstances.Sort((ManagementBaseObject x, ManagementBaseObject y) => x.ClassPath.ClassName.CompareTo(y.ClassPath.ClassName)); // ((string)x["Name"]).CompareTo((string)y["Name"]));

                    foreach (ManagementBaseObject item in sortedInstances)

                        ReportProgress(0, new WMIItemInfo(item, WMIItemType.Instance));

                }
                catch (Exception ex)

                {
                    // MessageBox.Show(ex.Message);
                }

            }

        }
    }
}
