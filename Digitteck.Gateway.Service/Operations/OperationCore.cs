using Digitteck.Gateway.Service.Attributes;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The base class of an operation
    /// </summary>
    public abstract class OperationCore
    {
        public string OperationName { get; }

        public string OperationTag { get; set; }

        public List<DirectiveCore> Directives { get; set; }

        protected OperationCore()
        {
            this.OperationName = OperationNameAttribute.GetAttributeFrom(this.GetType()).Name;
            this.Directives = new List<DirectiveCore>();
        }
    }
}
