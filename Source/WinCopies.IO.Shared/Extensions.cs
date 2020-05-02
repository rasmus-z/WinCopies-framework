using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WinCopies.Util;

namespace WinCopies.Linq
{
    // todo: to put in WinCopies.Linq (WinCopies.Util package)
   public static class Extensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Predicate<T> func)
        {
            foreach (T value in enumerable)

                if (func(value))

                    yield return value;
        }

        public static IEnumerable Where(this IEnumerable enumerable, Predicate func)
        {
            foreach (var value in enumerable)

                if (func(value))

                    yield return value;
        }
    }
}
