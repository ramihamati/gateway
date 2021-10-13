using Digitteck.Gateway.Service;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ValidatorTests
{
    public static class Fixture
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static GatewayConfiguration GetConfig()
        {
            return new GatewayConfiguration
            {

                GlobalDirectives = new List<DirectiveCore>
                {
                    new DirectiveAddHeadersToRequest
                    {
                        Arguments = new List<object>{},
                        Executor = "a,b"
                    }
                },
                RouteModels = new List<RouteDefinition>
                {
                    new RouteDefinition
                   {
                       Upstream = new Upstream
                       {
                           EntryTemplate = new TemplatePathAndQueryObject("/movies2"),
                           HttpMethod    = HttpMethodType.Connect
                       },
                       Downstream = new Downstream
                       {
                            RunAsync = false,
                            Operations = new List<OperationCore>
                            {
                               new OperationCall
                               {
                                 WaitFor = new List<string>{ },
                                 Directives = new List<DirectiveCore>{ },
                                 HttpMethod = HttpMethodType.Get,
                                 OperationTag = "_call",
                                 PathAndQuery = new TemplatePathAndQueryObject("/api/movies2"),
                                 Scheme =   new SchemeObject("https"),
                                 ServerHost = new HostObject("localhost"),
                                 ServerPort = new PortObject(5000)
                               },
                               new OperationAggregateResponse
                               {
                                   WaitFor =  new   List<string>{ "_call"},
                                   OperationTag = "_call_aggregated",
                                   Directives = new List<DirectiveCore>{ },
                                   Executor = "Digitteck.Gateway.Tests, Digitteck.Gateway.Tests.ValidatorTests.TypeForTests"
                               },
                               new OperationReturn
                               {
                                   Directives = new List<DirectiveCore>{ },
                                   OperationTag = "",
                                   ReturnTag = "_call_aggregated"
                               }
                            }
                       }
                   },
                   new RouteDefinition
                   {
                       Upstream = new Upstream
                       {
                           EntryTemplate = new TemplatePathAndQueryObject("/movies"),
                           HttpMethod    = HttpMethodType.Connect
                       },
                       Downstream = new Downstream
                       {
                            RunAsync = false,
                            Operations = new List<OperationCore>
                            {
                               new OperationCall
                               {
                                 WaitFor = new List<string>{ },
                                 Directives = new List<DirectiveCore>{ },
                                 HttpMethod = HttpMethodType.Get,
                                 OperationTag = "_call",
                                 PathAndQuery = new TemplatePathAndQueryObject("/api/movies"),
                                 Scheme =   new SchemeObject("https"),
                                 ServerHost = new HostObject("localhost"),
                                 ServerPort = new PortObject(5000)
                               },
                               new OperationAggregateResponse
                               {
                                   WaitFor =  new   List<string>{ "_call"},
                                   OperationTag = "_call_aggregated",
                                   Directives = new List<DirectiveCore>{ },
                                   Executor = "Digitteck.Gateway.Tests, Digitteck.Gateway.Tests.ValidatorTests.TypeForTests"
                               },
                               new OperationReturn
                               {
                                   Directives = new List<DirectiveCore>{ },
                                   OperationTag = "",
                                   ReturnTag = "_call_aggregated"
                               }
                            }
                       }
                   }
                }
            };
        }
    }
}
