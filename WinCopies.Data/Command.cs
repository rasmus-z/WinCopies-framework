using System;
using System.Collections.Generic;

namespace WinCopies.Data
{
    public class Command
    {

        public string Query { get; }

        public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public Command(string query)
        {

            if (query == null)

                throw new ArgumentNullException(nameof(query));

            if (string.IsNullOrWhiteSpace(query))

                throw new ArgumentException(string.Format(WinCopies.Util.Generic.StringParameterEmptyOrWhiteSpaces, nameof(query)));

            Query = query;

        }
    }
}
