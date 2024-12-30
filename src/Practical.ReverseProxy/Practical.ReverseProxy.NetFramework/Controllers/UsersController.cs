using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Practical.ReverseProxy.NetFramework.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ProxyController
    {
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [Route("{*path}")]
        public Task<HttpResponseMessage> Proxy(string path)
        {
            return SendAsync($"https://localhost:44352/api/users/{path}");
        }
    }
}
