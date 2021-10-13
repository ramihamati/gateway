using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationOperationAggregatesDataIntegrityTests
    {
        [Test]
        public void validate_when_aggregate_op_executor_is_ok()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);

            ValidationOperationAggregatesDataIntegrity validation = new ValidationOperationAggregatesDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsTrue(message.IsValid);
        }

        [Test]
        public void validate_when_aggregate_op_executor_is_empty()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.Executor = string.Empty;

            ValidationOperationAggregatesDataIntegrity validation = new ValidationOperationAggregatesDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_aggregate_op_executor_is_does_not_have_right_format()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.Executor = "assemblyonly";

            ValidationOperationAggregatesDataIntegrity validation = new ValidationOperationAggregatesDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_the_type_is_wrong()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.Executor = operation.Executor + "1";

            ValidationOperationAggregatesDataIntegrity validation = new ValidationOperationAggregatesDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles