using System;
using System.Collections.Generic;

namespace WinCopies.Util
{
    public class EnumComparer : IComparer<Enum>

    {
        public int Compare(Enum x, object y)
        {

            if (Util.IsNumber(y))

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

        public int Compare(object x, Enum y)
        {

            if (Util.IsNumber(x))

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

        public int Compare(Enum x, Enum y) => x.CompareTo(y);
    }

}
