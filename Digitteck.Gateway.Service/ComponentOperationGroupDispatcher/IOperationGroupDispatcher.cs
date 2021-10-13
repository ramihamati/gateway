using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public interface IOperationGroupDispatcher
    {
        void AddOperationBlock(OperationDispatchUnit operationBlock);
        Task<OperationResult> DispatchOperationBlocks(RouteContext route);
    }
}