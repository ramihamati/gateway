using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.Service.SpecializedModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    //With help of ocelot : with several changes:
    /* The request mapper will get the address from the uriObject
     * The request mapper will get the method from the httpMethodType
     * 
     */
    public sealed class RequestMapper : IRequestMapper
    {
        private readonly string[] _unsupportedHeaders = { "host" };

        public async Task<HttpRequestMessage> MapAsync(HttpRequest request, HttpMethodType httpMethodType, AbsoluteUriObject uriObject)
        {
            try
            {
                var requestMessage = new HttpRequestMessage()
                {
                    Content = await MapContent(request).ConfigureAwait(false),
                    Method = MapMethod(httpMethodType),
                    RequestUri = new Uri(uriObject.Value)
                };

                MapHeaders(request, requestMessage);

                return requestMessage;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.UnmapableRequest, ex.Message);
            }
        }

        private async Task<HttpContent> MapContent(HttpRequest request)
        {
            if (request.Body == null || (request.Body.CanSeek && request.Body.Length <= 0))
            {
                return null;
            }

            // Never change this to StreamContent again, I forgot it doesnt work in #464.
            var content = new ByteArrayContent(await ToByteArray(request.Body));

            if (!string.IsNullOrEmpty(request.ContentType))
            {
                content.Headers
                    .TryAddWithoutValidation("Content-Type", new[] { request.ContentType });
            }

            AddHeaderIfExistsOnRequest("Content-Language", content, request);
            AddHeaderIfExistsOnRequest("Content-Location", content, request);
            AddHeaderIfExistsOnRequest("Content-Range", content, request);
            AddHeaderIfExistsOnRequest("Content-MD5", content, request);
            AddHeaderIfExistsOnRequest("Content-Disposition", content, request);
            AddHeaderIfExistsOnRequest("Content-Encoding", content, request);

            return content;
        }

        private void AddHeaderIfExistsOnRequest(string key, HttpContent content, HttpRequest request)
        {
            if (request.Headers.ContainsKey(key))
            {
                content.Headers
                    .TryAddWithoutValidation(key, request.Headers[key].ToList());
            }
        }

        private HttpMethod MapMethod(HttpMethodType httpMethodType)
        {
            return httpMethodType switch
            {
                HttpMethodType.Connect => new HttpMethod(HttpMethods.Connect),
                HttpMethodType.Put => new HttpMethod(HttpMethods.Put),
                HttpMethodType.Post => new HttpMethod(HttpMethods.Post),
                HttpMethodType.Patch => new HttpMethod(HttpMethods.Patch),
                HttpMethodType.Trace => new HttpMethod(HttpMethods.Trace),
                HttpMethodType.Head=> new HttpMethod(HttpMethods.Head),
                HttpMethodType.Get=> new HttpMethod(HttpMethods.Get),
                HttpMethodType.Delete=> new HttpMethod(HttpMethods.Delete),
                HttpMethodType.Options => new HttpMethod(HttpMethods.Options),
                _ => throw new GatewayException(ErrorCode.UnmappableMethodType, $"Cannot map from {httpMethodType}")
            };
        }


        private void MapHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (KeyValuePair<string, StringValues> header in request.Headers)
            {
                if (IsSupportedHeader(header))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }

        private bool IsSupportedHeader(KeyValuePair<string, StringValues> header)
        {
            return !_unsupportedHeaders.Contains(header.Key.ToLower());
        }

        private async Task<byte[]> ToByteArray(Stream stream)
        {
            using (stream)
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    return memStream.ToArray();
                }
            }
        }
    }
}
