using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Services;
using UniLife_Backend_CPSC304_Proj.Models;
using UniLife_Backend_CPSC304_Proj.Utils;
using UniLife_Backend_CPSC304_Proj.Exceptions;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : Controller
    {
        private readonly IDbConnection dbConnection;
        private readonly AdvertisementService advertisementService;

        public AdvertisementsController(IDbConnection connection, AdvertisementService advertisementService)
        {
            dbConnection = connection;
            this.advertisementService = advertisementService;
        }

        [HttpPost]
        public ActionResult CreateNewAdvertisement([FromBody] CreateNewAdRequestObj createNewAdRequestObj)
        {
            try
            {
                advertisementService.CreateNewAdvertisement(
                    createNewAdRequestObj.ad_description,
                    createNewAdRequestObj.price,
                    createNewAdRequestObj.title);
                return Ok();
            }
            catch (SqlException ex)
            {
                return this.BadRequest($"[SQL Query Error]: {ex.Message}");
            }
        }
    }
}
