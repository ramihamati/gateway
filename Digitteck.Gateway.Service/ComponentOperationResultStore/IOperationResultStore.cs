using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{

    /// <summary>
    /// Holds results of operation calls
    /// </summary>
    public interface IOperationResultStore
    {
        public bool ContainsResponse(string operationTag);

        public Task AddResponseAsync(string operationTag, HttpResponseMessage message);

        public OperationResult GetResponse(string operationTag);

        void AddResponse(string operationTag, OperationByteArrayResponse apiResponse);

        void AddResponse(OperationResult componentResult);
    }
}
