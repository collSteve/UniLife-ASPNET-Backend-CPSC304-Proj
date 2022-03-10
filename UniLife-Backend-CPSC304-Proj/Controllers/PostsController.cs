using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        [HttpGet] 
        public IEnumerable<string> Get()
        {
            return new[] { "My Post", "test test", "Hi fuck you" };
        }
    }
}
