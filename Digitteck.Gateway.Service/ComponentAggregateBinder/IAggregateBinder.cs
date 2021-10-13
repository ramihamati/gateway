using System;

namespace Digitteck.Gateway.Service
{
    public interface IAggregateBinder
    {
        IAggregateResponse Activate(Type type);
    }
}