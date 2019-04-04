﻿using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.ComponentModel;

namespace WinCopies.IO
{
    /// <summary>
    /// Provides a container for the <see cref="IShellProperty"/> interface that notifies when the property has changed.
    /// </summary>
    public class ShellPropertyContainer : IShellProperty, INotifyPropertyChanged
    {

        private IShellProperty Property { get; set; } = null;

        /// <summary>
        /// Gets the property key that identifies this property.
        /// </summary>
        public PropertyKey PropertyKey => Property.PropertyKey;

        /// <summary>
        /// Get the property description object.
        /// </summary>
        public ShellPropertyDescription Description => Property.Description;

        /// <summary>
        /// Gets the case-sensitive name of the property as it is known to the system, regardless of its localized name.
        /// </summary>
        public string CanonicalName => Property.CanonicalName;

        /// <summary>
        /// Gets or sets a value that indicates whether this property has changed.
        /// </summary>
        public bool HasChanged { get; private set; } = false;

        /// <summary>
        /// Gets the image reference path and icon index associated with a property value. This API is only available in Windows 7.
        /// </summary>
        public IconReference IconReference => Property.IconReference;

        // public bool HasChanged => (Property.ValueAsObject == null && value != null) || (Property.ValueAsObject != null && value == null) || (Property.ValueAsObject != null && !Property.ValueAsObject.Equals(value));

        public bool IsReadOnly => Property.Description.TypeFlags.HasFlag(PropertyTypeOptions.IsInnate);

        private object value = null;

        /// <summary>
        /// Gets the value for this property using the generic Object type.
        /// </summary>
        /// <remarks>To obtain a specific type for this value, use the more strongly-typed <see cref="ShellProperty{T}" /> class. You can only set a value for this type using the <see cref="ShellProperty{T}" /> class.</remarks>
        public object ValueAsObject
        {

            get => value; set

            {
                object previous_Value = value; this.value = value; PropertyChanged?.Invoke(this, new WinCopies. Util.PropertyChangedEventArgs(nameof(ValueAsObject), previous_Value, value)); 

                HasChanged = (Property.ValueAsObject == null && value != null) || (Property.ValueAsObject != null && value == null) || (Property.ValueAsObject != null && !Property.ValueAsObject.Equals(value));

                PropertyChanged?.Invoke(this, new WinCopies. Util.PropertyChangedEventArgs(nameof(HasChanged), !HasChanged, HasChanged)); 

            }

        }

        /// <summary>
        /// Gets the <see cref="System.Type" /> value for this property.
        /// </summary>
        public System.Type ValueType => Property.ValueType;

        public event PropertyChangedEventHandler PropertyChanged;

        //public static IShellPropertyContainer CreateNew(IShellProperty property) 

        //{

        //    if (property.Description.TypeFlags.HasFlag(PropertyTypeOptions.IsInnate))

        //        return new ReadOnlyShellPropertyContainer(property);

        //    else

        //        return new ShellPropertyContainer(property);

        //}

        public ShellPropertyContainer(IShellProperty property) 

        {

            Property = property;

            value = property.ValueAsObject;

        }

        /// <summary>
        /// Gets a formatted, Unicode string representation of a property value.
        /// </summary>
        /// <param name="format">One or more PropertyDescriptionFormat flags chosen to produce the desired display format.</param>
        /// <returns>The formatted value as a string.</returns>
        public string FormatForDisplay(PropertyDescriptionFormatOptions format) => Property.FormatForDisplay(format);

    }

    //public class ReadOnlyShellPropertyContainer : IShellPropertyContainer

    //{

    //    public IShellProperty Property { get; set; } = null;

    //    public bool IsReadOnly => true;

    //    private object value = null;

    //    public object Value { get => value; set => throw new System.Exception("Value is read-only."); }

    //    public ReadOnlyShellPropertyContainer(IShellProperty property)

    //    {

    //        Property = property;

    //        value = property.ValueAsObject;

    //    }

    //}
}
