using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public  sealed class JSDirectiveUseRetryPolicyMap : DataMap<JSDirectiveUseRetryPolicy, DirectiveUseRetryPolicy>
    {
        public override DirectiveUseRetryPolicy Map(JSDirectiveUseRetryPolicy source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new DirectiveUseRetryPolicy
                {
                    RetryCount = source.RetryCount,
                    TimeBetweenRetries = source.TimeBetweenRetries
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
