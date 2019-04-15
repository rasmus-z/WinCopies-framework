/*
 * Authors: Khun Ly, Pierre Sprimont
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WinCopies.Data
{
    public class ReaderToEntityMapperAttribute : Attribute
    {

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public CultureInfo ConverterCultureInfo { get; set; }

    }

    public class SqlReaderToEntityMapperAttribute : ReaderToEntityMapperAttribute

    {

        public string TableColumnName { get; set; }

    }
}
