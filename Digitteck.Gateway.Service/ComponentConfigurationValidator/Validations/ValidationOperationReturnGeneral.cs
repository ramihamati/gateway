using System.Collections.Generic;
using System.Linq;
using Digitteck.Gateway.Service.Common.DataValidator;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    public sealed class ValidationOperationReturnGeneral : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => false;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            foreach (RouteDefinition route in notice.RouteModels)
            {
                List<OperationCore> routeOperations = route.Downstream.Operations;

                if (routeOperations.Count == 0)
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point {route.Upstream?.EntryTemplate ?? string.Empty} has an operation list with no entries."));
                }

                int nrOfRetuns = routeOperations.Count(x => x is OperationReturn);
                if (nrOfRetuns != 1)
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point {route.Upstream?.EntryTemplate ?? string.Empty} has {nrOfRetuns} returns of type {typeof(OperationReturn).FullName}. There must be only one defined."));
                }

                if (nrOfRetuns > 0 && !(routeOperations[routeOperations.Count - 1] is OperationReturn))
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point {route.Upstream?.EntryTemplate ?? string.Empty} has a return operation but it's not at the end of the operation group. Please place it at the end"));
                }
            }

            return message;
        }
    }
}