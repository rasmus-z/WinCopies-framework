namespace WinCopies.Util.Data
{

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    public interface ICheckableObject : IValueObject

    {

        bool IsChecked { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    public interface ICheckableObject<T> : IValueObject<T>

    {

        bool IsChecked { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    public class CheckableObject : ViewModelBase, ICheckableObject
    {

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the object is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => OnPropertyChanged(nameof(IsChecked), nameof(_isChecked), value, typeof(CheckableObject)); }

        private readonly object _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(CheckableObject)); }

        public CheckableObject() { }

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

        private readonly bool _isChecked = false;

        public bool IsChecked { get => _isChecked; set => OnPropertyChanged(nameof(IsChecked), nameof(_isChecked), value, typeof(CheckableObject<T>)); }

        private readonly T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(CheckableObject)); }

        public CheckableObject() { }

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
