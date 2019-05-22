using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public partial class Generic
    {

        public static Dictionary<string, System.Windows.ResourceDictionary> ResourceDictionaries { get; } = new Dictionary<string, System.Windows.ResourceDictionary>();

        public static T GetResource<T>(object key) => (T)ResourceDictionary[key];

        #region Resources

        /// <summary>
        /// Gets the NewTab resource.
        /// </summary>
        public static string NewTab => GetResource<string>(nameof(NewTab));

        /// <summary>
        /// Gets the NewWindow resource.
        /// </summary>
        public static string NewWindow => GetResource<string>(nameof(NewWindow));

        /// <summary>
        /// Gets the NewWindowInNewInstance resource.
        /// </summary>
        public static string NewWindowInNewInstance => GetResource<string>(nameof(NewWindowInNewInstance));

        /// <summary>
        /// Gets the CloseTab resource.
        /// </summary>
        public static string CloseTab => GetResource<string>(nameof(CloseTab));

        /// <summary>
        /// Gets the CloseAllTabs resource.
        /// </summary>
        public static string CloseAllTabs => GetResource<string>(nameof(CloseAllTabs));

        /// <summary>
        /// Gets the CloseWindow resource.
        /// </summary>
        public static string CloseWindow => GetResource<string>(nameof(CloseWindow));

        /// <summary>
        /// Gets the NewFolder resource.
        /// </summary>
        public static string NewFolder => GetResource<string>(nameof(NewFolder));

        /// <summary>
        /// Gets the NewArchive resource.
        /// </summary>
        public static string NewArchive => GetResource<string>(nameof(NewArchive));

        /// <summary>
        /// Gets the ShowFileProperties resource.
        /// </summary>
        public static string ShowFileProperties => GetResource<string>(nameof(ShowFileProperties));

        /// <summary>
        /// Gets the Rename resource.
        /// </summary>
        public static string Rename => GetResource<string>(nameof(Rename));

        /// <summary>
        /// Gets the DeletePermanently resource.
        /// </summary>
        public static string DeletePermanently => GetResource<string>(nameof(DeletePermanently));

        /// <summary>
        /// Gets the DeclaringTypesNotCorrespond resource.
        /// </summary>
        public static string DeclaringTypesNotCorrespond => GetResource<string>(nameof(DeclaringTypesNotCorrespond));

        /// <summary>
        /// Gets the FieldOrPropertyNotFound resource.
        /// </summary>
        public static string FieldOrPropertyNotFound => GetResource<string>(nameof(FieldOrPropertyNotFound));

        /// <summary>
        /// Gets the ArrayWithMoreThanOneDimension resource.
        /// </summary>
        public static string ArrayWithMoreThanOneDimension => GetResource<string>(nameof(ArrayWithMoreThanOneDimension));

        /// <summary>
        /// Gets the OneOrMoreSameKey resource.
        /// </summary>
        public static string OneOrMoreSameKey => GetResource<string>(nameof(OneOrMoreSameKey));

        /// <summary>
        /// Gets the NoValidEnumValue resource.
        /// </summary>
        public static string NoValidEnumValue => GetResource<string>(nameof(NoValidEnumValue));

        /// <summary>
        /// Gets the StringParameterEmptyOrWhiteSpaces resource.
        /// </summary>
        public static string StringParameterEmptyOrWhiteSpaces => GetResource<string>(nameof(StringParameterEmptyOrWhiteSpaces));

        /// <summary>
        /// Gets the BackgroundWorkerIsBusy resource.
        /// </summary>
        public static string BackgroundWorkerIsBusy => GetResource<string>(nameof(BackgroundWorkerIsBusy));

        /// <summary>
        /// Gets the InvalidEnumValue resource.
        /// </summary>
        public static string InvalidEnumValue => GetResource<string>(nameof(InvalidEnumValue));

        #endregion

        public static System.Windows.ResourceDictionary ResourceDictionary { get; } = null;

        static Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

        // public Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

        public static System.Windows.ResourceDictionary AddNewDictionary(string dictionaryUri, string key = null)

        {

            if (key == null)

#if DEBUG

            {

#endif 

                key = Assembly.GetCallingAssembly().GetName().Name;

#if DEBUG 

                Debug.WriteLine(key);

            }

#endif
            System.Windows.ResourceDictionary resourceDictionary = new System.Windows.ResourceDictionary
            {
                Source = new Uri(dictionaryUri, UriKind.RelativeOrAbsolute)
            };

            ResourceDictionaries.Add(key, resourceDictionary);

            return resourceDictionary;

        }

        public static System.Windows.ResourceDictionary GetResourceDictionary(string key = null)

        {

            if (key == null)

                key = Assembly.GetCallingAssembly().GetName().Name;

            return ResourceDictionaries[key];

        }

        public static object GetResource(object resourceKey, string resourceDictionaryKey = null) => GetResourceDictionary(resourceDictionaryKey)[resourceKey];
    }
}
