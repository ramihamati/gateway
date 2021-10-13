using System;
using System.Runtime.CompilerServices;

namespace Digitteck.Gateway.Service.Common.Guards
{
    public static class Ensure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull<T>(in T value, string paramName) where T: class
        {
            if (value == null)
            {
                throw new ArgumentNullException($"The parameter {paramName} must not be null");
            }
        }
    }
}
