using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static WinCopies.Util.Generic;

namespace WinCopies.Util
{
    /// <summary>
    /// Provides some static extensions methods.
    /// </summary>
    public static class Extensions
    {

        public static bool AddIfNotContains<T>(this IList<T> collection, T value)

        {

            if (collection.Contains(value)) return false;

            collection.Add(value);

            return true;

        }

        public static IList<T> AddRangeIfNotContains<T>(this IList<T> collection, IEnumerable<T> values)

        {

            List<T> addedValues = new List<T>();

            foreach (T value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Add(value);

                addedValues.Add(value);

            }

            return addedValues;

        }

        public static bool InsertIfNotContains<T>(this IList<T> collection, int index, T value)

        {

            if (collection.Contains(value)) return false;

            collection.Insert(index, value);

            return true;

        }

        public static IList<T> InsertRangeIfNotContains<T>(this IList<T> collection, int index, IEnumerable<T> values)

        {

            List<T> addedValues = new List<T>();

            foreach (T value in values)

            {

                if (collection.Contains(value)) continue;

                collection.Insert(index, value);

                addedValues.Add(value);

            }

            return addedValues;

        }

        public static bool RemoveIfContains<T>(this IList<T> collection, T value)

        {

            if (collection.Contains(value))

            {

                collection.Remove(value);

                return true;

            }

            return false;

        }

        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Key;

        }

