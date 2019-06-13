using System.ComponentModel;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides data for the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
    /// </summary>
    public class PropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        public object PreviousValue { get; set; } = null;

        public object NewValue { get; set; } = null;

        public PropertyChangedEventArgs(string propertyName) : base(propertyName)        { }

        public PropertyChangedEventArgs(string propertyName, object previousValue, object newValue) : base(propertyName)

        {

            PreviousValue = previousValue;

            NewValue = newValue;

        }
    }
}
