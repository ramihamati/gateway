using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSGWConfigurationMap : DataMap<JSGWConfiguration, GatewayConfiguration>
    {
        public override GatewayConfiguration Map(JSGWConfiguration source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                List<DirectiveCore> globalDirectives = provider.Map<JSDirectiveCore, DirectiveCore>(source.GlobalDirectives);

                List<RouteDefinition> routeModels = provider.Map<JSRouteDefinition, RouteDefinition>(source.RouteModels);

                return new GatewayConfiguration
                {
                    GlobalDirectives = globalDirectives,
                    RouteModels = routeModels
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
