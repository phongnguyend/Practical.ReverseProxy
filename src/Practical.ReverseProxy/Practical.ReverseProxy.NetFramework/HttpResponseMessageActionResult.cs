using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Practical.ReverseProxy.ReverseProxy.NetFramework
{
    public class HttpResponseMessageActionResult : IHttpActionResult
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        private readonly IEnumerable<string> _headers;

        public HttpResponseMessageActionResult(HttpResponseMessage httpResponseMessage, IEnumerable<string> headers = null)
        {
            _httpResponseMessage = httpResponseMessage;
            _headers = headers;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_headers == null || !_headers.Any())
            {
                _httpResponseMessage.Headers.Clear();
            }
            else
            {
                _httpResponseMessage.Headers.Remove("transfer-encoding");
                foreach (var header in _httpResponseMessage.Headers)
                {
                    if (!_headers.Contains(header.Key))
                    {
                        _httpResponseMessage.Headers.Remove(header.Key);
                    }
                }
            }
            return Task.FromResult(_httpResponseMessage);
        }
    }
}