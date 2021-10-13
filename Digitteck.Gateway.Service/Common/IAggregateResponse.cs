using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public interface IAggregateResponse
    {
        Task<OperationResponse> Execute();
    }
}