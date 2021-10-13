using System.Runtime.Serialization;

namespace Digitteck.Gateway.SourceModels
{
    /* JsonConvert uses EnumMember to match values of enum fields, but it also matches
     * the raw field name of the enum (case insensitive) to the income string value.
     */
    public enum JSHttpMethodType
    {
        [EnumMember(Value = "CONNECT")]
        Connect,

        [EnumMember(Value = "PUT")]
        Put,

        [EnumMember(Value = "POST")]
        Post,

        [EnumMember(Value = "PATCH")]
        Patch,

        [EnumMember(Value = "TRACE")]
        Trace,

        [EnumMember(Value = "HEAD")]
        Head,

        [EnumMember(Value = "GET")]
        Get,

        [EnumMember(Value = "DELETE")]
        Delete,

        [EnumMember(Value = "OPTIONS")]
        Options
    }
}
