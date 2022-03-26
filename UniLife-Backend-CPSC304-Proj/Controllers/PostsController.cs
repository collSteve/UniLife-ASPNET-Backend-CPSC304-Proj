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
    // www.server.com/api/posts
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


        // www.server.com/api/posts/Type/xxxx?  
        [HttpGet("Type/{postType}")]
        public ActionResult<List<PostModel>> GetPosts(string postType, PostModel.OrderByValue? orderBy, bool? asc) {
            try
            {
                return postService.GetPostsByType(postType,
                    orderBy ?? PostModel.OrderByValue.CreatedDate,
                    asc ?? false);
            } 
            catch(InvalidTypeException ex)
            {
                return this.BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpGet("SerachTitle/{postType}")]
        public ActionResult<List<PostModel>> GetPostBySerachTitle(string postType, string title,
            PostModel.OrderByValue? orderBy, bool? asc)
        {
            try
            {
                return postService.SearchPostsType(postType, title, 
                    orderBy ?? PostModel.OrderByValue.CreatedDate, 
                    asc ?? false);
            }
            catch (InvalidTypeException ex)
            {
                return this.BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpGet("Categories")]
        public ActionResult<List<PostModel>> GetPostByCategories(string postType, 
            [FromQuery(Name = "category")] string[] categories,
            PostModel.OrderByValue? orderBy, bool? asc)
        {
            try
            {
                return postService.GetPostsWithAllCategories(postType, categories,
                    orderBy ?? PostModel.OrderByValue.CreatedDate,
                    asc ?? false);
            }
            catch (InvalidTypeException ex)
            {
                return this.BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
    }
}
