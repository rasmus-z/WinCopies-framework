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
 * 
 * Authors: Khun Ly, Pierre Sprimont */

using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Reflection;
using System.Xml;

namespace WinCopies.Data
{
    public static class ReaderToEntityMapper
    {

        public static T ReadTo<T>(Func<ReaderToEntityMapperAttribute, string, object> getValueDelegate)
           where T : class, new()
        {
            var result = new T();

            ReadTo(result, getValueDelegate);

            return result;
        }

        public static void ReadTo<T>(T obj, Func<ReaderToEntityMapperAttribute, string, object> getValueDelegate)
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

        public static T ReadTo<T>(IDataReader reader)
           where T : class, new()
        {

            var result = new T();

            ReadTo(result, reader);

            return result;

        }

        public static void ReadTo<T>(T obj, IDataReader reader) => ReadTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) =>
        {

            object value = reader[readerToEntityMapperAttribute?.TableFieldName ?? propName];

            return value == DBNull.Value ? null : value;

        });

        public static T ReadTo<T>(XmlNode xmlNode)
           where T : class, new()
        {

            var result = new T();

            ReadTo(result, xmlNode);

            return result;

        }

        public static void ReadTo<T>(T obj, XmlNode xmlNode) => ReadTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) =>
        {

            object value = xmlNode[readerToEntityMapperAttribute?.TableFieldName ?? propName];

            return value == DBNull.Value ? null : value;

        });

        public static T ReadTo<T>(JObject jObject)
           where T : class, new()
        {

            var result = new T();

            ReadTo(result, jObject);

            return result;

        }

        public static void ReadTo<T>(T obj, JObject jObject) => ReadTo(obj, (ReaderToEntityMapperAttribute readerToEntityMapperAttribute, string propName) => jObject[propName]);
    }
}
