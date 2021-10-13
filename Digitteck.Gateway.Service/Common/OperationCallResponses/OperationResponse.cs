using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{

    public abstract class OperationResponse
    {
        public abstract ResponseContentType ContentType { get; }
        public abstract bool IsSuccessfull { get; }
    }
}
