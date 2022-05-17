using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Practical.ReverseProxy.ReverseProxy.NetFramework
{
    public class HttpResponseMessageActionResult : IHttpActionResult
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public HttpResponseMessageActionResult(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            _httpResponseMessage.Headers.Remove("transfer-encoding");
            _httpResponseMessage.Headers.Remove("access-control-allow-origin");
            return Task.FromResult(_httpResponseMessage);
        }
    }
}