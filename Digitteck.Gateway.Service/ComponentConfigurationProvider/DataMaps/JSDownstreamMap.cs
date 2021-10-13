using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using Digitteck.Gateway.SourceModels;
using System;
using System.Linq;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSDownstreamMap : DataMap<JSDownstream, Downstream>
    {
        public override Downstream Map(JSDownstream source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(source, nameof(source));
                Ensure.NotNull(provider, nameof(provider));

                return new Downstream
                {
                    RunAsync = source.RunAsync,
                    Operations = provider.Map<OperationCore>(source.Operations).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
