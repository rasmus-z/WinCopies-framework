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

        private WMIItemTypes _wmiItemTypes = WinCopies.Util.Util.GetAllEnumFlags<WMIItemTypes>();

        private ConnectionOptions _connectionOptions;

        private ObjectGetOptions _objectGetOptions;

        private EnumerationOptions _enumerationOptions;

        public WMIItemTypes WMIItemTypes

        {

            get => _wmiItemTypes;

            set

            {

                this.set

            }

        }

        /// <summary>
        /// Gets or sets options for the WMI connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public ConnectionOptions ConnectionOptions

        {

            get => _connectionOptions;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException($"The {nameof(WMIItemsLoader)} is busy.");

                _connectionOptions = value;

            }

        }

        /// <summary>
        /// Gets or sets options for getting management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public ObjectGetOptions ObjectGetOptions

        {

            get => _objectGetOptions;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException($"The {nameof(WMIItemsLoader)} is busy.");

                _objectGetOptions = value;

            }

        }

        /// <summary>
        /// Gets or sets options for management objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        public EnumerationOptions EnumerationOptions

        {

            get => _enumerationOptions;

            set

            {

                if (IsBusy)

                    throw new InvalidOperationException($"The {nameof(WMIItemsLoader)} is busy.");

                _enumerationOptions = value;

            }

        }

        public WMIItemsLoader(bool workerReportsProgress, bool workerSupportsCancellation) : base(workerReportsProgress, workerSupportsCancellation)
        {
        }

        public override bool CheckFilter(string path) => throw new NotImplementedException();

        protected override void InitializePath() { }

        protected override void OnDoWork()
        {

            if (Path is WMIItemInfo wmiItemInfo)

            {

                try
                {

                    if (wmiItemInfo.WMIItemType == WMIItemType.Namespace)

                    {

                        ManagementClass managementClass = wmiItemInfo.ManagementObject as ManagementClass ?? new ManagementClass(new ManagementScope(wmiItemInfo.Path, ConnectionOptions), new ManagementPath(wmiItemInfo.Path + ":__NAMESPACE"), ObjectGetOptions);

                        managementClass.Get();

                        ManagementObjectCollection instances = EnumerationOptions == null ? managementClass.GetInstances() : managementClass.GetInstances(EnumerationOptions);

                        var arrayBuilder = new ArrayAndListBuilder<ManagementBaseObject>();

                        foreach (ManagementBaseObject instance in instances)

                            _ = arrayBuilder.AddLast(instance);

                        var sortedInstances = arrayBuilder.ToList();

                        arrayBuilder.Clear();

                        sortedInstances.Sort((ManagementBaseObject x, ManagementBaseObject y) => ((string)x["Name"]).CompareTo((string)y["Name"]));

                        foreach (ManagementBaseObject item in sortedInstances)

                            ReportProgress(0, new WMIItemInfo(item, WMIItemType.Namespace));

                        instances = EnumerationOptions == null ? managementClass.GetSubclasses() : managementClass.GetSubclasses(EnumerationOptions);

                        foreach (ManagementBaseObject instance in instances)

                            _ = arrayBuilder.AddLast(instance);

                        sortedInstances = arrayBuilder.ToList();

                        sortedInstances.Sort((ManagementBaseObject x, ManagementBaseObject y) => x.ClassPath.Path.CompareTo(y.ClassPath.Path));

                        foreach (ManagementBaseObject item in sortedInstances)

                            try

                            {

                                ReportProgress(0, new WMIItemInfo(item, WMIItemType.Class));

                            }
                            catch (Exception) { }

                    }

                }
                catch (Exception ex)

                {

                }

            }

        }
    }
}
