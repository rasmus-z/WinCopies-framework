namespace WinCopies.Util.Data
{

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject : IValueObject

    {

        string Name { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject<T> : IValueObject<T>

    {

        string Name { get; set; }

    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public class NamedObject : ViewModelBase, INamedObject
    {

        private readonly string _name = null;

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name { get => _name; set => OnPropertyChanged(nameof(Name), nameof(_name), value, typeof(NamedObject)); }

        private readonly object _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(NamedObject)); }

        public NamedObject() { }

        public NamedObject(string name, object value) => _name = name;

    }

    /// <summary>
    /// Provides an object that defines a generic value with an associated name and notifies of the name or value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    public class NamedObject<T> : ViewModelBase, INamedObject<T>
    {

        private readonly string _name = null;

        public string Name { get => _name; set => OnPropertyChanged(nameof(Name), nameof(_name), value, typeof(NamedObject<T>)); }

        private readonly T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(NamedObject)); }

        public NamedObject() { }

        public NamedObject(string name, T value)
        {

            _value = value;

            _name = name;

        }
    }

}
