//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.IO.FilesProcesses
//{

//    public static class CommonHelper
//    {

//        public static void OnPropertyChangedHelper(INotifyPropertyChanged @object, string propertyName, string fieldName, object previousValue, object newValue)

//        {

//            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
//                         BindingFlags.Static | BindingFlags.Instance |
//                         BindingFlags.DeclaredOnly;
//            @object.GetType().GetField(fieldName, flags).SetValue(@object, newValue);

//#if DEBUG

//            Debug.WriteLine(string.Format("Property name: {0}, previous value: {1}, new value: {2}, is read-only: false", propertyName, previousValue == null ? "null": previousValue.ToString(), newValue == null ? "null" :   newValue.ToString()));

//#endif

//        }

//#if DEBUG 

//        public static void OnPropertyChangedReadOnlyHelper(INotifyPropertyChanged @object, String propertyName, Object previousValue, Object newValue)

//        {

//            Debug.WriteLine(string.Format("Property name: {0}, previous value: {1}, new value: {2}, is read-only: true", propertyName, previousValue==null?"null": previousValue.ToString(), newValue==null?"null" : newValue.ToString()));

//        }

//#endif

//    }

//}
