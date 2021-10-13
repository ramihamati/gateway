using Digitteck.Gateway.Service.Common.DataValidator;

namespace Digitteck.Gateway.Service
{
    public interface IConfigurationValidator
    {
        ValidationMessage Validate(GatewayConfiguration configuration);
    }
}