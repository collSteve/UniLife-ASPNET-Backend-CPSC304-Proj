using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UniLife_Backend_CPSC304_Proj.Controllers
{
    // www.here.com/api/posts
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        [HttpGet] 
        public IEnumerable<string> Get()
        {
            return new[] { "My Post", "test test", "Hi fuck you" };
        }

        // www.here.com/api/posts/hi
        [Route("hi")]
        [HttpGet]
        public IEnumerable<int> GetHi()
        {
            return new[] { 1, 2, 3, 4, 5 };
        }

        // www.here.com/api/posts/hi
        [Route("god")]
        [HttpGet]
        public IEnumerable<string> GetDog()
        {
            return new[] { "God" };
        }

    }
}
