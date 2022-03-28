using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Services
{
    public class GroupService
    {
        private readonly IDbConnection dbConnection;


        public GroupService(IDbConnection connection)
        {
            dbConnection = connection;
        }

        //gets all groups
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

        //inserts new group
        public void CreateGroup(int Gid, string GroupName)
        {
            string query = @"INSERT INTO [dbo].[Group]([Gid], [Group_Name])"  +
                            $"VALUES ('{Gid}', '{GroupName}'";

            QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
        }

        //delete group given gid
        public void DeleteGroup(int Gid) {
            string query = @"DELETE FROM [dbo].[Group] WHERE [Gid]=" + Gid;
        }

        //categories not an attribute yet
        /*
        public List<GroupModel> getGroupCategory(string cat)
        {
            string query = @"SELECT [Gid], [Group_Name]" +
                        "from [dbo].[Group]" +
                        "where [category] = '{cat}'";
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
        */

        //selects groups with groupnames containing name
        public List<GroupModel> getGroupByName(string name, bool? asc)
        {
            string query = @"SELECT [Group_Name]" +
                        "from [dbo].[Group]" +
                        "where [Group_Name] like '%{name}%'";
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
