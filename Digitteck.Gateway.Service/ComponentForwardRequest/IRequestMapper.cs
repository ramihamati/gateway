using System.Net.Http;
using System.Threading.Tasks;
using Digitteck.Gateway.Service.SpecializedModels;
using Microsoft.AspNetCore.Http;

namespace Digitteck.Gateway.Service
{
    public interface IRequestMapper
    {
        Task<HttpRequestMessage> MapAsync(HttpRequest request, HttpMethodType httpMethodType, AbsoluteUriObject uriObject);
    }
}