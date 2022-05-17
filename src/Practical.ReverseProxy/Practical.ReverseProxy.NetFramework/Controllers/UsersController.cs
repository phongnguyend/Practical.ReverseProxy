using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Practical.ReverseProxy.ReverseProxy.NetFramework.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private static HttpClient _httpClient = new HttpClient();

        private HttpRequestMessage CloneRequest()
        {
            var context = ActionContext;

            var request = new HttpRequestMessage
            {
                Method = context.Request.Method,
            };

            foreach (var header in context.Request.Headers)
            {
                if (header.Key == "Host")
                {
                    request.Headers.Add(header.Key, "localhost:44352");
                    continue;
                }

                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }

        private HttpResponseMessageActionResult ForwardResponse(HttpResponseMessage httpResponseMessage)
        {
            return new HttpResponseMessageActionResult(httpResponseMessage);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var request = CloneRequest();
            request.RequestUri = new Uri("https://localhost:44352/api/users");
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(object model)
        {
            var request = CloneRequest();
            request.RequestUri = new Uri("https://localhost:44352/api/users");
            request.Content = model.AsJsonContent();
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(string id, object model)
        {
            var request = CloneRequest();
            request.RequestUri = new Uri($"https://localhost:44352/api/users/{id}");
            request.Content = model.AsJsonContent();
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var request = CloneRequest();
            request.RequestUri = new Uri($"https://localhost:44352/api/users/{id}");
            var response = await _httpClient.SendAsync(request);
            return ForwardResponse(response);
        }
    }
}
