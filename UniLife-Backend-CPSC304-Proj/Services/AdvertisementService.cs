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
    public class AdvertisementService
    {
        private readonly IDbConnection dbConnection;

        public AdvertisementService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void CreateNewAdvertisement(string ad_description, float price, string title)
        {
            string hashString = $"{ad_description}{price}{title}";
            MD5 md5Hasher = MD5.Create();
            byte[] hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            int generatedAdID = BitConverter.ToInt32(hashed, 0);

            string advertisementQueryString = "INSERT [dbo].Advertisements([AdID], [ad_description], [price], [title], [clicks]) " +
                $"VALUES({generatedAdID}, '{ad_description}', {price}, '{title}', 0)";

            try
            {
                QueryHandler.SqlExecutionQueryFromConnection(advertisementQueryString, dbConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public List<AdvertisementModel> GetAdvertisementsWithAllCategories(
            string[] categories,
            AdvertisementModel.OrderByValue orderBy = AdvertisementModel.OrderByValue.CreatedDate,
            bool asc = false)
        {
            SelectionQueryObject<AdvertisementModel> sQuery = GetAllAdvertisementsQuery(orderBy, asc);

            string[] selectedCategoriesWhereConditions = new string[categories.Length];


            for (int i = 0; i < categories.Length; i++)
            {
                selectedCategoriesWhereConditions[i] = $"C.ctg_type='{categories[i]}'";
            }

            string selectedCategoriesWhereClause = String.Join(" or ", selectedCategoriesWhereConditions);

            // Division: P / selectedCategories
            sQuery.AddToWhereClause("WHERE not exists (" +
                $"(select C.ctg_type from Categories C where {selectedCategoriesWhereClause}) " +
                "except " +
                "(select AC.ctg_type from Ad_category AC where AD.AdID = AC.AdID) " +
                ")");
            // Make sure selected categories exist in the post_category
            sQuery.AddToWhereClause("and 0<(select Count(C.ctg_type) from Categories C " +
                $"where {selectedCategoriesWhereClause})");

            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }

        private SelectionQueryObject<AdvertisementModel> GetAllAdvertisementsQuery(AdvertisementModel.OrderByValue orderBy = AdvertisementModel.OrderByValue.CreatedDate, bool asc = false)
        {

            string orderAttribute = GetOrderByAttribute(orderBy);

            Func<DbDataReader, AdvertisementModel> mapFunction = (x) =>
            {
                AdvertisementModel a = new AdvertisementModel();
                a.Adid = (int)x[0];
                a.ad_description = (string)x[1];
                a.price = (float)x[2];
                a.title = (string)x[3];
                a.clicks = (int)x[4];
                a.CreatedDate = (DateTime)x[5];
                return a;
            };

            SelectionQueryObject<AdvertisementModel> sQuery = new SelectionQueryObject<AdvertisementModel>(mapFunction);
            sQuery.Select("AD.[AdID], AD.[ad_description], AD.[price], AD.[title], AD.[clicks], AD.[Create_Date]")
                .From("[dbo].[Advertisements] AD")
                .OrderBy(orderAttribute)
                .SetIsAscending(asc)
                .SetIsDistinct(true);

            return sQuery;
        }

        private string GetOrderByAttribute(AdvertisementModel.OrderByValue order)
        {
            switch (order)
            {
                case AdvertisementModel.OrderByValue.Title:
                    return "Title";
                case AdvertisementModel.OrderByValue.CreatedDate:
                    return "Create_Date";
                default:
                    throw new InvalidTypeException("Invalid Order By Value. Expecting " +
                        $"<{AdvertisementModel.OrderByValue.Title}>, or <{AdvertisementModel.OrderByValue.CreatedDate}>. " +
                        $"But received <{order}> instead");
            }
        }
    }
}
