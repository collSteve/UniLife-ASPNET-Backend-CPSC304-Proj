using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Services;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    // www.here.com/api/posts
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        internal class PostType
        {

            public const string SellingPost =  "SellingPost";
            public const string HousingPost = "HousingPost"; 
            public const string SocialMediaPost = "SocialMediaPost";

        }

        private readonly IDbConnection dbConnection;
        private readonly PostService postService;


        public PostsController(IDbConnection connection, PostService postService)
        {
            dbConnection = connection;
            this.postService = postService;
        }

        [HttpGet]
        public ActionResult<List<PostModel>> GetAllPosts() {
            try
            {
                return postService.GetAllPosts();

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }


        [HttpGet("Type/{postType}")]
        public ActionResult<List<PostModel>> GetPosts(string postType) {
            try
            {
                return postService.GetPostsByType(postType);

            } 
            catch(InvalidTypeException ex)
            {
                return this.BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
            /*string query = "";
            Func<DbDataReader, PostModel> mapFunction;

            if (postType.ToLower().Equals( PostType.SellingPost.ToLower()))
            {
                query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], "+
                                    "[Num_Dislikes], [Creator_UID], [Email], Phone_Num " +
                                    "from [dbo].[Post] P, [dbo].[Selling_Post] SP " +
                                    "where P.PID = SP.PID";
                mapFunction = (x) =>
                {
                    PostModel p = new PostModel();
                    p.Pid = (int)x[0];
                    p.Title = (string)x[1];
                    p.CreatedDate = (DateTime)x[2];
                    p.PostBody = (string)x[3];
                    p.NumLikes = (int)x[4];
                    p.NumDislikes = (int)x[5];
                    p.CreatorAid = (int)x[6];
                    p.Email = (string)x[7];
                    p.PhoneNum = Convert.ToString(x[8]);
                    return p;
                };
            }
            else if (postType.ToLower().Equals(PostType.HousingPost.ToLower()))
            {
                query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID], [Email], [Address] "+
                        "from [dbo].[Post] P, [dbo].[Housing_Post] SP "+
                        "where P.PID = SP.PID";
                mapFunction = (x) =>
                {
                    PostModel p = new PostModel();
                    p.Pid = (int)x[0];
                    p.Title = (string)x[1];
                    p.CreatedDate = (DateTime)x[2];
                    p.PostBody = (string)x[3];
                    p.NumLikes = (int)x[4];
                    p.NumDislikes = (int)x[5];
                    p.CreatorAid = (int)x[6];
                    p.Email = (string)x[7];
                    p.Address = (string)x[8];
                    return p;
                };
            }
            else if (postType.ToLower().Equals(PostType.SocialMediaPost.ToLower()))
            {
                query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID] "+
                        "from [dbo].[Post] P, [dbo].[Social_Media_Post] SP "+
                        "where P.PID = SP.PID";
                mapFunction = (x) =>
                {
                    PostModel p = new PostModel();
                    p.Pid = (int)x[0];
                    p.Title = (string)x[1];
                    p.CreatedDate = (DateTime)x[2];
                    p.PostBody = (string)x[3];
                    p.NumLikes = (int)x[4];
                    p.NumDislikes = (int)x[5];
                    p.CreatorAid = (int)x[6];
                    return p;
                };
            }
            else
            {
                return this.BadRequest($"Invalid post type. Expected types: " +
                        $"<{PostType.HousingPost}>, <{PostType.SellingPost}> and <{PostType.SocialMediaPost}>." +
                        $"But received <{postType}> instead.");
            }
                    
            try
            {
                List<PostModel> posts = QueryHandler.SqlQueryFromConnection<PostModel>(query,
                                            mapFunction,
                                            dbConnection);
                return posts;
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
                // return this.Problem(detail:ex.Message,title:"SQL Query Error");
            }*/
        }



    }

    
}
