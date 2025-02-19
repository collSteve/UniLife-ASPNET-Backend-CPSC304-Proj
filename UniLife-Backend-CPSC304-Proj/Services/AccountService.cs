﻿using System.Data;
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

        public void UpdateAccount(int aid, string? username, string? password, string? email, float? seller_rating)
        {
            string accountType = DetermineAccountType(aid) ??
                throw new NonExistingObjectException($"Post with PID {aid} does not exist.");

            List<string> setClauses = new List<string>();
            if (username != null) setClauses.Add($"Username = '{username}'");
            if (password != null) setClauses.Add($"Password = '{password}'");
            if (email != null) setClauses.Add($"Email = '{email}'");

            if (setClauses.Count > 0)
            {
                string setClause = String.Join(", ", setClauses.ToArray());
                string updateAccountQuery = $"UPDATE [dbo].Account SET {setClause} WHERE AID={aid}";
                QueryHandler.SqlExecutionQueryFromConnection(updateAccountQuery, dbConnection);
            }

            if (seller_rating != null)
            {
                string updateTypeAccountQuery = $"UPDATE [dbo].User_Account SET Seller_Rating = '{seller_rating}' WHERE AID={aid}";
                QueryHandler.SqlExecutionQueryFromConnection(updateTypeAccountQuery, dbConnection);
            }
        }

        public string? DetermineAccountType(int aid)
        {
            var numAccountQuery = (string a, int id) => $"Select Count(AID) from [dbo].{a} where Aid={id}";

            int user = QueryHandler.SqlQueryFromConnection<int>(numAccountQuery("User_Account", aid),
                 (s) => (int)s[0], dbConnection)[0];
            int admin = QueryHandler.SqlQueryFromConnection<int>(numAccountQuery("Admin_Account", aid),
                (s) => (int)s[0], dbConnection)[0];
            int business = QueryHandler.SqlQueryFromConnection<int>(numAccountQuery("Business_Account", aid),
                (s) => (int)s[0], dbConnection)[0];

            if (user > 0) return AccountType.UserAccount;
            if (admin > 0) return AccountType.AdminAccount;
            if (business > 0) return AccountType.BusinessAccount;
            return null;
        }
        public List<GetUsernameandEmailRequestObject> GetAllAccountsUsernameAndEmail()
        {
            List<GetUsernameandEmailRequestObject> userAccounts = GetAllUserAccountsUsernameAndEmail();
            List<GetUsernameandEmailRequestObject> adminAccounts = GetAllAdminAccountsUsernameAndEmail();
            List<GetUsernameandEmailRequestObject> businessAccounts = GetAllBusinessAccountsUsernameAndEmail();

            return adminAccounts.Concat(userAccounts.Concat(businessAccounts)).ToList();
        }

        public List<AccountModel> GetAllAccounts()
        {
            List<AccountModel> userAccounts = GetAllUserAccounts();
            List<AccountModel> adminAccounts = GetAllAdminAccounts();
            List<AccountModel> businessAccounts = GetAllBusinessAccounts();

            return adminAccounts.Concat(userAccounts.Concat(businessAccounts)).ToList();
        }

        public List<AccountModel> GetAllUserAccounts()
        {
            Func<DbDataReader, AccountModel> mapFunction = (x) =>
            {
                AccountModel a = new AccountModel();
                a.Aid = (int)x[0];
                a.Username = (string)x[1];
                a.Password = (string)x[2];
                a.Email = (string)x[3];
                return a;
            };

            SelectionQueryObject<AccountModel> sQuery = new SelectionQueryObject<AccountModel>(mapFunction);
            sQuery.Select("A.[AID], A.[Username], A.[Password], A.[Email]")
                .From("[dbo].[User_Account] UA, [dbo].[Account] A")
                .Where("UA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<AccountModel>(sQuery, dbConnection);
        }

        public List<AccountModel> GetAllAdminAccounts()
        {
            Func<DbDataReader, AccountModel> mapFunction = (x) =>
            {
                AccountModel a = new AccountModel();
                a.Aid = (int)x[0];
                a.Username = (string)x[1];
                a.Password = (string)x[2];
                a.Email = (string)x[3];
                return a;
            };

            SelectionQueryObject<AccountModel> sQuery = new SelectionQueryObject<AccountModel>(mapFunction);
            sQuery.Select("A.[AID], A.[Username], A.[Password], A.[Email]")
                .From("[dbo].[Admin_Account] AA, [dbo].[Account] A")
                .Where("AA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<AccountModel>(sQuery, dbConnection);
        }

        public List<AccountModel> GetAllBusinessAccounts()
        {
            Func<DbDataReader, AccountModel> mapFunction = (x) =>
            {
                AccountModel a = new AccountModel();
                a.Aid = (int)x[0];
                a.Username = (string)x[1];
                a.Password = (string)x[2];
                a.Email = (string)x[3];
                return a;
            };

            SelectionQueryObject<AccountModel> sQuery = new SelectionQueryObject<AccountModel>(mapFunction);
            sQuery.Select("A.[AID], A.[Username], A.[Password], A.[Email]")
                .From("[dbo].[Business_Account] BA, [dbo].[Account] A")
                .Where("BA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<AccountModel>(sQuery, dbConnection);
        }

        public List<GetUsernameandEmailRequestObject> GetAllUserAccountsUsernameAndEmail()
        {
            Func<DbDataReader, GetUsernameandEmailRequestObject> mapFunction = (x) =>
            {
                GetUsernameandEmailRequestObject a = new GetUsernameandEmailRequestObject();
                a.Username = (string)x[0];
                a.Email = (string)x[1];
                return a;
            };

            SelectionQueryObject<GetUsernameandEmailRequestObject> sQuery = new SelectionQueryObject<GetUsernameandEmailRequestObject>(mapFunction);
            sQuery.Select("A.[Username], A.[Email]")
                .From("[dbo].[User_Account] UA, [dbo].[Account] A")
                .Where("UA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<GetUsernameandEmailRequestObject>(sQuery, dbConnection);
        }

        public List<GetUsernameandEmailRequestObject> GetAllAdminAccountsUsernameAndEmail()
        {
            Func<DbDataReader, GetUsernameandEmailRequestObject> mapFunction = (x) =>
            {
                GetUsernameandEmailRequestObject a = new GetUsernameandEmailRequestObject();
                a.Username = (string)x[0];
                a.Email = (string)x[1];
                return a;
            };

            SelectionQueryObject<GetUsernameandEmailRequestObject> sQuery = new SelectionQueryObject<GetUsernameandEmailRequestObject>(mapFunction);
            sQuery.Select("A.[Username], A.[Email]")
                .From("[dbo].[Admin_Account] AA, [dbo].[Account] A")
                .Where("AA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<GetUsernameandEmailRequestObject>(sQuery, dbConnection);
        }

        public List<GetUsernameandEmailRequestObject> GetAllBusinessAccountsUsernameAndEmail()
        {
            Func<DbDataReader, GetUsernameandEmailRequestObject> mapFunction = (x) =>
            {
                GetUsernameandEmailRequestObject a = new GetUsernameandEmailRequestObject();
                a.Username = (string)x[0];
                a.Email = (string)x[1];
                return a;
            };

            SelectionQueryObject<GetUsernameandEmailRequestObject> sQuery = new SelectionQueryObject<GetUsernameandEmailRequestObject>(mapFunction);
            sQuery.Select("A.[Username], A.[Email]")
                .From("[dbo].[Business_Account] BA, [dbo].[Account] A")
                .Where("BA.AID = A.AID");

            return QueryHandler.SqlQueryFromConnection<GetUsernameandEmailRequestObject>(sQuery, dbConnection);
        }

        public void JoinGroup(int aid, int gid, string role)
        {

            string query = @"INSERT INTO [dbo].[Member_Of]([AID], [GID], [Role])" +
                $"VALUES ({aid}, {gid}, '{role}')";
            QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
        }

        public List<NumberUsersInUniverityObj> GetNumberUsersInUniversities()
        {
            SelectionQueryObject<NumberUsersInUniverityObj> sQuery =
                new SelectionQueryObject<NumberUsersInUniverityObj>((u) =>
                {
                    NumberUsersInUniverityObj o = new NumberUsersInUniverityObj();
                    o.UniversityName = (string)u[0];
                    o.numUsers = (int)u[1];
                    return o;
                });

            sQuery.Select("UniName, Count(AID)")
                .From("[dbo].Enrolls_in")
                .GroupBy("UniName");

            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }

        public List<UserswithMaximumRatingObj> GetUserswithMaximumRating()
        {
            SelectionQueryObject<UserswithMaximumRatingObj> sQuery =
                new SelectionQueryObject<UserswithMaximumRatingObj>((u) =>
                {
                    UserswithMaximumRatingObj o = new UserswithMaximumRatingObj();
                    o.AID = (int)u[0];
                    o.username = (string)u[1];
                    o.max_rating = (double)u[2];
                    return o;
                });

            sQuery.Select("A.[AID], A.[username], UA.[Seller_Rating]")
                .From("[dbo].User_Account UA, [dbo].Account A")
                .Where("UA.AID = A.AID and UA.Seller_Rating =" +
                "(Select MAX(UA2.Seller_Rating) from [dbo].User_Account UA2)");

            return QueryHandler.SqlQueryFromConnection(sQuery, dbConnection);
        }
    }
}
