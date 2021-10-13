using System;

#pragma warning disable RCS1194 // Implement exception constructors.

namespace Digitteck.Gateway.Service.Exceptions
{
    public class GatewayException : Exception
    {
        public ErrorCode[] ErrorCodes { get; }

        public GatewayException(ErrorCode errorCode, string message) : base(message)
        {
            this.ErrorCodes = new ErrorCode[] { errorCode };
        }

        public GatewayException(ErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorCodes = new ErrorCode[] { errorCode };
        }
    }
}
#pragma warning restore RCS1194 // Implement exception constructors.
