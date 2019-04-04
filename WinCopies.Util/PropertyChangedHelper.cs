using System;
using System.ComponentModel;
using System.Reflection;

namespace WinCopies.Util
{
    internal static class PropertyChangedHelper
    {
        internal static void OnPropertyChangedHelper(INotifyPropertyChanged @object, String propertyName, String fieldName, Object previousValue, Object newValue)

        {

            @object.raise

#if DEBUG

            Console.WriteLine(String.Format("Property name: {0}, previous value: {1}, new value: {2}, is read-only: false", propertyName, previousValue == null ? "null" : previousValue.ToString(), newValue == null ? "null" : newValue.ToString()));

#endif

        }

#if DEBUG 

        internal static void OnPropertyChangedReadOnlyHelper(INotifyPropertyChanged @object, String propertyName, Object previousValue, Object newValue)

        {

            Console.WriteLine(String.Format("Property name: {0}, previous value: {1}, new value: {2}, is read-only: true", propertyName, previousValue == null ? "null" : previousValue.ToString(), newValue == null ? "null" : newValue.ToString()));

        }

#endif
    }
}
