using Digitteck.Gateway.Service;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using MovieStartup = Digitteck.Gateway.TestApi.Movies.Startup;

namespace IntegrationTests
{
    public class FakeClientProvider : IHttpClientProvider
    {
        private WebApplicationFactory<MovieStartup> webApplicationFactory;

        public FakeClientProvider()
        {
            webApplicationFactory = new WebApplicationFactory<MovieStartup>();
        }

        public HttpClient GetClient(HttpMessageHandler httpMessageHandler = null)
        {
            return webApplicationFactory.CreateClient();
        }
    }
}
