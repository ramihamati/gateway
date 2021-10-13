using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class OperationByteArrayResponse : OperationResponse
    {
        public override bool IsSuccessfull => 200 <= (int)Status && (int)Status < 299;

        public override ResponseContentType ContentType => ResponseContentType.ByteArray;

        public HttpStatusCode Status { get; }

        public byte[] Result { get; }

        public OperationByteArrayResponse(byte[] result, HttpStatusCode code)
        {
            this.Status = code;
            this.Result = result ?? new byte[] { };
        }

        public static async Task<OperationByteArrayResponse> FromAsync(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                //works for all status codes
                return new OperationByteArrayResponse
                (
                    await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                    httpResponseMessage.StatusCode
                );
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationByteArrayResponse, ex.Message, ex);
            }
            //if (httpResponseMessage.IsSuccessStatusCode)
            //{
            //    return new OperationByteArrayResponse
            //    (
            //        await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
            //        httpResponseMessage.StatusCode
            //    );
            //}
            //else
            //{
            //    return new OperationByteArrayResponse(new byte[] { }, httpResponseMessage.StatusCode);
            //}
        }
    }
}
