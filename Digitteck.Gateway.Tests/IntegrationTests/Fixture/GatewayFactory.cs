using Digitteck.Gateway.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using GateStartup = Digitteck.Gateway.TestApi.Api.Startup;

namespace IntegrationTests
{
    public class GatewayFactory : WebApplicationFactory<GateStartup> 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //replace the client provider to get the client of the movie Factory 
                services.AddSingleton<IHttpClientProvider, FakeClientProvider>();
            });
        }
    }
}
