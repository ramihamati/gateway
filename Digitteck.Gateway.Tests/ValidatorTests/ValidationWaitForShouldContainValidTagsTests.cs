using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationWaitForShouldContainValidTagsTests
    {
        [Test]
        public void validate_when_wait_list_is_ok()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.RunAsync = true;

            ValidationWaitForShouldContainValidTags validation = new ValidationWaitForShouldContainValidTags();
            ValidationMessage message = validation.Validate(config);
            Assert.IsTrue(message.IsValid);
        }

        [Test]
        public void validate_when_waiting_for_itself()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.RunAsync = true;
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.WaitFor.Add(operation.OperationTag);

            ValidationWaitForShouldContainValidTags validation = new ValidationWaitForShouldContainValidTags();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_waiting_for_an_inexisting_tag()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Downstream.RunAsync = true;
            var operation = (OperationAggregateResponse)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationAggregateResponse);
            operation.WaitFor.Add("inexisting");

            ValidationWaitForShouldContainValidTags validation = new ValidationWaitForShouldContainValidTags();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles