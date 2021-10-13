using System;
using System.Reflection;

namespace Digitteck.Gateway.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class OperationNameAttribute : Attribute
    {
        public OperationNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static OperationNameAttribute GetAttributeFrom(Type type)
        {
            //extract name
            OperationNameAttribute nameAttribute = type.GetCustomAttribute<OperationNameAttribute>();

            if (nameAttribute == null)
            {
                throw new Exception($"The operation {type.FullName} must be decorated with the attribute {nameof(OperationNameAttribute)}");
            }

            return nameAttribute;
        }

        public static OperationNameAttribute GetAttributeFrom<T>()
        {
            return GetAttributeFrom(typeof(T));
        }
    }
}
