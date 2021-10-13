using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSOperationAggregateResponseMap : DataMap<JSOperationAggregateResponses, OperationAggregateResponse>
    {
        public override OperationAggregateResponse Map(JSOperationAggregateResponses source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new OperationAggregateResponse
                {
                    Directives = (source.Directives == null) ? new List<DirectiveCore>() : provider.Map<JSDirectiveCore, DirectiveCore>(source.Directives),
                    Executor = source.Executor,
                    OperationTag = source.OperationTag,
                    WaitFor = (source.WaitFor == null) ? new List<string>() : source.WaitFor.Select(x=>x).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
