using System;
using System.Globalization;
using System.Windows;
using WinCopies.Util.Data;

namespace WinCopies.GUI.Windows.Dialogs
{

    public struct KeyValuePair
    {

        public Enum Key { get; set; }

        public string Value { get; set; }

        public KeyValuePair(Enum key, string value) { Key = key; Value = value; }

        public override string ToString() => Value;

    }

    public class EnumToStringConverter : WinCopies.Util.Data.ConverterBase
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter is string _parameter && _parameter == "SelectedItem")

                return new KeyValuePair((Enum)value, (string)Themes.Generic.GetResource<ResourceDictionary>(string.Format("{0}s", value.GetType().Name))[value.ToString()]);

            string[] enumNames;

            if (parameter is Type t && t.IsEnum)

            {

                enumNames = t.GetEnumNames();

                KeyValuePair[] values = new KeyValuePair[enumNames.Length];

                for (int i = 0; i < enumNames.Length; i++)

                {

                    values[i] = new KeyValuePair((Enum)Enum.Parse(t, enumNames[i]), (string)Themes.Generic.GetResource<ResourceDictionary>(string.Format("{0}s", ((Type)parameter).Name))[enumNames[i].ToString()]);

                }

                return values;

            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //if (value == null)

            //    return 0;

            //else

            //    // todo : to add a 'new' Enum.TryParse() method in order to avoid types compatibility problems.

            //    try

            //    {

            //        return Enum.Parse(targetType, (string)value);

            //    }

            //    catch (Exception)

            //    {

            //        return 0;

            //    }

            if (parameter is string _parameter && _parameter == "SelectedItem")

                return ((KeyValuePair)value).Key; // return new KeyValuePair((Enum)Enum.Parse(t, enumNames[i]), (string)Themes.Generic.GetResource<ResourceDictionary>(string.Format("{0}s", ((Type)parameter).Name))[enumNames[i].ToString()]);

            else

                throw new NotImplementedException();

        }
    }
}
