using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace RefactorThis.Utilities
{
    public class SqliteUtil
    {
        private const string ConnectionString = "Data Source=App_Data/products.db";

        public static SqliteConnection NewConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        /// <summary>
        /// Execute NonQuery queries which just return affected rows, typically used in INSERT, UPDATE, and DELETE statements
        /// </summary>
        private static int NonQuery(string query, SqliteParameter[] parameters)
        {
            var result = 0;
            using (SqliteConnection connection = NewConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddRange(parameters);
                result = command.ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// Generic ExecuteReader, takes a Func<SqliteDataReader, T> mapToModelFunction which is used convert from the 
        /// SqliteDataReader to a specified T Model
        /// </summary>
        private static List<T> ExecuteReader<T>(Func<SqliteDataReader, T> mapToModelFn, string query, SqliteParameter[] parameters)
        {
            var results = new List<T>();
            
            using (SqliteConnection connection = NewConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddRange(parameters);
                var reader = command.ExecuteReader();

                while(reader.Read()) {
                    results.Add(mapToModelFn(reader));
                }
            }
            return results;
        }

        public static List<T> Query<T>(Func<SqliteDataReader, T> mapToModelFn, string query, SqliteParameter[] parameters = null)
        {
            if (parameters == null)
            {
                parameters = new SqliteParameter[] { };
            }

            return ExecuteReader(mapToModelFn, query, parameters);
        }

        public static int Insert(string query, SqliteParameter[] parameters)
        {
            return NonQuery(query, parameters);
        }

        public static int Update(string query, SqliteParameter[] parameters)
        {
            return NonQuery(query, parameters);
        }

        public static int Delete(string query, SqliteParameter[] parameters)
        {
            return NonQuery(query, parameters);
        }
    }
}