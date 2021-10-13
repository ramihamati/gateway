
using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Attributes;

namespace Digitteck.Gateway.Service
{
    [DirectiveName("UseCircuitBreaker")]
    public class DirectiveUseCircuitBreaker : DirectiveCore
    {
        public int? AttemptsToOpenCircuit { get; set; }

        public int? TimeoutValue { get; set; }

        public string FailoutResponseExecutor { get; set; }

        public DirectiveUseCircuitBreaker() 
        {

        }
    }

}
