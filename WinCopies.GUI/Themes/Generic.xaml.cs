using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinCopies.GUI.Themes
{
    public class Generic
    {

        public static T GetResource<T>(object key) => (T)ResourceDictionary[key];

        #region Resources

        /// <summary>
        /// Gets the Ok resource.
        /// </summary>
        public static string Ok => GetResource<string>(nameof(Ok));

        /// <summary>
        /// Gets the Apply resource.
        /// </summary>
        public static string Apply => GetResource<string>(nameof(Apply));

        /// <summary>
        /// Gets the Yes resource.
        /// </summary>
        public static string Yes => GetResource<string>(nameof(Yes));

        /// <summary>
        /// Gets the No resource.
        /// </summary>
        public static string No => GetResource<string>(nameof(No));

        /// <summary>
        /// Gets the Cancel resource.
        /// </summary>
        public static string Cancel => GetResource<string>(nameof(Cancel));

        #endregion

        public static ResourceDictionary ResourceDictionary { get; } = Util.Generic.AddNewDictionary("/WinCopies.GUI;component/Themes/Generic.xaml");

    }
}
