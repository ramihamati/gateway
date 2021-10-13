using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationOperationContentTests
    {
        [Test]
        public void validate_when_operations_are_ok()
        {
            GatewayConfiguration config = Fixture.GetConfig();

            ValidationOperationContent validation = new ValidationOperationContent();
            ValidationMessage message = validation.Validate(config);
            Assert.IsTrue(message.IsValid);
        }

        [Test]
        public void validate_when_there_is_no_call_or_aggregate()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.Operations = new List<OperationCore>
            {
                new OperationReturn()
            };

            ValidationOperationContent validation = new ValidationOperationContent();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_there_is_no_return()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.Operations = new List<OperationCore>
            {
                new OperationCall()
            };

            ValidationOperationContent validation = new ValidationOperationContent();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }
    }

    public sealed class ValidationNullChecksTests
    {
        [Test]
        public void validate_when_route_models_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_a_route_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels = new List<RouteDefinition> { null };

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_route_model_upstream_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Upstream = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_route_model_upstream_entrytemplate_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Upstream.EntryTemplate = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_route_model_downstream_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_route_model_downstream_operations_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.Operations = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_a_route_has_a_null_operation()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.Operations[0] = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_call_has_null_path()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var opCall = (OperationCall)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationCall);
            opCall.PathAndQuery = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_call_has_null_scheme()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var opCall = (OperationCall)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationCall);
            opCall.Scheme = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_call_has_null_serverhost()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var opCall = (OperationCall)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationCall);
            opCall.ServerHost = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_call_has_null_serverport()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var opCall = (OperationCall)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationCall);
            opCall.ServerPort = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_aggregate_has_null_executor()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.Executor = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_an_operation_return_has_null_return_tag()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationReturn)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationReturn);
            operation.ReturnTag = null;

            ValidationNullChecks validation = new ValidationNullChecks();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles