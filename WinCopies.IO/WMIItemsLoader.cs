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
    public class WMIItemsLoader : BrowsableObjectInfoItemsLoader
    {

        private readonly WMIItemTypes _wmiItemTypes = Util.Util.GetAllEnumFlags<WMIItemTypes>();

#pragma warning disable CS0649 // Set up using reflection.
        private readonly ConnectionOptions _connectionOptions;

        private readonly ObjectGetOptions _objectGetOptions;

        private readonly EnumerationOptions _enumerationOptions;
#pragma warning restore CS0649

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public WMIItemTypes WMIItemTypes { get => _wmiItemTypes; set => this.SetBackgroundWorkerProperty(nameof(WMIItemTypes), nameof(_wmiItemTypes), value, typeof(WMIItemsLoader), true); }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public ConnectionOptions ConnectionOptions { get => _connectionOptions; set => this.SetBackgroundWorkerProperty(nameof(ConnectionOptions), nameof(_connectionOptions), value, typeof(WMIItemsLoader), true); }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public ObjectGetOptions ObjectGetOptions { get => _objectGetOptions; set => this.SetBackgroundWorkerProperty(nameof(ObjectGetOptions), nameof(_objectGetOptions), value, typeof(WMIItemsLoader), true); }

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public EnumerationOptions EnumerationOptions { get => _enumerationOptions; set => this.SetBackgroundWorkerProperty(nameof(EnumerationOptions), nameof(_enumerationOptions), value, typeof(WMIItemsLoader), true); }

        public WMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) : base(workerReportsProgress, workerSupportsCancellation)        {        }

        public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void InitializePath() { }

        protected override void OnDoWork()
        {

            if (Path is WMIItemInfo wmiItemInfo)

            {

                if (wmiItemInfo.WMIItemType == WMIItemType.Namespace)

                {

                    ManagementClass managementClass = wmiItemInfo.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(wmiItemInfo.Path, ConnectionOptions), new ManagementPath(wmiItemInfo.Path), ObjectGetOptions);

                    ManagementObjectCollection instances;

                    var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                    List<ManagementBaseObject> sortedInstances;

                    try
                    {

                        managementClass.Get();

                        instances = EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(EnumerationOptions);

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
                    managementClass = new ManagementClass(new ManagementScope(wmiItemInfo.Path, ConnectionOptions), new ManagementPath(wmiItemInfo.Path.Substring(0, wmiItemInfo.Path.Length - ":__NAMESPACE".Length)), ObjectGetOptions);

                    instances = EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(EnumerationOptions);

                    if (wmiItemInfo.Path.Contains("CIM"))
                        MessageBox.Show(instances.Count.ToString());
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

                else if (wmiItemInfo.WMIItemType == WMIItemType.Class)

                {

                    ManagementClass managementClass = wmiItemInfo.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(wmiItemInfo.Path, ConnectionOptions), new ManagementPath(wmiItemInfo.Path), ObjectGetOptions);

                    ManagementObjectCollection instances;

                    ArrayAndListBuilder<ManagementBaseObject> arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                    List<ManagementBaseObject> sortedInstances;

                    try
                    {

                        managementClass.Get();

                        instances = EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(EnumerationOptions);

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
}
