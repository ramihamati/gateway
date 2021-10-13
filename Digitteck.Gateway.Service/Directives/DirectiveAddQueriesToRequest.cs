using Digitteck.Gateway.Service.Abstractions;
using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    [DirectiveName("AddQueriesToRequest")]
    public class DirectiveAddQueriesToRequest : DirectiveCore
    {
        public string Executor { get; set; }

        public IList<object> Arguments { get; set; }

        public DirectiveAddQueriesToRequest()
        {
            this.Arguments = new List<object>();
        }
    }

}
