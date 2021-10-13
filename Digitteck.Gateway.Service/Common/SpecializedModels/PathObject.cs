using Digitteck.Gateway.Service.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Digitteck.Gateway.Service
{
    /// <summary>
    /// A path represents the fragment between the host and the query (if this exists).
    /// <para>1. While the object HttpRequest separates this in Path and Query properties, the PathString object</para>
    /// does not signal an error if a query string is passed. 
    /// <para>2. It also throws an error if the paths don't start with "/"</para>
    /// <para>E.G. from http://localhost:8080/api/movies?rating=22 the path is "/api/movies"</para>
    /// Rules : 
    /// <para>- always start with /</para>
    /// <para>- never end with /</para>
    /// </summary>
    public sealed class PathObject : ValueObject<string>
    {
        public bool HasValue => !string.IsNullOrEmpty(this.Value);

        public PathObject(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                this.Value = string.Empty;
            }

            /*
             * Validations
             */
            if (path.Count(x => x == GlobalEnv.QueryDelimiter) > 0)
            {
                throw new Exception($"PathObject cannot accept a delimiter");
            }
            if (path.Contains(GlobalEnv.SchemeHttp, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"PathObject must contain only the relative path and query after the host string. " +
                    $"E.g. from \'http://localhost:8080/api/movies?rating=22\' the pathAndQuery is \'/api/movies?rating=22\'");
            }

            if (path.Contains(GlobalEnv.SchemeAndAuthorityDelimiter, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"PathObject must contain only the relative path and query after the host string. " +
                    $"E.g. from \'http://localhost:8080/api/movies?rating=22\' the pathAndQuery is \'/api/movies?rating=22\'");
            }

            string pathFixed = path;
            /*
             * Fixes
             */

            //replacing invalid characters
            pathFixed.Replace(GlobalEnv.InvalidPathDelimiter, GlobalEnv.PathDelimiter);
            //removing duplicate characters
            pathFixed = pathFixed.RemoveImmediateDuplicate(GlobalEnv.PathDelimiter);
            //let's make sure no path ends with '/'
            if (pathFixed.Length > 0 && pathFixed.EndsWith(GlobalEnv.PathDelimiter) || pathFixed.EndsWith(GlobalEnv.InvalidPathDelimiter))
            {
                pathFixed = pathFixed.Remove(pathFixed.Length - 1, 1);
            }
            //let's make sure the path starts with /
            if (pathFixed.Length > 0 && !pathFixed.StartsWith(GlobalEnv.PathDelimiter))
            {
                pathFixed = $"{GlobalEnv.PathDelimiter}{pathFixed}";
            }

            this.Value = pathFixed;
        }

        //Rule : we don't work with invalid items
        public PathObject(PathString path)
            : this(path.HasValue ? path.Value : string.Empty)
        {

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
            return obj is PathObject path && this.Value.Equals(path.Value);
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
