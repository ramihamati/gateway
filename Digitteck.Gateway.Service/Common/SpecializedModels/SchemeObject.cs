using System;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// The scheme value object contains two values (http or https). The delimiters are not treated here
    /// </summary>
    public sealed class SchemeObject : ValueObject<string>
    {
        public bool HasValue => !string.IsNullOrEmpty(this.Value);

        public SchemeObject(string scheme)
        {
            if (!(scheme.Equals("http") || scheme.Equals("https")))
            {
                throw new Exception($"A scheme can be either \'http\' or \'https\' with no delimiters");
            }

            this.Value = scheme;
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
            return obj is SchemeObject schemeObj && this.Value.Equals(schemeObj.Value);
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
