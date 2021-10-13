using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.Gateway.Service
{
    public sealed class GatewayMiddleware : IMiddleware
    {
        public IGWRuntime GWRuntime { get; }
        public IConfigurationProvider ConfigurationProvider { get; }

        public GatewayMiddleware(IGWRuntime executor, IConfigurationProvider configurationProvider)
        {
            GWRuntime = executor;
            ConfigurationProvider = configurationProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            GatewayConfiguration configuration = ConfigurationProvider.Build();

            try
            {
                //start gw piping
                OperationResult responseMessage = await GWRuntime.Execute(context, configuration).ConfigureAwait(false);

                if(responseMessage.ResponseValue.ContentType == ResponseContentType.ByteArray)
                {
                    OperationByteArrayResponse response = (OperationByteArrayResponse)responseMessage.ResponseValue;

                    //TODO : add headers in OperationResult then in Response
                    context.Response.StatusCode = (int)response.Status;
                    await context.Response.Body.WriteAsync(response.Result).ConfigureAwait(false);
                }
                else if (responseMessage.ResponseValue.ContentType == ResponseContentType.String)
                {
                    //from an aggregate we can receive a string
                    OperationStringReponse response = (OperationStringReponse)responseMessage.ResponseValue;

                    //TODO : add headers in OperationResult then in Response
                    context.Response.StatusCode = (int)response.Status;
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response.Response)).ConfigureAwait(false);
                }
                else
                {

                    //for the momemnt each response is deconstructed in a bytearray. OperationResult=>OperaetionRespnse(derivate=ApiByteArrayResponse). This is done in the OperationCallHandler
                    //for other types of responses we need to add suport there and handler the return here
                    throw new NotSupportedException();
                }
            }
            catch (System.Exception ex)
            {
                await context.Response.WriteAsync(ex.Message);
                throw;
            }
          
            //continue
            //await next(context).ConfigureAwait(false);
        }
    }
}
