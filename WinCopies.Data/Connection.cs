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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Data
{
    public class Connection
    {

        private readonly DbProviderFactory _factory;

        private readonly string _connectionString;

        public Connection(string connectionString, string invariantName)
        {
            _factory = DbProviderFactories.GetFactory(invariantName);

            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            return connection;
        }

        private IDbCommand CreateCommand(IDbConnection conn, Command cmd)
        {
            IDbCommand command = conn.CreateCommand();

            command.CommandText = cmd.Query;

            IDataParameter parameter;

            foreach (KeyValuePair<string, object> kvp in cmd.Parameters)
            {
                parameter = command.CreateParameter();

                parameter.ParameterName = kvp.Key;
                parameter.Value = kvp.Value;

                _ = command.Parameters.Add(parameter);
            }

            return command;
        }

        public IEnumerable<T> ExecuteReader<T>(Command cmd, Func<IDataReader, T> selector)
        {
            using (IDbConnection conn = CreateConnection())
            {
                conn.Open();

                using (IDbCommand command = CreateCommand(conn, cmd))
                {
                    IDataReader reader = command.ExecuteReader();

                    while (reader.Read())

                        yield return selector(reader);
                }
            }
        }

        public int ExecuteNonQuery(Command cmd)
        {
            using (IDbConnection conn = CreateConnection())
            {
                conn.Open();

                using (IDbCommand command = CreateCommand(conn, cmd))

                    return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(Command cmd)
        {
            using (IDbConnection conn = CreateConnection())
            {
                conn.Open();

                using (IDbCommand command = CreateCommand(conn, cmd))

                    return command.ExecuteScalar();
            }
        }
    }
}
