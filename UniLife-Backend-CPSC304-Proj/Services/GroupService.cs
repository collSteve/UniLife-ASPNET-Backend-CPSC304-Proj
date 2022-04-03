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
            string query = @"select GID, Group_Name, memberCount 
                            from
                            (select * from [Group]
                            inner join
                            (select g.gid as gd, count(aid) as memberCount from [Group] g, Member_Of m where g.gid = m.gid group by g.gid) p ON [Group].GID = p.gd)n";
            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
                g.Gid = (int)x[0];
                g.GroupName = (string)x[1];
                g.MemberCount = (int)x[2];
               
                return g;
            };

            return QueryHandler.SqlQueryFromConnection<GroupModel>(query,
                                        mapFunction,
                                        dbConnection);
        }

        //inserts new group
        public void CreateGroup(string GroupName, int Aid)
        {
            DateTime localDate = DateTime.Now;
            string hashString = $"{GroupName}{Aid}{localDate}";
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
        //checks if given aid is admin in gid
        /*public Boolean isAdmin(int gid, int aid) {
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
        }*/
        //selects groups excluding those given aid is in
        public List<GroupModel> getNewGroups(int aid)
        {
            string query = @$"select * from [Group] except(select m.Gid, Group_Name from Member_Of m, [Group] g where Aid = {aid} and m.gid = g.gid)";

            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
                g.Gid = (int)x[0];
                g.GroupName = (string)x[1];
                return g;
            };

            return QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);

    
        }
        // selects groups that given aid is in
        public List<GroupModel> getMyGroups(int aid)
        {
            string query = @$"select t.Group_Name, [Role], q.gid, x.memberCount from Member_Of q, [Group] t, (select GID, Group_Name, memberCount 
                               from(select * from [Group]inner join(select g.gid as gd, count(aid) as memberCount from [Group] g, Member_Of m where g.gid = m.gid group by g.gid) p ON [Group].GID = p.gd)n)x 
                                where Aid = {aid} and q.gid = t.gid and t.gid = x.gid";

            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();
   
                g.GroupName = (string)x[0];
                g.Role = (string)x[1];
                g.Gid = (int)x[2];
                g.MemberCount = (int)x[3];
                return g;
            };

            return QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);


        }

        public List<GroupModel> getGroupGivenGid(int gid) {
            string query = @$"select g.gid, g.Group_Name, count(m.AID)from [group] g, [Member_Of] m where g.gid = {gid} and g.gid = m.gid group by g.gid, g.Group_Name";

            Func<DbDataReader, GroupModel> mapFunction = (x) =>
            {
                GroupModel g = new GroupModel();

                g.Gid = (int)x[0];
                g.GroupName = (string)x[1];
                g.MemberCount = (int)x[2];
                return g;
            };

            return QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);
        }

        public List<PostModel> getGroupPostIDs(int gid)
        {
            string query = @$"select p.pid, Create_Date, Title, Post_Body, Num_Likes, Num_Dislikes, Creator_UID, h.gid from [Post]p, [Has_Group_Post]h where h.pid = p.pid and gid = {gid}";

            Func<DbDataReader, PostModel> mapFunction = (x) =>
            {
                PostModel g = new PostModel();

                g.Pid = (int)x[0];
                g.CreatedDate = (DateTime)x[1];
                g.Title = (string)x[2];
                g.PostBody = (string)x[3];
                g.NumLikes = (int)x[4];
                g.NumDislikes  = (int)x[5];
                g.CreatorAid = (int)x[6];
                g.Gid = (int)x[7];
               
                return g;
            };

            return QueryHandler.SqlQueryFromConnection(query, mapFunction, dbConnection);
        }

        public void CreateGroupPost(int Gid, int Pid)
        {
           
            string query = @"INSERT INTO [dbo].[Has_Group_Post]([gid], [pid])"  +
                            $"VALUES ({Gid}, {Pid})";

            QueryHandler.SqlExecutionQueryFromConnection(query, dbConnection);
         
        }

    }
}
