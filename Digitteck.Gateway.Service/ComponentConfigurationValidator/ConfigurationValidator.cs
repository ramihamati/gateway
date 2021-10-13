using Digitteck.Gateway.Service.Common.DataValidator;
using Digitteck.Gateway.Service.ComponentConfigurationValidator;

namespace Digitteck.Gateway.Service
{
    public sealed class ConfigurationValidator : IConfigurationValidator
    {
        private ValidationBuilder<GatewayConfiguration> validation;

        public ConfigurationValidator()
        {
            validation = new ValidationBuilder<GatewayConfiguration>();

            validation.AddValidation(new ValidationNullChecks());
            validation.AddValidation(new ValidationDuplicateQueryEntries());
            validation.AddValidation(new ValidationOperationReturnGeneral());
            validation.AddValidation(new ValidationOperationReturnDataIntegrity());
            validation.AddValidation(new ValidationOperationAggregatesDataIntegrity());
            validation.AddValidation(new ValidationOperationContent());
            validation.AddValidation(new ValidationWaitForShouldContainValidTags());
        }

        public ValidationMessage Validate(GatewayConfiguration configuration)
        {
            return validation.Validate(configuration);
        }
    }
}