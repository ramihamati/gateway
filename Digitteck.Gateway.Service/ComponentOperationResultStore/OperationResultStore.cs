using Digitteck.Gateway.Service.Exceptions;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class OperationResultStore : IOperationResultStore
    {
        public BlockingCollection<OperationResult> _storeResults;

        public OperationResultStore()
        {
            this._storeResults = new BlockingCollection<OperationResult>();
        }

        public void AddResponse(OperationResult componentResult)
        {
            EnsureNoDuplicates(componentResult.OperationTag);

            this._storeResults.Add(componentResult);
        }

        public void AddResponse(string operationTag, OperationByteArrayResponse apiResponse)
        {
            EnsureNoDuplicates(operationTag);

            this._storeResults.Add(new OperationResult(operationTag, apiResponse));
        }

        public async Task AddResponseAsync(string operationTag, HttpResponseMessage message)
        {
            EnsureNoDuplicates(operationTag);

            OperationByteArrayResponse apiResponse = await OperationByteArrayResponse.FromAsync(message).ConfigureAwait(false);
            this._storeResults.Add(new OperationResult(operationTag, apiResponse));
        }

        public bool ContainsResponse(string operationTag)
        {
            return _storeResults.Any(x => x.OperationTag == operationTag);
        }

        public OperationResult GetResponse(string operationTag)
        {
            return this._storeResults.FirstOrDefault(x => x.OperationTag == operationTag);
        }

        private void EnsureNoDuplicates(string tag)
        {
            if (this._storeResults.Any(x => x.OperationTag == tag))
            {
                throw new GatewayException(ErrorCode.OperationResultStore, $"The result from operation with tag \'{tag}\' was already added");
            }
        }

    }
}
