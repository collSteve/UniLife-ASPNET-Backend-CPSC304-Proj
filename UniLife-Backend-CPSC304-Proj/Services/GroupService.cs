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
        public void CreateGroup(string GroupName, int Aid)
        {

            string hashString = $"{GroupName}{Aid}";
            MD5 md5Hasher = MD5.Create();
            byte[] hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            int generatedGID = BitConverter.ToInt32(hashed, 0);

           

            string query = @"INSERT INTO [dbo].[Group]([Gid], [Group_Name])"  +
                            $"VALUES ({generatedGID}, '{GroupName}')";

            string squery = @"INSERT INTO [dbo].[Member_Of]([AID], [GID], [Role])" +
                $"VALUES ({Aid},{generatedGID}, 'admin')";

            try
            {
                QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
                QueryHandler.SqlExecutionQueryFromConnection(squery, dbConnection);
            }catch(SqlException s) {
                DeleteGroup(generatedGID);
            }
        }

        //delete group given gid
        public void DeleteGroup(int Gid) {
            string query = @"DELETE FROM [dbo].[Group] WHERE [Gid]=" + Gid;
            QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
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
            string query = @"SELECT [Gid],[Group_Name]" +
                        "from [dbo].[Group]" +
                        $"where [Group_Name] like '%{name}%'";
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

        public void updateGroupName(string name, int Gid) {
            string query = @"UPDATE [dbo].[Group]" +
                $"SET [Group_Name] = '{name}'" +
                $"WHERE [Gid] = {Gid}";

            if (name != "") { 
                QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
            }
        }

        public int getMemberCount(int Gid)
        {
            string query = @"SELECT COUNT([AID])" +
                $"FROM [dbo].[Member_Of]" +
                $"WHERE [Gid] = {Gid}";

            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
                g.Gid = (int)x[0];
                return g;
            };

            List<GroupModel> groupL = QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);
            return groupL[0].Gid;
        }

        public Boolean isAdmin(int gid, int aid) {
            string query = @$"SELECT Role FROM [dbo].[Member_Of] WHERE Gid = {gid} and Aid = {aid}";

            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
                g.GroupName = (string)x[0];
                return g;
            };

            List<GroupModel> groupL = QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);

            string val = groupL[0].GroupName;

            return (val.Equals("admin"));
        }


    }
}
