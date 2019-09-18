using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Collections
{

    /// <summary>
    /// Delegate for a non-generic comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns>An <see cref="int"/> which is lesser than 0 if x is lesser than y, 0 if x is equal to y and greater than 0 if x is greater than y.</returns>
    public delegate int Comparison(object x, object y);

    /// <summary>
    /// Delegate for a non-generic equality comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
    public delegate bool EqualityComparison(object x, object y);

    /// <summary>
    /// Delegate for a generic equality comparison.
    /// </summary>
    /// <param name="x">First parameter to compare</param>
    /// <param name="y">Second parameter to compare</param>
    /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
    public delegate bool EqualityComparison<in T>(T x, T y);

    public enum SortingType

    {

        Ascending,

        Descending

    }

    public interface IComparer : System.Collections.IComparer
    {

        SortingType SortingType { get; set; }

    }

    public sealed class Comparer : IComparer

    {

        public static readonly Comparer Default = new Comparer(System.Threading.Thread.CurrentThread.CurrentCulture);

        public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);

        private readonly System.Collections.Comparer _comparer;

        public SortingType SortingType { get; set; }

        public Comparer(CultureInfo cultureInfo) => _comparer = new System.Collections.Comparer(cultureInfo);

        public int Compare(object x, object y)

        {

            int result = _comparer.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;

        }

    }

    public sealed class CaseInsensitiveComparer : IComparer

    {

        public static CaseInsensitiveComparer Default => new CaseInsensitiveComparer(System.Threading.Thread.CurrentThread.CurrentCulture);

        public static CaseInsensitiveComparer DefaultInvariant => new CaseInsensitiveComparer(CultureInfo.InvariantCulture);

        private readonly System.Collections.CaseInsensitiveComparer _comparer;

        public SortingType SortingType { get; set; }

        public CaseInsensitiveComparer(CultureInfo cultureInfo) => _comparer = new System.Collections.CaseInsensitiveComparer(cultureInfo);

        public int Compare(object x, object y)

        {

            int result = _comparer.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;

        }

    }

    public interface IComparer< in T> : System.Collections.Generic.IComparer<T>

    {

        SortingType SortingType { get; set; }

    }

    public abstract class Comparer<T> : System.Collections.Generic.Comparer<T>, IComparer<T>

    {

        public SortingType SortingType { get; set; }

        protected abstract int CompareOverride(T x, T y);

        public sealed override int Compare(T x, T y)
        {

            int result = CompareOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;

        }
    }

}
