using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Services;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IDbConnection dbConnection;

        public AccountsController(IDbConnection connection)
        {

            dbConnection = connection;
        }


        // www.server.com/api/Account/group/(Gid)
        [HttpGet]
        public ActionResult<List<GroupModel>> JoinGroup(int Gid)
        {
            try
            {
                return groupService.GetAllGroups();

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }
        /*      
         *      These are just test Steve implemented when setting up the server. 
         *      For whoever is doing these do not get influenced.
         *      
                [Route("Accounts")]
                [HttpGet]
                public IEnumerable<string> GetAccounts()
                {
                    List<string> result = new List<string>();

                    string query = @"SELECT Username from [dbo].[Account]";

                    result = QueryHandler.SqlQueryFromConnection<string>(query, x => (string)x[0], dbConnection);

                    return result;
                }

                [Route("AddAccount")]
                [HttpGet]
                public IActionResult AddAccount(int aid)
                {
                    string query = "INSERT [dbo].Account([AID], Username, Email, [Password])"
                        + " VALUES(3, 'User3', 'User3@gmail.com', 'user3'); ";

                    try
                    {
                        QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
                        return CreatedAtAction(nameof(AddAccount), new { aid = aid });
                    }
                    catch (SqlException ex)
                    {
                        return this.BadRequest(ex.Message);
                    }
                }*/
    }
}
