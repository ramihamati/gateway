using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{ 
    /// <summary>
    /// The constructor can be used for parameter injection
    /// </summary>
    public abstract class OperationHandlerCore
    {
        /// <summary>
        /// Handler for an operation
        /// </summary>
        /// <param name="context">The gateway context containing the httpcontext and possible routes</param>
        /// <param name="routeContext">The matched route context</param>
        /// <returns></returns>
        public abstract Task<OperationResponse> Execute(GatewayContext context, RouteContext routeContext);
    }

    public abstract class OperationHandlerCore<T> : OperationHandlerCore 
        where T : OperationCore
    {
        public abstract void Initialize(T operation);
    }
}
