using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidateFixtureAsBaseForAllTests
    {
        [Test]
        public void validate_fixture_should_be_ok()
        {
            ConfigurationValidator validator = new ConfigurationValidator();
            GatewayConfiguration config = Fixture.GetConfig();

            ValidationMessage message = validator.Validate(config);
            Assert.IsTrue(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles