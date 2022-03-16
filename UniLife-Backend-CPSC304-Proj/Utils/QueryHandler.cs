using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace UniLife_Backend_CPSC304_Proj.Utils
{
    public class QueryHandler
    {

        public static List<T> RawSqlQuery<T>(string query, Func<System.Data.Common.DbDataReader, T> map, DbContext context)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }

        }

        public static List<T> SqlQueryFromConnection<T>(string query, Func<System.Data.Common.DbDataReader, T> map, IDbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                if (connection.State == ConnectionState.Closed) connection.Open();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map((System.Data.Common.DbDataReader)result));
                    }

                    return entities;
                }
            }

        }

        public static int SqlExecutionQueryFromConnection(string query, IDbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                try
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    return command.ExecuteNonQuery();
                } catch (SqlException ex)
                {
                    throw ex;
                }
                
            }

        }
    }
}
