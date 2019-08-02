using System;
using System.Collections.Generic;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.Collections
{
    public class EnumComparer : Comparer<Enum>

    {

        public virtual int Compare(Enum x, object y)
        {

            if (IsNumber(y))

            {

                object o = x.GetNumValue();

                if (o is sbyte sb) return sb.CompareTo(y);

                else if (o is byte b) return b.CompareTo(y);

                else if (o is short s) return s.CompareTo(y);

                else if (o is ushort us) return us.CompareTo(y);

                else if (o is int i) return i.CompareTo(y);

                else if (o is uint ui) return ui.CompareTo(y);

                else if (o is long l) return l.CompareTo(y);

                else if (o is ulong ul) return ul.CompareTo(y);

                else

                    // We shouldn't reach this point.

                    return 0;

            }

            else

                throw new ArgumentException("'y' is not from a numeric type.");

        }

        public virtual int Compare(object x, Enum y)
        {

            if (IsNumber(x))

            {

                object o = y.GetNumValue();

                if (o is sbyte sb) return -sb.CompareTo(x);

                else if (o is byte b) return -b.CompareTo(x);

                else if (o is short s) return -s.CompareTo(x);

                else if (o is ushort us) return -us.CompareTo(x);

                else if (o is int i) return -i.CompareTo(x);

                else if (o is uint ui) return -ui.CompareTo(x);

                else if (o is long l) return -l.CompareTo(x);

                else if (o is ulong ul) return -ul.CompareTo(x);

                else

                    // We shouldn't reach this point.

                    return 0;

            }

            else

                throw new ArgumentException("'x' is not from a numeric type.");

        }

        private protected virtual int CompareOverrideInternal(Enum x, Enum y) => x.CompareTo(y);

        protected sealed override int CompareOverride(Enum x, Enum y) => CompareOverride(x, y);

    }

    public class CustomizableEnumSortingTypeComparer : EnumComparer, IComparer<Enum>

    {

        public SortingType SortingType { get; set; }

        public override int Compare(Enum x, object y)
        {

            int result = base.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;

        }

        public override int Compare(object x, Enum y)
        {

            int result = base.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;

        }

        private protected sealed override int CompareOverrideInternal(Enum x, Enum y) 
        {

            int result = base.CompareOverride(x, y); 

            return SortingType == SortingType.Ascending ? result : -result; 

        }

    }

}
