using System.Net.Http;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The client provider has been defined to be able to replace the client in integration tests
    /// </summary>
    public interface IHttpClientProvider
    {
        HttpClient GetClient(HttpMessageHandler httpMessageHandler = null);
    }
}