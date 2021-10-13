using Digitteck.Gateway.Service;
using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;
using NUnit.Framework;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Naming Styles

namespace ValidatorTests
{
    public sealed class ValidationDuplicateQueryEntriesTests
    {
        [Test]

        public void validate_when_there_are_duplicates()
        {
            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Upstream.EntryTemplate = new TemplatePathAndQueryObject("/api");
            config.RouteModels[1].Upstream.EntryTemplate = new TemplatePathAndQueryObject("/api");

            ValidationDuplicateQueryEntries validation = new ValidationDuplicateQueryEntries();
            ValidationMessage message = validation.Validate(config);
            Assert.IsFalse(message.IsValid);
        }

        [Test]
        public void validate_when_there_are_no_duplicates()
        {

            GatewayConfiguration config = Fixture.GetConfig();
            config.RouteModels[0].Upstream.EntryTemplate = new TemplatePathAndQueryObject("/api1");
            config.RouteModels[1].Upstream.EntryTemplate = new TemplatePathAndQueryObject("/api2");

            ValidationDuplicateQueryEntries validation = new ValidationDuplicateQueryEntries();

            ValidationMessage message = validation.Validate(config);

            Assert.IsTrue(message.IsValid);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles