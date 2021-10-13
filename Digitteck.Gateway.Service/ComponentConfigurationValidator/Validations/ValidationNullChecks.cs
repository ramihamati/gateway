using System.Collections.Generic;
using System.Linq;
using Digitteck.Gateway.Service.Common.DataValidator;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    /// <summary>
    /// check for nulls
    /// </summary>
    public sealed class ValidationNullChecks : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => true;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            if (notice.RouteModels == null)
            {
                message.Add(ValidationMessage.Fail($"The gateway configuration has no route models. "));
                return message;
            }

            foreach (RouteDefinition route in notice.RouteModels)
            {
                if (route == null)
                {
                    message.Add(ValidationMessage.Fail($"Detected a route that is null"));
                    continue;
                }

                if (route.Upstream == null)
                {
                    message.Add(ValidationMessage.Fail($"Detected a route with no upstream"));
                }

                if (route.Upstream?.EntryTemplate == null)
                {
                    message.Add(ValidationMessage.Fail($"Detected a route with no upstream entry template"));
                }

                if (route.Downstream == null)
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point \'{route?.Upstream?.EntryTemplate ?? string.Empty}\' does not have the downstream defined."));
                }

                if (route.Downstream?.Operations == null)
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point \'{route?.Upstream?.EntryTemplate ?? string.Empty}\' does not have the downstream operations defined."));
                }
                //is there a null operation
                if (route.Downstream?.Operations?.Any(x => x == null) ?? true)
                {
                    message.Add(ValidationMessage.Fail($"The route with entry point \'{route?.Upstream?.EntryTemplate ?? string.Empty}\' contains a null operation"));
                }
                //is there an operaetion with a null operation tag
                foreach (OperationCore op in route.Downstream?.Operations ?? new List<OperationCore>())
                {
                    if (op?.OperationTag == null)
                    {
                        message.Add(ValidationMessage.Fail($"The route with entry point \'{route?.Upstream?.EntryTemplate ?? string.Empty}\' contains a null operation"));
                    }
                }
                //operation call - null path
                foreach (OperationCore op in route.Downstream?.Operations ?? new List<OperationCore>())
                {
                    if (op is OperationCall opcall)
                    {
                        if (opcall.PathAndQuery == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opcall.OperationTag}\' the {nameof(opcall.PathAndQuery)} is null"));
                        }
                        if (opcall.Scheme == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opcall.OperationTag}\' the {nameof(opcall.Scheme)} is null"));
                        }
                        if (opcall.ServerHost == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opcall.OperationTag}\' the {nameof(opcall.ServerHost)} is null"));
                        }
                        if (opcall.ServerPort == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opcall.OperationTag}\' the {nameof(opcall.ServerPort)} is null"));
                        }
                    }

                    if (op is OperationAggregateResponse aggregateResponse)
                    {
                        if (aggregateResponse.Executor == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{aggregateResponse.OperationTag}\' the {nameof(aggregateResponse.Executor)} is null"));
                        }
                    }

                    if (op is OperationReturn operationReturn)
                    {
                        if (operationReturn.ReturnTag == null)
                        {
                            message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{operationReturn.OperationTag}\' the {nameof(operationReturn.ReturnTag)} is null"));
                        }

                    }
                }
            }

            return message;
        }
    }
}