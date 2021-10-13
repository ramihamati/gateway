using System;
using System.Text;

namespace Digitteck.Gateway.Service.Common
{
    internal static class StringHelper
    {
        /// <summary>
        /// If there are two identical characters with the value of occurence situated one after another, it removes one of them
        /// </summary>
        public static string RemoveImmediateDuplicate(this string @this, char occurence)
        {
            if (@this is null || @this.Length <= 1)
            {
                return @this;
            }

            char lastChar = @this[0];
            //make it a span to work with references in the for loop
            ReadOnlySpan<char> syntax = @this.AsSpan();

            //start with first characted
            StringBuilder reconstructed = new StringBuilder();
            reconstructed.Append(lastChar);
            //using for instead of foreach because it's more performant
            for (int i = 1; i < syntax.Length; i++)
            {
                if (syntax[i] == lastChar && occurence == lastChar)
                {
                    lastChar = syntax[i];
                    continue;
                }
                reconstructed.Append(syntax[i]);
                lastChar = syntax[i];
            }

            return reconstructed.ToString();
        }
    }
}
