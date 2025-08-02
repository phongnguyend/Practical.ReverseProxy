using Practical.ReverseProxy.Client.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UriHelper;

namespace Practical.ReverseProxy.Client;

internal class Program
{
    private const int API_PORT = 44352;
    private const int PROXY_NET_FRAMEWORK_PORT = 44346;
    private const int PROXY_NET_CORE_PORT = 44375;
    private const int OCELOT_PORT = 44347;
    private const int YARP_PORT = 44348;

    static async Task Main(string[] args)
    {
        int PORT = PROXY_NET_CORE_PORT;

        var baseUrl = $"https://localhost:{PORT}";

        var httpService = new HttpService(new HttpClient());

        var tokenResponse = await httpService.GetToken(UriPath.Combine(baseUrl, $"/api/users/login"), new LoginRequest
        {
            UserName = "test@abc.com"
        });

        var users = await httpService.GetAsync<List<UserModel>>(url: UriPath.Combine(baseUrl, $"/api/users"), accessToken: tokenResponse["accessToken"]);

        tokenResponse = await httpService.RefreshToken(UriPath.Combine(baseUrl, $"/api/users/refreshToken"), new RefreshTokenRequest
        {
            UserName = "test@abc.com",
            RefreshToken = tokenResponse["refreshToken"]
        });

        var user = await httpService.PostAsync<UserModel>(url: UriPath.Combine(baseUrl, $"/api/users"),
            data: new UserModel { Id = "3" },
            accessToken: tokenResponse["accessToken"]);

        user = await httpService.PutAsync<UserModel>(url: UriPath.Combine(baseUrl, $"/api/users/{user.Id}"),
            data: new UserModel { },
            accessToken: tokenResponse["accessToken"]);

        await httpService.DeleteAsync(url: UriPath.Combine(baseUrl, $"/api/users/{user.Id}"), accessToken: tokenResponse["accessToken"]);
    }
}
