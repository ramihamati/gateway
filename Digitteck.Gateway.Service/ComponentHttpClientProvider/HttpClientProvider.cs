using System.Net.Http;

namespace Digitteck.Gateway.Service
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClient GetClient(HttpMessageHandler httpMessageHandler = null)
        {
            return new HttpClient(httpMessageHandler);
        }
    }
}
