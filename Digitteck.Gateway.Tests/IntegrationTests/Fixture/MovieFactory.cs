using Microsoft.AspNetCore.Mvc.Testing;
using MovieStartup = Digitteck.Gateway.TestApi.Movies.Startup;

namespace IntegrationTests
{
    public class MovieFactory : WebApplicationFactory<MovieStartup>
    {
        
    }
}
