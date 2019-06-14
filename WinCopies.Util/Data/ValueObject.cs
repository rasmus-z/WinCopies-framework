using System;
using System.ComponentModel;

namespace WinCopies.Util.Data
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
    public interface IValueObject<T> : INotifyPropertyChanged

    {

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        T Value { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    [Obsolete("This class has been replaced by the ViewModelBase class and will be removed in laters versions.")]
    public class ValueObject : IValueObject
    {

        private readonly object _value = null;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject)); }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class.
        /// </summary>
        public ValueObject() { }

        /// <summary>
        /// Initilizes a new instance of the <see cref="ValueObject"/> class with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public ValueObject(object value) => _value = value;

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {

            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

    }

    /// <summary>
    /// Provides an object that defines a generic value and notifies of the value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    [Obsolete("This class has been replaced by the ViewModelBase class and will be removed in laters versions.")]
    public class ValueObject<T> : IValueObject
    {

        private readonly T _value = default;

        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject<T>)); }

        object IValueObject.Value { get => Value; set => Value = (T)value; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public ValueObject() { }

        public ValueObject(T value) => _value = value;

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {

            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

    }
}
