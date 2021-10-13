using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using uriHelper = Microsoft.AspNetCore.Http.Extensions.UriHelper;
//using queryBuilder = Microsoft.AspNetCore.Http.Extensions.QueryBuilder;
//using streamCopyOps = Microsoft.AspNetCore.Http.Extensions.StreamCopyOperation;

namespace Digitteck.Gateway.Service
{
    public sealed class UriHelperService : IUriHelperService
    {

        /// <summary>
        /// get elements from queryObject2 that are not in 1 and creates a new object
        /// </summary>
        public QueryObject Union(QueryObject queryObject1, QueryObject queryObject2)
        {
            List<QueryPart> parts = new List<QueryPart>();

            foreach(var part in queryObject1.QueryParts)
            {
                parts.Add(part.Clone());
            }

            foreach(var part in queryObject2.QueryParts)
            {
                if (!parts.Any(x=> string.Equals(x.QueryKey, part.QueryKey, System.StringComparison.OrdinalIgnoreCase)))
                {
                    parts.Add(part.Clone());
                }
            }

            return QueryObject.FromParts(parts);
        }

        /// <summary>
        /// returns a relative path with querystring like : "/api/Movies?Rating=9.7"
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathAndQueryObject GetFullRelativePath(HttpRequest request)
        {
            //relative path including querystring and starting with "/"
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.extensions.urihelper.getencodedpathandquery?view=aspnetcore-3.0
            string relUri =  uriHelper.GetEncodedPathAndQuery(request);

            return new PathAndQueryObject(relUri);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string EnsureStartingPathDelimiter(string entry)
        {
            if (!entry.StartsWith(GlobalEnv.PathDelimiter))
            {
                return $"{GlobalEnv.PathDelimiter}{entry}";
            }

            return entry;
        }
    }
}
