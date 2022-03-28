using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
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
        public List<PostModel> GetPostsByType(string postType, 
            PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, 
            bool asc = false)
        {
            #region c4
            /*if (postType.ToLower().Equals(PostType.SellingPost.ToLower()))
            {
                return GetAllSellingPosts(orderBy, asc);
            }
            else if (postType.ToLower().Equals(PostType.HousingPost.ToLower()))
            {
                return GetAllHousingPosts(orderBy, asc);
            }
            else if (postType.ToLower().Equals(PostType.SocialMediaPost.ToLower()))
            {
                return GetAllSocialMediaPosts(orderBy, asc);
            }
            else
            {
                throw new InvalidTypeException($"Invalid post type. Expected types: " +
                        $"<{PostType.HousingPost}>, <{PostType.SellingPost}> and <{PostType.SocialMediaPost}>." +
                        $"But received <{postType}> instead.");
            }*/
            #endregion
            SelectionQueryObject<PostModel> sQuery = GetPostsByTypeQuery(postType, orderBy, asc);
            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }

        private string GetOrderByAttribute(PostModel.OrderByValue order)
        {
            switch (order)
            {
                case PostModel.OrderByValue.Title:
                    return "Title";
                case PostModel.OrderByValue.CreatedDate:
                    return "Create_Date";
                default:
                    throw new InvalidTypeException("Invalid Order By Value. Expecting " +
                        $"<{PostModel.OrderByValue.Title}>, or <{PostModel.OrderByValue.CreatedDate}>. " +
                        $"But received <{order}> instead");
            }
        }
        
        private SelectionQueryObject<PostModel> GetAllSellingPostsQuery(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {

            string orderAttribute = GetOrderByAttribute(orderBy);

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
                p.PhoneNum = Convert.ToString(x[8]);
                return p;
            };

            SelectionQueryObject<PostModel> sQuery = new SelectionQueryObject<PostModel>(mapFunction);
            sQuery.Select("P.pid, title, [Create_Date], [Post_Body], [Num_Likes], "
                            + "[Num_Dislikes], [Creator_UID], [Email], Phone_Num ")
                .From("[dbo].[Post] P, [dbo].[Selling_Post] SP")
                .Where("P.PID = SP.PID")
                .OrderBy(orderAttribute)
                .SetIsAscending(asc)
                .SetIsDistinct(true);

            return sQuery;
        }

        private SelectionQueryObject<PostModel> GetAllHousingPostsQuery(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {
            string orderAttribute = GetOrderByAttribute(orderBy);

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

            SelectionQueryObject<PostModel> sQuery = new SelectionQueryObject<PostModel>(mapFunction);

            sQuery.Select("P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID], [Email], [Address]")
                .From("[dbo].[Post] P, [dbo].[Housing_Post] SP")
                .Where("P.PID = SP.PID")
                .OrderBy(orderAttribute)
                .SetIsAscending(asc)
                .SetIsDistinct(true);
            return sQuery;
        }

        private SelectionQueryObject<PostModel> GetAllSocialMediaPostsQuery(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {
            string orderAttribute = GetOrderByAttribute(orderBy);

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

            SelectionQueryObject<PostModel> sQuery = new SelectionQueryObject<PostModel>(mapFunction);

            sQuery.Select("P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID]")
                .From("[dbo].[Post] P, [dbo].[Social_Media_Post] SP")
                .Where("P.PID = SP.PID")
                .OrderBy(orderAttribute)
                .SetIsAscending(asc)
                .SetIsDistinct(true);

            return sQuery; 
        }

        public SelectionQueryObject<PostModel> GetPostsByTypeQuery(string postType,
            PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate,
            bool asc = false)
        {
            if (postType.ToLower().Equals(PostType.SellingPost.ToLower()))
            {
                return GetAllSellingPostsQuery(orderBy, asc);
            }
            else if (postType.ToLower().Equals(PostType.HousingPost.ToLower()))
            {
                return GetAllHousingPostsQuery(orderBy, asc);
            }
            else if (postType.ToLower().Equals(PostType.SocialMediaPost.ToLower()))
            {
                return GetAllSocialMediaPostsQuery(orderBy, asc);
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
        public List<PostModel> GetAllSellingPosts(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {
            #region c1
            /*string orderAttribute = GetOrderByAttribute(orderBy);

            string orderDirection = asc ? "ASC" : "DESC";

            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], " +
                                    "[Num_Dislikes], [Creator_UID], [Email], Phone_Num " +
                                    "from [dbo].[Post] P, [dbo].[Selling_Post] SP " +
                                    "where P.PID = SP.PID "+
                                    $"Order By {orderAttribute} " +
                                    $"{orderDirection}";
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
                                        dbConnection);*/
            #endregion
            SelectionQueryObject<PostModel> sQuery = GetAllSellingPostsQuery(orderBy, asc);
            return QueryHandler.SqlQueryFromConnection<PostModel>(sQuery.SqlQuery(), sQuery.QueryMapFunction, dbConnection);
        }

        public List<PostModel> GetAllHousingPosts(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {
            #region c2
            /*string orderAttribute = GetOrderByAttribute(orderBy);

            string orderDirection = asc ? "ASC" : "DESC";

            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID], [Email], [Address] " +
                        "from [dbo].[Post] P, [dbo].[Housing_Post] SP " +
                        "where P.PID = SP.PID " +
                        $"Order By {orderAttribute} " +
                        $"{orderDirection}";

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
            };*/

            #endregion
            SelectionQueryObject<PostModel> sQuery = GetAllHousingPostsQuery(orderBy, asc);
            return QueryHandler.SqlQueryFromConnection<PostModel>(sQuery, dbConnection);
        }

        public List<PostModel> GetAllSocialMediaPosts(PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, bool asc = false)
        {
            #region c3
            /*string orderAttribute = GetOrderByAttribute(orderBy);

            string orderDirection = asc ? "ASC" : "DESC";

            string query = @"SELECT P.pid, title, [Create_Date], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID] " +
                        "from [dbo].[Post] P, [dbo].[Social_Media_Post] SP " +
                        "where P.PID = SP.PID " +
                        $"Order By {orderAttribute} " +
                        $"{orderDirection}";

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
                                        dbConnection);*/
            #endregion
            SelectionQueryObject<PostModel> sQuery = GetAllSocialMediaPostsQuery(orderBy, asc);
            return QueryHandler.SqlQueryFromConnection<PostModel>(sQuery, dbConnection);
        }

        public List<PostModel> SearchPostsType(string postType, 
            string title,
            PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate, 
            bool asc = false)
        {
            SelectionQueryObject<PostModel> sQuery = GetPostsByTypeQuery(postType, orderBy, asc);

            sQuery.AddToWhereClause($"and (title LIKE '%{title}%' or [Post_Body] LIKE '%{title}%')");

            return QueryHandler.SqlQueryFromConnection<PostModel>(sQuery, dbConnection);
        }

        public List<PostModel> GetPostsWithAllCategories(string postType, 
            string[] categories,
            PostModel.OrderByValue orderBy = PostModel.OrderByValue.CreatedDate,
            bool asc = false)
        {
            SelectionQueryObject<PostModel> sQuery = GetPostsByTypeQuery(postType, orderBy, asc);

            string[] selectedCategoriesWhereConditions = new string[categories.Length];


            for (int i = 0; i < categories.Length; i++)
            {
                selectedCategoriesWhereConditions[i] = $"C.ctg_type='{categories[i]}'";
            }

            string selectedCategoriesWhereClause = String.Join(" or ", selectedCategoriesWhereConditions);

            // Division: P / selectedCategories
            sQuery.AddToWhereClause("and not exists (" +
                $"(select C.ctg_type from Categories C where {selectedCategoriesWhereClause}) " +
                "except " +
                "(select PC.ctg_type from Post_category PC where P.PID = PC.PID) " +
                ")");
            // Make sure selected categories exist in the post_category
            sQuery.AddToWhereClause("and 0<(select Count(C.ctg_type) from Categories C " +
                $"where {selectedCategoriesWhereClause})");

            Console.WriteLine(sQuery.FormattedSqlQuery());

            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }

        public void InsertNewPost(string postType, string postTitle, string postBody, 
            DateTime createDate, int creatorUID, string? email, string? phoneNumber, string? address)
        {
            // generate pid by hashing
            string hashString = $"{creatorUID}{createDate}{postTitle}";
            MD5 md5Hasher = MD5.Create();
            byte[] hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            int generatedPID = BitConverter.ToInt32(hashed, 0);

            // insert to Post table
            string postQueryString = "INSERT [dbo].Post([PID], [Create_Date], [Title], [Post_Body], [Num_Likes], [Num_Dislikes], [Creator_UID]) " +
            $"VALUES({generatedPID}, '{createDate}', '{postTitle}', '{postBody}', 0, 0, {creatorUID})";

            Console.WriteLine(postQueryString);

            // insert to according types
            string insertTypeQuery = GetInsertQueryForPostTypes(postType, generatedPID,
                email, phoneNumber, address);
            Console.WriteLine(insertTypeQuery);

            QueryHandler.SqlExecutionQueryFromConnection(postQueryString, dbConnection);

            try
            {
                QueryHandler.SqlExecutionQueryFromConnection(insertTypeQuery, dbConnection);
            }
            catch (SqlException ex)
            {
                // delete added post 
                DeletePost(generatedPID);
                throw ex;
            }
        }

        private string GetInsertQueryForPostTypes(string postType, int pid, 
            string? email, string? phoneNumber, string? address)
        {
            string query = "";
            if (postType.ToLower().Equals(PostType.SellingPost.ToLower()))
            {
                query = "INSERT [dbo].Selling_Post([PID], [Phone_Num], [Email]) " +
                    $"VALUES({pid}, {phoneNumber ?? ""}, '{email ?? ""}')";
            }
            else if (postType.ToLower().Equals(PostType.HousingPost.ToLower()))
            {
                query = "INSERT [dbo].Housing_Post([PID], [Address], [Email]) " +
                    $"VALUES({pid}, '{address ?? ""}', '{email ?? ""}')";
            }
            else if (postType.ToLower().Equals(PostType.SocialMediaPost.ToLower()))
            {
                query = "INSERT [dbo].Social_Media_Post([PID]) " +
                    $"VALUES({pid})";
            }
            else
            {
                throw new InvalidTypeException($"Invalid post type. Expected types: " +
                        $"<{PostType.HousingPost}>, <{PostType.SellingPost}> and <{PostType.SocialMediaPost}>." +
                        $"But received <{postType}> instead.");
            }
            return query;
        }

        public void DeletePost(int pid)
        {
            string deleteQuery = $"DELETE FROM [dbo].Post WHERE pid={pid}";
            QueryHandler.SqlExecutionQueryFromConnection(deleteQuery, dbConnection);
        }

        public void UpdatePost(int pid, string? postTitle, string? postBody,
            int? numLikes, int? numDisLikes,
            string? email, string? phoneNumber, string? address)
        {
            string postType = DeterminePostType(pid)?? 
                throw new NonExistingObjectException($"Post with PID {pid} does not exist.");

            // update post table
            List<string> setClauses = new List<string>();
            if (postTitle != null) setClauses.Add($"title = '{postTitle}'");
            if (postBody != null) setClauses.Add($"Post_Body = '{postBody}'");
            if (numLikes != null && numLikes >= 0) setClauses.Add($"Num_Likes = {numLikes}");
            if (numDisLikes != null && numDisLikes >= 0) setClauses.Add($"Num_Dislikes = {numDisLikes}");

            if (setClauses.Count > 0)
            {
                string setClause = String.Join(", ", setClauses.ToArray());
                string updatePostQuery = $"UPDATE [dbo].Post SET {setClause} WHERE pid={pid}";
                QueryHandler.SqlExecutionQueryFromConnection(updatePostQuery, dbConnection);
            }


            // update type specific table
            List<string> typeSetClauses = new List<string>();
            string? updateTypePostQuery = null;
            switch (postType)
            {
                case PostType.SellingPost:
                    {
                        if (email != null) typeSetClauses.Add($"email = '{email}'");
                        if (phoneNumber != null) typeSetClauses.Add($"Phone_Num = '{phoneNumber}'");
                        string typeSetClause = String.Join(", ", typeSetClauses.ToArray());

                        updateTypePostQuery = $"UPDATE [dbo].Selling_Post SET {typeSetClause} WHERE pid={pid}";
                        break;
                    }
                    
                case PostType.HousingPost:
                    {
                        if (email != null) typeSetClauses.Add($"email = '{email}'");
                        if (address != null) typeSetClauses.Add($"address = '{address}'");
                        string typeSetClause = String.Join(", ", typeSetClauses.ToArray());

                        updateTypePostQuery = $"UPDATE [dbo].Housing_Post SET {typeSetClause} WHERE pid={pid}";
                        break;
                    }
                    
            }

            if (updateTypePostQuery != null && typeSetClauses.Count>0)
                QueryHandler.SqlExecutionQueryFromConnection(updateTypePostQuery, dbConnection);
        }

        private string? DeterminePostType(int pid)
        {
            var numPostQuery = (string s, int id) => $"Select Count(pid) from [dbo].{s} where pid={id}";

           int selling = QueryHandler.SqlQueryFromConnection<int>(numPostQuery("Selling_Post", pid), 
                (s) => (int)s[0], dbConnection)[0];
            int housing = QueryHandler.SqlQueryFromConnection<int>(numPostQuery("Housing_Post", pid),
                (s) => (int)s[0], dbConnection)[0];
            int socialMedia = QueryHandler.SqlQueryFromConnection<int>(numPostQuery("Social_Media_Post", pid),
                (s) => (int)s[0], dbConnection)[0];

            if (selling > 0) return PostType.SellingPost;
            if (housing > 0) return PostType.HousingPost;
            if (socialMedia > 0) return PostType.SocialMediaPost;
            return null;
        }

        public void CreateComment(int pid, int creatorUid, string commentBody)
        {
            // generate cid
            string hashString = $"{pid}{creatorUid}{commentBody}";
            MD5 md5Hasher = MD5.Create();
            byte[] hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            int cid = BitConverter.ToInt32(hashed, 0);

            string commentQueryString = "INSERT [dbo].Comment([CID], [Comment_Body], [Creator_UID], [PID]) " +
            $"VALUES({cid}, '{commentBody}', {creatorUid}, {pid})";

            QueryHandler.SqlExecutionQueryFromConnection(commentQueryString, dbConnection);
        }

        public void DeleteComment(int cid)
        {
            string deleteQuery = $"DELETE FROM [dbo].Comment WHERE cid={cid}";
            QueryHandler.SqlExecutionQueryFromConnection(deleteQuery, dbConnection);
        }
    }
}
