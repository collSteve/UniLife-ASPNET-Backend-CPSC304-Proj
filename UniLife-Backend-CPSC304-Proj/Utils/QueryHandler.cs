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
    }
}
