using System.Collections.Generic;
using System.Linq;
using Digitteck.Gateway.Service.Common.DataValidator;

namespace Digitteck.Gateway.Service.ComponentConfigurationValidator
{
    /// <summary>
    /// V001 : duplicate query entries : there should not be multiple end points having the same template (entry point)
    /// </summary>
    public sealed class ValidationDuplicateQueryEntries : Validation<GatewayConfiguration>
    {
        public override bool ExistIfValidationFails => false;

        public override ValidationMessage Validate(GatewayConfiguration field)
        {
            ValidationMessage message = ValidationMessage.Success();

            HashSet<string> entryPoints = new HashSet<string>();

            List<string> entryPointsAsErrors = new List<string>();

            foreach (RouteDefinition item in field.RouteModels)
            {
                string ep = item.Upstream.EntryTemplate.Value.Trim().Trim(GlobalEnv.PathDelimiter, GlobalEnv.InvalidPathDelimiter);

                if (entryPoints.Contains(ep))
                {
                    entryPointsAsErrors.Add(ep);
                }

                entryPoints.Add(ep);
            }

            foreach (var duplicateEP in entryPointsAsErrors.Distinct())
            {
                message.Add(ValidationMessage.Fail($"Entry point {duplicateEP} was found more then one"));
            }

            return message;
        }
    }
}