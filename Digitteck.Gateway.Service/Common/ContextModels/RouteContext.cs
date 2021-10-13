using Digitteck.Gateway.Service.Models;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The operations to be executed by the Route
    /// </summary>
    public class RouteContext
    {
        public UpstreamTemplate UpstreamTemplate { get; set; }

        public List<OperationCore> Operations { get; set; }

        /// <summary>
        /// If the flag is true, all operations defined downstream will run asynchronously, respeciting the order defined by the wait property
        /// defined in every operation.
        /// </summary>
        public bool RunAsync { get; set; } = false;
    }
}
