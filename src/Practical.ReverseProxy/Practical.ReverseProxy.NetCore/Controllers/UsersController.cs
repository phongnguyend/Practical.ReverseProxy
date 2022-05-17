using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.ReverseProxy.ReverseProxy.NetCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static HttpClient _httpClient = new HttpClient();

        private HttpRequestMessage CloneRequest(Uri uri)
        {
            var request = HttpContext.Request;

            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            // Copy the request headers
            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }

        private HttpResponseMessageActionResult ForwardResponse(HttpResponseMessage httpResponseMessage)
        {
            return new HttpResponseMessageActionResult(httpResponseMessage);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = CloneRequest(new Uri("https://localhost:44352/api/users"));
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(object model)
        {
            var request = CloneRequest(new Uri("https://localhost:44352/api/users"));
            request.Content = model.AsJsonContent();
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, object model)
        {
            var request = CloneRequest(new Uri($"https://localhost:44352/api/users/{id}"));
            request.Content = model.AsJsonContent();
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var request = CloneRequest(new Uri($"https://localhost:44352/api/users/{id}"));
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }
    }
}
