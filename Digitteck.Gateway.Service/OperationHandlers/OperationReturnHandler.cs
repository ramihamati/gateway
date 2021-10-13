using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The constructor can be used for parameter injection
    /// </summary>
    public sealed class OperationReturnHandler : OperationHandlerCore<OperationReturn>
    {
        public OperationReturn Operation { get; private set; }

        private readonly IOperationResultStore _resultStore;

        /// <summary>
        /// The constructor can be used for parameter injection
        /// </summary>
        public OperationReturnHandler(IOperationResultStore operationResultStore)
        {
            _resultStore = operationResultStore;
        }

        public override async Task<OperationResponse> Execute(GatewayContext context, RouteContext routeContext)
        {
            try
            {
                //becase the content is synchronouse, we use TaskRun to make CPU-bound work in a backgroud thread
                return await Task.Run(() => {
                    if (!_resultStore.ContainsResponse(Operation.ReturnTag))
                    {
                        throw new GatewayException(ErrorCode.OperationReturnHandler, $"Could not find a response with the tag {Operation.ReturnTag}");
                    }

                    //TODO : null check
                    return _resultStore.GetResponse(Operation.ReturnTag).ResponseValue;
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //TODO : maybe log this error
                return new OperationStringReponse(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public override void Initialize(OperationReturn operation)
        {
            this.Operation = operation;
        }
    }
}
