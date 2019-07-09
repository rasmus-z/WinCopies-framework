/*
 * Authors: Khun Ly, Pierre Sprimont
 */

using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Reflection;
using System.Xml;

namespace WinCopies.Data
{
    public static class ReaderToEntityMapper
    {

        public static T ReaderTo<T>(Func<ReaderToEntityMapperAttribute, string, object> getValueDelegate)
           where T : class, new()
        {
            var result = new T();

            ReaderTo(result, getValueDelegate);

            return result;
        }

        public static void ReaderTo<T>(T obj, Func<ReaderToEntityMapperAttribute, string, object> getValueDelegate)
        {

            ReaderToEntityMapperAttribute readerToEntityMapperAttribute;

            PropertyInfo[] properties = typeof(T).GetProperties();

            object value;

            foreach (PropertyInfo prop in properties)
            {

                readerToEntityMapperAttribute = prop.GetCustomAttribute<ReaderToEntityMapperAttribute>();

                value = getValueDelegate(readerToEntityMapperAttribute, prop.Name);

                if (value == null ? prop.PropertyType.IsClass || prop.PropertyType.IsInterface : prop.PropertyType.IsAssignableFrom(value.GetType()))

                    prop.SetValue(obj, readerToEntityMapperAttribute.Converter == null ? value : readerToEntityMapperAttribute.Converter.Convert(value, prop.PropertyType, readerToEntityMapperAttribute.ConverterParameter, readerToEntityMapperAttribute.ConverterCultureInfo));

            }
        }

        public static T ReaderTo<T>(IDataReader reader)
           where T : class, new()
        {

            var result = new T();

            ReaderTo(result, reader);

            return result;

        }

        public static void ReaderTo<T>(T obj, IDataReader reader) => ReaderTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) =>
        {

            object value = reader[readerToEntityMapperAttribute?.TableFieldName ?? propName];

            return value == DBNull.Value ? null : value;

        });

        public static T ReaderTo<T>(XmlNode xmlNode)
           where T : class, new()
        {

            var result = new T();

            ReaderTo(result, xmlNode);

            return result;

        }

        public static void ReaderTo<T>(T obj, XmlNode xmlNode) => ReaderTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) =>
        {

            object value = xmlNode[readerToEntityMapperAttribute?.TableFieldName ?? propName];

            return value == DBNull.Value ? null : value;

        });

        public static T ReaderTo<T>(JObject jObject)
           where T : class, new()
        {

            var result = new T();

            ReaderTo(result, jObject);

            return result;

        }

        public static void ReaderTo<T>(T obj, JObject jObject) => ReaderTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) => jObject[propName]);
    }
}