        public static IEnumerable<TValue> GetValues<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Value;

        }

        public static bool CheckIntegrity<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)

        {

            bool predicateByVal(TKey keyA, TKey keyB) => Equals(keyA, keyB);

            bool predicateByRef(TKey keyA, TKey keyB) => ReferenceEquals(keyA, keyB);

            Func<TKey, TKey, bool> predicate = typeof(TKey).IsClass ? predicateByRef : (Func<TKey, TKey, bool>)predicateByVal;

            IEnumerable<TKey> keys = array.GetKeys();

            IEnumerable<TKey> _keys = array.GetKeys();

            bool foundOneOccurrence = false;

            foreach (TKey key in keys)

            {

                if (key == null)

                    throw new ArgumentException("One or more key is null.");

                foreach (TKey _key in _keys)

                {

                    if (predicate(key, _key))

                        if (foundOneOccurrence)

                            return false;

                        else

                            foundOneOccurrence = true;

                }

                foundOneOccurrence = false;

            }

            return true;

        }

        public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static bool CheckPropertySetIntegrity(Type propertyObjectType, string propertyName, out string methodName, int skipFramesForStackFrame, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet)

        {

            PropertyInfo property = propertyObjectType.GetProperty(propertyName, bindingFlags);

            if (property == null)

                throw new ArgumentException(string.Format(FieldOrPropertyNotFound, propertyName, propertyObjectType));

            MethodBase method = new StackFrame(skipFramesForStackFrame).GetMethod();

            methodName = method.Name;

            //#if DEBUG 

            //            Debug.WriteLine("Property: " + property.Name + ", " + property.DeclaringType);

            //            Debug.WriteLine("Method: " + method.Name + ", " + method.DeclaringType);

            //#endif 

            return (property.CanWrite && property.GetSetMethod() != null) || property.DeclaringType == method.DeclaringType;

        }

        private static FieldInfo GetField(string fieldName, Type objectType, BindingFlags bindingFlags)

        {

            BindingFlags flags = bindingFlags;

            // var objectType = obj.GetType(); 

            FieldInfo field = objectType.GetField(fieldName, flags);

            if (field == null)

                throw new ArgumentException(string.Format(FieldOrPropertyNotFound, fieldName, objectType));

            return field;

        }

        public static (bool propertyChanged, object oldValue) SetProperty(this INotifyPropertyChanged obj, string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet)

        {

            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
            //             BindingFlags.Static | BindingFlags.Instance |
            //             BindingFlags.DeclaredOnly;
            //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

            // if (declaringType == null) 

            // {

            //while (objectType != declaringType && objectType != typeof(object))

            //    objectType = objectType.BaseType;

            //if (objectType != declaringType)

            //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

            // }

            //#if DEBUG

            //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //            foreach (var _field in fields)

            //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

            //#endif

            string methodName;

            // var objectType = obj.GetType();

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            object previousValue = field.GetValue(obj);

            if (performIntegrityCheck && !CheckPropertySetIntegrity(declaringType, propertyName, out methodName, 3, bindingFlags))

                throw new InvalidOperationException(string.Format(DeclaringTypesNotCorrespond, propertyName, methodName));

            if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))

            {

                field.SetValue(obj, newValue);

                return (true, previousValue);

                //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                //             BindingFlags.Static | BindingFlags.Instance |
                //             BindingFlags.DeclaredOnly;
                //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            }

            else

                return (false, previousValue);

        }

        public static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged

        {

            if (bgWorker.IsBusy)

                if (throwIfBusy)

                    throw new InvalidOperationException("Cannot change property value when BackgroundWorker is busy.");

                else

                    return (false, GetField(fieldName, declaringType, bindingFlags).GetValue(bgWorker));

            else

                return bgWorker.SetProperty(propertyName, fieldName, newValue, declaringType, performIntegrityCheck, bindingFlags);

        }

        /// <summary>
        /// Appends data to the table. The item type is not checked. Arrays must have only a dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static Array Append(this Array array, params Array[] arrays) => Util.Concatenate((object[])array, arrays);

        /// <summary>
        /// Appends data to the table using the <see cref="Array.LongLength"/> length property. The item type is not checked. Arrays must have only a dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static Array AppendLong(this Array array, params Array[] arrays) => Util.ConcatenateLong((object[])array, arrays);

        /// <summary>
        /// Sort an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        public static void Sort<T>(this System.Collections.ObjectModel.ObservableCollection<T> oc)

        {

            IList<T> sorted = oc.OrderBy(x => x).ToList();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);

        }

        /// <summary>
        /// Sort an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> with a user-defined comparer.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> providing comparison for sorting the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</param>
        public static void Sort<T>(this System.Collections.ObjectModel.ObservableCollection<T> oc, IComparer<T> comparer)

        {

            IList<T> sorted = oc.OrderBy(x => x, comparer).ToList();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);

        }

        public static object GetNumValue(this Enum @enum, string enumName) => Convert.ChangeType(@enum.GetType().GetField(enumName).GetValue(@enum), Enum.GetUnderlyingType(@enum.GetType()));

        // public static object GetNumValue(this Enum @enum) => GetNumValue(@enum, @enum.ToString());

        // todo : to test if Math.Log(Convert.ToInt64(flagsEnum), 2) == 'SomeInt64'; (no float, double ...) would be faster.

        public static bool HasMultipleFlags(this Enum flagsEnum)

        {

            Type type = flagsEnum.GetType();

            if (type.GetCustomAttributes(typeof(FlagsAttribute)).Count() == 0)

                return false; // throw new ArgumentException(string.Format("This enum does not implement the {0} attribute.", typeof(FlagsAttribute).Name));



            bool alreadyFoundAFlag = false;

            Enum enumValue = null;

            // FieldInfo field = null;



            foreach (string s in type.GetEnumNames())

            {

                enumValue = ((Enum)Enum.Parse(type, s));



                if (enumValue.GetNumValue(s).Equals(0)) continue;



                if (flagsEnum.HasFlag(enumValue))

                    if (!alreadyFoundAFlag) alreadyFoundAFlag = true;

                    else return true;

            }

            return false;

        }

        /// <summary>
        /// Determines whether the current enum value is within the enum values range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
        public static bool IsValidEnumValue(this Enum @enum)

        {

            ArrayList values = new ArrayList(@enum.GetType().GetEnumValues());

            values.Sort();

            // object _value = Convert.ChangeType(value, value.GetType().GetEnumUnderlyingType());

            return @enum.CompareTo(values[0]) >= 0 && @enum.CompareTo(values[values.Count - 1]) <= 0;

        }

        public static void ThrowIfNotValidEnumValue(this Enum @enum)

        {

            if (!@enum.IsValidEnumValue()) throw new ArgumentException(string.Format(InvalidEnumValue, @enum.ToString()));

        }

        //public static ImageSource ToImageSource(this Icon icon)

        //{

        //    IntPtr hIcon = icon.Handle;

        //    BitmapSource wpfIcon = Imaging.CreateBitmapSourceFromHIcon(
        //        hIcon,
        //        Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    //if (!Util.DeleteObject(hIcon))

        //    //    throw new Win32Exception();

        //    //using (MemoryStream memoryStream = new MemoryStream())

        //    //{

        //    //    icon.ToBitmap().Save(memoryStream, ImageFormat.Png);

        //    //    IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

        //    //    return (ImageSource) new ImageSourceConverter().ConvertFrom( iconBitmapDecoder);

        //    //}

        //    ImageSource imageSource;

        //    // Icon icon = Icon.ExtractAssociatedIcon(path);

        //    using (Bitmap bmp = icon.ToBitmap())
        //    {
        //        var stream = new MemoryStream();
        //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        imageSource = BitmapFrame.Create(stream);
        //    }

        //    return imageSource;

        //    return icon.ToBitmap().ToImageSource();

        //    return wpfIcon;

        //}

        public static ImageSource ToImageSource(this Bitmap bitmap)

        {

            bitmap.MakeTransparent();

            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!Util.DeleteObject(hBitmap))

                throw new Win32Exception();

            //using (MemoryStream stream = new MemoryStream())
            //{
            //    bitmap.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

            //    stream.Position = 0;
            //    BitmapImage result = new BitmapImage();
            //    result.BeginInit();
            //    // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
            //    // Force the bitmap to load right now so we can dispose the stream.
            //    result.CacheOption = BitmapCacheOption.OnLoad;
            //    result.StreamSource = stream;
            //    result.EndInit();
            //    result.Freeze();
            //    return result;
            //}

            return wpfBitmap;

        }

        public static ArrayList ToList(this IEnumerable array)

        {

            ArrayList arrayList = new ArrayList();

            foreach (object value in array)

                arrayList.Add(value);

            return arrayList;

        }

        public static List<T> ToList<T>(this IEnumerable<T> array)

        {

            List<T> arrayList = new List<T>();

            foreach (T value in array)

                arrayList.Add(value);

            return arrayList;

        }

        public static void AddRange(this IList collection, IEnumerable array)

        {

            foreach (object item in array)

                collection.Add(item);

        }

        public static void AddRange(this IList collection, IEnumerable array, int start, int length)

        {

            ArrayList arrayList = array.ToList();

            for (int i = start; i < length; i++)

                collection.Add(arrayList[i]);

        }

        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> array)

        {

            foreach (T item in array)

                collection.Add(item);

        }

        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> array, int start, int length)

        {

            List<T> arrayList = array.ToList();

            for (int i = start; i < length; i++)

                collection.Add(arrayList[i]);

        }

        /// <summary>
        /// Removes multiple items in an <see cref="IList"/> collection, from a start index to a given length.
        /// </summary>
        /// <param name="collection">The collection from which remove the items.</param>
        /// <param name="start">The start index in the collection from which delete the items.</param>
        /// <param name="length">The length to remove.</param>
        public static void RemoveRange(this IList collection, int start, int length)

        {

            for (int i = 0; i < length; i++)

                collection.RemoveAt(start + i);

        }

        public static void Execute(this ICommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command is RoutedCommand)

                ((RoutedCommand)command).Execute(commandParameter, commandTarget);

            else

                command.Execute(commandParameter);

        }

        public static bool TryExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand
                ? ((RoutedCommand)command).TryExecute(commandParameter, commandTarget)
                : command.TryExecute(commandParameter);

        /// <summary>
        /// Check if the command can be executed, and executes it if available. See the remarks section.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="commandParameter">The parameter of your command.</param>
        /// <remarks>
        /// This method only evaluates the commands of the common <see cref="ICommand"/> type. To evaluate a command of the <see cref="RoutedCommand"/> type, consider using the <see cref="TryExecute(RoutedCommand, object, IInputElement)"/> method. If you are not sure of the type of your command, so consider using the <see cref="ExecuteCommand(ICommandSource, ICommand, object, IInputElement)"/> method.
        /// </remarks>
        public static bool TryExecute(this ICommand command, object commandParameter)

        {

            if (command != null && command.CanExecute(commandParameter))

            {

                command.Execute(commandParameter);

                return true;

            }

            return false;

        }

        public static bool TryExecute(this RoutedCommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command.CanExecute(commandParameter, commandTarget))

            {
                try
                {
                    command.Execute(commandParameter, commandTarget);
                }
                catch (InvalidOperationException ex)
                {

                    Debug.WriteLine(ex.Message);
                }

                return true;

            }

            return false;

        }

        public static bool CanExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand
                ? ((RoutedCommand)command).CanExecute(commandParameter, commandTarget)
                : command.CanExecute(commandParameter);

        ///// <summary>
        ///// Checks if an object is a <see cref="FrameworkElement.Parent"/> or a <see cref="FrameworkElement.TemplatedParent"/> of an another object.
        ///// </summary>
        ///// <param name="source">The source object</param>
        ///// <param name="obj">The object to search in</param>
        ///// <returns><see langword="true"/> if 'obj' is a parent of the current object, otherwise <see langword="false"/>.</returns>
        //public static bool IsParent(this DependencyObject source, FrameworkElement obj)

        //{

        //    DependencyObject parent = obj.Parent ?? obj.TemplatedParent;

        //    while (parent != null && parent is FrameworkElement)

        //    {

        //        if (parent == source)

        //            return true;

        //        parent = ((FrameworkElement)parent).Parent ?? ((FrameworkElement)parent).TemplatedParent;

        //    }

        //    return false;

        //}

        /// <summary>
        /// Searches for the first parent of an object which is assignable from a given type.
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="type">The type to search</param>
        /// <param name="typeEquality">Indicates whether to check for the exact type equality. <see langword="true"/> to only search for objects with same type than the given type, <see langword="false"/> to search for all objects of type for which the given type is assignable from.</param>
        /// <returns>The first object that was found, if any, otherwise null.</returns>
        public static DependencyObject GetParent(this DependencyObject source, Type type, bool typeEquality)

        {

            if (!typeof(DependencyObject).IsAssignableFrom(type))

                throw new ArgumentException($"The DependencyObject type must be assignable from '{nameof(type)}'.");

            do

                source = (source is FrameworkElement frameworkElement ? frameworkElement.Parent ?? frameworkElement.TemplatedParent : null) ?? VisualTreeHelper.GetParent(source);

            while (source != null && source is FrameworkElement && (typeEquality ? source.GetType() != type : !type.IsAssignableFrom(source.GetType())));

            return source;

        }

        public static bool Contains(this string s, IEqualityComparer<char> comparer, string value)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i], value[j]))

                        break;

                    else if (j == value.Length - 1)

                        return true;

            return false;

        }

        public static bool Contains(this string s, char value, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                if (s[i] == value)

                {

                    index = i;

                    return true;

                }

            index = default;

            return false;

        }

        public static bool Contains(this string s, string value, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (s[i] != value[j])

                        break;

                    else if (j == value.Length - 1)

                    {

                        index = i;

                        return true;

                    }

            index = default;

            return false;

        }

        public static bool Contains(this string s, char value, IEqualityComparer<char> comparer, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                if (comparer.Equals(s[i], value))

                {

                    index = i;

                    return true;

                }

            index = default;

            return false;

        }

        public static bool Contains(this string s, string value, IEqualityComparer<char> comparer, out int index)

        {

            for (int i = 0; i < s.Length; i++)

                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i], value[j]))

                        break;

                    else if (j == value.Length - 1)

                    {

                        index = i;

                        return true;

                    }

            index = default;

            return false;

        }

    }
}
