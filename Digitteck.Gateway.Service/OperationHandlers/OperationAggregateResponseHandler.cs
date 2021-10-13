using Digitteck.Gateway.Service.Common.Helpers;
using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public sealed class OperationAggregateResponseHandler : OperationHandlerCore<OperationAggregateResponse>
    {
        public OperationAggregateResponse Operation { get; private set; }

        private readonly IOperationResultStore _resultStore;
        private readonly IAggregateBinder _aggregateBinder;

        /// <summary>
        /// The constructor can be used for parameter injection
        /// </summary>
        public OperationAggregateResponseHandler(IOperationResultStore resultStore, IAggregateBinder aggregateBinder)
        {
            _resultStore = resultStore;
            _aggregateBinder = aggregateBinder;
        }

        public override async Task<OperationResponse> Execute(GatewayContext context, RouteContext routeContext)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Operation.Executor))
                {
                    throw new GatewayException(ErrorCode.OperationAggregateResponseHandler, $"The aggregate response must have a fully qualified class name as the executor that inherits the {typeof(IAggregateResponse).FullName} interface");
                }

                //the executor has to provide the assembly followed by the FQTN
                //e.g. "Executor": "Digitteck.Gateway.Api, Digitteck.Gateway.Api.Aggregates.MovieAggregateResponse",
                string[] parts = this.Operation.Executor.Split(',');

                if (parts.Length != 2)
                {
                    throw new GatewayException(ErrorCode.OperationAggregateResponseHandler, @"The executor type has to provide the assembly followed by the FQTN.\n/e.g. ""Executor"": ""Digitteck.Gateway.Api, Digitteck.Gateway.Api.Aggregates.MovieAggregateResponse""");
                }

                string assemblyName = parts[0];
                string typeName = parts[1];

                Type executorType = AssemblyHelper.FindType(assemblyName, typeName);

                if (executorType == null)
                {
                    throw new GatewayException(ErrorCode.OperationAggregateResponseHandler, $"Could not load type {typeName} for the aggregate response");
                }

                IAggregateResponse aggregateResponse = _aggregateBinder.Activate(executorType);

                return await aggregateResponse.Execute();
            }
            catch (GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return new OperationStringReponse(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public override void Initialize(OperationAggregateResponse operation)
        {
            this.Operation = operation;
        }
    }
}
