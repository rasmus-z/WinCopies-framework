using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Reflection;
using System.Xml;

namespace WinCopies.Data
{
    public static class ReaderToEntityMapper
    {
        public static T ReaderTo<T>(IDataReader reader)
           where T : class, new()
        {

            T result = new T(); // Activator.CreateInstance<T>();

            SqlReaderToEntityMapperAttribute readerToEntityMapperAttribute = null;

            PropertyInfo[] properties = typeof(T).GetProperties();

            object value = null;

            foreach (PropertyInfo prop in properties)
            {

                readerToEntityMapperAttribute = prop.GetCustomAttribute<SqlReaderToEntityMapperAttribute>();

                value = reader[readerToEntityMapperAttribute == null ? prop.Name : readerToEntityMapperAttribute.TableColumnName];

                if (value == DBNull.Value)

                    value = null;

                if (prop.PropertyType.IsAssignableFrom(value?.GetType()))

                    prop.SetValue(result, readerToEntityMapperAttribute.Converter == null ? value : readerToEntityMapperAttribute.Converter.Convert(value, prop.PropertyType, readerToEntityMapperAttribute.ConverterParameter, readerToEntityMapperAttribute.ConverterCultureInfo));

            }

            return result;
        }

        //public static T ReaderTo<T>(XmlDocument xmlDoc)
        //   where T : class, new()
        //{

        //    T result = new T(); // Activator.CreateInstance<T>();

        //    ReaderToEntityMapperAttribute readerToEntityMapperAttribute = null;

        //    PropertyInfo[] properties = typeof(T).GetProperties();

        //    object value = null;

        //    foreach (PropertyInfo prop in properties)
        //    {
        //        readerToEntityMapperAttribute = prop.GetCustomAttribute<ReaderToEntityMapperAttribute>();

        //        value = reader[readerToEntityMapperAttribute == null ? prop.Name : readerToEntityMapperAttribute.TableColumnName];

        //        if (value == DBNull.Value)

        //            value = null;

        //        if (prop.PropertyType.IsAssignableFrom(value?.GetType()))

        //            prop.SetValue(result, readerToEntityMapperAttribute.Converter == null ? value : readerToEntityMapperAttribute.Converter.Convert(value, prop.PropertyType, readerToEntityMapperAttribute.ConverterParameter, readerToEntityMapperAttribute.ConverterCultureInfo));

        //    }

        //    return result;
        //}

        public static T ReaderTo<T>(JObject jObject)
           where T : class, new()
        {

            T result = new T(); // Activator.CreateInstance<T>();

            ReaderToEntityMapperAttribute readerToEntityMapperAttribute = null;

            PropertyInfo[] properties = typeof(T).GetProperties();

            object value = null;

            foreach (PropertyInfo prop in properties)
            {

                readerToEntityMapperAttribute = prop.GetCustomAttribute<ReaderToEntityMapperAttribute>();

                value = jObject[prop.Name];

                if (prop.PropertyType.IsAssignableFrom(value?.GetType()))

                    prop.SetValue(result, readerToEntityMapperAttribute.Converter == null ? value : readerToEntityMapperAttribute.Converter.Convert(value, prop.PropertyType, readerToEntityMapperAttribute.ConverterParameter, readerToEntityMapperAttribute.ConverterCultureInfo));

            }

            return result;
        }
    }
}
