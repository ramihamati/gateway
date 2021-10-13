using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.ComponentConfigurationProvider.Models;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSOperationCallMap : DataMap<JSOperationCall, OperationCall>
    {
        public override OperationCall Map(JSOperationCall source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                List<DirectiveCore> directives = new List<DirectiveCore>();

                if (source.Directives != null)
                {
                    foreach (JSDirectiveCore jsDirective in source.Directives)
                    {
                        directives.Add(provider.Map<DirectiveCore>(jsDirective));
                    }
                }

                TemplatePathAndQueryObject pathAndQuery = new TemplatePathAndQueryObject(source.Template);
                HostObject hostObject = new HostObject(source.ServerHost);
                PortObject portObject = new PortObject(source.ServerPort);
                SchemeObject schemeObject = provider.Map<JSScheme, SchemeObject>(source.Scheme);

                var operationCall = new OperationCall
                {
                    Directives = directives,
                    HttpMethod = provider.Map<JSHttpMethodType, HttpMethodType>(source.HttpMethod),
                    OperationTag = source.OperationTag,
                    ServerPort = portObject,
                    ServerHost = hostObject,
                    PathAndQuery = pathAndQuery,
                    Scheme = schemeObject,
                    WaitFor = (source.WaitFor == null) ? new List<string>() : source.WaitFor.Select(x => x).ToList()
                };

                return operationCall;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
