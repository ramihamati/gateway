using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public class Downstream
    {
        public List<OperationCore> Operations { get; set; }

        /// <summary>
        /// If the flag is true, all operations defined downstream will run asynchronously, respeciting the order defined by the wait property
        /// defined in every operation.
        /// </summary>
        public bool RunAsync { get; set; } = false;
        
        public Downstream()
        {
            this.Operations = new List<OperationCore>();
        }
    }

}
