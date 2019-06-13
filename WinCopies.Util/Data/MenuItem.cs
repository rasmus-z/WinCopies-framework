using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WinCopies.Util.Data
{
    public class MenuItem : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The method that is called to set a value to a property. If succeed, then call the <see cref="OnPropertyChanged(string, object, object)"/> method. See the Remarks section.
        /// </summary>
        /// <param name="propertyName">The name of the property to set in a new value into</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType)

        {

            if (IsSeparator)

                throw new InvalidOperationException($"This {nameof(MenuItem)} is a separator.");

            var (propertyChanged, oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// The method that is called to notify that a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property which has changed.</param>
        /// <param name="oldValue">The old value of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        private readonly object _header = null;

        /// <summary>
        /// Gets or sets a value that represents the header of this item.
        /// </summary>
        public object Header { get => _header; set => OnPropertyChanged(nameof(Header), nameof(_header), value, typeof(MenuItem)); }

        private readonly ImageSource _icon = null;

        public ImageSource Icon { get => _icon; set => OnPropertyChanged(nameof(Icon), nameof(_icon), value, typeof(MenuItem)); }

        private readonly ICommand _command = null;

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> that represents the action to do when user click on this menu item.
        /// </summary>
        public ICommand Command { get => _command; set => OnPropertyChanged(nameof(Command), nameof(_command), value, typeof(MenuItem)); }

        private readonly object _commandParameter = null;

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="CommandParameter"/> property.
        /// </summary>
        public object CommandParameter { get => _commandParameter; set => OnPropertyChanged(nameof(CommandParameter), nameof(_commandParameter), value, typeof(MenuItem)); }

        private readonly IInputElement _commandTarget = null;

        public IInputElement CommandTarget { get => _commandTarget; set => OnPropertyChanged(nameof(CommandTarget), nameof(_commandTarget), value, typeof(MenuItem)); }

        private readonly bool _isCheckable = false;

        /// <summary>
        /// Gets or sets a value that indicates whether this menu item can be checked.
        /// </summary>
        public bool IsCheckable { get => _isCheckable; set => OnPropertyChanged(nameof(IsCheckable), nameof(_isCheckable), value, typeof(MenuItem)); }

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether this menu item is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => OnPropertyChanged(nameof(IsChecked), nameof(_isChecked), value, typeof(MenuItem)); }

        // todo: do not reduce this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also TabControl.xaml.cs for the other use of this class in this solution.

        /// <summary>
        /// Gets or sets the items of this menu item.
        /// </summary>
        public ObservableCollection<MenuItem> Items { get; } = new ObservableCollection<MenuItem>();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="MenuItem"/> represents a separator.
        /// </summary>
        public bool IsSeparator { get; private set; } = false;

        public MenuItem()
        {

        }

        public MenuItem(bool isSeparator) => IsSeparator = isSeparator;

        public MenuItem(object header) => _header = header;

        public MenuItem(object header, ImageSource icon)

        {

            _header = header;

            _icon = icon;

        }

        public MenuItem(object header, ImageSource icon, ICommand command, object commandParameter, IInputElement commandTarget)
        {

            _header = header;

            _icon = icon;

            _command = command;

            _commandParameter = commandParameter;

            _commandTarget = commandTarget;

        }

        public MenuItem(object header, IEnumerable<MenuItem> items)
        {

            _header = header;

            Items.AddRange(items);

        }

    }
}
