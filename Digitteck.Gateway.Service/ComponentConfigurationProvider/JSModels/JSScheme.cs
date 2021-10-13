using System.Runtime.Serialization;

namespace Digitteck.Gateway.Service.ComponentConfigurationProvider.Models
{
    public enum JSScheme
    {
        NOTDEFINED = 0,

        [EnumMember(Value = "http")]
        HTTP,

        [EnumMember(Value = "https")]
        HTTPS
    }
}
