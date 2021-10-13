using Digitteck.Gateway.Service.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public class OperationStringReponse : OperationResponse
    {
        public override ResponseContentType ContentType => ResponseContentType.String;

        public override bool IsSuccessfull => 200 <= (int)Status && (int)Status < 299;

        public HttpStatusCode Status { get; }
        public string Response { get; }

        public OperationStringReponse(string response, HttpStatusCode httpStatusCode)
        {
            Status = httpStatusCode;
            Response = response;
        }

        public static async Task<OperationStringReponse> FromAsync(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                //works for all status codes
                return new OperationStringReponse
                  (
                      await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false),
                      httpResponseMessage.StatusCode
                  );
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.OperationByteArrayResponse, ex.Message, ex);
            }
            //if (httpResponseMessage.IsSuccessStatusCode)
            //{
            //    return new OperationStringReponse
            //    (
            //        await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false),
            //        httpResponseMessage.StatusCode
            //    );
            //}
            //else
            //{
            //    return new OperationStringReponse(string.Empty, httpResponseMessage.StatusCode);
            //}
        }
    }
}
