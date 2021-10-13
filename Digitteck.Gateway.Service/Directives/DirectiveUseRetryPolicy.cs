using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Attributes;

namespace Digitteck.Gateway.Service
{
    [DirectiveName("UseRetryPolicy")]
    public class DirectiveUseRetryPolicy : DirectiveCore
    {
        public int? RetryCount { get; set; }

        public int? TimeBetweenRetries { get; set; }
    }
}
