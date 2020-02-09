using System.ComponentModel;
using System.Reflection;

namespace WinCopies.Util
{
    public abstract class NotifyingPropertyChangeObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyingPropertyChangeObject()

        {

            PropertyChanging += NotifyingPropertyChangeObject_PropertyChanging;

        }

        protected void ChangePropertyValue(string propertyName, string fieldName, object previousValue, object newValue)

        {

            OnPropertyChanging(new PropertyChangingEventArgsInternal(propertyName, previousValue, newValue, fieldName));

        }

        protected virtual void OnPropertyChanging(object sender, PropertyChangingEventArgs e)

        {

            if (e.GetType() == typeof(PropertyChangingEventArgsInternal))

            {

                

            }

        }

        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {

        }
    }
}
