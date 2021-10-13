using System;
using System.Reflection;

namespace Digitteck.Gateway.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DirectiveNameAttribute : Attribute
    {
        public DirectiveNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static DirectiveNameAttribute GetAttributeFrom(Type type)
        {
            //extract name
            DirectiveNameAttribute nameAttribute = type.GetCustomAttribute<DirectiveNameAttribute>();

            if (nameAttribute == null)
            {
                throw new Exception($"The operation {type.FullName} must be decorated with the attribute {nameof(DirectiveNameAttribute)}");
            }

            return nameAttribute;
        }

        public static DirectiveNameAttribute GetAttributeFrom<T>()
        {
            return GetAttributeFrom(typeof(T));
        }
    }
}
