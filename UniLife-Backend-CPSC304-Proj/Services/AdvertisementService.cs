using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;
using System.Security.Cryptography;
using System.Text;

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
    }
}
