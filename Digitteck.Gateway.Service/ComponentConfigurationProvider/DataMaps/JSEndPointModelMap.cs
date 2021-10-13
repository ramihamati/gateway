using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSEndPointModelMap : DataMap<JSEndPointModel, Upstream>
    {
        public override Upstream Map(JSEndPointModel source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new Upstream
                {
                    EntryTemplate = new TemplatePathAndQueryObject(source.EntryTemplate),
                    HttpMethod = provider.Map<JSHttpMethodType, HttpMethodType>(source.HttpMethod)
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
