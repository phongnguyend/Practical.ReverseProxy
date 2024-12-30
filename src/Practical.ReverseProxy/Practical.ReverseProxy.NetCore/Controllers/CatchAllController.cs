using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.NetCore.Controllers;

[ApiController]
public class CatchAllController : ProxyController
{
    [HttpGet("{**catchAll}")]
    [HttpPost("{**catchAll}")]
    [HttpPut("{**catchAll}")]
    [HttpPatch("{**catchAll}")]
    [HttpDelete("{**catchAll}")]
    public async Task CatchAll(string catchAll)
    {
        await SendAsync($"https://localhost:44352/{catchAll}{Request.QueryString}");
    }
}
