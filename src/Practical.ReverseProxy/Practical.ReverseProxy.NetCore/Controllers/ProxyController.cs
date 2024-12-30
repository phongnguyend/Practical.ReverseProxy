using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practical.ReverseProxy.NetCore.Controllers;

public class ProxyController : Controller
{
    private const int StreamCopyBufferSize = 81920;

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
            request.Body.Seek(0, SeekOrigin.Begin);
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

    private async Task ForwardResponse(HttpResponseMessage httpResponseMessage)
    {
        var response = HttpContext.Response;

        response.StatusCode = (int)httpResponseMessage.StatusCode;
        foreach (var header in httpResponseMessage.Headers)
        {
            response.Headers[header.Key] = header.Value.ToArray();
        }

        foreach (var header in httpResponseMessage.Content.Headers)
        {
            response.Headers[header.Key] = header.Value.ToArray();
        }

        // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
        response.Headers.Remove("transfer-encoding");

        using (var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync())
        {
            await responseStream.CopyToAsync(response.Body, StreamCopyBufferSize, HttpContext.RequestAborted);
        }
    }

    protected async Task Send(string url)
    {
        var request = CloneRequest(new Uri(url));
        var response = await _httpClient.SendAsync(request);
        await ForwardResponse(response);
    }
}
