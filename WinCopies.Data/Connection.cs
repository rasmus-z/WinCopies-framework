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
            foreach (KeyValuePair<string, object> kvp in cmd.Parameters)
            {
                IDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = kvp.Key;
                parameter.Value = kvp.Value;
                command.Parameters.Add(parameter);
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
