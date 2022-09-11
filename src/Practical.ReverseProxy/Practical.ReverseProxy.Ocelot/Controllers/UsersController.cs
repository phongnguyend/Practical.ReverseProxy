using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Practical.ReverseProxy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public List<string> Get()
        //{
        //    return new List<string>()
        //    {
        //        "test"
        //    };
        //}
    }
}
