using Digitteck.Gateway.Service.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// provides parameters to constructor
    /// </summary>
    public sealed class AggregateBinder : IAggregateBinder
    {
        //operation responses stored
        private readonly IOperationResultStore _operationResultStore;

        //registered services
        private readonly IServiceProvider _serviceProvider;

        public AggregateBinder(IOperationResultStore operationResultStore, IServiceProvider serviceProvider)
        {
            _operationResultStore = operationResultStore;
            _serviceProvider = serviceProvider;
        }

        public IAggregateResponse Activate(Type type)
        {
            if (!typeof(IAggregateResponse).IsAssignableFrom(type))
            {
                throw new Exception($"The type {type} must implement the interface {typeof(IAggregateResponse).FullName}");
            }

            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var ctor in constructors)
            {
                if (InternalActivate(type, ctor, out IAggregateResponse tValue))
                {
                    return tValue;
                }
            }

            throw new Exception($"Could not find a valid constructor for the aggregate response class {type.FullName}");
        }

        private bool InternalActivate(Type type, ConstructorInfo ctor, out IAggregateResponse tValue)
        {
            ParameterInfo[] parameters = ctor.GetParameters();

            List<object> arguments = new List<object>();

            foreach (ParameterInfo param in parameters)
            {
                if (InternalGetParameter(param, out object paramValue))
                {
                    arguments.Add(paramValue);
                }
                else
                {
                    tValue = default;
                    return false;
                }
            }

            tValue = (IAggregateResponse)Activator.CreateInstance(type, arguments.ToArray());
            return true;
        }

        private bool InternalGetParameter(ParameterInfo param, out object paramValue)
        {
            //maybe get from response store
            if (param.GetCustomAttribute<OperationTagAttribute>() is OperationTagAttribute tagAttribute)
            {
                if (_operationResultStore.ContainsResponse(tagAttribute.OperationTag))
                {
                    paramValue = _operationResultStore.GetResponse(tagAttribute.OperationTag);
                    return true;
                }
            }
            //maybe get from service
            object service = this._serviceProvider.GetService(param.ParameterType);

            if (service != null)
            {
                paramValue = service;
                return true;
            }

            paramValue = null;
            return false;
        }
    }
}
