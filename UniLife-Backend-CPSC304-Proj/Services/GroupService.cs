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
    }
}
