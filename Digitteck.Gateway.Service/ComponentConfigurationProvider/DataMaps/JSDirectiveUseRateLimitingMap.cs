using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSDirectiveUseRateLimitingMap : DataMap<JSDirectiveUseRateLimiting, DirectiveUseRateLimiting>
    {
        public override DirectiveUseRateLimiting Map(JSDirectiveUseRateLimiting source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new DirectiveUseRateLimiting
                {
                    ClientWhitelist = source.ClientWhitelist,
                    Limit = source.Limit,
                    Period = source.Period,
                    PeriodTimespan = source.PeriodTimespan
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
