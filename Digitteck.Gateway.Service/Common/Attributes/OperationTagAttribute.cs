using System;

namespace Digitteck.Gateway.Service.Common.Attributes
{
    /// <summary>
    /// Used in the aggregate custom file in the constructor to bind the parameter to a tag response from a call
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OperationTagAttribute : Attribute
    {
        public OperationTagAttribute(string operationTag)
        {
            OperationTag = operationTag;
        }

        public string OperationTag { get; }
    }
}
