using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSHttpMethodTypeMap : DataMap<JSHttpMethodType, HttpMethodType>
    {
        public override HttpMethodType Map(JSHttpMethodType source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(provider, nameof(provider));

                return source switch
                {
                    JSHttpMethodType.Connect => HttpMethodType.Connect,
                    JSHttpMethodType.Put => HttpMethodType.Put,
                    JSHttpMethodType.Post => HttpMethodType.Post,
                    JSHttpMethodType.Patch => HttpMethodType.Patch,
                    JSHttpMethodType.Trace => HttpMethodType.Trace,
                    JSHttpMethodType.Head => HttpMethodType.Head,
                    JSHttpMethodType.Get => HttpMethodType.Get,
                    JSHttpMethodType.Delete => HttpMethodType.Delete,
                    JSHttpMethodType.Options => HttpMethodType.Options,

                    _ => throw new Exception("Not supported")
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
