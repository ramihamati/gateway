using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    [OperationName("AggregateResponses")]
    public class OperationAggregateResponse : OperationCore
    {
        /// <summary>
        /// This property will set the order of task execution if the downstream property RunAsync is true.
        /// It contains a list of operation tags, and this op will wait for other ops to be executed before executing.
        /// </summary>
        public IList<string> WaitFor { get; set; }

        public string Executor { get; set; }

        public OperationAggregateResponse() 
        {
            WaitFor = new List<string>();
        }
    }
}
