namespace Digitteck.Gateway.Service
{
    public interface IOperationHandlersStore
    {
        TOpHndl GetHandlerFor<TOp, TOpHndl>()
            where TOp : OperationCore
            where TOpHndl : OperationHandlerCore<TOp>;
        OperationHandlerCore GetHandlerFor(OperationCore operationCore);
    }
}