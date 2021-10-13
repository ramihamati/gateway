using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.Service
{
    public sealed class QueryPart
    {
        public string QueryKey { get; }

        public string QueryValue { get; }

        public QueryPart(string queryKey, string queryValue)
        {
            QueryValue = queryValue;
            QueryKey = queryKey;
        }

        public QueryPart Clone()
        {
            return new QueryPart(this.QueryKey, this.QueryValue);
        }
    }

    /// <summary>
    /// A query represents the fragment between after the ? (if it exists). 
    /// Similar to QueryString but with extra checks and rules in place
    /// <para>always start with ?</para>
    /// <para>never end with /</para>
    /// </summary>
    public sealed class QueryObject : ValueObject<string>
    {
        public bool HasValue => !string.IsNullOrEmpty(this.Value);

        public List<QueryPart> QueryParts { get; }

        public QueryObject(List<QueryPart> parts)
            : this($"?{string.Join('&', parts.Select(x => $"{x.QueryKey}={x.QueryValue}"))}")
        {

        }

        public static QueryObject FromParts(IList<QueryPart> parts)
        {
            if (parts.Count > 0)
            {
                string query = $"?{string.Join('&', parts.Select(x => $"{x.QueryKey}={x.QueryValue}"))}";
                return new QueryObject(query);
            }
            else
            {
                return new QueryObject(string.Empty);
            }
        }

        public QueryObject(string query)
        {
            this.QueryParts = new List<QueryPart>();
            string queryFixed = query;
            /*
             * fixes
             */
             if (query.Length == 1 && query == "?")
            {
                this.Value = string.Empty;
                return;
            }
            /*
             * validations
             */
            if (string.IsNullOrWhiteSpace(query))
            {
                this.Value = string.Empty;
                return;
            }

            if (!query.StartsWith(GlobalEnv.QueryDelimiter))
            {
                throw new Exception($"A non-empty value for {nameof(QueryObject)} must start with the query delimiter {GlobalEnv.QueryDelimiter}");
            }

            queryFixed.Replace(GlobalEnv.InvalidPathDelimiter, GlobalEnv.PathDelimiter);
            if (queryFixed.Count(x => x == GlobalEnv.PathDelimiter) > 0)
            {
                throw new Exception($"{nameof(QueryObject)} cannot contain path delimiter characters like \'{GlobalEnv.PathDelimiter}\'");
            }

            if (query.Contains(GlobalEnv.SchemeHttp, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"{nameof(QueryObject)} must contain only the relative path and query after the host string. " +
                    $"E.g. from \'http://localhost:8080/api/movies?rating=22\' the pathAndQuery is \'/api/movies?rating=22\'");
            }

            if (query.Contains(GlobalEnv.SchemeAndAuthorityDelimiter, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"{nameof(QueryObject)} must contain only the relative path and query after the host string. " +
                    $"E.g. from \'http://localhost:8080/api/movies?rating=22\' the pathAndQuery is \'/api/movies?rating=22\'");
            }

            /*
             * Fixes
             */

            //let's make sure no path ends with '/'
            if (queryFixed.EndsWith(GlobalEnv.PathDelimiter) || queryFixed.EndsWith(GlobalEnv.InvalidPathDelimiter))
            {
                queryFixed = queryFixed.Remove(queryFixed.Length - 1, 1);
            }

            this.Value = queryFixed;
            GetQueryParts();
        }

        private void GetQueryParts()
        {
            this.QueryParts.Clear();

            if (!string.IsNullOrWhiteSpace(this.Value))
            {
                string query = this.Value;
                if (query.StartsWith(GlobalEnv.QueryDelimiter))
                {
                    query = query.Remove(0, 1);
                }

                string[] queryParts = query.Split(GlobalEnv.QueryVarDelimiter);

                foreach (var part in queryParts)
                {
                    //each part should look like 'movie=name'
                    string[] partkvp = part.Split(GlobalEnv.QueryAssignment);

                    if (partkvp.Length != 2)
                    {
                        throw new Exception($"Could not get the query parts from {this.Value}");
                    }


                    this.QueryParts.Add(new QueryPart(partkvp[0], partkvp[1]));
                }
            }
        }

        //Rule : we don't work with invalid items
        public QueryObject(QueryString query)
            : this(query.HasValue ? query.Value : string.Empty)
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
            return obj is QueryObject queryObject ? this.Value.Equals(queryObject.Value) : false;
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
