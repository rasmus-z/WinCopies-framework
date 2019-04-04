//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Reflection;
//using System.Windows.Input;

//namespace WinCopies.GUI.Controls
//{

//    public class MenuItemData : INotifyPropertyChanged
//    {

//        private object header = null;

//        public object Header

//        {

//            get => header;

//            set

//            {

//                SetProperty("Header", "header", value);

//            }

//        }

//        private ICommand command = null;

//        public ICommand Command

//        {

//            get => command;

//            set

//            {

//                SetProperty("Command", "command", value);

//            }

//        }

//        private object commandParameter = null;

//        public object CommandParameter

//        {

//            get => commandParameter;

//            set

//            {

//                SetProperty("CommandParameter", "commandParameter", value);

//            }

//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MenuItemData"/> class.
//        /// </summary>
//        public MenuItemData()

//        {

//            IsSeparator = false;

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MenuItemData"/> class with possibility to indicate if this <see cref="MenuItemData"/> represents a separator.
//        /// </summary>
//        /// <param name="isSeparator">A <see cref="System.Boolean"/> value indicating if this <see cref="MenuItemData"/> represents a separator.</param>
//        public MenuItemData(bool isSeparator)

//        {

//            IsSeparator = isSeparator;

//        }

//        private void SetProperty(string propertyName, string fieldName, object newValue)

//        {

//            if (IsSeparator)

//                throw new System.Exception("IsSeparator is set to true.");

//            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
//                         BindingFlags.Static | BindingFlags.Instance |
//                         BindingFlags.DeclaredOnly;

//            object previousValue = null;

//            FieldInfo field = this.GetType().GetField(fieldName, flags);

//            previousValue = field.GetValue(this);

//            // todo: perform this check for all this kind of classes and structs in this solution

//            if (!newValue.Equals(previousValue))

//            {

//                field.SetValue(this, newValue);

//                PropertyChanged?.Invoke(this, new WinCopies.Util.PropertyChangedEventArgs(propertyName, previousValue, newValue));

//            }

//        }
//    }
//}
