using System;
using WinCopies.Util;

namespace WinCopies.Util.Data
{

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    public interface ICheckableObject : IValueObject

    {

        /// <summary>
        /// Gets or sets a value that indicates whether this object is checked.
        /// </summary>
        bool IsChecked { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    public interface ICheckableObject<T> : ICheckableObject, IValueObject<T>

    {

    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    public class CheckableObject : ViewModelBase, ICheckableObject
    {

        public bool IsReadOnly => false;

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the object is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => OnPropertyChanged(nameof(IsChecked), nameof(_isChecked), value, typeof(CheckableObject)); }

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(WinCopies.Util.IValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        private readonly object _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(CheckableObject)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class.
        /// </summary>
        public CheckableObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="value">The value of the object.</param>
        public CheckableObject(bool isChecked, object value)
        {

            _value = value;

            _isChecked = isChecked;

        }
    }

    /// <summary>
    /// Provides an object that defines a generic value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    public class CheckableObject<T> : ViewModelBase, ICheckableObject<T>
    {

        public bool IsReadOnly => false;

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the object is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => OnPropertyChanged(nameof(IsChecked), nameof(_isChecked), value, typeof(CheckableObject<T>)); }

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(WinCopies.Util.IValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(WinCopies.Util.IValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        private readonly T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(CheckableObject)); }

        object WinCopies.Util.IValueObject.Value
        {

            get => _value; set

            {

                if (value is T _value)

                    Value = _value;

                else

                    throw new ArgumentException("Invalid type.", nameof(value));

            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject{T}"/> class.
        /// </summary>
        public CheckableObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject{T}"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="value">The value of the object.</param>
        public CheckableObject(bool isChecked, T value)
        {

            _value = value;

            _isChecked = isChecked;

        }

        //private void SetProperty(string propertyName, string fieldName, object newValue)

        //{

        //    //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //    //             BindingFlags.Static | BindingFlags.Instance |
        //    //             BindingFlags.DeclaredOnly;
        //    //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

        //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue));

        //    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //                 BindingFlags.Static | BindingFlags.Instance |
        //                 BindingFlags.DeclaredOnly;

        //    object previousValue = null;

        //    FieldInfo field = GetType().GetField(fieldName, flags);

        //    previousValue = field.GetValue(this);

        //    if (!newValue.Equals(previousValue))

        //    {

        //        field.SetValue(this, newValue);

        //        //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //        //             BindingFlags.Static | BindingFlags.Instance |
        //        //             BindingFlags.DeclaredOnly;
        //        //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue));

        //    } 

        //}

    }

}
