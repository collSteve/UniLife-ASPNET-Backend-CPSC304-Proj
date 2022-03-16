using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    // www.here.com/api/posts
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IDbConnection dbConnection;

        public PostsController(IDbConnection connection)
        {
            dbConnection = connection;
        }

        [HttpGet] 
        public IEnumerable<string> Get()
        {
            return new[] { "My Post", "test test", "Hi fuck you" };
        }

        // www.here.com/api/posts/hi
        [Route("hi")]
        [HttpGet]
        public IEnumerable<int> GetHi()
        {
            return new[] { 1, 2, 3, 4, 5 };
        }

        // www.here.com/api/posts/hi
        [Route("god")]
        [HttpGet]
        public IEnumerable<string> GetDog()
        {
            return new[] { "God" };
        }

        [Route("accounts")]
        [HttpGet]
        public IEnumerable<string> GetAccounts()
        {
            List<string> result = new List<string>();
            //if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

            string query = @"SELECT Username from [dbo].[Account]";

            /*using (SqlCommand command = new SqlCommand(query, (SqlConnection)dbConnection))
            {
                command.ExecuteReader();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }*/

            result = QueryHandler.SqlQueryFromConnection<string>(query, x => (string)x[0], dbConnection);

            return result;
        }

    }
}
