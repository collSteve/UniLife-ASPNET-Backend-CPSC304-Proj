using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Models;
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
        public ActionResult<List<PostModel>> GetAllPosts() {
            string query = @"SELECT pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID]  from [dbo].[Post]";

            try
            {
                List<PostModel> posts = QueryHandler.SqlQueryFromConnection<PostModel>(query,
                x => {
                    PostModel p = new PostModel();
                    p.Pid = (int)x[0];
                    p.Title = (string)x[1];
                    p.CreatedDate = (DateTime)x[2];
                    p.PostBody = (string)x[3];
                    p.NumLikes = (int)x[4];
                    p.NumDislikes = (int)x[5];
                    p.CreatorAid = (int)x[6];
                    return p;
                },
                dbConnection);
                return posts;
            } catch (SqlException ex)
            {
                return this.BadRequest(ex.Message);
               // return this.Problem(detail:ex.Message,title:"SQL Query Error");
            }

        }


        
    }

    
}
