#if DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
    {

        public CultureInfo CultureInfo {get;set;}

        public bool Ordinal{get;set;}

        public bool IgnoreCase {get;set;}

        public bool AccentSensitive {get;set;}

        public StringComparer(CultureInfo cultureInfo, bool ordinal, bool ignoreCase, bool accentSensitive)

        {

CultureInfo
            
        }

        public override int Compare(string x, string y)
        {
            throw new NotImplementedException();
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(string x, string y)
        {
            throw new NotImplementedException();
        }

        public new bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

#endif
