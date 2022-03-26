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
                if (orderBy != null && asc != null)
                {
                    return postService.GetPostsByType(postType,(PostModel.OrderByValue) orderBy,(bool) asc);
                }
                else if (orderBy == null && asc != null)
                {
                    return postService.GetPostsByType(postType, asc: (bool)asc);
                }
                else if (orderBy != null && asc == null)
                {
                    return postService.GetPostsByType(postType, orderBy:(PostModel.OrderByValue)orderBy);
                }
                else
                {
                    return postService.GetPostsByType(postType);
                }
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

        [HttpGet("SerachTitle")]
        public ActionResult<List<PostModel>> GetPostBySerachTitle(string title)
        {
            try
            {
                // return postService.SearchSocialMediaPosts(title);
                return postService.SearchPostsType("sellingpost", title);
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
    }
}
