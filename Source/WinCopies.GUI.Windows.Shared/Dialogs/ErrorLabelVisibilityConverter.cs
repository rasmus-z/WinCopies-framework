﻿/* Copyright © Pierre Sprimont, 2019
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
using System.Globalization;
using System.Windows.Data;

//namespace WinCopies.GUI.Windows.Dialogs
//{
//    [ValueConversion(typeof(string), typeof(System.Windows.Visibility))]
//    public class ErrorLabelVisibilityConverter : Util.Data.ConverterBase
//    {
//        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
//            // string _value = (string)value;

//            string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value)
//                ? System.Windows.Visibility.Collapsed
//                : (object)System.Windows.Visibility.Visible;

//        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
//    }
//}
