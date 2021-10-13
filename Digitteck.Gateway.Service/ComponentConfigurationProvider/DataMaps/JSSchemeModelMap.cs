using Digitteck.Gateway.DMapper;
using Digitteck.Gateway.Service.Common.Guards;
using Digitteck.Gateway.Service.ComponentConfigurationProvider.Models;
using Digitteck.Gateway.Service.Exceptions;
using System;

namespace Digitteck.Gateway.Service.JsonModelProvider
{
    public sealed class JSSchemeModelMap : DataMap<JSScheme, SchemeObject>
    {
        public override SchemeObject Map(JSScheme source, DataMapper provider)
        {
            try
            {
                Ensure.NotNull(provider, nameof(provider));

                return source switch
                {
                    JSScheme.HTTP => new SchemeObject("http"),
                    JSScheme.HTTPS => new SchemeObject("https"),
                    _ => throw new NotSupportedException($"There is no conversion between {source.ToString()} and type {typeof(SchemeObject).FullName}")
                };
            }
            catch (Exception ex)
            {
                throw new GatewayException(ErrorCode.MappingError, ex.Message);
            }
        }
    }
}
