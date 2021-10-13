using System;
using System.Text.RegularExpressions;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The hostname is similar to HostString with exceptions
    /// - the host name can contain the full name with domain extension (en.wikipedia.com)
    /// - the host name cannot contain the scheme (http://) (unlike HostString)
    /// - the host name cannot contain any part of the path (/api) (unlike HostString)
    /// <para>E.G. from http://localhost:8080/api/movies?rating=22 the host name is "localhost"</para>
    /// </summary>
    public sealed class HostObject : ValueObject<string>
    {
        public bool HasValue => !string.IsNullOrWhiteSpace(this.Value);

        public HostObject(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new Exception("The host name cannot be null or empty");
            }

            if (!Regex.IsMatch(hostName, GlobalEnv.RegexPatterns.ValidHostname))
            {
                throw new Exception("The host name does not meet the rules. The host name cannot contain the port, query, path or scheme. " +
                    "E.G. Valid hostnames are \'en.wikipedia.com\', \'wikipedia.com\', \'wikipedia\'");
            }

            this.Value = hostName;
        }

        public override int CompareTo(ValueObject<string> other)
        {
            if (other is null)
            {
                //just a convention since this is never true. We consider that a nul object is at the bottom of the stack
                return 1;
            }
            return this.Value.CompareTo(other.Value);
        }

        protected override bool EqualsCore(object obj)
        {
            return obj is HostObject hostObject && this.Value.Equals(hostObject.Value);
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
