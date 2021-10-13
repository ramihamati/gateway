namespace Digitteck.Gateway.Service
{
    public interface IOperationResponseConverter
    {
        T ConvertResultTo<T>(OperationResult operationResult);
    }
}