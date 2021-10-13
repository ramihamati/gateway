using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Digitteck.Gateway.Service
{
    public interface IGWRuntime
    {
        Task<OperationResult> Execute(HttpContext gwHttpContext, GatewayConfiguration configuration);
    }
}