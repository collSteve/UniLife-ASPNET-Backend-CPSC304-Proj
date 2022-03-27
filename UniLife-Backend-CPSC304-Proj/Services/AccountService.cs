using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Services
{
    public class AccountService
    {
        private readonly IDbConnection dbConnection;

        public AccountService(IDbConnection connection)
        {
            dbConnection = connection;
        }

        //adds account to groupName
        public List<GroupModel> GetAllGroups()
        {
            string query = @"SELECT [Gid], [Group_Name]" +
                        "from [dbo].[Group]";
            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
                g.Gid = (int)x[0];
                g.GroupName = (string)x[1];

                return g;
            };

            return QueryHandler.SqlQueryFromConnection<GroupModel>(query,
                                        mapFunction,
                                        dbConnection);
        }
    }
}
