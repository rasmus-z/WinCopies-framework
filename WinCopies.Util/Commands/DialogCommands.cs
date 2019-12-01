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

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WinCopies.Util.Commands
{
    public static class DialogCommands
    {

        public static RoutedUICommand Ok { get; } = new RoutedUICommand(Resources.CommandTexts.OkWPF, nameof(Ok), typeof(DialogCommands));

        public static RoutedUICommand Cancel { get; } = new RoutedUICommand(Resources.CommandTexts.CancelWPF, nameof(Cancel), typeof(DialogCommands));

        public static RoutedUICommand Yes { get; } = new RoutedUICommand(Resources.CommandTexts.YesToAllWPF, nameof(Yes), typeof(DialogCommands));

        public static RoutedUICommand YesToAll { get; } = new RoutedUICommand(Resources.CommandTexts.YesToAllWPF, nameof(YesToAll), typeof(DialogCommands));

        public static RoutedUICommand No { get; } = new RoutedUICommand(Resources.CommandTexts.NoToAllWPF, nameof(No), typeof(DialogCommands));

        public static RoutedUICommand NoToAll { get; } = new RoutedUICommand(Resources.CommandTexts.NoToAllWPF, nameof(NoToAll), typeof(DialogCommands));

        public static RoutedUICommand Apply { get; } = new RoutedUICommand(Resources.CommandTexts.ApplyWPF, nameof(Apply), typeof(DialogCommands));

        public static RoutedUICommand Retry { get; } = new RoutedUICommand(Resources.CommandTexts.RetryWPF, nameof(Retry), typeof(DialogCommands));

        public static RoutedUICommand Ignore { get; } = new RoutedUICommand(Resources.CommandTexts.IgnoreWPF, nameof(Ignore), typeof(DialogCommands));

        public static RoutedUICommand Abort { get; } = new RoutedUICommand(Resources.CommandTexts.AbortWPF, nameof(Abort), typeof(DialogCommands));

        public static RoutedUICommand Continue { get; } = new RoutedUICommand(Resources.CommandTexts.ContinueWPF, nameof(Continue), typeof(DialogCommands));

    }
}

#endif