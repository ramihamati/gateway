using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSDirectiveAddHeadersToRequestMap : DataMap<JSDirectiveAddHeadersToRequest, DirectiveAddHeadersToRequest>
    {
        public override DirectiveAddHeadersToRequest Map(JSDirectiveAddHeadersToRequest source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new DirectiveAddHeadersToRequest
                {
                    Arguments = source.Arguments,
                    Executor = source.Executor
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
