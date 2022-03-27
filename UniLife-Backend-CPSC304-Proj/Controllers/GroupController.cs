using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {


        private readonly IDbConnection dbConnection;
        private readonly PostService postService;


        public GroupController(IDbConnection connection, PostService postService)
        {
            dbConnection = connection;
            this.postService = postService;
        }

        [HttpGet]
        public ActionResult<List<GroupModel>> GetAllGroup()
        {
            string query = @"SELECT GID, Group_Name  from [dbo].[Group]";

            try
            {
                List<GroupModel> group = QueryHandler.SqlQueryFromConnection<PostModel>(query,
                x => {
                    GroupModel g = new GroupModel();
                    g.Gid = (int)x[0];
                    g.Group_Name = (string)x[1];
                    return g;
                },
                dbConnection);
                return group;
            }
            catch (SqlException ex)
            {
                return this.BadRequest(ex.Message);
                // return this.Problem(detail:ex.Message,title:"SQL Query Error");
            }

        }

    }
}



