using Digitteck.Gateway.Service.Common.DataValidator;
using System.Linq;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    public sealed class ValidationWaitForShouldContainValidTags : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => false;

        public override ValidationMessage Validate(GatewayConfiguration notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            foreach(RouteDefinition route in notice.RouteModels)
            {
                if (route.Downstream == null)
                {
                    //null is handled elsewhere
                    continue;
                }

                if (route.Downstream.RunAsync)
                {
                    string[] allTagNames = route.Downstream.Operations.Select(x => x.OperationTag).ToArray();

                    foreach(OperationCore op in route.Downstream.Operations)
                    {
                        if (op is OperationCall opCall)
                        {
                            if (opCall.WaitFor is null)
                            {
                                continue;
                            }

                            //waiting for him self - no no no
                            if (opCall.WaitFor.Contains(opCall.OperationTag))
                            {
                                message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' " +
                                    $"has in the {nameof(OperationCall.WaitFor)} a reference to himself. A task cannot wait for itself :)"));
                            }

                            //waitFor defines what operation should it wait for, and it has to be a valid tag
                            foreach (string waitFor in opCall.WaitFor)
                            {
                                //waiting for somebody real
                                if (!allTagNames.Contains(waitFor))
                                {
                                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opCall.OperationTag}\' " +
                                    $"has in the {nameof(OperationCall.WaitFor)} a reference to operation tag \'{waitFor}\'. This tag does not exist."));
                                }
                            }
                        }

                        if (op is OperationAggregateResponse opAgg)
                        {
                            if (opAgg.WaitFor is null)
                            {
                                continue;
                            }

                            //waiting for him self - no no no
                            if (opAgg.WaitFor.Contains(opAgg.OperationTag))
                            {
                                message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opAgg.OperationTag}\' " +
                                    $"has in the {nameof(OperationCall.WaitFor)} a reference to himself. A task cannot wait for itself :)"));
                            }

                            //waitFor defines what operation should it wait for, and it has to be a valid tag
                            foreach (string waitFor in opAgg.WaitFor)
                            {
                                //waiting for somebody real
                                if (!allTagNames.Contains(waitFor))
                                {
                                    message.Add(ValidationMessage.Fail($"At route : <entry-point>:\'{route?.Upstream?.EntryTemplate ?? string.Empty}\' <operation-tag>:\'{opAgg.OperationTag}\' " +
                                    $"has in the {nameof(OperationCall.WaitFor)} a reference to operation tag \'{waitFor}\'. This tag does not exist."));
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