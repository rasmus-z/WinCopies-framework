/*
 * Authors: Khun Ly, Pierre Sprimont
 */

using System;
using System.Collections.Generic;

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

        private void Init(string query) => Query = Util.Util.IsNullEmptyOrWhiteSpace(query) ? throw new ArgumentException(string.Format(Util.Generic.StringParameterEmptyOrWhiteSpaces, nameof(query))) : query;

    }
}
