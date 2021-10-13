using System;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// Stores relation between the <see cref="OperationCore"/> and the <see cref="OperationHandlerCore{T}"/>
    /// </summary>
    public sealed class OperationHandlerTypeInfo
    {
        public Type OperationType { get; }
        public Type OperationHandlerType { get; }

        /// <summary>
        /// Don't allow public access. 
        /// </summary>
        private OperationHandlerTypeInfo(Type operationType, Type operationHandlerType)
        {
            this.OperationHandlerType = operationHandlerType;
            this.OperationType = operationType;
        }

        public static OperationHandlerTypeInfo Create<TOp, TOpHndl>()
            where TOp : OperationCore where TOpHndl : OperationHandlerCore<TOp>
        {
            return new OperationHandlerTypeInfo(typeof(TOp), typeof(TOpHndl));
        }
    }
}
