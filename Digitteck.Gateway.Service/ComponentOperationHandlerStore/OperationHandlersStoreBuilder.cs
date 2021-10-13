using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// A builder class that is used in the extension method to create the <see cref="OperationHandlersStore"/> class
    /// </summary>
    public sealed class OperationHandlingBuilder
    {
        private readonly List<OperationHandlerTypeInfo> handlerTypeInfos;
        private readonly IServiceProvider _serviceProvider;

        internal OperationHandlingBuilder(IServiceProvider serviceProvider)
        {
            handlerTypeInfos = new List<OperationHandlerTypeInfo>();
            _serviceProvider = serviceProvider;
        }

        public void AddHandlerForOperation<TOp, TOpHndl>()
              where TOp : OperationCore where TOpHndl : OperationHandlerCore<TOp>
        {
            this.handlerTypeInfos.Add(OperationHandlerTypeInfo.Create<TOp, TOpHndl>());
        }

        internal OperationHandlersStore Build()
        {
            return new OperationHandlersStore(_serviceProvider, this.handlerTypeInfos);
        }
    }
}
