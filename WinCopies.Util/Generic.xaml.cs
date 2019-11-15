//<!--<ResourceDictionary x:Class="WinCopies.Util.Generic"
//             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
//             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
//             xmlns:local="clr-namespace:WinCopies.Util"
//                    xmlns:sys="clr-namespace:System;assembly=mscorlib">
//    --><!--<sys:String x:Key="DeclaringTypeIsNotInObjectInheritanceHierarchyException" >'{0}' is not in the inheritance hierarchy of '{1}'.</sys:String>--><!--
//</ResourceDictionary>-->

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

        [Obsolete("Please use the WinCopies.Util.Resources.GetResource<T>(string name) method instead.")]
        public static T GetResource<T>(object key) => Resources.GetResource<T>((string)key);

        #region Resources

        /// <summary>
        /// Gets the NewTab resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string NewTab => GetResource<string>(nameof(NewTab));

        /// <summary>
        /// Gets the NewWindow resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string NewWindow => GetResource<string>(nameof(NewWindow));

        /// <summary>
        /// Gets the NewWindowInNewInstance resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string NewWindowInNewInstance => GetResource<string>(nameof(NewWindowInNewInstance));

        /// <summary>
        /// Gets the CloseTab resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string CloseTab => GetResource<string>(nameof(CloseTab));

        /// <summary>
        /// Gets the CloseAllTabs resource.
        /// </summary>
        public static string CloseAllTabs => GetResource<string>(nameof(CloseAllTabs));

        /// <summary>
        /// Gets the CloseWindow resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string CloseWindow => GetResource<string>(nameof(CloseWindow));

        /// <summary>
        /// Gets the NewFolder resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string NewFolder => GetResource<string>(nameof(NewFolder));

        /// <summary>
        /// Gets the NewArchive resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string NewArchive => GetResource<string>(nameof(NewArchive));

        /// <summary>
        /// Gets the ShowFileProperties resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string ShowFileProperties => GetResource<string>(nameof(ShowFileProperties));

        /// <summary>
        /// Gets the Rename resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string Rename => GetResource<string>(nameof(Rename));

        /// <summary>
        /// Gets the DeletePermanently resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string DeletePermanently => GetResource<string>(nameof(DeletePermanently));

        /// <summary>
        /// Gets the <see cref="DeselectAll"/> resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string DeselectAll => GetResource<string>(nameof(DeselectAll));

        /// <summary>
        /// Gets the <see cref="ReverseSelection"/> resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.CommandTexts class instead.")]
        public static string ReverseSelection => GetResource<string>(nameof(ReverseSelection));

        /// <summary>
        /// Gets the DeclaringTypesNotCorrespond resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string DeclaringTypesNotCorrespond => GetResource<string>(nameof(DeclaringTypesNotCorrespond));

        /// <summary>
        /// Gets the FieldOrPropertyNotFound resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string FieldOrPropertyNotFound => GetResource<string>(nameof(FieldOrPropertyNotFound));

        /// <summary>
        /// Gets the ArrayWithMoreThanOneDimension resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string ArrayWithMoreThanOneDimension => GetResource<string>(nameof(ArrayWithMoreThanOneDimension));

        /// <summary>
        /// Gets the OneOrMoreSameKey resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string OneOrMoreSameKey => GetResource<string>(nameof(OneOrMoreSameKey));

        /// <summary>
        /// Gets the NoValidEnumValue resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string NoValidEnumValue => GetResource<string>(nameof(NoValidEnumValue));

        /// <summary>
        /// Gets the StringParameterEmptyOrWhiteSpaces resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string StringParameterEmptyOrWhiteSpaces => GetResource<string>(nameof(StringParameterEmptyOrWhiteSpaces));

        /// <summary>
        /// Gets the BackgroundWorkerIsBusy resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string BackgroundWorkerIsBusy => GetResource<string>(nameof(BackgroundWorkerIsBusy));

        /// <summary>
        /// Gets the InvalidEnumValue resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string InvalidEnumValue => GetResource<string>(nameof(InvalidEnumValue));

        /// <summary>
        /// Gets the ReadOnlyCollection resource.
        /// </summary>
        [Obsolete("This property is obsolete. Please use the same property of the WinCopies.Util.Resources.ExceptionMessages class instead.")]
        public static string ReadOnlyCollection => GetResource<string>(nameof(ReadOnlyCollection));

        #endregion

        public static System.Windows.ResourceDictionary ResourceDictionary { get; } = null;

        static Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

        // public Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

        public static System.Windows.ResourceDictionary AddNewDictionary(string dictionaryUri, string key = null)

        {

            if (key == null)

                key = Assembly.GetCallingAssembly().GetName().Name;

            var resourceDictionary = new System.Windows.ResourceDictionary
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
