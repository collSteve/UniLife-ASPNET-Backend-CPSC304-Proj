﻿using Microsoft.AspNetCore.Http;
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
        [HttpGet("create")]

        public ActionResult CreateGroup(GroupNewObj gno) {

            int Gid = gno.Gid;
            string groupName = gno.GroupName;

            try
            {
               groupService.CreateGroup(Gid, groupName);
                return Ok();

            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }

        }

        // www.server.com/api/Group/delete
        [HttpGet("delete")]

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
    }
}



