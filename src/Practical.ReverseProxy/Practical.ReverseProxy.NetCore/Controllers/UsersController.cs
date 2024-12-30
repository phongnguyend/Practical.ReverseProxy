using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.NetCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ProxyController
{
    [HttpGet]
    public async Task Get()
    {
        await SendAsync("https://localhost:44352/api/users");
    }

    [HttpPost]
    public async Task Post(object model)
    {
        await SendAsync("https://localhost:44352/api/users");
    }

    [HttpPut("{id}")]
    public async Task Put(string id, object model)
    {
        await SendAsync($"https://localhost:44352/api/users/{id}");
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await SendAsync($"https://localhost:44352/api/users/{id}");
    }
}
