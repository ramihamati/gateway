using System;
using System.Text;

namespace Digitteck.Gateway.Service.SpecializedModels
{
    public sealed class AbsoluteUriObject : ValueObject<string>
    {
        public SchemeObject SchemeObject { get; private set; }
        public HostObject HostObject { get; private set; }
        public PortObject PortObject { get; private set; }
        public PathObject PathObject { get; private set; }
        public QueryObject QueryObject { get; private set; }

        //keep the constructor private. We need to throw an exception in the static method and prevent building the object if invalid
        private AbsoluteUriObject()
        {
            //do nothing, not even erasing this constructor
        }

        public static AbsoluteUriObject Create(SchemeObject schemeObject, HostObject hostObject, PortObject portObject = null, PathObject pathObject = null, QueryObject queryObject = null)
        {
            if (!hostObject.HasValue)
            {
                throw new Exception($"Cannot build the uri because the host is not valid");
            }

            var uri = new AbsoluteUriObject
            {
                SchemeObject = schemeObject,
                HostObject = hostObject,
                PortObject = portObject,
                PathObject = pathObject,
                QueryObject = queryObject
            };

            uri.BuildValue();
            return uri;
        }

        public static AbsoluteUriObject Create(HostObject hostObject, PortObject portObject = null, PathObject pathObject = null, QueryObject queryObject = null)
        {

            if (!hostObject.HasValue)
            {
                throw new Exception($"Cannot build the uri because the host is not valid");
            }

            var uri = new AbsoluteUriObject
            {
                HostObject = hostObject,
                PortObject = portObject,
                PathObject = pathObject,
                QueryObject = queryObject
            };

            uri.BuildValue();

            return uri;
        }

        //public static AbsoluteUriObject Create(string uri)
        //{
                //TODO : create this parse for the sake of it
        //}

        private void BuildValue()
        {
            StringBuilder sb = new StringBuilder();

            if (SchemeObject != null && SchemeObject.HasValue)
            {
                sb.Append(SchemeObject.Value);
                sb.Append(GlobalEnv.SchemeAndAuthorityDelimiter);
            }

            if (HostObject != null && HostObject.HasValue)
            {
                sb.Append(HostObject.Value);
                if (PortObject != null && PortObject.HasValue)
                {
                    sb.Append(GlobalEnv.PortDelimiter);
                    sb.Append(PortObject.Value);
                }
            }
            else
            {
                throw new Exception($"Cannot build the uri because the host is not valid");
            }

            if (PathObject != null && PathObject.HasValue)
            {
                //the rule of PathAndQuery adds / in from of the path
                sb.Append(PathObject.Value);
            }
            if (QueryObject != null && QueryObject.HasValue)
            {
                //the rule of PathAndQuery adds / in from of the path
                sb.Append(QueryObject.Value);
            }

            this.Value = sb.ToString();
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
            return obj is AbsoluteUriObject path && this.Value.Equals(path.Value);
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
