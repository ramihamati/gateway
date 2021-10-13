using Digitteck.Gateway.Service.Exceptions;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Digitteck.Gateway.Service
{
    public sealed class OperationResponseConverter : IOperationResponseConverter
    {
        public T ConvertResultTo<T>(OperationResult operationResult)
        {
            try
            {
                if (operationResult.ResponseValue == null)
                {
                    throw new GatewayException(ErrorCode.OperationResponseConverter, $"Could not find a valid response in operation result with tag {operationResult.OperationTag}");
                }

                if (operationResult.ResponseValue.ContentType == ResponseContentType.ByteArray)
                {
                    OperationByteArrayResponse byteArrayResponse = (OperationByteArrayResponse)operationResult.ResponseValue;

                    if (byteArrayResponse.Result == null || byteArrayResponse.Result.Length == 0)
                    {
                        return default;
                    }

                    string asString = Encoding.UTF8.GetString(byteArrayResponse.Result);

                    return JsonConvert.DeserializeObject<T>(asString);
                }
                else if (operationResult.ResponseValue.ContentType == ResponseContentType.String)
                {
                    OperationStringReponse stringReponse = (OperationStringReponse)operationResult.ResponseValue;

                    return JsonConvert.DeserializeObject<T>(stringReponse.Response);
                }
                else
                {
                    throw new GatewayException(ErrorCode.OperationResponseConverter, $"The response content type {operationResult.ResponseValue.ContentType} is not supported");
                }
            }
            catch (GatewayException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationResponseConverter, ex.Message, ex);
            }
        }
    }
}
