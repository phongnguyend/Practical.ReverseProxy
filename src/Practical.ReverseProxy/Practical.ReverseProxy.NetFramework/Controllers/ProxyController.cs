using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Practical.ReverseProxy.NetFramework.Controllers
{
    public class ProxyController : ApiController
    {
        private const int StreamCopyBufferSize = 81920;

        private static HttpClient _httpClient = new HttpClient();

        private async Task<HttpRequestMessage> CloneRequestAsync(Uri uri)
        {
            var request = Request;

            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (requestMethod != HttpMethod.Get &&
                requestMethod != HttpMethod.Head &&
                requestMethod != HttpMethod.Delete &&
                requestMethod != HttpMethod.Trace)
            {
                requestMessage.Content = new StreamContent(await Request.Content.ReadAsStreamAsync());

                foreach (var header in request.Content.Headers)
                {
                    requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            foreach (var header in request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = request.Method;

            return requestMessage;
        }

        private async Task<HttpResponseMessage> ForwardResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            var response = Request.CreateResponse(httpResponseMessage.StatusCode);
            response.Content = new StreamContent(await httpResponseMessage.Content.ReadAsStreamAsync());

            foreach (var header in httpResponseMessage.Headers)
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            foreach (var header in httpResponseMessage.Content.Headers)
            {
                response.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            return response;
        }

        protected async Task<HttpResponseMessage> SendAsync(string url)
        {
            var request = await CloneRequestAsync(new Uri(url));
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await ForwardResponseAsync(response);
        }
    }
}
