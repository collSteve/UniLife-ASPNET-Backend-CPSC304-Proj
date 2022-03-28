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
    public class AccountService
    {
        private readonly IDbConnection dbConnection;

        internal class AccountType
        {
            public const string UserAccount = "UserAccount";
            public const string BusinessAccount = "BusinessAccount";
            public const string AdminAccount = "AdminAccount";
        }

        public AccountService(IDbConnection connection)
        {
            dbConnection = connection;
        }

        public void CreateNewAccount(string accountType, string username, string password, string email)
        {
            string hashString = $"{accountType}{username}{email}";
            MD5 md5Hasher = MD5.Create();
            byte[] hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            int generatedAID = BitConverter.ToInt32(hashed, 0);

            string accountQueryString = "INSERT [dbo].Account([AID], [Username], [Email], [Password]) " +
                $"VALUES({generatedAID}, '{username}', '{password}', '{email}')";

            string createAccountTypeQuery = GetCreateQueryForAccountTypes(accountType, generatedAID);

            // Insert tuple into the Account Table
            QueryHandler.SqlExecutionQueryFromConnection(accountQueryString, dbConnection);

            try
            {
                // Insert tuple into its Account type table
                QueryHandler.SqlExecutionQueryFromConnection(createAccountTypeQuery, dbConnection);
            }
            catch (SqlException ex)
            {
                // delete added Added 
                string deleteQuery = "";

                throw ex;
            }
        }

        private string GetCreateQueryForAccountTypes(string accountType, int aid)
        {
            string query = "";
            if (accountType.ToLower().Equals(AccountType.UserAccount.ToLower()))
            {
                query = "INSERT [dbo].User_Account([AID], [Seller_Rating]) " +
                    $"VALUES({aid}, 0)";
            }
            else if (accountType.ToLower().Equals(AccountType.BusinessAccount.ToLower()))
            {
                query = "INSERT [dbo].Business_Account([AID]) " + $"VALUES({aid})";
            }
            else if (accountType.ToLower().Equals(AccountType.AdminAccount.ToLower()))
            {
                query = "INSERT [dbo].Admin_Account([AID]) " + $"VALUES({aid})";
            }
            else
            {
                throw new InvalidTypeException($"Invalid post type. Expected types: " +
                        $"<{AccountType.UserAccount}>, <{AccountType.BusinessAccount}> and <{AccountType.AdminAccount}>." +
                        $"But received <{accountType}> instead.");
            }
            return query;
        }

        public void DeleteAccount(int aid)
        {
            string deleteQuery = $"DELETE FROM [dbo].Account WHERE AID = {aid}";
            QueryHandler.SqlExecutionQueryFromConnection(deleteQuery, dbConnection);
        }

        public void UpdateAccount(int aid, string username, string password, string email)
        {

        }
        public void JoinGroup(int aid, int gid, string role) {

            string query = @"INSERT INTO [dbo].[Member_Of]([AID], [GID], [Role])" + 
                $"VALUES ({aid}, {gid}, '{role}')";
            QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
        }


        /*
        public List<AccountModel> GetAccountsinGroup()
        {
            List<AccountModel> UserAccounts = GetUserAccounts();
            return UserAccounts;

        }

        public List<AccountModel> GetUserAccounts()
        {
            SelectionQueryObject<AccountModel> sQuery = GetUserAccountsQuery();
            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }

        public List<AccountModel> GetUserAccountsQuery()
        {
            Func<DbDataReader, AccountModel> mapFunction = (x) =>
            {
                AccountModel acc = new AccountModel();
                acc.Username = (string)x[0];
                return acc;
            };

            SelectionQueryObject<AccountModel> sQuery = new SelectionQueryObject<AccountModel>(mapFunction);
            sQuery.Select("Account.Username")
                .From("[dbo].[Account], [dbo].[User_Account], [dbo.]")
                .Where("Account.AID = User_Account.AID")
                .SetIsDistinct(true);
            return sQuery;
        }
        */
    }
}
