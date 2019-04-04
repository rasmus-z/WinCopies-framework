using System;

namespace WinCopies.Util
{
    internal class PropertyChangingEventArgsInternal : PropertyChangingEventArgs
    {

        internal String FieldName { get; set; }

        internal PropertyChangingEventArgsInternal(String propertyName, Object previousValue, Object newValue, String fieldName) : base(propertyName, previousValue, newValue)

        {

            FieldName = fieldName;

        }
    }
}
