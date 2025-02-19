﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Exceptions;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Services;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;
using UniLife_Backend_CPSC304_Proj.Exceptions;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        internal class AccountType
        {
            public const string UserAccount = "UserAccount";
            public const string BusinessAccount = "BusinessAccount";
            public const string AdminAccount = "AdminAccount";
        }

        private readonly IDbConnection dbConnection;
        private readonly AccountService accountService;

        public AccountsController(IDbConnection connection, AccountService accountService)
        {
            dbConnection = connection;
            this.accountService = accountService;
        }

        [HttpPost]
        public ActionResult CreateNewAccount([FromBody] CreateNewAccountRequestObj createNewAccountRequestObj)
        {
            try
            {
                accountService.CreateNewAccount(
                    createNewAccountRequestObj.AccountType,
                    createNewAccountRequestObj.Username,
                    createNewAccountRequestObj.Email,
                    createNewAccountRequestObj.Password);
                return Ok();
            }
            catch (InvalidTypeException ex)
            {
                return BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult DeleteAccount([FromBody] int aid)
        {
            try
            {
                accountService.DeleteAccount(aid);
                return Ok();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult UpdateAccount([FromBody] UpdateAccountRequestObj updateAccountRequestObj)
        {
            try
            {
                accountService.UpdateAccount(
                    updateAccountRequestObj.Aid,
                    updateAccountRequestObj.username,
                    updateAccountRequestObj.password,
                    updateAccountRequestObj.email,
                    updateAccountRequestObj.seller_rating);
                return Ok();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
            catch (NonExistingObjectException ex)
            {
                return this.BadRequest($"[Non-Existing Object]: {ex.Message}");
            }
        }

        [HttpPut("Group")]
        public ActionResult JoinGroup([FromBody]accGroupObj acc) {

            int aid = acc.AID;
            int gid = acc.GID;
            string role = acc.Role;

            try
            {
                accountService.JoinGroup(aid, gid, role);
                return Ok();
            }
            catch (InvalidTypeException ex)
            {
                return this.BadRequest($"[Invalid Type Error]: {ex.Message}");
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        // Getting all accounts
        [HttpGet]
        public ActionResult<List<AccountModel>> GetAllAccounts()
        {
            try
            {
                return accountService.GetAllAccounts();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
        
        // Getting all accounts' Username
        [HttpGet("Username")]
        public ActionResult<List<GetUsernameandEmailRequestObject>> GetAllAccountsUsernameAndEmail()
        {
            try
            {
                return accountService.GetAllAccountsUsernameAndEmail();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpGet("Count/University")]
        public ActionResult<List<NumberUsersInUniverityObj>> GetNumberUsersInUniversities()
        {
            try
            {
                return accountService.GetNumberUsersInUniversities();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }

        [HttpGet("MaxR")]
        public ActionResult<List<UserswithMaximumRatingObj>> GetUsersWithMaximumRating()
        {
            try
            {
                return accountService.GetUserswithMaximumRating();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
    }
}
