using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Explorer
{
    static class BrowsableObjectInfoHelper
    {

        internal static void Init(IBrowsableObjectInfoHelper browsableObjectInfo) => browsableObjectInfo.SelectedItems = new ReadOnlyObservableCollection<IBrowsableObjectInfo>(((IBrowsableObjectInfoInternal)browsableObjectInfo).SelectedItems);

    }
}
