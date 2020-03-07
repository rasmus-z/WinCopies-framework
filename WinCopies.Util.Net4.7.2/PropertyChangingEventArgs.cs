using System;

namespace WinCopies.Util.Shared
{
    public class PropertyChangingEventArgs : System.ComponentModel.PropertyChangingEventArgs
    {
        public Object PreviousValue { get; set; } = null;

        public Object NewValue { get; set; } = null;

        public PropertyChangingEventArgs(String propertyName, Object previousValue, Object newValue) : base(propertyName)

        {

            PreviousValue = previousValue;

            NewValue = newValue;

        }
    }
}
