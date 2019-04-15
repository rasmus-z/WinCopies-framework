//using System;
//using System.Text;
//using System.Windows.Markup;

//namespace WinCopies.Util
//{
//    public class StringParserForXaml : MarkupExtension
//    {

//        public StringBuilder StringBuilder { get; set; } = null;

//        public StringParserForXaml(string format, object o)

//        {

//            var stringBuilder = new StringBuilder();

//            stringBuilder.AppendFormat(format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(string format, params object[] o)

//        {

//            var stringBuilder = new StringBuilder();

//            stringBuilder.AppendFormat(format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(string format, IFormatProvider formatProvider, object o)

//        {

//            var stringBuilder = new StringBuilder();

//            stringBuilder.AppendFormat(formatProvider, format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(string format, IFormatProvider formatProvider, params object[] o)

//        {

//            var stringBuilder = new StringBuilder();

//            stringBuilder.AppendFormat(formatProvider, format, o);

//            StringBuilder = stringBuilder;
//        }

//        public StringParserForXaml(StringBuilder stringBuilder, string format, object o)

//        {

//            stringBuilder.AppendFormat(format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(StringBuilder stringBuilder, string format, params object[] o)

//        {

//            stringBuilder.AppendFormat(format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(StringBuilder stringBuilder, string format, IFormatProvider formatProvider, object o)

//        {

//            stringBuilder.AppendFormat(formatProvider, format, o);

//            StringBuilder = stringBuilder;

//        }

//        public StringParserForXaml(StringBuilder stringBuilder, string format, IFormatProvider formatProvider, params object[] o)

//        {

//            stringBuilder.AppendFormat(formatProvider, format, o);

//            StringBuilder = stringBuilder;
//        }

//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            return StringBuilder.ToString();
//        }
//    }
//}
