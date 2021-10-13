using System.Diagnostics.CodeAnalysis;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// Rules : 
    /// <para>- always start with /</para>
    /// <para>- never end with /</para
    /// </summary>
    public class PathAndQueryObject : ValueObject<string>
    {
        private readonly PathObject pathObject;
        private readonly QueryObject queryObject;

        //query is not mandatory. if the path has a value then the object must state that
        public bool HasValue => pathObject.HasValue ;

        public PathAndQueryObject(string pathAndQuery)
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
            if (other==null)
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
