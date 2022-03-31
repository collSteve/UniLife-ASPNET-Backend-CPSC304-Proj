using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Services;
using UniLife_Backend_CPSC304_Proj.Utils;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {


        private readonly IDbConnection dbConnection;
        private readonly GroupService groupService;


        public GroupController(IDbConnection connection, GroupService groupService)
        {
            dbConnection = connection;
            this.groupService = groupService;
        }

        // www.server.com/api/Group
        [HttpGet]
        public ActionResult<List<GroupModel>> GetAllGroups()
        {
            try
            {
                return groupService.GetAllGroups();

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }
        // www.server.com/api/Group/create
        [HttpPost]
        public ActionResult CreateGroup([FromBody]GroupNewObj gno) {

   
            string groupName = gno.GroupName;
            int Aid = gno.Aid;

            try
            {
               groupService.CreateGroup(groupName, Aid);
                return Ok();

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }

        // www.server.com/api/Group/delete
        [HttpDelete]

        public ActionResult DeleteGroup(int Gid)
        {
            try
            {
                groupService.DeleteGroup(Gid);
                return Ok();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }

        // www.server.com/api/Group/Category
        /* [HttpGet("Category")]

         public ActionResult getGroupCategory(string cat)
         {
             try
             {
                 return groupService.getGroupCategory(cat);

             }
             catch (SqlException ex)
             {
                 return this.BadRequest($"[SQL Query Error]: {ex.Message}");
             }

         }*/

        
        // www.server.com/api/Group/search/(name)
        [HttpGet("search/{Group_Name}")]
        public ActionResult<List<GroupModel>> getGroupByName(string Group_Name, bool? asc)
        {
            try
            {
                return groupService.getGroupByName(Group_Name, asc ?? false);

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }

        [HttpPut]
        public ActionResult updateName(GroupUpdateObj obj) {

            string name = obj.name;
            int gid = obj.Gid;

            try {
                groupService.updateGroupName(name, gid);
                return Ok();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpGet("Members/{Gid}")]
        public ActionResult<int> getMemberCount(int Gid) {

            try
            {
                return groupService.getMemberCount(Gid);
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
        [HttpGet("Group/{Gid}/{Aid}")]
        public ActionResult<Boolean> isAdmin(int Gid, int Aid) {
            try
            {
                return groupService.isAdmin(Gid, Aid);
            }
            catch (SqlException ex){
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
    }
}



