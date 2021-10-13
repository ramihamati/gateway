using Digitteck.Gateway.Service.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public interface ITemplateToRequestMatchService
    {
        bool IsMatch(HttpRequest request, UpstreamTemplate template);

        RouteContext FindMatch(HttpRequest httpRequest, List<RouteContext> contexts);
    }
}
