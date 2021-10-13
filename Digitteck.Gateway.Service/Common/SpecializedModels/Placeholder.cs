using System;
using System.Diagnostics.CodeAnalysis;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// A placeholder is a special syntaxt used in the template to mark named variable within the template.
    /// E.G. from "/api/movies/{movieName}" the placeholder will be extracted as "movieName" and "{movieName}" (short and long)
    /// Note : IComparable used for binary search in <see cref="PlaceholderExtractor.IsProperSuperset(Span{Placeholder}, Span{Placeholder})"/>
    /// </summary>
    public class Placeholder : IComparable<Placeholder>
    {
        /// <summary>
        /// With brackets
        /// </summary>
        public string LongName { get; }
        
        /// <summary>
        /// Without brackets
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// name can be either '{name}' or simple 'name'
        /// </summary>
        public Placeholder(string name)
        {
            string longName = name;
            string shortName = name;

            if (!longName.StartsWith("{"))
            {
                longName = $"{{{longName}";
            }
            if (!longName.EndsWith("}"))
            {
                longName = $"{longName}}}";
            }

            if (shortName.StartsWith("{"))
            {
                shortName = shortName.Remove(0, 1);
            }
            if (shortName.EndsWith("}"))
            {
                shortName = shortName.Remove(shortName.Length - 1, 1);
            }
         
            this.LongName = longName;
            this.ShortName = shortName;
        }

        public override int GetHashCode()
        {
            return this.LongName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Placeholder placeholder)
            {
                return string.Equals(this.LongName, placeholder.LongName, System.StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public int CompareTo([AllowNull] Placeholder other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.LongName.CompareTo(other.LongName);
        }
    }
}
