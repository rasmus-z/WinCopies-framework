using System;
using System.ComponentModel;

namespace WinCopies.Util
{

    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    public interface IValueObject : INotifyPropertyChanged

    {

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        object Value { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    public class ValueObject : IValueObject
    {

        // The IDE0044 warning is disabled for some fields in this class because they are set in the SetProperty extension method that is defined in the WinCopies.Util namespace (see the file 'Extensions.cs' for more details.

        private readonly object _value = null;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject)); }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The method that is called to set a value to a property. If succeed, then call the <see cref="OnPropertyChanged(string, object, object)"/> method. See the Remarks section.
        /// </summary>
        /// <param name="propertyName">The name of the property to set in a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {

            var (propertyChanged, oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class.
        /// </summary>
        public ValueObject() { }

        /// <summary>
        /// Initilizes a new instance of the <see cref="ValueObject"/> class with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public ValueObject(object value) => _value = value;

    }

    /// <summary>
    /// Provides an object that defines a generic value and notifies of the value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    public class ValueObject<T> : INotifyPropertyChanged, IValueObject
    {

        // The IDE0044 warning is disabled for some fields in this clas because they are set in the SetProperty extension method that is defined in the WinCopies.Util namespace (see the file 'Extensions.cs' for more details.

#pragma warning disable IDE0044 // Ajouter un modificateur readonly
        private T _value = default(T);
#pragma warning restore IDE0044 // Ajouter un modificateur readonly

        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject<T>)); }

        object IValueObject.Value { get => Value; set => Value = (T)value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {

            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        public ValueObject() { }

        public ValueObject(T value) => _value = value;

    }
}
