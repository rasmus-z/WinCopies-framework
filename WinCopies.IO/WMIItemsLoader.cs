using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
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

        public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void InitializePath() { }

        protected override void OnDoWork()
        {

            if (Path.WMIItemType == WMIItemType.Namespace)

            {

                ManagementClass managementClass = Path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, Path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                ManagementObjectCollection instances;

                var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                List<ManagementBaseObject> sortedInstances;

                try
                {

                    managementClass.Get();

                    instances = Path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.WMIItemInfoFactory?.Options?.EnumerationOptions);

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
                managementClass = new ManagementClass(new ManagementScope(Path.Path, Path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path.Substring(0, Path.Path.Length - ":__NAMESPACE".Length)), Path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                instances = Path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(Path.WMIItemInfoFactory?.Options?.EnumerationOptions);

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

            else if (Path.WMIItemType == WMIItemType.Class)

            {

                ManagementClass managementClass = Path.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(Path.Path, Path.WMIItemInfoFactory?.Options?.ConnectionOptions), new ManagementPath(Path.Path), Path.WMIItemInfoFactory?.Options?.ObjectGetOptions);

                ManagementObjectCollection instances;

                var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                List<ManagementBaseObject> sortedInstances;

                try
                {

                    managementClass.Get();

                    instances = Path.WMIItemInfoFactory?.Options?.EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(Path.WMIItemInfoFactory?.Options?.EnumerationOptions);

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
