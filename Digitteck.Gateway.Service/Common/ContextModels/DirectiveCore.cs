using Digitteck.Gateway.Service.Attributes;
using System;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The base class of a directive
    /// </summary>
    public abstract class DirectiveCore
    {
        public string DirectiveName { get; }

        public Type DirectiveType { get; }

        protected DirectiveCore()
        {
            //no null check ?. because we must ensure this is defined
            this.DirectiveName = DirectiveNameAttribute.GetAttributeFrom(this.GetType()).Name;

            //extract type
            this.DirectiveType = this.GetType();
        }
    }
}
