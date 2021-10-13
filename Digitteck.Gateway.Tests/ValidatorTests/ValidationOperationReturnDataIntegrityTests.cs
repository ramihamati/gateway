using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationOperationReturnDataIntegrityTests
    {
        [Test]
        public void validate_when_return_tag_is_null()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationReturn)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationReturn);
            operation.ReturnTag = null;

            ValidationOperationReturnDataIntegrity validation = new ValidationOperationReturnDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_return_tag_is_empty()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            var operation = (OperationReturn)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationReturn);
            operation.ReturnTag = string.Empty;

            ValidationOperationReturnDataIntegrity validation = new ValidationOperationReturnDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_return_tag_points_to_not_existing_tag()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            OperationReturn returnOp = (OperationReturn)config.RouteModels[0].Downstream.Operations.Find(x => x is OperationReturn);
            //find operation with tag
            OperationCore op = config.RouteModels[0].Downstream.Operations.Find(x => x.OperationTag == returnOp.ReturnTag);

            Assert.IsNotNull(op);

            returnOp.ReturnTag = "somethingelese";

            ValidationOperationReturnDataIntegrity validation = new ValidationOperationReturnDataIntegrity();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles