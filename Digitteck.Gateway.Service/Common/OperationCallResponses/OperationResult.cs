namespace Digitteck.Gateway.Service
{
    public class OperationResult
    {
        public string OperationTag { get; }

        public OperationResponse ResponseValue { get; }

        private readonly OperationResponseConverter _converter;

        public OperationResult(string operationTag, OperationResponse response)
        {
            this.OperationTag = operationTag;
            this.ResponseValue = response;
            this._converter = new OperationResponseConverter();
        }

        public T GetResultAs<T>()
        {
            return _converter.ConvertResultTo<T>(this);
        }
    }
}
