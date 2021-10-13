using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.Service.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public class TemplateToRequestMatchService : ITemplateToRequestMatchService
    {
        private readonly IUriHelperService _uriHelperService;

        public TemplateToRequestMatchService(IUriHelperService uriHelperService)
        {
            _uriHelperService = uriHelperService;
        }

        public RouteContext FindMatch(HttpRequest httpRequest, List<RouteContext> contexts)
        {
            foreach (RouteContext routeContext in contexts)
            {
                if (IsMatch(httpRequest, routeContext.UpstreamTemplate))
                {
                    return routeContext;
                }
            }

            throw new GatewayException(ErrorCode.RouteMatching, $"Could not find a match for the request {httpRequest.Path}");
        }

        public bool IsMatch(HttpRequest request, UpstreamTemplate template)
        {
            PathAndQueryObject requestPathAndQuery = _uriHelperService.GetFullRelativePath(request);
            TemplatePathAndQueryObject templatePathAndQuery = template.EntryTemplate;

            PathObject requestPath = requestPathAndQuery.GetPathObject();
            PathObject templatePath = templatePathAndQuery.GetPathObject();

            string requestMethod = request.Method;

            if (!requestPathAndQuery.HasValue)
            {
                throw new GatewayException(ErrorCode.RouteMatching, $"Could not get the relative path ffrom the request");
            }
            //mathing is made only against the path. The query is dynamic and is not a path representative

            //bool m1 = Regex.IsMatch(requestPathAndQuery.Value, template.RegexTemplate.TemplateOne);
            //bool m2 = Regex.IsMatch(requestPathAndQuery.Value, template.RegexTemplate.TemplateTwo);
            bool m1 = IsMatch(requestPath.Value, templatePath.Value);
            bool m3 = HttpMethodTypeToString(template.HttpMethodType) == requestMethod.ToUpper();
            
            //return (m1 || m2) && m3;
            return m1 && m3;
        }

        private string HttpMethodTypeToString(HttpMethodType httpMethodType)
        {
            return httpMethodType switch
            {
                HttpMethodType.Get  => "GET",
                HttpMethodType.Delete => "DELETE",
                HttpMethodType.Post => "POST",
                HttpMethodType.Put => "PUT",
                HttpMethodType.Connect => "CONNECT",
                HttpMethodType.Head => "HEAD",
                HttpMethodType.Options => "OPTIONS",
                HttpMethodType.Patch => "PATCH",
                HttpMethodType.Trace => "TRACE",
                _ => throw new GatewayException(ErrorCode.RouteMatching, $"the type {httpMethodType} is not supported")
            };
        }

        public static bool IsMatch(string s1, string s2)
        {
            if (s1.StartsWith('/'))
            {
                s1 = s1.Remove(0, 1);
            }
            if (s1.EndsWith('/'))
            {
                s1 = s1.Remove(s1.Length - 1, 1);
            }
            if (s2.StartsWith('/'))
            {
                s2 = s2.Remove(0, 1);
            }
            if (s2.EndsWith('/'))
            {
                s2 = s2.Remove(s2.Length - 1, 1);
            }

            string[] s1parts = s1.Split('/');
            string[] s2parts = s2.Split('/');

            for (int i = 0; i < s1parts.Length && i < s2parts.Length; i++)
            {
                if (IsPlaceholder(s1parts[i]) || IsPlaceholder(s2parts[i]))
                {
                    continue;
                }
                if (IsRestOf(s1parts[i]) || IsRestOf(s2parts[i]))
                {
                    return true;
                }

                if (!string.Equals(s1parts[i], s2parts[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            //if we reached here and the parts are not equal, not '*' was found meaning the match
            //did not fill the rest of a sequence. Only if the parts are equal then there was no need to match
            //the rest
            if (s1parts.Length == s2parts.Length)
            {
                return true;
            }

            return false;
        }

        public static bool IsPlaceholder(string s)
        {
            return s.StartsWith('{') && s.EndsWith('}');
        }

        public static bool IsRestOf(string s)
        {
            return s == "*";
        }
    }
}
