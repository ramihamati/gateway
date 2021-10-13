using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public class JSDiscriminatorDef
    {
        public string NodeValue { get; }
        public Type ToType { get; }

        public JSDiscriminatorDef(string nodeValue, Type toType)
        {
            this.NodeValue = nodeValue;
            this.ToType = toType;
        }
    }
}
