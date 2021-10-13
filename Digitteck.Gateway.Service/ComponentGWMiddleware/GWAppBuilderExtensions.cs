using Microsoft.AspNetCore.Builder;

namespace Digitteck.Gateway.Service
{
    public static class GWAppBuilderExtensions
    {
        public static void UseGW(this IApplicationBuilder app)
        {
            app.UseMiddleware<GatewayMiddleware>();
        }
    }
}
