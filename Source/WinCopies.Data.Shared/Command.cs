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
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>.

 * Authors: Khun Ly, Pierre Sprimont */

using System;
using System.Collections.Generic;
using static WinCopies.Util.Util;

namespace WinCopies.Data
{
    public class Command
    {
        public string Query { get; private set; }

        public IDictionary<string, object> Parameters { get; set; }

        public Command(string query) : this(query, new Dictionary<string, object>()) { }

        public Command(string query, IDictionary<string, object> parameters)
        {
            Init(query);

            Parameters = parameters;
        }

        private void Init(string query) => Query = IsNullEmptyOrWhiteSpace(query) ? throw new ArgumentException(string.Format(Util.Resources.ExceptionMessages.StringParameterEmptyOrWhiteSpace, nameof(query))) : query;
    }
}
