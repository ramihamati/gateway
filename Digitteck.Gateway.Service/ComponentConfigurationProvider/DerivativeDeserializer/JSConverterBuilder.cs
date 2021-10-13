using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    /// <summary>
    /// The converter builder is a class used to create a special converter for deserializing json files.
    /// The converter is used to create derived models (with base parent) based on a discriminator.
    /// The converter reads a property in the JToken and if that property is found, based on it's value it will determine what type
    /// should it deserialize to. 
    /// </summary>
    public class JSConverterBuilder
    {
        private List<JSDiscriminatorDef> discriminators;
        private Type baseType;
        private string discriminatorNode;

        public JSConverterBuilder()
        {
            discriminators = new List<JSDiscriminatorDef>();
        }

        public JSConverterBuilder BaseType<T>()
        {
            this.baseType = typeof(T);
            return this;
        }

        public JSConverterBuilder DiscriminatorNode(string nodeName)
        {
            this.discriminatorNode = nodeName;
            return this;
        }

        public JSConverterBuilder Discriminate<T>(string value)
        {
            this.discriminators.Add(new JSDiscriminatorDef(value, typeof(T)));
            return this;
        }

        internal DerivatesConverter Build()
        {
            if (this.baseType == null)
            {
                throw new GatewayException(ErrorCode.JSConverterBuilder,$"You must define a base type using the method {nameof(BaseType)}");
            }
            if (this.discriminatorNode == null)
            {
                throw new GatewayException(ErrorCode.JSConverterBuilder,$"You must define a discriminator node using the method {nameof(DiscriminatorNode)}");
            }
            if (discriminators.Count == 0)
            {
                throw new GatewayException(ErrorCode.JSConverterBuilder, $"No discriminators have been defined");
            }

            return new DerivatesConverter(this.discriminators, this.baseType, this.discriminatorNode);
        }
    }
}
