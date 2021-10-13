using System.Collections.Generic;
using System.Linq;
using Digitteck.Gateway.Service.Common.DataValidator;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    public sealed class ValidationOperationReturnDataIntegrity : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails =>false;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            //the return op must have a return tag, and that return tag must be found in other ops
            foreach (RouteDefinition route in notice.RouteModels)
            {
                OperationReturn returnOp = route.Downstream.Operations.Find(x => x is OperationReturn) as OperationReturn;

                if (returnOp == null)
                {
                    //handled elsewhere
                    return message;
                }

                string returnTag = returnOp.ReturnTag;

                if (returnTag == null)
                {
                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{returnOp.OperationTag}\' the {nameof(OperationReturn.ReturnTag)} is null"));
                    continue;
                }

                if (returnTag.Length == 0)
                {
                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{returnOp.OperationTag}\' the {nameof(OperationReturn.ReturnTag)} is empty"));
                }

                //nulls are handles elsewhere, but we need not to have exceptions
                int matchingTag = route.Downstream?.Operations?.Count(x => x?.OperationTag == returnTag) ?? 1;

                if (matchingTag!= 1)
                {
                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{returnOp.OperationTag}\' the {nameof(OperationReturn.ReturnTag)} " +
                        $"with value \'{returnTag}\' was not found in another operation"));
                }
            }

            return message;
        }
    }
}