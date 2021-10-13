using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSOperationReturnMap : DataMap<JSOperationReturn, OperationReturn>
    {
        public override OperationReturn Map(JSOperationReturn source, DataMapper provider)
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

                return new OperationReturn
                {
                    Directives = directives,
                    ReturnTag = source.ReturnTag,
                    OperationTag = source.OperationTag
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
