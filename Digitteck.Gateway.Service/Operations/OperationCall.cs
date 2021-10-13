using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    [OperationName("Call")]
    public class OperationCall : OperationCore
    {
        private SchemeObject _scheme;

        public SchemeObject Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        public HostObject ServerHost { get; set; }

        public PortObject ServerPort { get; set; }

        public TemplatePathAndQueryObject PathAndQuery { get; set; }

        public HttpMethodType HttpMethod { get; set; }

        /// <summary>
        /// This property will set the order of task execution if the downstream property RunAsync is true.
        /// It contains a list of operation tags, and this op will wait for other ops to be executed before executing.
        /// </summary>
        public List<string> WaitFor { get; set; }

        public OperationCall()
        {
            this.WaitFor = new List<string>();
        }
    }
}
