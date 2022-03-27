using Microsoft.AspNetCore.Http;
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
                    createNewAccountRequestObj.Password,
                    createNewAccountRequestObj.Email);
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
                accountService.updatePost(
                    updateAccountRequestObj.Aid,
                    updateAccountRequestObj.username,
                    updateAccountRequestObj.password,
                    updateAccountRequestObj.email);
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
        /*
        // Getting all accounts in group
        [HttpGet]
        public ActionResult<List<AccountModel>> GetAccountsinGroup()
        {
            try
            {
                return accountService.GetAccountsinGroup();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
        */
    }
}
