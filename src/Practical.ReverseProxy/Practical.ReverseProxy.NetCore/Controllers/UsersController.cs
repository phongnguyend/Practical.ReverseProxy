using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.NetCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ProxyController
{
    [HttpPost("login")]
    public async Task Login()
    {
        await Send("https://localhost:44352/api/users/login");
    }

    [HttpPost("refreshToken")]
    public async Task RefreshToken()
    {
        await Send("https://localhost:44352/api/users/refreshToken");
    }

    [HttpGet]
    public async Task Get()
    {
        await Send("https://localhost:44352/api/users");
    }

    [HttpPost]
    public async Task Post(object model)
    {
        await Send("https://localhost:44352/api/users");
    }

    [HttpPut("{id}")]
    public async Task Put(string id, object model)
    {
        await Send($"https://localhost:44352/api/users/{id}");
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        await Send($"https://localhost:44352/api/users/{id}");
    }
}
