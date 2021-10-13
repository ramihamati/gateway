using Digitteck.Gateway.SourceModels;
using System.Collections.Generic;
using Digitteck.Gateway.Service.JsonModelProvider;
using System;
using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.ComponentConfigurationProvider.Models;
using Digitteck.Gateway.Service.Exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Digitteck.Gateway.Service
{
    public class JsonConfigurationProvider : IConfigurationProvider
    {
        private readonly IContentProvider _contentProvider;
        private DataMapper _dataMapper;

        public JsonConfigurationProvider(IContentProvider contentProvider)
        {
            this._contentProvider = contentProvider;
            BuildDataMapper();
        }

        private void BuildDataMapper()
        {
            MapDefinitionGroup mapDefinitionGroup = new MapDefinitionGroup();

            mapDefinitionGroup.AddMap<JSDirectiveAddHeadersToRequest, DirectiveAddHeadersToRequest, JSDirectiveAddHeadersToRequestMap>();
            mapDefinitionGroup.AddMap<JSDirectiveAddQueriesToRequest, DirectiveAddQueriesToRequest, JSDirectiveAddQueriesToRequestMap>();
            mapDefinitionGroup.AddMap<JSDirectiveUseCircuitBreaker, DirectiveUseCircuitBreaker, JSDirectiveUseCircuitBreakerMap>();
            mapDefinitionGroup.AddMap<JSDirectiveUseRateLimiting, DirectiveUseRateLimiting, JSDirectiveUseRateLimitingMap>();
            mapDefinitionGroup.AddMap<JSDirectiveUseRetryPolicy, DirectiveUseRetryPolicy, JSDirectiveUseRetryPolicyMap>();
            mapDefinitionGroup.AddMap<JSDownstream, Downstream, JSDownstreamMap>();
            mapDefinitionGroup.AddMap<JSEndPointModel, Upstream, JSEndPointModelMap>();
            mapDefinitionGroup.AddMap<JSGWConfiguration, GatewayConfiguration, JSGWConfigurationMap>();
            mapDefinitionGroup.AddMap<JSHttpMethodType, HttpMethodType, JSHttpMethodTypeMap>();
            mapDefinitionGroup.AddMap<JSOperationAggregateResponses, OperationAggregateResponse, JSOperationAggregateResponseMap>();
            mapDefinitionGroup.AddMap<JSOperationCall, OperationCall, JSOperationCallMap>();
            mapDefinitionGroup.AddMap<JSOperationReturn, OperationReturn, JSOperationReturnMap>();
            mapDefinitionGroup.AddMap<JSRouteDefinition, RouteDefinition, JSRouteDefinitionMap>();
            mapDefinitionGroup.AddMap<JSScheme, SchemeObject, JSSchemeModelMap>();

            this._dataMapper = mapDefinitionGroup.Build();
        }

        public GatewayConfiguration Build()
        {
            try
            {
                string globalFileContent = _contentProvider.GetGlobalFileContent();
                string[] routeFilesContent = _contentProvider.GetRoutesContent();

                JsonDeserializeDerivates jsonDeserializeDerivates = new JsonDeserializeDerivates()
                    .CreateConverter((builder) =>
                    {
                        //define the discriminator property for children of JSDirectiveCore
                        builder.BaseType<JSDirectiveCore>()
                           .DiscriminatorNode("Directive")
                           .Discriminate<JSDirectiveUseRateLimiting>("UseRateLimiting")
                           .Discriminate<JSDirectiveAddQueriesToRequest>("AddQueriesToRequest")
                           .Discriminate<JSDirectiveUseCircuitBreaker>("UseCircuitBreaker")
                           .Discriminate<JSDirectiveAddHeadersToRequest>("AddHeadersToRequest")
                           .Discriminate<JSDirectiveUseRetryPolicy>("UseRetryPolicy");
                    })
                    .CreateConverter((builder) =>
                    {
                        //define the discriminator property for children of JSOperationCore
                        builder.BaseType<JSOperationCore>()
                           .DiscriminatorNode("Operation")
                           .Discriminate<JSOperationAggregateResponses>("AggregateResponses")
                           .Discriminate<JSOperationCall>("Call")
                           .Discriminate<JSOperationReturn>("Return");
                    });

                JSGWConfigurationRaw globalConf = jsonDeserializeDerivates.Deserialize<JSGWConfigurationRaw>(globalFileContent);

                JSGWConfiguration configuration = new JSGWConfiguration();

                //copy directives
                foreach (JSDirectiveCore directive in globalConf.GlobalDirectives)
                {
                    configuration.GlobalDirectives.Add(directive.Clone());
                }
                //copy route definitions

                foreach (string routeContent in routeFilesContent)
                {
                    //deserializing once to determine if the file is an array or note
                    JToken token = JsonConvert.DeserializeObject<JToken>(routeContent);
                    if (token.Type == JTokenType.Array)
                    {
                        List<JSRouteDefinition> routeDef = jsonDeserializeDerivates.Deserialize<List<JSRouteDefinition>>(routeContent);
                        configuration.RouteModels.AddRange(routeDef);
                    }
                    else if (token.Type == JTokenType.Object)
                    {
                        JSRouteDefinition routeDef = jsonDeserializeDerivates.Deserialize<JSRouteDefinition>(routeContent);
                        configuration.RouteModels.Add(routeDef);
                    }
                    else
                    {
                        throw new GatewayException(ErrorCode.ConfigurationProvider, $"The route file {routeContent} is not valid. ");
                    }
                }

                return _dataMapper.Map<JSGWConfiguration, GatewayConfiguration>(configuration);
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.ConfigurationProvider, ex.Message);
            }
        }
    }
}
