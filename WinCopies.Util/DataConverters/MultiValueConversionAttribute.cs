using System;

namespace WinCopies.Util.Data
{
    public class MultiValueConversionAttribute : Attribute
    {

        public Type[] SourceTypes { get; }

        public Type TargetType { get; }

        public Type ParameterType { get; set; }

        public MultiValueConversionAttribute(Type[] sourceTypes, Type targetType)

        {

            SourceTypes = sourceTypes;

            TargetType = targetType;

        }

        public override object TypeId => this;

        public override int GetHashCode()
        {

            int sourceTypesHashCode = 0;

            foreach (Type t in SourceTypes)

                sourceTypesHashCode += t.GetHashCode();

            return sourceTypesHashCode + TargetType.GetHashCode();

        }
    }
}