﻿using Digitteck.Gateway.Service.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Digitteck.Gateway.Service
{
    public sealed class OperationHandlersStore : IOperationHandlersStore
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly List<OperationHandlerTypeInfo> handlerTypeInfos;

        /// <summary>
        /// Generated by the builder
        /// </summary>
        internal OperationHandlersStore(IServiceProvider serviceProvider, List<OperationHandlerTypeInfo> handlerTypeInfos)
        {
            _serviceProvider = serviceProvider;
            this.handlerTypeInfos = handlerTypeInfos;
        }

        public TOpHndl GetHandlerFor<TOp, TOpHndl>()
            where TOp : OperationCore where TOpHndl : OperationHandlerCore<TOp>
        {
            try
            {
                OperationHandlerTypeInfo info = this.handlerTypeInfos.Find(x => x.OperationType.Equals(typeof(TOp)));

                if (info is null)
                {
                    throw new GatewayException(ErrorCode.OperationHandlersStore, $"Could not find a handler for the operation {typeof(TOp).FullName}");
                }

                return ActivatorUtilities.CreateInstance<TOpHndl>(_serviceProvider);
            }
            catch (GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationHandlersStore, ex.Message, ex);
            }
        }

        public OperationHandlerCore GetHandlerFor(OperationCore operationCore)
        {
            try
            {
                Type opConcreteType = operationCore.GetType();

                OperationHandlerTypeInfo info = this.handlerTypeInfos.Find(x => x.OperationType.Equals(opConcreteType));

                if (info is null)
                {
                    throw new Exception($"Could not find a handler for the operation {opConcreteType.FullName}");
                }

                return ActivatorUtilities.CreateInstance(_serviceProvider, info.OperationHandlerType) as OperationHandlerCore;
            }
            catch(GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationHandlersStore, ex.Message, ex);
            }
        }
    }
}