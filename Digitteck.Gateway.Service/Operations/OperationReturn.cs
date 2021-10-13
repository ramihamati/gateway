using Digitteck.Gateway.Service.Attributes;

namespace Digitteck.Gateway.Service
{
    [OperationName("Return")]
    public class OperationReturn: OperationCore
    {
        public string ReturnTag { get; set; }
    }
}
