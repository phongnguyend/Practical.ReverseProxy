using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.ReverseProxy.NetCore
{
    public class HttpResponseMessageActionResult : IActionResult
    {
        private const int StreamCopyBufferSize = 81920;

        private readonly HttpResponseMessage _httpResponseMessage;

        public HttpResponseMessageActionResult(HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.StatusCode = (int)_httpResponseMessage.StatusCode;
            foreach (var header in _httpResponseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in _httpResponseMessage.Content.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            using (var responseStream = await _httpResponseMessage.Content.ReadAsStreamAsync())
            {
                await responseStream.CopyToAsync(response.Body, StreamCopyBufferSize, context.HttpContext.RequestAborted);
            }

        }
    }
}