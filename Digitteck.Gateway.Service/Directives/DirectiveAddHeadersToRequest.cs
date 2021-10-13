using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    [DirectiveName("AddHeadersToRequest")]
    public class DirectiveAddHeadersToRequest: DirectiveCore
    {
        public string Executor { get; set; }

        public IList<object> Arguments { get; set; }

        public DirectiveAddHeadersToRequest()
        {
            this.Arguments = new List<object>();
        }
    }
}
