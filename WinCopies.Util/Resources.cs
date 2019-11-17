/* Copyright © Pierre Sprimont, 2019
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

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
