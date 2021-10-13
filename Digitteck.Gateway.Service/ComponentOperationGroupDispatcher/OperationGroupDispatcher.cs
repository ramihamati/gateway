using Digitteck.Gateway.Service.ComponentTaskOrchestrator;
using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class OperationGroupDispatcher : IOperationGroupDispatcher
    {
        private List<OperationDispatchUnit> DispatchingBlocks { get; }

        public IOperationHandlersStore OperationHandlersStore { get; }
        public IOperationResultStore ResultStore { get; }

        public OperationGroupDispatcher(IOperationHandlersStore handlersStore, IOperationResultStore resultStore)
        {
            this.DispatchingBlocks = new List<OperationDispatchUnit>();
            OperationHandlersStore = handlersStore;
            ResultStore = resultStore;
        }

        public void AddOperationBlock(OperationDispatchUnit operationBlock)
        {
            DispatchingBlocks.Add(operationBlock);
        }

        public async Task<OperationResult> DispatchOperationBlocks(RouteContext route)
        {
            try
            {
                if (route.RunAsync)
                {
                    return await DispatchAsynchronous(route).ConfigureAwait(false);
                }
                else
                {
                    return await DispatchSynchronous(route).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, ex.Message, ex);
            }
        }

        private async Task<OperationResult> DispatchAsynchronous(RouteContext route)
        {
            /*
             * Validations
             */

            if (this.DispatchingBlocks.Count == 0)
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream has no operations defined");
            }
            //last operation must be a return
            if (!(this.DispatchingBlocks[this.DispatchingBlocks.Count - 1].OperationCore is OperationReturn))
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The last operation must be a return operation");
            }
            //must have only one return
            if (this.DispatchingBlocks.Count(x => x.OperationCore is OperationReturn) != 1)
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream must contain only one return statement");
            }

            //must contain at least one call or aggregate
            if (!(this.DispatchingBlocks.Any(x => x.OperationCore is OperationCall) || this.DispatchingBlocks.Any(x => x.OperationCore is OperationAggregateResponse)))
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream does not contain any operation calls or aggregates");
            }

            /*
            * Execution
            */

            List<Tuple<TaskDescriptor, string>> descriptors = new List<Tuple<TaskDescriptor, string>>();
            //create task tescriptors - all except the return
            for (int i = 0; i < this.DispatchingBlocks.Count - 1; i++)
            {
                TaskDescriptor taskDescriptor = new TaskDescriptor(this.DispatchingBlocks[i].OperationTag, async () =>
                {
                    OperationDispatchUnit dispatchUnit = this.DispatchingBlocks[i];
                    OperationResult response = await dispatchUnit.Execute(route).ConfigureAwait(false);
                    this.ResultStore.AddResponse(response);
                });

                descriptors.Add(Tuple.Create<TaskDescriptor, string>(taskDescriptor, this.DispatchingBlocks[i].OperationTag));
            }

            //create waitable processes
            foreach (var (descriptor, tagName) in descriptors)
            {
                OperationDispatchUnit operationDispatchUnit = this.DispatchingBlocks.Find(x => x.OperationTag == tagName);

                //this task should wait for : 

                string[] waitFor = descriptor.After;
                TaskDescriptor[] waitForDescriptors = descriptors.Where(x => waitFor.Contains(x.Item2)).Select(x => x.Item1).ToArray();
                if (waitFor.Length != waitForDescriptors.Length)
                {
                    throw new GatewayException(ErrorCode.OperationGroupDispatcher, $"Could not find all operations in the list {string.Join(',', waitFor)}");
                }

                descriptor.WaitFor(waitForDescriptors);
            }
            //create the orchestrator
            TaskOrchestrator taskOrchestrator = TaskOrchestrator.New();
            taskOrchestrator.Add(descriptors.Select(x => x.Item1).ToArray());

            // the json validator will ensure there is only 1 and min 1 return operation in the definition
            OperationDispatchUnit returnOp = this.DispatchingBlocks[this.DispatchingBlocks.Count - 1];
            OperationResult returnResponse = await returnOp.Execute(route).ConfigureAwait(false);
            this.ResultStore.AddResponse(returnResponse);

            //check if there is a response
            if (!this.ResultStore.ContainsResponse(returnOp.OperationTag))
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, $"Could not find the response with tag {returnOp.OperationTag}");
            }

            //return value
            //TODO null and data checks
            return this.ResultStore.GetResponse(returnOp.OperationTag);
        }

        private async Task<OperationResult> DispatchSynchronous(RouteContext route)
        {
            /*
             * Validations
             */

            if (this.DispatchingBlocks.Count == 0)
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream has no operations defined");
            }
            //last operation must be a return
            if (!(this.DispatchingBlocks[this.DispatchingBlocks.Count - 1].OperationCore is OperationReturn))
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The last operation must be a return operation");
            }
            //must have only one return
            if (this.DispatchingBlocks.Count(x => x.OperationCore is OperationReturn) != 1)
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream must contain only one return statement");
            }

            //must contain at least one call or aggregate
            if (!(this.DispatchingBlocks.Any(x => x.OperationCore is OperationCall) || this.DispatchingBlocks.Any(x => x.OperationCore is OperationAggregateResponse)))
            {
                throw new GatewayException(ErrorCode.OperationGroupDispatcher, "The downstream does not contain any operation calls or aggregates");
            }

            /*
             * Execution
             */
            //execute all except the last one which is a return statement
            for (int i = 0; i < this.DispatchingBlocks.Count - 1; i++)
            {
                OperationDispatchUnit dispatchUnit = this.DispatchingBlocks[i];
                OperationResult response = await dispatchUnit.Execute(route).ConfigureAwait(false);
                this.ResultStore.AddResponse(response);
            }

            //Execute return

            //the json validator will ensure there is only 1 and min 1 return operation in the definition
            OperationDispatchUnit returnOp = this.DispatchingBlocks[this.DispatchingBlocks.Count - 1];
            OperationResult returnResponse = await returnOp.Execute(route).ConfigureAwait(false);
            this.ResultStore.AddResponse(returnResponse);

            //check if there is a response
            if (!this.ResultStore.ContainsResponse(returnOp.OperationTag))
            {
                throw new System.Exception($"Could not find the response with tag {returnOp.OperationTag}");
            }

            //return value
            //TODO null and data checks
            return this.ResultStore.GetResponse(returnOp.OperationTag);
        }
    }
}
