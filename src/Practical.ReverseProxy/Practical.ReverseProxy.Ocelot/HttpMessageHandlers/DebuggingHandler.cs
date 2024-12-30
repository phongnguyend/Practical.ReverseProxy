using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.Ocelot.HttpMessageHandlers;

public class DebuggingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DebuggingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}
