using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.Common.Helpers;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    public sealed class ValidationOperationAggregatesDataIntegrity : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => false;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            //null checks are not handled here
            //all value objects must have value
            foreach (RouteDefinition route in notice.RouteModels)
            {
                foreach (OperationCore op in route.Downstream.Operations)
                {
                    if (op is OperationAggregateResponse opCall)
                    {
                        if (opCall.Executor is null)
                        {
                            //nulls not handled here
                            continue;
                        }

                        if (opCall.Executor.Trim() == string.Empty)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' the {nameof(OperationAggregateResponse.Executor)} is empty"));
                        }

                        //the executor must provide the assembly and FQCN sepparated by comma : "Assembly, SomeNamespace.SomeClass"
                        if (!opCall.Executor.Contains(','))
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' the {nameof(OperationAggregateResponse.Executor)} :" +
                                $" the executor must provide the assembly and FQCN sepparated by comma : \"Assembly, SomeNamespace.SomeClass\""));
                        }
                        else
                        {
                            string[] parts = opCall.Executor.Split(',');
                            if (parts.Length != 2)
                            {
                                message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' the {nameof(OperationAggregateResponse.Executor)} :" +
                                        $" the executor must provide the assembly and FQCN sepparated by comma : \"Assembly, SomeNamespace.SomeClass\""));
                            }
                            else
                            {
                                if (AssemblyHelper.FindType(parts[0], parts[1]) == null)
                                {
                                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' the {nameof(OperationAggregateResponse.Executor)} :" +
                                       $" defined a type \'{opCall.Executor}\' that does not exist"));
                                }
                            }
                        }
                    }
                }
            }

            return message;
        }
    }
}