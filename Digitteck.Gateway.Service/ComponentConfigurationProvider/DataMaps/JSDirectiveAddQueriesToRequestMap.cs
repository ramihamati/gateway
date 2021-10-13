using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSDirectiveAddQueriesToRequestMap : DataMap<JSDirectiveAddQueriesToRequest, DirectiveAddQueriesToRequest>
    {
        public override DirectiveAddQueriesToRequest Map(JSDirectiveAddQueriesToRequest source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new DirectiveAddQueriesToRequest
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
