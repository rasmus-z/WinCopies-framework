namespace WinCopies.Util
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
    public class NamedObject : ValueObject, INamedObject
    {

        // The IDE0044 warning is disabled for some fields in this class because they are set in the SetProperty extension method that is defined in the WinCopies.Util namespace (see the file 'Extensions.cs' for more details.

#pragma warning disable IDE0044 // Ajouter un modificateur readonly
        private string _name = null;
#pragma warning restore IDE0044 // Ajouter un modificateur readonly

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name { get => _name; set => OnPropertyChanged(nameof(Name), nameof(_name), value,typeof(NamedObject)); }

        public NamedObject() { }

        public NamedObject(string name, object value) : base(value) => _name = name;

    }

    /// <summary>
    /// Provides an object that defines a generic value with an associated name and notifies of the name or value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    public class NamedObject<T> : ValueObject<T>, INamedObject
    {

        // The IDE0044 warning is disabled for some fields in this clas because they are set in the SetProperty extension method that is defined in the WinCopies.Util namespace (see the file 'Extensions.cs' for more details.

#pragma warning disable IDE0044 // Ajouter un modificateur readonly
        private string _name = null;
#pragma warning restore IDE0044 // Ajouter un modificateur readonly

        public string Name { get => _name; set => OnPropertyChanged(nameof(Name), nameof(_name), value,typeof(NamedObject<T>)); }

        public NamedObject() { }

        public NamedObject(string name, T value) : base(value) => _name = name;

    }

}
