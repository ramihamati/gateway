using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSRouteDefinitionMap : DataMap<JSRouteDefinition, RouteDefinition>
    {
        public override RouteDefinition Map(JSRouteDefinition source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                Downstream downstream = provider.Map<JSDownstream, Downstream>(source.Downstream);
                Upstream upstream = provider.Map<JSEndPointModel, Upstream>(source.Endpoint);

                return new RouteDefinition
                {
                    Downstream = downstream,
                    Upstream = upstream
                };
            }
            catch (System.Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }

        }
    }
}
