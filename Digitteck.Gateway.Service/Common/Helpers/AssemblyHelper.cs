using System;
using System.Reflection;

namespace Digitteck.Gateway.Service.Common.Helpers
{
    internal static class AssemblyHelper
    {
        public static Type FindType(string assemblyName, string typeName)
        {
            typeName = typeName.Trim();
            assemblyName = assemblyName.Trim();

            Assembly foundAssembly = null;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == assemblyName)
                {
                    foundAssembly = assembly;
                    break;
                }
            }

            if (foundAssembly == null)
            {
                return null;
            }

            Type foundType = null;

            foreach (TypeInfo type in foundAssembly.DefinedTypes)
            {
                if (type.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                {
                    foundType = type;
                    break;
                }
            }

            return foundType;
        }
    }
}
