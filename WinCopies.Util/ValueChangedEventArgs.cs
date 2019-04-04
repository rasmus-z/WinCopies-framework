using System;

namespace WinCopies.Util
{
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e); 

    public class ValueChangedEventArgs : EventArgs
    {

        public object OldValue { get; } = null;

        public object NewValue { get; } = null;

        public ValueChangedEventArgs(object oldValue, object newValue)

        {

            this.OldValue = oldValue;

            this.NewValue = newValue;

        } 

    }
}
