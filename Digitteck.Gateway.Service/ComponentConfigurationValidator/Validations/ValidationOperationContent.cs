using Digitteck.Gateway.Service.Common.DataValidator;
using System.Linq;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    public sealed class ValidationOperationContent : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => false;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            foreach (RouteDefinition route in notice.RouteModels)
            {
                int countOpCalls = route?.Downstream?.Operations?.Count(x => x is OperationCall) ?? 0;
                int countOpAggregates = route?.Downstream?.Operations?.Count(x => x is OperationAggregateResponse) ?? 0;
                int countOpReturns = route?.Downstream?.Operations?.Count(x => x is OperationReturn) ?? 0;

                if (countOpCalls == 0 && countOpAggregates == 0)
                {
                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' there is no operation of type {typeof(OperationCall).Name} or {typeof(OperationAggregateResponse).Name} defined"));
                }

                if (countOpReturns != 1)
                {
                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' there has to be exactly one {typeof(OperationReturn).Name} defined"));
                }
            }

            return message;
        }
    }
}