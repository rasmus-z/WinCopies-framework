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

        internal static void Init(Explorer.IBrowsableObjectInfoHelper browsableObjectInfo) => browsableObjectInfo.SelectedItems = new ReadOnlyObservableCollection<Explorer.IBrowsableObjectInfo>(((Explorer.IBrowsableObjectInfoInternal)browsableObjectInfo).SelectedItems);

    }
}
