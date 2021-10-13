using Microsoft.AspNetCore.Http;

namespace Digitteck.Gateway.Service
{
    public interface IUriHelperService
    {
        PathAndQueryObject GetFullRelativePath(HttpRequest request);
        string EnsureStartingPathDelimiter(string entry);
        QueryObject Union(QueryObject queryObject1, QueryObject queryObject2);
    }
}