using System.Diagnostics.CodeAnalysis;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// Identical to PathAndQuery, but a different object to sepparate responsability
    /// </summary>
    public sealed class TemplatePathAndQueryObject : ValueObject<string>
    {
        private readonly PathObject pathObject;
        private readonly QueryObject queryObject;

        //query can be empty, at that stage it does not have a value
        public bool HasValue => pathObject.HasValue;// && queryObject.HasValue;

        public TemplatePathAndQueryObject(string pathAndQuery)
        {
            /*
             * validations : are made in the inner value objects
             */

            /*
             * sepparating elements
             */
            string[] parts = pathAndQuery.Split(GlobalEnv.QueryDelimiter);

            pathObject = parts.Length > 0 ? new PathObject(parts[0]) : new PathObject(string.Empty);

            queryObject = parts.Length > 1 ? new QueryObject($"{GlobalEnv.QueryDelimiter}{parts[1]}") : new QueryObject(string.Empty);

            this.Value = $"{pathObject}{queryObject}";
        }

        public TemplatePathAndQueryObject(PathAndQueryObject queryObject) : this(queryObject.Value)
        {

        }
        
        public PathObject GetPathObject() => pathObject;
        public QueryObject GetQueryObject() => queryObject;

        public bool Equals([AllowNull] PathAndQueryObject other)
        {
            if (other is null)
            {
                return false;
            }
            return this.Value.Equals(other.Value);
        }

        protected override bool EqualsCore(object obj)
        {
            if (obj is PathAndQueryObject pathAndQueryString)
            {
                return this.Value.Equals(pathAndQueryString.Value);
            }

            return false;
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        public override int CompareTo(ValueObject<string> other)
        {
            if (other == null)
            {
                return -1;
            }

            return this.Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
