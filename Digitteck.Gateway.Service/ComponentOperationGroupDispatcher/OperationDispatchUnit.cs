using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class OperationDispatchUnit
    {
        public string OperationName => OperationCore.OperationName;
        public string OperationTag => OperationCore.OperationTag;

        public List<DirectiveCore> Directives => OperationCore.Directives;

        public OperationCore OperationCore { get; }

        public GatewayContext GatewayContext { get; }

        private readonly IOperationHandlersStore _operationHandlersStore;

        public OperationDispatchUnit(OperationCore operationCore, GatewayContext gContext, IOperationHandlersStore operationHandlersStore)
        {
            OperationCore = operationCore;
            GatewayContext = gContext;
            _operationHandlersStore = operationHandlersStore;
        }

        public async Task<OperationResult> Execute(RouteContext executingRouteContext)
        {
            try
            {
                OperationHandlerCore handler = _operationHandlersStore.GetHandlerFor(this.OperationCore);
                MethodInfo methodInfo = handler.GetType().GetMethod(nameof(OperationHandlerCore<OperationCore>.Initialize));
                methodInfo.Invoke(handler, new[] { this.OperationCore });

                OperationResponse apiResponse = await handler.Execute(this.GatewayContext, executingRouteContext).ConfigureAwait(false);

                return new OperationResult(this.OperationTag, apiResponse);
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationDispatchUnit, ex.Message, ex);
            }
        }

        public string GetResponseTagName()
        {
            return this.OperationCore.OperationTag;
        }
    }
}
