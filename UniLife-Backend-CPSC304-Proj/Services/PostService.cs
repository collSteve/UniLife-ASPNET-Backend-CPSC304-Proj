using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Services
{

    public class PostService
    {
        private readonly IDbConnection dbConnection;

        internal class PostType
        {

            public const string SellingPost = "SellingPost";
            public const string HousingPost = "HousingPost";
            public const string SocialMediaPost = "SocialMediaPost";

        }

        public PostService(IDbConnection connection)
        {
            dbConnection = connection;
        }

        /*
         * Throws: SqlException
         */
        public List<PostModel> GetAllPosts()
        {
            List<PostModel> sellingPosts = GetAllSellingPosts();
            List<PostModel> housingPosts = GetAllHousingPosts();
            List<PostModel> socialMediaPosts = GetAllSocialMediaPosts();

            return sellingPosts.Concat(housingPosts.Concat(socialMediaPosts)).ToList();
        }

        /*
         * Throws: InvalidTypeException, SqlException
         */
        public List<PostModel> GetPostsByType(string postType)
        {
            if (postType.ToLower().Equals(PostType.SellingPost.ToLower()))
            {
                return GetAllSellingPosts();
            }
            else if (postType.ToLower().Equals(PostType.HousingPost.ToLower()))
            {
                return GetAllHousingPosts();
            }
            else if (postType.ToLower().Equals(PostType.SocialMediaPost.ToLower()))
            {
                return GetAllSocialMediaPosts();
            }
            else
            {
                throw new InvalidTypeException($"Invalid post type. Expected types: " +
                        $"<{PostType.HousingPost}>, <{PostType.SellingPost}> and <{PostType.SocialMediaPost}>." +
                        $"But received <{postType}> instead.");
            }
        }

        /*
         * Throws: SqlException
         */
        public List<PostModel> GetAllSellingPosts()
        {
            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], " +
                                    "[Num_Dislikes], [Creator_UID], [Email], Phone_Num " +
                                    "from [dbo].[Post] P, [dbo].[Selling_Post] SP " +
                                    "where P.PID = SP.PID";
            Func<DbDataReader, PostModel>  mapFunction = (x) =>
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

            return QueryHandler.SqlQueryFromConnection<PostModel>(query,
                                        mapFunction,
                                        dbConnection);
        }

        public List<PostModel> GetAllHousingPosts()
        {
            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID], [Email], [Address] " +
                        "from [dbo].[Post] P, [dbo].[Housing_Post] SP " +
                        "where P.PID = SP.PID";
            Func<DbDataReader, PostModel> mapFunction = (x) =>
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

            return QueryHandler.SqlQueryFromConnection<PostModel>(query,
                                        mapFunction,
                                        dbConnection);
        }

        public List<PostModel> GetAllSocialMediaPosts()
        {
            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID] " +
                        "from [dbo].[Post] P, [dbo].[Social_Media_Post] SP " +
                        "where P.PID = SP.PID";
            Func<DbDataReader, PostModel> mapFunction = (x) =>
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

            return QueryHandler.SqlQueryFromConnection<PostModel>(query,
                                        mapFunction,
                                        dbConnection);
        }
    }
}
