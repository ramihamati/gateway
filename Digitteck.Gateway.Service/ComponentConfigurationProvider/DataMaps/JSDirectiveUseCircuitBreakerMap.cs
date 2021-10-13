using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSDirectiveUseCircuitBreakerMap : DataMap<JSDirectiveUseCircuitBreaker, DirectiveUseCircuitBreaker>
    {
        public override DirectiveUseCircuitBreaker Map(JSDirectiveUseCircuitBreaker source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new DirectiveUseCircuitBreaker
                {
                    AttemptsToOpenCircuit = source.AttemptsToOpenCircuit,
                    FailoutResponseExecutor = source.FailoutResponseExecutor,
                    TimeoutValue = source.TimeoutValue
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
