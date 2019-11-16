using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public static class Resources
    {

        public static T GetResource<T>(string name) => (T)typeof(WinCopies.Util.Properties.Resources).GetProperty(name, WinCopies.Util.Util.DefaultBindingFlagsForPropertySet).GetValue(null);

        public static class CommandTexts
        {

            /// <summary>
            /// Gets the NewTab resource.
            /// </summary>
            public static string NewTab => Properties.Resources.NewTabWPF;

            /// <summary>
            /// Gets the NewWindow resource.
            /// </summary>
            public static string NewWindow => Properties.Resources.NewWindowWPF;

            /// <summary>
            /// Gets the NewWindowInNewInstance resource.
            /// </summary>
            public static string NewWindowInNewInstance => Properties.Resources.NewWindowInNewInstanceWPF;

            /// <summary>
            /// Gets the CloseTab resource.
            /// </summary>
            public static string CloseTab => Properties.Resources.CloseTabWPF;

            /// <summary>
            /// Gets the CloseAllTabs resource.
            /// </summary>
            public static string CloseAllTabs => Properties.Resources.CloseAllTabsWPF;

            /// <summary>
            /// Gets the CloseWindow resource.
            /// </summary>
            public static string CloseWindow => Properties.Resources.CloseWindowWPF;

            /// <summary>
            /// Gets the NewFolder resource.
            /// </summary>
            public static string NewFolder => Properties.Resources.NewFolderWPF;

            /// <summary>
            /// Gets the NewArchive resource.
            /// </summary>
            public static string NewArchive => Properties.Resources.NewArchiveWPF;

            /// <summary>
            /// Gets the ShowFileProperties resource.
            /// </summary>
            public static string ShowFileProperties => Properties.Resources.ShowFilePropertiesWPF;

            /// <summary>
            /// Gets the Rename resource.
            /// </summary>
            public static string Rename => Properties.Resources.RenameWPF;

            /// <summary>
            /// Gets the DeletePermanently resource.
            /// </summary>
            public static string DeletePermanently => Properties.Resources.DeletePermanentlyWPF;

            /// <summary>
            /// Gets the <see cref="DeselectAll"/> resource.
            /// </summary>
            public static string DeselectAll => Properties.Resources.DeselectAllWPF;

            /// <summary>
            /// Gets the <see cref="ReverseSelection"/> resource.
            /// </summary>
            public static string ReverseSelection => Properties.Resources.ReverseSelectionWPF;

        }

        public static class ExceptionMessages
        {

            /// <summary>
            /// Gets the DeclaringTypesNotCorrespond resource.
            /// </summary>
            public static string DeclaringTypesNotCorrespond => Properties.Resources.DeclaringTypesNotCorrespond;

            /// <summary>
            /// Gets the FieldOrPropertyNotFound resource.
            /// </summary>
            public static string FieldOrPropertyNotFound => Properties.Resources.FieldOrPropertyNotFound;

            /// <summary>
            /// Gets the ArrayWithMoreThanOneDimension resource.
            /// </summary>
            public static string ArrayWithMoreThanOneDimension => Properties.Resources.ArrayWithMoreThanOneDimension;

            /// <summary>
            /// Gets the OneOrMoreSameKey resource.
            /// </summary>
            public static string OneOrMoreSameKey => Properties.Resources.OneOrMoreSameKey;

            /// <summary>
            /// Gets the NoValidEnumValue resource.
            /// </summary>
            public static string NoValidEnumValue => Properties.Resources.NoValidEnumValue;

            /// <summary>
            /// Gets the StringParameterEmptyOrWhiteSpaces resource.
            /// </summary>
            public static string StringParameterEmptyOrWhiteSpaces => Properties.Resources.StringParameterEmptyOrWhiteSpaces;

            /// <summary>
            /// Gets the BackgroundWorkerIsBusy resource.
            /// </summary>
            public static string BackgroundWorkerIsBusy => Properties.Resources.BackgroundWorkerIsBusy;

            /// <summary>
            /// Gets the InvalidEnumValue resource.
            /// </summary>
            public static string InvalidEnumValue => Properties.Resources.InvalidEnumValue;

            /// <summary>
            /// Gets the ReadOnlyCollection resource.
            /// </summary>
            public static string ReadOnlyCollection => Properties.Resources.ReadOnlyCollection;

        }

    }
}
