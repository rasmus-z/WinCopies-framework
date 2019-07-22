using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    public interface IWMIItemsLoader : IBrowsableObjectInfoItemsLoader
    {

        /// <summary>
        /// Gets or sets the WMI item types to load.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown when this property is set while the <see cref="WMIItemsLoader"/> is busy.</exception>
        WMIItemTypes WMIItemTypes { get; set; }
}
}
